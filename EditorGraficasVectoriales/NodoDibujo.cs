namespace EditorGraficasVectoriales
{
    internal class NodoDibujo
    {
        public Dibujo Dibujo { get; set; }
        public NodoDibujo Siguiente { get; set; }

        public NodoDibujo(Dibujo dibujo)
        {
            Dibujo = dibujo;
            Siguiente = null;
        }
    }
}
