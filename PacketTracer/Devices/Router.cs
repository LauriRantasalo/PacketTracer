using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

using PacketTracer.Devices.Interfaces;
namespace PacketTracer.Devices
{
    public class Router : Device
    {
        /// <summary>
        /// [subnet, next-hop, interface]
        /// </summary>
        List<(string, string, PhysicalInterface)> routingTable = new List<(string, string, PhysicalInterface)>();
        public Router(Grid baseGrid, string name, int nroOfEthernetPorts) : base(name, baseGrid, nroOfEthernetPorts)
        {
            typeOfDevice = deviceType.Router;
        }

        public void RecievePacket(string destinationIpAdress, PhysicalInterface physicalInterface)
        {

        }

        /// <summary>
        /// Add's new route to routing tabel
        /// </summary>
        /// <param name="subnet"></param>
        /// <param name="nextHopIP"></param>
        /// <param name="physicalInterface"></param>
        public void AddNewRoute(string subnet, string nextHopIP, PhysicalInterface physicalInterface)
        {
            routingTable.Add((subnet, nextHopIP, physicalInterface));
            // Possible routing protocol here?
        }
    }
}
