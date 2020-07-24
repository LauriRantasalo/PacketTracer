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
        public abstract void SendPacket(Packet packet, PhysicalInterface physicalInterface);
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
