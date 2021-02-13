using System;

namespace TicTacToe
{
    public class MiniMaxBot
    {
        public void Play(GameState state)
        {
            //Bot is always max.
            var bestScore = int.MinValue;           
            (int i, int j) bestMove = (-1, -1);

            for (int i = 0; i < GameState.BoardSize; i++)
            {
                for (int j = 0; j < GameState.BoardSize; j++)
                {
                    //Try the next empty spot.
                    if (state.Board[i, j] == CellValues.Empty)
                    {
                        state.Board[i, j] = CellValues.Bot;
                        var score = GetBestScore(state, 0, int.MinValue, int.MaxValue, false);
                        state.Board[i, j] = CellValues.Empty;

                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove = (i, j);
                        }
                    }
                }
            }

            //Play best move.
            state.Board[bestMove.i, bestMove.j] = CellValues.Bot;
        }

        private int GetBestScore(GameState state, int depth, int alpha, int beta, bool isMax)
        {
            if (state.IsTerminal(out var winner))
            {
                return GetUtility(winner, depth);
            }
                
            var bestScore = isMax ? int.MinValue : int.MaxValue;

            for (int i = 0; i < GameState.BoardSize; i++)
            {
                for (int j = 0; j < GameState.BoardSize; j++)
                {
                    //Try the next empty spot.
                    if(state.Board[i, j] == CellValues.Empty)
                    {
                        //Bot is always max player.
                        state.Board[i, j] = isMax ? CellValues.Bot : CellValues.User;
                        var score = GetBestScore(state, depth + 1, alpha, beta, !isMax);
                        //Rollback the change.
                        state.Board[i, j] = CellValues.Empty;

                        bestScore = isMax ? Math.Max(bestScore, score) : Math.Min(bestScore, score);
                        if(isMax)
                        {
                            alpha = Math.Max(alpha, bestScore);
                            if (alpha >= beta) return bestScore;
                        }
                        else
                        {
                            beta = Math.Min(beta, bestScore);
                            if (beta <= alpha) return bestScore;
                        }
                    }
                }
            }

            return bestScore;
        }

        private int GetUtility(char winner, int depth)
        {
            if (winner == CellValues.Bot) return 10 - depth;
            if (winner == CellValues.User) return -(10 - depth);
            return 0;
        }
    }
}
