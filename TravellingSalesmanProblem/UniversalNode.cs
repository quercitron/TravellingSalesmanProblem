namespace TravellingSalesmanProblem
{
    public struct UniversalNode
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