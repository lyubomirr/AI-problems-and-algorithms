# N-Puzzle
This is an implementation of __A* and IDA* algorithms with Mahnattan distance heuristic__ used for solving the N-Puzzle problem.

On the first line the program expects the size of the board (8 for 3x3, 15 for 4x4 etc.).
On the second line it expects the index of the zero (the empty) cell in the final solution: `-1` is for the default value (down right corner).
On the next `n` lines the program expects the board itself.

The program outputs the moves needed to solve the puzzle and their count.

### Example input:
```
8
-1
1 2 3
4 5 6
0 7 8
```

### Example output:
```
2
left
left
```
