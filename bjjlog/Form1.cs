using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace bjjlog
{
    public partial class Form1 : Form
    {
        public string serverName = "AndresBorja\\SQLEXPRESS";
        public string databaseName = "usuarios";
        public string userNameG = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

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