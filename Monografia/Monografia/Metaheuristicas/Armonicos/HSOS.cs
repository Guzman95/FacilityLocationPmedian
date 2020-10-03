using Monografia.Funciones;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monografia.Metaheuristicas.Armonicos
{

    //ALGORITMO PRINCIPAL
    class HSOS : Algorithm
    {
        Random randon = new Random(255);
        private double HMCR = 0.657;
        private double PAR = 0.365;
        public int n = 10;
        private int tamPoblacion = 5;
        int[][] poblacion;
        int[][] distancias;
        public void HibridoOrganismosSimbioticos(p_mediana theProblem){
             distancias =  theProblem.floyd();

            int[] mejor = new int[n];
            int iter = 0;
            int itermax = 1;
            poblacion = new int[tamPoblacion][];
            for (int i = 0; i < tamPoblacion; i++){
                poblacion[i] = new int[n];
            }
            for (int i = 0; i < tamPoblacion; i++){
                for (int j = 0; j < n; j++){
                    if (randon.NextDouble() < 0.5){
                        poblacion[i][j] = 0;
                    }
                    else{
                        poblacion[i][j] = 1;
                    }
                }     
            }
            Console.WriteLine(" ");
            Console.WriteLine(" poblacion inicial");
            imprimirpoblacion();
            Console.WriteLine(" ");

            while (iter < itermax){
                int[] xi = new int[n];
                int[] xj = new int[n];
                mejor = mejorOrganismo(poblacion);
                int j;
                for (int i = 0; i < tamPoblacion; i++){  
                    xi = poblacion[i];
                    /*
                    j = posicionAleatoria(i);
                    xj = poblacion[j];
                    faseMutualismo(xi, xj, mejor, i);
                    faseMutualismo(xj, xi, mejor, j);
                    
                    j = posicionAleatoria(i);
                    xj = poblacion[j];
                    faceComensalismo(xi, xj, mejor, i);
                    
                    j = posicionAleatoria(i);
                    xj = poblacion[j];
                    faceParasitismo(xi, xj, j);
                    */
                    faceArmonia(xi, i);
                    Console.WriteLine(" ");
                    Console.WriteLine("organismo: " + i);
                    imprimirpoblacion();
                    Console.WriteLine(" ");



                }
                iter = iter + 1;
            }
        }

        //FUNCIONES

        //FASE DE MUTUALISMO
        private void faseMutualismo(int[] Xi, int[] Xj, int[] mejor, int indiceXi){
            int[] mutualvector = new int[n];
            int[] a = new int[n];
            int[] b = new int[n];
            Random randon = new Random();

            for (int k = 0; k < n; k++){
                if (randon.NextDouble() > 0.5){
                    mutualvector[k] = Xi[k];
                }
                else {
                    mutualvector[k] = Xj[k];
                }

            }
            a = organismoAleatorio();
            for (int k = 0; k < n; k++){
                if (mutualvector[k] == mejor[k]){
                    a[k] = mejor[k];
                }
            }
            b = organismoAleatorio();
            for (int k = 0; k < n; k++){
                if (a[k] == Xi[k]){
                    a[k] = Xi[k];
                }
            }
            a = reparar(a);
            if (evaluarSolucion(a) < evaluarSolucion(Xi)){
                poblacion[indiceXi] = a;
            }
        }

        //FASE DE COMENSALISMO
        private void faceComensalismo(int[] Xi, int[] Xj, int[] Xmejor,int indiceXi) {
            int[] c; int[] d;
            c = organismoAleatorio();
            for (int k = 0; k < n; k++) {
                if (Xj[k] == Xmejor[k]) {
                    c[k] = Xmejor[k];
                }
            }
            d = organismoAleatorio();
            for (int k = 0; k < n; k++) {
                if (c[k] == Xi[k]) {
                    d[k] = Xi[k];
                }
            }
            d = reparar(d);
            if (evaluarSolucion(d) < evaluarSolucion(Xi)) {
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
            vectorParasito = reparar(vectorParasito);
            if (evaluarSolucion(vectorParasito) < evaluarSolucion(Xj)) {
                poblacion[indiceXj] = vectorParasito;
            }
        }
        //FASE DE ARMONIA
        private void faceArmonia(int[] Xi, int indiceXi) {
            Random randon = new Random();
            int[] neko = new int[n];
            int posPeor;

            for (int k = 0; k < n; k++) {
                if (randon.NextDouble() <= HMCR){
                    neko[k] = poblacion[posicionAleatoria(indiceXi)][k];
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
                    neko[k] = valorAleatorio();
                }
            }
            neko = reparar(neko);
            posPeor = posPeorOrganismo(poblacion);
            if (evaluarSolucion(neko) < evaluarSolucion(poblacion[posPeor])) {
                poblacion[posPeor] = neko;
            }
            if (evaluarSolucion(neko) < evaluarSolucion(Xi)) {
                poblacion[indiceXi] = neko;
            }
        }

        //METODOS PRINCIPALES
        public int[] mejorOrganismo(int[][] poblacion)
        {
            int[] mejor = new int[n];
            int[] organismo = new int[n];
            double mejorEvaluacion = evaluarSolucion(poblacion[0]);
            for (int i = 1; i < poblacion.Length; i++)
            {
                organismo = poblacion[i];
                double evaluacion = evaluarSolucion(organismo);
                if (evaluacion < mejorEvaluacion)
                {
                    mejor = organismo;
                }
            }
            return mejor;
        }
        private int[] reparar(int[] X){
            int[] Xnew = new int[n];
            Xnew = X;
            return Xnew;
        }
        public double evaluarSolucion(int[] X)
        {
            double[] pesos = new double[] { 2.4, 4.6, 5.3, 5, 1.5, 2.4, 4, 3.2, 1, 0.5 };
            double evaluacion = 0;
            for (int i = 0; i < n; i++)
            {
                evaluacion = evaluacion + X[i] * pesos[i];
            }
            return evaluacion;
        }


        //METODOS SECUNDARIOS
        public int[] organismoAleatorio(){
            
            int[] organismo = new int[n];
            for (int i = 0; i < n; i++){
                organismo[i] = valorAleatorio();
            }
            return organismo;
        }

        private int valorAleatorio() {
            int valor;
            Random randon = new Random();
            double alea = randon.NextDouble();
            if ( alea < 0.5)
            {
                valor = 0;
            }
            else
            {
                valor = 1;
            }
            return valor;

        }
        public int posPeorOrganismo(int[][] poblacion)
        {
            int[] organismo;
            int posPeor =0;
            double peorEvaluacion = evaluarSolucion(poblacion[0]);
            for (int i = 1; i < poblacion.Length; i++)
            {
                organismo = poblacion[i];
                double evaluacion = evaluarSolucion(organismo);
                if (evaluacion > peorEvaluacion)
                {
                    posPeor = i;
                }
            }
            return posPeor;
        }

        private int posicionAleatoria(int pos){
            Random randon = new Random();
            int maxvalue = tamPoblacion; int minvalue = 0;
            int aleatorio ;
            do{
                aleatorio = randon.Next(minvalue, maxvalue);
            } while (aleatorio.Equals(pos));
            return aleatorio;
        }
        private int[] dimesionesAleatoria() {

            Random randon = new Random();
            int maxvalue = n; int minvalue = 1;
            int numerodimencionesN = randon.Next(minvalue, maxvalue);
            Console.WriteLine("numeroDim: " + numerodimencionesN);
            int[] dimencionesN = new int[numerodimencionesN];
            int valor;

            for (int i = 0; i < numerodimencionesN; i++)
            {
                valor = randon.Next(minvalue, maxvalue);
                Console.WriteLine(i + "pos: " + valor);
                dimencionesN[i] = valor - 1;

            }
            return dimencionesN;
        }

        private void imprimirpoblacion() {
            Console.WriteLine("Poblacion");
            for (int i = 0; i < tamPoblacion; i++) {
                for (int j = 0; j < n; j++){
                    Console.Write(" ");
                    Console.Write(poblacion[i][j]);
                }
                Console.WriteLine(" ");

            }
        }

        private void imprimirdistancias(){
            Console.WriteLine("Distancias");
            for (int i = 0; i < distancias.Length; i++){
                for (int j = 0; j < distancias.Length; j++){
                    Console.Write(" ");
                    Console.Write(distancias[i][j]);
                }
                Console.WriteLine(" ");
            }
        }

        private void imprimirOrganismo(int [] organismo) {
            for (int i = 0; i < n; i++) {
                Console.Write(" " + organismo[i]);
            }
        }

        public override void Ejecutar(p_mediana theProblem, Random myRandom){
            HibridoOrganismosSimbioticos(theProblem);   
        }
    }

}
