using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketTracer.Cables;
using PacketTracer.Devices;
using PacketTracer.Devices.Interfaces;
using PacketTracer.Devices.Routers;

namespace PacketTracer
{
    public class EntityManager
    {
        public List<Device> Devices { get; set; }
        public EntityManager()
        {
            Devices = new List<Device>();
        }
        // This might need a rework
        public void ConnectCableToDevices(Device deviceA, PhysicalInterface devAInterface, Device deviceB, PhysicalInterface devBInterface, Cable cable)
        {
            switch (cable.TypeOfCable)
            {
                case cableType.Ethernet:
                    devAInterface.ConnectedCable = cable;
                    devBInterface.ConnectedCable = cable;



                    if (deviceA.GetType() == typeof(Switch))
                    {
                        Switch temp = (Switch)deviceA;
                        string subnet = devAInterface.IpAddress.Remove(devAInterface.IpAddress.LastIndexOf("."));
                        string nextHopIp = devBInterface.IpAddress;
                        temp.AddNewRoutingTableRoute(subnet + ".0", nextHopIp, devAInterface);
                    }

                    if (deviceB.GetType() == typeof(Switch))
                    {
                        Switch temp = (Switch)deviceB;
                        string subnet = devBInterface.IpAddress.Remove(devBInterface.IpAddress.LastIndexOf("."));
                        string nextHopIp = devAInterface.IpAddress;
                        temp.AddNewRoutingTableRoute(subnet + ".0", nextHopIp, devBInterface);
                    }
                    break;
                case cableType.Console:
                    break;
                default:
                    throw new NotImplementedException();
            }
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

        public List<Switch> GetRouters()
        {
            List<Switch> routers = new List<Switch>();
            foreach (var device in Devices)
            {
                if (device.TypeOfDevice == deviceType.Router)
                {
                    routers.Add((Switch)device);
                }
            }
            return routers;
        }
    }
}
