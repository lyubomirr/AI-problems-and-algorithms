namespace NPuzzle
{
    public static class DirectionExtensions
    {
        public static bool IsOpposite(this Direction direction, Direction other)
        {
            return (direction == Direction.Up && other == Direction.Down)
                || (direction == Direction.Down && other == Direction.Up)
                || (direction == Direction.Left && other == Direction.Right)
                || (direction == Direction.Right && other == Direction.Left);
        }
    }
}
