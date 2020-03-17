using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelling_salesman
{
    class Program
    {
        public static string[] alfabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "R", "S", "T", "U", "W", "X", "Y", "Z" };
        public static double[] X = { 1, 2, 4, 8 };
        public static double[] Y = { 4, 1, 3, 5 };

        public class Point
        {
            public string name;
            public double x;
            public double y;
            public Point(string _n, double _x, double _y)
            {
                name = _n;
                x = _x;
                y = _y;
            }
            public override string ToString()
            {
                return "Point " + name + ": [" + x + ", " + y + "]";
            }
        }

        public class Letter
        {
            public string name;
            public bool reserved;
            public Letter(string _n, bool _r)
            {
                name = _n;
                reserved = _r;
            }
            public override string ToString()
            {
                return name + " - " + reserved;
            }
        }

        private static List<string> Permute(string str, int l, int r, List<string> list)
        {
            if (l == r)
                list.Add(str);
            else
            {
                for (int i = l; i <= r; i++)
                {
                    str = Swap(str, l, i);
                    Permute(str, l + 1, r, list);
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

        public static List<Point> CreatePoints(int amount)
        {
            List<Point> ListOfPoints = new List<Point>();
            for (int i = 0; i < amount; i++)
            {
                string alf = alfabet[i];
                Point tmp = new Point(alf, X[i], Y[i]);
                ListOfPoints.Add(tmp);
            }
            return ListOfPoints;
        }

        public static double RandomNumber(double a, double b)
        {
            Random random = new Random();
            return a + (b - a) * random.NextDouble();
        }

        static void Main(string[] args)
        {
            int N = 4;
            string str = "";
            List<Point> ListOfPoints = new List<Point>();
            List<string> ListOfConbinationS = new List<string>();
            ListOfPoints = CreatePoints(N);
            for(int i=0; i<ListOfPoints.Count; i++)
            {
                str += ListOfPoints[i].name;
            }
            ListOfConbinationS = Permute(str, 0, N - 1, ListOfConbinationS);

            for (int k = 0; k < ListOfConbinationS.Count; k++)
            {
                Console.WriteLine(ListOfConbinationS[k]);
            }

            Console.ReadKey();
        }
    }
}
