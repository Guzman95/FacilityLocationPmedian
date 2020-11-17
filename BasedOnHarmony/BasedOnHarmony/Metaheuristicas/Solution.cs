using System.Collections.Generic;

namespace BasedOnHarmony.Metaheuristicas{

    public partial class Solution{
        public Algorithm MyAlgorithm;

        public List<int> PosInstalaciones;
        public int[] Vertices; // {0, 1}
        public double Fitness { get; set; }

        public Solution(Algorithm theAlgorithm){
            MyAlgorithm = theAlgorithm;

            PosInstalaciones= new List<int>();
            Vertices = new int[MyAlgorithm.MyProblem.NumVertices];
            Fitness = 0;
        }

        public Solution(Solution origin)
        {
            MyAlgorithm = origin.MyAlgorithm;
            PosInstalaciones = new List<int>();
            PosInstalaciones.AddRange(origin.PosInstalaciones);
            Vertices = new int[MyAlgorithm.MyProblem.NumVertices];
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
                Vertices[i] = origin.Vertices[i];
            Fitness = origin.Fitness;
        }

        public void RecalculatePosInstalaciones()
        {
            PosInstalaciones = new List<int>();
            for (var i=0; i < MyAlgorithm.MyProblem.NumVertices; i++)
                if (Vertices[i]==1)
                    PosInstalaciones.Add(i);
        }

        public void Activar(int pos)
        {
            if (Vertices[pos] == 1) return;
            Vertices[pos] = 1;
            PosInstalaciones.Add(pos);
            PosInstalaciones.Sort();
        }

        public void InActivar(int pos)
        {
            if (Vertices[pos] == 0) return;
            Vertices[pos] = 0;
            PosInstalaciones.Remove(pos);
        }

        //Genera e inicializa una solucion de forma aleatoria
        public void RandomInitializationWithoutConstrains()
        {
            for (var i = 0; i < MyAlgorithm.MyProblem.NumVertices; i++)
                if (MyAlgorithm.MyRandom.Next(2) == 0)
                    InActivar(i);
                else
                    Activar(i);
        }

        //Agrega o elimina instalaciones aleatoriamente hasta que instalaciones igual a P 
        public void RepairSolutionRandomly()
        {
            int poskX;
            var pMedianas = MyAlgorithm.MyProblem.PMedianas;

            while (PosInstalaciones.Count < pMedianas)
            {
                poskX = VerticeValidation(1);
                Activar(poskX);
            }
            while (PosInstalaciones.Count > pMedianas)
            {
                poskX = VerticeValidation(0);
                InActivar(poskX);
            }
        }

        /// <summary>
        /// Seleciona aleatoriamente  una posicion de instalacion para ser agregada a la solucion
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private int VerticeValidation(int state)
        {
            int posAleatoria;
            do
            {
                posAleatoria = MyAlgorithm.MyRandom.Next(MyAlgorithm.MyProblem.NumVertices);
            } while (Vertices[posAleatoria] == state);
            return posAleatoria;
        }

        //Inicializa un organismo con el numero de instalaciones selecionadas 
        public void RandomOrganismInitialization()
        {
            var posiciones = Utils.RandomlySelectedExactDimensions(MyAlgorithm.MyRandom,
                MyAlgorithm.MyProblem.NumVertices, MyAlgorithm.MyProblem.PMedianas);
            foreach (var pos in posiciones)
                Activar(pos);
        }

        //Realiza la evaluacion de la funcion objetivo para una solucion X
        public void Evaluate()
        {
            Fitness = MyAlgorithm.MyProblem.Evaluate(PosInstalaciones);
            MyAlgorithm.EFOs++;
        }

        public override string ToString()
        {
            var result = "";
            for (var i = 0; i < PosInstalaciones.Count; i++)
                result = result + (PosInstalaciones[i] + " ");
            result = result + " f = " + Fitness;
            return result;
        }

        /*
        //Seleciona aleatoriamete un numero p de posiciones en funcion al tamaño de la solucion
        private int[] posicionesAleatorias(){     
            int maxvalue = MyProblem.NumVertices; int minvalue = 0;
            int[] posicionesP = new int[MyProblem.PMedianas]; //inicializar posiciones en 1
            int posAleatoria;

            for (int i = 0; i < posicionesP.Length; i++){
                do {
                    
                    posAleatoria = MyAlgorithm.MyRandom.Next(minvalue, maxvalue);
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
*/
                        /*REPARACION CON CONOCIMIENTO*/
                        /*
         //Agrega o elimina instalciones aplicando conocimiento hasta instalaciones igual a P               
        public int[] repararSolucionConocimiento(int[] solucionX){
            int poskX;
            int pMedianas = MyProblem.PMedianas;
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
                menoresDistancias[i] = MyProblem.DistanciaMenorPuntoDemanda(i, pInstalaciones);
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
                        double distanciaIJ = MyProblem.DistanciaFloydArista(i,j);
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
                    double menordistanciaIT = MyProblem.DistanciaMenorPuntoDemanda(i, copiapInstalaciones);
                    double dif = menordistanciaIT - menoresDistancias[i];
                    sumaMin = sumaMin + dif;
                }
                sumasGanaciaJX.Add(sumaMin);
            }
            return sumasGanaciaJX;
        }
        */
                      /*FUNCIONALIDADES NESESARIAS*/
        
        /*
        //Obtiene de la Population la solucion con mejor valor de funcion objetivo
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
        //Obtine de la Population la poscision de la solucion con peor valor de funcion objetivo 
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
        //Seleciona alatoriamente una posicion de la Population 
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
        */

                /*PROCEDIMIENTOS PARA IMPRIMIR POR CONSOLA*/
        /*
        //imprime el conjunto de intalaciones de una solucion X
        private void imprimirPinstalaciones(List<int> pInstalaciones){
            for (int i = 0; i < pInstalaciones.Count; i++)
            {
                Console.Write(pInstalaciones[i] + "-");
            }
        }
        //Imprime toda la Population hasta el momento
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
            int [][] distancias = MyProblem.DistanciasFloyd;
            Console.WriteLine("Distancias");
            for (int i = 0; i < distancias.Length; i++){
                for (int j = 0; j < distancias.Length; j++){
                    Console.Write(" ");
                    Console.Write(distancias[i][j]);
                }
                Console.WriteLine(" ");
            }
        } 
        */
        /*EVALUACION*/
        /*
        public bool IsOptimalKnown(){
            return Math.Abs(Fitness - MyProblem.OptimalLocation) < 1e-10;
        }
        */
    }
}