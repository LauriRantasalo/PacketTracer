using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Protocols
{
    public enum PacketType { icmp, arp, tcp, udp}

    public class Packet
    {
        public string DestinationIpAddress { get; set; }
        public string DestinationMacAddress { get; set; }
        public string SourceIpAddress{ get; set; }
        public string SourceMacAddress { get; set; }
        public DateTime SendTime { get; set; }
        public PacketType TypeOfPacket { get; set; }
        
        public Packet(string destAddress, string srcAddress, string sourceMacAddress)
        {
            DestinationIpAddress = destAddress;
            SourceIpAddress = srcAddress;
            SourceMacAddress = sourceMacAddress;
            SendTime = DateTime.Now;
            
        }

        
    }
}
