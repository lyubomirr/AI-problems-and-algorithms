namespace NPuzzle
{
    public static class Utils
    {
        public static void Swap(ref int first, ref int second)
        {
            if(first != second)
            {
                first ^= second;
                second ^= first;
                first ^= second;
            }
        }

        public static bool AreEqual(int[][] first, int[][] second)
        {
            //We assume they are square and have same lengths for the puropose of the task.        
            var length = first.GetLength(0);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    if (first[i][j] != second[i][j]) return false;
                }
            }

            return true;
        }
    }
}
