using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Probabilistic.Algorithms;
using Microsoft.ML.Probabilistic.Distributions;
using Microsoft.ML.Probabilistic.Models;


namespace MicrosoftResearch.Infer.Tutorials
{
    public class FirstExample
    {
        public void Run()
        {
            Variable<bool> firstCoin = Variable.Bernoulli(0.5).Named("firstCoin");
            Variable<bool> secondCoin = Variable.Bernoulli(0.5).Named("secondCoin");
            Variable<bool> bothHeads = (firstCoin & secondCoin).Named("bothHeads");
            InferenceEngine engine = new InferenceEngine();
            Console.WriteLine("Probability both coins are heads: " + engine.Infer(bothHeads));

            if (!(engine.Algorithm is VariationalMessagePassing))
            {
                Console.WriteLine("Probability both coins are heads: " + engine.Infer(bothHeads));
                bothHeads.ObservedValue = false;
                Console.WriteLine("Probability distribution over firstCoin: " + engine.Infer(firstCoin));
            }
            else
                Console.WriteLine("This example does not run with Variational Message Passing");
        }
        //Notatki
        /* Zmienna losowa z klasy Variable może być dowolnego typu. W przypadku chęci reprezentacji rzutów monetą można posłużyć  
         * zmienną typu bool. Zmienna bothHeads zbiera dwie monety i ustala za pomocą silnika engine prawdopodobieństwo losu reszki.
         * engine.Infer() jet dobrą metodą do określania tego prawdopodobieństwa. mogę zbadać w tej metodzie dowolną zmienną losową.
         */
    }
}

namespace TwoCoins
{
    class Program
    {
        static void Main(string[] args)
        {
            MicrosoftResearch.Infer.Tutorials.FirstExample coins = new MicrosoftResearch.Infer.Tutorials.FirstExample();
            coins.Run();
            Console.ReadKey();
        }
    }
}
