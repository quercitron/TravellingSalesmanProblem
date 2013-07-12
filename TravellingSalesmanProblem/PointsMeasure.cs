using System.Collections.Generic;

using Temp;

namespace TravellingSalesmanProblem
{
    public class PointsMeasure : IMeasure
    {
        private readonly IList<Point2DReal> _points;

        public PointsMeasure(IList<Point2DReal> points)
        {
            _points = points;
        }

        public double this[int i, int j]
        {
            get { return _points[i].Dist(_points[j]); }
        }

        public double[,] GetMatrix()
        {
            var n = _points.Count;
            var m = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    m[i, j] = _points[i].Dist(_points[j]);
                }
            }
            return m;
        }
    }
}
