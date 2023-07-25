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
        public int lunes, martes, miercoles, jueves, viernes, sabado, domingo = 0, cantidad = 0, cuantas = 0;
        public int identifi = 0;
        private void Planes_Load(object sender, EventArgs e)
        {
            cargarDatos();
            button1.Show();
            button2.Hide();
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
                    string query = "INSERT INTO Planes(nombre, descripcion, clase, cantidad) VALUES (@valor1, @valor2, @valor3, @valor4); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@valor1", textBox1.Text);
                    command.Parameters.AddWithValue("@valor2", textBox2.Text);
                    command.Parameters.AddWithValue("@valor3", cantidad);
                    command.Parameters.AddWithValue("@valor4", txtcuantas.Text);
                    int ultimoId = Convert.ToInt32(command.ExecuteScalar());

                    string query2 = "INSERT INTO Tarifas (plan_id,vigencia,precio) VALUES (@valor3, @valor4, @valor5); SELECT CAST(SCOPE_IDENTITY() AS INT);";

                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@valor3", ultimoId);
                    command2.Parameters.AddWithValue("@valor4", Convert.ToInt32(textBox4.Text));
                    command2.Parameters.AddWithValue("@valor5", Convert.ToDecimal(textBox3.Text));
                    int ultimoId2 = Convert.ToInt32(command2.ExecuteScalar());


                    string query3 = "INSERT INTO DiasPermitidos (plan_id,lunes,martes,miercoles,jueves,viernes,sabado,domingo) VALUES (@valor1, @valor2,@valor3, @valor4,@valor5, @valor6,@valor7, @valor8);";

                    SqlCommand command3 = new SqlCommand(query3, connection);
                    command3.Parameters.AddWithValue("@valor1", ultimoId);
                    command3.Parameters.AddWithValue("@valor2", lunes);
                    command3.Parameters.AddWithValue("@valor3", martes);
                    command3.Parameters.AddWithValue("@valor4", miercoles);
                    command3.Parameters.AddWithValue("@valor5", jueves);
                    command3.Parameters.AddWithValue("@valor6", viernes);
                    command3.Parameters.AddWithValue("@valor7", sabado);
                    command3.Parameters.AddWithValue("@valor8", domingo);
                    int ultimoId3 = Convert.ToInt32(command3.ExecuteScalar());

                    MessageBox.Show("Datos Guardados");
                    cargarDatos();
                    limpiar();
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
                        limpiar();
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
                SqlDataAdapter adapter = new SqlDataAdapter("Select Planes.id, Planes.nombre, Planes.descripcion, Tarifas.precio, Tarifas.vigencia, DiasPermitidos.lunes, DiasPermitidos.martes, DiasPermitidos.miercoles, DiasPermitidos.jueves,  DiasPermitidos.viernes,  DiasPermitidos.sabado, DiasPermitidos.domingo, Planes.clase, Planes.cantidad from Planes inner join Tarifas on Planes.id = Tarifas.plan_id inner join DiasPermitidos on Planes.id = DiasPermitidos.plan_id\r\n", connection);
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "NOMBRE PLAN";
                dataGridView1.Columns[2].HeaderText = "DESCRIPCION";
                dataGridView1.Columns[3].HeaderText = "TARIFAS";
                dataGridView1.Columns[4].HeaderText = "VIGENCIA";
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;
                dataGridView1.Columns[11].Visible = false;
                dataGridView1.Columns[12].Visible = false;
                dataGridView1.Columns[13].Visible = false;
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "ELIMINAR";
                buttonColumn.HeaderText = "ELIMINAR";
                dataGridView1.Columns.Add(buttonColumn);

                connection.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            button1.Hide();
            button2.Show();
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                string valorColumna1 = selectedRow.Cells[0].Value.ToString();
                string valorColumna2 = selectedRow.Cells[1].Value.ToString();
                string valorColumna3 = selectedRow.Cells[2].Value.ToString();
                string valorColumna4 = selectedRow.Cells[3].Value.ToString();
                string valorColumna5 = selectedRow.Cells[4].Value.ToString();
                string valorColumna6 = selectedRow.Cells[5].Value.ToString();
                string valorColumna7 = selectedRow.Cells[6].Value.ToString();
                string valorColumna8 = selectedRow.Cells[7].Value.ToString();
                string valorColumna9 = selectedRow.Cells[8].Value.ToString();
                string valorColumna10 = selectedRow.Cells[9].Value.ToString();
                string valorColumna11 = selectedRow.Cells[10].Value.ToString();
                string valorColumna12 = selectedRow.Cells[11].Value.ToString();
                string valorColumna13 = selectedRow.Cells[12].Value.ToString();
                string valorColumna14 = selectedRow.Cells[13].Value.ToString();

                identifi = Convert.ToInt32(valorColumna1);
                textBox1.Text = valorColumna2;
                textBox2.Text = valorColumna3;
                textBox3.Text = valorColumna4;
                textBox4.Text = valorColumna5;
                txtcuantas.Text = valorColumna14;

                if (valorColumna6 == "1") { checkBox1.Checked = true; } else { checkBox1.Checked = false; }
                if (valorColumna7 == "1") { checkBox2.Checked = true; } else { checkBox2.Checked = false; }
                if (valorColumna8 == "1") { checkBox3.Checked = true; } else { checkBox3.Checked = false; }
                if (valorColumna9 == "1") { checkBox4.Checked = true; } else { checkBox4.Checked = false; }
                if (valorColumna10 == "1") { checkBox5.Checked = true; } else { checkBox5.Checked = false; }
                if (valorColumna11 == "1") { checkBox6.Checked = true; } else { checkBox6.Checked = false; }
                if (valorColumna12 == "1") { checkBox7.Checked = true; } else { checkBox7.Checked = false; }
                if (valorColumna13 == "1") { checkBox9.Checked = true; } else { checkBox9.Checked = false; }
                checkBox1.Enabled = true;
                checkBox2.Enabled = true;
                checkBox3.Enabled = true;
                checkBox4.Enabled = true;
                checkBox5.Enabled = true;
                checkBox6.Enabled = true;
                checkBox7.Enabled = true;
                checkBox8.Enabled = true;
                checkBox9.Enabled = true;

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { lunes = 1; } else { lunes = 0; checkBox1.Checked = false; }
            checkBox1.Enabled = true;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked) { martes = 1; } else { martes = 0; checkBox2.Checked = false; }
            checkBox2.Enabled = true;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked) { miercoles = 1; } else { miercoles = 0; checkBox3.Checked = false; }
            checkBox3.Enabled = true;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked) { jueves = 1; } else { jueves = 0; checkBox4.Checked = false; }
            checkBox4.Enabled = true;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox5.Checked) { viernes = 1; } else { viernes = 0; checkBox5.Checked = false; }
            checkBox5.Enabled = true;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox6.Checked) { sabado = 1; } else { sabado = 0; checkBox6.Checked = false; }
            checkBox6.Enabled = true;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox7.Checked) { domingo = 1; } else { domingo = 0; checkBox7.Checked = false; }
            checkBox7.Enabled = true;
        }

        public void limpiar()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            txtcuantas.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            checkBox4.Checked = false;
            checkBox5.Checked = false;
            checkBox6.Checked = false;
            checkBox7.Checked = false;
            checkBox8.Checked = false;
            checkBox9.Checked = false;
            button1.Show();
            button2.Hide();
            cantidad = 0;
            cuantas = 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "update Planes set nombre = @valor1, descripcion = @valor2, clase = @valor4, cantidad = @valor5 where id = @valor3";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@valor1", textBox1.Text);
                command.Parameters.AddWithValue("@valor2", textBox2.Text);
                command.Parameters.AddWithValue("@valor3", identifi);
                command.Parameters.AddWithValue("@valor4", cantidad);
                command.Parameters.AddWithValue("@valor5", txtcuantas.Text);
                int filasActualizadas = command.ExecuteNonQuery();

                string query2 = "update Tarifas set vigencia = @valor1, precio = @valor2 where plan_id = @valor3";

                SqlCommand command2 = new SqlCommand(query2, connection);
                command2.Parameters.AddWithValue("@valor3", identifi);
                command2.Parameters.AddWithValue("@valor1", Convert.ToInt32(textBox4.Text));
                command2.Parameters.AddWithValue("@valor2", Convert.ToDecimal(textBox3.Text));
                int filasActualizadas2 = command2.ExecuteNonQuery();

                string query3 = "update DiasPermitidos set lunes = @valor1, martes = @valor2, miercoles = @valor3, jueves = @valor4, viernes = @valor5, sabado = @valor6, domingo = @valor7 where plan_id = @valor8";

                SqlCommand command3 = new SqlCommand(query3, connection);
                command3.Parameters.AddWithValue("@valor1", lunes);
                command3.Parameters.AddWithValue("@valor2", martes);
                command3.Parameters.AddWithValue("@valor3", miercoles);
                command3.Parameters.AddWithValue("@valor4", jueves);
                command3.Parameters.AddWithValue("@valor5", viernes);
                command3.Parameters.AddWithValue("@valor6", sabado);
                command3.Parameters.AddWithValue("@valor7", domingo);
                command3.Parameters.AddWithValue("@valor8", identifi);
                int filasActualizadas3 = command3.ExecuteNonQuery();

                MessageBox.Show("Datos Guardados");
                cargarDatos();
                limpiar();
                connection.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            limpiar();

        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox9.Checked)
            {
                txtcuantas.Visible = true;
                label8.Visible = true;
                cantidad = 1;
            }
            else
            {
                txtcuantas.Visible = false;
                label8.Visible = false;
                cantidad = 0;
            }
        }

        private void txtcuantas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
