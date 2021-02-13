using System;
using System.Text;

namespace TicTacToe
{
    public class GameState
    {
        public const int BoardSize = 3;

        public GameState()
        {
            Board = CreateInitialBoard();
        }

        public char[,] Board { get; set; }

        private char[,] CreateInitialBoard()
        {
            var board = new char[BoardSize, BoardSize];
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    board[i, j] = CellValues.Empty;
                }
            }

            return board;
        }

        public bool IsTerminal(out char winner)
        {
            var areEqual = true;
            //Check for empty cells. If no empty cells but no winner -> tie.
            var emptyCells = 0;

            //Check rows.
            for (int i = 0; i < BoardSize; i++)
            {
                areEqual = true;
                for(int j = 0; j < BoardSize - 1; j++)
                {
                    areEqual &= Board[i, j] == Board[i, j + 1];

                    //Check on first iteration if there are empty cells.
                    if (Board[i, j] == CellValues.Empty) emptyCells++;
                }

                //Check last cell if empty also.
                if (Board[i, BoardSize - 1] == CellValues.Empty) emptyCells++;

                if(areEqual && Board[i, 0] != CellValues.Empty)
                {
                    winner = Board[i, 0];
                    return true;
                }
            }

            //Check cols.
            for (int i = 0; i < BoardSize; i++)
            {
                areEqual = true;
                for (int j = 0; j < BoardSize - 1; j++)
                {
                    areEqual &= Board[j, i] == Board[j + 1, i];
                }

                if (areEqual && Board[0, i] != CellValues.Empty)
                {
                    winner = Board[0, i];
                    return true;
                }
            }

            //Check main diagonal.
            areEqual = true;
            for (int i = 0; i < BoardSize - 1; i++)
            {
                areEqual &= Board[i, i] == Board[i + 1, i + 1];                
            }

            if (areEqual && Board[0, 0] != CellValues.Empty)
            {
                winner = Board[0, 0];
                return true;
            }

            //Check secondary diagonal.
            areEqual = true;
            for (int i = 0; i < BoardSize - 1; i++)
            {
                areEqual &= Board[i, BoardSize - i - 1] == Board[i + 1, BoardSize - i - 2];
            }

            if (areEqual && Board[0, BoardSize - 1] != CellValues.Empty)
            {
                winner = Board[0, BoardSize - 1];
                return true;
            }

            winner = CellValues.Empty;
            return emptyCells == 0;
        }
    
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(new string('-', BoardSize * 2 + 1));
           
            for (int i = 0; i < BoardSize; i++)
            {
                builder.Append('|');
                for (int j = 0; j < BoardSize; j++)
                {
                    builder.Append(Board[i, j]);
                    builder.Append('|');                   
                }                
                builder.AppendLine();
            }

            builder.AppendLine(new string('-', BoardSize * 2 + 1));
            return builder.ToString();
        }
    }
}
