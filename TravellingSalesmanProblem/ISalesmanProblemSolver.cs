namespace TravellingSalesmanProblem
{
    public interface ISalesmanProblemSolver
    {
        int[] GetPath(int n, IMeasure measure);
    }
}