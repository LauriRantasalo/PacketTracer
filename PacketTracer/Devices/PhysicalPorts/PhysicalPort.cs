using PacketTracer.Cables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.PhysicalPorts
{
    public enum physicalPortType { Ethernet, Console}
    public class PhysicalPort
    {
        public string ipAddress;
        public physicalPortType typeOfPhysicalPort;
        public Cable connectedCable;
        public PhysicalPort(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }
    }
}
