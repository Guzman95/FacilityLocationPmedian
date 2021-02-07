using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            const int maxRep = 30;
            Console.WriteLine("Iniciando......");
            Console.WriteLine("Cargando Archivos de problemas.....");
            var myProblems = new List<PMediana>
            {
                
                new PMediana("pmed1.txt"), 
                new PMediana("pmed2.txt"), 
                new PMediana("pmed3.txt"),
                new PMediana("pmed4.txt"),
                new PMediana("pmed5.txt"), 
                new PMediana("pmed6.txt"),
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
                new PMediana("pmed40.txt"),
                new PMediana("pmed41.txt")            
                };
            Console.ReadKey();
            var myAlgorithms = new List<string>();
            
                myAlgorithms.Add("HSOS");
                myAlgorithms.Add("SBHS");
            Console.WriteLine("\nEjecutando Algoritmos.....");
            foreach (var nameAlgorithm in myAlgorithms)
            {
                Console.WriteLine($"{nameAlgorithm,80}");
                Console.Write($"{"Problem",-15} {"Vertices",6} {"PMedianas",6} {"Ideal",10} ");
                Console.WriteLine($"{"Avg-Fitness",15} {"fMRPE-Fitness",15} {"SD-Fitness",12} {"Avg-Efos",12} {"Success Rate",12} { "TimeAVG",12} { "TimeTotal",12}");

                foreach (var theProblem in myProblems)
                {
                    Console.Write($"{theProblem.FileName,-15} {theProblem.NumVertices,6} {theProblem.PMedianas,10} {theProblem.OptimalLocation,10} ");

                    var efos = new List<int>();
                    var mediaF = new List<double>();
                    var times = new List<double>();
                    var succesRate = 0;
                    var timeStart = DateTime.Now;
                    Parallel.For(0, maxRep, rep =>
                    { 
                        Debug.WriteLine("HILO :" + Thread.CurrentThread.ManagedThreadId + " Problema " + theProblem);
                        Algorithm theAlgorithm = null;
                        switch (nameAlgorithm)
                        {
                            case "HSOS":
                                theAlgorithm = new HSOS();
                                break;
                            case "SBHS":
                                theAlgorithm = new SBHS();
                                break;
                        }
                        var myRandom = new Random(rep);
                        var timeBegin = DateTime.Now;
                        theAlgorithm.MaxEFOs = maxEFOS;
                        theAlgorithm.Ejecutar(theProblem, myRandom);
                        times.Add((DateTime.Now - timeBegin).TotalSeconds);
                        //Console.WriteLine("\nrep: " + rep);
                        //theAlgorithm.Best.Imprimir();
                        mediaF.Add(theAlgorithm.Best.Fitness);
                        efos.Add(theAlgorithm.EFOs);
                        if (theAlgorithm.Best.Fitness <= theProblem.OptimalLocation) succesRate++;

                    });//End ParallelFor

                    var avg = mediaF.Average();
                    Console.Write($"{avg,15:0.000} ");
                    var rpe = ((avg - theProblem.OptimalLocation) /theProblem.OptimalLocation) * 100;
                    Console.Write($"{rpe,15:0.000} ");
                    var deviation = mediaF.Sum(d => (d - avg) * (d - avg));
                    deviation = Math.Sqrt(deviation / 30);
                    Console.Write($"{deviation,12:0.000} ");
                    Console.Write($"{efos.Average(),12:0.000} ");
                    Console.Write($"{succesRate * 100.0 / maxRep,112:0.00}% ");
                    Console.WriteLine($"{times.Average(),12:0.000000} ");
                    Console.WriteLine($"{(DateTime.Now-timeStart).TotalSeconds,12:0.000000} ");

                }
            }
            Console.WriteLine("Terminado, Presione una tecla para cerrar...");
            Console.ReadKey();
        }
    }
}
