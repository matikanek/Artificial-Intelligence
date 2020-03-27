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
        public static int[] Deadline = { 8, 4, 12 };
        public static int N = 3;

        public class Task
        {
            public char job;
            public float duration;
            public int dueDate;
            public Task(char _j, float _dr, int _dD)
            {
                job = _j;
                duration = _dr;
                dueDate = _dD;
            }
            public override string ToString()
            {
                return "Task: { Job: " + job + ", Duration: " + duration + ", Due Date: " + dueDate + " }";
            }
        }

        public static List<Task> CreateTasks()
        {
            List<Task> list = new List<Task>();
            for(int i = 0; i < N; i++)
            {
                char alf = alphabet[i];
                Task tmp = new Task(alf, Duration[i], Deadline[i]);
                list.Add(tmp);
            }
            return list;
        }

        static void Main(string[] args)
        {
            List<Task> ListOfTasks = new List<Task>();
            ListOfTasks = CreateTasks();
            for(int i = 0; i < N; i++)
            {
                Console.WriteLine(ListOfTasks[i]);
            }

            Console.ReadKey();
        }
    }
}
