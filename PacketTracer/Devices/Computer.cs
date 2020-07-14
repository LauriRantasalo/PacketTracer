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
using PacketTracer.Devices.Console;

namespace PacketTracer.Devices
{
    public class Computer : Device
    {
        public Computer(UIManager uiManager, Grid baseGrid, string name, int nroOfEthernetPorts, string defaultEthernetPortIp) : base(uiManager, name, baseGrid, nroOfEthernetPorts)
        {
            TypeOfDevice = deviceType.Computer;
            for (int i = 0; i < this.nroOfEthernetPorts; i++)
            {
                if (i == 0)
                {
                    EthernetPorts.Add(new EthernetPort(defaultEthernetPortIp));
                }
                else
                {
                    EthernetPorts.Add(new EthernetPort("NULL"));
                }
            }

            Terminal = new ComputerTerminal(uiManager, this);
        }

        public override void RecievePacket(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface, string echoType)
        {
            base.RecievePacket(destinationIpAdress, sourceIpAdress, physicalInterface, echoType);
        }
    }
}