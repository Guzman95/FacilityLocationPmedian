using System;
using System.Collections.Generic;

namespace BasedOnHarmony.Metaheuristicas
{
    class Utils
    {
        //Obtiena una conjunto de posiones aleatorias de acuerdo a la solucion
        /// <summary>
        ///  Determina las menores distancias de cada demanda a su instalacion masa cercana  
        /// </summary>
        /// <param name="myRandom"></param>
        /// <param name="numVertices"></param>
        /// <returns name="dimencionesN"></returns>
        public static List<int> RandomlySelectedDimensions(Random myRandom, int numVertices)
        {
            var numerodimencionesN = myRandom.Next(numVertices);
            return RandomlySelectedExactDimensions(myRandom, numVertices, numerodimencionesN);
        }
        /// <summary>
        ///  Determina las menores distancias de cada demanda a su instalacion masa cercana  
        /// </summary>
        /// <param name="myRandom"></param>
        /// <param name="numVertices"></param>
        /// <param name="exact"></param>
        /// <returns name="dimencionesN"></returns>
        public static List<int> RandomlySelectedExactDimensions(Random myRandom, int numVertices, int exact)
        {
            var dimencionesN = new List<int>();
            while (dimencionesN.Count < exact)
            {
                var valor = myRandom.Next(numVertices);
                if (dimencionesN.Exists(x => x == valor)) continue;
                dimencionesN.Add(valor);
            }
            return dimencionesN;
        }

        /// <summary>
        ///  Determina las menores distancias de cada demanda a su instalacion masa cercana  
        /// </summary>
        /// <param name="PosInstalaciones"></param>
        /// <param name="MyAlgorithm"></param>
        /// <returns name="menoresDistancias"></returns>
        public static double[] DeterminarMenoresDistanciasX(Algorithm MyAlgorithm, List<int> PosInstalaciones)
        {
            double[] menoresDistancias = new double[MyAlgorithm.MyProblem.NumVertices];
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                menoresDistancias[i] = MyAlgorithm.MyProblem.DistanciaMenorPuntoDemanda(i, PosInstalaciones);
            }
            return menoresDistancias;
        }
        /// <summary>
        ///  Determina la instalacion que al agregarla disminuya al maximo el valor la funcion objetivo  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <param name="MyAlgorithm"></param>
        /// <param name="Vertices"></param>
        /// <returns name="poskX"></returns>
        public static int DeterminarPosArgMax(double[] menoresDistancias, Algorithm MyAlgorithm, int[] Vertices)
        {
            int poskX = 0;
            double argMin = 0;
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
                sumasPerdidasAdicion[j] = sumaMin;
            }
            for (var s = 0; s < sumasPerdidasAdicion.Length; s++)
            {
                if (sumasPerdidasAdicion[s] > argMin)
                {
                    argMin = sumasPerdidasAdicion[s];
                    poskX = s;
                }
            }
            return poskX;
        }
        /// <summary>
        ///  Determina la instalacion que al eliminarna aumente al mimimo el  valor la funcion objetivo  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <param name="PosInstalaciones"></param>
        /// <param name="MyAlgorithm"></param>
        /// <returns name="poskX"></returns>
        public static int DeterminarPosArgMin(double[] menoresDistancias, List<int> PosInstalaciones, Algorithm MyAlgorithm)
        {
            int poskX = 0;
            double argMax = 1000000;
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
            for (var s = 0; s < sumasGanaciasEliminacion.Count; s++)
            {
                if (sumasGanaciasEliminacion[s] < argMax)
                {
                    argMax = sumasGanaciasEliminacion[s];
                    poskX = PosInstalaciones[s]; ;
                }
            }
            return poskX;
        }
    }
}
