using System;
using System.Collections.Generic;
using System.Linq;
using GAF;
using GAF.Extensions;
using GAF.Operators;

namespace GSP_by_GeneticAlgorithmFramework
{
    internal class Program
    {
        public static int K = 3;
        public static double optimumTime = 100.0;
        public static string[] names = { "Huey", "Dewey", "Louie", "Goofy", "Micky", "Donald", "Daisy", "Scrooge McDuck" };
        public static string bestWay = "";
        public static List<Factory> ListOfFactories = new List<Factory>();

        static void ga_OnRunComplete(object sender, GaEventArgs e)
        {
            Console.WriteLine("\nTasks for everyone guy:\n------\n" + bestWay);
        }

        private static void ga_OnGenerationComplete(object sender, GaEventArgs e)
        {
            var fittest = e.Population.GetTop(1)[0];
            var difference = CalculateWorkTime(fittest);
            Console.WriteLine("Generation: {0}, Fitness: {1}, Difference: {2}", e.Generation, fittest.Fitness, difference);
        }

        public class Factory
        {
            public string name;
            public List<string> works = new List<string>();
            public double localTotalTime;
            public Factory(string _n, double _l)
            {
                name = _n;
                localTotalTime = _l;
            }
            public override string ToString()
            {
                string result = name + ": LIST OF WORK: { ";
                for (int i = 0; i < works.Count; i++)
                {
                    result += works[i] + ", ";
                }
                return result.Substring(0, result.Length - 2) + " }";
            }
        }

        private static IEnumerable<Work> CreateTasks()
        {
            var tasks = new List<Work>
            {
                new Work("cutting", 4.2),
                new Work("colouring", 2.9),
                new Work("painting", 1.1),
                new Work("drawing", 5.0),
                new Work("writing", 3.2),
                new Work("cleaning", 3.7),
                new Work("cooking", 3.12),
                new Work("sweeping", 5.2),
                new Work("programming", 6.2),
                new Work("shopping", 7.1),
                new Work("washing", 1.1),
                new Work("ironing", 1.2)
            };
            return tasks;
        }

        public static double CalculateFitness(Chromosome chromosome)
        {
            var optimumTime = CalculateWorkTime(chromosome);
            return 1 / (optimumTime + 1);
        }

        public static void CleanData()
        {
            for (int i = 0; i < ListOfFactories.Count; i++)
            {
                ListOfFactories[i].localTotalTime = 0;
                ListOfFactories[i].works = new List<string>();
            }
        }

        private static double CalculateWorkTime(Chromosome chromosome)
        {
            int randomFactory = 0;
            double totalTime = 0;
            CleanData();
            foreach (var gene in chromosome.Genes)
            {
                var currentWork = (Work)gene.ObjectValue;
                if (randomFactory >= K)
                    randomFactory = 0;
                ListOfFactories[randomFactory].works.Add(currentWork.Name);
                ListOfFactories[randomFactory].localTotalTime += currentWork.WorkTime;
                totalTime = currentWork.AssignVarianceForFactories();
                randomFactory++;
            }
            if (totalTime < optimumTime)
            {
                bestWay = "";
                optimumTime = totalTime;
                for (int i = 0; i < ListOfFactories.Count; i++)
                    bestWay += ListOfFactories[i].ToString() + "\n";
            }
            return optimumTime;
        }

        public static bool Terminate(Population population, int currentGeneration, long currentEvaluation)
        {
            return currentGeneration > 200;
        }

        private static void Main(string[] args)
        {
            for (int i = 0; i < K; i++)
            {
                Factory tmp = new Factory(names[i], 0);
                ListOfFactories.Add(tmp);
            }

            // GAF components
            const int populationSize = 25;
            var tasks = CreateTasks().ToList();
            var population = new Population();
            for (var p = 0; p < populationSize; p++)
            {
                var chromosome = new Chromosome();
                foreach (var work in tasks)
                    chromosome.Genes.Add(new Gene(work));
                var rnd = GAF.Threading.RandomProvider.GetThreadRandom();
                chromosome.Genes.ShuffleFast(rnd);
                population.Solutions.Add(chromosome);
            }

            var elite = new Elite(5);
            var crossover = new Crossover(0.8)
            {
                CrossoverType = CrossoverType.DoublePointOrdered
            };
            var mutate = new SwapMutate(0.02);
            var ga = new GeneticAlgorithm(population, CalculateFitness);

            ga.OnGenerationComplete += ga_OnGenerationComplete;
            ga.OnRunComplete += ga_OnRunComplete;

            ga.Operators.Add(elite);
            ga.Operators.Add(crossover);
            ga.Operators.Add(mutate);

            ga.Run(Terminate);
            Console.Read();
        }
    }
}