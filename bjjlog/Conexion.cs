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
    public partial class Conexion : Form
    {
        public Conexion()
        {
            InitializeComponent();
        }

        private void btnguardar_Click(object sender, EventArgs e)
        {
            if (txtdatabase.Text == "" || txtserver.Text == "")
            {
                MessageBox.Show("Por favor llene todos los campos");
            }
            else
            {
                string filePath = Environment.CurrentDirectory + @"\datosserver.txt";
                File.WriteAllText(filePath, string.Empty);
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine($"{txtserver.Text},{txtdatabase.Text}");
                }
                Application.Restart();
            }
        }
    }
}
