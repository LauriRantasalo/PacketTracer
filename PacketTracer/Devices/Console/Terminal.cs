using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using PacketTracer.Devices.Console.Commands;

namespace PacketTracer.Devices.Console
{
    public abstract class Terminal
    {
        UIManager uiManager;
        public List<ConsoleCommand> CommandHistory { get; set; }
        protected Device device;
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
                    //Debug.WriteLine("value: " + value + "\nEOF");
                    terminalOutput = value;
                    Debug.WriteLine("terminalOutput" + terminalOutput + "\nEOF");
                    ComputerConfiguration temp = uiManager.GetComputerConfigurationWindow(device);
                    if (temp != null && temp.ContentFrame.Content.GetType() == typeof(ComputerConfigurationConsole))
                    {
                        ComputerConfigurationConsole console = (ComputerConfigurationConsole)temp.ContentFrame.Content;
                        uiManager.UpdateActiveConsoleAsync(console, terminalOutput);
                    }
                }
            }
        }

        public Terminal(UIManager uiManager, Device device)
        {
            this.device = device;
            this.uiManager = uiManager;
        }

        public abstract string ExecuteCommand(string command);
        
    }
}
