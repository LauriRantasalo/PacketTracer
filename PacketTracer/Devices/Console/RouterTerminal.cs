using PacketTracer.Devices.Console.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Console
{
    public class RouterTerminal : Terminal
    {
        ConsoleCommand[] commands;
        public RouterTerminal(UIManager uiManager, Device device) : base(uiManager, device)
        {
            commands = new ConsoleCommand[] { new PingCommand() };
        }

        public override string ExecuteCommand(string command)
        {
            throw new NotImplementedException();
        }
    }
}
