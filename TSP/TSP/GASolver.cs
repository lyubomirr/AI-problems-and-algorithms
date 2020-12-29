using System;
using System.Linq;

namespace TSP
{
    public static class GASolver
    {
        private static Random Random = new Random();

        public static void Solve(int pointCount, int populationSize, double mutationRate)
        {
            var distances = RandomDataGenerator.GenerateDistanceMatrix(pointCount);
            var currentPopulation = RandomDataGenerator.GenerateRandomPopulation(pointCount, populationSize);
            currentPopulation = currentPopulation.OrderByDescending(i => i.GetFitness(distances)).ToArray();

            var sameBestResultCount = 0;
            double latestBest = currentPopulation[0].GetTotalDistance(distances);            

            var generation = 0;          
            while (sameBestResultCount < 10)
            {                
                if((generation) % 10 == 0)
                {
                    PrintCurrentState(currentPopulation[0], distances, generation);
                }

                currentPopulation = CreateNextGeneration(currentPopulation, distances, populationSize, mutationRate);
                var populationBest = currentPopulation[0].GetTotalDistance(distances);

                if (Utils.AreEqual(populationBest, latestBest))
                {
                    sameBestResultCount++;                
                }
                else
                {
                    latestBest = populationBest;
                    sameBestResultCount = 0;
                }
                
                generation++;
                mutationRate += mutationRate * 0.03;
            }

            PrintCurrentState(currentPopulation[0], distances, generation);
        }
        
     
        private static void PrintCurrentState(Individual bestIndividual, double[,] distances, int generation)
        {
            Console.WriteLine($"Generation {generation}: ");
            Console.Write($"Best path: ");
            for (int i = 0; i < bestIndividual.CityOrder.Length - 1; i++)
            {
                Console.Write($"{bestIndividual.CityOrder[i]}->");
            }

            Console.WriteLine(bestIndividual.CityOrder[bestIndividual.CityOrder.Length - 1]);
            Console.WriteLine($"Total distance: {bestIndividual.GetTotalDistance(distances)}");
            Console.WriteLine();
        }

        private static Individual[] CreateNextGeneration(
            Individual[] currentPopulation, double[,] distances, int populationSize, double mutationRate)
        {
            var eliteSize = (int)Math.Ceiling(populationSize / 20d); //Always advance the top 5% to the next generation
            var elites = currentPopulation.Take(eliteSize).ToArray();

            currentPopulation = GetSelection(currentPopulation, distances, populationSize - eliteSize);
            currentPopulation = Crossover(currentPopulation);
            Mutate(currentPopulation, mutationRate);
            currentPopulation = elites.Concat(currentPopulation).ToArray();
            return currentPopulation.OrderByDescending(i => i.GetFitness(distances)).ToArray();
        }

        private static Individual[] GetSelection(Individual[] population, double[,] distances, int selectionLength)
        {
            var normalizedAccumulatedFitness = GetNormalizedAccumulatedFitnessValues(population, distances);
            var selection = new Individual[selectionLength];

            for (int i = 0; i < selectionLength; i++)
            {
                var pick = Random.NextDouble();
                for (int j = 0; j < normalizedAccumulatedFitness.Length; j++)
                {
                    if(pick <= normalizedAccumulatedFitness[j])
                    {
                        selection[i] = population[j];
                        break;
                    }
                }
            }

            return selection;   
        }

        private static double[] GetNormalizedAccumulatedFitnessValues(Individual[] population, double[,] distances)
        {
            var totalFitnes = population.Sum(i => i.GetFitness(distances));

            //Accumulate values
            var accumulatedFitnessValues = new double[population.Length];
            for (int i = 0; i < population.Length; i++)
            {
                accumulatedFitnessValues[i] = population[i].GetFitness(distances)
                    + (i == 0 ? 0 : accumulatedFitnessValues[i - 1]);
            }
            
            //Normalize
            return accumulatedFitnessValues.Select(v => v / totalFitnes).ToArray();
        }        

        private static Individual[] Crossover(Individual[] population)
        {
            var newPopulation = new Individual[population.Length];

            for(int i=0; i < population.Length - 1; i++)
            {
                var parentA = population[i];
                var parentB = population[i + 1];

                var (childA, childB) = Breed(parentA, parentB);

                newPopulation[i] = childA;
                newPopulation[i + 1] = childB;
            }

            return newPopulation;
        }

        private static (Individual childA, Individual childB) Breed(Individual parentA, Individual parentB)
        {
            var cityCount = parentA.CityOrder.Length;

            var childAOrder = Enumerable.Repeat(-1, cityCount).ToArray();
            var childBOrder = Enumerable.Repeat(-1, cityCount).ToArray();

            var startGene = Random.Next(0, cityCount);
            var endGene = Random.Next(startGene, cityCount);

            for (int gene = startGene; gene <= endGene; gene++)
            {
                childAOrder[gene] = parentA.CityOrder[gene];
                childBOrder[gene] = parentB.CityOrder[gene];
            }

            var childA = FillRemainingGenes(childAOrder, parentB.CityOrder, endGene);
            var childB = FillRemainingGenes(childBOrder, parentA.CityOrder, endGene);

            return (childA, childB);
        }

        private static Individual FillRemainingGenes(int[] childOrder, int[] parentOrder, int endGene)
        {
            var cityCount = parentOrder.Length;
            var childIndex = (endGene + 1) % cityCount;
            var parentIndex = childIndex;

            while (childOrder[childIndex] == -1)
            {
                while (childOrder.Contains(parentOrder[parentIndex]))
                {
                    parentIndex = (parentIndex + 1) % cityCount;
                }

                childOrder[childIndex] = parentOrder[parentIndex];
                childIndex = (childIndex + 1) % cityCount;
            }

            return new Individual(childOrder);
        }

        public static void Mutate(Individual[] population, double mutationRate)
        {
            foreach(var individual in population)
            {
                individual.SwapMutate(mutationRate);
            }
        }
    }
}
