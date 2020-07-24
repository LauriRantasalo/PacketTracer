using PacketTracer.Devices.Computers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Console.Commands
{
    class ArpCommand : ConsoleCommand
    {
        public ArpCommand()
        {
            Synonyms = new string[]{ "arp"};
        }
        public override void Execute(Device sourceDevice, List<string> commandParts)
        {
            try
            {
                Computer temp = (Computer)sourceDevice;
                if (temp.ArpTable.Count <= 0)
                {
                    sourceDevice.Terminal.TerminalOutput += "\nNo arp table entries";
                }
                else
                {
                    foreach (var row in temp.ArpTable)
                    {
                        sourceDevice.Terminal.TerminalOutput += "\n" + row.IpAddress + " " + row.MacAddress;
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
    }
}
