using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

using PacketTracer.Cables;
using Windows.UI.Xaml.Controls;

namespace PacketTracer.Devices
{
    public class Computer : Device
    {
        //public const string typeOfDevice = "Computer";
        public Computer(Grid baseGrid, string name, int nroOfEthernetPorts) : base(name, baseGrid, nroOfEthernetPorts)
        {
            typeOfDevice = deviceType.Computer;
        }
    }
}