using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media;

using PacketTracer.Devices;
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
        public Cable(Point start, Point end)
        {
            startPoint = start;
            endPoint = end;
            CableLine = new Polyline();
            CableLine.Points.Add(startPoint);
            CableLine.Points.Add(endPoint);

            CableLine.Stroke = new SolidColorBrush(Windows.UI.Colors.Green);
            CableLine.StrokeThickness = 4;
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
