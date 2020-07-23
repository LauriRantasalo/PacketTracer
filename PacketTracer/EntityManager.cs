using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacketTracer.Cables;
using PacketTracer.Devices;
using PacketTracer.Devices.Computers;
using PacketTracer.Devices.Interfaces;
using PacketTracer.Devices.Routers;
using PacketTracer.Devices.Switches;

namespace PacketTracer
{
    public class EntityManager
    {
        public List<Device> Devices { get; set; }
        private char[] macAddressChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        public EntityManager()
        {
            Devices = new List<Device>();
        }

        public string GenerateNewMacAddress()
        {
            string address = "";
            Random rnd = new Random();
            int addressLength = 17;
            for (int i = 1; i <= addressLength; i++)
            {
                if (i % 3 == 0)
                {
                    address += ":";
                }
                else
                {
                    address += macAddressChars[rnd.Next(macAddressChars.Length)];
                }
            }

            foreach (var device in Devices)
            {
                foreach (var port in device.EthernetPorts)
                {
                    if (port.MacAddress == address)
                    {
                        return GenerateNewMacAddress();
                    }
                }
            }
            return address;
                
        }

        // This might need a rework
        public void ConnectCableToDevices(Device deviceA, PhysicalInterface devAInterface, Device deviceB, PhysicalInterface devBInterface, Cable cable)
        {
            switch (cable.TypeOfCable)
            {
                case cableType.Ethernet:
                    devAInterface.ConnectedCable = cable;
                    devBInterface.ConnectedCable = cable;
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

        public List<NetworkSwitch> GetSwitches()
        {
            List<NetworkSwitch> switches = new List<NetworkSwitch>();
            foreach (var device in Devices)
            {
                if (device.TypeOfDevice == deviceType.Router)
                {
                    switches.Add((NetworkSwitch)device);
                }
            }
            return switches;
        }
    }
}
