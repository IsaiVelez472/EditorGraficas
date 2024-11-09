namespace EditorGraficasVectoriales
{
    internal class Dibujo
    {
        public Point PuntoInicial { get; set; }
        public Point PuntoFinal { get; set; }
        public int Tipo { get; set; }

        public Dibujo() { } // Constructor sin parámetros requerido para deserialización

        public Dibujo(Point puntoInicial, Point puntoFinal, int tipo)
        {
            PuntoInicial = puntoInicial;
            PuntoFinal = puntoFinal;
            Tipo = tipo;
        }
    }
}
