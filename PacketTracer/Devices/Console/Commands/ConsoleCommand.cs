using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Console.Commands
{
    public abstract class ConsoleCommand
    {
        public string[] Synonyms { get; set; }

        public abstract void Execute(Device sourceDevice, List<string> commandParts);
    }
}
