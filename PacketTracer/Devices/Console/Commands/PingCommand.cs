using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PacketTracer.Devices.Interfaces;
using PacketTracer.Protocols;

namespace PacketTracer.Devices.Console.Commands
{
    class PingCommand : ConsoleCommand
    {
        public PingCommand()
        {
            Synonyms = new string[] { "ping"};
        }

        public override string Execute(Device sourceDevice, List<string> commandParts)
        {
            string destinationAddress = commandParts[1];
            ICMPPacket packet = new ICMPPacket(destinationAddress, sourceDevice.EthernetPorts[0].IpAddress, sourceDevice.EthernetPorts[0].MacAddress, "Echo request");
            return sourceDevice.SendPacket(packet, sourceDevice.EthernetPorts[0]);
        }
    }
}
