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
                      /*INICIALIZACION ALEATORIA REPARADA*/
        public int[][] inicializarPoblacionReparada(int tamañoPoblacion, Random myRandom )
        {
            int [][] poblacion = new int[tamañoPoblacion][];
            //inicializacion de la poblacion
            for (int i = 0; i < tamañoPoblacion; i++)
            {
                int[] solucion = generarSolucionAleatorio(MyProblem.numVertices, myRandom);
                poblacion[i] = repararSolucion(solucion);
            }
            return poblacion;
         }
        //Genera e inicializa una solucion de forma aleatoria
        public int[] generarSolucionAleatorio(int n, Random myRandom)
        {
            int[] solucion = new int[n];
            for (int i = 0; i < n; i++)
            {
                solucion[i] = valorAleatorio(myRandom);
            }
            return solucion;
        }

                           /*INICIALIZACION ALEATORIA CONTROLADA*/
        //Inicializa la poblacion con organismos generados  con un numero p de instacionciones 
        public int[][] inicilaizarPoblacionControlada(int tamañoPoblacion, Random myRandom){
            int[][] poblacion = new int[tamañoPoblacion][];
            //inicializacion de la poblacion
            for (int i = 0; i < tamañoPoblacion; i++){
                int[] solucion = inicializacionAleatoriaOrganismo();
                poblacion[i] = solucion;
            }
            return poblacion;
        }
        //Inicializa un organismo con el numero de istalaciones selecionadas 
        private int [] inicializacionAleatoriaOrganismo() {
            int [] organismo = new int[MyProblem.numVertices];
            int [] posiciones = posicionesAleatorias();
            for (int d = 0; d < posiciones.Length; d++) {
                organismo[posiciones[d]] = 1;
            }
            return organismo; 
        }
        //Seleciona aleatoriamete un numero p de posiciones en funcion al tamaño de la solucion
        private int[] posicionesAleatorias(){     
            Random randon = new Random();
            int maxvalue = MyProblem.numVertices; int minvalue = 0;
            int[] posicionesP = new int[MyProblem.p_medianas]; //inicializar posiciones en 1
            int posAleatoria;

            for (int i = 0; i < posicionesP.Length; i++){
                do {
                    posAleatoria = randon.Next(minvalue, maxvalue);
                } while(posExistePosicion(posicionesP,posAleatoria));
                posicionesP[i] = posAleatoria;
            }
            return posicionesP;
        }
        //Determina si una posicion ya ha sido selecionada  
        private bool posExistePosicion(int[] posiciones, int pos){
            for (int p = 0; p < posiciones.Length; p++){
                if (pos == posiciones[p]){
                    return true;
                }
            }
            return false;
        }
                /*EVALUACION DE LA POBLACION*/
        public double[] evaluacionPoblacion(int [][] poblacion) {
            double[] evaluaciones = new double[poblacion.Length];
            for (int i = 0; i < poblacion.Length; i++) {
                evaluaciones[i] = evaluarSolucion(poblacion[i]);
            }
            return evaluaciones;
        
        }
                         /*REPARACION DE SOLUCION DE FORMA ALEATORIA */

        //Agrega o elimina instalaciones aleatoriamente hasta que instalaciones igual a P 
        public int[] repararSolucionAleatoria(int[] solucionX)
        {
            int poskX;
            int pMedianas = MyProblem.p_medianas;
            List<int> pInstalaciones = posicionesPinstalaciones(solucionX);
            while (pInstalaciones.Count < pMedianas){
                poskX = AgregarinstalacionAleatoria(pInstalaciones);
                solucionX[poskX] = 1;
                pInstalaciones.Add(poskX);
            }
            while (pInstalaciones.Count > pMedianas){
                poskX = EliminarinstalacionAleatoria(pInstalaciones);
                solucionX[poskX] = 0;
                pInstalaciones.Remove(poskX);
            }
            return solucionX;
        }
        //Seleciona aleatoriamente  una posicion de instalacion para ser agregada a la solucion
        private int AgregarinstalacionAleatoria(List<int> pInstalaciones) { 
            int posAleatoria;
            Random randon = new Random();
            int maxvalue = MyProblem.numVertices; int minvalue = 0;
            do{
                posAleatoria = randon.Next(minvalue, maxvalue);
            } while (posExisteInstalacion(pInstalaciones, posAleatoria));
            return posAleatoria;
          
        }
        //Seleciona aleatoriamente una instalacion de la solucion para ser eliminada
        private int EliminarinstalacionAleatoria(List<int> pInstalaciones)
        {
            int posAleatoria = -1;
            Random randon = new Random();
            int maxvalue = pInstalaciones.Count; int minvalue = 0;
            posAleatoria = randon.Next(minvalue, maxvalue);
            return pInstalaciones[posAleatoria];
        }
        //Determina si una posicion existe en el conjunto de instalaciones de una solución
        private bool posExisteInstalacion(List<int> instalaciones, int pos){
            for (int i = 0; i < instalaciones.Count; i++){
                if (pos == instalaciones[i]){
                    return true;
                }
            }
            return false;
        }
                        /*REPARACION CON CONOCIMIENTO*/

         //Agrega o elimina instalciones aplicando conocimiento hasta instalaciones igual a P               
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
        //Determina las menores distancias de cada demanda a su instalacion masa cercana
        private double[] determinarMenoresDistanciasX(int [] solucionX, List<int> pInstalaciones) {
            double[] menoresDistancias = new double[solucionX.Length];
            for (int i = 0; i < solucionX.Length; i++){
                menoresDistancias[i] = MyProblem.distanciaMenorPuntoDemanda(i, pInstalaciones);
            }
            return menoresDistancias;
        }
        //Determina la instalacion que al eliminarna aumente en menor valor la funcion objetivo
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
        //Calacula para cada instalacion  su adicion de valor a la funcion objetivo al ser elminada
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
        //Determina la instalacion que al agregarla disminuya al maximo el valor la funcion objetivo
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
        //Calacula para cada instalacion su eliminacion de valor a la funcion objetivo al ser agregada
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

                                /*EVALUACION*/
        //Realaiza la evaluacion de la funcion objetivo para una solucion X
        public double evaluarSolucion(int[] X){
            double evaluacion;
            List<int> pinstalaciones = posicionesPinstalaciones(X);
            evaluacion = MyProblem.Evaluate(pinstalaciones);
            return evaluacion;   
        }

        //Obtiene las pociones de las instalaciones en una solucion X
        private List<int> posicionesPinstalaciones(int[] X){
            List<int> pinstalaciones = new List<int>();
            for (int j = 0; j < X.Length; j++){
                if (X[j] == 1){
                    pinstalaciones.Add(j);
                }
            }
            return pinstalaciones;
        }
                      /*FUNCIONALIDADES NESESARIAS*/
        //Obtiene de la poblacion la solucion con mejor valor de funcion objetivo
        public int posMejorSolucion(double [] evaluaciones){
            double mejorEvaluacion = evaluaciones[0];
            int posMejor = 0;
            for (int i = 1; i < evaluaciones.Length; i++){
                double evaluacion = evaluaciones[i];
                if (evaluacion < mejorEvaluacion){
                    mejorEvaluacion = evaluacion;
                    posMejor = i;
                }
            }
            return posMejor;
        }
        //Obtine de la poblacion la poscision de la solucion con peor valor de funcion objetivo 
        public int posPeorSolucion(double [] evaluaciones ){
            int posPeor = 0;
            double peorEvaluacion = evaluaciones[0];
            for (int i = 1; i < evaluaciones.Length; i++){
                double evaluacion = evaluaciones[i];
                if (evaluacion > peorEvaluacion){
                    peorEvaluacion = evaluacion;
                    posPeor = i;
                }
            }
            return posPeor;
        }
        //Seleciona alatoriamente una posicion de la poblacion 
        public int posSolucionAleatoria(int pos, int tamPoblacion , Random myRandon){  // mover a solucion
            int maxvalue = tamPoblacion; int minvalue = 0;
            int aleatorio;
            do{
                aleatorio = myRandon.Next(minvalue, maxvalue);
            } while (aleatorio.Equals(pos));
            return aleatorio;
        }

        //Retorna un valor aleatorio de acuerdo a la probabilidad definida
        public int valorAleatorio(Random myRandom){  // mover a solucion
            int valor;
            double alea = myRandom.NextDouble();
            if (alea < 0.5){
                valor = 0;
            }
            else{
                valor = 1;
            }
            return valor;
        }

                /*PROCEDIMIENTOS PARA IMPRIMIR POR CONSOLA*/

        //Imprime los valores de una solucion dada
        public void imprimirSolucion(int[] solucion){
            for (int i = 0; i < solucion.Length; i++)
            {
                Console.Write(" " + solucion[i]);
            }
        }
        //imprime el conjunto de intalaciones de una solucion X
        private void imprimirPinstalaciones(List<int> pInstalaciones){
            for (int i = 0; i < pInstalaciones.Count; i++)
            {
                Console.Write(pInstalaciones[i] + "-");
            }
        }
        //Imprime toda la poblacion hasta el momento
        public void imprimirpoblacion(int[][] poblacion, int n)
        {   //mover a solucion
            for (int i = 0; i < poblacion.Length; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write("-");
                    Console.Write(poblacion[i][j]);
                }
                this.Evaluate(poblacion[i]);
                Console.WriteLine("  ");
            }
        }
        //Imprime la matriz de distancias de floyd
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
        //Retorna el valor de elvaluacion de la mejor solucion
        public void Evaluate(int [] solucion){
            _fitness = evaluarSolucion(solucion);
            //Console.Write("\nMejorFinesss" + _fitness);
            MyAlgorithm.EFOs++;
        }
        //Obtiene informacion del problema y su solucion
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