using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Console.Commands
{
    class PingCommand : ConsoleCommand
    {
        public PingCommand()
        {
            Synonyms = new string[] { "ping"};
        }


        public override string Execute(List<string> commandParts)
        {
            return "pingPong";
        }
    }
}
