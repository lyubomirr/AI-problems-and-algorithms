using System;
using System.Collections.Generic;
using System.Linq;

namespace NPuzzle
{
    public class BoardState
    {
        public static int GoalZeroPosition;

        private int? _manhattanDistance;
        private int? _hashCode;
        private (int Row, int Col) _zeroIndices; //Cache the zero indices for creating new board states easily. 

        public BoardState(int[][] board, (int, int) zeroIndices)
        {
            Board = board;
            _zeroIndices = zeroIndices;
        }      

        public int[][] Board { get; }

        public override bool Equals(object obj)
        {
            return obj is BoardState state && Utils.AreEqual(Board, state.Board);
        }

        public override int GetHashCode()
        {
            if(_hashCode.HasValue)
            {
                return _hashCode.Value;
            }

        
            unchecked
            {
                var boardLength = Board.GetLength(0);
                var hash = 17;

                for (var i = 0; i < boardLength; i++)
                {
                    for (var j = 0; j < boardLength; j++)
                    {
                        hash = hash * 31 + Board[i][j];
                    }
                }

                _hashCode = hash;
                return hash;
            }
        }      
        
        public int GetManhattanToGoal()
        {
            if (_manhattanDistance.HasValue)
            {
                return _manhattanDistance.Value;
            }

            int distance = 0;
            int boardLength = Board.GetLength(0);
            for (int i = 0; i < boardLength; i++)
            {
                for (int j = 0; j < boardLength; j++)
                {
                    int value = Board[i][j];
                    if (value == 0) continue;

                    int position = i * boardLength + j;
                    int goalPosition = GoalZeroPosition == 0 ? Board[i][j] 
                        : position <= GoalZeroPosition ? Board[i][j] - 1 : Board[i][j];

                    if (position == goalPosition) continue;

                    int goalRow = goalPosition / boardLength;
                    int goalCol = goalPosition % boardLength;

                    distance += Math.Abs(i - goalRow) + Math.Abs(j - goalCol);
                }
            }

            _manhattanDistance = distance;
            return distance;
        }

        public IDictionary<Direction, BoardState> GetNextPossibleStates()
        {
            var possibleDirections = GetPossibleDirectionsAndZeroIndices();
            var possibleStates = new Dictionary<Direction, BoardState>();

            foreach(var (direction, newZeroIndices) in possibleDirections)
            {
                //Copy the parent's board and swap the old and the new zero.
                var newBoard = Board.Select(r => r.ToArray()).ToArray();
                Utils.Swap(ref newBoard[_zeroIndices.Row][_zeroIndices.Col], 
                    ref newBoard[newZeroIndices.Item1][newZeroIndices.Item2]);

                possibleStates.Add(direction, new BoardState(newBoard, newZeroIndices));
            }

            return possibleStates;
        }

        private IDictionary<Direction, (int, int)> GetPossibleDirectionsAndZeroIndices()
        {
            var boardLength = Board.GetLength(0);
            var directions = new Dictionary<Direction, (int, int)>();

            if (_zeroIndices.Row > 0)
            {
                directions.Add(Direction.Down, (_zeroIndices.Row-1, _zeroIndices.Col));
            }

            if (_zeroIndices.Col > 0)
            {
                directions.Add(Direction.Right, (_zeroIndices.Row, _zeroIndices.Col-1));
            }

            if(_zeroIndices.Row < boardLength - 1)
            {
                directions.Add(Direction.Up, (_zeroIndices.Row+1, _zeroIndices.Col));
            }

            if(_zeroIndices.Col < boardLength - 1)
            {
                directions.Add(Direction.Left, (_zeroIndices.Row, _zeroIndices.Col+1));
            }

            return directions;
        }
    }
}
