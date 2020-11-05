﻿using Monografia.Funciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monografia.Metaheuristicas.Armonicos
{

    class HSOS : Algorithm
    {     
      
        public int tamPoblacion;
        public int[][] poblacion;
        int dimensionOrganismo;
        Solution solution;
        Random myRamdonL;

        //FASE DE MUTUALISMO
        private void faseMutualismo(int[] Xi, int[] Xj, int[] mejor, int indiceXi){
            int[] mutualvector = new int[dimensionOrganismo];
            int[] a;
            int[] b;
            Random randon = new Random();

            for (int k = 0; k < dimensionOrganismo; k++){
                if (randon.NextDouble() > 0.5){
                    mutualvector[k] = Xi[k];
                }
                else {
                    mutualvector[k] = Xj[k];
                }

            }
            a = solution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++){
                if (mutualvector[k] == mejor[k]){
                    a[k] = mejor[k];
                }
            }
            b = solution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++){
                if (a[k] == Xi[k]){
                    b[k] = Xi[k];
                }
            }
            b = solution.repararSolucion(b);
            if (solution.evaluarSolucion(b) <solution.evaluarSolucion(Xi)){
                poblacion[indiceXi] = b;
            }
        }

        //FASE DE COMENSALISMO
        private void faceComensalismo(int[] Xi, int[] Xj, int[] Xmejor,int indiceXi) {
            int[] c; int[] d;
            c = solution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++) {
                if (Xj[k] == Xmejor[k]) {
                    c[k] = Xmejor[k];
                }
            }
            d = solution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++) {
                if (c[k] == Xi[k]) {
                    d[k] = Xi[k];
                }
            }
            d = solution.repararSolucion(d);
            if (solution.evaluarSolucion(d) < solution.evaluarSolucion(Xi)) {
                poblacion[indiceXi] = d;
            }
        }
        //FASE DE PARASITISMO
        private void faceParasitismo(int []Xi, int []Xj, int indiceXj) {
            int[] vectorParasito = Xi;
            int[] nDimenciones = dimesionesAleatoria();
            int pos;
            for (int k = 0; k < nDimenciones.Length; k++) 
            {
                pos = nDimenciones[k];
                if (vectorParasito[pos] == 1) {
                    vectorParasito[pos] = 0;
                }
                else { 
                    vectorParasito[pos] = 1;
                }
            }
            vectorParasito = solution.repararSolucion(vectorParasito);
            if (solution.evaluarSolucion(vectorParasito) < solution.evaluarSolucion(Xj)) {
                poblacion[indiceXj] = vectorParasito;
            }
        }
        //FASE DE ARMONIA
        private void faceArmonia(int[] Xi, int indiceXi) {
            Random randon = new Random();
            double HMCR = 0.657;
            double PAR = 0.365;
            int[] neko = new int[dimensionOrganismo];
            int posPeor;
            int posAleatoria;

            for (int k = 0; k < dimensionOrganismo; k++) {
                if (randon.NextDouble() <= HMCR){
                    posAleatoria = solution.posSolucionAleatoria(indiceXi, tamPoblacion, myRamdonL);
                    neko[k] = poblacion[posAleatoria][k];
                    if (randon.NextDouble() <= PAR){
                        if (randon.NextDouble() > 0.5){
                            neko[k] = 0;
                        }
                        else {
                            neko[k] = 1;
                        }
                    }
                }
                else {
                    neko[k] = solution.valorAleatorio(myRamdonL);
                }
            }
            neko = solution.repararSolucion(neko);
            posPeor = solution.posPeorSolucion(poblacion);
            double evaluacionNeko = solution.evaluarSolucion(neko);
            if (evaluacionNeko < solution.evaluarSolucion(poblacion[posPeor])) {
                poblacion[posPeor] = neko;
            }
            if (evaluacionNeko < solution.evaluarSolucion(Xi)) {
                poblacion[indiceXi] = neko;
            }
        }

        private int[] dimesionesAleatoria() { 

            Random randon = new Random();
            int maxvalue = dimensionOrganismo; int minvalue = 1;
            int numerodimencionesN = randon.Next(minvalue, maxvalue);
            int[] dimencionesN = new int[numerodimencionesN];
            int valor;

            for (int i = 0; i < numerodimencionesN; i++)
            {
                valor = randon.Next(minvalue, maxvalue);
                dimencionesN[i] = valor - 1;

            }
            return dimencionesN;
        }

        public override void Ejecutar(p_mediana theProblem, Random myRandom) {
            solution = new Solution(theProblem, this);
            myRamdonL = myRandom;
            dimensionOrganismo = theProblem.numVertices;
            tamPoblacion = 5;
            int iter = 0;
            int itermax = 10;
            int j;
            poblacion = new int[tamPoblacion][];
            int[] xi;
            int[] xj;
            int[] mejor;
            for (int i = 0; i < tamPoblacion; i++)
            {
                poblacion[i] = solution.generarSolucionAleatorio(dimensionOrganismo, myRandom);
            }

            Console.WriteLine("\nPoblacion Inicial");
            solution.imprimirpoblacion(poblacion, dimensionOrganismo);
            Console.WriteLine(" ");
            mejor = solution.mejorSolucion(poblacion);
            Console.WriteLine("\nMejor Inicial");
            solution.imprimirSolucion(mejor);
            solution.Evaluate(mejor);

            while (iter < itermax)
            {
                Console.WriteLine("Iteracion" + iter);
                for (int i = 0; i < tamPoblacion; i++)
                {
                    xi = poblacion[i];

                    j = solution.posSolucionAleatoria(i, tamPoblacion, myRamdonL);
                    xj = poblacion[j];
                    faseMutualismo(xi, xj, mejor, i);
                    faseMutualismo(xj, xi, mejor, j);

                    j = solution.posSolucionAleatoria(i, tamPoblacion, myRamdonL);
                    xj = poblacion[j];
                    faceComensalismo(xi, xj, mejor, i);

                    j = solution.posSolucionAleatoria(i, tamPoblacion, myRamdonL);
                    xj = poblacion[j];
                    faceParasitismo(xi, xj, j);
                    faceArmonia(xi, i);

                    Console.WriteLine("poblacion local organismo: " + i);
                    solution.imprimirpoblacion(poblacion, dimensionOrganismo);
                    Console.WriteLine(" ");
                    mejor = solution.mejorSolucion(poblacion);
                    Console.WriteLine("\nMejor Local");
                    solution.imprimirSolucion(mejor);
                    solution.Evaluate(mejor);
                }
                iter++;
                Console.WriteLine("\nPoblacicon Global iteracion:"+ iter);
                solution.imprimirpoblacion(poblacion, dimensionOrganismo);
                Console.WriteLine(" ");
                Console.WriteLine("\nMejor Global");
                solution.imprimirSolucion(mejor);
                solution.Evaluate(mejor);
            }
        }
    }

}
