using PacketTracer.Devices.Console.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Console
{
    public class SwitchTerminal : Terminal
    {
        ConsoleCommand[] commands;
        public SwitchTerminal(UIManager uiManager, Device device) : base(uiManager, device)
        {
            commands = new ConsoleCommand[] { new PingCommand() };
        }

        public override void ExecuteCommand(string command)
        {
            throw new NotImplementedException();
        }
    }
}
