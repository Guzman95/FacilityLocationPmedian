using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BasedOnHarmony.Funciones;
using BasedOnHarmony.Metaheuristicas;
using BasedOnHarmony.Metaheuristicas.Armonicos;
using BasedOnHarmony.Servidor;

namespace BasedOnHarmony
{
    class Program
    {
        static void Main(string[] args)
        {
            const int maxEFOS = 1500;
            while (true)
            {
                try
                {
                    var myService = new ServiceClient();
                    var myDistributedTask = myService.GetTask();
                    myService.Close();

                    if (myDistributedTask is null)
                        return; // There is no tasks to be processed
                    Console.WriteLine("\nAleatorio CON LSGEN Tarea en proceso:");
                    Console.WriteLine(myDistributedTask.Problem + "-" + myDistributedTask.Seed + "-" + myDistributedTask.Algorithm);
                    Algorithm theAlgorithm = null;
                    
                    switch (myDistributedTask.Algorithm)
                    {
                        case "HSOS":
                            theAlgorithm = new HSOS() { MaxEFOs = maxEFOS };
                            break;
                        case "SBHS":
                            theAlgorithm = new SBHS() { MaxEFOs = maxEFOS };
                            break;
                    }
                    var the_problem  = new PMediana(myDistributedTask.Problem+".txt");
                    var myRandom = new Random(myDistributedTask.Seed);
                    Console.WriteLine("\nEjecutando algoritmo..... ");
                    theAlgorithm.Ejecutar(the_problem ,myRandom);                
                    myDistributedTask.Result_Best =  theAlgorithm.Best.Fitness;
                    Console.WriteLine("\nResultado:  "+theAlgorithm.Best.Fitness);
                    myDistributedTask.Status = "S";

                    var miServicio2 = new ServiceClient();
                    Console.WriteLine("\nGuardando resultados.....");
                    miServicio2.SaveResults(myDistributedTask);
                    //
                    miServicio2.Close();
                }
                catch (Exception e1)
                {
                    Console.WriteLine(e1.Message);
                }
            }
        }
    }
}
