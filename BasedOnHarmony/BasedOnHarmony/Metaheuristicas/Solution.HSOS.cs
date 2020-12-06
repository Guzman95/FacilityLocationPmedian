using System;
namespace BasedOnHarmony.Metaheuristicas
{
    public partial class Solution
    {
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

            //Console.WriteLine("\nsolucion");
            //b.Imprimir();
            //Console.WriteLine("\nestadosen1: " + b.PosInstalaciones.Count);
            //for (var i = 0; i < b.PosInstalaciones.Count; i++)
            //{
            //    Console.Write(b.PosInstalaciones[i]+"-");
            //}

            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (a.Vertices[k] == this.Vertices[k])
                {
                    if (b.PosInstalaciones.Count > (int)MyAlgorithm.MyProblem.PMedianas*0.25)
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
            //Console.WriteLine("\nSolucion");
            //b.Imprimir();
            //Console.WriteLine("\nEstadosen1: " + b.PosInstalaciones.Count);
            //for (var i = 0; i < b.PosInstalaciones.Count; i++)
            //{
            //    Console.Write(b.PosInstalaciones[i]+"-");
            //}

            b.RepararSolutionAwareness();
            //b.RepairSolutionRandomly();
            b.Evaluate();
            return b;
        }

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

            //d.Imprimir();
            //Console.WriteLine(" Possiones de instalaciones");
            //Console.WriteLine("estados1" + PosInstalaciones.Count);
            //for (var a = 0; a < d.PosInstalaciones.Count; a++) {
            //    Console.Write(d.PosInstalaciones[a]+"-");
            //}

            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (c.Vertices[k] == this.Vertices[k])
                {
                    if (d.PosInstalaciones.Count > (int)MyAlgorithm.MyProblem.PMedianas * 0.25)
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
            //Console.WriteLine("Solucion");
            //d.Imprimir();
            //Console.WriteLine("");
            //for (var a = 0; a < d.PosInstalaciones.Count; a++)
            //{
            //    Console.Write(d.PosInstalaciones[a]+"-");
            //}

            d.RepararSolutionAwareness();
            //d.RepairSolutionRandomly();
            d.Evaluate();
            if (d.Fitness < this.Fitness) return d;
            return this;
        }

        public Solution Parasitism(Solution xj)
        {
            var parasito = new Solution(this);
            
            var nDimenciones = Utils.RandomlySelectedDimensions(MyAlgorithm.MyRandom, MyAlgorithm.MyProblem.NumVertices);
            foreach (var pos in nDimenciones)
            {
                if (this.PosInstalaciones.Count > (int)MyAlgorithm.MyProblem.PMedianas * 0.25)
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