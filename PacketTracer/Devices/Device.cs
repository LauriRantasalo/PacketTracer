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

namespace PacketTracer.Devices
{
    public enum deviceType{Computer, Router, Switch};
    public class Device
    {
        public string name;
        public Grid baseGrid;
        /// <summary>
        /// This might be useless 'cause of GetType() == typeof(Computer)
        /// </summary>
        public deviceType typeOfDevice;
        public readonly int nroOfEthernetPorts;
        public List<EthernetPort> ethernetPorts;

        public Device(string name, Grid baseGrid, int nroOfEthernetPorts)
        {
            ethernetPorts = new List<EthernetPort>();
            this.nroOfEthernetPorts = nroOfEthernetPorts;
            
            this.name = name;
            this.baseGrid = baseGrid;
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
            if (ethernetPorts[0].ipAddress != destinationIpAdress)
            {
                Debug.WriteLine("Wrong place");
                Debug.WriteLine(destinationIpAdress + " _ " + ethernetPorts[0].ipAddress);
                throw new NotSupportedException();
            }
            else
            {

                (Device aDevice, Device bDevice) = physicalInterface.connectedCable.SortCableDevices(this);
                if (echoType == "Echo request")
                {
                    Debug.WriteLine("Right place request");
                    bDevice.RecievePacket(sourceIpAdress, ethernetPorts[0].ipAddress, ethernetPorts[0], "Echo reply");
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
            switch (cable.typeOfCable)
            {
                case cableType.Ethernet:
                    foreach (var port in ethernetPorts)
                    {
                        if (port.connectedCable == null)
                        {
                            if (typeOfDevice == deviceType.Router)
                            {
                                Router temp = (Router)this;
                                string subnet = port.ipAddress.Remove(port.ipAddress.LastIndexOf("."));
                                string nextHopIp = deviceB.ethernetPorts[0].ipAddress;
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
