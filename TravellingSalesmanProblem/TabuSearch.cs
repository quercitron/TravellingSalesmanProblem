using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Temp;

namespace TravellingSalesmanProblem
{
    public class TabuSearch : ISalesmanProblemSolver
    {
        public int[] GetPath(int n, IMeasure measure)
        {
            //var p = Permutations.GetRandomPermutation(n);
            var p = new Greedy2().GetPath(n, measure);
            //p = new[] {1, 3, 2, 0};
            var path = new PathNode[n];
            for (int i = 0; i < n - 1; i++)
            {
                path[p[i]].Next = p[i + 1];
            }
            for (int i = 1; i < n; i++)
            {
                path[p[i]].Prev = p[i - 1];
            }
            path[p[0]].Prev = p[n - 1];
            path[p[n - 1]].Next = p[0];

            double l = 0;
            var bestPath = new int[n];
            var t = 0;
            var count = 0;
            while (true)
            {
                bestPath[count] = t;
                count++;

                l += measure[t, path[t].Next];
                t = path[t].Next;
                if (t == 0)
                {
                    break;
                }
            }
            var bestL = l;

            var rnd = new Random();

            var T = 40;
            var tabu = new int[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    tabu[i, j] = int.MinValue;
                }
            }

            var stopwatch = Stopwatch.StartNew();

            var maxCycles = 100000;
            var timelimit = 1;
            for (int cycle = 0; ; cycle++)
            {
                if (cycle % 10000 == 0)
                {
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds > timelimit * 1000)
                    {
                        break;
                    }
                    stopwatch.Start();
                }

                var bestx = 0;
                var besty = 0;
                double bestDelta = -1e10;

                int xprev;
                int xnext;
                int yprev;
                int ynext;
                for (int k = 0; k < n * 10; k++)
                {
                    var x = rnd.Next(n);
                    var y = rnd.Next(n - 1);
                    //x = 1;
                    //y = 0;
                    if (y >= x)
                        y++;

                    xprev = path[x].Prev != y ? path[x].Prev : x;
                    xnext = path[x].Next != y ? path[x].Next : x;
                    yprev = path[y].Prev != x ? path[y].Prev : y;
                    ynext = path[y].Next != x ? path[y].Next : y;
                    var delta = measure[path[x].Prev, x] + measure[x, path[x].Next] + measure[path[y].Prev, y] + measure[y, path[y].Next] -
                               (measure[xprev, y] + measure[y, xnext] + measure[yprev, x] + measure[x, ynext]);

                    if (l - delta > bestL)
                    {
                        if (tabu[x, y] + T >= cycle)
                        {
                            continue;
                        }
                    }

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

                l -= bestDelta;
                xprev = path[bestx].Prev;// != besty ? path[bestx].Prev : bestx;
                xnext = path[bestx].Next;// != besty ? path[bestx].Next : bestx;
                yprev = path[besty].Prev;// != bestx ? path[besty].Prev : besty;
                ynext = path[besty].Next;// != bestx ? path[besty].Next : besty;
                path[xprev].Next = besty;
                path[xnext].Prev = besty;
                path[yprev].Next = bestx;
                path[ynext].Prev = bestx;
                var tmp = path[bestx].Prev;
                path[bestx].Prev = path[besty].Prev;
                path[besty].Prev = tmp;
                tmp = path[bestx].Next;
                path[bestx].Next = path[besty].Next;
                path[besty].Next = tmp;

                tabu[besty, bestx] = cycle;

                if (l < bestL)
                {
                    bestL = l;

                    t = 0;
                    count = 0;
                    while (true)
                    {
                        bestPath[count] = t;
                        count++;

                        t = path[t].Next;
                        if (t == 0)
                        {
                            break;
                        }
                    }
                }
            }

            return bestPath;
        }
    }

    public struct PathNode
    {
        public int Prev { get; set; }
        public int Next { get; set; }

        public override string ToString()
        {
            return string.Format("Prev: {0}, Next: {1}", Prev, Next);
        }
    }
}
