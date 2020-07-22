using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using PacketTracer.Cables;
using PacketTracer.Devices.Interfaces;
using PacketTracer.Devices.Console;
using PacketTracer.Devices.Routers;
using PacketTracer.Protocols;
using PacketTracer.Devices.Computers;

namespace PacketTracer.Devices
{
    public enum deviceType{Computer, Router, Switch};
    public abstract class Device
    {
        UIManager uiManager;
        public string Name { get; set; }
        /// <summary>
        /// This might be useless 'cause of GetType() == typeof(Computer)
        /// </summary>
        public deviceType TypeOfDevice { get; set; }
        public Terminal Terminal { get; set; }
        public List<EthernetPort> EthernetPorts { get; set; }
        public int nroOfEthernetPorts { get; }
        public Grid BaseGrid { get; set; }

        public Device(UIManager uiManager, string name, Grid baseGrid, int nroOfEthernetPorts)
        {
            this.nroOfEthernetPorts = nroOfEthernetPorts;
            this.uiManager = uiManager;
            EthernetPorts = new List<EthernetPort>();
            Name = name;
            BaseGrid = baseGrid;
        }
       
        public string SendPacket(Packet packet, PhysicalInterface physicalInterface)
        {

            if (physicalInterface.ConnectedCable != null)
            {
                (Device aDebvice, Device bDevice) = physicalInterface.ConnectedCable.SortCableDevices(this);
                switch (packet.TypeOfPacket)
                {
                    case PacketType.icmp:
                        if (GetType() == typeof(Computer))
                        {
                            Computer temp = (Computer)this;
                            ArpTableRow row = temp.CheckArpTable(packet.DestinationIpAddress);
                            if (row == null)
                            {
                                // Send arp request
                                bDevice.RecievePacketAsync(new ARPPacket(packet.DestinationIpAddress, physicalInterface.IpAddress, packet.SourceMacAddress, "Request"), physicalInterface);
                                return "Sending arp request";
                            }
                            else
                            {
                                packet.DestinationMacAddress = row.MacAddress;
                                bDevice.RecievePacketAsync(packet, physicalInterface);
                                return "Pinging " + packet.DestinationIpAddress + " from " + physicalInterface.IpAddress;
                            }
                        }


                        /*
                         * string destinationMacAddress = CheckArpTable(packet.DestinationIpAddress);
                        if (destinationMacAddress != "NO MATCH")
                        {
                            bDevice.RecievePacketAsync(packet, physicalInterface);
                            Debug.WriteLine(Name + " @" + physicalInterface.InterfaceName + ":" + physicalInterface.IpAddress + " sending packet to " + packet.DestinationIpAddress);
                            
                        }
                        else
                        {
                            // Generate and broadcast arp request to all ports in local network
                            bDevice.RecievePacketAsync(new ARPPacket(packet.DestinationIpAddress, physicalInterface.IpAddress, packet.SourceMacAddress, "Request"), physicalInterface);
                        }
                         */
                        break;
                    case PacketType.arp:
                        bDevice.RecievePacketAsync(packet, physicalInterface);
                        //bDevice.RecievePacketAsync(new ARPPacket(packet.DestinationIpAddress, physicalInterface.IpAddress, packet.SourceMacAddress, "Request"), physicalInterface);
                        break;
                    case PacketType.tcp:
                        break;
                    case PacketType.udp:
                        break;
                    default:
                        break;
                }

            }
            return "No ethernet cable connected";
        }
        
        public abstract void RecievePacketAsync(Packet packet, PhysicalInterface physicalInterface);

        public PhysicalInterface GetFreeEthernetPort()
        {
            foreach (var port in EthernetPorts)
            {
                if (port.ConnectedCable == null)
                {
                    return port;
                }
            }
            Debug.WriteLine("No free ethernet ports available on " + Name + ". Returned null");
            return null;
        }


    }
}
