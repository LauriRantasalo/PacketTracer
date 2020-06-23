using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

using PacketTracer.Cables;
using System.Diagnostics;
using PacketTracer.Helpers;
namespace PacketTracer.Devices
{
    public class Device
    {
        public string name;
        public Rectangle rect;
        /// <summary>
        /// Simulates cable connection to device
        /// </summary>
        public Dictionary<Device, Cable> connectedTo = new Dictionary<Device, Cable>();
        public int nroOfEthernetPorts;
        /// <summary>
        /// Simulates ethernet port connections
        /// </summary>
        public List<Cable> ethernetPorts;

        public Device(string name, Rectangle rect, int nroOfEthernetPorts)
        {
            ethernetPorts = new List<Cable>();
            this.nroOfEthernetPorts = nroOfEthernetPorts;
            this.name = name;
            this.rect = rect;
        }
        
        public void AddCable(Cable cable, Device connectedDevice)
        {
            switch (cable.type)
            {
                case EthernetCable.cableType:
                    connectedTo.Add(connectedDevice, cable);
                    ethernetPorts.Add(cable);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
