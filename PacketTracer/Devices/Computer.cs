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

        public async override void RecievePacketAsync(Packet packet, PhysicalInterface physicalInterface)
        {
            await Task.Delay(100);
            foreach (var port in EthernetPorts)
            {
                if (port.ipAddress == packet.DestinationIpAddress)
                {
                    (Device aDevice, Device bDevice) = physicalInterface.connectedCable.SortCableDevices(this);
                    if (packet.EchoType == "Echo request")
                    {
                        packet.ToReply();
                        bDevice.RecievePacketAsync(packet, EthernetPorts[0]);
                    }
                    else if (packet.EchoType == "Echo reply")
                    {

                        Terminal.TerminalOutput += "\n" + "Reply from " + packet.SourceIpAddress + ": Time: " + (DateTime.Now - packet.SendTime).ToString();
                        packet.ToRequest();
                        if (packet.NroOfRoundsDone < packet.MaxNroOfRounds)
                        {
                            packet.SendTime = DateTime.Now;
                            bDevice.RecievePacketAsync(packet, EthernetPorts[0]);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    return;
                }
            }
        }
        /*
         * public async override void RecievePacketAsync(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface, string echoType)
        {
            await Task.Delay(100);
            foreach (var port in EthernetPorts)
            {
                if (port.ipAddress == destinationIpAdress)
                {
                    (Device aDevice, Device bDevice) = physicalInterface.connectedCable.SortCableDevices(this);
                    if (echoType == "Echo request")
                    {
                        bDevice.RecievePacketAsync(sourceIpAdress, EthernetPorts[0].ipAddress, EthernetPorts[0], "Echo reply");
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
         */
    }
}