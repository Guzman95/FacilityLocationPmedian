using System;
using System.Collections.Generic;
using System.IO;

namespace BasedOnHarmony.Funciones
{
    public class PMediana
    {
        //Lee la direccion donde esta el problema
        private const string RootDirectory = "F://UNIVERSIDAD//TESIS//FacilityLocationPmedian//BasedOnHarmony//BasedOnHarmony//problemas//";
        //private const string RootDirectory = "C://Users//cobos//Desktop//FacilityLocation//BasedOnHarmony//BasedOnHarmony//problemas//";
        //private const string RootDirectory = "C://Users//santi//Desktop//FacilityLocationPmedian//BasedOnHarmony//BasedOnHarmony//problemas//";

        //atributos para el dataset
        public int NumVertices;
        public int TotalAristas;
        public int OptimalLocation;
        public int PMedianas;
        private readonly List<Arista> _aristas = new List<Arista>(); //Always should be sort by position
        public string FileName;
        public int[][] DistanciasFloyd;
        public const int Cst = 9999;

        /// <summary>
        /// Constructor para leer y generar la matriz de distancias
        /// </summary>
        /// <param name="fileName"></param>
        public PMediana(string fileName)
        {
            FileName = fileName; 
            ReadFile(RootDirectory + fileName);
            MatrisDistancias();
        }

        /// <summary>
        /// lee cada dataset para generar las aristas
        /// </summary>
        /// <param name="fullFileName"></param>
        public void ReadFile(string fullFileName)
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
        /// Evalua la solucion de ir a un punto de demanda i
        /// </summary>
        /// <param name="pInstalaciones"></param>
        /// <returns></returns>
        public double Evaluate(List<int> pInstalaciones)
        {
            var summ = 0.0; 
            for (var i = 0; i < NumVertices; i++) {               
                summ += DistanciaMenorPuntoDemanda(i, pInstalaciones);
            }
            return summ;
        }

        //obtiene la menor distancia de un punto de demanda a los demas nodos
        public double DistanciaMenorPuntoDemanda(int puntoDemanda, List<int> pInstalaciones ) {
            double distanciaMenor = DistanciasFloyd[puntoDemanda][pInstalaciones[0]];
            for (var t = 1; t < pInstalaciones.Count; t++)
            {
                double distancia = DistanciasFloyd[puntoDemanda][pInstalaciones[t]];
                if (distancia < distanciaMenor)
                {
                    distanciaMenor = distancia;
                }
            }
            return distanciaMenor;
        }

        //genera la matriz de distancias
        public void MatrisDistancias() {
             
            DistanciasFloyd = new int[NumVertices][];
            for (var i = 0; i < NumVertices; i++) {
                DistanciasFloyd[i] = new int[NumVertices];
                for (var j = 0; j < NumVertices; j++) {
                    if (i == j) {
                        DistanciasFloyd[i][j] = 0;
                    }
                    else {
                        DistanciasFloyd[i][j]= Cst;
                    }
                    
                }
            }

            for (var i = 0; i < _aristas.Count; i++) {
                var  art = _aristas[i];
                DistanciasFloyd[art.VerticeFinal - 1][art.VerticeInicial - 1] = art.DistanciaArista;
            }
            for(var k = 0; k < NumVertices; ++k)
            {
                for (var i = 0; i < NumVertices; ++i)
                {
                    for (var j = 0; j < NumVertices; ++j)
                    {
                        var sumaIkKj = DistanciasFloyd[i][k] + DistanciasFloyd[k][j];
                        if (DistanciasFloyd[j][i] > sumaIkKj)
                            DistanciasFloyd[j][i] = sumaIkKj;
                    }
                }
            }      

        }

        //retorna la distancia que hay de un punto de demanda a su instalacion
        public double DistanciaFloydArista(int demanda, int instalacion)
        {
            return DistanciasFloyd[demanda][instalacion];
        }

        public List<Arista> GetVariables()
        {
            return new List<Arista>(_aristas);
        }

        public override string ToString()
        {
            var result = "p_medians:" + PMedianas.ToString("##0") + "\n" +
                   "OptimalLocation:" + OptimalLocation.ToString("##0.00") + "\n";

            for (var i = 0; i < TotalAristas; i++)
                result += _aristas[i] + "\n";
            return result;
        }

        //Imprime la matriz de distancias
        public void ImprimirMatriz()
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
    }
}
