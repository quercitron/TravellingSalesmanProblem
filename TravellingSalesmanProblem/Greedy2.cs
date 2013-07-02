using System.Collections.Generic;
using System.Linq;
using Temp;

namespace TravellingSalesmanProblem
{
    public class Greedy2 : SalesmanProblemBase
    {
        public override int[] GetPath(int n, double[,] matrix)
        {
            var edges = new List<Edge>((n * (n - 1)) % 2);
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    edges.Add(new Edge(i, j));
                }
            }

            var set = new DisjointSetUnion<int>();
            for (int i = 0; i < n; i++)
            {
                set.MakeSet(i);
            }

            var result = new List<Edge>(n - 1);
            var r = new int[n];

            foreach (var edge in edges.OrderBy(a => matrix[a.From, a.To]))
            {
                if (r[edge.From] < 2 && r[edge.To] < 2 && set.FindSet(edge.From) != set.FindSet(edge.To))
                {
                    result.Add(edge);
                    set.UnionSets(edge.From, edge.To);
                    r[edge.From]++;
                    r[edge.To]++;
                }
            }

            return GetResult(n, result);
        }
    }
}
