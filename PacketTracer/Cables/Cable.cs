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
    public class Cable
    {
        public Polyline line;
        public Point startPoint, endPoint;
        public Device deviceA, deviceB;
        public string type;
        public Cable(Point start, Point end)
        {
            startPoint = start;
            endPoint = end;
            line = new Polyline();
            line.Points.Add(startPoint);
            line.Points.Add(endPoint);

            line.Stroke = new SolidColorBrush(Windows.UI.Colors.Green);
            line.StrokeThickness = 4;
        }

        public void ReDrawCable(Point start, Point end)
        {
            line.Points.Clear();
            line.Points.Add(start);
            line.Points.Add(end);
        }
    }

    
}
