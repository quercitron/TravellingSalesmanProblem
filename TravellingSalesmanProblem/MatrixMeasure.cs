namespace TravellingSalesmanProblem
{
    public class MatrixMeasure : IMeasure
    {
        private readonly int _n;

        private readonly double[] _matrix;

        public MatrixMeasure(int n, double[] matrix)
        {
            _n = n;
            _matrix = matrix;
        }

        public double this[int i, int j]
        {
            get { return _matrix[_n * i + j]; }
        }

        public double[,] GetMatrix()
        {
            var m = new double[_n,_n];
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    m[i, j] = _matrix[_n * i + j];
                }
            }
            return m;
        }
    }
}
