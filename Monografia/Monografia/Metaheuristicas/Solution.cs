using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Monografia.Funciones;

namespace Monografia.Metaheuristicas{
    public class Solution{
        public Algorithm MyAlgorithm;
        public p_mediana MyProblem;
        protected readonly int[] Objects; // {0, 1}
        protected double Weight;
        public double _fitness;
        public double Fitness => _fitness;
        public Solution(p_mediana theProblem, Algorithm theAlgorithm){
            MyProblem = theProblem;
            MyAlgorithm = theAlgorithm;       
            Objects = new int[MyProblem.totalAristas];
            Weight = 0; 
            _fitness = 0;
        }

        public int[] repararSolucion(int[] solucionX){
            int poskX;
            int pMedianas = MyProblem.p_medianas;
            List<int> pInstalaciones = posicionesPinstalaciones(solucionX);
            double[] menoresDistancias = determinarMenoresDistanciasX(solucionX, pInstalaciones);
            while (pInstalaciones.Count< pMedianas) {
                poskX = determinarArgMax(solucionX, menoresDistancias);
                solucionX[poskX] = 1;
                pInstalaciones.Add(poskX);
                menoresDistancias = determinarMenoresDistanciasX(solucionX, pInstalaciones);
            }
            while (pInstalaciones.Count > pMedianas) {
                
                poskX = determinarArgMin(pInstalaciones, menoresDistancias);
                solucionX[poskX] = 0;
                pInstalaciones.Remove(poskX);
                menoresDistancias = determinarMenoresDistanciasX(solucionX, pInstalaciones);
            }
            return solucionX;
        }

        private double[] determinarMenoresDistanciasX(int [] solucionX, List<int> pInstalaciones) {
            double[] menoresDistancias = new double[solucionX.Length];
            for (int i = 0; i < solucionX.Length; i++){
                menoresDistancias[i] = MyProblem.distanciaMenorPuntoDemanda(i, pInstalaciones);
            }
            return menoresDistancias;
        }
        private  int determinarArgMax (int [] solucionX, double[] menoresDistancias) {
            int poskX = 0;
            double argMin = 0;
            double[] sumasPerdidasJX = adicionPerdida(solucionX, menoresDistancias);
            for (int j = 0; j < sumasPerdidasJX.Length; j++) {
                double sumaperdidaJX = sumasPerdidasJX[j];
                if (sumaperdidaJX < argMin) {
                    argMin = sumaperdidaJX;
                    poskX =j;
                }
            }
            return poskX; 
        }

        private double[] adicionPerdida(int [] solucionX,double[] menoresDistancias) {
            double[] sumaperdidaJX = new double[solucionX.Length];
            for (int j = 0; j < solucionX.Length; j++){
                double sumaMin = 0;
                if (solucionX[j] == 0) {
                    for (int i = 0; i < menoresDistancias.Length; i++){
                        double distanciaIJ = MyProblem.distanciaFloydArista(i,j);
                        double dif =  distanciaIJ - menoresDistancias[i];
                        if (dif < 0){
                            sumaMin = sumaMin + dif;
                        }
                    }
                    sumaperdidaJX[j] = sumaMin;
                }
                sumaperdidaJX[j] = 0;
            }
        return sumaperdidaJX;
        }

        private int determinarArgMin(List<int> pInstalaciones, double[] menoresDistancias){
            int poskX = 0;
            double argMax = 1000000;
            List<double> sumasGanaciasJSX = eliminarGanacia(pInstalaciones, menoresDistancias);
            for (int j = 0; j < sumasGanaciasJSX.Count; j++){
                double sumagananciaJX = sumasGanaciasJSX[j];
                if (sumagananciaJX < argMax){
                    argMax = sumagananciaJX;
                    poskX = pInstalaciones[j];;
                }
            }
            return poskX;
        }
        private List<double> eliminarGanacia(List<int> pInstalaciones, double[] menoresDistancias){
            List<double> sumasGanaciaJX= new List<double>();        
            for (int j = 0; j < pInstalaciones.Count; j++) {
                double sumaMin = 0;
                List<int> copiapInstalaciones = new List<int>(pInstalaciones);
                copiapInstalaciones.RemoveAt(j);                
                for (int i = 0; i < menoresDistancias.Length; i++) {
                    double menordistanciaIT = MyProblem.distanciaMenorPuntoDemanda(i, copiapInstalaciones);
                    double dif = menordistanciaIT - menoresDistancias[i];
                    sumaMin = sumaMin + dif;
                }
                sumasGanaciaJX.Add(sumaMin);
            }
            return sumasGanaciaJX;
        }

        public double evaluarSolucion(int[] X){
            double evaluacion;
            List<int> pinstalaciones = posicionesPinstalaciones(X);
            if (pinstalaciones.Count == 0){
                evaluacion = 0;
            }else{
                evaluacion = MyProblem.Evaluate(pinstalaciones);
            }
            return evaluacion;   
        }

        private void imprimirPinstalaciones (List<int> pInstalaciones) {
            for (int i = 0; i < pInstalaciones.Count; i++) {
                Console.Write(pInstalaciones[i]+"-");
            }
        }

        public int[] mejorSolucion(int[][] poblacion){
            int[] mejor;
            double mejorEvaluacion = evaluarSolucion(poblacion[0]);
            mejor = poblacion[0];
            for (int i = 1; i < poblacion.Length; i++){
                double evaluacion = evaluarSolucion(poblacion[i]);
                if (evaluacion < mejorEvaluacion){
                    mejorEvaluacion = evaluacion;
                    mejor = poblacion[i];
                }
            }
            return mejor;
        }

        public int posPeorSolucion(int[][] poblacion){
            int posPeor = 0;
            double peorEvaluacion = evaluarSolucion(poblacion[0]);
            for (int i = 1; i < poblacion.Length; i++){
                double evaluacion = evaluarSolucion(poblacion[i]);
                if (evaluacion > peorEvaluacion){
                    peorEvaluacion = evaluacion;
                    posPeor = i;
                }
            }
            return posPeor;
        }
      
        public int posSolucionAleatoria(int pos, int tamPoblacion , Random myRandon){  // mover a solucion
            int maxvalue = tamPoblacion; int minvalue = 0;
            int aleatorio;
            do{
                aleatorio = myRandon.Next(minvalue, maxvalue);
            } while (aleatorio.Equals(pos));
            return aleatorio;
        }

        public void imprimirSolucion(int[] solucion){   
            for (int i = 0; i < solucion.Length; i++){
                Console.Write(" " + solucion[i]);
            }
        }

        public int[] generarSolucionAleatorio(int n, Random myRandom){     
            int[] solucion = new int[n];
            for (int i = 0; i < n; i++){
                solucion[i] =valorAleatorio(myRandom);
            }
            return solucion;
        }

        public int valorAleatorio(Random myRandom){  // mover a solucion
            int valor;
            double alea = myRandom.NextDouble();
            if (alea < 0.65){
                valor = 0;
            }
            else{
                valor = 1;
            }
            return valor;
        }

        public void imprimirpoblacion(int[][] poblacion, int  n){   //mover a solucion
            Console.WriteLine("Poblacion");
            for (int i = 0; i < poblacion.Length; i++){
                for (int j = 0; j < n; j++){
                    Console.Write(" ");
                    Console.Write(poblacion[i][j]);
                }
                Console.WriteLine(" ");
            }
        }
        private List<int> posicionesPinstalaciones(int [] X) {
            List<int> pinstalaciones = new List<int>();
            for (int j = 0; j < X.Length; j++){
                if (X[j] == 1){
                    pinstalaciones.Add(j);
                }
            }
            return pinstalaciones;
        }

        public void imprimirdistancias(){
            int [][] distancias = MyProblem.distanciasFloyd;
            Console.WriteLine("Distancias");
            for (int i = 0; i < distancias.Length; i++){
                for (int j = 0; j < distancias.Length; j++){
                    Console.Write(" ");
                    Console.Write(distancias[i][j]);
                }
                Console.WriteLine(" ");
            }
        }

        public void Evaluate(int [] solucion){
            _fitness = evaluarSolucion(solucion);
            //Console.WriteLine("\n Mejor Fitness:"+_fitness);
            MyAlgorithm.EFOs++;
        }

        public override string ToString(){
            var result = "";
            for (var i = 0; i < MyProblem.totalAristas; i++)
                result = result + (Objects[i] + " ");
            result = result + " f = " + _fitness +  " w = " + Weight;
            return result;
        }

        public bool IsOptimalKnown(){
            return Math.Abs(Fitness - MyProblem.OptimalLocation) < 1e-10;
        }
        
    }
       
}