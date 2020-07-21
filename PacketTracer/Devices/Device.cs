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
using Windows.UI.Xaml.Media;

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
                bDevice.RecievePacketAsync(packet, physicalInterface);

                Debug.WriteLine(Name + " @" + physicalInterface.InterfaceName + ":" + physicalInterface.IpAddress + " sending packet to " + packet.DestinationIpAddress);
                return "Pinging " + packet.DestinationIpAddress + " from " + physicalInterface.IpAddress;
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
