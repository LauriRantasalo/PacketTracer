using System;
using System.Collections.Generic;
using System.Linq;

using PacketTracer.Devices.Console.Commands;

namespace PacketTracer.Devices.Console
{
    public class Terminal
    {
        UIManager uiManager;
        protected List<ConsoleCommand> CommandHistory { get; set; }

        private string terminalOutput = "";
        public string TerminalOutput
        {
            get
            {
                return terminalOutput;
            }
            set
            {
                if (value != null)
                {
                    //Debug.WriteLine("value was not null: " + value + " in " + device.EthernetPorts[0].ipAddress);
                    terminalOutput = value;
                    ComputerConfiguration temp = uiManager.GetComputerConfigurationWindow(device);
                    if (temp != null && temp.ContentFrame.Content.GetType() == typeof(ComputerConfigurationConsole))
                    {
                        ComputerConfigurationConsole console = (ComputerConfigurationConsole)temp.ContentFrame.Content;
                        uiManager.UpdateActiveConsoleAsync(console, value);
                    }
                }
            }
        }

        private ConsoleCommand[] computerCommands;
        private ConsoleCommand[] routerCommands;
        private Device device;
        public Terminal(UIManager uiManager, Device device)
        {
            computerCommands = new ConsoleCommand[] { new PingCommand() };
            routerCommands = new ConsoleCommand[] { new PingCommand() };
            this.device = device;
            this.uiManager = uiManager;
            TerminalOutput = "";
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
                            return computerCommand.Execute(device, commandParts);
                        }
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
