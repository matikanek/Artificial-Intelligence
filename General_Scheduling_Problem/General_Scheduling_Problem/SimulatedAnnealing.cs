using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    public class SimulatedAnnealing
    {
        public static double ResetTemperature(double actualTemp)
        {
            return actualTemp;
        }

        public static double CoolingScheme(double actualTemp)
        {
            double alfa = 0.95;
            return actualTemp * alfa;
        }

        public static bool TemperatureLenght()
        {
            return true;
        }

        public static bool AcceptanceCriterion(double actualSol, double bestSol, double T)
        {
            double diff = actualSol - bestSol;
            if (diff >= 0) return true;
            else
            {
                if (Math.Exp(-diff / T) > RandomNumber(0, 1)) return true;
                else return false;
            }
        }

        public static double ExplorationCriterion(int[] interval, List<Way> data)
        {
            int amount = 10;
            double candidate;
            double best = f(1, data); // Uwaga, wartość ta jest dobra tylko dla określonych warunków funkcji f
            for (int i = 0; i < amount; i++)
            {
                candidate = f(RandomNumber(interval[0], interval[1]), data);
                if (candidate < best) best = candidate;
            }
            return best;
        }

        public static bool StoppingCriterion(int iterator)
        {
            if (iterator < 1000) return false;
            else return true;
        }

        public static double InitialTemperature(double k)
        {
            return k;
        }

        public static int RandomNumber(int a, int b)
        {
            Random random = new Random();
            return random.Next(a, b);
        }

        public static double f(int x, List<Way> data)    // funkcja bazowa
        {
            int i = 0;
            while (x != data[i].index)
            {
                i++;
            }
            return data[i].worktime;
        }
    }
}
