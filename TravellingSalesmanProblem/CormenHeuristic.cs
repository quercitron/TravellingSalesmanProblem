using System;
using System.Collections.Generic;

namespace TravellingSalesmanProblem
{
    public class CormenHeuristic : SalesmanProblemBase
    {
        public override int[] GetPath(int n, IMeasure measure)
        {
            var e = new List<int>[n];
            for (int i = 0; i < n; i++)
            {
                e[i] = new List<int>();
            }

            var d = new double[n];
            for (int i = 0; i < n; i++)
            {
                d[i] = double.MaxValue;
            }

            var s = new Random().Next(n);
            d[s] = 0;
            var v = new bool[n];
            var p = new int[n];
            for (int i = 0; i < n; i++)
            {
                p[i] = -1;
            }

            for (int k = 0; k < n; k++)
            {
                var mini = -1;
                for (int i = 0; i < n; i++)
                {
                    if (!v[i] && (mini == -1 || d[i] < d[mini]))
                    {
                        mini = i;
                    }
                }

                if (p[mini] != -1)
                {
                    e[mini].Add(p[mini]);
                    e[p[mini]].Add(mini);
                }

                v[mini] = true;
                for (int i = 0; i < n; i++)
                {
                    if (!v[i] && d[i] > measure[mini, i])
                    {
                        d[i] = measure[mini, i];
                        p[i] = mini;
                    }
                }
            }

            v = new bool[n];
            var path = new List<int>();
            BuildPath(s, e, v, path);

            return path.ToArray();
        }

        private void BuildPath(int t, IList<List<int>> e, IList<bool> v, ICollection<int> path)
        {
            v[t] = true;
            path.Add(t);

            foreach (var i in e[t])
            {
                if (!v[i])
                {
                    BuildPath(i, e, v, path);
                }
            }
        }
    }
}
