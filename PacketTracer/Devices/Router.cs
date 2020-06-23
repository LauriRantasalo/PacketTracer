using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace PacketTracer.Devices
{
    public class Router : Device
    {
        public Router(Grid baseGrid, string name, int nroOfEthernetPorts) : base(name, baseGrid, nroOfEthernetPorts)
        {
            typeOfDevice = deviceType.Router;
        }
    }
}
