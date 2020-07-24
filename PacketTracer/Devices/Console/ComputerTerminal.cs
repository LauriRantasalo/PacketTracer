using PacketTracer.Devices.Console.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Console
{
    public class ComputerTerminal : Terminal
    {
        private ConsoleCommand[] commands;
        public ComputerTerminal(UIManager uiManager, Device device) : base(uiManager, device)
        {
            commands = new ConsoleCommand[] { new PingCommand(), new ArpCommand() };
        }

        public override void ExecuteCommand(string command)
        {
            command.Trim();
            List<string> commandParts = command.Split(" ").ToList();
            foreach (var c in commands)
            {
                if (commandParts[0].ToLower() == c.Synonyms[0])
                {
                    c.Execute(this.device, commandParts);
                    return;
                }
            }
            device.Terminal.TerminalOutput += "\n" + commandParts[0] + " is not regognized as a command";
            //return commandParts[0] + " is not regognized as a command";

        }
    }
}
