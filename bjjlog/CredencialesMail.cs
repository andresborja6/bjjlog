using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bjjlog
{
    public partial class CredencialesMail : Form
    {
        public CredencialesMail()
        {
            InitializeComponent();
        }
        public string connectionString { get; set; }
        int Id = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            Actualizar();
        }
        public void cargarRegistros()
        {
            string query = "select * from UpdateCorreo";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        Id = Convert.ToInt32(reader[0].ToString());
                        textBox1.Text = reader[1].ToString();
                        textBox2.Text = reader[2].ToString();
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void CredencialesMail_Load(object sender, EventArgs e)
        {
            cargarRegistros();
        }
        public void Actualizar()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "Update UpdateCorreo set Remitente = @valor1,Contra = @valor2 where Id = @valor3";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@valor1", textBox1.Text);
                command.Parameters.AddWithValue("@valor2", textBox2.Text);
                command.Parameters.AddWithValue("@valor3", Id);
                int filasActualizadas = command.ExecuteNonQuery();
                if (filasActualizadas > 0)
                {
                    MessageBox.Show("Datos Actualizados");
                    this.Close();
                }
            }
        }
    }
}
