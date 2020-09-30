namespace Monografia.Funciones
{
    public class Arista
    {
        public int posicion;
        public int verticeInicial;
        public int verticeFinal;
        public int distanciaArista;

        

        public Arista(int p, int vi, int vf, int ad)
        {
            posicion = p;
            verticeInicial = vi;
            verticeFinal = vf;
            distanciaArista = ad;
        }

        public override string ToString()
        {
            return "P: "+ $"{posicion,3:##0}" + 
                   " V: " + $"{verticeInicial, 6:##0.0}" + 
                   " W: " + $"{verticeFinal,6:##0.0}" + 
                   " D: " + $"{distanciaArista,6:##0.0}";
        }
    }
}