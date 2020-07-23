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
using PacketTracer.Devices.Switches;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="physicalInterface">Interface to send the packet from</param>
        public void SendPacket(Packet packet, PhysicalInterface physicalInterface)
        {
            (Device devA, Device devB) = physicalInterface.ConnectedCable.SortCableDevices(this);
            switch (TypeOfDevice)
            {
                case deviceType.Computer:
                    Computer computer = (Computer)this;
                    if (computer.CheckArpTable(packet.DestinationIpAddress) == null)
                    {
                        // Send arp request.
                        Debug.WriteLine("Sending arp request");
                        Terminal.TerminalOutput += "\nSending arp request";
                        ARPPacket arpPacket = new ARPPacket(packet.DestinationIpAddress, packet.SourceIpAddress, packet.SourceMacAddress, "Request");
                        devB.RecievePacketAsync(arpPacket, physicalInterface);
                    }
                    else
                    {
                        // Send packet
                    }
                    break;
                case deviceType.Router:
                    break;
                case deviceType.Switch:
                    NetworkSwitch networkSwitch = (NetworkSwitch)this;
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
