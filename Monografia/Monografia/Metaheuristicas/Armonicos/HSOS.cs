using Monografia.Funciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monografia.Metaheuristicas.Armonicos{
    class HSOS : Algorithm{
        public int tamPoblacion;
        public int[][] poblacion;
        double [] evalPoblacion;
        int dimensionOrganismo;
        Random myRamdonL;
                        /*FASE DE MOVIMIMIENTO DEL ALGORITMO*/

        //Fase de mutualimo para xi
        private void faseMutualismoXi(int[] Xi, int[] Xj, int[] mejor, int posXi){
            int[] mutualvector = new int[dimensionOrganismo];
            int[] a; int[] b;
            Random randon = new Random();
            for (int k = 0; k < dimensionOrganismo; k++){
                if (randon.NextDouble() > 0.5){
                    mutualvector[k] = Xi[k];
                }
                else{
                    mutualvector[k] = Xj[k];
                }
            }
            a = BestSolution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++){
                if (mutualvector[k] == mejor[k]){
                    a[k] = mejor[k];
                }
            }
            b = BestSolution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++){
                if (a[k] == Xi[k]){
                    b[k] = Xi[k];
                }
            }
            b = BestSolution.repararSolucion(b);
            double evaluacionB = BestSolution.evaluarSolucion(b);
            if (evaluacionB < evalPoblacion[posXi]){
                poblacion[posXi] = b;
                evalPoblacion[posXi] = evaluacionB;
            }
        }

        //Fase de Mutulismo para xj
        private void faseMutualismoXj(int[] Xj, int[] Xi, int[] mejor, int posXj){
            int[] mutualvector = new int[dimensionOrganismo];
            int[] a; int[] b;
            Random randon = new Random();
            for (int k = 0; k < dimensionOrganismo; k++){
                if (randon.NextDouble() > 0.5){
                    mutualvector[k] = Xj[k];
                }
                else{
                    mutualvector[k] = Xi[k];
                }
            }
            a = BestSolution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++){
                if (mutualvector[k] == mejor[k]){
                    a[k] = mejor[k];
                }
            }
            b = BestSolution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++){
                if (a[k] == Xj[k]){
                    b[k] = Xj[k];
                }
            }
            b = BestSolution.repararSolucion(b);
            double evaluacionB = BestSolution.evaluarSolucion(b);
            if (evaluacionB < evalPoblacion[posXj]){
                poblacion[posXj] = b;
                evalPoblacion[posXj] = evaluacionB;
            }
        }

        //Fase de comensalismo
        private void faceComensalismo(int[] Xi, int[] Xj, int[] Xmejor,int posXi) {
            int[] c; int[] d;
            c = BestSolution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++) {
                if (Xj[k] == Xmejor[k]) {
                    c[k] = Xmejor[k];
                }
            }
            d = BestSolution.generarSolucionAleatorio(dimensionOrganismo, myRamdonL);
            for (int k = 0; k < dimensionOrganismo; k++) {
                if (c[k] == Xi[k]) {
                    d[k] = Xi[k];
                }
            }
            d = BestSolution.repararSolucion(d);
            double evaluacionD = BestSolution.evaluarSolucion(d);
            if (evaluacionD < evalPoblacion[posXi]) {
                poblacion[posXi] = d;
                evalPoblacion[posXi] = evaluacionD;
            }
        }

        //Fase de parasitismo
        private void faceParasitismo(int []Xi, int []Xj, int PosXj) {
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
            vectorParasito = BestSolution.repararSolucion(vectorParasito);
            double evaluacionParasito = BestSolution.evaluarSolucion(vectorParasito);
            if (evaluacionParasito < evalPoblacion[PosXj]) {
                poblacion[PosXj] = vectorParasito;
                evalPoblacion[PosXj] = evaluacionParasito;
            }
        }

        //Fase de armonia
        private void faceArmonia(int [] Xi, int posXi) {
            Random randon = new Random();
            double HMCR = 1-(10/dimensionOrganismo);
            double PAR = 0.365;
            int[] neko = new int[dimensionOrganismo];
            int posPeor;
            int posAleatoria;
            for (int k = 0; k < dimensionOrganismo; k++) {
                if (randon.NextDouble() <= HMCR){
                    posAleatoria = BestSolution.posSolucionAleatoria(posXi, tamPoblacion, myRamdonL);
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
                    neko[k] = BestSolution.valorAleatorio(myRamdonL);
                }
            }
            neko = BestSolution.repararSolucion(neko);
            posPeor = BestSolution.posPeorSolucion(evalPoblacion);
            double evaluacionXi = evalPoblacion[posXi];
            double evaluacionPeor = evalPoblacion[posPeor];
            double evaluacionNeko = BestSolution.evaluarSolucion(neko);
            if (evaluacionNeko < evaluacionPeor) {
                poblacion[posPeor] = neko;
                evalPoblacion[posPeor] = evaluacionNeko;
            }
            if (evaluacionNeko < evaluacionXi) {
                poblacion[posXi] = neko;
                evalPoblacion[posXi] = evaluacionXi;
            }
        }

        //Obtiena una conjunto de posiones aleatorias de acuerdo a la solucion
        private int[] dimesionesAleatoria() { 

            Random randon = new Random();
            int maxvalue = dimensionOrganismo; int minvalue = 0;
            int numerodimencionesN = randon.Next(minvalue, maxvalue);
            int[] dimencionesN = new int[numerodimencionesN];
            int valor;
            for (int i = 0; i < numerodimencionesN; i++){
                valor = randon.Next(minvalue, maxvalue);
                dimencionesN[i] = valor;

            }
            return dimencionesN;
        }

                            /*ALGORITMO PRINCIPAL*/
        public override void Ejecutar(p_mediana theProblem, Random myRandom) {
            //Declaracion de variables 
            BestSolution = new Solution(theProblem, this);
            myRamdonL = myRandom;
            dimensionOrganismo = theProblem.numVertices;
            tamPoblacion = 30;
            int iter = 0;
            int itermax = 100;
            //Inicializar la poblacion
            poblacion = BestSolution.inicializarPoblacionReparada(tamPoblacion, myRandom);
            evalPoblacion = BestSolution.evaluacionPoblacion(poblacion);
            //Obtencion del mejor organismo
            int posMejor = BestSolution.posMejorSolucion(evalPoblacion);
            int[] mejor = poblacion[posMejor]; 
            //Iteracion del algoritmo
            while (iter < itermax){
                //Console.WriteLine("\n-------------ITERACION" + iter);
                int j; int[] xi; int[] xj; 
                //Recorrido de la poblacon
                for (int i = 0; i < tamPoblacion; i++){
                    //Organismo Actual
                    xi = poblacion[i];
                    //Mutualismo
                    j = BestSolution.posSolucionAleatoria(i, tamPoblacion, myRamdonL);
                    xj = poblacion[j];
                    faseMutualismoXi(xi, xj, mejor, i);
                    faseMutualismoXj(xj, xi, mejor, j);
                    //Comensalimo
                    j = BestSolution.posSolucionAleatoria(i, tamPoblacion, myRamdonL);
                    xj = poblacion[j];
                    faceComensalismo(xi, xj, mejor, i);
                    //Parasitimo
                    j = BestSolution.posSolucionAleatoria(i, tamPoblacion, myRamdonL);
                    xj = poblacion[j];
                    faceParasitismo(xi, xj, j);
                    //Armonia
                    faceArmonia(xi, i);

                    posMejor = BestSolution.posMejorSolucion(evalPoblacion);
                    mejor = poblacion[posMejor];
                }
                iter++;                            
                //Console.WriteLine("\n EvalMejor"+evalPoblacion[posMejor]);
            }
            //Evalucion de la mejor solucion
            BestSolution.Evaluate(mejor);
        }
    }

}
