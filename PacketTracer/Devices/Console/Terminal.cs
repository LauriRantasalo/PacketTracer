using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PacketTracer.Devices.Console.Commands;
namespace PacketTracer.Devices.Console
{
    public class Terminal
    {
        protected List<ConsoleCommand> CommandHistory { get; }
        public string TerminalOutput { get; }

        private ConsoleCommand[] computerCommands;
        private ConsoleCommand[] routerCommands = { new PingCommand() };
        private Device device;
        public Terminal(Device device)
        {
            computerCommands = new ConsoleCommand[] { new PingCommand() };
            this.device = device;
        }
       

        public string ExecuteCommand(string command)
        {
            command.Trim();
            List<string> commandParts = command.Split(" ").ToList();
            switch (device.TypeOfDevice)
            {
                case deviceType.Computer:
                    foreach (var computerCommand in computerCommands)
                    {
                        if (commandParts[0].ToLower() == computerCommand.Synonyms[0])
                        {
                            return computerCommand.Execute(commandParts);
                        }
                        Debug.WriteLine("I dont think this should happen whit just one command in computercommands");
                    }
                    return commandParts[0] + " is not regognized as a computer command";
                case deviceType.Router:
                    throw new NotImplementedException();
                case deviceType.Switch:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
