namespace TravellingSalesmanProblem
{
    public class SimpleGreedy : ISalesmanProblemSolver
    {
        public int[] GetPath(int n, IMeasure measure)
        {
            var result = new int[n];
            result[0] = 0;
            var v = new bool[n];
            v[0] = true;
            for (int k = 1; k < n; k++)
            {
                var prev = result[k - 1];
                int next = -1;
                double bestCost = 0;
                for (int i = 0; i < n; i++)
                {
                    if (!v[i])
                    {
                        if (next == -1 || bestCost > measure[prev, i])
                        {
                            next = i;
                            bestCost = measure[prev, i];
                        }
                    }
                }

                result[k] = next;
                v[next] = true;
            }
            return result;
        }
    }
}
