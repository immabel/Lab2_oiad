using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2_oiad
{
    public class Cluster
    {
        List<MyPoint> points = new List<MyPoint>();
        public MyPoint currCentr;
        public MyPoint lastCentr;
        public List<MyPoint> Points { get { return points; } }
        public double Distance { get; set; }

        public Cluster(MyPoint pointCentr)
        {
            currCentr = pointCentr;
        }

        public int Size() { return points.Count; }

        public void Add(MyPoint point) { points.Add(point); }

        public void SetCenter()
        {
            double sumX = 0.0, sumY = 0.0;
            int size = Size();
            
            for (int i = 0; i < size; i++)
            {
                sumX += points[i].X;
                sumY += points[i].Y;
            }

            lastCentr.X = currCentr.X;
            lastCentr.Y = currCentr.Y;
            currCentr.X = sumX / size;
            currCentr.Y = sumY / size;
        }

        public double GetDistanceTo(MyPoint point)
        {
            return Distance = Math.Pow(Math.Pow(currCentr.X - point.X, 2) + Math.Pow(currCentr.Y - point.Y, 2), 0.5);
        }

        public void Clear()
        {
            points.Clear();
        }
    }
}
