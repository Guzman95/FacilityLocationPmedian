﻿using Monografia.Funciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monografia.Metaheuristicas.Armonicos
{
    class SBHS : Algorithm
    {


        public int posSolucionAleatoria(int tamPoblacion, Random myRandon)
        {  // mover a solucion
            int maxvalue = tamPoblacion; int minvalue = 0;
            int aleatorio;
            
            aleatorio = myRandon.Next(minvalue, maxvalue);
            //Console.WriteLine(aleatorio);
            return aleatorio;
        }

        public override void Ejecutar(p_mediana theProblem, Random myRandom)
        {
            //Hereda los metodos que se comparten con diferentes algoritmos implementados
            //Solution solucion = new Solution(theProblem, this);
            BestSolution = new Solution(theProblem, this);
            int n = theProblem.numVertices;
            double HMCR = 1 - (10 / n);
            int HMS = 30;
            int[][] HM = new int[HMS][];
            int NI= 100; //numero de iteraciones
            int posPeor;
            int[] Xnew = new int[n];
            int[] r1 = new int[n];
            int[] r2 = new int[n];
            int[] mejor = new int[n];
            int posr1;
            int posr2;
            //Inicializar por metodo de probabilidad
            for (int i = 0; i < HMS; i++)
            {
                HM[i] = BestSolution.generarSolucionAleatorio(n, myRandom);

                HM[i] = BestSolution.repararSolucion(HM[i]);
            }
            int k = 0;
            while(k < NI)
            {
                //Se crea la nueva armonia de tamaño n
                for (int i = 0; i < n; i++)
                {
                    if(myRandom.NextDouble() <= HMCR)
                    {
                        //Console.Write("r1->"+r3);
                        posr1 = posSolucionAleatoria(HMS, myRandom);
                        posr2 = BestSolution.posSolucionAleatoria(posr1, HMS, myRandom);
                        r1 = HM[posr1];
                        r2 = HM[posr2];
                        mejor = BestSolution.mejorSolucion(HM);
                        //Simplifican la tasa de consideracion de memoria y el pitch adjusment por medio de la formula
                        Xnew[i] = (int)(r1[i] + Math.Pow((-1),(r1[i])) * Math.Abs(mejor[i] - r2[i]));
                    }
                    else
                    {
                        //se crea una nota con un valor al azar para ser diversificado
                        Xnew[i] = BestSolution.valorAleatorio(myRandom);
                    }

                }
                //Se debe reparar la solución ya que puede existir una nueva armonia no factible
                Xnew = BestSolution.repararSolucion(Xnew);
                //Evalua la nueva armonia con la peor solucion almacenada en la memoria armonica
                if(BestSolution.evaluarSolucion(Xnew) < BestSolution.evaluarSolucion(HM[BestSolution.posPeorSolucion(HM)]))
                {
                    posPeor = BestSolution.posPeorSolucion(HM);
                    HM[posPeor] = Xnew;
                    
                }
                k++;
                //solucion.Evaluate(solucion.mejorSolucion(HM));
            }
            BestSolution.Evaluate(BestSolution.mejorSolucion(HM));
            //solucion.imprimirpoblacion(HM, n);
        }

    }
}

