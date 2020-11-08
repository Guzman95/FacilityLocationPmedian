using Monografia.Funciones;
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
            if (solution.evaluarSolucion(b) < solution.evaluarSolucion(Xi))
            {
                this.poblacion[indiceXi] = b;
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
                this.poblacion[indiceXi] = d;
            }
        }
        //FASE DE PARASITISMO
        private void faceParasitismo(int []Xi, int []Xj, int indiceXj) {
            int[] vectorParasito = new int[Xi.Length];
            for (int i = 0; i < Xi.Length; i++) {
                vectorParasito[i] = Xi[i];
            }
            int[] nDimenciones = dimesionesAleatoria();
            int pos;
            for (int k = 0; k < nDimenciones.Length; k++) {
                pos = nDimenciones[k];
                if (vectorParasito[pos] == 1) {
                    vectorParasito[pos] = 0;
                } else { 
                    vectorParasito[pos] = 1;
                }
            }
            vectorParasito = solution.repararSolucion(vectorParasito);
            if (solution.evaluarSolucion(vectorParasito) < solution.evaluarSolucion(Xj)) {
                this.poblacion[indiceXj] = vectorParasito;
            }
        }
        //FASE DE ARMONIA
        private void faceArmonia(int[] Xi, int indiceXi) {
            Random randon = new Random();
            double HMCR = 1-(10/dimensionOrganismo);
            double PAR = 0.365;
            int[] neko = new int[dimensionOrganismo];
            int posPeor;
            int posAleatoria;
            for (int k = 0; k < dimensionOrganismo; k++) {
                if (randon.NextDouble() <= HMCR){
                    posAleatoria = solution.posSolucionAleatoria(indiceXi, tamPoblacion, myRamdonL);
                    neko[k] = this.poblacion[posAleatoria][k];
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
            posPeor = solution.posPeorSolucion(this.poblacion);
            double evaluacionNeko = solution.evaluarSolucion(neko);
            if (evaluacionNeko < solution.evaluarSolucion(this.poblacion[posPeor])) {
                this.poblacion[posPeor] = neko;
            }
            if (evaluacionNeko < solution.evaluarSolucion(Xi)) {
                this.poblacion[indiceXi] = neko;
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
            int itermax = 100;
            int j;
            poblacion = new int[tamPoblacion][];
            int[] xi;
            int[] xj;
            int[] mejor;
            for (int i = 0; i < tamPoblacion; i++)
            {
                int[] organismo = solution.generarSolucionAleatorio(dimensionOrganismo, myRandom);
                poblacion[i] = solution.repararSolucion(organismo);
            }            
            mejor = solution.mejorSolucion(this.poblacion);
            /*
            Console.WriteLine("\nMejor Inicial");
            solution.imprimirSolucion(mejor);
            solution.Evaluate(mejor); */

            while (iter < itermax)
            {
                //Console.WriteLine("\n-------------ITERACION" + iter);
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
                    /*
                    mejor = solution.mejorSolucion(this.poblacion);
                    Console.WriteLine("Mejor Local");
                    solution.imprimirSolucion(mejor);
                    solution.Evaluate(mejor);
                    Console.WriteLine("\n");*/
                }
                iter++;
                /*
                mejor = solution.mejorSolucion(this.poblacion);
                Console.WriteLine("Mejor Global");
                solution.imprimirSolucion(mejor);
                solution.Evaluate(mejor);
                Console.WriteLine("\n");
                */
            }
        }
    }

}
