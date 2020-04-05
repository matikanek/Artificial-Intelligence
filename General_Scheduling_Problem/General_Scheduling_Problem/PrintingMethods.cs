using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    class PrintingMethods
    {
        public static string print(int day, List<Factory> listF)
        {
            string headline = "Day " + day + "\n";
            string result = "";
            for (int i = 0; i < Parameters.K; i++)
            {
                if (listF[i].isWorking == true)
                    result += " | " + listF[i].name + " Factory | \n\tJob: " + listF[i].job + " --- " + listF[i].progress + " / "
                        + listF[i].duration + " progress\n";
                else
                    result += " | " + listF[i].name + " Factory | \n\t--- WORK COMPLETE --- \n";
            }
            return headline + result + "\n";
        }

        public static void PrintOutput(List<Way> BestWays)
        {
            Console.WriteLine("Your best ways of work:");
            for (int j = 0; j < BestWays.Count; j++)
            {
                Console.WriteLine(BestWays[j]);
            }
            Console.WriteLine("\nWould you like to see all animations of the best ways of work? Please write 'yes' or 'no' in console.");
            string answer;
            answer = Console.ReadLine();
            if (answer == "yes" || answer == "y")
            {
                Console.WriteLine("Now you will see production process day by day for all ways of work.\n");
                for (int k = 0; k < BestWays.Count; k++)
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
