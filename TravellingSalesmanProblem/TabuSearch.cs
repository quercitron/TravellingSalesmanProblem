using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Temp;

namespace TravellingSalesmanProblem
{
    public class TabuSearch : ISalesmanProblemSolver
    {
        public int[] GetPath(int n, double[,] matrix)
        {
            var p = Permutations.GetRandomPermutation(n);
            var path = new PathNode[n];
            for (int i = 0; i < n - 1; i++)
            {
                path[p[i]].Next = p[i + 1];
            }
            for (int i = 1; i < n; i++)
            {
                path[p[i]].Next = p[i + 1];
            }
            path[p[0]].Prev = p[n - 1];
            path[p[n - 1]].Next = p[0];

            double bestL = 0;
            var bestPath = new int[n];
            var t = 0;
            var count = 0;
            while (true)
            {
                bestPath[count] = t;
                count++;

                bestL += matrix[t, path[t].Next];
                t = path[t].Next;
                if (t == 0)
                {
                    break;
                }
            }

            var rnd = new Random();

            for (int g = 0; g < 100; g++)
            {
                var bestx = 0;
                var besty = 0;
                double bestDelta = 1e-10;

                for (int k = 0; k < 100; k++)
                {
                    var x = rnd.Next(n);
                    var y = rnd.Next(n - 1);
                    if (y >= x)
                        y++;

                    var delta = matrix[path[x].Prev, y] + matrix[y, path[x].Next] + matrix[path[y].Prev, x] + matrix[x, path[y].Next] -
                               (matrix[path[x].Prev, x] + matrix[x, path[x].Next] + matrix[path[y].Prev, y] + matrix[y, path[y].Next]);
                    if (delta > bestDelta)
                    {
                        bestDelta = delta;
                        bestx = x;
                        besty = y;
                    }

                    if (delta > 0)
                    {
                        break;
                    }
                }
            }
        }
    }

    public struct PathNode
    {
        public int Prev { get; set; }
        public int Next { get; set; }
    }
}
