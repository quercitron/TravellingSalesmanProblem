using System.Collections.Generic;

namespace TravellingSalesmanProblem
{
    internal class GenerationState
    {
        public int N { get; set; }
        public double[,] C { get; set; }
        public double J { get; set; }
        public List<Edge> Edges { set; get; }
        public int[] VertexSet { get; set; }
    }
}
