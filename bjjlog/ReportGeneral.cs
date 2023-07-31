using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace bjjlog
{
    public partial class ReportGeneral : Form
    {
        public ReportGeneral()
        {
            InitializeComponent();
        }
        public string connectionString { get; set; }
        public int Idregistro { get; set; }
        string rutaimagen = "";
        List<string> nombresMeses = new List<string>();
        List<int> diasAsis = new List<int>();
        private void ReportGeneral_Load(object sender, EventArgs e)
        {
            cargarDatos();
            cargarChart();
            cargarImagen(rutaimagen);
        }
        public void cargarDatos()
        {
            string query = "SELECT r.*, m.fechainicio, m.fechafin, p.nombre, totalasis, p.clase,  m.cantidad FROM registros r JOIN movimientos m ON r.Id = m.idregistro JOIN Planes p ON p.id = m.idplan JOIN asistencia a ON r.Id = a.idregistro JOIN (SELECT idregistro, COUNT(id) as totalasis FROM asistencia GROUP BY idregistro) as ac ON r.Id = ac.idregistro WHERE r.Id = " + Idregistro + " AND m.Id = (SELECT MAX(Id) FROM movimientos WHERE idregistro = r.Id) AND a.Id = (SELECT MAX(Id) FROM asistencia WHERE idregistro = r.Id);";
            string query2 = "select DISTINCT DATENAME(MONTH, fecha) AS NombreMes, COUNT(id) as cantidad from asistencia where idregistro = " + Idregistro + " GROUP BY  DATENAME(MONTH, fecha) ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        label1.Text = "Reporte: " + reader[1].ToString() + " " + reader[2].ToString();
                        rutaimagen = reader[16].ToString();
                        lblcelular.Text = "Celular: " + reader[10].ToString();
                        lblcorreo.Text = "Correo: " + reader[11].ToString();
                        lbldireccion.Text = "Direccion: " + reader[4].ToString();
                        lblbarrio.Text = "Barrio: " + reader[5].ToString();
                        lblmunicipio.Text = "Municipio: " + reader[6].ToString();
                        lbltplan.Text = "Tipo de plan: " + reader[19].ToString();
                        lblclases.Text = "Clases restantes: " + reader[22].ToString();
                        if (reader[21].ToString() == "0")
                        {
                            lblclases.Hide();
                        }
                        
                        DateTime inicio = Convert.ToDateTime(reader[17].ToString());
                        DateTime fin = Convert.ToDateTime(reader[18].ToString());
                        DateTime registro = Convert.ToDateTime(reader[15].ToString());
                        lblfecharegistro.Text = "Fecha de registro: " + registro.ToString("yyyy-MM-dd");
                        lblasistencia.Text = "Total asistencia: " + reader[20].ToString();
                        lblfechaini.Text = "Fecha inicio: " + inicio.ToString("yyyy-MM-dd");
                        lblfechafin.Text = "Fecha fin: " + fin.ToString("yyyy-MM-dd");

                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                connection.Close();
                SqlCommand command2 = new SqlCommand(query2, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader2 = command2.ExecuteReader();

                    while (reader2.Read())
                    {
                        nombresMeses.Add(reader2["NombreMes"].ToString());
                        diasAsis.Add(Convert.ToInt32(reader2["cantidad"]));
                    }

                    reader2.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                connection.Close();
            }
        }
        public void cargarChart()
        {
            chart1.Series.Clear(); // Limpiar series anteriores
            chart1.ChartAreas.Clear(); // Limpiar áreas de gráfico anteriores

            ChartArea chartArea = new ChartArea();
            chart1.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.ChartType = SeriesChartType.Column; // Puedes usar el tipo de gráfico que desees
            chart1.Series.Add(series);
            // Agregar los datos a la serie del gráfico
            for (int i = 0; i < nombresMeses.Count; i++)
            {
                series.Points.AddXY(nombresMeses[i] + " Clases: " + diasAsis[i], diasAsis[i]);
            }


            // Personalizar el gráfico si lo deseas (título, leyendas, etc.)
            chart1.Titles.Add(new Title("Gráfico de asistencia por mes", Docking.Top, new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.Black));
            chart1.ChartAreas[0].AxisX.Title = "Meses";
            chart1.ChartAreas[0].AxisY.Title = "Asistencias";
            chart1.Legends.Add(new Legend("Leyenda"));
            series.IsVisibleInLegend = false;
            chart1.Legends[0].Docking = Docking.Bottom;

            // Actualizar el gráfico
            chart1.Update();
        }
        public void cargarImagen(string ruta)
        {
            try
            {
                // Descargar la imagen desde la URL
                using (WebClient webClient = new WebClient())
                {
                    byte[] imageData = webClient.DownloadData(ruta);

                    // Convertir los datos de la imagen a un objeto Image
                    using (var stream = new System.IO.MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(stream);
                        // Mostrar la imagen en el PictureBox
                        int nuevoAncho = 402;
                        int nuevoAlto = 336;
                        Image imagenRedimensionada = ResizeImage(image, nuevoAncho, nuevoAlto);

                        pictureBox1.Image = imagenRedimensionada;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que ocurra al descargar o mostrar la imagen
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private Image ResizeImage(Image imagenOriginal, int nuevoAncho, int nuevoAlto)
        {
            Bitmap imagenRedimensionada = new Bitmap(nuevoAncho, nuevoAlto);

            using (Graphics graficos = Graphics.FromImage(imagenRedimensionada))
            {
                graficos.DrawImage(imagenOriginal, 0, 0, nuevoAncho, nuevoAlto);
            }

            return imagenRedimensionada;
        }
    }
}
