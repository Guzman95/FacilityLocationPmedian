using System;
using System.Collections.Generic;
using BasedOnHarmony.Funciones;

namespace BasedOnHarmony.Metaheuristicas
{
    public abstract class Algorithm
    {
        public int MaxEFOs;
        public int EFOs;
        public Solution Best;

        public PMediana MyProblem;
        public Random MyRandom;

        public abstract void Ejecutar(PMediana theProblem, Random myRandom);

        /// <summary>
        ///  //Inicializa la Population con organismos generados  con un numero p de instacionciones 
        /// </summary>
        /// <param name="populationSize"></param>
        /// <returns></returns>
        public List<Solution> InitializeFixedPopulation(int populationSize)
        {
            var poblacion = new List<Solution>();
            for (var i = 0; i < populationSize; i++)
            {
                var sol = new Solution(this);
                sol.RandomInitializationWithoutConstrains();
                //sol.RepairSolutionRandomly();
                sol.RepararSolutionAwareness();
                sol.Evaluate();
                poblacion.Add(sol);
            }
            return poblacion;
        }
        /// <summary>
        ///  //Inicializa la Population con organismos generados  con un numero p de instacionciones
        /// </summary>
        /// <param name="populationSize"></param>
        /// <returns></returns>
        public List<Solution> InitializeControlledPopulation(int populationSize)
        {
            var poblacion = new List<Solution>();
            for (var i = 0; i < populationSize; i++)
            {
                var sol = new Solution(this);
                sol.RandomOrganismInitialization();
                sol.Evaluate();
                poblacion.Add(sol);
            }
            return poblacion;
        }
    }
}
