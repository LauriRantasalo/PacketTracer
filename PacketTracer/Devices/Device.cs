using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;

using PacketTracer.Cables;
using Windows.UI.Xaml.Controls;

namespace PacketTracer.Devices
{
    public enum deviceType{Computer, Router, Switch};
    public class Device
    {
        public string name;
        public Grid baseGrid;
        public deviceType typeOfDevice;
        /// <summary>
        /// Simulates cable connection to device
        /// </summary>
        public Dictionary<Device, Cable> connectedTo = new Dictionary<Device, Cable>();
        public int nroOfEthernetPorts;
        /// <summary>
        /// Simulates ethernet port connections
        /// </summary>
        public List<Cable> ethernetPorts;

        public Device(string name, Grid baseGrid, int nroOfEthernetPorts)
        {
            ethernetPorts = new List<Cable>();
            this.nroOfEthernetPorts = nroOfEthernetPorts;
            this.name = name;
            this.baseGrid = baseGrid;
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
