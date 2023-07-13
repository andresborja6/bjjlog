using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AForge.Math.FourierTransform;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Net.Mail;

namespace bjjlog
{
    public partial class Updregistro : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private Bitmap capturedImage;
        private bool isVideoPlaying = true;
        public Updregistro()
        {
            InitializeComponent();
        }
        public string connectionString { get; set; }
        public int Idregistro { get; set; }
        public string rutaimagen = "";
        int idsel, vigenci = 0;
        private void Updregistro_Load(object sender, EventArgs e)
        {
            button1.Hide();
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in videoDevices)
            {
                comboBox1.Items.Add(info.Name);
            }
            comboBox1.SelectedIndex = 0;
            videoSource = new VideoCaptureDevice();


            CargarDatosComboBox();
            dtpsalida.Enabled = false;
            string query = "SELECT r.*, m.fechainicio,m.fechafin, m.idplan FROM registros r JOIN movimientos m ON r.Id = m.idregistro WHERE r.Id = " + Idregistro + " and m.Id = (SELECT MAX(Id) FROM movimientos WHERE idregistro = r.Id)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        txtnombre.Text = reader[1].ToString();
                        txtapellido.Text = reader[2].ToString();
                        txtidentificacion.Text = reader[3].ToString();
                        txtdireccion.Text = reader[4].ToString();
                        txtbarrio.Text = reader[5].ToString();
                        txtmunicipio.Text = reader[6].ToString();
                        txteps.Text = reader[7].ToString();
                        dtpfechan.Value = Convert.ToDateTime(reader[8].ToString());
                        txttelefonofijo.Text = reader[9].ToString();
                        txtcelular.Text = reader[10].ToString();
                        txtemail.Text = reader[11].ToString();
                        txtempresa.Text = reader[12].ToString();
                        txttempresa.Text = reader[13].ToString();
                        cbexperiencia.SelectedItem = reader[14].ToString();
                        rutaimagen = reader[16].ToString();
                        dtpfechainicio.Value = Convert.ToDateTime(reader[17].ToString());
                        dtpsalida.Value = Convert.ToDateTime(reader[18].ToString());
                        int idGuardado = Convert.ToInt32(reader[19].ToString());

                        Item item = cbplan.Items.Cast<Item>().FirstOrDefault(i => i.ID == idGuardado);

                        // Si se encuentra el elemento, selecciónalo en el ComboBox
                        if (item != null)
                        {
                            cbplan.SelectedItem = item;
                        }
                        cargarImagen(rutaimagen);
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
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
                        pictureBox1.Image = image;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que ocurra al descargar o mostrar la imagen
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        private void CargarDatosComboBox()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select Planes.id, Planes.nombre, Tarifas.vigencia from Planes inner join Tarifas on Planes.id = Tarifas.plan_id";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    int id = Convert.ToInt32(row[0]);
                    string texto = row[1].ToString();
                    int vigencia = Convert.ToInt32(row[2]);
                    cbplan.Items.Add(new Item { ID = id, Texto = texto, Vigencia = vigencia });
                }

                cbplan.DisplayMember = "Texto";
                cbplan.ValueMember = "ID";
            }
        }
        public class Item
        {
            public int ID { get; set; }
            public string Texto { get; set; }
            public int Vigencia { get; set; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog.Title = "Seleccionar imagen";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string rutaImagen = openFileDialog.FileName;
                Image imagenOriginal = Image.FromFile(rutaImagen);
                int nuevoAncho = 384;
                int nuevoAlto = 247;
                Image imagenRedimensionada = ResizeImage(imagenOriginal, nuevoAncho, nuevoAlto);

                pictureBox1.Image = imagenRedimensionada;
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
        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (isVideoPlaying)
            {

                using (Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone())
                {
                    int nuevoAncho = 384;
                    int nuevoAlto = 247;
                    Bitmap resizedBitmap = new Bitmap(bitmap, nuevoAncho, nuevoAlto);
                    pictureBox1.Image = resizedBitmap;
                }
            }
        }

        private void Updregistro_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }

            if (capturedImage != null)
            {
                capturedImage.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                videoSource.NewFrame += new NewFrameEventHandler(videoSource_NewFrame);
                videoSource.Start();
            }
            button1.Show();
            button3.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                Bitmap currentFrame = (Bitmap)pictureBox1.Image.Clone();

                capturedImage = currentFrame;

                videoSource.SignalToStop();
                videoSource.WaitForStop();
                isVideoPlaying = false;

                pictureBox1.Image = capturedImage;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtnombre.Text == "" && txtapellido.Text == "" && txtidentificacion.Text == "")
            {
                MessageBox.Show("Por lo menos llenar informacion basica");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "Update registros set Nombres = @valor1,Apellidos = @valor2,Identificacion = @valor3,Direccion = @valor4,Barrio = @valor5,Municipio = @valor6,EPS = @valor7,FechaNacimiento = @valor8, TFijo = @valor9, TCelular = @valor10, Email = @valor11, Empresa = @valor12,Tempresa = @valor13, Emarcial = @valor14 where Id = @valor15";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@valor1", txtnombre.Text);
                command.Parameters.AddWithValue("@valor2", txtapellido.Text);
                command.Parameters.AddWithValue("@valor3", txtidentificacion.Text);
                command.Parameters.AddWithValue("@valor4", txtdireccion.Text);
                command.Parameters.AddWithValue("@valor5", txtbarrio.Text);
                command.Parameters.AddWithValue("@valor6", txtmunicipio.Text);
                command.Parameters.AddWithValue("@valor7", txteps.Text);
                command.Parameters.AddWithValue("@valor8", dtpfechan.Value);
                command.Parameters.AddWithValue("@valor9", txttelefonofijo.Text);
                command.Parameters.AddWithValue("@valor10", txtcelular.Text);
                command.Parameters.AddWithValue("@valor11", txtemail.Text);
                command.Parameters.AddWithValue("@valor12", txtempresa.Text);
                command.Parameters.AddWithValue("@valor13", txttempresa.Text);
                command.Parameters.AddWithValue("@valor14", cbexperiencia.Text);
                command.Parameters.AddWithValue("@valor15", Idregistro);
                int filasActualizadas = command.ExecuteNonQuery();
                if (filasActualizadas > 0)
                {
                    GuardarImagenDesdePictureBox(rutaimagen);
                    DateTime fechaFinal = dtpfechainicio.Value.AddDays(vigenci);
                    string query2 = "Insert into movimientos (idregistro,idplan,fechainicio,fechafin,pago) VALUES (@valor1,@valor2,@valor3,@valor4,@valor5)";

                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@valor1", Idregistro);
                    command2.Parameters.AddWithValue("@valor2", idsel);
                    command2.Parameters.AddWithValue("@valor3", dtpfechainicio.Value);
                    command2.Parameters.AddWithValue("@valor4", fechaFinal);
                    command2.Parameters.AddWithValue("@valor5", cbpago.Text);
                    int ultimoId2 = Convert.ToInt32(command2.ExecuteScalar());
                }
            }
            this.Close();
        }
        private void GuardarImagenDesdePictureBox(string ruta)
        {
            Image imagen = pictureBox1.Image;

            if (imagen != null)
            {

                imagen.Save(ruta);
            }
        }

        private void cbplan_SelectedIndexChanged(object sender, EventArgs e)
        {
            idsel = ((Item)cbplan.SelectedItem).ID;
            vigenci = ((Item)cbplan.SelectedItem).Vigencia;
        }
    }
}
