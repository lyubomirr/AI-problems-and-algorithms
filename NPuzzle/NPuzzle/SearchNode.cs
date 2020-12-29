using System.Collections.Generic;

namespace NPuzzle
{
    public class SearchNode
    {
        public Direction Direction { get; set; }
        public BoardState State { get; set; }
        public SearchNode Parent { get; set; }
        public int Cost { get; set; } 
        
        public IEnumerable<SearchNode> GetNextPossibleNodes()
        {
            var nextNodes = new List<SearchNode>();
            var nextStates = State.GetNextPossibleStates();

            foreach(var (direction, state) in nextStates)
            {
                if(direction.IsOpposite(Direction))
                {
                    //Don't go to previous state.
                    continue;
                }

                nextNodes.Add(new SearchNode
                {
                    Cost = Cost+1,
                    Direction = direction,
                    Parent = this,
                    State = state
                });
            }

            nextNodes.Sort(new SearchNodeManhattanComparer());
            return nextNodes;
        }
        
        public bool IsGoal()
        {
            return State.GetManhattanToGoal() == 0;
        }

        public int GetTotalCost()
        {
            return Cost + State.GetManhattanToGoal();
        }        
    }
}
