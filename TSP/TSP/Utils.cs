using System;

namespace TSP
{
    public static class Utils
    {
        public static bool AreEqual(double first, double second)
        {
            double tolerance = 1e-8;
            return Math.Abs(first - second) < tolerance;
        }
    }
}
