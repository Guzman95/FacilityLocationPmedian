using System;
namespace BasedOnHarmony.Metaheuristicas
{
    public partial class Solution
    {
        /// <summary>
        /// Fase de mutualismo para los organismos
        /// Cada uno de los organismos se beneficia del su compañero y del mejor de la poblacion 
        /// </summary>
        /// <param name="xj,best></param>
        /// <returns></returns>
        public Solution Mutualism(Solution xj, Solution best)
        {
            var mutualvector = new int[MyAlgorithm.MyProblem.NumVertices];
            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (MyAlgorithm.MyRandom.NextDouble() > 0.5)
                {
                    mutualvector[k] = this.Vertices[k];
                }
                else
                {
                    mutualvector[k] = xj.Vertices[k];
                }
            }
            var a = new Solution(this.MyAlgorithm);
            a.RandomInitializationWithoutConstrains();
            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (mutualvector[k] == best.Vertices[k])
                {
                    a.Vertices[k] = best.Vertices[k];
                }
            }

            var b = new Solution(this.MyAlgorithm);
            b.RandomInitializationWithoutConstrains();
            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (a.Vertices[k] == this.Vertices[k])
                {
                    if (b.PosInstalaciones.Count > Math.Floor(MyAlgorithm.MyProblem.PMedianas * .25))
                    {                    
                        if (Vertices[k] == 1) b.Activar(k);
                        else b.InActivar(k);
                    }
                    else if (this.Vertices[k] == 1)
                    {
                        b.Activar(k);
                    }
                }
            }
            b.RepararSolutionAwareness();
            //b.RepairSolutionRandomly();
            b.Evaluate();
            return b;
        }
        /// <summary>
        /// Fase de Comesalismo para los organismos
        /// Un solo organismo se beneficia del otro y el del mejor de la poblacion
        /// </summary>
        /// <param name="xj,best></param>
        /// <returns></returns>
        public Solution Commensalism(Solution xj, Solution best)
        {
            var c = new Solution(this.MyAlgorithm);
            c.RandomInitializationWithoutConstrains();
            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (xj.Vertices[k] == best.Vertices[k])
                {
                    c.Vertices[k] = best.Vertices[k];
                }
            }
            var d = new Solution(this.MyAlgorithm);
            d.RandomInitializationWithoutConstrains();
            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (c.Vertices[k] == this.Vertices[k])
                {
                    if (d.PosInstalaciones.Count > Math.Floor(MyAlgorithm.MyProblem.PMedianas * .25))
                    {
                        if (Vertices[k] == 1) d.Activar(k);
                        else d.InActivar(k);
                    }
                    else if (this.Vertices[k] == 1)
                    {
                        d.Activar(k);
                    }
                }
            }
            d.RepararSolutionAwareness();
            //d.RepairSolutionRandomly();
            d.Evaluate();
            if (d.Fitness < this.Fitness) return d;
            return this;
        }
        /// <summary>
        /// Fase de Comesalismo para los organismos
        /// A partir de un organismo se crea un parasito que intentara elimianar la organismo compañero
        /// </summary>
        /// <param name="xj,best></param>
        /// <returns></returns>
        public Solution Parasitism(Solution xj)
        {
            var parasito = new Solution(this);
            
            var nDimenciones = Utils.RandomlySelectedDimensions(MyAlgorithm.MyRandom, MyAlgorithm.MyProblem.NumVertices);
            foreach (var pos in nDimenciones)
            {
                if (this.PosInstalaciones.Count > Math.Floor(MyAlgorithm.MyProblem.PMedianas * .25))
                {
                    if (parasito.Vertices[pos] == 1) InActivar(pos);
                    else Activar(pos);
                }
                else if (parasito.Vertices[pos] == 1)
                {
                    this.Activar(pos);
                }    
            }
            parasito.RepararSolutionAwareness();
            //parasito.RepairSolutionRandomly();
            parasito.Evaluate();
            if (parasito.Fitness < xj.Fitness) return parasito;
            return xj;
        }
    }
}