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
using AForge.Video;
using AForge.Video.DirectShow;


namespace bjjlog
{
    public partial class Addregistro : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private Bitmap capturedImage;
        private bool isVideoPlaying = true;
        public string connectionString { get; set; }
        int idsel, vigenci = 0;
        public Addregistro()
        {
            InitializeComponent();
        }

        private void Addregistro_Load(object sender, EventArgs e)
        {
            button1.Hide();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy";
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo info in videoDevices)
            {
                comboBox1.Items.Add(info.Name);
            }
            comboBox1.SelectedIndex = 0;
            videoSource = new VideoCaptureDevice();

            CargarDatosComboBox();

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

        private void Addregistro_FormClosing(object sender, FormClosingEventArgs e)
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
        private Image ResizeImage(Image imagenOriginal, int nuevoAncho, int nuevoAlto)
        {
            Bitmap imagenRedimensionada = new Bitmap(nuevoAncho, nuevoAlto);

            using (Graphics graficos = Graphics.FromImage(imagenRedimensionada))
            {
                graficos.DrawImage(imagenOriginal, 0, 0, nuevoAncho, nuevoAlto);
            }

            return imagenRedimensionada;
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
                    comboBox3.Items.Add(new Item { ID = id, Texto = texto, Vigencia = vigencia });
                }

                comboBox3.DisplayMember = "Texto";
                comboBox3.ValueMember = "ID";
            }
        }
        public class Item
        {
            public int ID { get; set; }
            public string Texto { get; set; }
            public int Vigencia { get; set; }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && textBox2.Text == "" && textBox3.Text == "")
            {
                MessageBox.Show("Por lo menos llenar informacion basica");
                return;
            }
            string carpetaDestino = Environment.CurrentDirectory + @"\imagenes";
            string nombreArchivo = textBox1.Text + "-" + textBox3.Text + ".jpg";

            string rutaDestino = Path.Combine(carpetaDestino, nombreArchivo);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO registros (Nombres,Apellidos,Identificacion,Direccion,Barrio,Municipio,EPS,FechaNacimiento,TFijo,TCelular,Email,Empresa,Tempresa,Emarcial,FechaEntrada,Imagen) VALUES (@valor1,@valor2,@valor3,@valor4,@valor5,@valor6 ,@valor7,@valor8,@valor9,@valor10,@valor11,@valor12,@valor13,@valor14,@valor15,@valor16); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@valor1", textBox1.Text);
                command.Parameters.AddWithValue("@valor2", textBox2.Text);
                command.Parameters.AddWithValue("@valor3", textBox3.Text);
                command.Parameters.AddWithValue("@valor4", textBox4.Text);
                command.Parameters.AddWithValue("@valor5", textBox5.Text);
                command.Parameters.AddWithValue("@valor6", textBox6.Text);
                command.Parameters.AddWithValue("@valor7", textBox7.Text);
                command.Parameters.AddWithValue("@valor8", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@valor9", textBox8.Text);
                command.Parameters.AddWithValue("@valor10", textBox9.Text);
                command.Parameters.AddWithValue("@valor11", textBox10.Text);
                command.Parameters.AddWithValue("@valor12", textBox11.Text);
                command.Parameters.AddWithValue("@valor13", textBox12.Text);
                command.Parameters.AddWithValue("@valor14", comboBox2.Text);
                command.Parameters.AddWithValue("@valor15", dateTimePicker2.Value);
                command.Parameters.AddWithValue("@valor16", rutaDestino);
                int ultimoId = Convert.ToInt32(command.ExecuteScalar());
                if (ultimoId > 0)
                {
                    GuardarImagenDesdePictureBox(rutaDestino);
                    DateTime fechaFinal = dateTimePicker2.Value.AddDays(vigenci);
                    string query2 = "Insert into movimientos (idregistro,idplan,fechainicio,fechafin,pago) VALUES (@valor1,@valor2,@valor3,@valor4,@valor5)";

                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@valor1", ultimoId);
                    command2.Parameters.AddWithValue("@valor2", idsel);
                    command2.Parameters.AddWithValue("@valor3", dateTimePicker2.Value);
                    command2.Parameters.AddWithValue("@valor4", fechaFinal);
                    command2.Parameters.AddWithValue("@valor5", comboBox4.Text);
                    int ultimoId2 = Convert.ToInt32(command2.ExecuteScalar());
                }
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            idsel = ((Item)comboBox3.SelectedItem).ID;
            vigenci = ((Item)comboBox3.SelectedItem).Vigencia;
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
        private void GuardarImagenDesdePictureBox(string ruta)
        {
            Image imagen = pictureBox1.Image;

            if (imagen != null)
            {

                imagen.Save(ruta);
            }
        }

    }
}
