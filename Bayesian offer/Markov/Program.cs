using System;
using System.Linq;
using Microsoft.ML.Probabilistic;
using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Models;

namespace Markov
{
    class Program
    {
        public static int CountFlags(bool[] bArr, bool flag)
        {
            int result = 0;
            for(int i=0; i<bArr.Count(); i++)
            {
                if (bArr[i] == flag)
                    result++;
            }
            return result;
        }

        static void Main(string[] args)
        {
            bool[] projectionsArray = { true, false, false, true, false, false, true, true, false, true };
            double trueCount = CountFlags(projectionsArray, true);
            double falseCount = CountFlags(projectionsArray, false);
            double pOSuccess = trueCount / projectionsArray.Length;
            Variable<double> beta = Variable.Beta(trueCount, falseCount);
            Variable<int> binomial = Variable.Binomial(projectionsArray.Length, pOSuccess);
            InferenceEngine engine = new InferenceEngine();
            Console.WriteLine("The Beta distribution: " + engine.Infer(beta));
            Console.WriteLine("The Binomal distribution: " + engine.Infer(binomial));
            Console.ReadKey();

            // Notatki dotyczące kodu:
            /* 
             * Co należało zrobić?
             * Należało dokonać wnioskowania Bayesowskiego do oceny rozkładu prawdopodobieństwa sukcesu p rozkładu Bernoulliego 
             * (dwumianowego). Do implementacji należało użyć biblioteki Infer.NET. Jako rozkład a priori parametru p 
             * (czyli taki rozkład narzuczony z góry, z łaciny - uprzedzający fakty, z założenia) można przyjąć rozkład beta z paramertrami
             * alfa = beta = 1.
             * 
             * Zadanie zostało zrealizowane na przykładzie rzutu monetą mającą dwie możliwe opcje (orzeł i reszka). Rozszyfrujmy po pierwsze
             * co to znaczy, że rozkład a priori parametru p może przyjąć rozkład beta z parametrami alfa = beta = 1.
             * Przede wszystkim należy zauważyć, że mając do czynienia z rozkładem beta z parametrami alfa = beta = 1, mamy do czynienia
             * ze szczególnym przypadkiem. Możemy dowiedzieć się, że w takim przypadku rozkład beta przyjmuje postać tzw. rozkładu jednostajnego.
             * Z takimi parametrami można zauważyć, że wartość oczekiwana takiego rozkładu jest równa 0.5. Powstaje więc pytanie w jaki sposób 
             * można byłoby sprawić by na poziomie kodu tak się rzeczy miały. 
             * I teraz ja to rozumiem tak: rzucam sobie monetą i w tablicy "projectionsArray" zamieściłem odpowiednio wartości tych rzutów:
             * true - orzeł, false - reszka (lub na odwrót jak kto woli). W mojej tablicy specjalnie zdefiniowałem tą samą ilość
             * wylosowania orła jak i reszki, ponieważ licząc wartość oczekiwaną dla tego zestawu rzutów wyjdzie nam:
             * (1 * 5 + 0 * 5) / 10     (1 - umowna wartość orła, 0 - umowna wartość rezki, 10 - liczba rzutów monetą)
             * licząc powyższe równanie dochodzę do winiku 0.5, który wychodzi jako wartość oczekiwana w rozkładzie beta, kiedy parametry 
             * alfa = beta = 1. 
             * Wykorzystuję ten fakt i nastepnie zmienna pOSuccess (probability of succes) przyjmuje właśnie wartość 0.5 liczoną jako 
             * (liczba wyrzuconych orłów) / (liczbę rzutów). Jest to mało eleganckie rozwiązanie ponieważ mógłbym wydobywać tą wartość 
             * na poziomie Infer.NET dobierając się do niej przez engine.Infer(beta). Należałoby wówczas zamienić cały output 
             * tej operacji na stringa, odpowiednio go splitować i wartość numeryczną sparsować na double'a. Kosztowałoby to jednak dużo więcej
             * linijek kodu, które w takim podejściu jakie zaprezentowałem w kodzie byłyby zbędne. Oczywiście lepiej byłoby to tak zaprogramować
             * by sam kod więcej bazował na bibliotece Infer.NET niż na zwykłym C#.
             * 
             * Napiszę jeszcze kilka słów o samym rozkładzie beta, który w Infer.NET przyjmuje dwie wartości (liczbę sukcesów, liczbę porażek).
             * W tym celu na poziomie czytego C# napisałem krótką metodę bazującą na tablicy projectionsArray, która wylicza ile w tablicy 
             * występuje sukcesów a ile porażek. Zmienne trueCount i falseCount zawierają te informacje.
             * 
             * Sam rokzład dwumianowy (binomial) również przyjmuje dwie wartości (liczbę rzutów oraz liczbę sukcesów). W moim przypadku jest to 
             * oczywiście kolejno wielkość tablicy projectionsArray oraz wcześniej wspomiana zmienna pOSuccess.
             * 
             * Zmierzając już do końca programu...
             * Infer.NET korzysta z czegoś takiego co nazywa silnikiem. Należy utworzyć zmienną typu InferenceEngine by móc cieszyć się 
             * esencją tej biblioteki. Za pomocą tej zmiennej liczone są wszelkie interesujące nas dane. Postanowiłem wyświetlić rozkład beta
             * oraz rozkład dwumianowy. 
             * Na wyjściu jesteśmy w stanie zobaczyć średnią wartość oczekiwaną rozkładu beta (oczywiście 0.5) a następnie zobaczyć
             * dyskretny rozkład prawdopodobieństwa dla rozkłądu dwumianowego. Są to oczywiście wartości punktowe, które należałoby umieścić
             * na wykresie co stały interwał i zaobserwować efekt. Efektem będzie oczywiście piękny wykres punktowy przypominający kształtem
             * kształt funkcji rozkładu Gaussa. Nawet gdybyśmy nie mieli pewnosci co do parametru p to po rozrysowaniu sobie takiego wykresu
             * na podstwawie naszych danych wyjściowych moglibyśmy odetchnać z ulgą i zobaczyć, że nawet kształt wykresu świadczy o tym, że jest
             * to 0.5. Widzimy więc, że przyjął on wartość rozkładu beta, który sam był z parametrami alfa = beta = 1.
             */
        }
    }
}
