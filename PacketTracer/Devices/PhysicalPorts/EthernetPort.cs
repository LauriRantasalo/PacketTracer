using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.PhysicalPorts
{
    public class EthernetPort : PhysicalPort
    {
        public EthernetPort(string ipAddress) : base(ipAddress)
        {
            typeOfPhysicalPort = physicalPortType.Ethernet;
        }
    }
}
