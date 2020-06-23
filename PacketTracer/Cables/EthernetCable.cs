using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

using PacketTracer.Devices;
namespace PacketTracer.Cables
{
    public class EthernetCable : Cable
    {
        
       public EthernetCable(Point start, Point end) : base(start, end)
       {
            this.type = "Ethernet";
       }

        
    }
}
