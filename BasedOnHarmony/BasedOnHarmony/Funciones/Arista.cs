namespace BasedOnHarmony.Funciones
{
    public class Arista
    {
        public int Posicion;
        public int VerticeInicial;
        public int VerticeFinal;
        public int DistanciaArista;
        
        public Arista(int p, int vi, int vf, int ad)
        {
            Posicion = p;
            VerticeInicial = vi;
            VerticeFinal = vf;
            DistanciaArista = ad;
        }

        public override string ToString()
        {
            return "P: "+ $"{Posicion,3:##0}" + 
                   " V: " + $"{VerticeInicial, 6:##0.0}" + 
                   " W: " + $"{VerticeFinal,6:##0.0}" + 
                   " D: " + $"{DistanciaArista,6:##0.0}";
        }
    }
}