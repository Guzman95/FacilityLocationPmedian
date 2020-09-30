using System;
using Monografia.Funciones;

namespace Monografia.Metaheuristicas
{
    public abstract class Algorithm
    {
        public int MaxEFOs;
        public int EFOs;
        public Solution BestSolution;

        public abstract void Ejecutar(p_mediana theProblem, Random myRandom);
    }
}
