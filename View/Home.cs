using Core;
using HttpFileDownloader;
using Receiver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCP;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace View
{
    public partial class HOME : Form
    {
        public HOME()
        {
            InitializeComponent();
            this.Size = new Size(550, 400);
            Observer.Instance.Register(EventId.OnHasError, MainForm_OnHasError);
            Observer.Instance.Register(EventId.OnGetLinkByHttp, OnGetLinkHttp);
            ClaimURL claimURL = new ClaimURL();
            claimURL.ClaimExtension();
        }



        private void btnInternet_Click(object sender, EventArgs e)
        {

            panel2.Location = panel1.Location;
            panel2.Size = panel1.Size;

            panel1.Visible = false;
            panel2.Visible = true;
        }

        private void btnLAN_Click(object sender, EventArgs e)
        {
            panel1.Location = panel2.Location;
            panel1.Size = panel2.Size;

            panel2.Visible = false;
            panel1.Visible = true;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            SettingInfo settingInfo = Utilities.LoadFromJson();

            if (dataGridView1.SelectedRows.Count != 1 || settingInfo == null) return;
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            string fileName = selectedRow.Cells["_fileName"].Value.ToString();


            string serverIP = settingInfo.serverIP;
            int serverPort = settingInfo.serverPort;
            string filePath = settingInfo.pathSave + "\\" + fileName;
            int numThreads = settingInfo.connectionLimit;


            if (comboBox1.SelectedIndex == 0)
            {
                TCPFileDownloader downloader = new TCPFileDownloader(serverIP, serverPort, fileName, filePath, numThreads);
                DetailDownloadForm detailForm = new DetailDownloadForm(downloader);
                detailForm.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                Client downloader = new Client(serverIP, serverPort, fileName, filePath, numThreads);
                DetailDownloadForm detailForm = new DetailDownloadForm(downloader);
                detailForm.ShowDialog();
            }
        }

        private void btnGetFile_Click(object sender, EventArgs e)
        {
            SettingInfo settingInfo = Utilities.LoadFromJson();
            if (settingInfo == null || settingInfo.serverIP == null || settingInfo.serverPort == 0) return;
            if (comboBox1.SelectedIndex == 0)
            {
                TCPFileDownloader downloader = new TCPFileDownloader(settingInfo.serverIP, settingInfo.serverPort);
                if (downloader.GetAvailableFiles() != null)
                {
                    int count = downloader.GetAvailableFiles().Count;
                    dataGridView1.DataSource = downloader.GetAvailableFiles().Select(p => new
                    {
                        _fileName = p.fileName,
                        _size = (p.totalBytes / (1024 * 1024)).ToString("F0") + "MB",
                    }).ToList();
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                Client downloader = new Client(settingInfo.serverIP, settingInfo.serverPort);
                if (downloader.GetFileList() != null && downloader.GetFileList().Count != 0)
                {
                    int count = downloader.GetFileList().Count;
                    dataGridView1.DataSource = downloader.GetFileList().Select(p => new
                    {
                        _fileName = p.fileName,
                        _size = (p.totalBytes / (1024 * 1024)).ToString("F0") + "MB",
                    }).ToList();
                }
            }

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var sform = new SettingForm();
            sform.ShowDialog();
        }
        public void MainForm_OnHasError(object obj)
        {
            string message = (string)obj;
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public void OnGetLinkHttp(object obj)
        {
            MessageBox.Show("sda");
            this.Invoke((MethodInvoker)delegate
            {
                string url = (string)obj;
                SettingInfo settingInfo = Utilities.LoadFromJson();
                Http.HttpClientDownloader downloader = new Http.HttpClientDownloader(url, settingInfo.pathSave, settingInfo.connectionLimit);
                DetailDownloadForm detailForm = new DetailDownloadForm(downloader);
                detailForm.ShowDialog();

            });


        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer.Instance.Unregister(EventId.OnHasError, MainForm_OnHasError);
            Observer.Instance.Unregister(EventId.OnGetLinkByHttp, OnGetLinkHttp);
        }


        private void btn_download(object sender, EventArgs e)
        {
            SettingInfo settingInfo = Utilities.LoadFromJson();
            string url = textBoxUrl.Text;
            if (String.IsNullOrEmpty(url)) return;
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (!result)
            {
                MessageBox.Show("Please enter a valid url to start download!", "Invalid url", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Http.HttpClientDownloader downloader = new Http.HttpClientDownloader(url, settingInfo.pathSave, settingInfo.connectionLimit);
            DetailDownloadForm detailForm = new DetailDownloadForm(downloader);
            detailForm.ShowDialog();
        }


        private void btn_SettingInternet(object sender, EventArgs e)
        {
            var sform = new SettingForm();
            sform.ShowDialog();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
