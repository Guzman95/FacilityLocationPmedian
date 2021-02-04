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
        /// <returns>b</returns>
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
            var indicador = MyAlgorithm.MyProblem.PMedianas * 0.5;
            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (a.Vertices[k] == this.Vertices[k])
                {
                    
                    if (b.PosInstalaciones.Count > Math.Floor(indicador))
                    {                    
                        if (a.Vertices[k] == 1) b.Activar(k);
                        else b.InActivar(k);
                    }
                    else if (a.Vertices[k] == 1) b.Activar(k);
                    
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
        /// <returns>d</returns>
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
            var indicador = MyAlgorithm.MyProblem.PMedianas * 0.5;
            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (c.Vertices[k] == this.Vertices[k]) 
                { 
                    
                    if (d.PosInstalaciones.Count > Math.Floor(indicador))
                    {
                        if (c.Vertices[k] == 1) d.Activar(k);
                        else d.InActivar(k);
                    }
                    else if (c.Vertices[k] == 1) d.Activar(k);
                }
            }
            d.RepararSolutionAwareness();
            //d.RepairSolutionRandomly();
            d.Evaluate();
            return d;
        }
        /// <summary>
        /// Fase de Comesalismo para los organismos
        /// A partir de un organismo se crea un parasito que intentara elimianar la organismo compañero
        /// </summary>
        /// <param name="xj,best></param>
        /// <returns>parasito</returns>
        public Solution Parasitism()
        {
            var parasito = new Solution(this);
            var nDimenciones = Utils.RandomlySelectedDimensions(MyAlgorithm.MyRandom, MyAlgorithm.MyProblem.NumVertices);
            var indicador = MyAlgorithm.MyProblem.PMedianas * 0.5;
            foreach (var pos in nDimenciones)
            {
                
                if (parasito.PosInstalaciones.Count > Math.Floor(indicador))
                {
                    if (parasito.Vertices[pos] == 1) parasito.InActivar(pos);
                    else parasito.Activar(pos);
                }
                else if (parasito.Vertices[pos] == 0) parasito.Activar(pos);  
               
            }
            parasito.RepararSolutionAwareness();
            //parasito.RepairSolutionRandomly();
            parasito.Evaluate();
            return parasito;
        }
    }
}