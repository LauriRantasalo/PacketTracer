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
                if (physicalInterface.connectedCable.deviceA == this)
                {
                    //port.connectedCable.deviceB.RecievePing(destinationIpAddress);
                }
                else
                {
                    //port.connectedCable.deviceA.RecievePing(destinationIpAddress);
                }
            }
        }

        public void SetIpAddress(PhysicalInterface port, string ipAddress)
        {
            port.ipAddress = ipAddress;
        }

        public void AddCable(Cable cable, Device connectedDevice)
        {
            switch (cable.typeOfCable)
            {
                case cableType.Ethernet:
                    //connectedTo.Add(connectedDevice, cable);
                    //ethernetPorts.Add(cable);
                    foreach (var port in ethernetPorts)
                    {
                        if (port.connectedCable == null)
                        {
                            port.connectedCable = cable;
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
