using System;
using System.Collections.Generic;
using System.Linq;
using BasedOnHarmony.Funciones;

namespace BasedOnHarmony.Metaheuristicas.Armonicos{
    class HSOS : Algorithm{

        public int PopulationSize = 30;
        public List<Solution> Population;

        /// <summary>
        /// Ejecutart el algoritmo HSOS
        /// </summary>
        /// <param name="theProblem, myRandom"></param>
        public override void Ejecutar(PMediana theProblem, Random myRandom)
        {
            MyProblem = theProblem;
            MyRandom = myRandom;
            EFOs = 0;

            //Inicializar la Population
            Population = InitializeFixedPopulation(PopulationSize);
            //Population = InitializeControlledPopulation(PopulationSize);
            Population.Sort((x,y) => x.Fitness.CompareTo(y.Fitness));
            Best = new Solution(Population[0]);
            
           //Iteracion del algoritmo
            while (EFOs < MaxEFOs ){
                //Recorrido de la poblacon
                for (var i = 0; i < PopulationSize; i++){

                    //Mutualismo
                    var j = myRandom.Next(PopulationSize); while (i==j) j = myRandom.Next(PopulationSize);
                    var m1 = Population[i].Mutualism(Population[j], Best);
                    if (m1.Fitness < Population[i].Fitness) Population[i] = m1;
                    if (EFOs >= MaxEFOs) break;
                    
                    var m2 = Population[j].Mutualism(Population[i], Best);
                    if (m2.Fitness < Population[j].Fitness) Population[j] = m2;
                    if (EFOs >= MaxEFOs) break;
                    
                    //Comensalimo
                    j = myRandom.Next(PopulationSize); while (i == j) j = myRandom.Next(PopulationSize);
                    Population[i] = Population[i].Commensalism(Population[j], Best);
                    //Console.WriteLine("Before Comensalismo");
                    if (EFOs >= MaxEFOs) break;
                    
                    //Parasitimo
                    j = myRandom.Next(PopulationSize); while (i == j) j = myRandom.Next(PopulationSize);
                    Population[j] = Population[i].Parasitism(Population[j]);
                    if (EFOs >= MaxEFOs) break;

                    //Improisación de una nueva armonia
                    Improvisation(i);
                    if (EFOs >= MaxEFOs) break;
                }

                Population.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
                Best = new Solution(Population[0]);
            }
        }
        /// <summary>
        /// Fase de improvisacion para los organismos
        /// </summary>
        /// <param name="posXi"></param>
        public void Improvisation(int posXi)
        {
            const double par = 0.365;
            var hmcr = 1.0 - (10.0 / MyProblem.NumVertices);

            var neko = new Solution(this);
            for (var k = 0; k < MyProblem.NumVertices; k++)
            {
                if (MyRandom.NextDouble() <= hmcr)
                {
                    var posAleatoria = MyRandom.Next(Population.Count);
                    while (posAleatoria == posXi) posAleatoria = MyRandom.Next(Population.Count);

                    neko.Vertices[k] = Population[posAleatoria].Vertices[k];  ///Aqui
                    if (MyRandom.NextDouble() <= par)
                    {
                        if (MyRandom.NextDouble() > 0.5)
                            neko.Vertices[k] = 0;    //aqui
                        else
                            neko.Vertices[k] = 1;
                    }
                }
                else
                {
                    neko.Vertices[k] = MyRandom.Next(2); //aqui
                }
            }
            neko.RecalculatePosInstalaciones();
            neko.RepararSolutionAwareness();
            //neko.RepairSolutionRandomly();
            neko.Evaluate();

            var worstFitness = Population.Max(x => x.Fitness);
            var posworstFitness = Population.FindIndex(x => Math.Abs(x.Fitness - worstFitness) < 1e-10);
            if (neko.Fitness < Population[posworstFitness].Fitness)
                Population[posworstFitness] = neko;

            if (neko.Fitness < Population[posXi].Fitness)
                Population[posXi] = neko;
        }

    }
}