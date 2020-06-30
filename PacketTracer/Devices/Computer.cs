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

        public override void RecievePacket(string destinationIpAdress, PhysicalInterface physicalInterface)
        {
            if (ethernetPorts[0].ipAddress != destinationIpAdress)
            {
                Debug.WriteLine("Wrong place");
                Debug.WriteLine(destinationIpAdress + " _ " + ethernetPorts[0].ipAddress);
            }
            else
            {
                Debug.WriteLine("Right place");
            }
            //base.RecievePacket(destinationIpAdress, physicalInterface);
        }
    }
}