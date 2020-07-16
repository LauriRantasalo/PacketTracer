using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

using PacketTracer.Cables;
using PacketTracer.Devices.Interfaces;
using PacketTracer.Devices.Console;
using PacketTracer.Devices.Routers;

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
            if (physicalInterface.connectedCable != null)
            {
                (Device aDebvice, Device bDevice) = physicalInterface.connectedCable.SortCableDevices(this);
                bDevice.RecievePacketAsync(packet, physicalInterface);

                Debug.WriteLine("sending packet to " + packet.DestinationIpAddress + " from " + physicalInterface.ipAddress);
                return "Pinging " + packet.DestinationIpAddress + " from " + physicalInterface.ipAddress;
            }
            return "No ethernet cable connected";
        }
        /*public string SendPacket(string destinationIpAddress, PhysicalInterface physicalInterface)
        {
            if (physicalInterface.connectedCable != null)
            {
                (Device aDebvice, Device bDevice) = physicalInterface.connectedCable.SortCableDevices(this);
                bDevice.RecievePacketAsync(destinationIpAddress, physicalInterface.ipAddress, physicalInterface, "Echo request");
                Debug.WriteLine("sending packet to " + destinationIpAddress + " from " + physicalInterface.ipAddress);
                return "Pinging " + destinationIpAddress + " from " + physicalInterface.ipAddress;
            }
            return "No ethernet cable connected";
        }
         */
        //public abstract void RecievePacketAsync(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface, string echoType);
        public abstract void RecievePacketAsync(Packet packet, PhysicalInterface physicalInterface);

        public void AddCable(Cable cable, Device connectedDevice)
        {
            (Device deviceA, Device deviceB) = cable.SortCableDevices(this);
            switch (cable.TypeOfCable)
            {
                case cableType.Ethernet:
                    foreach (var port in EthernetPorts)
                    {
                        if (port.connectedCable == null)
                        {
                            if (TypeOfDevice == deviceType.Router)
                            {
                                Router temp = (Router)this;
                                string subnet = port.ipAddress.Remove(port.ipAddress.LastIndexOf("."));
                                string nextHopIp = deviceB.EthernetPorts[0].ipAddress;
                                // TODO: This probably still needs to take into consideration that subnets might be less than the first 3 segments of the address

                                port.connectedCable = cable;
                                temp.AddNewRoutingTableRoute(subnet + ".0", nextHopIp, port);
                            }
                            else
                            {
                                port.connectedCable = cable;
                            }
                            return;
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void SetIpAddress(PhysicalInterface port, string ipAddress)
        {
            port.ipAddress = ipAddress;
        }
    }
}
