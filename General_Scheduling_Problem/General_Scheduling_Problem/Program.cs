using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    class Program
    {
        // ******************************* General Scheduling Problem Components ******************************* \\
        public static char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
        public static string[] names = { "Van Nelle", "Aerzen Green", "Scrroge&Marley", "McLaren Technology", "Olisur Olive", "Green House", "Simon's Company", "Anchor Brewing", "Bang and Olufsen", "Lockheed Martin" };
        public static int[] Duration = { 6, 4, 5 };
        public static int N = Duration.Length;
        public static int K = 2;
        public static string animation = "";

        public class Task
        {
            public char job;
            public int duration;
            public Task(char _j, int _dr)
            {
                job = _j;
                duration = _dr;
            }
            public override string ToString()
            {
                return "Task: { Job: " + job + ", Duration: " + duration + " }";
            }
        }

        public class Way
        {
            public string combination;
            public int worktime;
            public int index;
            public string history;
            public Way(string _c, int _w, int _i, string _h)
            {
                combination = _c;
                worktime = _w;
                index = _i;
                history = _h;
            }
            public override string ToString()
            {
                return "Way: { Combination: " + combination + ", Worktime: " + worktime + ", Index: " + index + " }";
            }
        }

        public class Factory : Task
        {
            public string name;
            public bool isWorking;
            public int progress;
            public Factory(char _j, int _dr, string _n, bool _i, int _p) : base(_j, _dr)
            {
                name = _n;
                isWorking = _i;
                progress = _p;
            }
            public override string ToString()
            {
                if (isWorking == false)
                    return "Factory: {\n" +
                        "\tName: " + name + "\n" +
                        "\tState: IS NOT WORKING\n" +
                        "}";
                else
                    return "Factory: {\n" +
                        "\tName: " + name + "\n" +
                        "\tState: IS WORKING {\n" +
                        "\t\tJob: " + job + "\n" +
                        "\t\tProgress: " + progress + " / " + duration + "\n" +
                        "\t}\n}";
            }
        }

        public static string print(int day, List<Factory> listF)
        {
            string headline = "Day " + day + "\n";
            string result = "";
            for(int i=0; i<K; i++)
            {
                if(listF[i].isWorking == true)
                    result += " | " + listF[i].name + " Factory | \n\tJob: " + listF[i].job + " --- " + listF[i].progress + " / "
                        + listF[i].duration + " progress\n";
                else
                    result += " | " + listF[i].name + " Factory | \n\t--- WORK COMPLETE --- \n";
            }
            return headline + result + "\n";
        }

        public static int AssignDuration(string combination, List<Task> listT, int k, List<Factory> listF)
        {
            int day = 0;
            char[] works = combination.ToCharArray();
            Task work = new Task('X', -1);
            Factory factory = new Factory('X', -1, "Lazy Factory", false, -1);
            while (IsThereAnyWork(works))
            {
                work = PullWork(works, listT);
                works = works.Skip(1).ToArray();
                if (IsThereALazyFactory(listF))
                {
                    factory = GetFreeFactory(listF);
                    factory.job = work.job;
                    factory.duration = work.duration;
                    factory.progress = 0;
                    factory.isWorking = true;
                }
                else
                {
                    while(!CheckFactories(listF))
                    {
                        animation += print(day, listF);
                        Sunset(listF);
                        day++;
                    }
                    factory = GetFreeFactory(listF);
                    factory.job = work.job;
                    factory.duration = work.duration;
                    factory.progress = 0;
                    factory.isWorking = true;
                }
            }
            while (IsThereAnyLazyFactory(listF))
            {
                animation += print(day, listF);
                Sunset(listF);
                day++;
            }
            animation += print(day, listF);
            return day;
        }

        public static bool IsThereAnyLazyFactory(List<Factory> listF)
        {
            int numberOfWorkingFactories = K;
            for (int i = 0; i < K; i++)
            {
                if (listF[i].progress == listF[i].duration || listF[i].isWorking == false)
                {
                    listF[i].isWorking = false;
                    numberOfWorkingFactories--;
                }
            }
            if (numberOfWorkingFactories > 0)
                return true;
            else
                return false;
        }

        public static void Sunset(List<Factory> listF)
        {
            for (int i = 0; i < K; i++) listF[i].progress++;
        }

        public static bool CheckFactories(List<Factory> listF)
        {
            for(int i=0; i<K; i++)
            {
                if (listF[i].progress == listF[i].duration) listF[i].isWorking = false;
                if (listF[i].isWorking == false) return true;
            }
            return false;
        }

        public static Factory GetFreeFactory(List<Factory> listF)
        {
            int i = 0;
            while (listF[i].isWorking == true) i++;
            return listF[i];
        }

        public static bool IsThereALazyFactory(List<Factory> listF)
        {
            for(int i=0; i<K; i++)
            {
                if (listF[i].isWorking == false)
                    return true;
            }
            return false;
        }

        public static bool IsThereAnyWork(char[] works)
        {
            if (works.Length == 0) return false;
            else return true;
        }

        public static Task PullWork(char[] works, List<Task> listT)
        {
            char sign = works[0];
            int i = 0;
            Task work = new Task('X', -1);
            while (listT[i].job != sign) i++;
            work.job = listT[i].job;
            work.duration = listT[i].duration;
            return work;
        }

        public static List<string> Permute(List<Task> ListOfTasks, int l, int r)
        {
            List<string> list = new List<string>();
            string str = "";
            for (int i = 0; i < ListOfTasks.Count; i++)
            {
                str += ListOfTasks[i].job;
            }
            list = ToPermute(str, l, r, list);
            return list;
        }

        private static List<string> ToPermute(string str, int l, int r, List<string> list)
        {
            if (l == r)
                list.Add(str);
            else
            {
                for (int i = l; i <= r; i++)
                {
                    str = Swap(str, l, i);
                    ToPermute(str, l + 1, r, list);
                    str = Swap(str, l, i);
                }
            }
            return list;
        }
        public static string Swap(string a, int i, int j)
        {
            char temp;
            char[] charArray = a.ToCharArray();
            temp = charArray[i];
            charArray[i] = charArray[j];
            charArray[j] = temp;
            string s = new string(charArray);
            return s;
        }

        public static List<Factory> CreateFactories()
        {
            List<Factory> list = new List<Factory>();
            for(int i=0; i<K; i++)
            {
                string name = names[i];
                Factory tmp = new Factory('X', -1, name, false, -1);
                list.Add(tmp);
            }
            return list;
        }

        public static List<Task> CreateTasks()
        {
            List<Task> list = new List<Task>();
            for (int i = 0; i < N; i++)
            {
                char alf = alphabet[i];
                Task tmp = new Task(alf, Duration[i]);
                list.Add(tmp);
            }
            return list;
        }

        // ******************************* Simulated Annealing Components ******************************* \\
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

        static void Main(string[] args)
        {
            // ******************************* General Scheduling Problem Components ******************************* \\
            List<Task> ListOfTasks = new List<Task>();
            List<string> ListOfCombinations = new List<string>();
            List<Way> ListOfWays = new List<Way>();
            List<Way> BestWays = new List<Way>();
            List<Factory> ListOfFactories = new List<Factory>();
            ListOfTasks = CreateTasks();
            ListOfCombinations = Permute(ListOfTasks, 0, N - 1);
            ListOfFactories = CreateFactories();
            for (int j=0; j<ListOfCombinations.Count; j++)
            {
                Way tmp = new Way(ListOfCombinations[j], AssignDuration(ListOfCombinations[j], ListOfTasks, K, ListOfFactories), j, animation);
                animation = "";
                ListOfWays.Add(tmp);
            }

            // ******************************* Simulated Annealing Components ******************************* \\
            int[] range = { 0, ListOfWays.Count };
            double s0 = f(1, ListOfWays);
            double best_solution_s8;
            double solution_s;
            double incumbent_solution_s6;
            double T0 = InitialTemperature(1000);
            best_solution_s8 = incumbent_solution_s6 = s0;
            int i = 0;

            while (!StoppingCriterion(i))
            {
                solution_s = ExplorationCriterion(range, ListOfWays);
                if (AcceptanceCriterion(solution_s, best_solution_s8, T0))
                {
                    incumbent_solution_s6 = solution_s;
                    if (incumbent_solution_s6 < best_solution_s8)
                        best_solution_s8 = incumbent_solution_s6;
                }
                if (TemperatureLenght())
                    T0 = CoolingScheme(T0);
                T0 = ResetTemperature(T0);
                i++;
                //Console.WriteLine(best_solution_s8);
            }

            for(int j=0; j<ListOfWays.Count; j++)
                if (ListOfWays[j].worktime == (int)best_solution_s8) BestWays.Add(ListOfWays[j]);


            // ******************************* Output ******************************* \\
            Console.WriteLine("Your best ways of work:");
            for(int j=0; j<BestWays.Count; j++)
            {
                Console.WriteLine(BestWays[j]);
            }
            Console.WriteLine("\nWould you like to see all animations of the best ways of work? Please write 'yes' or 'no' in console.");
            string answer;
            answer = Console.ReadLine();
            if (answer == "yes" || answer == "y")
            {
                Console.WriteLine("Now you will see production process day by day for all ways of work.\n");
                for (int k=0; k<BestWays.Count; k++)
                {
                    Console.WriteLine("<--- Way: " + BestWays[k].combination + " --->");
                    string str = "";
                    char[] s = BestWays[k].history.ToArray();
                    char prev;
                    char next = s[0];
                    str += next;
                    int u = 0;
                    for (int j = 1; j < s.Length; j++)
                    {
                        prev = next;
                        next = s[j];
                        str += next;
                        if (prev == '\n' && next == '\n')
                        {
                            u++;
                            Console.WriteLine(str);
                            str = "";
                            Console.ReadKey();
                        }
                    }
                    Console.WriteLine("\n");
                }
            }
            Console.ReadKey();
        }
    }
}