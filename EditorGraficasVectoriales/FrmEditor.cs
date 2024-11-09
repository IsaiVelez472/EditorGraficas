using System.Text.Json;
using System.IO;

namespace EditorGraficasVectoriales
{
    public partial class FrmEditor : Form
    {
        private Point puntoInicial;   // Punto inicial de la línea
        private Point puntoFinal;     // Punto final de la línea
        private bool dibujando = false;  // Indica si se está dibujando
        private bool borrando = false; // Indica si estamos en modo borrador
        private const int ToleranciaBorrado = 10; // Distancia de tolerancia para borrar
        private ListaDibujo listaDibujos;


        public FrmEditor()
        {
            InitializeComponent();
            cmbTrazo.SelectedIndex = 0;
            listaDibujos = new ListaDibujo();
        }

        private void pnlDibujo_MouseDown(object sender, MouseEventArgs e)
        {
            // Al primer clic, establecemos el punto inicial y activamos el modo de dibujo
            if (e.Button == MouseButtons.Left)
            {
                if (borrando)
                {
                    // Si estamos en modo borrador, intentamos borrar un dibujo
                    EliminarDibujo(e.Location);
                    pnlDibujo.Invalidate();
                }
                else
                {
                    // Modo de dibujo normal
                    puntoInicial = e.Location;
                    dibujando = true;
                    borrando = false;

                }
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
            if (e.Button == MouseButtons.Left && dibujando)
            {
                dibujando = false;
                puntoFinal = e.Location;

                // Guarda el dibujo en la lista
                Dibujo nuevoDibujo = new Dibujo(puntoInicial, puntoFinal, cmbTrazo.SelectedIndex);
                listaDibujos.AgregarDibujo(nuevoDibujo);

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

            // Dibuja todos los dibujos guardados
            NodoDibujo actual = listaDibujos.ObtenerCabeza();
            while (actual != null)
            {
                Dibujo dibujo = actual.Dibujo;
                using (Pen pen = new Pen(Color.White, 2))
                {
                    switch (dibujo.Tipo)
                    {
                        case 0:
                            e.Graphics.DrawLine(pen, dibujo.PuntoInicial, dibujo.PuntoFinal);
                            break;
                        case 1:
                            e.Graphics.DrawRectangle(pen, new RectangleF(dibujo.PuntoInicial,
                                new Size(dibujo.PuntoFinal.X - dibujo.PuntoInicial.X, dibujo.PuntoFinal.Y - dibujo.PuntoInicial.Y)));
                            break;
                        case 2:
                            e.Graphics.DrawEllipse(pen, new RectangleF(dibujo.PuntoInicial,
                                new Size(dibujo.PuntoFinal.X - dibujo.PuntoInicial.X, dibujo.PuntoFinal.Y - dibujo.PuntoInicial.Y)));
                            break;
                    }
                }
                actual = actual.Siguiente;
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            borrando = false;
            listaDibujos = new ListaDibujo();
            pnlDibujo.Invalidate();
        }

        // Método para eliminar un dibujo cercano al punto clicado
        private void EliminarDibujo(Point punto)
        {
            NodoDibujo actual = listaDibujos.ObtenerCabeza();
            NodoDibujo previo = null;

            while (actual != null)
            {
                Dibujo dibujo = actual.Dibujo;
                Rectangle areaDibujo = new Rectangle(
                    Math.Min(dibujo.PuntoInicial.X, dibujo.PuntoFinal.X) - ToleranciaBorrado,
                    Math.Min(dibujo.PuntoInicial.Y, dibujo.PuntoFinal.Y) - ToleranciaBorrado,
                    Math.Abs(dibujo.PuntoFinal.X - dibujo.PuntoInicial.X) + 2 * ToleranciaBorrado,
                    Math.Abs(dibujo.PuntoFinal.Y - dibujo.PuntoInicial.Y) + 2 * ToleranciaBorrado);

                if (areaDibujo.Contains(punto))
                {
                    // Elimina el dibujo actual de la lista
                    if (previo == null)
                        listaDibujos.EliminarCabeza(); // Si es el primer nodo
                    else
                        previo.Siguiente = actual.Siguiente;

                    break; // Sale una vez se borra el dibujo
                }
                previo = actual;
                actual = actual.Siguiente;
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            borrando = true;
        }

        private void btnDibujar_Click(object sender, EventArgs e)
        {
            borrando = false;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Archivos JSON (*.json)|*.json";
                saveFileDialog.Title = "Guardar Dibujo";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    GuardarArchivo(saveFileDialog.FileName);
                }
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos JSON (*.json)|*.json";
                openFileDialog.Title = "Abrir Dibujo";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    AbrirArchivo(openFileDialog.FileName);
                    pnlDibujo.Invalidate(); // Refresca el panel para mostrar el dibujo cargado
                }
            }
        }

        private void GuardarArchivo(string rutaArchivo)
        {
            // Convierte la lista de dibujos en una lista serializable
            List<Dibujo> dibujos = listaDibujos.ObtenerListaDibujos();
            string json = JsonSerializer.Serialize(dibujos, new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(rutaArchivo, json);
            MessageBox.Show("Dibujo guardado exitosamente en " + rutaArchivo);
        }

        private void AbrirArchivo(string rutaArchivo)
        {
            string json = File.ReadAllText(rutaArchivo);
            List<Dibujo> dibujos = JsonSerializer.Deserialize<List<Dibujo>>(json);

            listaDibujos = new ListaDibujo();
            foreach (var dibujo in dibujos)
            {
                listaDibujos.AgregarDibujo(dibujo);
            }

            MessageBox.Show("Dibujo cargado exitosamente desde " + rutaArchivo);
        }
    }
}
