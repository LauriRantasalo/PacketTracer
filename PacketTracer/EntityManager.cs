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

        public List<Computer> GetComputers()
        {
            List<Computer> computers = new List<Computer>();
            foreach (var device in devices)
            {
                if (device.typeOfDevice == deviceType.Computer)
                {
                    computers.Add((Computer)device);
                }
            }
            return computers;
        }

        public List<Router> GetRouters()
        {
            List<Router> routers = new List<Router>();
            foreach (var device in devices)
            {
                if (device.typeOfDevice == deviceType.Router)
                {
                    routers.Add((Router)device);
                }
            }
            return routers;
        }
    }
}
