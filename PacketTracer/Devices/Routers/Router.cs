using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;

using PacketTracer.Devices.Interfaces;
using PacketTracer.Devices.Console;

namespace PacketTracer.Devices.Routers
{
    public class Router : Device
    {
        /// <summary>
        /// [subnet, next-hop, interface]
        /// </summary>
        //List<(string subnet, string nextHop, PhysicalInterface physicalInterface)> routingTable = new List<(string, string, PhysicalInterface)>();
        List<RoutingTableRow> routingTable = new List<RoutingTableRow>();
        public Router(UIManager uiManager, Grid baseGrid, string name, int nroOfEthernetPorts) : base(uiManager, name, baseGrid, nroOfEthernetPorts)
        {
            TypeOfDevice = deviceType.Router;
            for (int i = 0; i < this.nroOfEthernetPorts; i++)
            {
                EthernetPorts.Add(new EthernetPort("192.168.0." + (10 + i).ToString()));
            }
            Terminal = new RouterTerminal(uiManager, this);
        }

        /// <summary>
        /// Either route packets here from routers RecievePacket method or from Computers SendPacket
        /// </summary>
        /// <param name="destinationIpAdress"></param>
        /// <param name="physicalInterface">last device physical interface</param>
        public override void RecievePacket(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface, string echoType)
        {
            bool isDestinationDevice = false;
            foreach (var port in EthernetPorts)
            {
                if (port != null && port.ipAddress == destinationIpAdress)
                {
                    Debug.WriteLine("here");
                    isDestinationDevice = true;
                    (Device iDevice, Device jDevice) = port.connectedCable.SortCableDevices(this);
                    jDevice.RecievePacket(sourceIpAdress, port.ipAddress, port, "Echo reply");
                    break;
                }
            }

            if (!isDestinationDevice)
            {
                RoutingTableRow routingTableRow = CheckRoutingTable(destinationIpAdress, sourceIpAdress, physicalInterface, echoType);

                (Device aDevice, Device bDevice) = routingTableRow.PhysicalInterface.connectedCable.SortCableDevices(this);
                bDevice.RecievePacket(destinationIpAdress, sourceIpAdress, routingTableRow.PhysicalInterface, echoType);
            }
            
        }

        /// <summary>
        /// Checks if there is a known route and returns routing table row
        /// </summary>
        /// <returns></returns>
        public RoutingTableRow CheckRoutingTable(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface, string echoType)
        {
            Debug.WriteLine("Checking routing table");
            string destinationSubnet = destinationIpAdress.Remove(destinationIpAdress.LastIndexOf(".")) + ".0";
            bool noRoute = true;
            foreach (var routingTableRow in routingTable)
            {
                if (routingTableRow.NextHop == destinationIpAdress)
                {
                    Debug.WriteLine("Routing " + echoType + " from: " + sourceIpAdress + " to: " + destinationIpAdress);
                    noRoute = false;
                    return routingTableRow;
                }// If there is a subnet match to destination subnet and it is not the sending devices address
                else if (routingTableRow.Subnet == destinationSubnet && routingTableRow.NextHop != physicalInterface.ipAddress)
                {
                    // Hop to next router
                    //throw new NotImplementedException();
                }
            }

            if (noRoute)
            {
                Debug.WriteLine("No route found to: " + destinationIpAdress);
                return null;
            }

            return null;
        }

        public void AddNewRoutingTableRoute(string subnet, string nextHopIP, PhysicalInterface physicalInterface)
        {
            routingTable.Add(new RoutingTableRow(subnet, nextHopIP, physicalInterface));
            foreach (var item in routingTable)
            {
                Debug.WriteLine(item.Subnet + " " + item.NextHop + " " + item.PhysicalInterface.ipAddress);
            }
            //Debug.WriteLine("From " + this.Name + ": " + routingTable[0]);
            // Possible routing protocol begins here?
        }
    }
}
