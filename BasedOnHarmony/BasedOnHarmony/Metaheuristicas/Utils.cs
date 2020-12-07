using System;
using System.Collections.Generic;
using System.Linq;

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
        ///  Determina la instalacion que al agregarla disminuya al maximo el valor la funcion objetivo  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <param name="MyAlgorithm"></param>
        /// <param name="Vertices"></param>
        /// <returns name="poskX"></returns>
        public static int DeterminarPosArgMin(Algorithm MyAlgorithm, int[] Vertices)
        {          
            var sumasPerdidasAdicion = new List<KeyValuePair<int, double>>();
            for (var j = 0; j < MyAlgorithm.MyProblem.NumVertices; j++)
            {
                if (Vertices[j] == 0)
                {
                    double sumaMin = 0;
                    for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
                    {
                         sumaMin += MyAlgorithm.MyProblem.DistanciasFloyd[i][j];
                    }
                    sumasPerdidasAdicion.Add(new KeyValuePair<int, double>(j, sumaMin));
                }
            }
            var min = sumasPerdidasAdicion.Min(x => x.Value);
            var poskX = sumasPerdidasAdicion.Find(x => Math.Abs(x.Value - min) < 1e-10);
            return poskX.Key;
        }
        /// <summary>
        ///  Determina la instalacion que al eliminarna aumente al mimimo el  valor la funcion objetivo  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <param name="PosInstalaciones"></param>
        /// <param name="MyAlgorithm"></param>
        /// <returns name="poskX"></returns>
        public static int DeterminarPosArgMax( List<int> PosInstalaciones, Algorithm MyAlgorithm)
        {         
            var  sumasGanaciasEliminacion = new List<KeyValuePair<int, double>>();
            for (var j = 0; j < PosInstalaciones.Count; j++)
            {
                double sumaMax = 0;
                for (var i = 0; i <MyAlgorithm.MyProblem.NumVertices; i++)
                {
                     sumaMax += MyAlgorithm.MyProblem.DistanciasFloyd[i][ PosInstalaciones[j]];
                }
                sumasGanaciasEliminacion.Add(new KeyValuePair<int,double>(PosInstalaciones[j],sumaMax));
            }
            var max = sumasGanaciasEliminacion.Max(x => x.Value);
            var poskX = sumasGanaciasEliminacion.Find(x => Math.Abs(x.Value - max) < 1e-10);
            return poskX.Key;
        }
    }
}
