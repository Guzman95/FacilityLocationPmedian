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
                menoresDistancias[i] = distancias.Min();
            }
            return menoresDistancias;
        }
      
        /// <summary>
        ///  Determina las menores distancias de cada demanda a su instalacion masa cercana  
        /// </summary>
        /// <param name="PosInstalaciones"></param>
        /// <param name="MyAlgorithm"></param>
        /// <returns name="menoresDistancias"></returns>
        public static double[] ActualizarMenoresDistanciasAgregacion(Algorithm myalgorithm, int posk, double[] menoresdistancias)
        {
            for (var i = 0; i < myalgorithm.MyProblem.NumVertices; i++)
            {
                var dist = myalgorithm.MyProblem.DistanciasFloyd[i][posk];
                if (dist < menoresdistancias[i]) menoresdistancias[i] = dist;
            }
            return menoresdistancias;
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
                        var distanciaIJ = MyAlgorithm.MyProblem.DistanciasFloyd[i][j];
                        var dif = distanciaIJ - menoresDistancias[i];
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
        public static int DeterminarPosArgMax(double[] menoresdistancias, List<int> posInstalaciones, Algorithm myalgorithm)
        {
            var sumasganaciaseliminacion = new List<KeyValuePair<int, double>>();
            for (var j = 0; j < posInstalaciones.Count; j++)
            {
                double sumamin = 0;
                List<int> copiapinstalaciones = new List<int>(posInstalaciones);
                copiapinstalaciones.RemoveAt(j);
                for (var i = 0; i < menoresdistancias.Length; i++)
                {
                    var distancias = new List<double>();
                    for (var t = 0; t < copiapinstalaciones.Count; t++)
                    {
                        var dist = myalgorithm.MyProblem.DistanciasFloyd[i][copiapinstalaciones[t]];
                        distancias.Add(dist);
                    }
                    sumamin += distancias.Min() - menoresdistancias[i];
                }
                sumasganaciaseliminacion.Add(new KeyValuePair<int, double>(posInstalaciones[j], sumamin));
            }
            var min = sumasganaciaseliminacion.Min(x => x.Value);
            var poskx = sumasganaciaseliminacion.Find(x => Math.Abs(x.Value - min) < 1e-10);
            return poskx.Key;
        }


        ///Metodos para calculo de otra forma lo estoy probando 
        /*
        public static List<KeyValuePair<int, double>> DeterminarMenoresDistanciasX(Algorithm MyAlgorithm, List<int> PosInstalaciones)
        {

            var menoresDistancias = new List<KeyValuePair<int, double>>();
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                var distancias = new List<KeyValuePair<int, double>>();
                for (var t = 0; t < PosInstalaciones.Count; t++)
                {
                    var dist = MyAlgorithm.MyProblem.DistanciasFloyd[i][PosInstalaciones[t]];
                    distancias.Add(new KeyValuePair<int, double>(PosInstalaciones[t], dist));
                }
                var min = distancias.Min(x => x.Value);
                var post = distancias.Find(x => Math.Abs(x.Value - min) < 1e-10);
                menoresDistancias.Add(new KeyValuePair<int, double>(post.Key, min));
            }
            return menoresDistancias;
        }

        public static List<KeyValuePair<int, double>> ActualizarMenoresDistanciasAgregacion(Algorithm myalgorithm, int posk, List<KeyValuePair<int, double>> menoresdistancias)
        {
            for (var i = 0; i < myalgorithm.MyProblem.NumVertices; i++)
            {
                var dist = myalgorithm.MyProblem.DistanciasFloyd[i][posk];
                if (dist < menoresdistancias[i].Value) menoresdistancias[i] = new KeyValuePair<int, double>(posk, dist);
            }
            return menoresdistancias;
        }

        public static List<KeyValuePair<int, double>> ActualizarMenoresDistanciasEliminacion(Algorithm MyAlgorithm, int posk, List<KeyValuePair<int, double>> menoresDistancias, List<int> PosInstalaciones)
        {

            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                if (menoresDistancias[i].Key == posk)
                {
                    var distancias = new List<double>();
                    for (var t = 0; t < PosInstalaciones.Count; t++)
                    {
                        distancias.Add(MyAlgorithm.MyProblem.DistanciasFloyd[i][PosInstalaciones[t]]);
                    }
                    menoresDistancias[i] = new KeyValuePair<int, double>(i, distancias.Min());
                }
            }
            return menoresDistancias;
        }

        public static int determinarposargmin(List<KeyValuePair<int, double>> menoresdistancias, Algorithm myalgorithm, int[] vertices)
        {
            var sumasperdidasadicion = new List<KeyValuePair<int, double>>();
            for (var j = 0; j < myalgorithm.MyProblem.NumVertices; j++)
            {
                if (vertices[j] == 0)
                {
                    double sumamin = 0;
                    for (var i = 0; i < menoresdistancias.Count; i++)
                    {
                        var distanciaij = myalgorithm.MyProblem.DistanciasFloyd[i][j];
                        var dif = distanciaij - menoresdistancias[i].Value;
                        if (dif < 0)
                        {
                            sumamin += dif * -1;
                        }
                    }
                    sumasperdidasadicion.Add(new KeyValuePair<int, double>(j, sumamin));
                }
            }
            var max = sumasperdidasadicion.Max(x => x.Value);
            var poskx = sumasperdidasadicion.Find(x => Math.Abs(x.Value - max) < 1e-10);
            return poskx.Key;
        }

        public static int DeterminarPosArgMax(List<KeyValuePair<int, double>> menoresDistancias, List<int> PosInstalaciones, Algorithm MyAlgorithm)
        {
            var sumasGanaciasEliminacion = new List<KeyValuePair<int, double>>();
            for (var j = 0; j < PosInstalaciones.Count; j++)
            {
                double sumaMin = 0;
                List<int> copiapInstalaciones = new List<int>(PosInstalaciones);
                copiapInstalaciones.RemoveAt(j);
                for (var i = 0; i < menoresDistancias.Count; i++)
                {
                    var distancias = new List<double>();
                    for (var t = 0; t < copiapInstalaciones.Count; t++)
                    {
                        var dist = MyAlgorithm.MyProblem.DistanciasFloyd[i][copiapInstalaciones[t]];
                        distancias.Add(dist);
                    }
                    sumaMin += distancias.Min() - menoresDistancias[i].Value;
                }
                sumasGanaciasEliminacion.Add(new KeyValuePair<int, double>(PosInstalaciones[j], sumaMin));
            }
            var min = sumasGanaciasEliminacion.Min(x => x.Value);
            var poskX = sumasGanaciasEliminacion.Find(x => Math.Abs(x.Value - min) < 1e-10);
            return poskX.Key;
        } */
    }
}
