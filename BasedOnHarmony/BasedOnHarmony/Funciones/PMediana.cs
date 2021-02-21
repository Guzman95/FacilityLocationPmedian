using System;
using System.Collections.Generic;
using System.IO;

namespace BasedOnHarmony.Funciones
{
    public class PMediana
    {
        //Lee la direccion donde esta el problema
        
        private string RootDirectoryProblems= Path.GetFullPath("..\\..\\problemas\\");
        private string RootDirectoryarrays = Path.GetFullPath("..\\..\\matrices_distancia\\");
        //atributos para el dataset
        public int NumVertices;
        public int TotalAristas;
        public int OptimalLocation;
        public int PMedianas;
        public string FileName;
        public int[][] DistanciasFloyd;
        public const int inf = 99999 ;
        private readonly List<Arista> _aristas = new List<Arista>(); //Always should be sort by position

        /// <summary>
        /// Constructor para leer y generar la matriz de distancias
        /// </summary>
        /// <param name="fileName"></param>
        public PMediana(string fileName)
        {
            FileName = fileName;
            var problemArrayPath = RootDirectoryarrays + fileName;
            
            if (File.Exists(problemArrayPath))
            {
                ReadFileMatrix(problemArrayPath);
                Console.WriteLine(problemArrayPath);
            }
            else
            {
                if (!Directory.Exists(RootDirectoryarrays))
                {
                    Console.WriteLine("Creando el directorio: {0}", RootDirectoryarrays);
                    DirectoryInfo di = Directory.CreateDirectory(RootDirectoryarrays);
                }
                var problemDataPath = RootDirectoryProblems + fileName;
                if (File.Exists(problemDataPath))
                {
                    ReadFileData(problemDataPath);
                    CreateDistancesMatrix();
                    CreateFileMatrix(problemArrayPath);
                    Console.WriteLine(problemDataPath);
                }
                else Console.WriteLine("Archivo no existe");
            }
            //ImprimirMatriz();
        }

        /// <summary>
        /// lee cada dataset para generar las propiedades de un problema
        /// </summary>
        /// <param name="fullFileName"></param>
        private void ReadFileMatrix(string problemArrayPath)
        {
            
                var lines = File.ReadAllLines(problemArrayPath);
                var firstline = lines[0].Split(',');
                NumVertices = int.Parse(firstline[0]);
                TotalAristas = int.Parse(firstline[1]);
                PMedianas = int.Parse(firstline[2]);
                OptimalLocation = int.Parse(firstline[3]);
                var positionLine = 1;
                DistanciasFloyd = new int[NumVertices][];
                for (var i = 0; i < NumVertices; i++)
                {
                    var line = lines[positionLine++].Split(',');
                    DistanciasFloyd[i] = new int[NumVertices];
                    for (var j = 0; j < NumVertices; j++)
                    {
                        DistanciasFloyd[i][j] = int.Parse(line[j]);
                    }
                }   
        }

        private void ReadFileData(string fullFileName)
        {
            
            //read the problem
            var lines = File.ReadAllLines(fullFileName);
            var firstline = lines[0].Split(';');
            NumVertices = int.Parse(firstline[1]);
            TotalAristas = int.Parse(firstline[2]);
            PMedianas = int.Parse(firstline[3]);
            OptimalLocation = int.Parse(firstline[4]);
            var positionLine = 1;
            for (var i = 0; i < TotalAristas; i++)
            {
                var line = lines[positionLine++].Split(';');
                var verticeInicial = int.Parse(line[1]);
                var verticeFinal = int.Parse(line[2]);
                var distanciaArista = int.Parse(line[3]);
                var newAriasta = new Arista(i, verticeInicial, verticeFinal, distanciaArista);
                _aristas.Add(newAriasta);
            }
        }

        private void CreateDistancesMatrix()
        {

            DistanciasFloyd = new int[NumVertices][];
            for (var i = 0; i < NumVertices; i++)
            {
                DistanciasFloyd[i] = new int[NumVertices];
                for (var j = 0; j < NumVertices; j++)
                {
                    if (i == j)
                    {
                        DistanciasFloyd[i][j] = 0;
                    }
                    else
                    {
                        DistanciasFloyd[i][j] = inf;
                    }
                }
            }
            for (var i = 0; i < _aristas.Count; i++)
            {
                var art = _aristas[i];
                DistanciasFloyd[art.VerticeInicial - 1][art.VerticeFinal - 1] = art.DistanciaArista;
                DistanciasFloyd[art.VerticeFinal - 1][art.VerticeInicial - 1] = art.DistanciaArista;
            }
            for (var k = 0; k < NumVertices; ++k)
            {
                for (var i = 0; i < NumVertices; ++i)
                {
                    for (var j = 0; j < NumVertices; ++j)
                    {
                        var sumaIkKj = DistanciasFloyd[i][k] + DistanciasFloyd[k][j];
                        if (DistanciasFloyd[i][j] > sumaIkKj)
                            DistanciasFloyd[i][j] = sumaIkKj;
                    }
                }
            }
        }


        private void CreateFileMatrix(string problemArrayPath)
        {
            
            using (StreamWriter writer = new StreamWriter(problemArrayPath, false))
            {
                var datos = NumVertices + "," + TotalAristas + "," + PMedianas + "," + OptimalLocation;
                writer.WriteLine(datos);
                for (var c = 0; c < NumVertices; c++)
                {
                    var linea = "";
                    for (var f = 0; f < NumVertices; f++)
                    {
                        linea = linea + DistanciasFloyd[c][f] + ",";
                    }
                    writer.WriteLine(linea);
                }
            }
        }
        /// <summary>
        /// Imprime la matriz de distancias
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns
        private void ImprimirMatriz()
        {
            for (var i = 0; i < this.DistanciasFloyd.Length; i++)
            {
                for (var j = 0; j < this.DistanciasFloyd.Length; j++)
                {

                    Console.Write("-" + DistanciasFloyd[i][j]);
                }
                Console.WriteLine("\n");
            }
        }

        public override string ToString()
        {
            return FileName;
        }
    }
}
