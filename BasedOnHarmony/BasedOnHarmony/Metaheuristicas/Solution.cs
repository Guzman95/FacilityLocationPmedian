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
                if (MyAlgorithm.MyRandom.NextDouble() < 0.7)
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
            var pMedianas = MyAlgorithm.MyProblem.PMedianas;
            while (PosInstalaciones.Count < pMedianas)
            {
                Activar(VerticeValidation(1));
            }
            while (PosInstalaciones.Count > pMedianas)
            {
                InActivar(VerticeValidation(0));
            }
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
            int pMedianas = MyAlgorithm.MyProblem.PMedianas;
            double[] menoresDistancias = DeterminarMenoresDistanciasX();
            while (PosInstalaciones.Count < pMedianas)
            {
                //Console.WriteLine("menor");
                var pos = DeterminarPosArgMax(menoresDistancias);
                //Console.WriteLine("pos"+pos);
                Activar(pos);
                menoresDistancias = DeterminarMenoresDistanciasX();
                //Console.WriteLine("\ndiastancias");
                //for (var i = 0; i < menoresDistancias.Length; i++)
                //{
                //    Console.Write(menoresDistancias[i] + "-");
                //}
                //Console.WriteLine("\nEstadosen1: " + PosInstalaciones.Count);
                //for (var i = 0; i < PosInstalaciones.Count; i++)
                //{
                //    Console.Write(PosInstalaciones[i] + "-");
                //}
            }
            while (PosInstalaciones.Count > pMedianas)
            {
                //Console.WriteLine("mayor");
                var pos = DeterminarPosArgMin(menoresDistancias);
                InActivar(pos);
                //Console.WriteLine("pos" + pos);
                menoresDistancias = DeterminarMenoresDistanciasX();
                //Console.WriteLine("\ndiastancias");
                //for (var i = 0; i < menoresDistancias.Length; i++)
                //{
                //    Console.Write(menoresDistancias[i] + "-");
                //}
                //Console.WriteLine("\nEstadosen1: " + PosInstalaciones.Count);
                //for (var i = 0; i <PosInstalaciones.Count; i++)
                //{
                //    Console.Write(PosInstalaciones[i] + "-");
                //}
            }
        }
        /// <summary>
        ///  Determina las menores distancias de cada demanda a su instalacion masa cercana  
        /// </summary>
        /// <param></param>
        /// <returns name="menoresDistancias"></returns>
        private double[] DeterminarMenoresDistanciasX()
        {
            double[] menoresDistancias = new double[MyAlgorithm.MyProblem.NumVertices];
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                menoresDistancias[i] = MyAlgorithm.MyProblem.DistanciaMenorPuntoDemanda(i, PosInstalaciones);
            }
            return menoresDistancias;
        }
        /// <summary>
        ///  Determina la instalacion que al eliminarna aumente en menor valor la funcion objetivo  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <returns name="poskX"></returns>
        private int DeterminarPosArgMax(double[] menoresDistancias)
        {
            int poskX = 0;
            double argMin = 0;
            double[] sumasPerdidasAdicion = CalcularPerdidaDeAdicion(menoresDistancias);
            for (var j = 0; j < sumasPerdidasAdicion.Length; j++)
            {
                if (sumasPerdidasAdicion[j] > argMin)
                {
                    argMin = sumasPerdidasAdicion[j];
                    poskX = j;
                }
            }
            return poskX;
        }
        /// <summary>
        ///  Calcula para cada instalacion  su adicion de valor a la funcion objetivo al ser elminada
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <returns name="sumaperdidaJX"></returns>
        private double[] CalcularPerdidaDeAdicion(double[] menoresDistancias)
        {
            double[] sumasPerdidasAdicion = new double[MyAlgorithm.MyProblem.NumVertices];
            for (var j = 0; j < MyAlgorithm.MyProblem.NumVertices; j++)
            {
                double sumaMin = 0;
                if (Vertices[j] == 0)
                {
                    for (var i = 0; i < menoresDistancias.Length; i++)
                    {
                        double distanciaIJ = MyAlgorithm.MyProblem.DistanciasFloyd[i][j];
                        double dif = distanciaIJ - menoresDistancias[i];
                        if (dif < 0)
                        {
                            sumaMin += dif * -1;
                        }
                    }
                    sumasPerdidasAdicion[j] = sumaMin;
                }
                sumasPerdidasAdicion[j] = sumaMin ;
            }
            return sumasPerdidasAdicion;
        }
        /// <summary>
        ///  Determina la instalacion que al agregarla disminuya al maximo el valor la funcion objetivo  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <returns name="poskX"></returns>
        private int DeterminarPosArgMin(double[] menoresDistancias)
        {
            int poskX = 0;
            double argMax = 1000000;
            List<double> sumasGanaciasEliminacion = CalcularGananciasDeEliminacion(menoresDistancias);
            for (int j = 0; j < sumasGanaciasEliminacion.Count; j++)
            {
                if (sumasGanaciasEliminacion[j] < argMax)
                {
                    argMax = sumasGanaciasEliminacion[j];
                    poskX = PosInstalaciones[j]; ;
                }
            }
            return poskX;
        }
        /// <summary>
        ///  Calcula para cada instalacion su eliminacion de valor a la funcion objetivo al ser agregada  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <returns name="sumasGanaciaJX"></returns>
        private List<double> CalcularGananciasDeEliminacion(double[] menoresDistancias)
        {
            List<double> sumasGanaciasEliminacion = new List<double>();
            for (var j = 0; j < PosInstalaciones.Count; j++)
            {
                double sumaMin = 0;
                List<int> copiapInstalaciones = new List<int>(PosInstalaciones);
                copiapInstalaciones.RemoveAt(j);
                for (var i = 0; i < menoresDistancias.Length; i++)
                {
                    double menordistanciaIT = MyAlgorithm.MyProblem.DistanciaMenorPuntoDemanda(i, copiapInstalaciones);
                    double dif = menordistanciaIT - menoresDistancias[i];
                    sumaMin += dif;
                }
                sumasGanaciasEliminacion.Add(sumaMin);
            }
            return sumasGanaciasEliminacion;
        }
        public void Imprimir()
        {
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                Console.Write("-" + Vertices[i]);
            }
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