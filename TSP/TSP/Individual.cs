using System;
using System.Diagnostics;
using System.Linq;

namespace TSP
{
    public class Individual
    {
        private static Random Random = new Random();

        private double? _fitness;
        private double? _totalDistance;

        public Individual(int[] cityOrder)
        {
            CityOrder = cityOrder;
        }

        public int[] CityOrder { get; }
        
        public double GetFitness(double[,] distanceMatrix)
        {
            if(_fitness.HasValue)
            {
                return _fitness.Value;
            }

            double totalDistance = GetTotalDistance(distanceMatrix);
        
            _fitness = 1 / (totalDistance + 1);
            return _fitness.Value;
        }

        public void SwapMutate(double mutationRate)
        {
            bool mutated = false;

            for(int posA = 0; posA < CityOrder.Length; posA++)
            {
                if(Random.NextDouble() < mutationRate)
                {
                    mutated = true;
                    var posB = Random.Next(0, CityOrder.Length);
                    while(posA == posB)
                    {
                        posB = Random.Next(0, CityOrder.Length);
                    }

                    CityOrder[posA] ^= CityOrder[posB];
                    CityOrder[posB] ^= CityOrder[posA];
                    CityOrder[posA] ^= CityOrder[posB];
                }
            }

            if(mutated)
            {
                _fitness = null;
                _totalDistance = null;
            }

            Debug.Assert(CityOrder.Distinct().Count() == CityOrder.Count());
        }

        public double GetTotalDistance(double[,] distanceMatrix)
        {
            if(_totalDistance.HasValue)
            {
                return _totalDistance.Value;
            }

            double totalDistance = 0;
            for (int i = 0; i < CityOrder.Length - 1; i++)
            {
                totalDistance += distanceMatrix[CityOrder[i], CityOrder[i + 1]];
            }

            _totalDistance = totalDistance;
            return totalDistance;
        }
    }
}
