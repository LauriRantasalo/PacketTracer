﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;

using PacketTracer.Cables;
using PacketTracer.Devices.Interfaces;
using System.Diagnostics;
using PacketTracer.Devices.Console;
using PacketTracer.Protocols;

namespace PacketTracer.Devices.Computers
{
    public class Computer : Device
    {
        public List<ArpTableRow> ArpTable { get; set; }
        public Computer(UIManager uiManager, EntityManager entityManager, Grid baseGrid, string name, int nroOfEthernetPorts, string defaultEthernetPortIp) : base(uiManager, name, baseGrid, nroOfEthernetPorts)
        {
            TypeOfDevice = deviceType.Computer;
            ArpTable = new List<ArpTableRow>();
            for (int i = 0; i < this.nroOfEthernetPorts; i++)
            {
                if (i == 0)
                {
                    EthernetPorts.Add(new EthernetPort(defaultEthernetPortIp, "Gi0/0", entityManager.GenerateNewMacAddress()));
                }
                else
                {
                    EthernetPorts.Add(new EthernetPort("NULL", "Gi0/" + i.ToString(), entityManager.GenerateNewMacAddress()));
                }
            }

            Terminal = new ComputerTerminal(uiManager, this);
        }

        public override void SendPacket(Packet packet, PhysicalInterface physicalInterface)
        {
            (Device devA, Device devB) = physicalInterface.ConnectedCable.SortCableDevices(this);
            switch (packet.TypeOfPacket)
            {
                case PacketType.icmp:
                    Debug.WriteLine("Send icmp");
                    if (CheckArpTable(packet.DestinationIpAddress) == null)
                    {
                        // Send arp request.
                        Debug.WriteLine("Sending arp request");
                        Terminal.TerminalOutput += "\nSending arp request";
                        ARPPacket arpPacket = new ARPPacket(packet.DestinationIpAddress, packet.SourceIpAddress, packet.SourceMacAddress, "Request");
                        devB.RecievePacketAsync(arpPacket, physicalInterface);
                    }
                    else
                    {
                        // Send icmp packet
                        devB.RecievePacketAsync(packet, physicalInterface);
                    }
                    break;
                case PacketType.arp:
                    if (CheckArpTable(packet.DestinationIpAddress) == null)
                    {
                        // Send arp request.
                        Debug.WriteLine("Sending arp request");
                        Terminal.TerminalOutput += "\nSending arp request";
                        ARPPacket arpPacket = new ARPPacket(packet.DestinationIpAddress, packet.SourceIpAddress, packet.SourceMacAddress, "Request");
                        devB.RecievePacketAsync(arpPacket, physicalInterface);
                    }
                    else
                    {
                        Debug.WriteLine("HERE");
                        // Send packet
                        devB.RecievePacketAsync(packet, physicalInterface);
                    }
                    break;
                case PacketType.tcp:
                    break;
                case PacketType.udp:
                    break;
                default:
                    break;
            }
           
        }
        public async override void RecievePacketAsync(Packet packet, PhysicalInterface physicalInterface)
        {
            await Task.Delay(100);
            //Debug.WriteLine(Name + " got packet to" + packet.DestinationIpAddress);
            ArpTableRow arpTableRow = CheckArpTable(packet.SourceMacAddress);
            PhysicalInterface recievingPort = physicalInterface.ConnectedCable.GetPortOfDevice(this);
            if (arpTableRow == null && packet.SourceIpAddress != "NONE")
            {
                Debug.WriteLine("new arp table row at: " + Name + " - " + packet.SourceMacAddress + " " + packet.SourceIpAddress);
                ArpTable.Add(new ArpTableRow(packet.SourceMacAddress, packet.SourceIpAddress));
            }
            switch (packet.TypeOfPacket)
            {
                case PacketType.icmp:
                    ICMPPacket icmpPacket = (ICMPPacket)packet;
                    foreach (var port in EthernetPorts)
                    {
                        if (port.IpAddress == packet.DestinationIpAddress)
                        {
                            (Device aDevice, Device bDevice) = physicalInterface.ConnectedCable.SortCableDevices(this);
                            if (icmpPacket.EchoType == "Echo request")
                            {
                                icmpPacket.ToReply();
                                //bDevice.RecievePacketAsync(packet, EthernetPorts[0]);
                                SendPacket(icmpPacket, EthernetPorts[0]);
                            }
                            else if (icmpPacket.EchoType == "Echo reply")
                            {

                                Terminal.TerminalOutput += "\nReply from " + icmpPacket.SourceIpAddress + " Time: " + (DateTime.Now - icmpPacket.SendTime).ToString();
                                icmpPacket.ToRequest();
                                Debug.WriteLine("Reply recieved");
                                if (icmpPacket.NroOfRoundsDone < icmpPacket.MaxNroOfRounds)
                                {
                                    packet.SendTime = DateTime.Now;
                                    //bDevice.RecievePacketAsync(packet, EthernetPorts[0]);
                                    SendPacket(packet, EthernetPorts[0]);
                                }
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                            return;
                        }
                    }
                    break;
                case PacketType.arp:
                    ARPPacket arpPacket = (ARPPacket)packet;
                    if (arpPacket.DestinationIpAddress == recievingPort.IpAddress)
                    {
                        Debug.WriteLine("Arp destination reached at: " + Name);
                        if (arpPacket.EchoType == "Request")
                        {
                            Debug.WriteLine("Request to reply at " + Name + "  " + recievingPort.MacAddress + " " + EthernetPorts[0].MacAddress + " " + EthernetPorts[0].InterfaceName);
                            arpPacket.ToReply(recievingPort.MacAddress);
                            SendPacket(arpPacket, recievingPort);
                        }
                        else if(arpPacket.EchoType == "Reply")
                        {
                            Debug.WriteLine("HERE");
                            Terminal.TerminalOutput += "\nArp reply recieved";
                            foreach (var row in ArpTable)
                            {
                                Terminal.TerminalOutput += "\narptable row " + row.MacAddress + " " + row.IpAddress; 
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                    }
                    else
                    {
                        Debug.WriteLine("Dropping packet at " + Name + " " + arpPacket.EchoType);
                    }
                    break;
                case PacketType.tcp:
                    break;
                case PacketType.udp:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Returns comuter arp table row
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public  ArpTableRow CheckArpTable(string ipAddress)
        {
            foreach (var row in ArpTable)
            {
                if (row.IpAddress == ipAddress)
                {
                    return row;
                }
            }
            return null;
        }

    }
}