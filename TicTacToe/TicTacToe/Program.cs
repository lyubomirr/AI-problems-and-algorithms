using System;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Do you want to start first? (y/n)");
            var answer = Console.ReadLine();
            var isUserTurn = answer == "y";
            Game.Start(isUserTurn);

            //var s = new GameState
            //{
            //    Board = new char[,]
            //    {
            //         {'X', 'O', 'X'},
            //         {'X', 'O', 'O'},
            //         {'O', 'X', 'X'}
            //    }
            //};

            //var a = s.IsTerminal(out var winner);
        }
    }
}
