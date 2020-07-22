using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PacketTracer.Devices.Interfaces;
namespace PacketTracer.Devices.Computers
{
    public class ArpTableRow
    {
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }

        public ArpTableRow(string macAddress, string ipAddress)
        {
            MacAddress = macAddress;
            IpAddress = ipAddress;
        }
    }
}
