﻿using System;
using System.Collections.Generic;
using System.Linq;
using BasedOnHarmony.Funciones;

namespace BasedOnHarmony.Metaheuristicas.Armonicos{
    class HSOS : Algorithm{

        public int PopulationSize = 25;
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
            //Population = InitializeFixedPopulation(PopulationSize);
            Population = InitializeControlledPopulation(PopulationSize);
            List<Solution> Populationtemp = new List<Solution>(Population);
            //Populationtemp.Sort((x,y) => x.Fitness.CompareTo(y.Fitness));
            //Best = new Solution(Populationtemp[0]);
            Population.Sort((x,y) => x.Fitness.CompareTo(y.Fitness));
            Best = new Solution(Population[0]);

            //Iteracion del algoritmo
            while (EFOs < MaxEFOs && Best.Fitness > theProblem.OptimalLocation)
            {
                //Recorrido de la poblacon
                //Console.Write("\n");
                for (var i = 0; i < PopulationSize; i++) {
                   // Console.Write("-" + i);
                    
                    //MutualismoJ
                    var j = myRandom.Next(PopulationSize); while (i == j) j = myRandom.Next(PopulationSize);
                    var m1 = Population[i].Mutualism(Population[j], Best);
                    if (m1.MyAlgorithm.MyRandom.NextDouble() < 0.3) m1 = Utils.LocalSearch(m1,3);
                    if (m1.Fitness < Population[i].Fitness) Population[i] = m1;                
                    if (EFOs >= MaxEFOs) break;
                    
                    
                    //MutualismoI
                    var m2 = Population[j].Mutualism(Population[i], Best);
                    if (m2.MyAlgorithm.MyRandom.NextDouble() < 0.3)  m2 = Utils.LocalSearch(m2,3);
                    if (m2.Fitness < Population[j].Fitness)Population[j] = m2;
                    if (EFOs >= MaxEFOs) break;
                   
                    //Comensalimo
                    j = myRandom.Next(PopulationSize); while (i == j) j = myRandom.Next(PopulationSize);
                    var m3 = Population[i].Commensalism(Population[j], Best);
                    if (m3.MyAlgorithm.MyRandom.NextDouble() < 0.3) m3 = Utils.LocalSearch(m3,3);
                    if (m3.Fitness < Population[i].Fitness)Population[i] = m3;
                    if (EFOs >= MaxEFOs) break;
                    
                    //Parasitimo
                    j = myRandom.Next(PopulationSize); while (i == j) j = myRandom.Next(PopulationSize);
                    var m4 = Population[i].Parasitism();
                    if (m4.MyAlgorithm.MyRandom.NextDouble() < 0.3) m4 = Utils.LocalSearch(m4,3);
                    if (m4.Fitness < Population[i].Fitness) Population[i] = m4;
                    if (EFOs >= MaxEFOs) break;
                    
                    //Improisación de una nueva armonia
                    var m5 = Improvisation(i);
                    if (m5.MyAlgorithm.MyRandom.NextDouble() < 0.3) m5 = Utils.LocalSearch(m5,3);
                    var worstFitness = Population.Max(x => x.Fitness);
                    var posworstFitness = Population.FindIndex(x => Math.Abs(x.Fitness - worstFitness) < 1e-10);
                    if (m5.Fitness < Population[posworstFitness].Fitness)Population[posworstFitness] = m5;
                    if (m5.Fitness < Population[i].Fitness) Population[i] = m5;
                    if (EFOs >= MaxEFOs) break;
                    
                    //Populationtemp = new List<Solution>(Population); 
                    //Populationtemp.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
                    //Best = new Solution(Populationtemp[0]);
                }
                Population.Sort((x, y) => x.Fitness.CompareTo(y.Fitness));
                Best = new Solution(Population[0]);
                //Console.WriteLine("\nBest");<
                //Best.Imprimir();
            }
           
        }
        /// <summary>
        /// Fase de improvisacion para los organismos
        /// </summary>
        /// <param name="posXi"></param>
        public Solution Improvisation(int posXi)
        {
            const double par = 0.5;
            var hmcr = 0.95;

            var neko = new Solution(this);
            for (var k = 0; k < MyProblem.NumVertices; k++)
            {
                if (MyRandom.NextDouble() <= hmcr)
                {
                    var posAleatoria = MyRandom.Next(Population.Count);
                    while (posAleatoria == posXi) posAleatoria = MyRandom.Next(Population.Count);

                    neko.Vertices[k] = Population[posAleatoria].Vertices[k];  
                    if (MyRandom.NextDouble() <= par)
                    {
                        if (MyRandom.NextDouble() > 0.5)
                            neko.Vertices[k] = 0;    
                        else
                            neko.Vertices[k] = 1;
                    }
                }
                else
                {
                    neko.Vertices[k] = MyRandom.Next(2); 
                }
            }
            neko.RecalculatePosInstalaciones();
            neko.RepararSolutionAwareness();
            //neko.RepairSolutionRandomly();
            neko.Evaluate();
            return neko;
        }

    }
}