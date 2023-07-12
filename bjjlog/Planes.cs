using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.Design.AxImporter;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace bjjlog
{
    public partial class Planes : Form
    {
        public Planes()
        {
            InitializeComponent();
        }
        public string connectionString { get; set; }
        private void Planes_Load(object sender, EventArgs e)
        {
            cargarDatos();
        }
        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox8.Checked)
            {
                checkBox1.Checked = true;
                checkBox1.Enabled = false;
                checkBox2.Checked = true;
                checkBox2.Enabled = false;
                checkBox3.Checked = true;
                checkBox3.Enabled = false;
                checkBox4.Checked = true;
                checkBox4.Enabled = false;
                checkBox5.Checked = true;
                checkBox5.Enabled = false;
                checkBox6.Checked = true;
                checkBox6.Enabled = false;
                checkBox7.Checked = true;
                checkBox7.Enabled = false;
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = true;
                checkBox7.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Por favor llenar los datos ");
                return;
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Planes(nombre, descripcion) VALUES (@valor1, @valor2); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@valor1", textBox1.Text);
                    command.Parameters.AddWithValue("@valor2", textBox2.Text);
                    int ultimoId = Convert.ToInt32(command.ExecuteScalar());

                    string query2 = "INSERT INTO Tarifas (plan_id,vigencia,precio) VALUES (@valor3, @valor4, @valor5); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@valor3", ultimoId);
                    command2.Parameters.AddWithValue("@valor4", Convert.ToInt32(textBox4.Text));
                    command2.Parameters.AddWithValue("@valor5", Convert.ToDecimal(textBox3.Text));
                    int ultimoId2 = Convert.ToInt32(command2.ExecuteScalar());

                    if (checkBox1.Checked)
                    {
                        string query3 = "INSERT INTO DiasPermitidos (plan_id,diaSemana) VALUES (@valor6, @valor7); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        SqlCommand command3 = new SqlCommand(query3, connection);
                        command3.Parameters.AddWithValue("@valor6", ultimoId);
                        command3.Parameters.AddWithValue("@valor7", 1);
                        int ultimoId3 = Convert.ToInt32(command3.ExecuteScalar());
                    }
                    if (checkBox2.Checked)
                    {
                        string query4 = "INSERT INTO DiasPermitidos (plan_id,diaSemana) VALUES (@valor8, @valor9); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        SqlCommand command4 = new SqlCommand(query4, connection);
                        command4.Parameters.AddWithValue("@valor8", ultimoId);
                        command4.Parameters.AddWithValue("@valor9", 2);
                        int ultimoId4 = Convert.ToInt32(command4.ExecuteScalar());
                    }
                    if (checkBox3.Checked)
                    {
                        string query5 = "INSERT INTO DiasPermitidos (plan_id,diaSemana) VALUES (@valor1, @valor2); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        SqlCommand command5 = new SqlCommand(query5, connection);
                        command5.Parameters.AddWithValue("@valor1", ultimoId);
                        command5.Parameters.AddWithValue("@valor2", 3);
                        int ultimoId5 = Convert.ToInt32(command5.ExecuteScalar());
                    }
                    if (checkBox4.Checked)
                    {
                        string query6 = "INSERT INTO DiasPermitidos (plan_id,diaSemana) VALUES (@valor1, @valor2); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        SqlCommand command6 = new SqlCommand(query6, connection);
                        command6.Parameters.AddWithValue("@valor1", ultimoId);
                        command6.Parameters.AddWithValue("@valor2", 4);
                        int ultimoId6 = Convert.ToInt32(command6.ExecuteScalar());
                    }
                    if (checkBox5.Checked)
                    {
                        string query7 = "INSERT INTO DiasPermitidos (plan_id,diaSemana) VALUES (@valor1, @valor2); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        SqlCommand command7 = new SqlCommand(query7, connection);
                        command7.Parameters.AddWithValue("@valor1", ultimoId);
                        command7.Parameters.AddWithValue("@valor2", 5);
                        int ultimoId7 = Convert.ToInt32(command7.ExecuteScalar());
                    }
                    if (checkBox6.Checked)
                    {
                        string query8 = "INSERT INTO DiasPermitidos (plan_id,diaSemana) VALUES (@valor1, @valor2); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        SqlCommand command8 = new SqlCommand(query8, connection);
                        command8.Parameters.AddWithValue("@valor1", ultimoId);
                        command8.Parameters.AddWithValue("@valor2", 6);
                        int ultimoId8 = Convert.ToInt32(command8.ExecuteScalar());
                    }
                    if (checkBox7.Checked)
                    {
                        string query9 = "INSERT INTO DiasPermitidos (plan_id,diaSemana) VALUES (@valor1, @valor2); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                        SqlCommand command9 = new SqlCommand(query9, connection);
                        command9.Parameters.AddWithValue("@valor1", ultimoId);
                        command9.Parameters.AddWithValue("@valor2", 7);
                        int ultimoId9 = Convert.ToInt32(command9.ExecuteScalar());
                    }
                    MessageBox.Show("Datos Guardados");
                }
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
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
                        string queryDelete = "DELETE FROM DiasPermitidos where plan_id =  @idCliente";
                        using (SqlCommand commandDelete = new SqlCommand(queryDelete, connection))
                        {
                            commandDelete.Parameters.AddWithValue("@idCliente", id);
                            int rowsAffected = commandDelete.ExecuteNonQuery();
                        }

                        string queryDelete2 = "DELETE FROM Tarifas where plan_id = @idCliente";
                        using (SqlCommand commandDelete2 = new SqlCommand(queryDelete2, connection))
                        {
                            commandDelete2.Parameters.AddWithValue("@idCliente", id);
                            int rowsAffected = commandDelete2.ExecuteNonQuery();
                        }

                        string queryDelete3 = "DELETE FROM Planes where id =  @idCliente";
                        using (SqlCommand commandDelete3 = new SqlCommand(queryDelete3, connection))
                        {
                            commandDelete3.Parameters.AddWithValue("@idCliente", id);
                            int rowsAffected = commandDelete3.ExecuteNonQuery();
                            MessageBox.Show($"Registros eliminados: {rowsAffected}");
                        }
                        cargarDatos();
                        connection.Close();
                    }
                }
                else
                {

                }
            }
        }
        public void cargarDatos()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                dataGridView1.Columns.Clear();
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(" Select Planes.id, Planes.nombre, Planes.descripcion, Tarifas.precio from Planes inner join Tarifas on Planes.id = Tarifas.plan_id", connection);
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "NOMBRE PLAN";
                dataGridView1.Columns[2].HeaderText = "DESCRIPCION";
                dataGridView1.Columns[3].HeaderText = "TARIFAS";
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "ELIMINAR";
                buttonColumn.HeaderText = "ELIMINAR";
                dataGridView1.Columns.Add(buttonColumn);

                connection.Close();
            }
        }
    }
}
