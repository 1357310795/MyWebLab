using MyWebLab;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using USTCORi.WebLabClient.Common;

namespace USTCORi.WebLabClient.Views.DownloadWindow
{
    public partial class ShowWindow : Window
    {
        public ShowWindow()
        {
            this.InitializeComponent();
           
            DispatcherHelper.DoEvents();
        }

        public void start()
        {
            this.timer = new DispatcherTimer();
            this.timer.Interval = new TimeSpan(0, 0, 1);
            this.timer.IsEnabled = true;
            this.timer.Tick += this.timer_Tick;
            this.timer.Start();
        }
        
        private void timer_Tick(object sender, EventArgs e)
        {
            Process[] myProcesses = Process.GetProcesses();
            foreach (Process myProcess in myProcesses)
            {
                if (myProcess.ProcessName.Trim() == "Main" || myProcess.ProcessName.Trim() == "wowexec")
                {
                    this.timer.Stop();
                    base.Close();
                    return;
                }
            }
            Process running = App.runningInfo.Global.RunningProcess;
            if (running != null && !running.HasExited)
            {
                this.timer.Stop();
                base.Close();
                return;
            }
        }

        private void element_MediaEnded(object sender, RoutedEventArgs e)
        {
            ((MediaElement)sender).Position = ((MediaElement)sender).Position.Add(TimeSpan.FromMilliseconds(1.0));
        }

        private DispatcherTimer timer = new DispatcherTimer();

    }
}
