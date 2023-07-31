using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Numerics;

namespace bjjlog
{
    public partial class Addregistro : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private Bitmap capturedImage;
        private bool isVideoPlaying = true;
        string RemitenteC = "";
        string PassC = "";
        public string connectionString { get; set; }
        int idsel, cantidad, vigenci = 0;
        string texto = "";
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
                    comboBox3.Items.Add(new Item { ID = id, Texto = texto, Vigencia = vigencia, Cantidad = cantidad });
                }

                comboBox3.DisplayMember = "Texto";
                comboBox3.ValueMember = "ID";
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

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || comboBox3.Text == "" || comboBox3.Text == "")
            {
                MessageBox.Show("Por lo menos llenar informacion basica o validar pago");
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
                    string query2 = "Insert into movimientos (idregistro,idplan,fechainicio,fechafin,pago,cantidad) VALUES (@valor1,@valor2,@valor3,@valor4,@valor5,@valor6)";

                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@valor1", ultimoId);
                    command2.Parameters.AddWithValue("@valor2", idsel);
                    command2.Parameters.AddWithValue("@valor3", dateTimePicker2.Value);
                    command2.Parameters.AddWithValue("@valor4", fechaFinal);
                    command2.Parameters.AddWithValue("@valor5", comboBox4.Text);
                    command2.Parameters.AddWithValue("@valor6", cantidad);
                    int ultimoId2 = Convert.ToInt32(command2.ExecuteScalar());
                    EnviarCorreo(textBox10.Text, textBox1.Text + " " + textBox2.Text, RemitenteC, PassC);
                    if (comboBox4.Text == "Si")
                    {
                        EnviarPago(textBox10.Text, textBox1.Text + " " + textBox2.Text, dateTimePicker2.Value, fechaFinal, texto, RemitenteC, PassC);
                    }
                    this.Close();
                }
            }

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            idsel = ((Item)comboBox3.SelectedItem).ID;
            vigenci = ((Item)comboBox3.SelectedItem).Vigencia;
            cantidad = ((Item)comboBox3.SelectedItem).Cantidad;
            texto = ((Item)comboBox3.SelectedItem).Texto;
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
        static void EnviarCorreo(string destinatario, string nombre, string Remitente, string Pass)
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
                mail.Subject = "Bienvenido a la familia Checkmat Colombia";
                mail.Body = "<!DOCTYPE html><html><head><title>Bienvenido/a al equipo de Jiujitsu</title><style>@media only screen and (max-width: 600px){.container{width: 100% !important;}}.image-container {display: inline-block;margin-right: 10px;}</style></head><body><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\"><tr><td align=\"center\" bgcolor=\"white\" style=\"padding: 20px;\"><div class=\"image-container\"><img src=\"https://lh3.googleusercontent.com/p/AF1QipON79mde03CWrFcylPvQ-EqWgTW03lZxlq65Kgg=s1360-w1360-h1020\" alt=\"Imagen de bienvenida\" style=\"width: 150px; height: 150px;\"></div><div class=\"image-container\"><img src=\"https://checkmatmadrid.com/wp-content/uploads/2016/02/checkmat-logo.jpg\" alt=\"Imagen de bienvenida\" style=\"width: 200px; height: 200px;\"></div><br><h1 style=\"color: #333333; text-align: center;\">¡Bienvenido/a Checkmat Colombia</h1><p style=\"font-size: 16px; text-align: center;\">Estimado/a  " + nombre + ",</p><p style=\"font-size: 16px; text-align: center;\">¡Gracias por unirte a nuestro equipo! Estamos emocionados de tenerte como parte de nuestra comunidad y esperamos compartir increíbles experiencias de entrenamiento y crecimiento contigo.</p><p style=\"font-size: 16px; text-align: center;\">Como miembro de nuestro equipo, tendrás acceso a entrenamientos de alta calidad, aprendizaje constante y una comunidad apasionada y solidaria.</p><p style=\"font-size: 16px; text-align: center;\">Si tienes alguna pregunta, no dudes en comunicarte con nosotros. Estamos aquí para ayudarte en tu viaje en el mundo del Jiujitsu.</p><p style=\"font-size: 16px; text-align: center;\">¡Bienvenido/a una vez más y nos vemos en los entrenamientos!</p><p style=\"font-size: 16px; text-align: center;\">Atentamente,</p><p style=\"font-size: 16px; text-align: center;\">Alessandro Nagaishi</p></td></tr></table></body></html>";
                mail.IsBodyHtml = true;
                smtpClient.Send(mail);

                Console.WriteLine("Correo enviado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
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
                mail.Body = "<!DOCTYPE html><html><head><title>Confirmación de Pago</title><style>body {font-family: Arial, sans-serif;text-align: center; margin: 50px; }h1 { color: #007bff; } p {font-size: 18px; }.button {display: inline-block;padding: 10px 20px;background-color: #007bff;color: #fff;text-decoration: none; border-radius: 5px;margin-top: 20px; } .image-container { display: inline-block;margin-right: 10px; }</style></head><body><table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"max-width: 600px;\"><tr><td align=\"center\" bgcolor=\"white\" style=\"padding: 20px;\"><div class=\"image-container\"><img src=\"https://lh3.googleusercontent.com/p/AF1QipON79mde03CWrFcylPvQ-EqWgTW03lZxlq65Kgg=s1360-w1360-h1020\" alt=\"Imagen de bienvenida\" style=\"width: 150px; height: 150px;\"></div><div class=\"image-container\"><img src=\"https://checkmatmadrid.com/wp-content/uploads/2016/02/checkmat-logo.jpg\"alt=\"Imagen de bienvenida\" style=\"width: 150px; height: 150px;\"></div><br><h1>¡Pago Registrado con Éxito!</h1><p>Hola " + nombre + " Gracias por realizar el pago. Hemos registrado la transacción en nuestro sistema.</p><p>Adquiriste el plan " + plan + " </p><p>Tiene vigencia desde " + inicio.ToString("yyyy-MM-dd") + " hasta " + fin.ToString("yyyy-MM-dd") + "</p><p>Si tienes alguna pregunta o inquietud, no dudes en contactarnos.</p><a class=\"button\" href=\"https://api.whatsapp.com/send?phone=573104372005&text=Hola tengo una inquietud con mi pago.\" target=\"_blank\">Contactar</a></td></tr></table></body></html>";
                mail.IsBodyHtml = true;
                smtpClient.Send(mail);

                Console.WriteLine("Correo enviado correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
