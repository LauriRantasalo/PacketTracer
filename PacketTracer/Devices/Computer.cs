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
            foreach (var port in EthernetPorts)
            {
                if (port.ipAddress == destinationIpAdress)
                {
                    (Device aDevice, Device bDevice) = physicalInterface.connectedCable.SortCableDevices(this);
                    if (echoType == "Echo request")
                    {
                        bDevice.RecievePacket(sourceIpAdress, EthernetPorts[0].ipAddress, EthernetPorts[0], "Echo reply");
                    }
                    else if (echoType == "Echo reply")
                    {
                        Terminal.TerminalOutput += "\n" + "Reply from " + sourceIpAdress;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    return;
                }
            }
        }
    }
}