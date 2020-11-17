/*
using System;
using BasedOnHarmony.Funciones;

namespace BasedOnHarmony.Metaheuristicas.Armonicos
{
    class SBHS : Algorithm
    {
        public double[] evalPoblacion { get; private set; }

        public int posSolucionAleatoria(int PopulationSize, Random myRandon)
        {  // mover a solucion
            int maxvalue = PopulationSize; int minvalue = 0;
            int aleatorio;

            aleatorio = myRandon.Next(minvalue, maxvalue);
            //Console.WriteLine(aleatorio);
            return aleatorio;
        }

        public override void Ejecutar(PMediana theProblem, Random myRandom)
        {
            MyProblem = theProblem;
            MyRandom = myRandom;
            EFOs = 0;

            //Hereda los metodos que se comparten con diferentes algoritmos implementados
            //RandomOrganismInitialization solucion = new RandomOrganismInitialization(theProblem, this);
            Best = new RandomOrganismInitialization(theProblem, this);
            int n = theProblem.NumVertices;
            double HMCR = 1 - (10 / n);
            int HMS = 30;
            int[][] HM = new int[HMS][];
            int NI = 10000; //numero de iteraciones
            int posPeor;
            int[] Xnew = new int[n];
            int[] r1 = new int[n];
            int[] r2 = new int[n];
            int[] mejor = new int[n];
            int[] peor = new int[n];
            int posr1;
            int posr2;
            int posmejor;
            //Inicializar Population
            HM = Best.InitializeFixedPopulation(HMS, myRandom);
            //Se genera el vector de evaaluaci
            evalPoblacion = Best.evaluacionPoblacion(HM);
            //Determina el mejor para la primera interacion
            posmejor = Best.posMejorSolucion(evalPoblacion);
            mejor = HM[posmejor];
            int k = 0;
            while (k < NI)
            {
                //Se crea la nueva armonia de tamaño n
                for (int i = 0; i < n; i++)
                {
                    if (myRandom.NextDouble() <= HMCR)
                    {
                        //Console.Write("r1->"+r3);
                        posr1 = posSolucionAleatoria(HMS, myRandom);
                        posr2 = Best.posSolucionAleatoria(posr1, HMS, myRandom);
                        r1 = HM[posr1];
                        r2 = HM[posr2];
                        //Simplifican la tasa de consideracion de memoria y el pitch adjusment por medio de la formula
                        Xnew[i] = (int)(r1[i] + Math.Pow((-1), (r1[i])) * Math.Abs(mejor[i] - r2[i]));
                    }
                    else
                    {
                        //se crea una nota con un valor al azar para ser diversificado
                        Xnew[i] = Best.valorAleatorio(myRandom);
                    }
                }
                //Console.WriteLine(k);
                //Se debe reparar la solución ya que puede existir una nueva armonia no factible
                Xnew = Best.repararSolucionConocimiento(Xnew);
                //Xnew = Best.RepairSolutionRandomly(Xnew);
                //Evaluacion de la nueva Armonia
                Best.Evaluate(Xnew);
                double evalXnew = Best.Fitness;
				//Posiscion de la peor armonia
                posPeor = Best.posPeorSolucion(evalPoblacion);

                //Evalua la evaluacion de la nueva armonia con la evaluacion de la peor solucion de la memoria
                if (evalXnew < evalPoblacion[posPeor])
                {
                    //Si se cumple la condicion se remplaza la peor armonia por la nueva armonia 
                    HM[posPeor] = Xnew;
                    //Tambien se debe remplazar en el vector de evaluaciones la evaluacion del peor organismo con la evaluacion de la nueva
                    evalPoblacion[posPeor] = evalXnew;
                }
                //Se Determina la mejor armonia para la siguiente iteracion
                posmejor = Best.posMejorSolucion(evalPoblacion);
                mejor = HM[posmejor];
                //Se actualiza el contador de interaciones 
                k++;
            }
            //Se envia a evaluar el mejor organismo de la ejecucion 
            Best.Evaluate(mejor);
        }

    }
}

*/