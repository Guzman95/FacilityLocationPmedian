using System;
using System.Collections.Generic;
using System.IO;

namespace BasedOnHarmony.Funciones
{
    public class PMediana
    {
        //Lee la direccion donde esta el problema
        
        private string RootDirectory= Path.GetFullPath("..\\..\\ProblemasMatrizDistancias\\");
        //atributos para el dataset
        public int NumVertices;
        public int TotalAristas;
        public int OptimalLocation;
        public int PMedianas;
        public string FileName;
        public int[][] DistanciasFloyd;
        public const int inf = 99999 ;

        /// <summary>
        /// Constructor para leer y generar la matriz de distancias
        /// </summary>
        /// <param name="fileName"></param>
        public PMediana(string fileName)
        {
            FileName = fileName;
            Console.WriteLine(RootDirectory + fileName);
            ReadFile(RootDirectory + fileName);
            //ImprimirMatriz();
        }

        /// <summary>
        /// lee cada dataset para generar las propiedades de un problema
        /// </summary>
        /// <param name="fullFileName"></param>
        private void ReadFile(string fullFileName)
        {
            var lines = File.ReadAllLines(fullFileName);
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
