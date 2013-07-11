using System.Collections.Generic;

using Temp;

namespace TravellingSalesmanProblem
{
    public class MatrixMeasureFactory
    {
        public MatrixMeasure CreateMatrixMeasure(IList<Point2DReal> points)
        {
            var n = points.Count;
            var m = new double[n * n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    m[i * n + j] = points[i].Dist(points[j]);
                }
            }

            return new MatrixMeasure(n, m);
        }
    }
}
