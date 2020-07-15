using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PacketTracer.Devices.Interfaces;
namespace PacketTracer.Devices.Routers
{
    public class RoutingTableRow
    {
        public string Subnet { get; set; }
        public string NextHop { get; set; }
        public PhysicalInterface PhysicalInterface { get; set; }
        public RoutingTableRow(string subnet, string nextHop, PhysicalInterface physicalInterface)
        {
            Subnet = subnet;
            NextHop = nextHop;
            PhysicalInterface = physicalInterface;
        }
    }
}
