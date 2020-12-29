namespace NPuzzle
{
    public class SearchResult
    {
        public SearchNode GoalNode { get; set; }
        public bool IsGoal => GoalNode != null;
        public int NewThreshold;
        public bool IsNotFound => NewThreshold == int.MaxValue;
    }
}
