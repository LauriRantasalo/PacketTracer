using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PacketTracer.Devices.Interfaces;
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
            Packet packet = new Packet(destinationAddress, sourceDevice.EthernetPorts[0].ipAddress, "Echo request");
            return sourceDevice.SendPacket(packet, sourceDevice.EthernetPorts[0]);
        }
    }
}
