using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bjjlog
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }
        public string connectionString { get; set; }
        private void Menu_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Registros registros = new Registros();
            registros.connectionString = connectionString;
            registros.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
            Form1 form1 = new Form1();
            form1.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Reportes reportes = new Reportes();
            reportes.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Planes planes = new Planes();
            planes.connectionString = connectionString;
            planes.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UsuarioApp usuarioApp = new UsuarioApp();
            usuarioApp.ShowDialog();
        }
    }
}
