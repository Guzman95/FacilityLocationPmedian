using System;
using System.Collections.Generic;
using System.Linq;
using Monografia.Funciones;
using Monografia.Metaheuristicas;

namespace Monografia
{
    class Program
    {
        static void Main(string[] args)
        {
            static void Main(string[] args)
            {
                var myProblems = new List<Knapsack>
            {
                new Knapsack("f1.txt"),
                new Knapsack("f2.txt"),
                new Knapsack("f3.txt"),
                new Knapsack("f4.txt"),
                new Knapsack("f5.txt"),
                new Knapsack("f6.txt"),
                new Knapsack("f7.txt"),
                new Knapsack("f8.txt"),
                new Knapsack("f9.txt"),
                new Knapsack("f10.txt"),
                new Knapsack("Knapsack1.txt"),
                new Knapsack("Knapsack2.txt"),
                new Knapsack("Knapsack3.txt"),
                new Knapsack("Knapsack4.txt"),
                new Knapsack("Knapsack5.txt"),
                new Knapsack("Knapsack6.txt")
            };

                var maxEFOS = 5000;
                var myAlgorithms = new List<Algorithm>
            {
            };

                const int maxRep = 30;

                foreach (var theAlgorithm in myAlgorithms)
                {
                    Console.WriteLine($"{theAlgorithm,80}");
                    Console.Write($"{"Problem",-15} {"Items",6} {"Ideal",10} ");
                    Console.WriteLine($"{"Avg-Fitness",15} {"SD-Fitness",15} {"Avg-Efos",15} {"Success Rate",15} { "Time",15}");

                    foreach (var theProblem in myProblems)
                    {
                        Console.Write($"{theProblem.FileName,-15} {theProblem.TotalItems,6} {theProblem.OptimalKnown,10} ");

                        var efos = new List<int>();
                        var mediaF = new List<double>();
                        var times = new List<double>();
                        var succesRate = 0;
                        for (var rep = 0; rep < maxRep; rep++)
                        {
                            var myRandom = new Random(rep);
                            var timeBegin = DateTime.Now;
                            theAlgorithm.Ejecutar(theProblem, myRandom);

                            times.Add((DateTime.Now - timeBegin).TotalSeconds);
                            mediaF.Add(theAlgorithm.BestSolution.Fitness);
                            efos.Add(theAlgorithm.EFOs);
                            if (Math.Abs(theAlgorithm.BestSolution.Fitness - theProblem.OptimalKnown) < 1e-10)
                                succesRate++;
                        }

                        var avg = mediaF.Average();
                        Console.Write($"{avg,15:0.000} ");
                        var deviation = mediaF.Sum(d => (d - avg) * (d - avg));
                        deviation = Math.Sqrt(deviation / maxRep);
                        Console.Write($"{deviation,15:0.000} ");
                        Console.Write($"{efos.Average(),15:0.000} ");
                        Console.Write($"{succesRate * 100.0 / maxRep,15:0.00}% ");
                        Console.WriteLine($"{times.Average(),15:0.000000} ");
                    }
                }

                Console.ReadKey();
            }
        }
    }
}
