﻿using Core;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TCP;
using Http;
using Receiver;

namespace View
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Observer.Instance.Register(EventId.OnHasError, MainForm_OnHasError);
        }
        public void MainForm_OnHasError(object obj)
        {
            string message = (string)obj;
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAddDownload_Click(object sender, EventArgs e)
        {
            //new EnterURLForm().ShowDialog();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var sform = new SettingForm();
            sform.ShowDialog();
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
            else if(comboBox1.SelectedIndex == 1)
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
            else if(comboBox1.SelectedIndex == 1)
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

        private void btnStartDowloadHttp_Click(object sender, EventArgs e)
        {
            SettingInfo settingInfo = Utilities.LoadFromJson();
            string url = textBoxUrl.Text;
            if (String.IsNullOrEmpty(url)) return;
            Http.HttpFileDownloader downloader = new Http.HttpFileDownloader(url, settingInfo.pathSave, settingInfo.connectionLimit);
            DetailDownloadForm detailForm = new DetailDownloadForm(downloader);
            detailForm.ShowDialog();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Observer.Instance.Unregister(EventId.OnHasError, MainForm_OnHasError);
        }
    }
}