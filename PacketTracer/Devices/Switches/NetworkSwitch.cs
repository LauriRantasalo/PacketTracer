using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;

using PacketTracer.Devices.Interfaces;
using PacketTracer.Devices.Console;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;
using PacketTracer.Protocols;

namespace PacketTracer.Devices.Switches
{
    public class NetworkSwitch : Device
    {
        public List<MacAddressTableRow> MacAddressTable { get; set; }
        public NetworkSwitch(UIManager uIManager, EntityManager entityManager, Grid baseGrid, string name, int nroOfEthernetPorts) : base(uIManager, name, baseGrid, nroOfEthernetPorts)
        {
            TypeOfDevice = deviceType.Switch;
            MacAddressTable = new List<MacAddressTableRow>();
            for (int i = 0; i < nroOfEthernetPorts; i++)
            {
                EthernetPort temp = new EthernetPort("NONE", "f0/" + i.ToString(), entityManager.GenerateNewMacAddress());
                EthernetPorts.Add(temp);
            }
            Terminal = new SwitchTerminal(uIManager, this);
        }

        public override void SendPacket(Packet packet, PhysicalInterface physicalInterface)
        {
            (Device devA, Device devB) = physicalInterface.ConnectedCable.SortCableDevices(this);
            switch (packet.TypeOfPacket)
            {
                case PacketType.icmp:
                    break;
                case PacketType.arp:
                    devB.RecievePacketAsync(packet, physicalInterface);
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
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="physicalInterface">Sending device interface</param>
        public async override void RecievePacketAsync(Packet packet, PhysicalInterface physicalInterface)
        {
            await Task.Delay(50);
            // Add new routes to arp table
            //Debug.WriteLine(Name + " Got packet to " + packet.DestinationIpAddress);
            PhysicalInterface recievingPort = physicalInterface.ConnectedCable.GetPortOfDevice(this);
            MacAddressTableRow destinationMacAddressTableRow = CheckArpTable(packet.DestinationMacAddress);
            MacAddressTableRow sourceMacAddressTableRow = CheckArpTable(packet.SourceMacAddress);
            if (sourceMacAddressTableRow == null)
            {
                MacAddressTableRow temp = new MacAddressTableRow(packet.SourceMacAddress, recievingPort);
                MacAddressTable.Add(temp);
                sourceMacAddressTableRow = temp;
            }

            switch (packet.TypeOfPacket)
            {
                case PacketType.icmp:
                    Debug.WriteLine("Send icmp packet");
                    break;
                case PacketType.arp:
                    ARPPacket arpPacket = (ARPPacket)packet;
                    if (arpPacket.EchoType == "Request")
                    {
                        foreach (var port in EthernetPorts)
                        {
                            if (port.ConnectedCable != null &&  port != recievingPort)
                            {
                                Debug.WriteLine("Broadcasting arp to port " + port.InterfaceName);
                                ARPPacket temp = new ARPPacket(arpPacket.DestinationIpAddress, arpPacket.SourceIpAddress, arpPacket.SourceMacAddress, arpPacket.EchoType);
                                SendPacket(temp, port);
                            }
                        }
                    }else if (arpPacket.EchoType == "Reply")
                    {
                        Debug.WriteLine("Reply recieved, sending to " + packet.DestinationIpAddress);
                        if (destinationMacAddressTableRow != null)
                        {
                            Debug.WriteLine("Sending reply to " + packet.DestinationIpAddress);
                            SendPacket(arpPacket, destinationMacAddressTableRow.PhysicalInterface);
                        }
                        else
                        {
                            Debug.WriteLine("No route in mac-address table to " + arpPacket.DestinationMacAddress);
                            foreach (var row in MacAddressTable)
                            {
                                Debug.WriteLine(row.MacAddress + " " + row.PhysicalInterface.InterfaceName);
                            }
                        }
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

        public MacAddressTableRow CheckArpTable(string macAddress)
        {
            foreach (var row in MacAddressTable)
            {
                Debug.WriteLine("Row " + row.MacAddress + " check " + macAddress);
                if (row.MacAddress == macAddress)
                {
                    return row;
                }
            }
            return null;
        }

    }
    
}
