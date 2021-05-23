using System.Collections.Generic;
using System;
using System.Linq;

namespace BasedOnHarmony.Metaheuristicas
{
    public partial class Solution
    {
        public Algorithm MyAlgorithm;

        public List<int> PosInstalaciones;
        public int[] Vertices; // {0, 1}
        public double Fitness { get; set; }

        public Solution(Algorithm theAlgorithm)
        {
            MyAlgorithm = theAlgorithm;

            PosInstalaciones= new List<int>();
            Vertices = new int[MyAlgorithm.MyProblem.NumVertices];
            Fitness = 0;
        }

        public Solution(Solution origin)
        {
            MyAlgorithm = origin.MyAlgorithm;
            PosInstalaciones = new List<int>();
            PosInstalaciones.AddRange(origin.PosInstalaciones);
            Vertices = new int[MyAlgorithm.MyProblem.NumVertices];
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
                Vertices[i] = origin.Vertices[i];
            Fitness = origin.Fitness;
        }
        /// <summary>
        ///  //Recalcula la lista de pociones de instalaciones
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public void RecalculatePosInstalaciones()
        {
            //PosInstalaciones = new List<int>();
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
                if (Vertices[i] == 1)
                    PosInstalaciones.Add(i);
        }
        /// <summary>
        ///  //Activa una posicion de la solucion y agrega esa posicion a la lista de instalaciones
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public void Activar(int pos)
        {
            if (Vertices[pos] == 1) return;
            Vertices[pos] = 1;
            PosInstalaciones.Add(pos);
            PosInstalaciones.Sort();
        }
        /// <summary>
        /// Desactiva una posicion de la solucion y elimina esa posicion a la lista de instalaciones
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public void InActivar(int pos)
        {
            if (Vertices[pos] == 0) return;
            Vertices[pos] = 0;
            PosInstalaciones.Remove(pos);
        }
        /// <summary>
        ///  //Genera e inicializa una solucion de forma aleatoria
        /// </summary>
        /// <param ></param>
        /// <returns></returns>
        public void RandomInitializationWithoutConstrains()
        {
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
                if (MyAlgorithm.MyRandom.NextDouble() < 0.7)
                    InActivar(i);
                else
                    Activar(i);
        }

        /// <summary>
        ///  Inicializa un organismo con el numero de instalaciones selecionadas 
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public void RandomOrganismInitialization()
        {
            var posiciones = Utils.RandomlySelectedExactDimensions(MyAlgorithm.MyRandom,
                MyAlgorithm.MyProblem.NumVertices, MyAlgorithm.MyProblem.PMedianas);
            foreach (var pos in posiciones)
                Activar(pos);
        }
        /// <summary>
        /// Agrega o elimina instalaciones aleatoriamente hasta que instalaciones igual a P 
        /// </summary>
        /// <param ></param>
        /// <returns></returns> 
        public void RepairSolutionRandomly()
        {
            var pMedianas = MyAlgorithm.MyProblem.PMedianas;
            while (PosInstalaciones.Count < pMedianas)
            {
                Activar(Utils.VerticeValidation(Vertices,MyAlgorithm, 1));
            }
            while (PosInstalaciones.Count > pMedianas)
            {

                InActivar(Utils.VerticeValidation(Vertices, MyAlgorithm, 0));
            }
        }

        /// <summary>
        ///  Agrega o elimina instalaciones aplicando conocimiento hasta instalaciones igual a P  
        /// </summary>
        /// <param></param>
        /// <returns></returns>             
        public void RepararSolutionAwareness()
        {
            int pMedianas = MyAlgorithm.MyProblem.PMedianas;
            var menoresDistancias = Utils.DeterminarMenoresDistanciasX(MyAlgorithm, PosInstalaciones);

            while (PosInstalaciones.Count < pMedianas)
            {
                var pos = Utils.DeterminarPosArgMax(menoresDistancias, MyAlgorithm, Vertices);
                Activar(pos);
                menoresDistancias = Utils.ActualizarMenoresDistanciasAgregacion(MyAlgorithm, pos, menoresDistancias);
            }
            while (PosInstalaciones.Count > pMedianas)
            {
               
                var pos = Utils.DeterminarPosArgMin(menoresDistancias, PosInstalaciones, MyAlgorithm);
                InActivar(pos);
                menoresDistancias = Utils.ActualizarMenoresDistanciasEliminacion(MyAlgorithm, pos, menoresDistancias, PosInstalaciones);
               
            }
        }
        /// <summary>
        ///  Imprime los valores de la solucion  
        /// </summary>
        /// <param></param>
        /// <returns></returns>   
        public void Imprimir()
        {
            Console.WriteLine("Solucion");
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                Console.Write("-" + Vertices[i]);
            }
            Console.WriteLine("\nPinstalaciones");
            for (var k = 0; k < PosInstalaciones.Count; k++)
            {
                Console.Write("-" + PosInstalaciones[k]);
            }
            Console.WriteLine("\nFiness: "+Fitness);
        }


        /// <summary>
        ///  Realiza la evaluacion de la funcion objetivo para la solucion 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public void Evaluate()
        {
            double summ = 0;
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                var distancias = new List<double>();
                for (var t = 0; t < PosInstalaciones.Count; t++)
                {
                    distancias.Add(MyAlgorithm.MyProblem.DistanciasFloyd[i][PosInstalaciones[t]]);
                }
                summ += distancias.Min();
            }                 
            Fitness = summ;
            MyAlgorithm.EFOs++;
        }

        public override string ToString()
        {
            var result = "";
            for (var i = 0; i < PosInstalaciones.Count; i++)
                result += (PosInstalaciones[i] + " ");
            result = result + " f = " + Fitness;
            return result;
        }       
    }
}