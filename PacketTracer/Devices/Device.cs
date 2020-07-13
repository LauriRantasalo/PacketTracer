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

namespace PacketTracer.Devices
{
    public enum deviceType{Computer, Router, Switch};
    public class Device
    {
        public string Name { get; set; }
        /// <summary>
        /// This might be useless 'cause of GetType() == typeof(Computer)
        /// </summary>
        public deviceType TypeOfDevice { get; set; }
        public Terminal Terminal { get; set; }
        public List<EthernetPort> EthernetPorts { get; set; }
        public int nroOfEthernetPorts { get; }
        public Grid BaseGrid { get; set; }

        public Device(string name, Grid baseGrid, int nroOfEthernetPorts)
        {
            EthernetPorts = new List<EthernetPort>();
            this.nroOfEthernetPorts = nroOfEthernetPorts;
            
            Name = name;
            BaseGrid = baseGrid;
        }

       
        public void SendPacket(string destinationIpAddress, PhysicalInterface physicalInterface)
        {
            if (physicalInterface.connectedCable != null)
            {
                (Device aDebvice, Device bDevice) = physicalInterface.connectedCable.SortCableDevices(this);

                if(bDevice.GetType() == typeof(Computer))
                {
                    Computer temp = (Computer)bDevice;
                    temp.RecievePacket(destinationIpAddress, physicalInterface.ipAddress, physicalInterface, "Echo request");
                }
                else if (bDevice.GetType() == typeof(Router))
                {
                    Router temp = (Router)bDevice;
                    temp.RecievePacket(destinationIpAddress, physicalInterface.ipAddress, physicalInterface, "Echo request");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }
        /// <summary>
        /// This is made only for computers right now, need to take routers and other devices into consideration
        /// </summary>
        /// <param name="destinationIpAdress"></param>
        /// <param name="sourceIpAdress"></param>
        /// <param name="physicalInterface"></param>
        /// <param name="echoType"></param>
        public virtual void RecievePacket(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface, string echoType)
        {
            if (EthernetPorts[0].ipAddress != destinationIpAdress)
            {
                Debug.WriteLine("Wrong place");
                Debug.WriteLine(destinationIpAdress + " _ " + EthernetPorts[0].ipAddress);
                throw new NotSupportedException();
            }
            else
            {

                (Device aDevice, Device bDevice) = physicalInterface.connectedCable.SortCableDevices(this);
                if (echoType == "Echo request")
                {
                    Debug.WriteLine("Right place request");
                    bDevice.RecievePacket(sourceIpAdress, EthernetPorts[0].ipAddress, EthernetPorts[0], "Echo reply");
                }
                else if (echoType == "Echo reply")
                {
                    Debug.WriteLine("Right place reply");
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void SetIpAddress(PhysicalInterface port, string ipAddress)
        {
            port.ipAddress = ipAddress;
        }

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
                                // TODO: This need to take into consideration that subnets might be less than the first 3 segments of the address
                                if (subnet == nextHopIp.Remove(nextHopIp.LastIndexOf(".")))
                                {
                                    //nextHopIp = "LOCAL";
                                }

                                temp.AddNewRoute(subnet + ".0", nextHopIp, port);
                                port.connectedCable = cable;
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
    }
}
