using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Protocols
{
    public class ARPPacket : Packet
    {
        public string EchoType { get; set; }
        public ARPPacket(string destAddress, string sourceAddress, string sourceMacAddress, string echoType) : base(destAddress, sourceAddress, sourceMacAddress)
        {
            TypeOfPacket = PacketType.arp;
            EchoType = echoType;
        }

        public ARPPacket ToReply(string destinationMacAddress)
        {
            DestinationMacAddress = SourceMacAddress;
            SourceMacAddress = destinationMacAddress; // This is correct, dont worry
            string tempAddr = DestinationIpAddress;
            DestinationIpAddress = SourceIpAddress;
            SourceIpAddress = tempAddr;
            EchoType = "Reply";
            return this;
        }

    }
}
