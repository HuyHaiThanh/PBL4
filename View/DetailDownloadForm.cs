using Core;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using TCP;
using Receiver;
using HttpFileDownloader;

namespace View
{
    public partial class DetailDownloadForm : Form
    {
        TCPFileDownloader tcpFileDownloader;
        Http.HttpClientDownloader httpDowloader;
        InternetFileDownload customHttpDownloader;
        Client client;

        public DetailDownloadForm(TCPFileDownloader tcp)
        {
            InitializeComponent();
            tcpFileDownloader = tcp;
            RegisterEvent();
        }
        public DetailDownloadForm(Http.HttpClientDownloader httpDownload)
        {
            InitializeComponent();
            httpDowloader = httpDownload;
            RegisterEvent();
        }
        public DetailDownloadForm(InternetFileDownload httpDownload)
        {
            InitializeComponent();
            customHttpDownloader = httpDownload;
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
            Observer.Instance.Register(EventId.OnHasError, DetailForm_OnHasError);
        }
        public void UnRegisterEvent()
        {
            Observer.Instance.Unregister(EventId.OnProcessDownloadStart, TCP_OnProgressDownloadStart);
            Observer.Instance.Unregister(EventId.OnProcessDownloadProgress, TCP_OnProgressDownloadProgress);
            Observer.Instance.Unregister(EventId.OnProcessDownloadCompleted, TCP_OnProgressDownloadCompleted);
            Observer.Instance.Register(EventId.OnHasError, DetailForm_OnHasError);
        }

        public void DetailForm_OnHasError(object obj)
        {
            this.Invoke((MethodInvoker)delegate
            {
                string message = (string)obj;
                MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (tcpFileDownloader != null)
                {
                    tcpFileDownloader.Cancelled();
                }
                else if (httpDowloader != null)
                {
                    httpDowloader.Cancelled();
                }
                else if(customHttpDownloader != null)
                {
                    customHttpDownloader.Cancelled();
                }
                this.Close();
            });
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
            else if (client != null)
            {
                Task.Run(() => client.StartDownload());
            }
            else if (customHttpDownloader != null)
            {
                Task.Run(() => customHttpDownloader.DownloadFileAsync());
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
            tcpFileDownloader?.TogglePause();
            httpDowloader?.TogglePause();
            customHttpDownloader?.TogglePause();
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
            else if(customHttpDownloader != null)
            {
                customHttpDownloader.Cancelled();
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
                    lbStatus.Text = "Get file infomation";
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
                lbBytesReceived.Text = ((double)obj.downloadedBytes / (1024 * 1024)).ToString("F2") + "MB";
                lbProgress.Text = obj.progress + "%";
                lbSpeed.Text = obj.speed.ToString("F2") + "MB/s";
                lbStatus.Text = obj.status.ToString();
                progressBar1.Value = obj.progress;
                lbThreads.Text = obj.numThreads.ToString();
            });
        }

        private void DetailDownloadForm_FormClosing(object sender, FormClosingEventArgs e)
        {

             if (tcpFileDownloader != null)
             {
                    tcpFileDownloader.Cancelled();
             }
             else if (httpDowloader != null)
             {
                    httpDowloader.Cancelled();
             }
             else if(customHttpDownloader != null)
            {
                customHttpDownloader.Cancelled();
            }

            UnRegisterEvent();
        }
    }

}
//https://drive.google.com/uc?authuser=0&id=1Il7sUWuNpWOnnWCAjLjtTaViXVuRO6yD&export=download
//https://drive.usercontent.google.com/u/0/uc?id=12GI_BmNAhQm1vWKzRDs-Vqg2K7UQTEqg&export=download

//https://drive.usercontent.google.com/download?id=1WVb5mnrxh-LjgXA4Z-mEBpWIWi09i60b&authuser=0&confirm=t&uuid=0bde2abf-171a-49ed-9864-25ca1a7efee6&at=AENtkXbvif4bpOOVtHVTPwxtSATi%3A1731933259364
