﻿using System;
using System.Collections.Generic;
using System.Linq;
using Monografia.Funciones;
using Monografia.Metaheuristicas;
using Monografia.Metaheuristicas.Armonicos;

namespace Monografia
{
    class Program
    {
        static void Main(string[] args)
        {
                var myProblems = new List<p_mediana>{
<<<<<<< HEAD
                    new p_mediana("pmed2.txt"),
                    new p_mediana("pmed3.txt"),
=======
                new p_mediana("pmed1.txt"),
                new p_mediana("pmed2.txt"),
                new p_mediana("pmed3.txt"),
                new p_mediana("pmed4.txt"),
                new p_mediana("pmed5.txt"),
                new p_mediana("pmed6.txt"),
                new p_mediana("pmed7.txt"),
                new p_mediana("pmed8.txt"),
                new p_mediana("pmed9.txt"),
                new p_mediana("pmed10.txt"),
                new p_mediana("pmed11.txt"),
                new p_mediana("pmed12.txt"),
                new p_mediana("pmed13.txt"),
                new p_mediana("pmed14.txt"),
                new p_mediana("pmed15.txt"),
                new p_mediana("pmed16.txt"),
                new p_mediana("pmed17.txt"),
                new p_mediana("pmed18.txt"),
                new p_mediana("pmed19.txt"),
                new p_mediana("pmed20.txt"),
                new p_mediana("pmed21.txt"),
                new p_mediana("pmed22.txt"),
                new p_mediana("pmed23.txt"),
                new p_mediana("pmed24.txt"),
                new p_mediana("pmed25.txt"),
                new p_mediana("pmed26.txt"),
                new p_mediana("pmed27.txt"),
                new p_mediana("pmed28.txt"),
                new p_mediana("pmed29.txt"),
                new p_mediana("pmed30.txt"),
                new p_mediana("pmed31.txt"),
                new p_mediana("pmed32.txt"),
                new p_mediana("pmed33.txt"),
                new p_mediana("pmed34.txt"),
                new p_mediana("pmed35.txt"),
                new p_mediana("pmed36.txt"),
                new p_mediana("pmed37.txt"),
                new p_mediana("pmed38.txt"),
                new p_mediana("pmed39.txt"),
                new p_mediana("pmed40.txt"),
>>>>>>> 99325214d22eb306af7f8797bffdca898ebd64a8
                };
                var maxEFOS = 5000;
            var myAlgorithms = new List<Algorithm>{
                //new HSOS(),
                new SBHS(),
                };
                const int maxRep = 30;

                foreach (var theAlgorithm in myAlgorithms)
                {
                    Console.WriteLine($"{theAlgorithm,80}");
                    Console.Write($"{"Problem",-15} {"Items",6} {"Ideal",10} ");
                    Console.WriteLine($"{"Avg-Fitness",15} {"SD-Fitness",15} {"Avg-Efos",15} {"Success Rate",15} { "Time",15}");

                    foreach (var theProblem in myProblems)
                    {
                        Console.Write($"{theProblem.FileName,-15} {theProblem.totalAristas,6} {theProblem.OptimalLocation,10} ");

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
                            if (Math.Abs(theAlgorithm.BestSolution.Fitness - theProblem.OptimalLocation) < 1e-10)
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
