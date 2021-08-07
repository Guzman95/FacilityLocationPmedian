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
                    var FileName = ArrayLine[0];
                    var Avg = double.Parse(ArrayLine[1]);
                    var Rpe = double.Parse(ArrayLine[2]);
                    var EfosAvg = double.Parse(ArrayLine[3]);
                    var Desviation = double.Parse(ArrayLine[4]);
                    var PorcSucces = double.Parse(ArrayLine[5]);
                    var TimeAvg = double.Parse(ArrayLine[6]);
                    var TimeReal = double.Parse(ArrayLine[7]);
                    var PersProblem = new PersistenceProblem(FileName, Avg, Rpe, Desviation, EfosAvg, PorcSucces, TimeAvg, TimeReal);
                    dataPersistence.Add(PersProblem);
                }
            }
            return dataPersistence;
        }
        public static Solution LocalSearch(Solution solution, int valor) {
            Console.WriteLine("Antes");
            solution.Imprimir();
            var newSolution = CopySolution(solution);
            List<int> unviableNeighborsOn = new List<int>();
            List<int> unviableNeighborsOff = new List<int>();
            for (var k = 0; k < valor; k++)
            {
                var a = selectInstalacion(unviableNeighborsOff, solution, 0);   // Aleatorio controlado para no repetir
                if (a == -1)
                {
                    return solution;
                }
                var b = NearestNeighbor(unviableNeighborsOn, solution, a);// Aleatorio controlado para no repetir
                newSolution.InActivar(a);
                newSolution.Activar(b);
                newSolution.Evaluate();

                if (newSolution.Fitness < solution.Fitness)
                {
                    solution.InActivar(a);
                    solution.Activar(b);
                    solution.Evaluate();
                    unviableNeighborsOff.Add(a);
                }
                else
                {
                    unviableNeighborsOff.Add(a);
                    unviableNeighborsOn.Add(b);
                    newSolution.InActivar(b);
                    newSolution.Activar(a);
                }
            }
            Console.WriteLine("Despues");
            solution.Imprimir();
            return solution;
        }
        public static Solution LocalSearchGen(Solution solution, Solution best)
        {
            Console.WriteLine("\nAntes");
            solution.Imprimir();
            Console.WriteLine("Best");
            best.Imprimir();
            List<int> listSecondGen = new List<int>();
            var newsolution = CopySolution(solution);
            Console.WriteLine("Antes copia");
            solution.Imprimir();
            Console.WriteLine("\n");
            for (var i = 0; i < solution.MyAlgorithm.MyProblem.NumVertices; i++)
            {
                if (newsolution.Vertices[i] != best.Vertices[i]) {
                    if (best.Vertices[i] == 1) newsolution.Activar(i);
                    listSecondGen.Add(i);
                }
            }
            Console.WriteLine("Mutado");
            newsolution.Imprimir();
            newsolution = RepairOperatorLocalSearch(newsolution, listSecondGen);
            Console.WriteLine("Reparado");
            newsolution.Imprimir();
            newsolution.Evaluate();
            if (newsolution.Fitness < solution.Fitness)
            {
                return newsolution;
            }
            Console.WriteLine("Despues");
            solution.Imprimir();
            return solution;
        }
        public static Solution RepairOperatorLocalSearch(Solution solution, List<int> listSecondGen)
        {
            Random rnd = new Random();
            while (solution.PosInstalaciones.Count > solution.MyAlgorithm.MyProblem.PMedianas)
            {
                Console.WriteLine("postinslaciones: "+ solution.PosInstalaciones.Count);
                solution.Imprimir();
                for (var j = 0; j < listSecondGen.Count; j++) {
                    Console.WriteLine(listSecondGen[j]+"-");
                }
                int posSeconfGen = listSecondGen[ rnd.Next(listSecondGen.Count)];

                Console.WriteLine("posSeconfGen", posSeconfGen);
                solution.InActivar(posSeconfGen);
                listSecondGen.Remove(posSeconfGen);
            }
            return solution;
        }
        public static int NearestNeighbor(List<int> unviableNeighborsOn, Solution solution, int a)
        {
            var distancias = new List<KeyValuePair<int, double>>();

            for (var i = 0; i < solution.MyAlgorithm.MyProblem.NumVertices; i++)
            {

                if (solution.PosInstalaciones.Contains(i) == false && unviableNeighborsOn.Contains(i) == false)
                {
                    var dist = solution.MyAlgorithm.MyProblem.DistanciasFloyd[a][i];
                    distancias.Add(new KeyValuePair<int, double>(i, dist));
                }
            }
            var min = distancias.Min(x => x.Value);
            var posk = distancias.Find(x => Math.Abs(x.Value - min) < 1e-10);
            return posk.Key;
        }

        public static int selectInstalacion(List<int> unviableNeighborsOff, Solution solution, int state)
        {
            int pos ;
            do
            {
                if (unviableNeighborsOff.Count == solution.MyAlgorithm.MyProblem.PMedianas) { return -1; }

                pos = solution.MyAlgorithm.MyRandom.Next(solution.MyAlgorithm.MyProblem.NumVertices);
            } while ((solution.Vertices[pos] == state) || (unviableNeighborsOff.Contains(pos)));
            return pos;
        }
        public static int SelectOffIstalation(List<int> unviableNeighborsOff, Solution solution) { 
            var distancias = new List<KeyValuePair<int, double>>();
            for (var t = 0; t < solution.PosInstalaciones.Count; t++)
            {
                if (unviableNeighborsOff.Contains(solution.PosInstalaciones[t]) == false)
                {
                    var summ = 0;
                    for (var i = 0; i < solution.MyAlgorithm.MyProblem.NumVertices; i++)
                    {

                        summ += solution.MyAlgorithm.MyProblem.DistanciasFloyd[i][solution.PosInstalaciones[t]];
                    }
                    distancias.Add(new KeyValuePair<int, double>(solution.PosInstalaciones[t], summ));
                }
            }
            var mas = distancias.Max(x => x.Value);
            var posk = distancias.Find(x => Math.Abs(x.Value - mas) < 1e-10);
            return posk.Key;
        }

        public static Solution CopySolution(Solution solution) {
            var solutionCopy = new Solution(solution.MyAlgorithm);
            for (var i = 0; i < solution.PosInstalaciones.Count; i++)
            {
                solutionCopy.Activar(solution.PosInstalaciones[i]);
            }
            return solutionCopy;
        }
    }
}
