using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketTracer.Devices.Interfaces
{
    public class Packet
    {
        public string DestinationIpAddress { get; set; }
        public string SourceIpAddress{ get; set; }
        public string EchoType { get; set; }
        public DateTime SendTime { get; set; }
        public int NroOfRoundsDone = 0;
        public int MaxNroOfRounds = 4;
        public Packet(string destAddress, string srcAddress, string echoType)
        {
            DestinationIpAddress = destAddress;
            SourceIpAddress = srcAddress;
            EchoType = echoType;
            SendTime = DateTime.Now;
        }

        public void ToReply()
        {
            string tempDest = DestinationIpAddress;
            DestinationIpAddress = SourceIpAddress;
            SourceIpAddress = tempDest;
            EchoType = "Echo reply";
            NroOfRoundsDone++;
        }
        public void ToRequest()
        {
            string tempDest = DestinationIpAddress;
            DestinationIpAddress = SourceIpAddress;
            SourceIpAddress = tempDest;
            EchoType = "Echo request";
        }
    }
}
