using System;
using System.Linq;

namespace TicTacToe
{
    public static class Game
    {
        public static void Start(bool isUserTurn)
        {
            var state = new GameState();
            var bot = new MiniMaxBot();
            char winner = CellValues.Empty;

            if (isUserTurn)
            {
                Console.WriteLine(state.ToString());
            }

            while(!state.IsTerminal(out winner))
            {
                if (isUserTurn)
                {
                    PlayUserTurn(state);
                }
                else
                {
                    PlayBotTurn(state, bot);
                }

                Console.WriteLine(state.ToString());
                isUserTurn = !isUserTurn;
            }

            ShowUserEndMessage(winner);            
        }

        private static void PlayUserTurn(GameState state)
        {
            (int i, int j) = GetUserMove();
            while (state.Board[i, j] != CellValues.Empty)
            {
                Console.WriteLine("You this cell is already occupied. Try again.");
                (i, j) = GetUserMove();
            }

            state.Board[i, j] = CellValues.User;
        }

        private static (int i, int j) GetUserMove()
        {
            Console.WriteLine("Enter the position you wanna play in (e.g '1,3'):");
            var positions = Console.ReadLine().Split(',').Select(int.Parse).ToArray();
            return (positions[0] - 1, positions[1] - 1);
        }

        private static void PlayBotTurn(GameState state, MiniMaxBot bot)
        {
            Console.WriteLine("Bot plays: ");
            bot.Play(state);
        }

        private static void ShowUserEndMessage(char winner)
        {
            if (winner == CellValues.User)
            {
                Console.WriteLine("Congratulations! You win!");
                return;
            }

            if (winner == CellValues.Bot)
            {
                Console.WriteLine("Unfortunately, you lose! Try again. :)");
                return;
            }

            if (winner == CellValues.Empty)
            {
                Console.WriteLine("Its a tie!");
            }
        }
    }
}
