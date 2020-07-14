using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Console.Commands
{
    class UnknownCommand : ICommand
    {
        public string[] Synonyms { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Execute(Device sourceDevice, List<string> commandParts)
        {
            throw new NotImplementedException();
        }
    }
}
