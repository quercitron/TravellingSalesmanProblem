using System;
using System.Diagnostics;
using System.Threading;
using Temp;

namespace TravellingSalesmanProblem
{
    public class Opt3 : SalesmanProblemBase
    {
        private int totalCount;
        private bool[,] S;
        private int cycle;
        private bool timeout;
        private int[] bestPath;
        private double bestLength;
        private Stopwatch stopwatch;
        private int timelimit;

        private readonly object _lockTotalCount = new object();
        private readonly object _lockUpdateBest = new object();
        private readonly object _lockStopwatch = new object();

        private readonly int ThreadCount = 3;

        public override int[] GetPath(int n, IMeasure measure)
        {
            stopwatch = Stopwatch.StartNew();
            timelimit = (int)(RunProperties.RunTimeInSeconds * 1000);
            cycle = 0;
            timeout = false;
            bestPath = null;
            bestLength = 1e30;
            totalCount = 0;
            S = new bool[n, n];

            var threads = new Thread[ThreadCount];
            for (int t = 0; t < ThreadCount; t++)
            {
                threads[t] = new Thread(() => RunSearch(n, measure));
            }
            for (int i = 0; i < ThreadCount; i++)
            {
                threads[i].Start();
            }
            for (int i = 0; i < ThreadCount; i++)
            {
                threads[i].Join();
            }

            return bestPath;
        }

        private void RunSearch(int n, IMeasure measure)
        {
            var rnd = new Random();

            while (!timeout)
            {
                // Generate random path
                //var p = new[] {1, 3, 2, 0};
                var p = Permutations.GetRandomPermutation(n);
                var path = new UniversalNode[n];
                for (int i = 0; i < n - 1; i++)
                {
                    path[p[i]].Right = p[i + 1];
                }
                for (int i = 1; i < n; i++)
                {
                    path[p[i]].Left = p[i - 1];
                }
                path[p[0]].Left = p[n - 1];
                path[p[n - 1]].Right = p[0];

                /*var pst = 0;
                var st = path[0].Left;*/

                int q = totalCount > 0 ? 1 : 0;
                while (true)
                {
                    // init
                    var i = rnd.Next(n);
                    var inext = path[i].Left;
                    var ipr = path[i].Next(inext);

                    int jpr = 0;
                    int j = 0;
                    int jnext = 0;

                    int kpr = 0;
                    int k = 0;
                    int knext = 0;

                    int trType = -1;
                    var found = false;

                    for (int icount = 0; icount < n; icount++)
                    {
                        // iterate
                        ipr = i;
                        i = inext;
                        inext = path[i].Next(ipr);

                        if (q == 1 && S[i, inext])
                        {
                            continue;
                        }

                        // init
                        j = inext;
                        jnext = i;
                        jpr = path[j].Next(jnext);

                        for (int jcount = 0; jcount < n - 3; jcount++)
                        {
                            // iterate
                            jpr = j;
                            j = jnext;
                            jnext = path[j].Next(jpr);

                            cycle++;
                            if (cycle >= ThreadCount * 10000)
                            {
                                lock (_lockStopwatch)
                                {
                                    stopwatch.Stop();
                                    if (stopwatch.ElapsedMilliseconds > timelimit)
                                    {
                                        timeout = true;
                                        break;
                                    }
                                    cycle = 0;
                                    stopwatch.Start();
                                }
                            }

                            /*// run 2-opt
                            if (inext != j && jnext != i)
                            {
                                if (measure[i, inext] + measure[j, jnext] > measure[i, j] + measure[inext, jnext])
                                {
                                    found = true;
                                    break;
                                }
                            }*/

                            // run 3-opt

                            // init
                            kpr = jpr;
                            k = j;
                            knext = jnext;

                            for (int kcount = 0; kcount < n - 3 - jcount; kcount++)
                            {
                                // iterate
                                kpr = k;
                                k = knext;
                                knext = path[k].Next(kpr);

                                double d;
                                if (measure[inext, k] + measure[i, jnext] <= measure[inext, jnext] + measure[i, k])
                                {
                                    d = measure[inext, k] + measure[i, jnext];
                                    trType = 0;
                                }
                                else
                                {
                                    d = measure[inext, jnext] + measure[i, k];
                                    trType = 1;
                                }

                                if (d + measure[j, knext] < measure[i, inext] + measure[j, jnext] + measure[k, knext])
                                {
                                    found = true;
                                    break;
                                }

                                // TODO: need more condition?
                                /*if (inext != j && jnext != k && knext != i)
                                {
                                    if (measure[i, inext] + measure[j, jnext] + measure[k, knext]
                                        > measure[i, jnext] + measure[j, knext] + measure[k, inext])
                                    {
                                        trType = 0;
                                        found = true;
                                        break;
                                    }
                                    if (measure[i, inext] + measure[j, jnext] + measure[k, knext]
                                        > measure[i, jnext] + measure[j, k] + measure[inext, knext])
                                    {
                                        trType = 1;
                                        found = true;
                                        break;
                                    }
                                }*/
                            }

                            /*if (found)
                            {
                                break;
                            }
                            k = -1;*/

                            if (timeout || found)
                            {
                                break;
                            }

                            /*if (i == st)
                            {
                                break;
                            }*/
                        }

                        if (timeout || found)
                        {
                            break;
                        }
                    }

                    if (!found)
                    {
                        if (q == 1)
                        {
                            q = 0;
                            continue;
                        }
                        break;
                    }

                    //var inext2 = path[inext].Next(i);
                    //var jprev = path[j].Next(jnext);
                    if (k < 0)
                    {
                        path[i].Change(inext, j);
                        path[j].Change(jnext, i);
                        path[inext].Change(i, jnext);
                        path[jnext].Change(j, inext);
                    }
                    else
                    {
                        if (trType == 0)
                        {
                            path[i].Change(inext, jnext);
                            path[j].Change(jnext, knext);
                            path[k].Change(knext, inext);
                            path[inext].Change(i, k);
                            path[jnext].Change(j, i);
                            path[knext].Change(k, j);
                        }
                        else
                        {
                            path[i].Change(inext, k);
                            path[j].Change(jnext, knext);
                            path[k].Change(knext, i);
                            path[inext].Change(i, jnext);
                            path[jnext].Change(j, inext);
                            path[knext].Change(k, j);
                        }
                    }


                    // heuristic shift
                    /*var sttmp = st;
                    st = path[st].Next(pst);
                    pst = sttmp;*/
                }

                var tpr = 0;
                var t = path[0].Right;
                p[0] = 0;
                var count = 1;
                while (t != 0)
                {
                    p[count] = t;
                    count++;
                    var tmp = t;
                    t = path[t].Next(tpr);
                    tpr = tmp;
                }

                S[p[0], p[n - 1]] = true;
                S[p[n - 1], p[0]] = true;
                for (int i = 0; i < n - 1; i++)
                {
                    S[p[i], p[i + 1]] = true;
                    S[p[i + 1], p[i]] = true;
                }

                var l = CalcLength(p, measure);
                lock (_lockUpdateBest)
                {
                    if (l < bestLength)
                    {
                        bestLength = l;
                        bestPath = p;
                    }
                }

                lock (_lockTotalCount)
                {
                    totalCount++;
                }
            }
        }
    }
}
