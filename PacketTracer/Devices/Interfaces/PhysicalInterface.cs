using PacketTracer.Cables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Interfaces
{
    public enum PhysicalInterfaceType { Ethernet, Serial, Console}
    public class PhysicalInterface
    {
        public string IpAddress { get; set; }
        public PhysicalInterfaceType TypeOfPhysicalInterface { get; set; }
        public Cable ConnectedCable { get; set; }
        public string MacAddress { get; set; }
        public string InterfaceName { get; set; }
        public PhysicalInterface(string ipAddress, string interfaceName, string macAddress)
        {
            IpAddress = ipAddress;
            InterfaceName = interfaceName;
            MacAddress = macAddress;
        }
    }
}
