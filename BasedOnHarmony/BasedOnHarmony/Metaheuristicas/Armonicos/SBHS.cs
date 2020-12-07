using System;
using System.Collections.Generic;
using System.Linq;
using BasedOnHarmony.Funciones;

namespace BasedOnHarmony.Metaheuristicas.Armonicos
{
    class SBHS : Algorithm
    {
        public int PopulationSize =30;
        public List<Solution> Population;

        public int posSolucionAleatoria(int PopulationSize, Random myRandon)
        {  // mover a solucion
            int maxvalue = PopulationSize; int minvalue = 0;
            int aleatorio;

            aleatorio = myRandon.Next(minvalue, maxvalue);
            //Console.WriteLine(aleatorio);
            return aleatorio;
        }

        public override void Ejecutar(PMediana theProblem, Random myRandom)
        {
            MyProblem = theProblem;
            MyRandom = myRandom;
            
            //Ajuste de los parametros
            double HMCR = 1 - (10 / theProblem.NumVertices);
            EFOs = 0;
            //Inicializar la Poblacion
            Population = InitializeFixedPopulation(PopulationSize);
            //Population = InitializeControlledPopulation(PopulationSize);

            Population.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
            Best = new Solution(Population[0]);
            
            while (EFOs < MaxEFOs)
            {
                var Xnew = new Solution(this);
                //Se genera la armonia de tamaño NumVertices
                for (int i = 0; i < theProblem.NumVertices; i++)
                {
                    if (myRandom.NextDouble() <= HMCR)
                    {
                        //Console.Write("r1->"+r3);
                        var posr1 = myRandom.Next(0, PopulationSize -1);
                        //r1[i]
                        //r2[i]
                        //best[i]
                        var posr2 = MyRandom.Next(PopulationSize); while (posr1 == posr2) posr2 = myRandom.Next(PopulationSize);
                        //Simplifican la tasa de consideracion de memoria y el pitch adjusment por medio de la formula

                        var state = (int)(Population[posr1].Vertices[posr1] + Math.Pow((-1), (Population[posr1].Vertices[posr1])) * Math.Abs(Best.Vertices[i] - Population[posr2].Vertices[posr2]));
                        
                        if(state == 1)
                        {
                            Xnew.Activar(i);
                            //Xnew.Vertices[i] = 1;
                        }
                        else
                        {
                            //el 25% de las medianas deben estar prendidas
                            if(Xnew.PosInstalaciones.Count < Math.Floor(theProblem.PMedianas * .25)) 
                            {
                                Xnew.Activar(i);
                            }
                        }
                        
                    }
                    else
                    {
                        //se crea una nota con un valor al azar para ser diversificado
                        if (MyRandom.NextDouble() < 0.5)
                        {
                            if (Xnew.PosInstalaciones.Count < Math.Floor(theProblem.PMedianas * .25))
                            {
                                Xnew.Activar(i);
                            }
                        }
                        else
                            Xnew.Activar(i);
                    }
                }
                Xnew.RepararSolutionAwareness();
                //Xnew.RepairSolutionRandomly();
                Xnew.Evaluate();
                var worstFitness = Population.Max(x => x.Fitness);
                var posworstFitness = Population.FindIndex(x => Math.Abs(x.Fitness - worstFitness) < 1e-10);
                //Evalua la evaluacion de la nueva armonia con la evaluacion de la peor solucion de la memoria
                if (Xnew.Fitness < Population[posworstFitness].Fitness)
                {
                    //Si se cumple la condicion se remplaza la peor armonia por la nueva armonia
                    Population[posworstFitness] = Xnew;
                }
                Population.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
                Best = new Solution(Population[0]);
            } 
        }

    }
}
