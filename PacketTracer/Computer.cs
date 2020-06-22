using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

namespace PacketTracer
{
    public class Computer
    {
        public Dictionary<Computer, Cable> connectedTo = new Dictionary<Computer, Cable>();
        public Rectangle rect;
        public string ipAddress;
        public string name;
        public Computer(Rectangle rectangle, string name, string ip)
        {
            rect = rectangle;
            this.name = name;
            ipAddress = ip;
        }
    }
}
