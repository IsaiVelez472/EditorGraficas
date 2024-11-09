namespace EditorGraficasVectoriales
{
    internal class ListaDibujo
    {
        private NodoDibujo cabeza;

        public ListaDibujo()
        {
            cabeza = null;
        }

        public void AgregarDibujo(Dibujo dibujo)
        {
            NodoDibujo nuevoNodo = new NodoDibujo(dibujo);
            if (cabeza == null)
                cabeza = nuevoNodo;
            else
            {
                NodoDibujo actual = cabeza;
                while (actual.Siguiente != null)
                    actual = actual.Siguiente;
                actual.Siguiente = nuevoNodo;
            }
        }

        public NodoDibujo ObtenerCabeza()
        {
            return cabeza;
        }

        public void EliminarCabeza()
        {
            if (cabeza != null)
                cabeza = cabeza.Siguiente;
        }

        public List<Dibujo> ObtenerListaDibujos()
        {
            List<Dibujo> dibujos = new List<Dibujo>();
            NodoDibujo actual = cabeza;
            while (actual != null)
            {
                dibujos.Add(actual.Dibujo);
                actual = actual.Siguiente;
            }
            return dibujos;
        }
    }
}
