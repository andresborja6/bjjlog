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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace bjjlog
{
    public partial class UsuarioApp : Form
    {
        public UsuarioApp()
        {
            InitializeComponent();
        }
        public string connectionString { get; set; }
        public int control, valorColumna1 = 0;
        private void UsuarioApp_Load(object sender, EventArgs e)
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
                SqlDataAdapter adapter = new SqlDataAdapter("select * from usuarios", connection);
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "Usuario";
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[3].HeaderText = "Tipo";
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "ELIMINAR";
                buttonColumn.HeaderText = "ELIMINAR";
                dataGridView1.Columns.Add(buttonColumn);

                connection.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.Text == "")
            {
                MessageBox.Show("Por favor llenar todos los campos");
                return;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (control == 1)
                {
                    string query = "Update usuarios set Usuario = @valor1,Contrasena = @valor2,Tipo = @valor3 where Id = @valor4";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@valor1", textBox1.Text);
                    command.Parameters.AddWithValue("@valor2", textBox2.Text);
                    command.Parameters.AddWithValue("@valor3", comboBox1.Text);
                    command.Parameters.AddWithValue("@valor4", valorColumna1);
                    int ultimoId = Convert.ToInt32(command.ExecuteScalar());
                    MessageBox.Show("Datos actualizados");
                    limpiar();
                    cargarRegistros();
                }
                else
                {
                    string query = "Insert into usuarios (Usuario,Contrasena,Tipo) VALUES (@valor1, @valor2, @valor3)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@valor1", textBox1.Text);
                    command.Parameters.AddWithValue("@valor2", textBox2.Text);
                    command.Parameters.AddWithValue("@valor3", comboBox1.Text);
                    int ultimoId = Convert.ToInt32(command.ExecuteScalar());
                    MessageBox.Show("Datos guardados");
                    limpiar();
                    cargarRegistros();
                }
                connection.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                valorColumna1 = Convert.ToInt32(selectedRow.Cells[0].Value.ToString());
                string valorColumna2 = selectedRow.Cells[1].Value.ToString();
                string valorColumna3 = selectedRow.Cells[2].Value.ToString();
                string valorColumna4 = selectedRow.Cells[3].Value.ToString();

                textBox1.Text = valorColumna2;
                textBox2.Text = valorColumna3;
                comboBox1.SelectedItem = valorColumna4;

                control = 1;

            }
        }
        public void limpiar()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1.Text = "";
            control = 0;
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "ELIMINAR" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                DataGridViewButtonCell celBoton = dataGridView1.Rows[e.RowIndex].Cells["ELIMINAR"] as DataGridViewButtonCell;
                Icon icoAtomico = new Icon(Environment.CurrentDirectory + @"\Cancel_48px.ico");
                e.Graphics.DrawIcon(icoAtomico, e.CellBounds.Left + 20, e.CellBounds.Top + 3);
                dataGridView1.Rows[e.RowIndex].Height = icoAtomico.Height + 5;
                dataGridView1.Columns[e.ColumnIndex].Width = icoAtomico.Width + 40;
                e.Handled = true;
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "ELIMINAR")
            {
                DataGridViewButtonCell buttonCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
                if (buttonCell != null)
                {
                    buttonCell.Style.BackColor = Color.Red;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                if (e.ColumnIndex == dataGridView1.Columns["ELIMINAR"].Index && e.RowIndex >= 0)
                {
                    DialogResult resultado = MessageBox.Show("¿Está seguro de que desea eliminar el registro? ", "Confirmar eliminación", MessageBoxButtons.YesNo);
                    if (resultado == DialogResult.Yes)
                    {
                        int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                        string queryDelete = "Delete usuarios where Id = @idCliente";
                        using (SqlCommand commandDelete = new SqlCommand(queryDelete, connection))
                        {
                            commandDelete.Parameters.AddWithValue("@idCliente", id);
                            int rowsAffected = commandDelete.ExecuteNonQuery();
                        }
                        cargarRegistros();
                        limpiar();
                        connection.Close();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CredencialesMail credencialesMail = new CredencialesMail();
            credencialesMail.connectionString = connectionString;
            credencialesMail.Show();
        }
    }
}
