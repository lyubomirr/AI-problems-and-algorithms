using System;

namespace TSP
{
    class Program
    {

        static void Main(string[] args)
        {
            var pointCount = int.Parse(Console.ReadLine());
            var populationSize = 2 * pointCount;
            var mutationRate = 5d / pointCount;

            GASolver.Solve(pointCount, populationSize, mutationRate);
        }
    }
}
