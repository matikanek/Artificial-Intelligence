using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    class Parameters
    {
        public static char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K',
            'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
        public static string[] names = { "Van Nelle", "Aerzen Green", "Scrroge&Marley", "McLaren Technology",
            "Olisur Olive", "Green House", "Simon's Company", "Anchor Brewing", "Bang and Olufsen", "Lockheed Martin" };
        public static int[] Duration = { 6, 4, 5 };
        public static int N = Duration.Length;
        public static int K = 2;
        public static string animation = "";
    }
}
