using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace travelling_salesman
{
    class Program
    {
        // ******************************* Travelling Salesman Components ******************************* \\
        public static char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'W', 'X', 'Y', 'Z' };
        public static double[] X = { 9, 4, 3, 1, 8 };
        public static double[] Y = { 1, 5, 2, 8, 7 };
        public static int N = 5;
        public static string BEST_PATH = "";

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
                char alf = alphabet[i];
                Point tmp = new Point(alf, X[i], Y[i]);
                BEST_PATH += tmp.name;
                ListOfPoints.Add(tmp);
            }
            return ListOfPoints;
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

        public static double ExplorationCriterion(int[] interval, List<Track> data)
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

        public static double f(int x, List<Track> data)    // funkcja bazowa
        {
            int i = 0;
            while (x != data[i].index)
            {
                i++;
            }
            return data[i].value;
        }

        static void Main(string[] args)
        {
            // ******************************* Travelling Salesman Components ******************************* \\
            List<Point> ListOfPoints = new List<Point>();
            List<string> ListOfConbinations = new List<string>();
            List<Track> ListOfTracks = new List<Track>();
            ListOfPoints = CreatePoints(N);
            ListOfConbinations = Permute(ListOfPoints, 0, N - 1);
            ListOfTracks = AssignLengthOfTracks(ListOfConbinations, ListOfPoints);


            // ******************************* Simulated Annealing Components ******************************* \\
            int[] range = { 0, 10 };
            double s0 = f(1, ListOfTracks);   // Uwaga, wartość ta jest dobra tylko dla określonych warunków funkcji f
            double best_solution_s8;
            double solution_s;
            double incumbent_solution_s6;
            double T0 = InitialTemperature(1000);
            best_solution_s8 = incumbent_solution_s6 = s0;
            int i = 0;

            while (!StoppingCriterion(i))
            {
                solution_s = ExplorationCriterion(range, ListOfTracks);
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
                Console.WriteLine(best_solution_s8);
            }

            int j = 0;
            while (best_solution_s8 != ListOfTracks[j].value)
            {
                BEST_PATH = ListOfTracks[j].combination;
                j++;
            }
            BEST_PATH = ListOfTracks[j].combination;
            Console.WriteLine(BEST_PATH);

            Console.ReadKey();

            // Notatki
            /*
             * Problematyka programu: 
             * Za pomocą algorytmu symulowanego wyżarzania rozwiązać problem tzw. podrózującego sprzedawcy (Travelling Salesman).
             * Problem podróżującego sprzedawcy można opisać w następujący sposób:
             * Sprzedawca chce sprzedać swoje towary w wybranych przez siebie miastach. Chcąc nie przemęczać swoich nóg pragnie znaleźć 
             * najkrótszą trasę, która umożliwi mu wizytę we wszystkich miastach chodząc bezpośrednio od jednego do drugiego. 
             * W ten sposób sprzedawca chce zaliczyć każde miasto najmniejszym kosztem dystansu podróży.
             * 
             * NOTATKI DOTYCZĄCE KODU:
             * Program w swojej strukturze dzieli się na dwie części: 
             *  - Część I:  Przygotowanie danych do rozpoczęcia działania algorytmu symulowanego wyżarzania
             *  - Część II: Poszukiwanie rozwiązania za pomocą algorytmu symulowanego wyżarzania
             * 
             * Powołując się na poprzedni program dotyczący implementacji samego algorytmu symulowanego wyżarzania pokrótce tylko wytłumaczę 
             * jego działanie, ponieważ sposób jego działania nie uległ większym modyfikacjom.
             * 
             * 1) Część I - Przygotowanie danych:
             * Komponenty należące do tej części są oddzielone odpowiednim komentarzem. 
             * Moje podejście sprowadzało się do kilku etapów by przygotować odpowiednie dane dla drugiej części programu.
             * Potrzebne składniki:
             *  A) Lista punktów, które będą reprezentować miasta (wyznaczane na sztywno przez programiste)
             *      Aa) Nazwy punktów (tablica znaków przechowująca listę nazw - zmienna statyczna alphabet)
             *      Ab) Zmienna przechowująca ich ilość - zmienna statyczna N
             *      Ac) Współrzędne punktów (tablice zmiennych x-owych i y-owych - zmienne statyczne X i Y)    
             *  B) Lista możliwych kombinacji (permutacji) danych punktów
             *  C) Lista możliwych ścieżek wyznaczana na podstawie listy permutacji
             *  
             * Sposób realizacji i idea:
             * Wszystko sprowadza się do omówienia kodu części pierwszej z funkcji "main".:
             *  
             *  (1) -- List<Point> ListOfPoints = new List<Point>();
             *  (2) -- List<string> ListOfConbinations = new List<string>();
             *  (3) -- List<Track> ListOfTracks = new List<Track>();
             *  (4) -- ListOfPoints = CreatePoints(N);
             *  (5) -- ListOfConbinations = Permute(ListOfPoints, 0, N - 1);
             *  (6) -- ListOfTracks = AssignLengthOfTracks(ListOfConbinations, ListOfPoints);
             *  
             * (1) ListOfPoints:
             * Lista punktów (miast). Do tego celu stworzyłem klasę Punkt, która zawiera elementy: nazwa punktu, współrzędna x, współrzędna y
             * Lista punktów tworzona jest za pomocą metody CreatePoints przyjmującej tylko informację o ich ilości.
             * 
             * (2) ListOfCombinations:
             * Lista permutacji danych punktów. Chodzi o to, że na jej wyjściu chciałbym mieć ładną tablicę wszystkich możliwych permutacji
             * zależną od ilości punktów, które podam. Lista permutacji tworzona jest za pomocą metody Permute przyjmującej informacje o:
             * liście punktów (zmienna ListOfPoints), punkcie, od którego chciałbym rozpatrywać permutacje i punkcie na jakim chciałbym skończyć.
             * Dlaczego nie samo "N"? - ponieważ tablice iteruje się od 0 do N - 1 dla N elementów. Sama funkcje Permute korzysta jeszcze
             * z kilku innych pomocnych jej funkcji.
             * 
             * (3) ListOfTracks:
             * Lista ścieżek dla zadanych permutacji. Wszystko po to by otrzymać na wyjściu ładny zbiór elementów np. dla Punktów ABCD:
             *  Track ABCD, distance: 10.05237, index: 0
             *  Track ABDC, distance: 12.87348, index: 1
             *  Track ACBD, distance: 11.77288, index: 2
             *  Track ACDB, distance: 11.56621, index: 3
             *      ...
             * Takie przedstawienie danych, dałoby możliwość pracy algorytmowi symulowanego wyżarzania. Prosze zobaczyć, że na podstawie tych
             * danych mogę utowrzyć funkcję, której argumenty należą do zbioru liczb naturalnych, a każdy z nich ma odpowiednią wartość.
             * Jedyne co trzeba zrobić to zmodyfikować algorytm symulowanego wyżarzania o losowanie punktów, które nie będą już losowane
             * ze zbioru liczb rzeczywistych tylko ze zbioru liczb całkowitych. Ponadto funkca f musiałaby tylko zwracać odpowiednią wartość
             * "distance" dla zadanego indeksu. Idea wydaje się piękna (i mam nadzieję, że tak ten problem należało mniej więcej rozwiązać)
             * ale jak to zostało zrealizowane?
             * Stworzyłem klasę, która zawiera 3 wyżej wymienione komponenty: kombinację, wartość (dystans) i indeks. Na podstawie metody
             * AssignLengthOfTracks generuję te dane by móc później je w taki sposób wyświetlić jak rozpisałem kilka przykładowych wierszy.
             * Metoda AssignLengthOfTracks w pętli przydziela zmiennej combination kolejne wartości zmiennej ListOfCombination. Czyli np.:
             * ABCD, ABDC ... . Podobnie zmienna index w przebiegu pętli po prostu zbiera wartości iterowanego "i". Natomiast zmienną value, 
             * która odpowiada za dystans trzeba już wyznaczyć osobnym algorytmem. W tym celu potrzebowałem funkcji CountLengthOfTrack, która 
             * przy zadanej kombinacji liczy dystanse pomiędzy kolejnymi dwoma punktami a następnie dodaje te dystanse do zmiennej result 
             * zwracanej jako wynik końcowy tej funkcji. W ten sposób otrzymuje sumę dystansów między kolejnymi punktami. 
             * Oczywiście by policzyć odległość między dwoma punktami potrzebne jest twierdzenie Pitagorasa, dlatego 
             * funkcja ta korzysta z jeszcze jednej osobnej funckji liczącej odległość pomiędzy dwoma punktami. 
             * W ten sposób uzyskałem wartości ListOfTracks.
             * 
             * 2) Część II - Simulated Annealing
             * Algorytm ten musi mieć jakąś funkcję na której może bazować. Tą funkcją stają się w pewnym sensie wartości, które można otrzymać 
             * ze zmiennej ListOfTracks. Jak już wspomniałem wcześniej należy tylko losować liczby całkowite zamiast rzeczywistych, ponieważ tylko 
             * one należą do dziedziny nowej funkcji. W ten sposób nie tylko na wyściu programu znam wartość najkrótszego dystansu między wszystkimi
             * miastami ale znam również całą trasę dzieki zmiennej combination.
             * 
             * 
             * 
             * Powyższy program bazuje sztywno na wymyślonych przeze mnie danych - Zbiorze punktów ABCDE. 
             * Po rospisaniu na kartce papieru podanych przeze mnie punktów, można przekonać się że generowana przez program najkrótsza 
             * ścieżka pomiędzy danymi punktami rzeczywiście wygląda na optymalną. Można zmodyfikować program zalecając się do poniższego
             * podpunktu "Wskazówka" by znaleźć najdłuższą ścieżkę i również można zobaczyć na kartce papieru obliczony rezultat. 
             * W przypadku chęci zmiany danych należy zmodyfikować zmienne statyczne na samej górze programu: X, Y, N uważając na wpisywane 
             * dane modyfikując względem nich zmienną range z głównej funkcji main programu. Zmienna range określa granice argumentów funkcji.
             * Jeżeli nowo podany punkt będzie leżał poza tymi granicami, program może zwrócić błędny wynik.
             * 
             * Wskazówka:
             * Jeżeli chciałbym wyznaczyć najdłuższą ścieżkę na podstawie tych samych danych należy dokonać takich samych zmian, które 
             * wymieniłem w poprzednim programie dotyczącym prostej implementacji algorytmu symulowanego wyżarzania.
             */
        }
    }
}
