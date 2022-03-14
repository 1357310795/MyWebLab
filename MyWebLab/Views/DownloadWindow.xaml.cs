using MyWebLab;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using USTCORi.WebLabClient.BizServiceReference;
using USTCORi.WebLabClient.DataModel;

namespace USTCORi.WebLabClient.Views.DownloadWindow
{
    public partial class DownloadWindow : Window
    {
        public DownloadWindow()
        {
            this.InitializeComponent();
            this.downto = App.runningInfo.Const.DownloadExperimentPath;
        }

        public void SetExperiment(LABINFO ex, TheHall thehall, string path)
        {
            this.experiment = ex;
            string baseadd = "http://" + App.runningInfo.Const.ServerIP + "/Upload/lab/";
            this.downloadAddress = new Uri(baseadd + ex.LabFileUrl);
            this.downto = path + "DownLoad\\" + ex.LABTYPENAME + "\\";
            Directory.CreateDirectory(this.downto);
            this.downto = this.downto + this.experiment.LABNAME + ".lab";
            this.BeginDownload();
        }

        public void BeginDownload()
        {
            this.downloadClient = new WebClient();
            try
            {
                this.downloadClient.DownloadProgressChanged += this.downloadClient_DownloadProgressChanged;
                this.downloadClient.DownloadFileCompleted += this.downloadClient_DownloadFileCompleted;
                this.downloadClient.DownloadFileAsync(this.downloadAddress, this.downto);
                this.CalBytesTime.Tick += this.CalBytesTime_Tick;
                this.CalBytesTime.Start();
                if (this.AddOneToLabFileDownLoadTask == null)
                {
                    this.AddOneToLabFileDownLoadTask = new Report();
                    this.AddOneToLabFileDownLoadTask.BizCode = "UstcOri.BLL.BLLLabClent";
                    this.AddOneToLabFileDownLoadTask.MethodName = "AddOneToLabFileDownLoadCount";
                    this.AddOneToLabFileDownLoadTask.ReturnType = typeof(int);
                }
                LABFILE labFile = new LABFILE();
                labFile.LABID = this.experiment.LabID;
                labFile.FILETYPE = 1;
                this.AddOneToLabFileDownLoadTask.SetParameter("labFile", labFile);
                try
                {
                    SvcResponse response = this.AddOneToLabFileDownLoadTask.Run();
                    if (string.IsNullOrEmpty(response.Message))
                    {
                        if (response.Data != null)
                        {
                            int result = (int)response.Data;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                base.Close();
            }
        }

        private void downloadClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.TotalBytesToReceive = e.TotalBytesToReceive;
            //this.DownloadProgressBar.Value = (double)(e.BytesReceived * 100L / e.TotalBytesToReceive);
            //this.txt.Text = (e.BytesReceived * 100L / e.TotalBytesToReceive).ToString();
            this.BytesReceived = e.BytesReceived;
        }

        public void CalBytesTime_Tick(object sender, EventArgs e)
        {
            long rate = this.BytesReceived / (long)this.timeIndex;
            this.timeIndex++;
            //this.ratetxt.Text = ((double)rate / 1024.0).ToString("F2");
            if ((double)rate / 1024.0 < 1024.0)
            {
                //this.warn.Visibility = Visibility.Visible;
            }
            else
            {
                //this.warn.Visibility = Visibility.Collapsed;
            }
        }

        private void downloadClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            this.CalBytesTime.Stop();
            FileInfo file = new FileInfo(this.downto);
            if (file.Length == 0L || file.Length != this.TotalBytesToReceive)
            {
                MessageBox.Show("文件下载出错");
                try
                {
                    file.Delete();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
                base.DialogResult = new bool?(false);
                base.Close();
            }
            else
            {
                //MenuManager mm = new MenuManager(this.path);
                //mm.AddDownloadXElement(this.experiment, this.path);
                base.DialogResult = new bool?(true);
                base.Close();
            }
        }

        private void DownloadCancel_Click(object sender, RoutedEventArgs e)
        {
            this.downloadClient.CancelAsync();
            base.DialogResult = new bool?(false);
            base.Close();
        }

        public void SetBackground(Brush b)
        {
            //this.MainBorder.Background = b;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private WebClient downloadClient;

        private Uri downloadAddress;

        private string downto;

        public LABINFO experiment;

        public Report AddOneToLabFileDownLoadTask;

        public long TotalBytesToReceive = 0L;

        private DispatcherTimer CalBytesTime = new DispatcherTimer
        {
            Interval = new TimeSpan(0, 0, 0, 1)
        };

        private int timeIndex = 1;

        private long BytesReceived = 0L;
    }
}
