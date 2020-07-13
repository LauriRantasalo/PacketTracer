using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Console.Commands
{
    public class ConsoleCommand : ICommand
    {
        public string[] Synonyms { get; set; }

        public virtual string Execute(List<string> commandParts)
        {
            return "base";
        }
    }
}
