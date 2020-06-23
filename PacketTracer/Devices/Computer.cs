using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

using PacketTracer.Cables;
namespace PacketTracer.Devices
{
    public class Computer : Device
    {
        public Computer(Rectangle rectangle, string name, int nroOfEthernetPorts) : base(name, rectangle, nroOfEthernetPorts){}
    }
}