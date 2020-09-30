﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Monografia.Metaheuristicas.Armonicos
{
    class SBHS
    {
        public static int[][] InicializarMemoria()
        {
            var aleatorio = new Random(255);
            int[][] HarmonyMemory = new int[10][]; //HMS
            for (int i = 0; i < 10; i++)
            {
                HarmonyMemory[i] = new int[10]; //Tamaño de la población
                for (int j = 0; j < 10; j++)
                {
                    if (aleatorio.NextDouble() < 0.5)
                    {
                        HarmonyMemory[i][j] = 1;

                    }
                    else
                        HarmonyMemory[i][j] = 0;
                }
                //Console.WriteLine("Antes de reparar "+ HarmonyMemory[i][0]);
                reparar(HarmonyMemory[i]);
                //Console.WriteLine("Despues de reparar de reparar"+ HarmonyMemory[i][0]);
            }
            return HarmonyMemory;
        }

        private static void reparar(int[] vs)
        {
            double Vt = 0.0;
            if (!esFactible(vs))
            {
                Vt = volumenArmonia(vs);
            }
        }

        private static double volumenArmonia(int[] vs)
        {
            double sumar = 0.0;
            for (int i = 0; i < vs.Length; i++)
            {
                sumar += vs[i];
            }
            return sumar;
        }

        private static bool esFactible(int[] vs)
        {
            return false;
        }
    }
}
