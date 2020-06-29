using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Interfaces
{
    public class EthernetPort : PhysicalInterface
    {
        public EthernetPort(string ipAddress) : base(ipAddress)
        {
            typeOfPhysicalInterface = physicalInterfaceType.Ethernet;
        }
    }
}
