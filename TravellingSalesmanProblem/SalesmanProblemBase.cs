using System.Collections.Generic;

namespace TravellingSalesmanProblem
{
    public abstract class SalesmanProblemBase : ISalesmanProblemSolver
    {
        public abstract int[] GetPath(int n, IMeasure measure);

        protected static double CalcLength(int[] path, IMeasure measure)
        {
            var n = path.Length;
            var length = measure[path[n - 1], path[0]];
            for (int i = 0; i < n - 1; i++)
            {
                length += measure[path[i], path[i + 1]];
            }
            return length;
        }

        protected static int[] GetResult(int n, List<Edge> edges)
        {
            int[] c = new int[n];
            foreach (var edge in edges)
            {
                c[edge.From]++;
                c[edge.To]++;
            }

            int s = 0;
            for (int i = 0; i < n; i++)
            {
                if (c[i] == 1)
                {
                    s = i;
                    break;
                }
            }

            int[] result = new int[n];
            bool[] u = new bool[n];
            result[0] = s;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1; j++)
                {
                    if (!u[j])
                    {
                        if (edges[j].From == s)
                        {
                            result[i + 1] = edges[j].To;
                            s = edges[j].To;
                            u[j] = true;
                            break;
                        }
                        if (edges[j].To == s)
                        {
                            result[i + 1] = edges[j].From;
                            s = edges[j].From;
                            u[j] = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}