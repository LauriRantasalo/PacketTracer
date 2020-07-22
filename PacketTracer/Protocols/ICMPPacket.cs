using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Protocols
{
    // NOTE: It might be bad that we are sending the same packet over and over again, its fine for now but probably need to change this to how it is irl
    public class ICMPPacket : Packet
    {
        public string EchoType { get; set; }
        public int NroOfRoundsDone = 0;
        public int MaxNroOfRounds = 4;
        public ICMPPacket(string destAddress, string srcAddress, string sourceMacAddress, string echoType) : base (destAddress, srcAddress, sourceMacAddress)
        {
            EchoType = echoType;
            TypeOfPacket = PacketType.icmp;
        }

        public Packet ToReply()
        {
            string tempDest = DestinationIpAddress;
            DestinationIpAddress = SourceIpAddress;
            SourceIpAddress = tempDest;
            EchoType = "Echo reply";
            NroOfRoundsDone++;
            return this;
        }
        public Packet ToRequest()
        {
            string tempDest = DestinationIpAddress;
            DestinationIpAddress = SourceIpAddress;
            SourceIpAddress = tempDest;
            EchoType = "Echo request";
            return this;
        }
    }
}
