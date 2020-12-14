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
                var distancias = new List<double>();
                for (var t = 0; t < PosInstalaciones.Count; t++)
                {

                    distancias.Add(MyAlgorithm.MyProblem.DistanciasFloyd[i][PosInstalaciones[t]]);
                }
                menoresDistancias[i]= distancias.Min();
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
        public static int DeterminarPosArgMin(double[] menoresDistancias, Algorithm MyAlgorithm, int[] Vertices)
        {          
            var sumasPerdidasAdicion = new List<KeyValuePair<int, double>>();
            for (var j = 0; j < MyAlgorithm.MyProblem.NumVertices; j++)
            {
                if (Vertices[j] == 0)
                {
                    double sumaMin = 0;
                    for (var i = 0; i < menoresDistancias.Length; i++)
                    {
                        double distanciaIJ = MyAlgorithm.MyProblem.DistanciasFloyd[i][j];
                        double dif = distanciaIJ - menoresDistancias[i];
                        if (dif < 0)
                        {
                            sumaMin += dif * -1;
                        }
                    }
                    sumasPerdidasAdicion.Add(new KeyValuePair<int, double>(j, sumaMin));
                }
            }
            var max = sumasPerdidasAdicion.Max(x => x.Value);
            var poskX = sumasPerdidasAdicion.Find(x => Math.Abs(x.Value - max) < 1e-10);
            return poskX.Key;
        }
        /// <summary>
        ///  Determina la instalacion que al eliminarna aumente al mimimo el  valor la funcion objetivo  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <param name="PosInstalaciones"></param>
        /// <param name="MyAlgorithm"></param>
        /// <returns name="poskX"></returns>
        public static int DeterminarPosArgMax(double[] menoresDistancias, List<int> PosInstalaciones, Algorithm MyAlgorithm)
        {         
            var  sumasGanaciasEliminacion = new List<KeyValuePair<int, double>>();
            for (var j = 0; j < PosInstalaciones.Count; j++)
            {
                double sumaMin = 0;
                //Console.WriteLine("\nints" + PosInstalaciones.Count);
                List<int> copiapInstalaciones = new List<int>(PosInstalaciones);
                copiapInstalaciones.RemoveAt(j);
                for (var i = 0; i < menoresDistancias.Length; i++)
                {
                    var distancias = new List<double>();
                    for (var t = 0; t < copiapInstalaciones.Count; t++)
                    {
                        var dist = MyAlgorithm.MyProblem.DistanciasFloyd[i][copiapInstalaciones[t]];
                        //Console.WriteLine("dist: "+dist);
                        distancias.Add(dist);
                    }
                    var mini = distancias.Min();
                    //Console.WriteLine("mini: " + mini);
                    //Console.WriteLine("menoresDistancia "+i+": "+ menoresDistancias[i]);
                    var dif = mini - menoresDistancias[i] ;
                    //Console.WriteLine("dif: " + dif);
                    sumaMin += dif;
                    //Console.ReadKey();
                }
                //Console.WriteLine("sumamin: "+PosInstalaciones[j]+":"+ sumaMin);
                sumasGanaciasEliminacion.Add(new KeyValuePair<int,double>(PosInstalaciones[j],sumaMin));
                //Console.ReadKey();
            }
            var min = sumasGanaciasEliminacion.Min(x => x.Value);
            //Console.WriteLine("min:" + min);
            var poskX = sumasGanaciasEliminacion.Find(x => Math.Abs(x.Value - min) < 1e-10);
            return poskX.Key;
        }
    }
}
