using Microsoft.Win32;
using System;
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
    public partial class Registros : Form
    {
        public Registros()
        {
            InitializeComponent();
        }
        public string connectionString { get; set; }
        private void Registros_Load(object sender, EventArgs e)
        {
            cargarRegistros();
        }
        public void cargarRegistros()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                dataGridView1.Columns.Clear();
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Nombres, Apellidos, Identificacion, Direccion, Email  FROM registros", connection);
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].HeaderText = "NOMBRES";
                dataGridView1.Columns[1].HeaderText = "APELLIDOS";
                dataGridView1.Columns[2].HeaderText = "IDENTIFICACION";
                dataGridView1.Columns[3].HeaderText = "DIRECCION";
                dataGridView1.Columns[4].HeaderText = "EMAIL";


                connection.Close();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                dataGridView1.Columns.Clear();
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Nombres, Apellidos, Identificacion, Direccion, Email  FROM registros where Identificacion like '%" + textBox1.Text + "%'", connection);
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].HeaderText = "NOMBRES";
                dataGridView1.Columns[1].HeaderText = "APELLIDOS";
                dataGridView1.Columns[2].HeaderText = "IDENTIFICACION";
                dataGridView1.Columns[3].HeaderText = "DIRECCION";
                dataGridView1.Columns[4].HeaderText = "EMAIL";


                connection.Close();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Addregistro addregistro = new Addregistro();
            addregistro.connectionString = connectionString;
            addregistro.ShowDialog();
        }
    }
}
