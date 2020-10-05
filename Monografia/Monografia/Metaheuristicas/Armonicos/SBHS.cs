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
            for (int i = 0; i < HMS; i++)
            {
                HM[i] = solucion.generarSolucionAleatorio(n, myRandom);

                HM[i] = solucion.repararSolucion(HM[i]);
            }
            solucion.imprimirpoblacion(HM, n);
            int k = 0;
            while(k < NI)
            {
                //Se crea la nueva armonia de tamaño n
                
                for (int i = 0; i < n; i++)
                {
                    if(myRandom.NextDouble() <= HMCR)
                    {
                        //se escogen dos armonias de manera aleatoria
                        int r3 = solucion.posSolucionAleatoria(0, HMS, myRandom);
                        //Console.Write("r1->"+r3);
                        r1 = HM[solucion.posSolucionAleatoria(0,HMS -1,myRandom)];
                        r2 = HM[solucion.posSolucionAleatoria(0, HMS -1, myRandom)];
                        mejor = solucion.mejorSolucion(HM);
                        Xnew[i] = r1[i] + ((-1) ^ (r1[i])) * Math.Abs(mejor[i] - r2[i]);
                        Console.WriteLine(r1[i]);
                        Console.WriteLine(r2[i]);
                        Console.WriteLine(mejor[i]);




                    }
                    else
                    {
                        Xnew[i] = solucion.valorAleatorio(myRandom);
                    }

                }
                Xnew = solucion.repararSolucion(Xnew);
                if(solucion.evaluarSolucion(Xnew) < solucion.evaluarSolucion(HM[solucion.posPeorSolucion(HM)]))
                {
                    posPeor = solucion.posPeorSolucion(HM);
                    HM[posPeor] = Xnew;
                    Console.WriteLine(HM[posPeor]);
                }
                k++;
            }
            solucion.imprimirpoblacion(HM, n);
        }

    }
}

