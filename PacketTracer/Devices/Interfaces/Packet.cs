using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Interfaces
{
    public enum PacketType { icmp, tcp, udp}

    public class Packet
    {
        public string DestinationIpAddress { get; set; }
        public string SourceIpAddress{ get; set; }
        public string EchoType { get; set; }
        public DateTime SendTime { get; set; }
        public PacketType TypeOfPacket { get; set; }
        public int NroOfRoundsDone = 0;
        public int MaxNroOfRounds = 4;
        public Packet(string destAddress, string srcAddress, PacketType typeOfPacket, string echoType)
        {
            DestinationIpAddress = destAddress;
            SourceIpAddress = srcAddress;
            EchoType = echoType;
            SendTime = DateTime.Now;
            TypeOfPacket = typeOfPacket;
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
