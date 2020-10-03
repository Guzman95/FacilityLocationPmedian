using System;
using System.Collections.Generic;
using System.Linq;
using Monografia.Funciones;

namespace Monografia.Metaheuristicas
{
    public class Solution
    {
        public Algorithm MyAlgorithm;
        public p_mediana MyProblem;
        protected readonly int[] Objects; // {0, 1}
        protected double Weight;
        public double _fitness;

        public double Fitness => _fitness;

        public Solution(p_mediana theProblem, Algorithm theAlgorithm)
        {
            MyProblem = theProblem;
            MyAlgorithm = theAlgorithm;
            int tam = theProblem.numVertices;
            Objects = new int[MyProblem.totalAristas];
            Weight = 0;
            _fitness = 0;
        }

        //METODOS PRINCIPALES
        public int[][] inicializarPoblacion(int tamPoblacion, int n)
        {    // mover a solucion
            int[][] poblacion;
            Random randon = new Random(255);
            poblacion = new int[tamPoblacion][];
            //int n = MyProblem.numVertices;
            for (int i = 0; i < tamPoblacion; i++)
            {
                poblacion[i] = new int[n];
            }
            for (int i = 0; i < tamPoblacion; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (randon.NextDouble() < 0.5)
                    {
                        poblacion[i][j] = 0;
                    }
                    else
                    {
                        poblacion[i][j] = 1;
                    }
                }
            }
            return poblacion;
        }

        public int[] repararSolucion(int[] X, int n)
        { // mover a solucion
            int[] Xnew = new int[n];
            Xnew = X;
            return Xnew;
        }

        public double evaluarSolucion(int[] X)    // mover a solucion
        {
            double[] pesos = generarPesos(X.Length);
            double evaluacion = 0;
            for (int i = 0; i < X.Length; i++)
            {
                evaluacion = evaluacion + X[i] * pesos[i];
            }
            return evaluacion;
        }

        public int[] mejorOrganismo(int[][] poblacion, int n) // mover a solucion
        {
            int[] mejor = new int[n];
            int[] organismo = new int[n];
            double mejorEvaluacion = evaluarSolucion(poblacion[0]);
            for (int i = 1; i < poblacion.Length; i++)
            {
                organismo = poblacion[i];
                double evaluacion = evaluarSolucion(organismo);
                if (evaluacion < mejorEvaluacion)
                {
                    mejor = organismo;
                }
            }
            return mejor;
        }

        public int posPeorOrganismo(int[][] poblacion) // mover a solucion
        {
            int[] organismo;
            int posPeor = 0;
            double peorEvaluacion = evaluarSolucion(poblacion[0]);
            for (int i = 1; i < poblacion.Length; i++)
            {
                organismo = poblacion[i];
                double evaluacion = evaluarSolucion(organismo);
                if (evaluacion > peorEvaluacion)
                {
                    posPeor = i;
                }
            }
            return posPeor;
        }
      
        public int posicionAleatoria(int pos, int tamPoblacion)
        {  // mover a solucion
            Random randon = new Random();
            int maxvalue = tamPoblacion; int minvalue = 0;
            int aleatorio;
            do
            {
                aleatorio = randon.Next(minvalue, maxvalue);
            } while (aleatorio.Equals(pos));
            return aleatorio;
        }

        public void imprimirOrganismo(int[] organismo,int n)
        {    //moversolucion
            for (int i = 0; i < n; i++)
            {
                Console.Write(" " + organismo[i]);
            }
        }

        public int[] generarOrganismoAleatorio(int n)
        {          //mover a solucion

            int[] organismo = new int[n];
            for (int i = 0; i < n; i++)
            {
                organismo[i] = valorAleatorio();
            }
            return organismo;
        }

        public int valorAleatorio()
        {  // mover a solucion
            int valor;
            Random randon = new Random();
            double alea = randon.NextDouble();
            if (alea < 0.5)
            {
                valor = 0;
            }
            else
            {
                valor = 1;
            }
            return valor;

        }

        public void imprimirpoblacion(int[][] poblacion, int  n)
        {   //mover a solucion
            Console.WriteLine("Poblacion");
            for (int i = 0; i < poblacion.Length; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(" ");
                    Console.Write(poblacion[i][j]);
                }
                Console.WriteLine(" ");

            }
        }

        public void imprimirdistancias()
        {
            int [][] distancias = MyProblem.floyd();
            Console.WriteLine("Distancias");
            for (int i = 0; i < distancias.Length; i++)
            {
                for (int j = 0; j < distancias.Length; j++)
                {
                    Console.Write(" ");
                    Console.Write(distancias[i][j]);
                }
                Console.WriteLine(" ");
            }
        }

        private double[] generarPesos(int n)
        {
            double[] pesos = new double[n];
            Random randon = new Random();
            for (int i = 0; i < n; i++)
            {
                pesos[i] = randon.NextDouble();
            }
            return pesos;
        }

        public void Evaluate()
        {
            _fitness = Weight <= MyProblem.p_medianas ? MyProblem.Evaluate(Objects) : double.NegativeInfinity;
            MyAlgorithm.EFOs++;
        }

        public override string ToString()
        {
            var result = "";
            for (var i = 0; i < MyProblem.totalAristas; i++)
                result = result + (Objects[i] + " ");
            result = result + " f = " + _fitness +  " w = " + Weight;
            return result;
        }

   

        public bool IsOptimalKnown()
        {
            return Math.Abs(Fitness - MyProblem.OptimalLocation) < 1e-10;
        }
        
    }
       
}