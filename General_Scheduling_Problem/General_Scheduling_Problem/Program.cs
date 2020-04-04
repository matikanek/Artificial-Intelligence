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
        public static float[] Duration = { 6, 4, 5 };
        public static int N = 3;
        public static int K = 2;

        public class Task
        {
            public char job;
            public float duration;
            public Task(char _j, float _dr)
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
            public float worktime;
            public int index;
            public Way(string _c, float _w, int _i)
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

        // Tutaj rozpocznie się jazda
        public static float AssignDuration(string combination, List<Task> list, int k)
        {
            return 0;
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
            ListOfTasks = CreateTasks();
            ListOfCombinations = Permute(ListOfTasks, 0, N - 1);
            for(int i=0; i<ListOfCombinations.Count; i++)
            {
                Way tmp = new Way(ListOfCombinations[i], AssignDuration(ListOfCombinations[i], ListOfTasks, K), i);
                ListOfWays.Add(tmp);
            }
            

            Console.ReadKey();
        }
    }
}