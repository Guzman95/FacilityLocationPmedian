using BasedOnHarmony.Funciones;
using BasedOnHarmony.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BasedOnHarmony.Metaheuristicas
{
    class Utils
    {

        /// <summary>
        /// Obtiene una conjunto aleatorio de posiones aleatorias de acuerdo a la solucion 
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
        /// Obtiena una conjunto determinado de posiones aleatorias de acuerdo a la solucion  
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
        /// Seleciona aleatoriamente  una posicion de instalacion para ser agregada a la solucion
        /// </summary>
        /// <param name="state"></param>
        /// <returns name="posAleatoria"></returns>
        public static int VerticeValidation(int[] Vertices, Algorithm MyAlgorithm, int state)
        {
            int posAleatoria;
            do
            {
                posAleatoria = MyAlgorithm.MyRandom.Next(MyAlgorithm.MyProblem.NumVertices);
            } while (Vertices[posAleatoria] == state);
            return posAleatoria;
        }

        /// <summary>
        ///  Determina las menor distancia de cada demanda a  una instalacion mas cercana  
        /// </summary>
        /// <param name="PosInstalaciones"></param>
        /// <param name="MyAlgorithm"></param>
        /// <returns name="menoresDistancias"></returns>
        public static List<KeyValuePair<int, double>> DeterminarMenoresDistanciasX(Algorithm MyAlgorithm, List<int> PosInstalaciones)
        {

            var menoresDistancias = new List<KeyValuePair<int, double>>();
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                var disMin = 99999999;
                var PosInst = -1;
                for (var t = 0; t < PosInstalaciones.Count; t++)
                {
                    var dist = MyAlgorithm.MyProblem.DistanciasFloyd[i][PosInstalaciones[t]];
                    if (dist < disMin) disMin = dist; PosInst = PosInstalaciones[t];
                }
                menoresDistancias.Add(new KeyValuePair<int, double>(PosInst, disMin));
            }
            return menoresDistancias;
        }

        /// <summary>
        ///  Actualiza la menor distancia de cada una de las demandas con la nueva  instalacion  
        /// </summary>
        /// <param name="PosInstalaciones"></param>
        /// <param name="MyAlgorithm"></param>
        /// <returns name="menoresDistancias"></returns>
        public static List<KeyValuePair<int, double>> ActualizarMenoresDistanciasAgregacion(Algorithm myalgorithm, int posk, List<KeyValuePair<int, double>> menoresdistancias)
        {
            for (var i = 0; i < myalgorithm.MyProblem.NumVertices; i++)
            {
                var dist = myalgorithm.MyProblem.DistanciasFloyd[i][posk];
                if (dist < menoresdistancias[i].Value) menoresdistancias[i] = new KeyValuePair<int, double>(posk, dist);
            }
            return menoresdistancias;
        }
        /// <summary>
        ///  Actualiza  la menor distancia de las  demandas de la instalacion eliminada   
        /// </summary>
        /// <param name="PosInstalaciones"></param>
        /// <param name="MyAlgorithm"></param>
        /// <returns name="menoresDistancias"></returns>

        public static List<KeyValuePair<int, double>> ActualizarMenoresDistanciasEliminacion(Algorithm MyAlgorithm, int posk, List<KeyValuePair<int, double>> menoresDistancias, List<int> PosInstalaciones)
        {
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
            {
                if (menoresDistancias[i].Key == posk)
                {
                    var disMin = 99999999;
                    var PosInst = -1;
                    for (var t = 0; t < PosInstalaciones.Count; t++)
                    {
                        var dist = MyAlgorithm.MyProblem.DistanciasFloyd[i][PosInstalaciones[t]];
                        if (dist < disMin) disMin = dist; PosInst = PosInstalaciones[t];
                    }
                    menoresDistancias[i] = new KeyValuePair<int, double>(PosInst, disMin);
                }
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
        public static int DeterminarPosArgMax(List<KeyValuePair<int, double>> menoresdistancias, Algorithm myalgorithm, int[] vertices)
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

        /// <summary>
        ///  Determina la instalacion que al eliminarna aumente al mimimo el  valor la funcion objetivo  
        /// </summary>
        /// <param name="menoresDistancias"></param>
        /// <param name="PosInstalaciones"></param>
        /// <param name="MyAlgorithm"></param>
        /// <returns name="poskX"></returns>
        public static int DeterminarPosArgMin(List<KeyValuePair<int, double>> menoresDistancias, List<int> PosInstalaciones, Algorithm MyAlgorithm)
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
        }

        public static List<PersistenceProblem> LoadPersistenceProblem()
        {
            List<PersistenceProblem> dataPersistence = new List<PersistenceProblem>();

            var RootDirectorData = Path.GetFullPath("..\\..\\data_persistence\\solution.txt");
            if (File.Exists(RootDirectorData))
            {
                var lines = File.ReadAllLines(RootDirectorData);
                foreach (var line in lines)
                {
                    var ArrayLine = line.Split(';');
                    var FileName = ArrayLine[0] ;
                    var Avg = double.Parse(ArrayLine[1]);
                    var Rpe = double.Parse(ArrayLine[2]);
                    var EfosAvg = double.Parse(ArrayLine[3]);
                    var Desviation = double.Parse(ArrayLine[4]);
                    var PorcSucces = double.Parse(ArrayLine[5]);
                    var TimeAvg = double.Parse(ArrayLine[6]);
                    var TimeReal = double.Parse(ArrayLine[7]);
                    var PersProblem =  new PersistenceProblem(FileName, Avg, Rpe, Desviation, EfosAvg, PorcSucces, TimeAvg, TimeReal);
                    dataPersistence.Add(PersProblem);
                }
            }
            return dataPersistence;
        }

    }
}
