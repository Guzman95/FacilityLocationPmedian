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

        //FASE DE MUTUALISMO
        private void faseMutualismo(int[] Xi, int[] Xj, int[] mejor, int indiceXi){
            int[] mutualvector = new int[dimensionOrganismo];
            int[] a = new int[dimensionOrganismo];
            int[] b = new int[dimensionOrganismo];
            Random randon = new Random();

            for (int k = 0; k < dimensionOrganismo; k++){
                if (randon.NextDouble() > 0.5){
                    mutualvector[k] = Xi[k];
                }
                else {
                    mutualvector[k] = Xj[k];
                }

            }
            a = solution.generarSolucionAleatorio(dimensionOrganismo);
            for (int k = 0; k < dimensionOrganismo; k++){
                if (mutualvector[k] == mejor[k]){
                    a[k] = mejor[k];
                }
            }
            b = solution.generarSolucionAleatorio(dimensionOrganismo);
            for (int k = 0; k < dimensionOrganismo; k++){
                if (a[k] == Xi[k]){
                    a[k] = Xi[k];
                }
            }
            a = solution.repararSolucion(a);
            if (solution.evaluarSolucion(a) <solution.evaluarSolucion(Xi)){
                poblacion[indiceXi] = a;
            }
        }

        //FASE DE COMENSALISMO
        private void faceComensalismo(int[] Xi, int[] Xj, int[] Xmejor,int indiceXi) {
            int[] c; int[] d;
            c = solution.generarSolucionAleatorio(dimensionOrganismo);
            for (int k = 0; k < dimensionOrganismo; k++) {
                if (Xj[k] == Xmejor[k]) {
                    c[k] = Xmejor[k];
                }
            }
            d = solution.generarSolucionAleatorio(dimensionOrganismo);
            for (int k = 0; k < dimensionOrganismo; k++) {
                if (c[k] == Xi[k]) {
                    d[k] = Xi[k];
                }
            }
            d = solution.repararSolucion(d);
            if (solution.evaluarSolucion(d) <solution.evaluarSolucion(Xi)) {
                poblacion[indiceXi] = d;
            }
        }
        //FASE DE PARASITISMO
        private void faceParasitismo(int []Xj, int []Xi, int indiceXj) {
            int[] vectorParasito= Xi;
            int[] nDimenciones = dimesionesAleatoria();
            int pos;
            for (int k = 0; k < nDimenciones.Length; k++) 
            {
                pos= nDimenciones[k];
                if (vectorParasito[pos] == 1) {
                    vectorParasito[pos] = 0;
                }
                else { 
                    vectorParasito[pos] = 1;
                }
            }
            vectorParasito = solution.repararSolucion(vectorParasito);
            if (solution.evaluarSolucion(vectorParasito) <solution.evaluarSolucion(Xj)) {
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
                    posAleatoria = solution.posSolucionAleatoria(indiceXi, tamPoblacion);
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
                    neko[k] = solution.valorAleatorio();
                }
            }
            neko = solution.repararSolucion(neko);
            posPeor = solution.posPeorSolucion(poblacion);
            if (solution.evaluarSolucion(neko) < solution.evaluarSolucion(poblacion[posPeor])) {
                poblacion[posPeor] = neko;
            }
            if (solution.evaluarSolucion(neko) <solution.evaluarSolucion(Xi)) {
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

     
        public override void Ejecutar(p_mediana theProblem, Random myRandom){

            solution = new Solution(theProblem, this);
            dimensionOrganismo = theProblem.numVertices;
            tamPoblacion = 5;
            int[] mejor = new int[dimensionOrganismo];
            int iter = 0;
            int itermax = 1;

            poblacion = solution.inicializarPoblacion (tamPoblacion);
            Console.WriteLine(" ");
            Console.WriteLine(" poblacion inicial");
            solution.imprimirpoblacion(poblacion, dimensionOrganismo);
            Console.WriteLine(" ");

            while (iter < itermax)
            {
                int[] xi = new int[dimensionOrganismo];
                int[] xj = new int[dimensionOrganismo];
                mejor = solution.mejorSolucion(poblacion);
                int j;
                for (int i = 0; i < tamPoblacion; i++)
                {
                    xi = poblacion[i];

                    j = solution.posSolucionAleatoria(i, tamPoblacion);
                    xj = poblacion[j];
                    faseMutualismo(xi, xj, mejor, i);
                    faseMutualismo(xj, xi, mejor, j);

                    j = solution.posSolucionAleatoria(i, tamPoblacion);
                    xj = poblacion[j];
                    faceComensalismo(xi, xj, mejor, i);

                    j = solution.posSolucionAleatoria(i, tamPoblacion);
                    xj = poblacion[j];
                    faceParasitismo(xi, xj, j);
                    faceArmonia(xi, i);
                    Console.WriteLine(" ");
                    Console.WriteLine("organismo: " + i);
                    solution.imprimirpoblacion(poblacion, dimensionOrganismo);
                    Console.WriteLine(" ");
                }
                iter = iter + 1;
            }
        }
    }

}
