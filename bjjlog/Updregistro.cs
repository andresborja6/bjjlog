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
        string RemitenteC = "";
        string PassC = "";
        public Updregistro()
        {
            InitializeComponent();
        }
        public string connectionString { get; set; }
        public int Idregistro { get; set; }
        public string texto, rutaimagen = "";
        int idsel, cantidad, vigenci = 0;
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
                string query = "select Planes.id, Planes.nombre, Tarifas.vigencia, Planes.cantidad from Planes inner join Tarifas on Planes.id = Tarifas.plan_id";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);

                foreach (DataRow row in dataTable.Rows)
                {
                    int id = Convert.ToInt32(row[0]);
                    string texto = row[1].ToString();
                    int vigencia = Convert.ToInt32(row[2]);
                    int cantidad = Convert.ToInt32(row[3]);
                    cbplan.Items.Add(new Item { ID = id, Texto = texto, Vigencia = vigencia, Cantidad = cantidad });
                }

                cbplan.DisplayMember = "Texto";
                cbplan.ValueMember = "ID";
                connection.Close();
                string query2 = "select * from UpdateCorreo";
                SqlCommand command2 = new SqlCommand(query2, connection);


                connection.Open();

                SqlDataReader reader2 = command2.ExecuteReader();

                if (reader2.Read())
                {
                    RemitenteC = reader2[1].ToString();
                    PassC = reader2[2].ToString();
                }
                reader2.Close();
                connection.Close();
            }
        }
        public class Item
        {
            public int ID { get; set; }
            public string Texto { get; set; }
            public int Vigencia { get; set; }
            public int Cantidad { get; set; }
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
            if (txtnombre.Text == "" || txtapellido.Text == "" || txtidentificacion.Text == "" || cbpago.Text == "")
            {
                MessageBox.Show("Por lo menos llenar informacion basica o validar pago");
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
                    string query2 = "Insert into movimientos (idregistro,idplan,fechainicio,fechafin,pago,cantidad) VALUES (@valor1,@valor2,@valor3,@valor4,@valor5,@valor6)";

                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@valor1", Idregistro);
                    command2.Parameters.AddWithValue("@valor2", idsel);
                    command2.Parameters.AddWithValue("@valor3", dtpfechainicio.Value);
                    command2.Parameters.AddWithValue("@valor4", fechaFinal);
                    command2.Parameters.AddWithValue("@valor5", cbpago.Text);
                    command2.Parameters.AddWithValue("@valor6", cantidad);
                    int ultimoId2 = Convert.ToInt32(command2.ExecuteScalar());
                    if (cbpago.Text == "Si")
                    {
                        EnviarPago(txtemail.Text, txtnombre.Text + " " + txtapellido.Text, dtpfechainicio.Value, fechaFinal, texto, RemitenteC, PassC);
                    }
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
            cantidad = ((Item)cbplan.SelectedItem).Cantidad;
            texto = ((Item)cbplan.SelectedItem).Texto;
        }
        static void EnviarPago(string destinatario, string nombre, DateTime inicio, DateTime fin, string plan, string Remitente, string Pass)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.office365.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(Remitente, Pass);

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(Remitente);
                mail.To.Add(destinatario);
                mail.Subject = "Recibimos tu pago!!!";
                mail.Body = "<!DOCTYPE html><html><head><title>Confirmación de Pago</title><style>body {font-family: Arial, sans-serif;text-align: center; margin: 50px; }h1 { color: #007bff; } p {font-size: 18px; }.button {display: inline-block;padding: 10px 20px;background-color: #007bff;color: #fff;text-decoration: none; border-radius: 5px;margin-top: 20px; } .image-container { display: inline-block;margin-right: 10px; }</style></head><body><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\"><tr><td align=\"center\" bgcolor=\"white\" style=\"padding: 20px;\"><div class=\"image-container\"><img src=\"https://lh3.googleusercontent.com/p/AF1QipON79mde03CWrFcylPvQ-EqWgTW03lZxlq65Kgg=s1360-w1360-h1020\" alt=\"Imagen de bienvenida\" style=\"width: 150px; height: 150px;\"></div><div class=\"image-container\"><img src=\"https://checkmatmadrid.com/wp-content/uploads/2016/02/checkmat-logo.jpg\"alt=\"Imagen de bienvenida\" style=\"width: 150px; height: 150px;\"></div><br><h1>¡Pago Registrado con Éxito!</h1><p>Hola " + nombre + " Gracias por realizar el pago. Hemos registrado la transacción en nuestro sistema.</p><p>Adquiriste el plan: " + plan + " </p><p>Tiene vigencia desde " + inicio.ToString("yyyy-MM-dd") + " hasta " + fin.ToString("yyyy-MM-dd") + "</p><p>Si tienes alguna pregunta o inquietud, no dudes en contactarnos.</p><a class=\"button\" href=\"https://api.whatsapp.com/send?phone=573104372005&text=Hola tengo una inquietud con mi pago.\" target=\"_blank\">Contactar</a></td></tr></table></body></html>";
                mail.IsBodyHtml = true;
                smtpClient.Send(mail);

                Console.WriteLine("Correo enviado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }
    }
}
