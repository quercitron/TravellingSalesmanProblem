namespace TravellingSalesmanProblem
{
    public interface IMeasure
    {
        double this[int i, int j] { get; }

        double[,] GetMatrix();
    }
}
