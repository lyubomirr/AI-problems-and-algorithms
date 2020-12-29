using System;
using System.Diagnostics;

namespace NQueens
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            var solver = new Solver();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var resultBoard = solver.Solve(n);
            stopwatch.Stop();
            
            if(n <= 50)
            {
                PrintBoard(resultBoard);
            }

            Console.WriteLine("Time elapsed: " + stopwatch.ElapsedMilliseconds / (double)1000 + " seconds.");
        }

        private static void PrintBoard(int[] board)
        {
            for(int i=0; i < board.Length; i++)
            {
                for(int j=0; j < board.Length; j++)
                {
                    Console.Write(j == board[i] ? "*" : "-"); 
                }

                Console.WriteLine();
            }
        }
    }
}
