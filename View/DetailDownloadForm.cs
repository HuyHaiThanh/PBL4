using Core;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCP;
using Receiver;

namespace View
{
    public partial class DetailDownloadForm : Form
    {
        TCPFileDownloader tcpFileDownloader;
        Http.HttpFileDownloader httpDowloader;
        Client client;

        public DetailDownloadForm(TCPFileDownloader tcp)
        {
            InitializeComponent();
            tcpFileDownloader = tcp;
            RegisterEvent();
        }
        public DetailDownloadForm(Http.HttpFileDownloader httpDownload)
        {
            InitializeComponent();
            httpDowloader = httpDownload;
            RegisterEvent();
        }
        public DetailDownloadForm(Client client)
        {
            InitializeComponent();
            this.client = client;
            RegisterEvent();
        }
        public void RegisterEvent()
        {
            Observer.Instance.Register(EventId.OnProcessDownloadStart, TCP_OnProgressDownloadStart);
            Observer.Instance.Register(EventId.OnProcessDownloadProgress, TCP_OnProgressDownloadProgress);
            Observer.Instance.Register(EventId.OnProcessDownloadCompleted, TCP_OnProgressDownloadCompleted);
        }
        public void UnRegisterEvent()
        {
            Observer.Instance.Unregister(EventId.OnProcessDownloadStart, TCP_OnProgressDownloadStart);
            Observer.Instance.Unregister(EventId.OnProcessDownloadProgress, TCP_OnProgressDownloadProgress);
            Observer.Instance.Unregister(EventId.OnProcessDownloadCompleted, TCP_OnProgressDownloadCompleted);
        }


        private void DetailDownloadForm_Load(object sender, EventArgs e)
        {
            if (tcpFileDownloader != null)
            {
                Task.Run(() => tcpFileDownloader.StartDownload());
            }
            else if (httpDowloader != null)
            {
                Task.Run(() => httpDowloader.StartDownload());
            }
            else if(client != null)
            {
                Task.Run(() =>  client.StartDownload());
            }

        }


        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            if (btnPauseResume.Text == "Pause")
            {
                btnPauseResume.Text = "Continue";
            }
            else
                btnPauseResume.Text = "Pause";
            tcpFileDownloader.TogglePause();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(tcpFileDownloader != null)
            {
                tcpFileDownloader.Cancelled();
            }
            else if(httpDowloader != null)
            {
                httpDowloader.Cancelled();
            }
            this.Close();
        }
        public void TCP_OnProgressDownloadCompleted(object obj)
        {
            this.Invoke((MethodInvoker)delegate
            {
                Close();
            });
        }
        public void TCP_OnProgressDownloadStart(object obj)
        {
            this.Invoke((MethodInvoker)delegate
            {
                FileDownLoadStatus status = (FileDownLoadStatus)obj;
                if ((int)status == (int)FileDownLoadStatus.Connect)
                {
                    lbStatus.Text = "Tìm kiếm file";
                }
            });

        }
        public void TCP_OnProgressDownloadProgress(object file)
        {
            this.Invoke((MethodInvoker)delegate
            {
                FileDetail obj = (FileDetail)file;
                lbFileName.Text = obj.fileName;
                lbContentSize.Text = obj.totalBytes.ToString("F2") + "MB";
                lbBytesReceived.Text = obj.downloadedBytes.ToString("F2") + "MB";
                lbProgress.Text = obj.progress + "%";
                lbSpeed.Text = obj.speed.ToString("F2") + "MB/s";
                lbStatus.Text = obj.status.ToString();
                progressBar1.Value = obj.progress;
                lbThreads.Text = obj.numThreads.ToString();
            });
        }

        private void DetailDownloadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnRegisterEvent();
        }
    }

}
//https://drive.google.com/uc?authuser=0&id=1Il7sUWuNpWOnnWCAjLjtTaViXVuRO6yD&export=download
//https://drive.usercontent.google.com/u/0/uc?id=1No3sYXcIv4wZ2Jgj5vusiEdjUaBRng85&export=download
//https://drive.usercontent.google.com/u/0/uc?id=12GI_BmNAhQm1vWKzRDs-Vqg2K7UQTEqg&export=download
