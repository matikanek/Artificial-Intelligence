using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simulated_annealing
{
    class Program
    {
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

        public static double ExplorationCriterion(double[] interval)
        {
            int amount = 10;
            double candidate;
            double best = f(1); // Uwaga, wartość ta jest dobra tylko dla określonych warunków funkcji f
            for (int i = 0; i < amount; i++)
            {
                candidate = f(RandomNumber(interval[0], interval[1]));
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

        public static double RandomNumber(double a, double b)
        {
            Random random = new Random();
            return a + (b - a) * random.NextDouble();
        }

        public static double f(double x)    // funkcja bazowa
        {
            //return Math.Pow(x, 5) - 3 * Math.Pow(x, 4) + 3 * Math.Pow(x, 2) + 0.5 * x + 1.25;
            return x * x;
        }

        static void Main(string[] args)
        {
            double[] range = { -1, 3 };
            double s0 = f(1);   // Uwaga, wartość ta jest dobra tylko dla określonych warunków funkcji f
            double best_solution_s8;
            double solution_s;
            double incumbent_solution_s6;
            double T0 = InitialTemperature(1000);
            best_solution_s8 = incumbent_solution_s6 = s0;
            int i = 0;

            while (!StoppingCriterion(i))
            {
                solution_s = ExplorationCriterion(range);
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

            //Console.WriteLine(best_solution_s8);
            Console.ReadKey();
        }

        // Notatki:
        /*
         * Na wstępie chciałbym podzielić się obserwacją, że C# nie jest dobrym językiem do programowania metod numerycznych.
         * Ze względu na swoją budowę okazuje się, że czas wykonywania programu negatywnie wpływa na wartości losowe losowane w jego trakcie.
         * Wystarczy usunąć linijkę "Console.WriteLine(best_solution_s8)" z pętli programu by przekonać się, że program zwraca głupoty jeżeli 
         * nie da sobie czasu na wypisanie pośrednich wartości poszukiwań najlepszego rozwiązania. Wartości losowe są w praktyce wartościami pseudo 
         * losowymi, ponieważ są losowane ze względu na czas. W języku C zasiewa się tak swane "ziarno" i korzysta z biblioteki "c.time"
         * C# natomiast ma wbudowaną zmienną Random, która widocznie działa na innych zasadach niż w C a przez nią generwoany jest błąd jaki
         * nie miałby miejsca w języku C przy takim samym podejściu programistycznym jak w powyższym kodzie.
         * 
         * NOTATKI DOTYCZĄCE KODU:
         * Algorytm symulowanego wyżarzania (simulated annealing) opiera się na ściśle określonym podejściu i kluczowych metodach (komponentach) 
         * w trakcie swojej pracy.
         * Metody te są następujące:
         *  InitialTemperature      - wybór temperatury początkowej
         *  StoppingCriterion       - kryterium zatrzymania głównej pętli programu
         *  ExplorationCriterion    - kryterium eksploracji, które wybiera rozwiązanie w sąsiedztwie
         *  AcceptanceCriterion     - kryterium akceptacji, które określa czy nowe rozwiązanie ma zastąpić dotychczasowe
         *  TemperatureLength       - która wskazuje czy temperatura ma zostać zaktualizowana
         *  CoolingScheme           - schemat chłodzenia, który aktualizuje temperaturę (obniża jej wartość)
         *  TemperatureRestart      - komponent odpowiedzialny za resetowanie temperatury do pierwotnej wartości lub innej wysokiej wartości
         *  
         * Każdy z tych komponentów jest możliwy do zrealizowania na wiele różnych sposobów. Istnieje wiele podejść do każdego z nich.
         * Wiele z nich działa dobrze z wybranymi metodami z innych komponentów a niektóre idealnie pasują do wyboru innych metod dla innych 
         * komponentów. Jednym słowem algorytm symulowanego wyżarzania jest jak czysta orkiestra symfoniczna, która skada się z wielu
         * instrumentów a dyrygentem, który decyduje o odgrywanej sztuce (jej głośności, harmonii, tonie i doborze instrumentów grających 
         * akurat w jednym czasie) jest programista. Programista może okazać się dyrygentem, który zaprezentuje publiczności melodię "wlazł kotek na płotek"
         * dobierając najprostsze do implementacji metody lub zaprezentuje piękny utwór symfoniczny jednego ze znanych światowych wirtuozów
         * kreatywnie tworząc dobrze współpracujące ze sobą komponenty. Zatęm zagłębiając się w nuty mojego wykonania algorytmu symulowanego 
         * wyżarzania omówię pokrótce ich działanie.:
         * 
         * 1) InitialTemperature
         * Zgodnie z artykułem naliczyłem się aż 9 różnorodnych podejść do zainicjowania początkowej temperatury. Najprostszym z nich jest
         * podanie na sztywno wartości początkowej. Symbolicznie stworzyłem metodę, która przypisuje dokonuje tej operacji.
         * Można by było zrobić to w jednej linijce przypisując od razu do zmiennej T0 wartosć liczbową, jednak ze względu na określony schemat
         * algorytmu chciałem wyodrębnić ten zabieg w osobnej metodzie, która w przyszłości mogłaby być zmodyfikowana. 
         * 
         * 2) StoppingCriterion
         * I poniwnie istnieje wiele możliwości określenia zatrzymania działania pętli. Wiele podejść wymaga bieżącego monitorowania wartości
         * innych zmiennych. W moim podejściu wybrałem zatrzymanie pętli po osiągnięciu tak zwanej maksymalnej ilości czasu (ustaliłem 1000).
         * 
         * 3) ExplorationCriterion
         * Jest to jeden z kluczowych komponentów działania algorytmu. Najprostszą metodą jest wylosowanie na danym przedziale sąsiedztwa
         * wartości argumentu i zwrócenia jego wartości. Jednakże mr. Connolly w 1990 roku zauważył nieefektywność takiego podejścia
         * ponieważ dla niższych temperatur potencjalne ulepszenia mogłyby zostać pominięte. Zaproponował więc szukanie potencjalnego
         * rozwiązania za pomocą pewnych ściśle określonych sekwencji. Dopiero 1995 roku Ishibuchi postanowił zmodernizować najprostszą metodę
         * losowania wartości o prosty zabieg - wylosowaniu pewnej liczby k wartości i znalezieniu najlepszej z nich. W moim programie
         * postanowiłem pójść w ślady Ishibuchi-ego, co można zobaczyć zaglądając w kod dla tego komponentu.
         * 
         * 4) AcceptanceCritierion
         * W mojej ocenie jest to drugi z kluczowych komponentów (jeżeli by nie powiedzieć, że najważniejszy) działania algorytmu.
         * Mając do dyspozycji gamę podejść do tego komponentu wybrałem tak zwany "Metropolis-based criteria".
         * Jest to bardzo sprytna metoda. Poroszę zauważyć, że jeżeli dostanę lepszą wartość funkcji (actualSol) od mojej dotychczasowo 
         * najlepszej (bestSol) to bez chwili wachania mówię "spoko, zwróć pozwolenie (true)" ale jeżeli nie dostanę lepszej wartości (więc gorszą) 
         * to algorytm mówi "no okej, ale może w innych rejonach znajdę się w pobiżu globalnie najlepszej wartości. 
         * I teraz cały spryt polega na tym, że funkcja Exp(-x) dla wzrostu argumentu daje coraz mniejsze wartości. Mamy tu do czynienia 
         * z prawdopodobieństwem. Zatem z meleniem temperatury argument "x" będzie coraz większy - co za tym idzie pod koniec działania 
         * algorytmu coraz rzadziej napotkamy tak "ryzykowny skok" w inne regiony funkcji na zadanym przedziale. 
         * 
         * 5) TemperatureLength 
         * Niektórzy programiści algorytmu symulowanego wyżarzania aktualizują temperaturę po określonej liczbie zaakceptowanych ruchów.
         * Dlatego zapytanie o możliwość zmiany temperatury ma sens. Z kolei nie ma jednak sensu w podejściu innych programistów (w tym i moim), 
         * ponieważ niektórzy łączą to podejście z maksymalną liczbą całkowitych ruchów (StoppingCriterion). Mój program zakłada limit 
         * ruchów więc zakładam, że zmiana temperatury nastąpi zawsze. Stąd moja metoda zwraca tylko wartość true. W związku z powyższym metoda ta 
         * jest zbędna w moim programie, jednak ze względu na chęć zachowania przeze mnie pewnego konwenansu struktury algorytmu opisanego 
         * w artykule chciałbym podkreślić jej istnienie.
         * 
         * 6) CoolingScheme 
         * Metoda ta ochładza temperaturę. Poniwnie można zrobić to na wiele sposobów. Ja wybrałem przemnożenie aktualnej wartości temperatury
         * przez stałą liczbę 'alfa' należącą do zbioru (0,1). Policzona w ten sposób wartość staje się nową wartością temperatury.
         * Obniżenie temperatury pozwala na coraz mniejsze prawdopodobieństwo skoku w inne regiony funkcji, gdzie może istnieć globalne
         * minimum lub maksimu w zależności od tego jakiego rodzaju ekstremum poszukujemy.
         * 
         * 7) TemperatureRestart
         * Metoda ta również posiada wiele możliwości podejścia. Tak wiele, że ktoś postanowił bezczynność tej metody już nazwać metodą :)
         * Zadowolony tą możliwością skorzystałem z tego, więc zwracaną wartością jest po prostu otrzymana wartość temperatury.
         * Warto jednak powiedzieć trochę o tym jaką możliwość daje ta metoda. 
         * Potocznie mówiąc pozwala ona na nie utknięcie w jakimś extremum lokalnym ciesząc się, że pozornie zbliżamy się do rozwiązania
         * jednak nie zdając sobie z tego sprawy, że lokalne rozwiązanie jakie staramy się wydobyć nie jest tym globalnym, którego poszukujemy
         * dla danej funkcji. Komponent ten pozwala na podwyższenie temperatury określane jako tzw. "dogrzanie temperatury", które umozliwia nam 
         * ucieczkę z regionu gdzie pozornie myśleliśmy, że jest globalne rozwiązanie i szansę na znalezienie tego właściwego.
         * 
         * 
         * Zadanie domowe dotyczyło implementacji algorytmu symulowanego wyżarzania dla funkcji f = x*x na przedziale [-1, 1]. Jednak
         * funkcja ta posiada tylko jedno minimum lokalne, które jest zarazem globalnym rozwiązaniem dlatego też zdecydowałem się by 
         * metoda TemperatureRestart praktycznie nie istaniała bo nie miałaby takiej potrzeby. 
         * Program mój można jednak przetestować dla funkcji, którą zostawiłem w komentarzu w metodzie "f", która posiada dwa minima lokalne.
         * Najmniejsza wartość minimum lokalnego dla tej funckji jest równa około -2. Należy podać tylko do zmiennej "range" przedział [-1, 3].
         * 
         * Wskazówka:
         * Jeżeli chciałbym znaleźć maksimum globalne dla podanej funkcji wykorzystując ten program, należy zmienić znak z "<" na ">"
         * - w zapytaniu: if (incumbent_solution_s6 < best_solution_s8) w głównej pętli programu
         * - w zapytaniu: if (candidate < best)  w metodzie ExplorationCriterion
         * oraz ze znaku ">=" na "<="
         * - w zapytaniu: if (diff >= 0) w metodzie AcceptanceCriterion
         */
    }
}
