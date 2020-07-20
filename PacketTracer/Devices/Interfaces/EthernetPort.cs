﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

namespace PacketTracer.Devices.Interfaces
{
    public class EthernetPort : PhysicalInterface
    {
        public EthernetPort(string ipAddress, string interfaceName) : base(ipAddress, interfaceName)
        {
            TypeOfPhysicalInterface = PhysicalInterfaceType.Ethernet;
        }
    }
}
