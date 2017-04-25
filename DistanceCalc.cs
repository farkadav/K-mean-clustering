using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kMeanClust
{
    public class DistanceCalc
    {

        public DistanceCalc(List<Point> data, List<Point> center)
        {
            Data = data;
            Centers = center;

        }

        public List<List<Point>> SortPointsToClusters()
        {
            var clusters = new List<List<Point>>();
            var cluster1 = new List<Point>();
            var cluster2 = new List<Point>();

            foreach (var point in Data)
            {
                var distance1 = Math.Sqrt(Math.Pow(point.X - Centers[0].X, 2) + Math.Pow(point.Y - Centers[0].Y, 2));
                var distance2 = Math.Sqrt(Math.Pow(point.X - Centers[1].X, 2) + Math.Pow(point.Y - Centers[1].Y, 2));

                if (distance1 < distance2)
                    cluster1.Add(point);
                else
                    cluster2.Add(point);
            }
            clusters.Add(cluster1);
            clusters.Add(cluster2);
            return clusters;
        }


        public List<Point> Data { get; }
        public List<Point> Centers { get; }

    }
}
