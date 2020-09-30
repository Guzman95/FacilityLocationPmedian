using Monografia.Funciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monografia.Metaheuristicas.Armonicos
{
    class HSOS : Algorithm
    {
        Random randon = new Random(255);
        private double HMCR = 0.05;
        private double PAR = 0.3;
        public int n = 10;
        private int tamPoblacion = 5;
        int[][] poblacion;
        int[][] distancias;
        public void HibridoOrganismosSimbioticos(p_mediana theProblem){
             distancias =  theProblem.floyd();

            int[] mejor = new int[n];
            int iter = 0;
            int itermax = 300;
            poblacion = new int[tamPoblacion][];
            for (int i = 0; i < tamPoblacion; i++){
                poblacion[i] = new int[n];
            }
            for (int i = 0; i < tamPoblacion; i++){
                for (int j = 0; j < n; j++){
                    if (randon.NextDouble() < 0.5){
                        poblacion[i][j] = 0;
                    }
                    else{
                        poblacion[i][j] = 1;
                    }
                }     
            }
            imprimirpoblacion();
            imprimirdistancias();
            /*
            while (iter < itermax)
            {
                int[] xi = new int[n];
                int[] xj = new int[n];
                mejor = mejorOrganismo(poblacion);
                for (int i = 0; i < tamPoblacion; i++)
                {
                    xi = poblacion[i];
                    int j = posicionAleatoria(i);
                    xj = poblacion[j];
                    faseMutualismo(xi, xj, mejor, i);
                    faseMutualismo(xj, xi, mejor, j);
                }
            }
            */
        }

        private void faseMutualismo(int[] Xi, int[] Xj, int[] mejor, int indiceXi)
        {
            int[] mutualvector = new int[n];
            int[] a = new int[n];
            int[] b = new int[n];
            Random randon = new Random(255);

            for (int k = 0; k < n; k++)
            {
                if (randon.NextDouble() > 0.5)
                {
                    mutualvector[k] = Xi[k];
                }
                else
                {
                    mutualvector[k] = Xj[k];
                }

            }
            a = organismoAleatorio();
            for (int k = 0; k < n; k++)
            {
                if (mutualvector[k] == mejor[k])
                {
                    a[k] = mejor[k];
                }
            }
            b = organismoAleatorio();
            for (int k = 0; k < n; k++)
            {
                if (a[k] == Xi[k])
                {
                    a[k] = Xi[k];
                }
            }
            a = reparar(a);
            if (evaluarSolucion(a) < evaluarSolucion(Xi))
            {
                poblacion[indiceXi] = a;
            }


        }

        private int[] reparar(int[] X)
        {
            int[] Xnew = new int[n];
            Xnew = X;
            return Xnew;
        }

        public int[] organismoAleatorio()
        {
            Random randon = new Random(255);
            int[] organismo = new int[n];
            for (int i = 0; i <= n; i++)
            {
                if (randon.NextDouble() < 0.5)
                {
                    organismo[i] = 0;
                }
                else
                {
                    organismo[i] = 1;
                }

            }
            return organismo;
        }

        public int[] mejorOrganismo(int[][] poblacion)
        {
            int[] mejor = new int[n];
            int[] organismo = new int[n];
            int mejorEvaluacion = 0;
            for (int i = 0; i <= n; i++)
            {
                organismo = poblacion[i];
                double evaluacion = evaluarSolucion(organismo);
                if (mejorEvaluacion > evaluacion)
                {
                    mejor = organismo;
                }
            }
            return mejor;
        }

        public double evaluarSolucion(int[] X)
        {
            double[] pesos = new double[] { 2.4, 4.6, 5.3, 5, 1.5, 2.4, 4, 3.2, 1, 0.5 };
            double evaluacion = 0;
            for (int i = 0; i <= n; i++)
            {
                evaluacion = X[i] * pesos[i];
            }
            return evaluacion;

        }

       
        private int posicionAleatoria(int pos){
            Random randon = new Random();
            int maxvalue = tamPoblacion; int minvalue = 1;
            int aleatorio = -1;
            while (aleatorio == pos)
            {
                aleatorio = randon.Next(minvalue, maxvalue);
            }
            return aleatorio;
        }

        private void imprimirpoblacion() {
            Console.WriteLine("Poblacion");
            for (int i = 0; i < tamPoblacion; i++) {
                for (int j = 0; j < n; j++){
                    Console.Write(" ");
                    Console.Write(poblacion[i][j]);
                }
                Console.WriteLine(" ");

            }
        }

        private void imprimirdistancias(){
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

        public override void Ejecutar(p_mediana theProblem, Random myRandom)
        {
            HibridoOrganismosSimbioticos(theProblem);   
        }
    }

}
