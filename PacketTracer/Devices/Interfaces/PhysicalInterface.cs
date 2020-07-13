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
        public string ipAddress { get; set; }
        public physicalInterfaceType typeOfPhysicalInterface { get; set; }
        public Cable connectedCable { get; set; }
        public PhysicalInterface(string ipAddress)
        {
            this.ipAddress = ipAddress;
        }
    }
}
