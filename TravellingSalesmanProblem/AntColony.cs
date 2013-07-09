using System;
using System.Diagnostics;
using System.Threading;

namespace TravellingSalesmanProblem
{
    public class AntColony : ISalesmanProblemSolver
    {
        private readonly Random Rnd = new Random();

        public int[] GetPath(int n, double[,] matrix)
        {
            var vis = new double[n,n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i != j)
                    {
                        vis[i, j] = 1 / matrix[i, j];
                    }
                }
            }

            var cstart = 1000;
            var Q = 1000;

            var bestL = 1e30;
            int[] solution = new int[n];
            var trail = new double[n,n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    trail[i, j] = cstart;
                }
            }

            var m = n;
            var pr = new double[n,n];

            var alpha = 1;
            var beta = 4;

            var rho = 0.5;

            // var stopwatch = Stopwatch.StartNew();

            var threadCount = 4;

            var maxSteps = 3000;
            for (int g = 0; g < maxSteps; g++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        pr[i, j] = Math.Pow(trail[i, j], alpha) * Math.Pow(vis[i, j], beta);
                        /*if (pr[i, j] < 1e-25)
                        {
                            pr[i, j] = 0;
                        }*/
                    }
                }

                var path = new int[m,n];

                var threads = new Thread[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    var left = (i * m) / threadCount;
                    var right = ((i + 1) * m) / threadCount;
                    var thread = new Thread(() => RunSearch(left, right, n, pr, path));
                    threads[i] = thread;
                    thread.Start();
                }

                for (int i = 0; i < threadCount; i++)
                {
                    threads[i].Join();
                }

                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        trail[i, j] *= rho;
                    }
                }

                double delta;
                for (int k = 0; k < m; k++)
                {
                    var l = matrix[path[k, n - 1], path[k, 0]];
                    for (int i = 0; i < n - 1; i++)
                    {
                        l += matrix[path[k, i], path[k, i + 1]];
                    }

                    if (bestL > l)
                    {
                        bestL = l;
                        for (int i = 0; i < n; i++)
                        {
                            solution[i] = path[k, i];
                        }
                    }

                    delta = Q / l;
                    trail[path[k, n - 1], path[k, 0]] += delta;
                    for (int i = 0; i < n - 1; i++)
                    {
                        trail[path[k, i], path[k, i + 1]] += delta;
                    }
                }

                // elitist strategy
                var e = n / 10.0;
                delta = Q / bestL * e;
                trail[solution[n - 1], solution[0]] += delta;
                for (int i = 0; i < n - 1; i++)
                {
                    trail[solution[i], solution[i + 1]] += delta;
                }
            }

            return solution;
        }

        private void RunSearch(int left, int right, int n, double[,] pr, int[,] path)
        {
            var prSum = new double[n];
            for (int k = left; k < right; k++)
            {
                var tabu = new bool[n];
                for (int i = 0; i < n; i++)
                {
                    prSum[i] = 0;
                    for (int j = 0; j < n; j++)
                    {
                        prSum[i] += pr[i, j];
                    }
                }

                var st = Rnd.Next(n);
                path[k, 0] = st;
                tabu[st] = true;
                for (int i = 0; i < n; i++)
                {
                    prSum[i] -= pr[i, st];
                }

                for (int t = 0; t < n - 1; t++)
                {
                    var cur = path[k, t];
                    var r = Rnd.NextDouble() * prSum[cur];
                    var next = 0;
                    for (int i = 0; i < n; i++)
                    {
                        if (tabu[i])
                        {
                            continue;
                        }
                        next = i;
                        if (r <= pr[cur, i])
                        {
                            next = i;
                            break;
                        }
                        r -= pr[cur, i];
                    }

                    path[k, t + 1] = next;
                    tabu[next] = true;
                    for (int i = 0; i < n; i++)
                    {
                        prSum[i] -= pr[i, next];
                    }
                }
            }
        }
    }
}
