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
            Objects = new int[MyProblem.totalAristas];
            Weight = 0;
            _fitness = 0;
        }

        //METODOS PRINCIPALES
        public int[][] inicializarPoblacion(int tamPoblacion)
        {    // mover a solucion
            int[][] poblacion;
            int dimensionSolucion = MyProblem.numVertices;
            poblacion = new int[tamPoblacion][];
            //int n = MyProblem.numVertices;
            for (int i = 0; i < tamPoblacion; i++)
            {
                poblacion[i] = generarSolucionAleatorio(dimensionSolucion);
            }
            return poblacion;
        }

        public int[] repararSolucion(int[] X)
        { // mover a solucion
            int[] Xnew = new int[X.Length];
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

        public int[] mejorSolucion(int[][] poblacion) // mover a solucion
        {
            int[] mejor = new int[poblacion[0].Length];
            int[] solucion ;
            double mejorEvaluacion = evaluarSolucion(poblacion[0]);
            for (int i = 1; i < poblacion.Length; i++)
            {
                solucion = poblacion[i];
                double evaluacion = evaluarSolucion(solucion);
                if (evaluacion < mejorEvaluacion)
                {
                    mejor = solucion;
                }
            }
            return mejor;
        }

        public int posPeorSolucion(int[][] poblacion) // mover a solucion
        {
            int[] solucion;
            int posPeor = 0;
            double peorEvaluacion = evaluarSolucion(poblacion[0]);
            for (int i = 1; i < poblacion.Length; i++)
            {
                solucion = poblacion[i];
                double evaluacion = evaluarSolucion(solucion);
                if (evaluacion > peorEvaluacion)
                {
                    posPeor = i;
                }
            }
            return posPeor;
        }
      
        public int posSolucionAleatoria(int pos, int tamPoblacion)
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

        public void imprimirSolucion(int[] solucion){    //moversolucion
            for (int i = 0; i < solucion.Length; i++){
                Console.Write(" " + solucion[i]);
            }
        }

        public int[] generarSolucionAleatorio(int n){          //mover a solucion
            int[] solucion = new int[n];
            for (int i = 0; i < n; i++){
                solucion[i] = valorAleatorio();
            }
            return solucion;
        }

        public int valorAleatorio(){  // mover a solucion
            int valor;
            Random randon = new Random();
            double alea = randon.NextDouble();
            if (alea < 0.5){
                valor = 0;
            }
            else{
                valor = 1;
            }
            return valor;
        }

        public void imprimirpoblacion(int[][] poblacion, int  n){   //mover a solucion
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