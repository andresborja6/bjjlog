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
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Id,Nombres, Apellidos, Identificacion, Email  FROM registros", connection);
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "NOMBRES";
                dataGridView1.Columns[2].HeaderText = "APELLIDOS";
                dataGridView1.Columns[3].HeaderText = "IDENTIFICACION";
                dataGridView1.Columns[4].HeaderText = "EMAIL";
                DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn();
                buttonColumn1.Name = "MODIFICAR";
                buttonColumn1.HeaderText = "MODIFICAR";
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "ELIMINAR";
                buttonColumn.HeaderText = "ELIMINAR";

                dataGridView1.Columns.Add(buttonColumn1);
                dataGridView1.Columns.Add(buttonColumn);

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
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT Id,Nombres, Apellidos, Identificacion,  Email  FROM registros where Identificacion like '%" + textBox1.Text + "%'", connection);
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].HeaderText = "NOMBRES";
                dataGridView1.Columns[2].HeaderText = "APELLIDOS";
                dataGridView1.Columns[3].HeaderText = "IDENTIFICACION";
                dataGridView1.Columns[4].HeaderText = "EMAIL";
                DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn();
                buttonColumn1.Name = "MODIFICAR";
                buttonColumn1.HeaderText = "MODIFICAR";
                DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
                buttonColumn.Name = "ELIMINAR";
                buttonColumn.HeaderText = "ELIMINAR";

                dataGridView1.Columns.Add(buttonColumn1);
                dataGridView1.Columns.Add(buttonColumn);

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "MODIFICAR" && e.RowIndex >= 0)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All);
                DataGridViewButtonCell celBoton = dataGridView1.Rows[e.RowIndex].Cells["MODIFICAR"] as DataGridViewButtonCell;
                Icon icoAtomico = new Icon(Environment.CurrentDirectory + @"\modify.ico");
                e.Graphics.DrawIcon(icoAtomico, e.CellBounds.Left + 20, e.CellBounds.Top + 3);
                dataGridView1.Rows[e.RowIndex].Height = icoAtomico.Height + 5;
                dataGridView1.Columns[e.ColumnIndex].Width = icoAtomico.Width + 40;
                e.Handled = true;
            }
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
            if (dataGridView1.Columns[e.ColumnIndex].Name == "MODIFICAR")
            {
                DataGridViewButtonCell buttonCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
                if (buttonCell != null)
                {
                    buttonCell.Style.BackColor = Color.Black;
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
                    }
                }
                else if (e.ColumnIndex == dataGridView1.Columns["MODIFICAR"].Index && e.RowIndex >= 0)
                {
                    int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                    Updregistro updregistro = new Updregistro();
                    updregistro.Idregistro = id;
                    updregistro.connectionString = connectionString;
                    updregistro.ShowDialog();
                }

                connection.Close();
            }
        }
    }
}
