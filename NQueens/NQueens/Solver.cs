using System;
using System.Collections.Generic;
using System.Linq;

namespace NQueens
{
    public class Solver
    {
        private static readonly Random Random = new Random();

        private int[] _queenPositionsForColumn;
        private int[] _rowQueenCount;
        private int[] _mainDiagonalQueenCount;
        private int[] _secondaryDiagonalQueenCount;

        private int Size => _queenPositionsForColumn.Length;

        public int[] Solve(int n)
        {
            var times = new List<int>();

            InitializeData(n);
            InitializePseudoRandomBoard();
            SolveInternal();

            return _queenPositionsForColumn;
        }

        private void InitializeData(int n)
        {
            _queenPositionsForColumn = Enumerable.Repeat(-1, n).ToArray();
            _rowQueenCount = new int[n];
            _mainDiagonalQueenCount = new int[2 * n - 1];
            _secondaryDiagonalQueenCount = new int[2 * n - 1];
        }

        private void InitializeLBoard()
        {
            var row = 0;
            for(int col = 0; col < Size; col++)
            {
                while (_rowQueenCount[row] > 0)
                {
                    row = (row + 1) % Size;
                }

                PlaceQueen(col, row);
                row = (row + 2) % Size;
            }
        }

        private void InitializePseudoRandomBoard()
        {           
            for (int col = 0; col < Size; col++)
            {
                int minConflicts = -1;
                int minRow = -1;

                for (int i=0; i < Size; i++)
                {
                    var row = Random.Next(0, Size);
                    var conflicts = GetConfilctsForPosition(col, row);
                    if(conflicts == 0)
                    {
                        minRow = row;
                        break;
                    }

                    if(minConflicts == -1 || conflicts < minConflicts)
                    {
                        minRow = row;
                        minConflicts = conflicts;
                    }
                }

                PlaceQueen(col, minRow);                
            }
        }

        private void InitializeMinConflictBoard()
        {
            for (int col = 0; col < Size; col++)
            {
                var minConflicts = -1;
                var rowConflicts = new int[Size];

                for (int row = 0; row < Size; row++)
                {
                    //Don't place a queen if there is another on the same row.
                    if (_rowQueenCount[row] > 0)
                    {
                        continue;
                    }

                    rowConflicts[row] = GetConfilctsForPosition(col, row);
                    if (minConflicts == -1 || rowConflicts[row] < minConflicts)
                    {
                        minConflicts = rowConflicts[row];
                    }
                }

                var minConflictRows = new List<int>();
                for (int row = 0; row < Size; row++)
                {
                    //Choose only empty rows.
                    if (rowConflicts[row] == minConflicts && _rowQueenCount[row] == 0) minConflictRows.Add(row);
                }

                var minRow = minConflictRows.Count == 1 ? minConflictRows[0] : minConflictRows[Random.Next(0, minConflictRows.Count)];
                PlaceQueen(col, minRow);
            }
        }
             
        private int GetConfilctsForPosition(int col, int row)
        {
            var (mainDiagonalIndex, secondaryDiagonalIndex) = GetDiagonalIndexes(col, row);

            var conflicts = _rowQueenCount[row] + _mainDiagonalQueenCount[mainDiagonalIndex] 
                + _secondaryDiagonalQueenCount[secondaryDiagonalIndex];

            var isPositionOccupied = _queenPositionsForColumn[col] == row;
            //If the position is occupied remove the queen count from the two diagonals and the row, hence -3 conflicts.
            return  isPositionOccupied ? conflicts - 3 : conflicts; 
        }

        private (int, int) GetDiagonalIndexes(int col, int row)
        {
            var mainDiagonalIndex = col - row + Size - 1;
            var secondaryDiagonalIndex = col + row;

            return (mainDiagonalIndex, secondaryDiagonalIndex);
        }

        private void PlaceQueen(int col, int row)
        {
            var (mainDiagonalIndex, secondaryDiagonalIndex) = GetDiagonalIndexes(col, row);
       
            _queenPositionsForColumn[col] = row;
            _rowQueenCount[row]++;
            _mainDiagonalQueenCount[mainDiagonalIndex]++;
            _secondaryDiagonalQueenCount[secondaryDiagonalIndex]++;
        }
        
        private void SolveInternal()
        {
            int iterations = 0;

            while(true)
            {
                var col = GetColWithMaxConfilctQueen();          
                if(col == -1)
                {
                    return;
                }

                var newRow = GetRowWithMinConflicts(col);
                if(newRow > -1)
                {
                    MoveQueen(col, _queenPositionsForColumn[col], newRow);
                }

                iterations++;
                if (iterations == 3*Size)
                {
                    Console.WriteLine("Resetting...");
                    InitializeData(Size);
                    InitializePseudoRandomBoard();
                    iterations = 0;
                }
            }
        }

        private int GetColWithMaxConfilctQueen()
        {
            var maxConflicts = -1;
            int[] colConflicts = new int[Size];

            for(int col = 0; col < Size; col++)
            {
                colConflicts[col] = GetConfilctsForPosition(col, _queenPositionsForColumn[col]);
                if(colConflicts[col] > maxConflicts)
                {
                    maxConflicts = colConflicts[col];
                }
            }

            if (maxConflicts == 0) return -1;

            var maxConflictColumns = new List<int>();
            for(int col=0; col < Size; col++)
            {
                if (colConflicts[col] == maxConflicts) maxConflictColumns.Add(col);
            }

            //Return a random max conflict col.
            var maxConflictCol = maxConflictColumns.Count == 1 ? maxConflictColumns[0] 
                : maxConflictColumns[Random.Next(0, maxConflictColumns.Count)];

            return maxConflictCol;
        }

        private int GetRowWithMinConflicts(int col)
        {
            int currentConflicts = -1;
            int[] rowConflicts = new int[Size];
            int minConflicts = -1;

            for (int row = 0; row < Size; row++)
            {
                if (_queenPositionsForColumn[col] == row)
                {
                    currentConflicts = GetConfilctsForPosition(col, row);
                    continue;
                }

                rowConflicts[row] = GetConfilctsForPosition(col, row);
                if(minConflicts == -1 || rowConflicts[row] < minConflicts)
                {
                    minConflicts = rowConflicts[row];                    
                }
            }

            //No need to move if it will make it worse.
            if (currentConflicts < minConflicts)
            {
                return -1;
            }

            var minConflictRows = new List<int>();
            for (int row = 0; row < Size; row++)
            {
                if (rowConflicts[row] == minConflicts && _queenPositionsForColumn[col] != row) minConflictRows.Add(row);
            }

            return minConflictRows.Count == 1 ? minConflictRows[0] : minConflictRows[Random.Next(0, minConflictRows.Count)];
        }

        private void MoveQueen(int col, int oldRow, int newRow)
        {
            RemoveQueen(col, oldRow);
            PlaceQueen(col, newRow);
        }

        private void RemoveQueen(int col, int row)
        {
            var (mainDiagonalIndex, secondaryDiagonalIndex) = GetDiagonalIndexes(col, row);

            _rowQueenCount[row]--;
            _mainDiagonalQueenCount[mainDiagonalIndex]--;
            _secondaryDiagonalQueenCount[secondaryDiagonalIndex]--;
        }
    }
}
