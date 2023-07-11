using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace bjjlog
{
    public partial class Form1 : Form
    {
        public string serverName = "";
        public string databaseName = "";
        public string userNameG = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] filePath = new string[0];
            filePath = File.ReadAllLines(Environment.CurrentDirectory + @"\datosserver.txt");
            foreach (string linea in filePath)
            {
                string[] campos = linea.Split(',');
                serverName = campos[0].Trim().Replace("\\\\", "\\");
                databaseName = campos[1].Trim();

            }
            string connectionString = "Data Source=" + serverName + ";Initial Catalog=" + databaseName + ";Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Por favor actualice los datos de conexion a la base de datos");
                    Conexion con = new Conexion();
                    this.WindowState = FormWindowState.Minimized;
                    con.Show();
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtusuario.Text;
            string password = txtpass.Text;

            SqlConnection conn = new SqlConnection("Data Source=" + serverName + ";Initial Catalog=" + databaseName + ";Integrated Security=true");

            try
            {
                conn.Open();

                string sql = "SELECT * FROM usuarios where Usuario =  @userName AND Contrasena = @passWord";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@userName", username);
                cmd.Parameters.AddWithValue("@passWord", password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Menu mainForm = new Menu();
                        //mainForm.userNameG = reader.GetString(0);
                        mainForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Nombre de usuario o contraseña incorrectos.", "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al intentar iniciar sesión: " + ex.Message, "Error de inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}