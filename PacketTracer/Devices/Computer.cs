using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;

using PacketTracer.Cables;
using PacketTracer.Devices.Interfaces;
using System.Diagnostics;

namespace PacketTracer.Devices
{
    public class Computer : Device
    {
        public Computer(Grid baseGrid, string name, int nroOfEthernetPorts, string defaultEthernetPortIp) : base(name, baseGrid, nroOfEthernetPorts)
        {
            typeOfDevice = deviceType.Computer;
            for (int i = 0; i < this.nroOfEthernetPorts; i++)
            {
                if (i == 0)
                {
                    ethernetPorts.Add(new EthernetPort(defaultEthernetPortIp));
                }
                else
                {
                    ethernetPorts.Add(new EthernetPort("NULL"));
                }
            }
        }

        public override void RecievePacket(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface, string echoType)
        {
            base.RecievePacket(destinationIpAdress, sourceIpAdress, physicalInterface, echoType);
        }
    }
}