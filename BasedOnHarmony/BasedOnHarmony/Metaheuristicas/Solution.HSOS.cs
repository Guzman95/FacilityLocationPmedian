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
            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (a.Vertices[k] == this.Vertices[k])
                {
                    b.Vertices[k] = this.Vertices[k];
                }
            }
            b.RecalculatePosInstalaciones();
            //b.RepararSolutionAwareness();
            b.RepairSolutionRandomly();
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
            for (var k = 0; k < MyAlgorithm.MyProblem.NumVertices; k++)
            {
                if (c.Vertices[k] == this.Vertices[k])
                {
                    d.Vertices[k] = this.Vertices[k];
                }
            }
            d.RecalculatePosInstalaciones();
            //d.RepararSolutionAwareness();
            d.RepairSolutionRandomly();
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
                if (parasito.Vertices[pos] == 1)
                    InActivar(pos);
                else
                    Activar(pos);
            }
            //parasito.RepararSolutionAwareness();
            parasito.RepairSolutionRandomly();
            parasito.Evaluate();

            if (parasito.Fitness < xj.Fitness) return parasito;
            return xj;
        }
    }
}