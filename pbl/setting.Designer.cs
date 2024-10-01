namespace pbl
{
    partial class setting
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
            this.chkChrome = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtExtId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSaveFolder = new System.Windows.Forms.TextBox();
            this.btnOpenFBD = new AltoControls.AltoButton();
            this.btnSaveSettings = new AltoControls.AltoButton();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // chkChrome
            // 
            this.chkChrome.AutoSize = true;
            this.chkChrome.Location = new System.Drawing.Point(42, 112);
            this.chkChrome.Margin = new System.Windows.Forms.Padding(4);
            this.chkChrome.Name = "chkChrome";
            this.chkChrome.Size = new System.Drawing.Size(131, 20);
            this.chkChrome.TabIndex = 30;
            this.chkChrome.Text = "Integrate Chrome";
            this.chkChrome.UseVisualStyleBackColor = true;
            this.chkChrome.CheckedChanged += new System.EventHandler(this.chkChrome_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 52);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 16);
            this.label3.TabIndex = 29;
            this.label3.Text = "Chrome Extension Id:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtExtId
            // 
            this.txtExtId.Location = new System.Drawing.Point(192, 48);
            this.txtExtId.Margin = new System.Windows.Forms.Padding(4);
            this.txtExtId.Name = "txtExtId";
            this.txtExtId.Size = new System.Drawing.Size(340, 22);
            this.txtExtId.TabIndex = 28;
            this.txtExtId.TextChanged += new System.EventHandler(this.txtExtId_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 16);
            this.label2.TabIndex = 27;
            this.label2.Text = "Connection Limit:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(192, 80);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(79, 22);
            this.numericUpDown1.TabIndex = 26;
            this.numericUpDown1.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 16);
            this.label1.TabIndex = 25;
            this.label1.Text = "Save Folder:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtSaveFolder
            // 
            this.txtSaveFolder.Location = new System.Drawing.Point(192, 16);
            this.txtSaveFolder.Margin = new System.Windows.Forms.Padding(4);
            this.txtSaveFolder.Name = "txtSaveFolder";
            this.txtSaveFolder.ReadOnly = true;
            this.txtSaveFolder.Size = new System.Drawing.Size(340, 22);
            this.txtSaveFolder.TabIndex = 24;
            this.txtSaveFolder.TextChanged += new System.EventHandler(this.txtSaveFolder_TextChanged);
            // 
            // btnOpenFBD
            // 
            this.btnOpenFBD.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnOpenFBD.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnOpenFBD.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenFBD.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOpenFBD.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnOpenFBD.ForeColor = System.Drawing.Color.Black;
            this.btnOpenFBD.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnOpenFBD.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnOpenFBD.Location = new System.Drawing.Point(541, 16);
            this.btnOpenFBD.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFBD.Name = "btnOpenFBD";
            this.btnOpenFBD.Radius = 0;
            this.btnOpenFBD.Size = new System.Drawing.Size(33, 23);
            this.btnOpenFBD.Stroke = false;
            this.btnOpenFBD.StrokeColor = System.Drawing.Color.Gray;
            this.btnOpenFBD.TabIndex = 22;
            this.btnOpenFBD.Text = "...";
            this.btnOpenFBD.Transparency = false;
            this.btnOpenFBD.Click += new System.EventHandler(this.btnOpenFBD_Click);
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnSaveSettings.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnSaveSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnSaveSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSaveSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSaveSettings.ForeColor = System.Drawing.Color.Black;
            this.btnSaveSettings.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnSaveSettings.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnSaveSettings.Location = new System.Drawing.Point(480, 111);
            this.btnSaveSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Radius = 10;
            this.btnSaveSettings.Size = new System.Drawing.Size(95, 31);
            this.btnSaveSettings.Stroke = false;
            this.btnSaveSettings.StrokeColor = System.Drawing.Color.Gray;
            this.btnSaveSettings.TabIndex = 23;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.Transparency = false;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 184);
            this.Controls.Add(this.chkChrome);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtExtId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSaveFolder);
            this.Controls.Add(this.btnOpenFBD);
            this.Controls.Add(this.btnSaveSettings);
            this.Name = "setting";
            this.Text = "setting";
            this.Load += new System.EventHandler(this.setting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkChrome;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtExtId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSaveFolder;
        private AltoControls.AltoButton btnOpenFBD;
        private AltoControls.AltoButton btnSaveSettings;
    }
}