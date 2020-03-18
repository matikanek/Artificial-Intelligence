using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelling_salesman
{
    class Program
    {
        public static char[] alfabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
        public static double[] X = { 1, 2, 4, 8 };
        public static double[] Y = { 4, 1, 3, 5 };

        public class Track
        {
            public string combination;
            public double value;
            public int index;
            public Track(string _c, double _v ,int _i)
            {
                combination = _c;
                value = _v;
                index = _i;
            }
            public override string ToString()
            {
                return "Track " + combination + ", distance: " + value + ", index: " + index;
            }
        }

        public class Point
        {
            public char name;
            public double x;
            public double y;
            public Point(char _n, double _x, double _y)
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

        public static double Pythagoras(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.y - a.y, 2));
        }

        public static Point Search(char c, List<Point> points)
        {
            int i = 0;
            while (c != points[i].name) i++;
            return points[i];
        }

        public static double CountLengthOfTrack(string combination, List<Point> points)
        {
            double result = 0;
            char prev = combination[0];
            char next = combination[1];
            Point dataPrev = Search(prev, points);
            Point dataNext = Search(next, points);
            for (int i=2; i < combination.Length; i++)
            {
                result += Pythagoras(dataPrev, dataNext);
                prev = next;
                next = combination[i];
                dataPrev = Search(prev, points);
                dataNext = Search(next, points);
            }
            result += Pythagoras(dataPrev, dataNext);
            return result;
        }

        public static List<Track> AssignLengthOfTracks(List<string> ListOfCombinations, List<Point> ListOfPoints)
        {
            List<Track> list = new List<Track>();
            for(int i=0; i < ListOfCombinations.Count; i++)
            {
                double tmp = CountLengthOfTrack(ListOfCombinations[i], ListOfPoints);
                list.Add(new Track(ListOfCombinations[i], tmp, i));
            }
            return list;
        }

        public static List<string> Permute(List<Point> ListOfPoints, int l, int r)
        {
            List<string> list = new List<string>();
            string str = "";
            for (int i = 0; i < ListOfPoints.Count; i++)
            {
                str += ListOfPoints[i].name;
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

        public static List<Point> CreatePoints(int amount)
        {
            List<Point> ListOfPoints = new List<Point>();
            for (int i = 0; i < amount; i++)
            {
                char alf = alfabet[i];
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
            List<Point> ListOfPoints = new List<Point>();
            List<string> ListOfConbinations = new List<string>();
            List<Track> ListOfTracks = new List<Track>();
            ListOfPoints = CreatePoints(N);
            ListOfConbinations = Permute(ListOfPoints, 0, N - 1);
            ListOfTracks = AssignLengthOfTracks(ListOfConbinations, ListOfPoints);

            for(int i=0; i < ListOfTracks.Count; i++)
            {
                Console.WriteLine(ListOfTracks[i]);
            }

            Console.ReadKey();
        }
    }
}
