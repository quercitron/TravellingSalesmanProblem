using System;
using System.Collections.Generic;
using System.Linq;
using Temp;

namespace TravellingSalesmanProblem
{
    public class LittleAlgorithm
    {
        private readonly double Inf = 2e9;
        private readonly double InfBound = 1e9;
        private readonly double Eps = 1e-2;

        public int[] GetPath(int n, double[,] matrix)
        {
            var baseState = new GenerationState
                {
                    C = new double[n,n],
                    Edges = new List<Edge>(n),
                    N = n,
                    VertexSet = new int[n]
                };
            Array.Copy(matrix, baseState.C, matrix.Length);
            for (int i = 0; i < n; i++)
            {
                baseState.C[i, i] = Inf;
            }
            baseState.J = ReductMatrix(n, baseState.C);
            for (int i = 0; i < n; i++)
            {
                baseState.VertexSet[i] = i;
            }

            var queue = new PriorityQueue<GenerationState>((a, b) => -a.J.CompareTo(b.J));
            queue.Enqueue(baseState);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (current.Edges.Count == n - 1)
                {
                    var result = GetResult(n, current);
                    return result;
                }

                Process(current, queue);
            }

            throw new ApplicationException("Path not found");
        }

        private static int[] GetResult(int n, GenerationState state)
        {
            var edges = state.Edges;
            int[] c = new int[n];
            foreach (var edge in edges)
            {
                c[edge.From]++;
                c[edge.To]--;
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
            result[0] = s;
            for (int i = 0; i < n - 1; i++)
            {
                foreach (var edge in edges)
                {
                    if (edge.From == s)
                    {
                        result[i + 1] = edge.To;
                        s = edge.To;
                        break;
                    }
                }
            }
            return result;
        }

        private void Process(GenerationState state, PriorityQueue<GenerationState> queue)
        {
            //state.J = ReductMatrix(state.N, state.C);

            int bestI = 0, bestJ = 0;
            double bestMax = -1;
            int n = state.N;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (state.C[i, j] < Eps)
                    {
                        double m = GetMinSum(n, state.C, i, j);
                        if (bestMax < m)
                        {
                            bestMax = m;
                            bestI = i;
                            bestJ = j;
                        }
                    }
                }
            }

            if (state.C[bestI, bestJ] > InfBound)
            {
                return;
            }

            // get state 1
            var stateWithoutEdge = new GenerationState
                {
                    N = state.N,
                    C = new double[n,n],
                    Edges = state.Edges,
                    VertexSet = state.VertexSet
                };
            Array.Copy(state.C, stateWithoutEdge.C, state.C.Length);
            stateWithoutEdge.C[bestI, bestJ] = Inf; // TODO: Remove j -> i ??
            double r2 = ReductMatrix(n, stateWithoutEdge.C);
            stateWithoutEdge.J = state.J + r2;


            // get state 2
            var stateWithEdge = new GenerationState
            {
                N = state.N,
                C = new double[n, n],
                Edges = new List<Edge>(state.Edges),
                VertexSet = new int[n]
            };

            Array.Copy(state.C, stateWithEdge.C, state.C.Length);
            for (int i = 0; i < n; i++)
            {
                stateWithEdge.C[i, bestJ] = Inf;
            }
            for (int j = 0; j < n; j++)
            {
                stateWithEdge.C[bestI, j] = Inf;
            }
            stateWithEdge.C[bestJ, bestI] = Inf;

            Array.Copy(state.VertexSet, stateWithEdge.VertexSet, n);
            int c1 = stateWithEdge.VertexSet[bestI];
            int c2 = stateWithEdge.VertexSet[bestJ];
            for (int i = 0; i < n; i++)
            {
                if (stateWithEdge.VertexSet[i] == c1)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (stateWithEdge.VertexSet[j] == c2)
                        {
                            stateWithEdge.C[i, j] = Inf;
                            stateWithEdge.C[j, i] = Inf;
                        }
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                if (stateWithEdge.VertexSet[i] == c2)
                {
                    stateWithEdge.VertexSet[i] = c1;
                }
            }

            double r1 = ReductMatrix(n, stateWithEdge.C);
            stateWithEdge.J = state.J + r1;
            stateWithEdge.Edges.Add(new Edge(bestI, bestJ));

            queue.Enqueue(stateWithEdge);
            queue.Enqueue(stateWithoutEdge);

            return;
        }

        private double GetMinSum(int n, double[,] matrix, int x, int y)
        {
            double columnMin = Inf;
            for (int i = 0; i < n; i++)
            {
                if (i != x)
                {
                    if (columnMin > matrix[i, y])
                    {
                        columnMin = matrix[i, y];
                    }
                }
            }
            
            double rowMin = Inf;
            for (int j = 0; j < n; j++)
            {
                if (j != y)
                {
                    if (rowMin > matrix[x, j])
                    {
                        rowMin = matrix[x, j];
                    }
                }
            }

            return columnMin + rowMin;
        }

        private double ReductMatrix(int n, double[,] matrix)
        {
            double result = 0;

            var a = GetRowMin(n, matrix);
            result += a.Sum();

            var b = GetColumnMin(n, matrix);
            result += b.Sum();

            return result;
        }

        private double[] GetColumnMin(int n, double[,] matrix)
        {
            double[] b = new double[n];
            for (int i = 0; i < n; i++)
            {
                b[i] = Inf;
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] < b[j])
                    {
                        b[j] = matrix[i, j];
                    }
                }
            }

            for (int j = 0; j < n; j++)
            {
                if (b[j] > InfBound)
                {
                    b[j] = 0;
                }
            }

            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < n; i++)
                {

                    matrix[i, j] -= b[j];
                }
            }
            return b;
        }

        private double[] GetRowMin(int n, double[,] matrix)
        {
            double[] a = new double[n];
            for (int i = 0; i < n; i++)
            {
                a[i] = Inf;
            }
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] < a[i])
                    {
                        a[i] = matrix[i, j];
                    }
                }
            }

            for (int i = 0; i < n; i++)
            {
                if (a[i] > InfBound)
                {
                    a[i] = 0;
                }
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] -= a[i];
                }
            }
            return a;
        }
    }
}
