using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    class Program
    {
        static void Main(string[] args)
        {
            // ******************************* General Scheduling Problem Components ******************************* \\
            List<Task> ListOfTasks = new List<Task>();
            List<string> ListOfCombinations = new List<string>();
            List<Way> ListOfWays = new List<Way>();
            List<Way> BestWays = new List<Way>();
            List<Factory> ListOfFactories = new List<Factory>();
            ListOfTasks = CreatingComponents.CreateTasks();
            ListOfCombinations = Permutation.Permute(ListOfTasks, 0, Parameters.N - 1);
            ListOfFactories = CreatingComponents.CreateFactories();
            for (int j = 0; j < ListOfCombinations.Count; j++)
            {
                Way tmp = new Way(ListOfCombinations[j], SchedulingAlgorithm.AssignDuration(ListOfCombinations[j],
                    ListOfTasks, Parameters.K, ListOfFactories), j, Parameters.animation);
                Parameters.animation = "";
                ListOfWays.Add(tmp);
            }

            // ******************************* Simulated Annealing Components ******************************* \\
            int[] range = { 0, ListOfWays.Count };
            double s0 = SimulatedAnnealing.f(1, ListOfWays);
            double best_solution_s8;
            double solution_s;
            double incumbent_solution_s6;
            double T0 = SimulatedAnnealing.InitialTemperature(1000);
            best_solution_s8 = incumbent_solution_s6 = s0;
            int i = 0;

            while (!SimulatedAnnealing.StoppingCriterion(i))
            {
                solution_s = SimulatedAnnealing.ExplorationCriterion(range, ListOfWays);
                if (SimulatedAnnealing.AcceptanceCriterion(solution_s, best_solution_s8, T0))
                {
                    incumbent_solution_s6 = solution_s;
                    if (incumbent_solution_s6 < best_solution_s8)
                        best_solution_s8 = incumbent_solution_s6;
                }
                if (SimulatedAnnealing.TemperatureLenght())
                    T0 = SimulatedAnnealing.CoolingScheme(T0);
                T0 = SimulatedAnnealing.ResetTemperature(T0);
                i++;
            }

            for (int j = 0; j < ListOfWays.Count; j++)
                if (ListOfWays[j].worktime == (int)best_solution_s8) BestWays.Add(ListOfWays[j]);


            // ******************************* Output ******************************* \\
            PrintingMethods.PrintOutput(BestWays);
        }
    }
}