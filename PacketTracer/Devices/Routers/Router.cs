using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketTracer.Devices.Interfaces;
using PacketTracer.Protocols;

namespace PacketTracer.Devices.Routers
{
    class Router : Device
    {
        public Router(UIManager uIManager, EntityManager entityManager) : base(uIManager, "null", new Windows.UI.Xaml.Controls.Grid(), 0)
        {

        }
        public override void RecievePacketAsync(Packet packet, PhysicalInterface physicalInterface)
        {
            throw new NotImplementedException();
        }
    }
}
