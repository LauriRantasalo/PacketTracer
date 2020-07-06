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
        List<(string subnet, string nextHop, PhysicalInterface physicalInterface)> routingTable = new List<(string, string, PhysicalInterface)>();
        public Router(Grid baseGrid, string name, int nroOfEthernetPorts) : base(name, baseGrid, nroOfEthernetPorts)
        {
            typeOfDevice = deviceType.Router;
            for (int i = 0; i < this.nroOfEthernetPorts; i++)
            {
                ethernetPorts.Add(new EthernetPort("192.168.0." + (10 + i).ToString()));
            }
        }

        /// <summary>
        /// Either route packets here from routers RecievePacket method or from Computers SendPacket
        /// </summary>
        /// <param name="destinationIpAdress"></param>
        /// <param name="physicalInterface">source physical interface</param>
        public override void RecievePacket(string destinationIpAdress, PhysicalInterface physicalInterface)
        {
            Debug.WriteLine("Checking routing table");
            string destinationSubnet = destinationIpAdress.Remove(destinationIpAdress.LastIndexOf(".")) + ".0";
            bool noRoute = true;
            foreach (var routingTableRow in routingTable)
            {
                (Device aDevice, Device bDevice) = routingTableRow.physicalInterface.connectedCable.SortCableDevices(this);

                
                
                if (routingTableRow.nextHop == destinationIpAdress)
                {
                    Debug.WriteLine("Routing packet to destination ip");
                    // Routes  the packet to next router according to the routing table
                    bDevice.RecievePacket(destinationIpAdress, routingTableRow.physicalInterface);
                    noRoute = false;
                    break;
                }// If there is a subnet match to destination subnet and it is not the sending devices address
                else if (routingTableRow.subnet == destinationSubnet && routingTableRow.nextHop != physicalInterface.ipAddress)
                {
                    // Hop to next router
                }
            }

            if (noRoute)
            {
                Debug.WriteLine("No route found to: " + destinationIpAdress);
            }
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
            Debug.WriteLine("From " + this.name + ": " + routingTable[0]);
            // Possible routing protocol begins here?
        }
    }
}
