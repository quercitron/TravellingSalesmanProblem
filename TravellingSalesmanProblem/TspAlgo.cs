using System.Drawing;

namespace TravellingSalesmanProblem
{
    public class TspAlgo
    {
        public string Name { get; set; }

        public ISalesmanProblemSolver Solver { get; set; }

        public bool IsActive { get; set; }

        public double Length { get; set; }

        public int[] Path { get; set; }

        public Color Color { get; set; }
    }
}
