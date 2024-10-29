namespace EditorGraficasVectoriales
{
    public partial class FrmEditor : Form
    {
        private Point puntoInicial;   // Punto inicial de la línea
        private Point puntoFinal;     // Punto final de la línea
        private bool dibujando = false;  // Indica si se está dibujando

        public FrmEditor()
        {
            InitializeComponent();
            cmbTrazo.SelectedIndex = 0;
        }

        private void pnlDibujo_MouseDown(object sender, MouseEventArgs e)
        {
            // Al primer clic, establecemos el punto inicial y activamos el modo de dibujo
            if (e.Button == MouseButtons.Left)
            {
                puntoInicial = e.Location;
                dibujando = true;
            }
        }

        private void pnlDibujo_MouseMove(object sender, MouseEventArgs e)
        {
            // Mientras movemos el mouse, actualizamos el punto final y refrescamos el pnlDibujo
            if (dibujando)
            {
                puntoFinal = e.Location;
                pnlDibujo.Invalidate(); // Refresca el pnlDibujo para que se vuelva a dibujar
            }
        }

        private void pnlDibujo_MouseUp(object sender, MouseEventArgs e)
        {
            // Al soltar el clic, dejamos de dibujar y fijamos el punto final
            if (e.Button == MouseButtons.Left)
            {
                dibujando = false;
                puntoFinal = e.Location;
                pnlDibujo.Invalidate(); // Refresca el pnlDibujo para el trazo final
            }
        }

        private void pnlDibujo_Paint(object sender, PaintEventArgs e)
        {
            // Dibuja la línea mientras se arrastra el mouse
            if (dibujando)
            {
                using (Pen pen = new Pen(Color.White, 2))
                {
                    switch (cmbTrazo.SelectedIndex)
                    {
                        case 0:
                            e.Graphics.DrawLine(pen, puntoInicial, puntoFinal);
                            break;
                        case 1:
                            e.Graphics.DrawRectangle(pen, new RectangleF(puntoInicial,
                                new Size(puntoFinal.X - puntoInicial.X, puntoFinal.Y - puntoInicial.Y)));
                            break;
                        case 2:
                            e.Graphics.DrawEllipse(pen, new RectangleF(puntoInicial,
                                new Size(puntoFinal.X - puntoInicial.X, puntoFinal.Y - puntoInicial.Y)));
                            break;
                    }
                }
            }
        }
    }
}
