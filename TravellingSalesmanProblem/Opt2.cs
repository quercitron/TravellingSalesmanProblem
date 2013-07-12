using System.Diagnostics;
using Temp;

namespace TravellingSalesmanProblem
{
    public class Opt2 : SalesmanProblemBase
    {
        public override int[] GetPath(int n, IMeasure measure)
        {
            var stopwatch = Stopwatch.StartNew();
            var cycle = 0;
            var timelimit = 290 * 60;
            var timeout = false;
            int[] bestPath = null;
            var bestLength = 1e30;

            while (!timeout)
            {
                // Generate random path
                //var p = new[] {1, 3, 2, 0};
                var p = Permutations.GetRandomPermutation(n);
                var path = new Node[n];
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

                    while (true)
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

                        jpr = i;
                        j = path[i].Next(ipr);
                        jnext = path[j].Next(jpr);
                        while (j != i)
                        {
                            if (inext != j && jnext != i)
                            {
                                if (measure[i, inext] + measure[j, jnext] > measure[i, j] + measure[inext, jnext])
                                {
                                    found = true;
                                    break;
                                }
                            }

                            jpr = j;
                            j = jnext;
                            jnext = path[j].Next(jpr);
                        }

                        if (found)
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
                    path[i].Change(inext, j);
                    path[j].Change(jnext, i);
                    path[inext].Change(i, jnext);
                    path[jnext].Change(j, inext);

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

    public struct Node
    {
        public override string ToString()
        {
            return string.Format("Left: {0}, Right: {1}", Left, Right);
        }

        public int Left { get; set; }
        public int Right { get; set; }

        public int Next(int prev)
        {
            return prev == Left ? Right : Left;
        }

        public void Change(int i, int j)
        {
            if (Left == i)
                Left = j;
            else
                Right = j;
        }
    }
}
