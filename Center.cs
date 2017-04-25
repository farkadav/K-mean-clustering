using System;
using System.Collections.Generic;

namespace kMeanClust
{
    public class Center 
    {
       
        public Center(List<Point> data )
        {
            Data = data;
        }

        public List<Point> Centers()
        {
            int K = 2;
            double maxX = 0;
            double maxY = 0;
            double minX = Data[0].X;
            double minY = Data[0].Y;
            foreach (var point in Data)
            {
                if (point.X > maxX) maxX = point.X;
                if (point.Y > maxY) maxY = point.Y;
                if (point.X < minX) minX = point.X;
                if (point.Y < minY) minY = point.Y;

            }
            List<Point> centers = new List<Point>();
            Random k1 = new Random();
            for (int i = 0; i < K; i++)
            {
                double c1 = k1.NextDouble() * maxX - minX;
                double c2 = k1.NextDouble() * maxY - minY;
                centers.Add(new Point(c1, c2));
            }
            return centers;
        }
     
        public List<Point> Data { get; private set; }
    }

    
}
