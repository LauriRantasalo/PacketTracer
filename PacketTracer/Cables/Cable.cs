using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

using PacketTracer.Devices;
using System.Diagnostics;
using PacketTracer.Devices.Interfaces;

namespace PacketTracer.Cables
{
    public enum cableType {Ethernet, Console};
    public class Cable
    {
        private Point startPoint;
        private Point endPoint;
        public Polyline CableLine { get; set; }
        public cableType TypeOfCable { get; set; }
        public Device DeviceA { get; set; }
        public Device DeviceB{ get; set; }
        public Rectangle IndicatorA { get; set; }
        public Rectangle IndicatorB { get; set; }
        public Dictionary<Device, Rectangle> DeviceIndicatorDict { get; set; }
        public Cable(Point start, Point end, Device deviceA, Device deviceB)
        {
            DeviceIndicatorDict = new Dictionary<Device, Rectangle>();
            IndicatorA = new Rectangle();
            IndicatorB = new Rectangle();

            IndicatorA.Fill = new SolidColorBrush(Windows.UI.Colors.Red);
            IndicatorA.Width = 7;
            IndicatorA.Height = 7;

            IndicatorB.Fill = new SolidColorBrush(Windows.UI.Colors.Red);
            IndicatorB.Width = 7;
            IndicatorB.Height = 7;

            DeviceA = deviceA;
            DeviceB = deviceB;

            DeviceIndicatorDict.Add(DeviceA, IndicatorA);
            DeviceIndicatorDict.Add(DeviceB, IndicatorB);

            startPoint = start;
            endPoint = end;

            CableLine = new Polyline();
            CableLine.Points.Add(startPoint);
            CableLine.Points.Add(endPoint);

            CableLine.Stroke = new SolidColorBrush(Windows.UI.Colors.Green);
            CableLine.StrokeThickness = 4;
        }


        public PhysicalInterface GetPortOfDevice(Device device)
        {
            foreach (var port in device.EthernetPorts)
            {
                if (port.ConnectedCable == this)
                {
                    return port;
                }
            }
            return null;
        }
        public void ReDrawCable(Point start, Point end)
        {
            CableLine.Points.Clear();
            CableLine.Points.Add(start);
            CableLine.Points.Add(end);
        }
        /// <summary>
        /// Returns the 2 devices of connected cable so that deviceA is the deviceToSortBy
        /// </summary>
        /// <param name="deviceToSortBy"></param>
        /// <returns></returns>
        public (Device aDevice, Device bDevice) SortCableDevices(Device deviceToSortBy)
        {
            if (DeviceA == deviceToSortBy)
            {
                return (DeviceA, DeviceB);
            }
            else if (DeviceB == deviceToSortBy)
            {
                return (DeviceB, DeviceA);
            }
            else
            {
                return (null, null);
            }
        }
    }

    
}
