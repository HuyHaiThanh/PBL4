using System;

namespace Core
{
    public enum FileDownLoadStatus
    {
        Connect,
        Completed,
        Downloading,
        Stopped,
        Merging
    }
    [Serializable]
    public class FileDetail
    {
        public string fileName;
        public int progress;
        public long totalBytes;
        public long downloadedBytes;
        public FileDownLoadStatus status;
        public double speed;
        public bool resumeability;
        public int numThreads;
        public FileDetail() { }
        public FileDetail(string fileName, long totalByte,  int numThreads)
        {
            this.fileName = fileName;
            this.progress = 0;
            this.totalBytes = totalByte;
            this.downloadedBytes = 0;
            this.status = FileDownLoadStatus.Downloading;
            this.speed = 0f;
            this.resumeability = true;
            this.numThreads = numThreads;
        }
        public FileDetail(string fileName, long totalByte)
        {
            this.fileName = fileName;
            this.progress = 0;
            this.totalBytes = totalByte;
            this.downloadedBytes = 0;
            this.status = FileDownLoadStatus.Downloading;
            this.speed = 0f;
            this.resumeability = true;
        }

        public void UpdateInfo(int progress, long contentSize, double speed, FileDownLoadStatus status)
        {
            this.progress = progress;
            this.downloadedBytes = contentSize;
            this.speed = speed;
            this.status = status;
        }

    }
}
