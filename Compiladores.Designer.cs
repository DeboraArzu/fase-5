namespace WFA_COMPILADORES_AJ
{
    partial class FRMPRINCIPAL
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.rchtb_Texto = new System.Windows.Forms.RichTextBox();
            this.txtMensaje = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dgvProducciones = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dtgvNoTerminalTable = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TABLA1 = new System.Windows.Forms.DataGridView();
            this.Num_Token = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Precedencia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Asosiatividad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.guardarGOTOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cARGARDOCUMENTOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aNALIZARToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducciones)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvNoTerminalTable)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TABLA1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cARGARDOCUMENTOToolStripMenuItem,
            this.aNALIZARToolStripMenuItem,
            this.guardarGOTOToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1147, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // rchtb_Texto
            // 
            this.rchtb_Texto.BackColor = System.Drawing.SystemColors.MenuText;
            this.rchtb_Texto.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rchtb_Texto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rchtb_Texto.ForeColor = System.Drawing.Color.White;
            this.rchtb_Texto.Location = new System.Drawing.Point(10, 26);
            this.rchtb_Texto.Name = "rchtb_Texto";
            this.rchtb_Texto.Size = new System.Drawing.Size(533, 405);
            this.rchtb_Texto.TabIndex = 1;
            this.rchtb_Texto.Text = "";
            this.rchtb_Texto.TextChanged += new System.EventHandler(this.rchtb_Texto_TextChanged);
            // 
            // txtMensaje
            // 
            this.txtMensaje.BackColor = System.Drawing.SystemColors.MenuText;
            this.txtMensaje.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMensaje.Enabled = false;
            this.txtMensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMensaje.ForeColor = System.Drawing.Color.White;
            this.txtMensaje.Location = new System.Drawing.Point(9, 501);
            this.txtMensaje.Margin = new System.Windows.Forms.Padding(2);
            this.txtMensaje.Name = "txtMensaje";
            this.txtMensaje.Size = new System.Drawing.Size(834, 20);
            this.txtMensaje.TabIndex = 2;
            this.txtMensaje.TextChanged += new System.EventHandler(this.txtMensaje_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label1.Location = new System.Drawing.Point(9, 477);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Mensage";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnSave
            // 
            this.btnSave.AccessibleRole = System.Windows.Forms.AccessibleRole.Link;
            this.btnSave.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSave.Location = new System.Drawing.Point(922, 471);
            this.btnSave.Margin = new System.Windows.Forms.Padding(2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(81, 36);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Generar .DAT";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.Black;
            this.tabPage3.Controls.Add(this.dgvProducciones);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage3.Size = new System.Drawing.Size(544, 388);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Tabla Producciones";
            // 
            // dgvProducciones
            // 
            this.dgvProducciones.AllowUserToAddRows = false;
            this.dgvProducciones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvProducciones.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvProducciones.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgvProducciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.dgvProducciones.EnableHeadersVisualStyles = false;
            this.dgvProducciones.GridColor = System.Drawing.Color.CornflowerBlue;
            this.dgvProducciones.Location = new System.Drawing.Point(4, 4);
            this.dgvProducciones.Margin = new System.Windows.Forms.Padding(2);
            this.dgvProducciones.Name = "dgvProducciones";
            this.dgvProducciones.RowHeadersVisible = false;
            this.dgvProducciones.RowTemplate.Height = 24;
            this.dgvProducciones.Size = new System.Drawing.Size(539, 384);
            this.dgvProducciones.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Produccion";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 104;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Longitud";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 88;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Sig. Produccion";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 121;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Elementos";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 99;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Black;
            this.tabPage2.Controls.Add(this.dtgvNoTerminalTable);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(544, 388);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tabla No Terminal";
            // 
            // dtgvNoTerminalTable
            // 
            this.dtgvNoTerminalTable.AllowUserToAddRows = false;
            this.dtgvNoTerminalTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dtgvNoTerminalTable.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dtgvNoTerminalTable.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dtgvNoTerminalTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvNoTerminalTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn3});
            this.dtgvNoTerminalTable.EnableHeadersVisualStyles = false;
            this.dtgvNoTerminalTable.GridColor = System.Drawing.Color.CornflowerBlue;
            this.dtgvNoTerminalTable.Location = new System.Drawing.Point(5, 5);
            this.dtgvNoTerminalTable.Margin = new System.Windows.Forms.Padding(2);
            this.dtgvNoTerminalTable.Name = "dtgvNoTerminalTable";
            this.dtgvNoTerminalTable.RowHeadersVisible = false;
            this.dtgvNoTerminalTable.RowTemplate.Height = 24;
            this.dtgvNoTerminalTable.Size = new System.Drawing.Size(536, 382);
            this.dtgvNoTerminalTable.TabIndex = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Numero";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 83;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "No Terminal";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 110;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Produccion";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 104;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "First";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 60;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Black;
            this.tabPage1.Controls.Add(this.TABLA1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(544, 388);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Tabla Tokens";
            // 
            // TABLA1
            // 
            this.TABLA1.AllowUserToAddRows = false;
            this.TABLA1.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TABLA1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TABLA1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Num_Token,
            this.ID,
            this.Precedencia,
            this.Asosiatividad});
            this.TABLA1.EnableHeadersVisualStyles = false;
            this.TABLA1.GridColor = System.Drawing.Color.CornflowerBlue;
            this.TABLA1.Location = new System.Drawing.Point(5, 5);
            this.TABLA1.Margin = new System.Windows.Forms.Padding(2);
            this.TABLA1.Name = "TABLA1";
            this.TABLA1.RowHeadersVisible = false;
            this.TABLA1.RowTemplate.Height = 24;
            this.TABLA1.Size = new System.Drawing.Size(538, 382);
            this.TABLA1.TabIndex = 4;
            // 
            // Num_Token
            // 
            this.Num_Token.HeaderText = "Num_Token";
            this.Num_Token.Name = "Num_Token";
            // 
            // ID
            // 
            this.ID.HeaderText = "Simbolo";
            this.ID.Name = "ID";
            // 
            // Precedencia
            // 
            this.Precedencia.HeaderText = "Precedencia";
            this.Precedencia.Name = "Precedencia";
            // 
            // Asosiatividad
            // 
            this.Asosiatividad.HeaderText = "Asosiatividad";
            this.Asosiatividad.Name = "Asosiatividad";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(549, 46);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(552, 418);
            this.tabControl1.TabIndex = 5;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.tabPage4.Controls.Add(this.richTextBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage4.Size = new System.Drawing.Size(544, 388);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Estados";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.Window;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(2);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(547, 392);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // btnGenerar
            // 
            this.btnGenerar.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnGenerar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGenerar.Location = new System.Drawing.Point(1007, 470);
            this.btnGenerar.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(81, 36);
            this.btnGenerar.TabIndex = 8;
            this.btnGenerar.Text = "Generar .TOK";
            this.btnGenerar.UseVisualStyleBackColor = false;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // guardarGOTOToolStripMenuItem
            // 
            this.guardarGOTOToolStripMenuItem.Image = global::WFA_COMPILADORES_AJ.Resource1.if_file_documents_11_854152;
            this.guardarGOTOToolStripMenuItem.Name = "guardarGOTOToolStripMenuItem";
            this.guardarGOTOToolStripMenuItem.Size = new System.Drawing.Size(115, 24);
            this.guardarGOTOToolStripMenuItem.Text = "Guardar GOTO";
            this.guardarGOTOToolStripMenuItem.Click += new System.EventHandler(this.guardarGOTOToolStripMenuItem_Click);
            // 
            // cARGARDOCUMENTOToolStripMenuItem
            // 
            this.cARGARDOCUMENTOToolStripMenuItem.Image = global::WFA_COMPILADORES_AJ.Resource1.if_file_documents_09_854153;
            this.cARGARDOCUMENTOToolStripMenuItem.Name = "cARGARDOCUMENTOToolStripMenuItem";
            this.cARGARDOCUMENTOToolStripMenuItem.Size = new System.Drawing.Size(140, 24);
            this.cARGARDOCUMENTOToolStripMenuItem.Text = "Cargar Documento";
            this.cARGARDOCUMENTOToolStripMenuItem.Click += new System.EventHandler(this.cARGARDOCUMENTOToolStripMenuItem_Click);
            // 
            // aNALIZARToolStripMenuItem
            // 
            this.aNALIZARToolStripMenuItem.Image = global::WFA_COMPILADORES_AJ.Resource1.if_magnifier_data_532758;
            this.aNALIZARToolStripMenuItem.Name = "aNALIZARToolStripMenuItem";
            this.aNALIZARToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.aNALIZARToolStripMenuItem.Text = "Analizar";
            this.aNALIZARToolStripMenuItem.Click += new System.EventHandler(this.aNALIZARToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Image = global::WFA_COMPILADORES_AJ.Resource1.if_17_330399;
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(61, 24);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // FRMPRINCIPAL
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1147, 528);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMensaje);
            this.Controls.Add(this.rchtb_Texto);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FRMPRINCIPAL";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "COMPILADORES";
            this.Load += new System.EventHandler(this.FRMPRINCIPAL_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducciones)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgvNoTerminalTable)).EndInit();
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TABLA1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cARGARDOCUMENTOToolStripMenuItem;
        private System.Windows.Forms.RichTextBox rchtb_Texto;
        private System.Windows.Forms.ToolStripMenuItem aNALIZARToolStripMenuItem;
        private System.Windows.Forms.TextBox txtMensaje;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dgvProducciones;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dtgvNoTerminalTable;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView TABLA1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Num_Token;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Precedencia;
        private System.Windows.Forms.DataGridViewTextBoxColumn Asosiatividad;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guardarGOTOToolStripMenuItem;
    }
}

