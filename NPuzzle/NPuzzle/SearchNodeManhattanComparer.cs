using System.Collections.Generic;

namespace NPuzzle
{
    public class SearchNodeManhattanComparer : IComparer<SearchNode>
    {
        public int Compare(SearchNode x, SearchNode y)
        {
            return x.GetTotalCost() - y.GetTotalCost();
        }
    }
}
