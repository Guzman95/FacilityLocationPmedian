using System;
using System.Collections.Generic;
using System.Linq;
using BasedOnHarmony.Funciones;
using BasedOnHarmony.Metaheuristicas;
using BasedOnHarmony.Metaheuristicas.Armonicos;

namespace BasedOnHarmony
{
    class Program
    {
        static void Main(string[] args)
        {
            const int maxEFOS = 1000;
            const int maxRep = 3;
            Console.WriteLine("Iniciando......");
            Console.WriteLine("Cargando Archivos de problemas.....");
            var myProblems = new List<PMediana>{
                new PMediana("pmed1.txt"),
                new PMediana("pmed2.txt"),
                new PMediana("pmed3.txt"),
                new PMediana("pmed4.txt"),
                new PMediana("pmed5.txt"),
              /*  new PMediana("pmed6.txt"),
                new PMediana("pmed7.txt"),
                new PMediana("pmed8.txt"),
                new PMediana("pmed9.txt"),
                new PMediana("pmed10.txt"),
                new PMediana("pmed11.txt"),
                new PMediana("pmed12.txt"),
                new PMediana("pmed13.txt"),
                new PMediana("pmed14.txt"),
                new PMediana("pmed15.txt"),
                new PMediana("pmed16.txt"),
                new PMediana("pmed17.txt"),
                new PMediana("pmed18.txt"),
                new PMediana("pmed19.txt"),
                new PMediana("pmed20.txt"),
                new PMediana("pmed21.txt"),
                new PMediana("pmed22.txt"),
                new PMediana("pmed23.txt"),
                new PMediana("pmed24.txt"),
                new PMediana("pmed25.txt"),
                new PMediana("pmed26.txt"),
                new PMediana("pmed27.txt"),
                new PMediana("pmed28.txt"),
                new PMediana("pmed29.txt"),
                new PMediana("pmed30.txt"),
                new PMediana("pmed31.txt"),
                new PMediana("pmed32.txt"),
                new PMediana("pmed33.txt"),
                new PMediana("pmed34.txt"),
                new PMediana("pmed35.txt"),
                new PMediana("pmed36.txt"),
                new PMediana("pmed37.txt"),
                new PMediana("pmed38.txt"),
                new PMediana("pmed39.txt"),
                new PMediana("pmed40.txt"), */
                };
            var myAlgorithms = new List<Algorithm>{
                //new HSOS(){ MaxEFOs=maxEFOS},
                new SBHS() { MaxEFOs=maxEFOS},
                };
            Console.WriteLine("Ejecutando Algoritmos.....");
            foreach (var theAlgorithm in myAlgorithms)
            {
                Console.WriteLine($"{theAlgorithm,80}");
                Console.Write($"{"Problem",-15} {"Items",6} {"Ideal",10} ");
                Console.WriteLine($"{"Avg-Fitness",15} {"SD-Fitness",15} {"Avg-Efos",15} {"Success Rate",15} { "Time",15}");

                foreach (var theProblem in myProblems)
                {
                    Console.Write($"{theProblem.FileName,-15} {theProblem.TotalAristas,6} {theProblem.OptimalLocation,10} ");

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
                        mediaF.Add(theAlgorithm.Best.Fitness);
                        efos.Add(theAlgorithm.EFOs);
                        if (Math.Abs(theAlgorithm.Best.Fitness - theProblem.OptimalLocation) < 1e-10)
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
