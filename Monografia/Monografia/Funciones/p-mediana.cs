using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monografia.Funciones
{
    public class p_mediana
    {
        
        //private const string RootDirectory = "F://UNIVERSIDAD//TESIS//FacilityLocationPmedian//Monografia//Monografia//problemas//";
        private const string RootDirectory = "C://Users//santi//Desktop//FacilityLocationPmedian//Monografia//Monografia//problemas//";
        public int numVertices;
        public int totalAristas;
        public int OptimalLocation;
        public int p_medianas;
        private readonly List<Arista> aristas = new List<Arista>(); //Always should be sort by position
        public string FileName;
        public const int cst = 9999;

        public p_mediana(string fileName)
        {
            FileName = fileName; 
            ReadFile(RootDirectory + fileName);
            
           
        }

        public void ReadFile(string fullFileName)
        {
            //read the problem
            var lines = File.ReadAllLines(fullFileName);
            var firstline = lines[0].Split(';');
             numVertices = int.Parse(firstline[0]);
             totalAristas = int.Parse(firstline[1]);
             p_medianas = int.Parse(firstline[2]);
             OptimalLocation = int.Parse(firstline[3]);


            var positionLine = 1;
            for (var i = 0; i < totalAristas; i++)
            {
                var line = lines[positionLine++].Split(';');
                var verticeInicial = int.Parse(line[0]);
                var verticeFinal = int.Parse(line[1]);
                var distanciaArista = int.Parse(line[2]);
                var newAriasta = new Arista(i, verticeInicial, verticeFinal, distanciaArista);
                aristas.Add(newAriasta);

            }
        }

        public double Evaluate(int[] dim)
        {
            var summ = 0.0;
            for (var i = 0; i < totalAristas; i++)
                summ += dim[i] * aristas[i].distanciaArista;

            return summ;
        }
        public int[][] floyd() {
             
            int[][] matrizFloyd = new int[numVertices][];
            for (int i = 0; i < numVertices; i++) {
                matrizFloyd[i] = new int[numVertices];
                for (int j = 0; j < numVertices; j++) {
                    if (i == j) {
                        matrizFloyd[i][j] = 0;
                    }
                    else {
                        matrizFloyd[i][j]= cst;
                    }
                    
                }
            }

            for (int i = 0; i < aristas.Count; i++) {
                Arista  art = aristas[i];
                matrizFloyd[art.verticeInicial - 1][art.verticeFinal - 1] = art.distanciaArista;
            }
            for(int k = 0; k < numVertices; ++k)
            {
                for (int i = 0; i < numVertices; ++i)
                {
                    for (int j = 0; j < numVertices; ++j)
                    {
                        if (matrizFloyd[i][k] + matrizFloyd[k][j] < matrizFloyd[i][j])
                            matrizFloyd[i][j] = matrizFloyd[i][k] + matrizFloyd[k][j];
                    }
                }
            }
            return matrizFloyd;

        }

        
        public double distanciaArista(int index)
        {
            return aristas[index].distanciaArista;
        }
        public List<Arista> GetVariables()
        {
            return new List<Arista>(aristas);
        }

        public override string ToString()
        {
            var result = "p_medians:" + p_medianas.ToString("##0") + "\n" +
                   "OptimalLocation:" + OptimalLocation.ToString("##0.00") + "\n";

            for (var i = 0; i < totalAristas; i++)
                result += aristas[i] + "\n";
            return result;
        }
    }
}
