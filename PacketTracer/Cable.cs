using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace PacketTracer
{
    public class Cable
    {
        public Polyline line;
        public Point startPoint, endPoint;
        public Computer computerA, computerB;
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

        public void ReDrawLine(Point start, Point end)
        {
            line.Points.Clear();
            line.Points.Add(start);
            line.Points.Add(end);
        }

    }
}
