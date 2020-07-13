using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using System.Diagnostics;

using PacketTracer.Devices.Interfaces;
using PacketTracer.Devices.Console;

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
            TypeOfDevice = deviceType.Router;
            for (int i = 0; i < this.nroOfEthernetPorts; i++)
            {
                EthernetPorts.Add(new EthernetPort("192.168.0." + (10 + i).ToString()));
            }
            Terminal = new RouterTerminal(this);
        }

        /// <summary>
        /// Either route packets here from routers RecievePacket method or from Computers SendPacket
        /// </summary>
        /// <param name="destinationIpAdress"></param>
        /// <param name="physicalInterface">source physical interface</param>
        public override void RecievePacket(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface, string echoType)
        {
            Debug.WriteLine("Checking routing table");
            string destinationSubnet = destinationIpAdress.Remove(destinationIpAdress.LastIndexOf(".")) + ".0";
            bool noRoute = true;
            foreach (var routingTableRow in routingTable)
            {
                (Device aDevice, Device bDevice) = routingTableRow.physicalInterface.connectedCable.SortCableDevices(this);

                
                
                if (routingTableRow.nextHop == destinationIpAdress)
                {
                    Debug.WriteLine("Routing " + echoType + " from: " + sourceIpAdress + " to: " + destinationIpAdress);
                    noRoute = false;
                    // Routes  the packet to next router according to the routing table
                    bDevice.RecievePacket(destinationIpAdress, sourceIpAdress, routingTableRow.physicalInterface, echoType);
                }// If there is a subnet match to destination subnet and it is not the sending devices address
                else if (routingTableRow.subnet == destinationSubnet && routingTableRow.nextHop != physicalInterface.ipAddress)
                {
                    // Hop to next router
                    //throw new NotImplementedException();
                }
            }
            // This does not work
            foreach (var port in EthernetPorts)
            {
                if (port.ipAddress == destinationIpAdress)
                {
                    noRoute = false;
                    (Device aDevice, Device bDevice) = port.connectedCable.SortCableDevices(this);
                    bDevice.RecievePacket(sourceIpAdress, port.ipAddress, port, "Echo reply");
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
            Debug.WriteLine("From " + this.Name + ": " + routingTable[0]);
            // Possible routing protocol begins here?
        }
    }
}
