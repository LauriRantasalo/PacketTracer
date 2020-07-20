using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;

using PacketTracer.Devices.Interfaces;
using PacketTracer.Devices.Console;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

namespace PacketTracer.Devices.Routers
{
    public class Switch : Device
    {
        /// <summary>
        /// [subnet, next-hop, interface]
        /// </summary>
        //List<(string subnet, string nextHop, PhysicalInterface physicalInterface)> routingTable = new List<(string, string, PhysicalInterface)>();
        List<SwitchingTableRow> switchingTable = new List<SwitchingTableRow>();
        public Switch(UIManager uiManager, Grid baseGrid, string name, int nroOfEthernetPorts) : base(uiManager, name, baseGrid, nroOfEthernetPorts)
        {
            TypeOfDevice = deviceType.Router;
            for (int i = 0; i < this.nroOfEthernetPorts; i++)
            {
                EthernetPorts.Add(new EthernetPort("192.168.0." + (10 + i).ToString(), "Gi0/" + i.ToString()));
            }
            Terminal = new RouterTerminal(uiManager, this);
        }

        /// <summary>
        /// Either route packets here from routers RecievePacket method or from Computers SendPacket
        /// </summary>
        /// <param name="destinationIpAdress"></param>
        /// <param name="physicalInterface">last device physical interface</param>
        public async override void RecievePacketAsync(Packet packet, PhysicalInterface physicalInterface)
        {
            await Task.Delay(100);
            bool isDestinationDevice = false;
            foreach (var port in EthernetPorts)
            {
                if (port != null && port.IpAddress == packet.DestinationIpAddress)
                {
                    isDestinationDevice = true;
                    (Device iDevice, Device jDevice) = port.ConnectedCable.SortCableDevices(this);
                    packet.ToReply();
                    SwitchingTableRow tempRow = CheckSwitchingTable(packet.DestinationIpAddress, packet.SourceIpAddress, physicalInterface);
                    SendPacket(packet, tempRow.PhysicalInterface);
                    break;
                }
            }

            if (!isDestinationDevice)
            {
                SwitchingTableRow routingTableRow = CheckSwitchingTable(packet.DestinationIpAddress, packet.SourceIpAddress, physicalInterface);

                (Device aDevice, Device bDevice) = routingTableRow.PhysicalInterface.ConnectedCable.SortCableDevices(this);
                SendPacket(packet, routingTableRow.PhysicalInterface);
            }

        }

        /// <summary>
        /// Checks if there is a known route and returns routing table row
        /// </summary>
        /// <returns></returns>
        public SwitchingTableRow CheckSwitchingTable(string destinationIpAdress, string sourceIpAdress, PhysicalInterface physicalInterface)
        {
            string destinationSubnet = destinationIpAdress.Remove(destinationIpAdress.LastIndexOf(".")) + ".0";
            bool noRoute = true;
            foreach (var routingTableRow in switchingTable)
            {
                /*
                 * if (routingTableRow.NextHop == destinationIpAdress)
                {
                    //Debug.WriteLine("Routing " + echoType + " from: " + sourceIpAdress + " to: " + destinationIpAdress);
                    noRoute = false;
                    return routingTableRow;
                }// If there is a subnet match to destination subnet and it is not the sending devices address
                else if (routingTableRow.Subnet == destinationSubnet && routingTableRow.NextHop != physicalInterface.IpAddress)
                {
                    // Hop to next router
                    //throw new NotImplementedException();
                }
                 */
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
            //switchingTable.Add(new SwitchingTableRow(subnet, nextHopIP, physicalInterface));
            /*
            foreach (var item in routingTable)
            {
                Debug.WriteLine(item.Subnet + " " + item.NextHop + " " + item.PhysicalInterface.ipAddress);
            }
            */
            //Debug.WriteLine("From " + this.Name + ": " + routingTable[0]);
            // Possible routing protocol begins here?
        }
    }
}
