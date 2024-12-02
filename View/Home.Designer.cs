namespace View
{
    partial class HOME
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
            this.components = new System.ComponentModel.Container();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxUrl = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.button3 = new System.Windows.Forms.Button();
            this.altoButton1 = new AltoControls.AltoButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.uDPToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tCPToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGetFile = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSettings = new AltoControls.AltoButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.uDPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tCPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "TCP",
            "UDP",
            "HTTP"});
            this.comboBox2.Location = new System.Drawing.Point(371, 8);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 38;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(141, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 37;
            this.button1.Text = "Download";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btn_download);
            // 
            // textBoxUrl
            // 
            this.textBoxUrl.Location = new System.Drawing.Point(24, 39);
            this.textBoxUrl.Name = "textBoxUrl";
            this.textBoxUrl.Size = new System.Drawing.Size(100, 20);
            this.textBoxUrl.TabIndex = 36;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(24, 72);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(474, 217);
            this.dataGridView2.TabIndex = 35;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(24, 14);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(56, 19);
            this.button3.TabIndex = 33;
            this.button3.Text = "start download";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // altoButton1
            // 
            this.altoButton1.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.altoButton1.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.altoButton1.BackColor = System.Drawing.Color.Transparent;
            this.altoButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.altoButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.altoButton1.ForeColor = System.Drawing.Color.Black;
            this.altoButton1.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.altoButton1.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.altoButton1.Location = new System.Drawing.Point(205, 8);
            this.altoButton1.Name = "altoButton1";
            this.altoButton1.Radius = 10;
            this.altoButton1.Size = new System.Drawing.Size(98, 25);
            this.altoButton1.Stroke = false;
            this.altoButton1.StrokeColor = System.Drawing.Color.Gray;
            this.altoButton1.TabIndex = 32;
            this.altoButton1.Text = "Settings";
            this.altoButton1.Transparency = false;
            this.altoButton1.Click += new System.EventHandler(this.btn_SettingInternet);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "TCP",
            "UDP",
            "HTTP"});
            this.comboBox1.Location = new System.Drawing.Point(371, 8);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 38;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(24, 72);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(474, 217);
            this.dataGridView1.TabIndex = 35;
            // 
            // uDPToolStripMenuItem1
            // 
            this.uDPToolStripMenuItem1.Name = "uDPToolStripMenuItem1";
            this.uDPToolStripMenuItem1.Size = new System.Drawing.Size(71, 20);
            this.uDPToolStripMenuItem1.Text = "INTERNET";
            this.uDPToolStripMenuItem1.CheckStateChanged += new System.EventHandler(this.btnInternet_Click);
            this.uDPToolStripMenuItem1.Click += new System.EventHandler(this.btnInternet_Click);
            // 
            // tCPToolStripMenuItem1
            // 
            this.tCPToolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tCPToolStripMenuItem1.Name = "tCPToolStripMenuItem1";
            this.tCPToolStripMenuItem1.Size = new System.Drawing.Size(42, 20);
            this.tCPToolStripMenuItem1.Text = "LAN";
            this.tCPToolStripMenuItem1.Click += new System.EventHandler(this.btnLAN_Click);
            // 
            // btnGetFile
            // 
            this.btnGetFile.Location = new System.Drawing.Point(84, 14);
            this.btnGetFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetFile.Name = "btnGetFile";
            this.btnGetFile.Size = new System.Drawing.Size(56, 19);
            this.btnGetFile.TabIndex = 34;
            this.btnGetFile.Text = "GetFile";
            this.btnGetFile.UseVisualStyleBackColor = true;
            this.btnGetFile.Click += new System.EventHandler(this.btnGetFile_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(24, 14);
            this.btnStart.Margin = new System.Windows.Forms.Padding(2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(56, 19);
            this.btnStart.TabIndex = 33;
            this.btnStart.Text = "start download";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnSettings.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSettings.ForeColor = System.Drawing.Color.Black;
            this.btnSettings.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnSettings.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnSettings.Location = new System.Drawing.Point(205, 8);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Radius = 10;
            this.btnSettings.Size = new System.Drawing.Size(98, 25);
            this.btnSettings.Stroke = false;
            this.btnSettings.StrokeColor = System.Drawing.Color.Gray;
            this.btnSettings.TabIndex = 32;
            this.btnSettings.Text = "Settings";
            this.btnSettings.Transparency = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tCPToolStripMenuItem1,
            this.uDPToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1190, 24);
            this.menuStrip1.TabIndex = 39;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // uDPToolStripMenuItem
            // 
            this.uDPToolStripMenuItem.Name = "uDPToolStripMenuItem";
            this.uDPToolStripMenuItem.Size = new System.Drawing.Size(97, 22);
            this.uDPToolStripMenuItem.Text = "UDP";
            // 
            // tCPToolStripMenuItem
            // 
            this.tCPToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uDPToolStripMenuItem});
            this.tCPToolStripMenuItem.Name = "tCPToolStripMenuItem";
            this.tCPToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.tCPToolStripMenuItem.Text = "TCP";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tCPToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 26);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboBox2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.textBoxUrl);
            this.panel2.Controls.Add(this.dataGridView2);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.altoButton1);
            this.panel2.Location = new System.Drawing.Point(569, 27);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(528, 352);
            this.panel2.TabIndex = 41;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.btnGetFile);
            this.panel1.Controls.Add(this.btnStart);
            this.panel1.Controls.Add(this.btnSettings);
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(548, 352);
            this.panel1.TabIndex = 40;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // HOME
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1190, 425);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "HOME";
            this.Text = "Home";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxUrl;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button button3;
        private AltoControls.AltoButton altoButton1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem uDPToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tCPToolStripMenuItem1;
        private System.Windows.Forms.Button btnGetFile;
        private System.Windows.Forms.Button btnStart;
        private AltoControls.AltoButton btnSettings;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem uDPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tCPToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
    }
}