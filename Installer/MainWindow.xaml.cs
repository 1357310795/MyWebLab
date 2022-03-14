using System.ComponentModel;
using System.Windows;
using System.IO;
using System;

namespace Installer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public static string localdir = Environment.GetEnvironmentVariable("LocalAppData");
        public MainWindow()
        {
            InitializeComponent();
            Path = localdir + "\\MyWebLab";
            this.DataContext = this;
        }

        private string path;

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                this.RaisePropertyChanged("Path");
            }
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (Path=="")
            {
                MessageBox.Show("路径不能为空");
                return;
            }
            InstallWindow w = new InstallWindow(Path);
            Application.Current.MainWindow = w;
            w.Show();
            this.Close();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", "");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog d = new System.Windows.Forms.FolderBrowserDialog();
            d.Description = "请选择文件路径";

            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Path = d.SelectedPath + "\\MyWebLab";
            }
        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }


        #endregion

    }
}
