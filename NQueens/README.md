# N-Queens
This is an implementation of __Min-conflicts algorithm__ used for solving the constraint satisfaction problem N-Queens.
There are three ways of the initial placing of the queens on the boards:
1. Placing the queen on the min-conflict row of its column.
2. Placing them in an L shape.
3. Placing them at random row until getting zero conflicts or reaching a certain amount of tries.

__By default it uses the third approach.__

On the first line the program expects the size of the chess board.
The program outputs the board itself if the size is less than or equal to 50 and also the time elapsed. If board is bigger you see the time elapsed only.

### Example input:
```
5
```

### Example output:
```
---*-
-*---
----*
--*--
*----
Time elapsed: 0.014 seconds.
```
