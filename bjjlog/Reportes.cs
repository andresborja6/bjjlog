using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace bjjlog
{
    public partial class Reportes : Form
    {
        public Reportes()
        {
            InitializeComponent();
        }
        public string connectionString { get; set; }
        int idsel, cantidad, vigenci = 0;
        private void Reportes_Load(object sender, EventArgs e)
        {
            cargarRegistros();
            CargarDatosComboBox();
        }

        private void button1_Click(object sender, EventArgs e)
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
                SqlDataAdapter adapter = new SqlDataAdapter("select r.Id, a.id, r.Nombres, r.Apellidos, r.Identificacion, r.Emarcial, p.nombre, m.pago ,a.fecha from registros r JOIN asistencia a on r.Id = a.idregistro JOIN Planes p on p.id = a.idplan JOIN movimientos m on m.idregistro = r.Id", connection);
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].HeaderText = "NOMBRES";
                dataGridView1.Columns[3].HeaderText = "APELLIDOS";
                dataGridView1.Columns[4].HeaderText = "IDENTIFICACION";
                dataGridView1.Columns[5].HeaderText = "EXPERIENCIA MARCIAL";
                dataGridView1.Columns[6].HeaderText = "PLAN";
                dataGridView1.Columns[7].HeaderText = "PAGO";
                dataGridView1.Columns[8].HeaderText = "FECHA";
                DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn();
                buttonColumn1.Name = "MODIFICAR";
                buttonColumn1.HeaderText = "VER MAS";

                dataGridView1.Columns.Add(buttonColumn1);

                connection.Close();
            }

        }
        public void cargarRegistroslike()
        {
            string consulta = "select r.Id, a.id, r.Nombres, r.Apellidos, r.Identificacion, r.Emarcial, p.nombre, m.pago ,a.fecha from registros r JOIN asistencia a on r.Id = a.idregistro JOIN Planes p on p.id = a.idplan JOIN movimientos m on m.idregistro = r.Id";
            List<string> filters = new List<string>();

            if (idsel > 0)
            {
                filters.Add("p.id = " + idsel);
            }
            if (comboBox1.Text != "")
            {
                filters.Add("r.Emarcial = '" + comboBox1.Text + "'");
            }
            if (comboBox3.Text != "")
            {
                filters.Add("m.pago = '" + comboBox3.Text + "'");
            }
            if (txtnombre.Text != "")
            {
                filters.Add("r.Nombres LIKE '%" + txtnombre.Text + "%'");
            }
            if (txtapellido.Text != "")
            {
                filters.Add("r.Apellidos LIKE '%" + txtapellido.Text + "%'");
            }
            if (txtidentificacion.Text != "")
            {
                filters.Add("r.Identificacion LIKE '%" + txtidentificacion.Text + "%'");
            }
            if (dateTimePicker1.Value != DateTimePicker.MinimumDateTime)
            {
                // Agrega el filtro de fecha a la lista de filtros
                filters.Add("a.fecha >= '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "'");
            }
            if (dateTimePicker2.Value != DateTimePicker.MinimumDateTime)
            {
                // Agrega el filtro de fecha a la lista de filtros
                filters.Add("a.fecha <= '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'");
            }

            string whereClause = string.Join(" AND ", filters);

            if (!string.IsNullOrEmpty(whereClause))
            {
                consulta = consulta + " WHERE " + whereClause;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();


                dataGridView1.Columns.Clear();
                DataTable table = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(consulta, connection);
                adapter.Fill(table);

                dataGridView1.DataSource = table;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].HeaderText = "NOMBRES";
                dataGridView1.Columns[3].HeaderText = "APELLIDOS";
                dataGridView1.Columns[4].HeaderText = "IDENTIFICACION";
                dataGridView1.Columns[5].HeaderText = "EXPERIENCIA MARCIAL";
                dataGridView1.Columns[6].HeaderText = "PLAN";
                dataGridView1.Columns[7].HeaderText = "PAGO";
                dataGridView1.Columns[8].HeaderText = "FECHA";
                DataGridViewButtonColumn buttonColumn1 = new DataGridViewButtonColumn();
                /* buttonColumn1.Name = "MODIFICAR";
                 buttonColumn1.HeaderText = "VER MAS";

                 dataGridView1.Columns.Add(buttonColumn1);*/

                connection.Close();
            }

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            cargarRegistroslike();
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
                    comboBox2.Items.Add(new Item { ID = id, Texto = texto, Vigencia = vigencia, Cantidad = cantidad });
                }

                comboBox2.DisplayMember = "Texto";
                comboBox2.ValueMember = "ID";
            }
        }
        public class Item
        {
            public int ID { get; set; }
            public string Texto { get; set; }
            public int Vigencia { get; set; }
            public int Cantidad { get; set; }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            idsel = ((Item)comboBox2.SelectedItem).ID;
            vigenci = ((Item)comboBox2.SelectedItem).Vigencia;
            cantidad = ((Item)comboBox2.SelectedItem).Cantidad;
            cargarRegistroslike();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarRegistroslike();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarRegistroslike();
        }

        private void txtapellido_TextChanged(object sender, EventArgs e)
        {
            cargarRegistroslike();
        }

        private void txtidentificacion_TextChanged(object sender, EventArgs e)
        {
            cargarRegistroslike();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            cargarRegistroslike();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            cargarRegistroslike();
        }
        public void limpiar()
        {
            txtnombre.Text = "";
            txtapellido.Text = "";
            txtidentificacion.Text = "";
            comboBox1.SelectedIndex = -1;
            comboBox1.Text = "";
            comboBox3.SelectedIndex = -1;
            comboBox3.Text = "";
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            limpiar();
        }
    }
}
