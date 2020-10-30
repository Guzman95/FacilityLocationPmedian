using Monografia.Funciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monografia.Metaheuristicas.Armonicos
{
    class SBHS : Algorithm
    {


        
      
        public override void Ejecutar(p_mediana theProblem, Random myRandom)
        {
            //Hereda los metodos que se comparten con diferentes algoritmos implementados
            Solution solucion = new Solution(theProblem, this);
            int n = theProblem.numVertices;
            double HMCR = 1 - (10 / n);
            int HMS = 5;
            int[][] HM = new int[HMS][];
            int NI= 100; //numero de iteraciones
            int posPeor;
            int[] Xnew = new int[n];
            int[] r1 = new int[n];
            int[] r2 = new int[n];
            int[] mejor = new int[n];
            //Inicializar por metodo de probabilidad
            for (int i = 0; i < HMS; i++)
            {
                HM[i] = solucion.generarSolucionAleatorio(n, myRandom);

                HM[i] = solucion.repararSolucion(HM[i]);
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
                        r1 = HM[solucion.posSolucionAleatoria(0,HMS -1,myRandom)];
                        r2 = HM[solucion.posSolucionAleatoria(0, HMS -1, myRandom)];
                        mejor = solucion.mejorSolucion(HM);
                        //Simplifican la tasa de consideracion de memoria y el pitch adjusment por medio de la formula
                        Xnew[i] = (int)(r1[i] + Math.Pow((-1),(r1[i])) * Math.Abs(mejor[i] - r2[i]));
                    }
                    else
                    {
                        //se crea una nota con un valor al azar para ser diversificado
                        Xnew[i] = solucion.valorAleatorio(myRandom);
                    }

                }
                //Se debe reparar la solución ya que puede existir una nueva armonia no factible
                Xnew = solucion.repararSolucion(Xnew);
                //Evalua la nueva armonia con la peor solucion almacenada en la memoria armonica
                if(solucion.evaluarSolucion(Xnew) < solucion.evaluarSolucion(HM[solucion.posPeorSolucion(HM)]))
                {
                    posPeor = solucion.posPeorSolucion(HM);
                    HM[posPeor] = Xnew;
                    
                }
                k++;
            }
            solucion.imprimirpoblacion(HM, n);
        }

    }
}

