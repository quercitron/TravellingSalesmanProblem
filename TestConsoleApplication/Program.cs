using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using Temp;
using TravellingSalesmanProblem;

namespace TestConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            var lines = args[0].Split('\n');
            var n = int.Parse(lines[0]);
            var points = new Point2DReal[n];
            for (int i = 0; i < n; i++)
            {
                var line = lines[i + 1].Split();
                var x = double.Parse(line[0]);
                var y = double.Parse(line[1]);
                points[i] = new Point2DReal(x, y);
            }
            /*var matrix = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = points[i].Dist(points[j]);
                }
            }

            double avgDist = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    avgDist += matrix[i, j];
                }
            }
            avgDist /= (n * n);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] /= avgDist;
                }
            }*/

            var measure = new MatrixMeasureFactory().CreateMatrixMeasure(points);

            var path = new TabuSearch().GetPath(n, measure);

            var ans = points[path[0]].Dist(points[path[n - 1]]);
            for (int i = 0; i < n - 1; i++)
            {
                ans += points[path[i]].Dist(points[path[i + 1]]);
            }

            Console.WriteLine("{0} 0", ans);
            Console.WriteLine(string.Join(" ", path));
        }

        private static void SquareTest()
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
                var measure = new MatrixMeasureFactory().CreateMatrixMeasure(points);
                var path = algorithm.GetPath(n, measure);
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
