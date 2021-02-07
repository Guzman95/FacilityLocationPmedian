using System;
using System.Collections.Generic;
using System.IO;

namespace BasedOnHarmony.Funciones
{
    public class PMediana
    {
        //Lee la direccion donde esta el problema
        
        private string RootDirectory = Path.GetFullPath("..\\..\\problemas\\");
        private string RootDirectory2 = Path.GetFullPath("..\\..\\ProblemasMatrizDistancias\\");
        //atributos para el dataset
        public int NumVertices;
        public int TotalAristas;
        public int OptimalLocation;
        public int PMedianas;

        public int NumVertices2;
        public int TotalAristas2;
        public int OptimalLocation2;
        public int PMedianas2;
        private readonly List<Arista> _aristas = new List<Arista>(); //Always should be sort by position
        public string FileName;
        public int[][] DistanciasFloyd;
        public int[][] DistanciasFloyd2;
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
            MatrisDistancias();
            Console.WriteLine(RootDirectory2 + fileName);
            leerDatos(RootDirectory2 + fileName);
            var res= compara(DistanciasFloyd, DistanciasFloyd2);
            Console.WriteLine(res);
            //GuardarMatriz(fileName);

            //ImprimirMatriz();
        }

        /// <summary>
        /// lee cada dataset para generar las propiedades de un problema
        /// </summary>
        /// <param name="fullFileName"></param>
        private void leerDatos(string fullFileName)
        {
            var lines = File.ReadAllLines(fullFileName);
            var firstline = lines[0].Split(',');
            NumVertices2 = int.Parse(firstline[0]);
            TotalAristas2 = int.Parse(firstline[1]);
            PMedianas2 = int.Parse(firstline[2]);
            OptimalLocation2 = int.Parse(firstline[3]);
            var positionLine = 1;
            DistanciasFloyd2 = new int[NumVertices][];
            for (var i = 0; i < NumVertices; i++)
            {
                var line = lines[positionLine++].Split(',');
                DistanciasFloyd2[i] = new int[NumVertices];
                for (var j = 0; j < NumVertices; j++)
                {
                    DistanciasFloyd2[i][j] = int.Parse(line[j]);
                }
            }
        }
        private void ReadFile(string fullFileName)
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

        /// <summary>
        ///genera la matriz de distancias para un problema
        /// </summary>
        /// <param name="pInstalaciones"></param>
        /// <returns></returns>

        private void MatrisDistancias() {
             
            DistanciasFloyd = new int[NumVertices][];
            for (var i = 0; i < NumVertices; i++) {
                DistanciasFloyd[i] = new int[NumVertices];
                for (var j = 0; j < NumVertices; j++) {
                    if (i == j) {
                        DistanciasFloyd[i][j] = 0;
                    }
                    else {
                        DistanciasFloyd[i][j]= inf;
                    }                    
                }
            }
            for (var i = 0; i < _aristas.Count; i++) {
                var  art = _aristas[i];
                DistanciasFloyd[art.VerticeInicial - 1][art.VerticeFinal - 1] = art.DistanciaArista;
                DistanciasFloyd[art.VerticeFinal - 1][art.VerticeInicial - 1] = art.DistanciaArista;
            }
            for(var k = 0; k < NumVertices; ++k)
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

        bool compara(int[][] a, int[][] b)
        {
            var la0 = a.GetLength(0);
            var lb0 = b.GetLength(0);


            if (la0 == lb0)
            {
                for (int f = 0; f != la0; ++f)
                    for (int c = 0; c != la0; ++c)
                        if (a[f][c] != b[f][c])
                            return false;
            }
            else
            {
                Console.WriteLine("Las matrices no se pueden comparar");
                return false;
            }

            return true;
        }


        private void GuardarMatriz(string fileName) {
            using (StreamWriter writer = new StreamWriter("F:\\UNIVERSIDAD\\TESIS\\FacilityLocationPmedian\\BasedOnHarmony\\BasedOnHarmony\\ProblemasMatrizDistancias\\" + fileName, false))
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
    }
}
