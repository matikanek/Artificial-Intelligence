using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    class Program
    {
        public static char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
        public static string[] names = { "Van Nelle", "Aerzen Green", "Scrroge&Marley", "McLaren Technology", "Olisur Olive", "Green House", "Simon's Company", "Anchor Brewing", "Bang and Olufsen", "Lockheed Martin" };
        public static int[] Duration = { 6, 4, 5 };
        public static int N = 3;
        public static int K = 2;

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
            public Way(string _c, int _w, int _i)
            {
                combination = _c;
                worktime = _w;
                index = _i;
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
                Sunset(listF);
                day++;
            }
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

        static void Main(string[] args)
        {
            List<Task> ListOfTasks = new List<Task>();
            List<string> ListOfCombinations = new List<string>();
            List<Way> ListOfWays = new List<Way>();
            List<Factory> ListOfFactories = new List<Factory>();
            ListOfTasks = CreateTasks();
            ListOfCombinations = Permute(ListOfTasks, 0, N - 1);
            ListOfFactories = CreateFactories();
            for (int i=0; i<ListOfCombinations.Count; i++)
            {
                Way tmp = new Way(ListOfCombinations[i], AssignDuration(ListOfCombinations[i], ListOfTasks, K, ListOfFactories), i);
                ListOfWays.Add(tmp);
            }
            for(int i=0; i<ListOfCombinations.Count; i++)
            {
                Console.WriteLine(ListOfWays[i]);
            }
            

            Console.ReadKey();
        }
    }
}