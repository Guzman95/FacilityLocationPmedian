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
                        int[] r1 = HM[solucion.posSolucionAleatoria(0,n)];
                        int[] r2 = HM[solucion.posSolucionAleatoria(0, n)];
                        int[] mejor = solucion.mejorSolucion(HM);
                        Xnew[i] = r1[i] +( (-1) ^ (r1[i])) * Math.Abs(mejor[i]- r2[i]);

                    }
                    else
                    {
                        Xnew[i] = valorAleatorio(myRandom);
                    }

                }
                Xnew = solucion.repararSolucion(Xnew);
                if(solucion.evaluarSolucion(Xnew) < solucion.evaluarSolucion(HM[solucion.posPeorSolucion(HM)]))
                {
                    int posPeor = solucion.posPeorSolucion(HM);
                    HM[posPeor] = Xnew;
                }
                k++;
            }
        }

    }
}

