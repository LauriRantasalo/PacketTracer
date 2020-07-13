using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Console.Commands
{
    interface ICommand
    {
        string[] Synonyms { get; set; }
        string Execute(List<string> commandParts);
    }
}
