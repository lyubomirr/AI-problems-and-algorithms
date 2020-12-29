using System;
using System.Collections.Generic;
using System.Linq;

namespace NPuzzle
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = CreateSolver();            
            var goalNode = solver.SolveIDAStar();       
            PrintAnswer(goalNode);
        }

        private static Solver CreateSolver()
        {
            var size = int.Parse(Console.ReadLine());

            if (Math.Sqrt(size + 1) % 1 != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(size), "Invalid board size!");
            }
        
            var zeroGoalPosition = int.Parse(Console.ReadLine());
            BoardState.GoalZeroPosition = zeroGoalPosition == -1 ? size : zeroGoalPosition;

            var boardLength = (int)Math.Sqrt(size + 1);
            var initialBoard = new int[boardLength][];

            for (var i = 0; i < boardLength; i++)
            {
                initialBoard[i] = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            }
 
            return new Solver(boardLength, new SearchNode
            {
                Cost = 0,
                Direction = Direction.None,
                //We get the zero indices for the intial board, then we can easily calculate it for the child nodes.
                State = new BoardState(initialBoard, GetZeroIndices(initialBoard, boardLength))
            });
        }

        private static (int, int) GetZeroIndices(int[][] board, int boardLength)
        {
            for (int i = 0; i < boardLength; i++)
            {          
                for(int j = 0; j < boardLength; j++)
                {
                    if(board[i][j] == 0)
                    {
                        return (i, j);
                    }
                }
            }

            throw new ArgumentException("No zero in intial board!");
        }

        private static void PrintAnswer(SearchNode goalNode)
        {
            Console.WriteLine(goalNode.Cost);

            var directions = new List<Direction>();
            while (goalNode.Parent != null)
            {
                directions.Add(goalNode.Direction);
                goalNode = goalNode.Parent;
            }

            directions.Reverse();
            foreach (var direction in directions)
            {
                Console.WriteLine(direction.ToString().ToLower());
            }
        }
    }
}
