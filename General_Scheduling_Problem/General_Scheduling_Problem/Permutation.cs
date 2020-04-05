using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    class Permutation
    {
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
    }
}
