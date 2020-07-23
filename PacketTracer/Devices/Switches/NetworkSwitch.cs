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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="physicalInterface">Sending device interface</param>
        public override void RecievePacketAsync(Packet packet, PhysicalInterface physicalInterface)
        {
            //await Task.Delay(50);
            // Add new routes to arp table
            //Debug.WriteLine(Name + " Got packet to " + packet.DestinationIpAddress);
            PhysicalInterface recievingPort = physicalInterface.ConnectedCable.GetPortOfDevice(this);
            MacAddressTableRow destinationMacAddressTableRow = CheckArpTable(packet.DestinationIpAddress);
            MacAddressTableRow sourceMacAddressTableRow = CheckArpTable(packet.SourceIpAddress);
            if (sourceMacAddressTableRow == null)
            {
                MacAddressTableRow temp = new MacAddressTableRow(packet.SourceMacAddress, recievingPort);
                MacAddressTable.Add(temp);
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
                                SendPacket(arpPacket, port);
                            }
                        }
                    }
                    else if (arpPacket.EchoType == "Reply")
                    {
                        if (destinationMacAddressTableRow != null)
                        {
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
                if (row.MacAddress == macAddress)
                {
                    return row;
                }
            }
            return null;
        }

    }


    /*
    public class NetworkSwitch : Device
    {
        /// <summary>
        /// [subnet, next-hop, interface]
        /// </summary>
        //List<(string subnet, string nextHop, PhysicalInterface physicalInterface)> routingTable = new List<(string, string, PhysicalInterface)>();
        public List<ArpTableRow> ArpTable { get; set; }

        public NetworkSwitch(UIManager uiManager, EntityManager entityManager, Grid baseGrid, string name, int nroOfEthernetPorts) : base(uiManager, name, baseGrid, nroOfEthernetPorts)
        {
            ArpTable = new List<ArpTableRow>();
            TypeOfDevice = deviceType.Switch;
            for (int i = 0; i < this.nroOfEthernetPorts; i++)
            {
                EthernetPorts.Add(new EthernetPort("192.168.0." + (10 + i).ToString(), "Gi0/" + i.ToString(), entityManager.GenerateNewMacAddress()));
            }
            Terminal = new RouterTerminal(uiManager, this);
        }

        /// <summary>
        /// Either route packets here from routers RecievePacket method or from Computers SendPacket
        /// </summary>
        /// <param name="destinationIpAdress"></param>
        /// <param name="physicalInterface">last device physical interface</param>
        public async override void RecievePacketAsync(Packet packet, PhysicalInterface physicalInterface)
        {
            ArpTableRow tempRow = CheckArpTable(packet.SourceIpAddress);
            if (tempRow == null)
            {
                // New connection, add to arp table;
                ArpTable.Add(new ArpTableRow(packet.SourceMacAddress, packet.SourceIpAddress, ))// Need to get the port that the request came in on?
            }
            await Task.Delay(100);
            bool isDestinationDevice = false;
            switch (packet.TypeOfPacket)
            {
                case PacketType.icmp:
                    ICMPPacket icmpPacket = (ICMPPacket)packet;
                    foreach (var port in EthernetPorts)
                    {
                        if (port != null && port.IpAddress == icmpPacket.DestinationIpAddress)
                        {
                            isDestinationDevice = true;
                            (Device iDevice, Device jDevice) = port.ConnectedCable.SortCableDevices(this);
                            icmpPacket.ToReply();
                            //SwitchingTableRow tempRow = CheckSwitchingTable(icmpPacket.DestinationIpAddress, icmpPacket.SourceIpAddress, physicalInterface);
                            ArpTableRow tempRow = CheckArpTable();
                            SendPacket(icmpPacket, tempRow.PhysicalInterface);
                            break;
                        }
                    }

                    if (!isDestinationDevice)
                    {
                       //SwitchingTableRow routingTableRow = CheckSwitchingTable(icmpPacket.DestinationIpAddress, icmpPacket.SourceIpAddress, physicalInterface);

                        (Device aDevice, Device bDevice) = routingTableRow.PhysicalInterface.ConnectedCable.SortCableDevices(this);
                        SendPacket(icmpPacket, routingTableRow.PhysicalInterface);
                    }

                    break;
                case PacketType.arp:
                    ARPPacket arpPacket = (ARPPacket)packet;

                    if (arpPacket.EchoType == "Request")
                    {
                        foreach (var port in EthernetPorts)
                        {
                            if (port.IpAddress == arpPacket.DestinationIpAddress)
                            {
                                arpPacket.ToReply(port.MacAddress);
                                SendPacket(arpPacket, );
                                break;
                            }
                        }
                    }

                    // Broadcast arp request to all ports
                    foreach (var port in EthernetPorts)
                    {
                        if (port != physicalInterface)
                        {
                            SendPacket(arpPacket, port);
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

        public override ArpTableRow CheckArpTable(string macAddress)
        {
            foreach (var row in ArpTable)
            {
                if (row.MacAddress == macAddress)
                {
                    return row;
                }
            }
            return null;
        }

        /*
                public SwitchingTableRow CheckSwitchingTable(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface)
                {
                    string destinationSubnet = destinationIpAdress.Remove(destinationIpAdress.LastIndexOf(".")) + ".0";
                    bool noRoute = true;
                    foreach (var switchingTableRow in switchingTable)
                    {

                         * if (routingTableRow.NextHop == destinationIpAdress)
                        {
                            //Debug.WriteLine("Routing " + echoType + " from: " + sourceIpAdress + " to: " + destinationIpAdress);
                            noRoute = false;
                            return routingTableRow;
                        }// If there is a subnet match to destination subnet and it is not the sending devices address
                        else if (routingTableRow.Subnet == destinationSubnet && routingTableRow.NextHop != physicalInterface.IpAddress)
                        {
                            // Hop to next router
                            //throw new NotImplementedException();
                        }

                    }

                    if (noRoute)
                    {
                        Debug.WriteLine("No route found to: " + destinationIpAdress);
                        return null;
                    }

                    return null;
                }


        public void AddNewSwitchingTableRow(string macAddress, PhysicalInterface physicalInterface)
        {

            switchingTable.Add(new ArpTableRow(macAddress, physicalInterface));
            /*
            foreach (var item in routingTable)
            {
                Debug.WriteLine(item.Subnet + " " + item.NextHop + " " + item.PhysicalInterface.ipAddress);
            }
            //Debug.WriteLine("From " + this.Name + ": " + routingTable[0]);
            // Possible routing protocol begins here?
        }
    }
*/
}
