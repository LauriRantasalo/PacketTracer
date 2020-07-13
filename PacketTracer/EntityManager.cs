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
        public List<Device> Devices { get; set; }
        public EntityManager()
        {
            Devices = new List<Device>();
        }
        public Device GetDeviceByName(string name)
        {
            return Devices.Find(x => x.Name == name);
        }
        public List<Computer> GetComputers()
        {
            List<Computer> computers = new List<Computer>();
            foreach (var device in Devices)
            {
                if (device.TypeOfDevice == deviceType.Computer)
                {
                    computers.Add((Computer)device);
                }
            }
            return computers;
        }

        public List<Router> GetRouters()
        {
            List<Router> routers = new List<Router>();
            foreach (var device in Devices)
            {
                if (device.TypeOfDevice == deviceType.Router)
                {
                    routers.Add((Router)device);
                }
            }
            return routers;
        }
    }
}
