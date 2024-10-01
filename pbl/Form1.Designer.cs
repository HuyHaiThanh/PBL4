namespace pbl
{
    partial class Form1
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddDownload = new AltoControls.AltoButton();
            this.btnResume = new AltoControls.AltoButton();
            this.btnDelete = new AltoControls.AltoButton();
            this.btnSettings = new AltoControls.AltoButton();
            this.btnIntegrateChrome = new AltoControls.AltoButton();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader7,
            this.columnHeader3,
            this.columnHeader8,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(59, 101);
            this.listView1.Margin = new System.Windows.Forms.Padding(4);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(999, 271);
            this.listView1.TabIndex = 14;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 155;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Progress";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Total Bytes Received";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "ContentSize";
            this.columnHeader3.Width = 110;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Status";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Speed";
            this.columnHeader4.Width = 103;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Resumeability";
            this.columnHeader5.Width = 146;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Url";
            this.columnHeader6.Width = 156;
            // 
            // btnAddDownload
            // 
            this.btnAddDownload.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnAddDownload.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnAddDownload.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDownload.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAddDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAddDownload.ForeColor = System.Drawing.Color.Black;
            this.btnAddDownload.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnAddDownload.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnAddDownload.Location = new System.Drawing.Point(59, 34);
            this.btnAddDownload.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddDownload.Name = "btnAddDownload";
            this.btnAddDownload.Radius = 10;
            this.btnAddDownload.Size = new System.Drawing.Size(131, 31);
            this.btnAddDownload.Stroke = false;
            this.btnAddDownload.StrokeColor = System.Drawing.Color.Gray;
            this.btnAddDownload.TabIndex = 15;
            this.btnAddDownload.Text = "Add Download";
            this.btnAddDownload.Transparency = false;
            this.btnAddDownload.Click += new System.EventHandler(this.btnAddDownload_Click);
            // 
            // btnResume
            // 
            this.btnResume.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnResume.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnResume.BackColor = System.Drawing.Color.Transparent;
            this.btnResume.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnResume.ForeColor = System.Drawing.Color.Black;
            this.btnResume.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnResume.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnResume.Location = new System.Drawing.Point(216, 34);
            this.btnResume.Margin = new System.Windows.Forms.Padding(4);
            this.btnResume.Name = "btnResume";
            this.btnResume.Radius = 10;
            this.btnResume.Size = new System.Drawing.Size(131, 31);
            this.btnResume.Stroke = false;
            this.btnResume.StrokeColor = System.Drawing.Color.Gray;
            this.btnResume.TabIndex = 16;
            this.btnResume.Text = "Resume";
            this.btnResume.Transparency = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnDelete.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnDelete.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnDelete.Location = new System.Drawing.Point(376, 34);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Radius = 10;
            this.btnDelete.Size = new System.Drawing.Size(131, 31);
            this.btnDelete.Stroke = false;
            this.btnDelete.StrokeColor = System.Drawing.Color.Gray;
            this.btnDelete.TabIndex = 17;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Transparency = false;
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
            this.btnSettings.Location = new System.Drawing.Point(753, 34);
            this.btnSettings.Margin = new System.Windows.Forms.Padding(4);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Radius = 10;
            this.btnSettings.Size = new System.Drawing.Size(131, 31);
            this.btnSettings.Stroke = false;
            this.btnSettings.StrokeColor = System.Drawing.Color.Gray;
            this.btnSettings.TabIndex = 18;
            this.btnSettings.Text = "Settings";
            this.btnSettings.Transparency = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnIntegrateChrome
            // 
            this.btnIntegrateChrome.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnIntegrateChrome.Active2 = System.Drawing.Color.Lime;
            this.btnIntegrateChrome.BackColor = System.Drawing.Color.Transparent;
            this.btnIntegrateChrome.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnIntegrateChrome.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnIntegrateChrome.ForeColor = System.Drawing.Color.Black;
            this.btnIntegrateChrome.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnIntegrateChrome.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnIntegrateChrome.Location = new System.Drawing.Point(905, 34);
            this.btnIntegrateChrome.Margin = new System.Windows.Forms.Padding(4);
            this.btnIntegrateChrome.Name = "btnIntegrateChrome";
            this.btnIntegrateChrome.Radius = 10;
            this.btnIntegrateChrome.Size = new System.Drawing.Size(153, 31);
            this.btnIntegrateChrome.Stroke = false;
            this.btnIntegrateChrome.StrokeColor = System.Drawing.Color.Gray;
            this.btnIntegrateChrome.TabIndex = 19;
            this.btnIntegrateChrome.Text = "Integrate Chrome";
            this.btnIntegrateChrome.Transparency = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1175, 450);
            this.Controls.Add(this.btnIntegrateChrome);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnResume);
            this.Controls.Add(this.btnAddDownload);
            this.Controls.Add(this.listView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private AltoControls.AltoButton btnAddDownload;
        private AltoControls.AltoButton btnResume;
        private AltoControls.AltoButton btnDelete;
        private AltoControls.AltoButton btnSettings;
        private AltoControls.AltoButton btnIntegrateChrome;
    }
}

