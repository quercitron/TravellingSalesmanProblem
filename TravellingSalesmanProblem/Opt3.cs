using System.Diagnostics;
using Temp;

namespace TravellingSalesmanProblem
{
    public class Opt3 : SalesmanProblemBase
    {
        public override int[] GetPath(int n, IMeasure measure)
        {
            var stopwatch = Stopwatch.StartNew();
            var cycle = 0;
            var timelimit = RunProperties.RunTimeInSeconds;
            var timeout = false;
            int[] bestPath = null;
            var bestLength = 1e30;

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

                var pst = 0;
                var st = path[0].Left;

                while (true)
                {
                    var found = false;

                    var i = st;
                    var inext = path[i].Right;
                    var ipr = path[i].Next(inext);

                    int jpr = 0;
                    int j = 0;
                    int jnext = 0;

                    int kpr = 0;
                    int k = -1;
                    int knext = 0;
                    int trType = -1;

                    while (true)
                    {
                        jpr = i;
                        j = path[i].Next(ipr);
                        jnext = path[j].Next(jpr);
                        while (j != i)
                        {
                            cycle++;
                            if (cycle == 10000)
                            {
                                stopwatch.Stop();
                                if (stopwatch.ElapsedMilliseconds > timelimit * 1000)
                                {
                                    timeout = true;
                                    break;
                                }
                                cycle = 0;
                                stopwatch.Start();
                            }

                            // run 2-opt
                            if (inext != j && jnext != i)
                            {
                                if (measure[i, inext] + measure[j, jnext] > measure[i, j] + measure[inext, jnext])
                                {
                                    found = true;
                                    break;
                                }
                            }

                            // run 3-opt
                            kpr = j;
                            k = path[j].Next(jpr);
                            knext = path[k].Next(kpr);

                            while (k != i)
                            {
                                // TODO: need more condition?
                                if (inext != j && jnext != k && knext != i)
                                {
                                    if (measure[i, inext] + measure[j, jnext] + measure[k, knext] > measure[i, jnext] + measure[j, knext] + measure[k, inext])
                                    {
                                        trType = 0;
                                        found = true;
                                        break;
                                    }
                                    if (measure[i, inext] + measure[j, jnext] + measure[k, knext] > measure[i, jnext] + measure[j, k] + measure[inext, knext])
                                    {
                                        trType = 1;
                                        found = true;
                                        break;
                                    }
                                }

                                // iterate
                                kpr = k;
                                k = knext;
                                knext = path[k].Next(kpr);
                            }

                            if (found)
                            {
                                break;
                            }
                            k = -1;

                            // iterate
                            jpr = j;
                            j = jnext;
                            jnext = path[j].Next(jpr);
                        }

                        if (timeout || found)
                        {
                            break;
                        }

                        ipr = i;
                        i = inext;
                        inext = path[i].Next(ipr);

                        if (i == st)
                        {
                            break;
                        }
                    }

                    if (!found)
                    {
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
                            path[i].Change(inext, jnext);
                            path[j].Change(jnext, k);
                            path[k].Change(knext, j);
                            path[inext].Change(i, knext);
                            path[jnext].Change(j, i);
                            path[knext].Change(k, inext);
                        }
                    }


                    // heuristic shift
                    var sttmp = st;
                    st = path[st].Next(pst);
                    pst = sttmp;
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

                var l = CalcLength(p, measure);
                if (l < bestLength)
                {
                    bestLength = l;
                    bestPath = p;
                }
            }

            return bestPath;
        }
    }
}
