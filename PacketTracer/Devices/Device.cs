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
                    temp.RecievePacket(destinationIpAddress, physicalInterface);
                }
                else if (bDevice.GetType() == typeof(Router))
                {
                    Router temp = (Router)bDevice;
                    temp.RecievePacket(destinationIpAddress, physicalInterface);
                }
                else
                {
                    throw new NotImplementedException();
                }
                #region old
                /*

                Device deviceA, deviceB;
                deviceA = physicalInterface.connectedCable.deviceA;
                deviceB = physicalInterface.connectedCable.deviceB;
                // Determines what device is on which end of cable and then calls the overriding method of RecievePacket on the derived class
                if (deviceA == this)
                {
                    if (deviceB.GetType() == typeof(Computer))
                    {
                        Computer temp = (Computer)deviceB;
                        temp.RecievePacket(destinationIpAddress, physicalInterface);
                    }
                    else if (deviceB.GetType() == typeof(Router))
                    {
                        Router temp = (Router)deviceB;
                        temp.RecievePacket(destinationIpAddress, physicalInterface);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else if (deviceB == this)
                {
                    if (deviceA.GetType() == typeof(Computer))
                    {
                        Computer temp = (Computer)deviceA;
                        temp.RecievePacket(destinationIpAddress, physicalInterface);
                    }
                    else if (deviceA.GetType() == typeof(Router))
                    {
                        Router temp = (Router)deviceA;
                        temp.RecievePacket(destinationIpAddress, physicalInterface);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
                */
                #endregion
            }

        }
        /// <summary>
        /// I don't know if this needs to be virtual or not but just keeping it this way for now
        /// </summary>
        /// <param name="destinationIpAdress"></param>
        /// <param name="physicalInterface"></param>
        public virtual void RecievePacket(string destinationIpAdress, PhysicalInterface physicalInterface)
        {

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
                                    nextHopIp = "LOCAL";
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
