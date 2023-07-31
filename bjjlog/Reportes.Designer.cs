namespace bjjlog
{
    partial class Reportes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Reportes));
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            button1 = new Button();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            tableLayoutPanel5 = new TableLayoutPanel();
            tableLayoutPanel6 = new TableLayoutPanel();
            txtapellido = new TextBox();
            label3 = new Label();
            txtnombre = new TextBox();
            label4 = new Label();
            label5 = new Label();
            label7 = new Label();
            label1 = new Label();
            dateTimePicker1 = new DateTimePicker();
            label2 = new Label();
            dateTimePicker2 = new DateTimePicker();
            label6 = new Label();
            label8 = new Label();
            txtidentificacion = new TextBox();
            comboBox1 = new ComboBox();
            comboBox2 = new ComboBox();
            comboBox3 = new ComboBox();
            button3 = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 6.170599F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 93.8294F));
            tableLayoutPanel1.Size = new Size(876, 551);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel2.Controls.Add(dataGridView1, 0, 2);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel5, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 37);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(870, 511);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(864, 147);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 4;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel4.Controls.Add(button1, 1, 0);
            tableLayoutPanel4.Controls.Add(button2, 2, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(175, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(512, 141);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Center;
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(131, 3);
            button1.Name = "button1";
            button1.Size = new Size(122, 135);
            button1.TabIndex = 0;
            button1.Text = "Por persona";
            button1.TextAlign = ContentAlignment.BottomCenter;
            button1.TextImageRelation = TextImageRelation.ImageAboveText;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Center;
            button2.Dock = DockStyle.Fill;
            button2.Location = new Point(259, 3);
            button2.Name = "button2";
            button2.Size = new Size(122, 135);
            button2.TabIndex = 1;
            button2.Text = "General";
            button2.TextAlign = ContentAlignment.BottomCenter;
            button2.TextImageRelation = TextImageRelation.ImageAboveText;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 258);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(864, 250);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.CellPainting += dataGridView1_CellPainting;
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel5.Controls.Add(tableLayoutPanel6, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 156);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 1;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(864, 96);
            tableLayoutPanel5.TabIndex = 2;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 6;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.666666F));
            tableLayoutPanel6.Controls.Add(txtapellido, 3, 0);
            tableLayoutPanel6.Controls.Add(label3, 0, 0);
            tableLayoutPanel6.Controls.Add(txtnombre, 1, 0);
            tableLayoutPanel6.Controls.Add(label4, 2, 0);
            tableLayoutPanel6.Controls.Add(label5, 4, 0);
            tableLayoutPanel6.Controls.Add(label7, 0, 1);
            tableLayoutPanel6.Controls.Add(label1, 0, 2);
            tableLayoutPanel6.Controls.Add(dateTimePicker1, 1, 2);
            tableLayoutPanel6.Controls.Add(label2, 2, 2);
            tableLayoutPanel6.Controls.Add(dateTimePicker2, 3, 2);
            tableLayoutPanel6.Controls.Add(label6, 2, 1);
            tableLayoutPanel6.Controls.Add(label8, 4, 1);
            tableLayoutPanel6.Controls.Add(txtidentificacion, 5, 0);
            tableLayoutPanel6.Controls.Add(comboBox1, 1, 1);
            tableLayoutPanel6.Controls.Add(comboBox2, 3, 1);
            tableLayoutPanel6.Controls.Add(comboBox3, 5, 1);
            tableLayoutPanel6.Controls.Add(button3, 4, 2);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(89, 3);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 3;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel6.Size = new Size(685, 90);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // txtapellido
            // 
            txtapellido.Dock = DockStyle.Fill;
            txtapellido.Location = new Point(345, 3);
            txtapellido.Name = "txtapellido";
            txtapellido.Size = new Size(108, 27);
            txtapellido.TabIndex = 7;
            txtapellido.TextChanged += txtapellido_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Right;
            label3.Location = new Point(47, 0);
            label3.Name = "label3";
            label3.Size = new Size(64, 30);
            label3.TabIndex = 2;
            label3.Text = "Nombre";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtnombre
            // 
            txtnombre.Dock = DockStyle.Fill;
            txtnombre.Location = new Point(117, 3);
            txtnombre.Name = "txtnombre";
            txtnombre.Size = new Size(108, 27);
            txtnombre.TabIndex = 6;
            txtnombre.TextChanged += textBox1_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Right;
            label4.Location = new Point(273, 0);
            label4.Name = "label4";
            label4.Size = new Size(66, 30);
            label4.TabIndex = 3;
            label4.Text = "Apellido";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Right;
            label5.Location = new Point(468, 0);
            label5.Name = "label5";
            label5.Size = new Size(99, 30);
            label5.TabIndex = 8;
            label5.Text = "Identificacion";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Right;
            label7.Location = new Point(26, 30);
            label7.Name = "label7";
            label7.Size = new Size(85, 30);
            label7.TabIndex = 10;
            label7.Text = "Experiencia";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Right;
            label1.Location = new Point(24, 60);
            label1.Name = "label1";
            label1.Size = new Size(87, 30);
            label1.TabIndex = 0;
            label1.Text = "Fecha inicio";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Dock = DockStyle.Fill;
            dateTimePicker1.Format = DateTimePickerFormat.Short;
            dateTimePicker1.Location = new Point(117, 63);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(108, 27);
            dateTimePicker1.TabIndex = 4;
            dateTimePicker1.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Right;
            label2.Location = new Point(271, 60);
            label2.Name = "label2";
            label2.Size = new Size(68, 30);
            label2.TabIndex = 1;
            label2.Text = "Fecha fin";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Dock = DockStyle.Fill;
            dateTimePicker2.Format = DateTimePickerFormat.Short;
            dateTimePicker2.Location = new Point(345, 63);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(108, 27);
            dateTimePicker2.TabIndex = 5;
            dateTimePicker2.ValueChanged += dateTimePicker2_ValueChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Right;
            label6.Location = new Point(302, 30);
            label6.Name = "label6";
            label6.Size = new Size(37, 30);
            label6.TabIndex = 9;
            label6.Text = "Plan";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Right;
            label8.Location = new Point(525, 30);
            label8.Name = "label8";
            label8.Size = new Size(42, 30);
            label8.TabIndex = 11;
            label8.Text = "Pago";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtidentificacion
            // 
            txtidentificacion.Dock = DockStyle.Fill;
            txtidentificacion.Location = new Point(573, 3);
            txtidentificacion.Name = "txtidentificacion";
            txtidentificacion.Size = new Size(109, 27);
            txtidentificacion.TabIndex = 12;
            txtidentificacion.TextChanged += txtidentificacion_TextChanged;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Si", "No" });
            comboBox1.Location = new Point(117, 33);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(108, 28);
            comboBox1.TabIndex = 13;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // comboBox2
            // 
            comboBox2.Dock = DockStyle.Fill;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(345, 33);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(108, 28);
            comboBox2.TabIndex = 14;
            comboBox2.SelectedIndexChanged += comboBox2_SelectedIndexChanged;
            // 
            // comboBox3
            // 
            comboBox3.Dock = DockStyle.Fill;
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "Si", "No" });
            comboBox3.Location = new Point(573, 33);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(109, 28);
            comboBox3.TabIndex = 15;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // button3
            // 
            button3.Dock = DockStyle.Fill;
            button3.Location = new Point(459, 63);
            button3.Name = "button3";
            button3.Size = new Size(108, 24);
            button3.TabIndex = 16;
            button3.Text = "Limpiar";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // Reportes
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(876, 551);
            Controls.Add(tableLayoutPanel1);
            Name = "Reportes";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Reportes";
            WindowState = FormWindowState.Maximized;
            Load += Reportes_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
        private Button button1;
        private Button button2;
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private DateTimePicker dateTimePicker1;
        private DateTimePicker dateTimePicker2;
        private TextBox txtnombre;
        private TextBox txtapellido;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox txtidentificacion;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private Button button3;
    }
}