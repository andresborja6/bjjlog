using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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
        List<string> nombresMeses = new List<string>();
        List<int> diasAsis = new List<int>();
        private void ReportGeneral_Load(object sender, EventArgs e)
        {
            cargarDatos();
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
        public void cargarDatos()
        {
            string query = "SELECT Nombres, Apellidos, Identificacion, Imagen FROM registros WHERE Id = " + Idregistro;
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
                        //txtnombre.Text = reader[1].ToString();
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

    }
}
