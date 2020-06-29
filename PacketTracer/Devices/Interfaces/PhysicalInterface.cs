using PacketTracer.Cables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Interfaces
{
    public enum physicalInterfaceType { Ethernet, Serial, Console}
    public class PhysicalInterface
    {
        public string ipAddress;
        public physicalInterfaceType typeOfPhysicalInterface;
        public Cable connectedCable;
        public PhysicalInterface(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }
    }
}
