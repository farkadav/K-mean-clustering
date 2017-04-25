using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kMeanClust
{
    public class CenterOfMass
    {
        public CenterOfMass(List<Point> cluster )
        {
            Cluster = cluster;
        }

        public Point Calculate()
        {
            double sumX = 0;
            double sumY = 0;
            foreach (var point in Cluster)
            {
               sumX += point.X;
               sumY += point.Y;
            }
            double cX = sumX/Cluster.Count;
            double cY = sumY / Cluster.Count;

            if (double.IsNaN(cX) | double.IsNaN(cY))
            {
                Random k1 = new Random();
                cX = k1.NextDouble()*20;
                cY = k1.NextDouble() * 20;
            }

            Point new_center = new Point(cX,cY);
            return new_center;
        }

        public List<Point> Cluster { get; private set; }
    }
}
