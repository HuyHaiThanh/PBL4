namespace View
{
    partial class DetailDownloadForm
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
            this.lblResumeability = new System.Windows.Forms.Label();
            this.btnPauseResume = new AltoControls.AltoButton();
            this.lb3 = new System.Windows.Forms.Label();
            this.lb6 = new System.Windows.Forms.Label();
            this.lb4 = new System.Windows.Forms.Label();
            this.lb1 = new System.Windows.Forms.Label();
            this.lb2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lb7 = new System.Windows.Forms.Label();
            this.lb8 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lb5 = new System.Windows.Forms.Label();
            this.btnCancel = new AltoControls.AltoButton();
            this.lbContentSize = new System.Windows.Forms.Label();
            this.lbBytesReceived = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbThreads = new System.Windows.Forms.Label();
            this.lbSpeed = new System.Windows.Forms.Label();
            this.lbProgress = new System.Windows.Forms.Label();
            this.lbFileName = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblResumeability
            // 
            this.lblResumeability.AutoSize = true;
            this.lblResumeability.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lblResumeability.ForeColor = System.Drawing.Color.Red;
            this.lblResumeability.Location = new System.Drawing.Point(314, 66);
            this.lblResumeability.Name = "lblResumeability";
            this.lblResumeability.Size = new System.Drawing.Size(10, 13);
            this.lblResumeability.TabIndex = 16;
            this.lblResumeability.Text = " ";
            // 
            // btnPauseResume
            // 
            this.btnPauseResume.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnPauseResume.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnPauseResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPauseResume.BackColor = System.Drawing.Color.Transparent;
            this.btnPauseResume.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPauseResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnPauseResume.ForeColor = System.Drawing.Color.Black;
            this.btnPauseResume.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnPauseResume.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnPauseResume.Location = new System.Drawing.Point(193, 215);
            this.btnPauseResume.Name = "btnPauseResume";
            this.btnPauseResume.Radius = 10;
            this.btnPauseResume.Size = new System.Drawing.Size(86, 25);
            this.btnPauseResume.Stroke = false;
            this.btnPauseResume.StrokeColor = System.Drawing.Color.Gray;
            this.btnPauseResume.TabIndex = 21;
            this.btnPauseResume.Text = "Pause";
            this.btnPauseResume.Transparency = false;
            this.btnPauseResume.Click += new System.EventHandler(this.btnPauseResume_Click);
            // 
            // lb3
            // 
            this.lb3.AutoSize = true;
            this.lb3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lb3.Location = new System.Drawing.Point(5, 90);
            this.lb3.Name = "lb3";
            this.lb3.Size = new System.Drawing.Size(85, 13);
            this.lb3.TabIndex = 19;
            this.lb3.Text = "Bytes Received:";
            // 
            // lb6
            // 
            this.lb6.AutoSize = true;
            this.lb6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lb6.Location = new System.Drawing.Point(238, 90);
            this.lb6.Name = "lb6";
            this.lb6.Size = new System.Drawing.Size(41, 13);
            this.lb6.TabIndex = 20;
            this.lb6.Text = "Speed:";
            // 
            // lb4
            // 
            this.lb4.AutoSize = true;
            this.lb4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lb4.Location = new System.Drawing.Point(5, 114);
            this.lb4.Name = "lb4";
            this.lb4.Size = new System.Drawing.Size(63, 13);
            this.lb4.TabIndex = 14;
            this.lb4.Text = "Last Status:";
            // 
            // lb1
            // 
            this.lb1.AutoSize = true;
            this.lb1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lb1.Location = new System.Drawing.Point(5, 42);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(86, 13);
            this.lb1.TabIndex = 15;
            this.lb1.Text = "Server Filename:";
            // 
            // lb2
            // 
            this.lb2.AutoSize = true;
            this.lb2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lb2.Location = new System.Drawing.Point(5, 66);
            this.lb2.Name = "lb2";
            this.lb2.Size = new System.Drawing.Size(70, 13);
            this.lb2.TabIndex = 18;
            this.lb2.Text = "Content Size:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(238, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 17;
            this.label5.Text = "Resumeability:";
            // 
            // lb7
            // 
            this.lb7.AutoSize = true;
            this.lb7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lb7.Location = new System.Drawing.Point(238, 114);
            this.lb7.Name = "lb7";
            this.lb7.Size = new System.Drawing.Size(51, 13);
            this.lb7.TabIndex = 20;
            this.lb7.Text = "Progress:";
            // 
            // lb8
            // 
            this.lb8.AutoSize = true;
            this.lb8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lb8.Location = new System.Drawing.Point(238, 139);
            this.lb8.Name = "lb8";
            this.lb8.Size = new System.Drawing.Size(55, 13);
            this.lb8.TabIndex = 14;
            this.lb8.Text = "Last Error:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            // 
            // lb5
            // 
            this.lb5.AutoSize = true;
            this.lb5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lb5.Location = new System.Drawing.Point(5, 139);
            this.lb5.Name = "lb5";
            this.lb5.Size = new System.Drawing.Size(82, 13);
            this.lb5.TabIndex = 20;
            this.lb5.Text = "Active Threads:";
            // 
            // btnCancel
            // 
            this.btnCancel.Active1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.btnCancel.Active2 = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(164)))), ((int)(((byte)(183)))));
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Inactive1 = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(188)))), ((int)(((byte)(210)))));
            this.btnCancel.Inactive2 = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(167)))), ((int)(((byte)(188)))));
            this.btnCancel.Location = new System.Drawing.Point(294, 213);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Radius = 10;
            this.btnCancel.Size = new System.Drawing.Size(86, 25);
            this.btnCancel.Stroke = false;
            this.btnCancel.StrokeColor = System.Drawing.Color.Gray;
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Transparency = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lbContentSize
            // 
            this.lbContentSize.AutoSize = true;
            this.lbContentSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbContentSize.Location = new System.Drawing.Point(81, 66);
            this.lbContentSize.Name = "lbContentSize";
            this.lbContentSize.Size = new System.Drawing.Size(10, 13);
            this.lbContentSize.TabIndex = 26;
            this.lbContentSize.Text = " ";
            // 
            // lbBytesReceived
            // 
            this.lbBytesReceived.AutoSize = true;
            this.lbBytesReceived.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbBytesReceived.Location = new System.Drawing.Point(96, 90);
            this.lbBytesReceived.Name = "lbBytesReceived";
            this.lbBytesReceived.Size = new System.Drawing.Size(10, 13);
            this.lbBytesReceived.TabIndex = 27;
            this.lbBytesReceived.Text = " ";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbStatus.Location = new System.Drawing.Point(74, 114);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(10, 13);
            this.lbStatus.TabIndex = 28;
            this.lbStatus.Text = " ";
            // 
            // lbThreads
            // 
            this.lbThreads.AutoSize = true;
            this.lbThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbThreads.Location = new System.Drawing.Point(93, 139);
            this.lbThreads.Name = "lbThreads";
            this.lbThreads.Size = new System.Drawing.Size(10, 13);
            this.lbThreads.TabIndex = 29;
            this.lbThreads.Text = " ";
            // 
            // lbSpeed
            // 
            this.lbSpeed.AutoSize = true;
            this.lbSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbSpeed.Location = new System.Drawing.Point(285, 90);
            this.lbSpeed.Name = "lbSpeed";
            this.lbSpeed.Size = new System.Drawing.Size(10, 13);
            this.lbSpeed.TabIndex = 30;
            this.lbSpeed.Text = " ";
            // 
            // lbProgress
            // 
            this.lbProgress.AutoSize = true;
            this.lbProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbProgress.Location = new System.Drawing.Point(289, 114);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(10, 13);
            this.lbProgress.TabIndex = 31;
            this.lbProgress.Text = " ";
            // 
            // lbFileName
            // 
            this.lbFileName.AutoSize = true;
            this.lbFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.lbFileName.Location = new System.Drawing.Point(97, 42);
            this.lbFileName.Name = "lbFileName";
            this.lbFileName.Size = new System.Drawing.Size(10, 13);
            this.lbFileName.TabIndex = 32;
            this.lbFileName.Text = " ";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 178);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(372, 23);
            this.progressBar1.TabIndex = 33;
            // 
            // DetailDownloadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 250);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbFileName);
            this.Controls.Add(this.lbProgress);
            this.Controls.Add(this.lbSpeed);
            this.Controls.Add(this.lbThreads);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbBytesReceived);
            this.Controls.Add(this.lbContentSize);
            this.Controls.Add(this.lblResumeability);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnPauseResume);
            this.Controls.Add(this.lb3);
            this.Controls.Add(this.lb5);
            this.Controls.Add(this.lb7);
            this.Controls.Add(this.lb6);
            this.Controls.Add(this.lb8);
            this.Controls.Add(this.lb4);
            this.Controls.Add(this.lb1);
            this.Controls.Add(this.lb2);
            this.Controls.Add(this.label5);
            this.Name = "DetailDownloadForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Downloader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DetailDownloadForm_FormClosing);
            this.Load += new System.EventHandler(this.DetailDownloadForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblResumeability;
        private AltoControls.AltoButton btnPauseResume;
        private System.Windows.Forms.Label lb3;
        private System.Windows.Forms.Label lb6;
        private System.Windows.Forms.Label lb4;
        private System.Windows.Forms.Label lb1;
        private System.Windows.Forms.Label lb2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lb7;
        private System.Windows.Forms.Label lb8;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lb5;
        private AltoControls.AltoButton btnCancel;
        private System.Windows.Forms.Label lbContentSize;
        private System.Windows.Forms.Label lbBytesReceived;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbThreads;
        private System.Windows.Forms.Label lbSpeed;
        private System.Windows.Forms.Label lbProgress;
        private System.Windows.Forms.Label lbFileName;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}