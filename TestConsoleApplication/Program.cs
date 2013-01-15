using System;
using System.Collections.Generic;
using Temp;
using TravellingSalesmanProblem;

namespace TestConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            int n = 8;
            double[,] m = new double[n,n];
            /*for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    m[i, j] = 1;
                }
            }*/

            var points = new List<Point2DReal>
                {
                    new Point2DReal(0, 0),
                    new Point2DReal(2, 2),
                    new Point2DReal(2, 0),
                    new Point2DReal(0, 2),
                    new Point2DReal(1, 2),
                    new Point2DReal(0, 1),
                    new Point2DReal(1, 0),
                    new Point2DReal(2, 1),
                };

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    m[i, j] = points[i].Dist(points[j]);
                }
            }

            try
            {
                var algorithm = new LittleAlgorithm();
                var path = algorithm.GetPath(n, m);
                for (int i = 0; i < n; i++)
                {
                    Console.Write("{0} ", path[i]);
                }
                Console.WriteLine();
                for (int i = 0; i < n; i++)
                {
                    Console.WriteLine("{0} {1}", points[path[i]].X, points[path[i]].Y);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
