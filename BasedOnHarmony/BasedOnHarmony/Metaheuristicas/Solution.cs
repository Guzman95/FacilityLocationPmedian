﻿using System.Collections.Generic;
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
            for (var i=0; i < MyAlgorithm.MyProblem.NumVertices; i++)
                if (Vertices[i]==1)
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
            //PosInstalaciones.Sort();
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
                if (MyAlgorithm.MyRandom.Next(2) == 0)
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
            int poskX;
            var pMedianas = MyAlgorithm.MyProblem.PMedianas;

            while (PosInstalaciones.Count < pMedianas)
            {
                poskX = VerticeValidation(1);
                Activar(poskX);
            }
            while (PosInstalaciones.Count > pMedianas)
            {
                poskX = VerticeValidation(0);
                InActivar(poskX);
            }
        }

        /// <summary>
        /// Seleciona aleatoriamente  una posicion de instalacion para ser agregada a la solucion
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
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
            int poskX;
            if (PosInstalaciones.Count > 0)
            {
                RandomInitializationWithoutConstrains();
            }
            int pMedianas = MyAlgorithm.MyProblem.PMedianas;
            double[] menoresDistancias = determinarMenoresDistanciasX();
            while (PosInstalaciones.Count < pMedianas)
            {
                poskX = determinarArgMax(menoresDistancias);
                Activar(poskX);
                menoresDistancias = determinarMenoresDistanciasX();
            }
            while (PosInstalaciones.Count > pMedianas)
            {

                poskX = determinarArgMin(menoresDistancias);
                InActivar(poskX);
                menoresDistancias = determinarMenoresDistanciasX();
            }
            

        }
        /// <summary>
        ///  Determina las menores distancias de cada demanda a su instalacion masa cercana  
        /// </summary>
        /// <param></param>
        /// <returns name="menoresDistancias"></returns>
        private double[] determinarMenoresDistanciasX()
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
        private int determinarArgMax(double[] menoresDistancias)
        {
            int poskX = 0;
            double argMin = 0;
            double[] sumasPerdidasJX = adicionPerdida(menoresDistancias);
            for (var j = 0; j < sumasPerdidasJX.Length; j++)
            {
                if (sumasPerdidasJX[j] < argMin)
                {
                    argMin = sumasPerdidasJX[j];
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
        private double[] adicionPerdida(double[] menoresDistancias)
        {
            double[] sumaperdidaJX = new double[MyAlgorithm.MyProblem.NumVertices];
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
                            sumaMin = sumaMin + dif;
                        }
                    }
                    sumaperdidaJX[j] = sumaMin;
                }
                sumaperdidaJX[j] = 0;
            }
            return sumaperdidaJX;
        }
        /// <summary>
        ///  Determina la instalacion que al agregarla disminuya al maximo el valor la funcion objetivo  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <returns name="poskX"></returns>
        private int determinarArgMin(double[] menoresDistancias)
        {
            int poskX = 0;
            double argMax = 1000000;
            List<double> sumasGanaciasJSX = eliminarGanacia(menoresDistancias);
            for (int j = 0; j < sumasGanaciasJSX.Count; j++)
            {
                if (sumasGanaciasJSX[j] < argMax)
                {
                    argMax = sumasGanaciasJSX[j];
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
        private List<double> eliminarGanacia(double[] menoresDistancias)
        {
            List<double> sumasGanaciaJX = new List<double>();
            for (int j = 0; j < PosInstalaciones.Count; j++)
            {
                double sumaMin = 0;
                List<int> copiapInstalaciones = new List<int>(PosInstalaciones);
                copiapInstalaciones.RemoveAt(j);
                for (int i = 0; i < menoresDistancias.Length; i++)
                {
                    double menordistanciaIT = MyAlgorithm.MyProblem.DistanciaMenorPuntoDemanda(i, copiapInstalaciones);
                    double dif = menordistanciaIT - menoresDistancias[i];
                    sumaMin = sumaMin + dif;
                }
                sumasGanaciaJX.Add(sumaMin);
            }
            return sumasGanaciaJX;
        }
        public void imprimir()
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
                result = result + (PosInstalaciones[i] + " ");
            result = result + " f = " + Fitness;
            return result;
        }
        
       
    }
}