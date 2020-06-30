using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;

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
            for (int i = 0; i < this.nroOfEthernetPorts; i++)
            {
                ethernetPorts.Add(new EthernetPort("192.168.0.10"));
            }
        }

        public override void RecievePacket(string destinationIpAdress, PhysicalInterface physicalInterface)
        {
            Debug.WriteLine("Check routing table");
            //base.RecievePacket(destinationIpAdress, physicalInterface);
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
            Debug.WriteLine(routingTable[0]);
            // Possible routing protocol begins here?
        }
    }
}
