using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monografia.Funciones
{
    public class p_mediana
    {
        
        private const string RootDirectory = "F://UNIVERSIDAD//TESIS//FacilityLocationPmedian//Monografia//Monografia//problemas//";
        //private const string RootDirectory = "C://Users//santi//Desktop//FacilityLocationPmedian//Monografia//Monografia//problemas//";
        public int numVertices;
        public int totalAristas;
        public int OptimalLocation;
        public int p_medianas;
        private readonly List<Arista> aristas = new List<Arista>(); //Always should be sort by position
        public string FileName;
        public int[][] distanciasFloyd;
        public const int cst = 9999;

        public p_mediana(string fileName)
        {
            FileName = fileName; 
            ReadFile(RootDirectory + fileName);
            MatrisDistancias();
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

        public double Evaluate(List<int> pInstalaciones)
        {
            double summ = 0.0; 
            int i;
            for (i = 0; i < numVertices; i++) {               
                summ = summ + distanciaMenorPuntoDemanda(i, pInstalaciones);
            }
            return summ;
        }
        public double distanciaMenorPuntoDemanda(int puntoDemanda, List<int> pInstalaciones ) {
            double distanciaMenor = 0;
            double distancia = 0;
            distanciaMenor = distanciasFloyd[puntoDemanda][pInstalaciones[0]];
            for (int t = 1; t < pInstalaciones.Count; t ++)
            {
                distancia = distanciasFloyd[puntoDemanda][pInstalaciones[t]];
                if (distancia < distanciaMenor)
                {
                    distanciaMenor = distancia;
                }
            }
            return distanciaMenor;
        }
        public void MatrisDistancias() {
             
            distanciasFloyd = new int[numVertices][];
            for (int i = 0; i < numVertices; i++) {
                distanciasFloyd[i] = new int[numVertices];
                for (int j = 0; j < numVertices; j++) {
                    if (i == j) {
                        distanciasFloyd[i][j] = 0;
                    }
                    else {
                        distanciasFloyd[i][j]= cst;
                    }
                    
                }
            }

            for (int i = 0; i < aristas.Count; i++) {
                Arista  art = aristas[i];
                distanciasFloyd[art.verticeInicial - 1][art.verticeFinal - 1] = art.distanciaArista;
            }
            for(int k = 0; k < numVertices; ++k)
            {
                for (int i = 0; i < numVertices; ++i)
                {
                    for (int j = 0; j < numVertices; ++j)
                    {
                        if (distanciasFloyd[i][k] + distanciasFloyd[k][j] < distanciasFloyd[i][j])
                            distanciasFloyd[i][j] = distanciasFloyd[i][k] + distanciasFloyd[k][j];
                    }
                }
            }      

        }

        
        public double distanciaFloydArista(int demanda, int instalacion)
        {
            return distanciasFloyd[demanda][instalacion];
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
