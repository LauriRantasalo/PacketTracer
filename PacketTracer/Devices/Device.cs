using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

using PacketTracer.Cables;
namespace PacketTracer.Devices
{
    public class Device
    {
        public string name;
        public Rectangle rect;

        public Dictionary<Device, Cable> connectedTo = new Dictionary<Device, Cable>();
    }
}
