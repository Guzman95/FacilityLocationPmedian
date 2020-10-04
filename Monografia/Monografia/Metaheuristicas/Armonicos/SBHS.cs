using Monografia.Funciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monografia.Metaheuristicas.Armonicos
{
    class SBHS : Algorithm
    {
        Solution solucion;


        
        public int[] organismoAleatorio(Random myRandom, int n)
        {
            int[] organismo = new int[n];
            for (int i = 0; i < n; i++)
            {
                organismo[i] = valorAleatorio(myRandom);
            }
            return organismo;
        }
        private int valorAleatorio(Random myRandom)
        {
            int valor;
            double alea = myRandom.NextDouble();
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
        /*
        public double evaluarSolucion(int[] X)
        {
            double[] pesos = new double[] { 2.4, 4.6, 5.3, 5, 1.5, 2.4, 4, 3.2, 1, 0.5 };
            double evaluacion = 0;
            for (int i = 0; i < X.Length; i++)
            {
                evaluacion = evaluacion + X[i] * pesos[i];
            }
            return evaluacion;
        }
        */
        public override void Ejecutar(p_mediana theProblem, Random myRandom)
        {
            int n = theProblem.numVertices;
            double HMCR = 1 - (10 / n);
            int HMS = 5;
            int[][] HM = new int[HMS][];
            int NI= 100; //numero de iteraciones
            for(int i = 0; i < HMS; i++)
            {
                HM[i] = organismoAleatorio(myRandom, n);
                HM[i] = solucion.repararSolucion(HM[i]);
            }
            int k = 0;
            while(k < NI)
            {
                //Se crea la nueva armonia de tamaño n
                int[] Xnew = new int[n];
                for (int i = 0; i < n; i++)
                {
                    if(myRandom.NextDouble() <= HMCR)
                    {
                        //se escogen dos armonias de manera aleatoria
                        int[] r1 = HM[myRandom.Next(0, n - 1)];
                        int[] r2 = HM[myRandom.Next(0, n - 1)];
                        int[] mejor = solucion.mejorSolucion(HM);
                        Xnew[i] = r1[i] +( (-1) ^ (r1[i])) * Math.Abs(mejor[i]- r2[i]);

                    }
                    else
                    {
                        Xnew[i] = valorAleatorio(myRandom);
                    }

                }
                Xnew = solucion.repararSolucion(Xnew);
                if(solucion.evaluarSolucion(Xnew) < solucion.evaluarSolucion(peorHarmonia(HM)))
                {
                    int posPeor = solucion.posPeorSolucion(HM);
                    HM[posPeor] = Xnew;
                }
                k++;
            }
        }
        /*
        public int posPeorHarmonia(int[][] poblacion)
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
        */
        /*
        public int[] mejorHarmonia(int[][] memoriaArmonica)
        {
            int[] mejor = new int[memoriaArmonica[0].Length];
            int[] copia = new int[mejor.Length];
            double mejorEvaluacion = evaluarSolucion(memoriaArmonica[0]);
            for (int i = 1; i < memoriaArmonica.Length; i++)
            {
                copia = memoriaArmonica[i];
                double evaluacion = evaluarSolucion(copia);
                if (evaluacion < mejorEvaluacion)
                {
                    mejor = copia;
                }
            }
            return mejor;
        }
        */

        public int[] peorHarmonia(int[][] memoriaArmonica)
        {
            int[] peor = new int[memoriaArmonica[0].Length];
            int[] copia = new int[peor.Length];
            double mejorEvaluacion = solucion.evaluarSolucion(memoriaArmonica[0]);
            for (int i = 1; i < memoriaArmonica.Length; i++)
            {
                copia = memoriaArmonica[i];
                double evaluacion = solucion.evaluarSolucion(copia);
                if (evaluacion > mejorEvaluacion)
                {
                    peor = copia;
                }
            }
            return peor;
        }
    }
}

