using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PacketTracer.Devices.Interfaces;
namespace PacketTracer.Devices.Routers
{
    public class SwitchingTableRow
    {
        public string MacAddress { get; set; }
        public PhysicalInterface PhysicalInterface { get; set; }
        public SwitchingTableRow(string macAddress, PhysicalInterface physicalInterface)
        {
            MacAddress = macAddress;
            PhysicalInterface = physicalInterface;
        }
    }
}
