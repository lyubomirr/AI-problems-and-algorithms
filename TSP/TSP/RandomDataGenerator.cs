using System;
using System.Linq;

namespace TSP
{
    public static class RandomDataGenerator
    {
        private const int CoordinatesMinValue = -100;
        private const int CoordinatesMaxValue = 100;

        private static Random Random = new Random();

        public static double[,] GenerateDistanceMatrix(int pointCount)
        {
            var points = GenerateRandomPoints(pointCount);
            if(pointCount <= 20)
            {
                PrintPoints(points);
            }

            var distanceMatrix = new double[pointCount, pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    if(i == j)
                    {
                        distanceMatrix[i, j] = 0;
                        continue;
                    }                   
                
                    distanceMatrix[i, j] = distanceMatrix[j, i] = CalculateDistance(points[i], points[j]);                    
                }
            }

            return distanceMatrix;
        }

        private static (int X, int Y)[] GenerateRandomPoints(int pointCount)
        {
            var points = new (int X, int Y)[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                points[i] = (Random.Next(CoordinatesMinValue, CoordinatesMaxValue), 
                    Random.Next(CoordinatesMinValue, CoordinatesMaxValue));
            }

            return points;
        }

        private static void PrintPoints((int X, int Y)[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                Console.WriteLine($"Point {i}: X={points[i].X}, Y={points[i].Y}");
            }
            Console.WriteLine();
        }

        private static double CalculateDistance((int X, int Y) pointA, (int X, int Y) pointB)
        {
            return Math.Sqrt((pointA.X - pointB.X) * (pointA.X - pointB.X) 
                + (pointA.Y - pointB.Y) * (pointA.Y - pointB.Y));
        }

        public static Individual[] GenerateRandomPopulation(int pointCount, int populationSize)
        {
            var population = new Individual[populationSize];
            for(int i = 0; i < populationSize; i++)
            {
                var order = Enumerable.Range(0, pointCount).OrderBy(x => Random.Next()).ToArray();
                population[i] = new Individual(order);               
            }

            return population;
        }
    }
}
