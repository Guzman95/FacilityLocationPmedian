using System.Collections.Generic;
using System;

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
                //if (MyAlgorithm.MyRandom.NextDouble() < Pmedianas * 0.25)
                if (MyAlgorithm.MyRandom.NextDouble() < 0.5)
                    InActivar(i);
                else
                    Activar(i);
        }
        /// <summary>
        /// Agrega o elimina instalaciones aleatoriamente hasta que instalaciones igual a P 
        /// </summary>
        /// <param ></param>
        /// <returns></returns> 
        public void RepairSolutionRandomly()
        {
            //Console.WriteLine("\nSolucion LLega");
            //Imprimir();
            //Console.WriteLine("\nLlegan: " + PosInstalaciones.Count);
            var pMedianas = MyAlgorithm.MyProblem.PMedianas;
            while (PosInstalaciones.Count < pMedianas)
            {
                //Console.WriteLine("Menor: " + PosInstalaciones.Count);
                Activar(VerticeValidation(1));
            }
            while (PosInstalaciones.Count > pMedianas)
            {
                //Console.WriteLine("Mayor: " + PosInstalaciones.Count);
                InActivar(VerticeValidation(0));
            }
            //Console.WriteLine("\nSalen: " + PosInstalaciones.Count);
            //Console.WriteLine("\nSolucion Sale");
            //Imprimir();
        }

        /// <summary>
        /// Seleciona aleatoriamente  una posicion de instalacion para ser agregada a la solucion
        /// </summary>
        /// <param name="state"></param>
        /// <returns name="posAleatoria"></returns>
        private int VerticeValidation(int state)
        {
            int posAleatoria;
            do
            {
                posAleatoria = MyAlgorithm.MyRandom.Next(MyAlgorithm.MyProblem.NumVertices);
            } while (Vertices[posAleatoria] == state);
            return posAleatoria;
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
        ///  Agrega o elimina instalaciones aplicando conocimiento hasta instalaciones igual a P  
        /// </summary>
        /// <param></param>
        /// <returns></returns>             
        public void RepararSolutionAwareness()
        {
            Console.WriteLine("\nSolucion LLega");
            //Imprimir();
            //Console.WriteLine("\nLlegan: " + PosInstalaciones.Count);
            int pMedianas = MyAlgorithm.MyProblem.PMedianas;
            //Console.WriteLine("\nINSTALACINES");
            //for (var b = 0; b < PosInstalaciones.Count; b++)
            //{
            //    Console.Write(PosInstalaciones[b] + "-");
            //}         
            var menoresDistancias = Utils.DeterminarMenoresDistanciasX(MyAlgorithm, PosInstalaciones);
            //Console.WriteLine("\nMENORESDISTANCIAS");
            //for (var a = 0; a < menoresDistancias.Length; a++)
            //{
            //    Console.Write(menoresDistancias[a] + "-");
            //}
            while (PosInstalaciones.Count < pMedianas)
            {
                var pos = Utils.DeterminarPosArgMin(menoresDistancias, MyAlgorithm, Vertices);
                Activar(pos);
                menoresDistancias = Utils.DeterminarMenoresDistanciasX(MyAlgorithm, PosInstalaciones);
                Console.WriteLine("Menor: " + PosInstalaciones.Count);
            }
            while (PosInstalaciones.Count > pMedianas)
            {
                var pos = Utils.DeterminarPosArgMax(menoresDistancias, PosInstalaciones, MyAlgorithm);
                InActivar(pos);
                menoresDistancias = Utils.DeterminarMenoresDistanciasX(MyAlgorithm, PosInstalaciones);
                Console.WriteLine("Mayor: " + PosInstalaciones.Count);
            }
            //Console.WriteLine("\nSalen: " + PosInstalaciones.Count);
            //Console.WriteLine("\nSolucion Sale");
            //Imprimir();
        }
        /// <summary>
        ///  Imprime los valores de la solucion  
        /// </summary>
        /// <param></param>
        /// <returns></returns>    
        public void Imprimir()
        {
            Console.WriteLine("\nSolucion");
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                Console.Write("-" + Vertices[i]);
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
            Fitness = MyAlgorithm.MyProblem.Evaluate(PosInstalaciones);
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