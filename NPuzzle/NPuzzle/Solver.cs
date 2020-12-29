using C5;
using System;

namespace NPuzzle
{
    public class Solver
    {
        private readonly int _boardLength;
        private readonly SearchNode _initialNode;
        private readonly System.Collections.Generic.HashSet<BoardState> _seenStates;

        public Solver(int boardLength, SearchNode intialNode)
        {
            _boardLength = boardLength;
            _initialNode = intialNode;
            _seenStates = new System.Collections.Generic.HashSet<BoardState>();
        }

        public SearchNode SolveAStar()
        {
            var fringe = new IntervalHeap<SearchNode>(new SearchNodeManhattanComparer());

            fringe.Add(_initialNode);
            _seenStates.Add(_initialNode.State);

            while (fringe.Count > 0)
            {
                var currentNode = fringe.DeleteMin();
                if (currentNode.IsGoal())
                {
                    return currentNode;
                }

                var childNodes = currentNode.GetNextPossibleNodes();
                foreach (var node in childNodes)
                {
                    if (!_seenStates.Contains(node.State))
                    {
                        _seenStates.Add(node.State);
                        fringe.Add(node);
                    }
                }
            }

            throw new InvalidOperationException("Could'n be solved!");
        }

        public SearchNode SolveIDAStar()
        {
            var threshold = _initialNode.GetTotalCost();
            while (true)
            {
                _seenStates.Add(_initialNode.State);
                var result = Search(_initialNode, threshold);
                if (result.IsNotFound)
                {
                    throw new InvalidOperationException("Goal node not found!");
                }

                if (result.IsGoal)
                {
                    return result.GoalNode;
                }

                threshold = result.NewThreshold;
                _seenStates.Clear();
            }
        }

        private SearchResult Search(SearchNode node, int threshold)
        {
            var totalCost = node.GetTotalCost();
            if (totalCost > threshold)
            {
                return new SearchResult { NewThreshold = totalCost };
            }

            if (node.IsGoal())
            {
                return new SearchResult { GoalNode = node };
            }

            var minNewThreshold = int.MaxValue;
            var childNodes = node.GetNextPossibleNodes();

            foreach(var childNode in childNodes)
            {
                if (_seenStates.Contains(childNode.State))
                {
                    continue;
                }
                _seenStates.Add(node.State);

                var searchResult = Search(childNode, threshold);
                if (searchResult.IsGoal)
                {
                    return searchResult;
                }
                if(searchResult.NewThreshold < minNewThreshold)
                {
                    minNewThreshold = searchResult.NewThreshold;
                }               
            }

            return new SearchResult { NewThreshold = minNewThreshold };
        }
    }
}