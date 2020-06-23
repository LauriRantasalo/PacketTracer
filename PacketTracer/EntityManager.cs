using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PacketTracer.Devices;

namespace PacketTracer
{
    public class EntityManager
    {
        public List<Device> devices;

        public EntityManager()
        {
            devices = new List<Device>();
        }

    }
}
