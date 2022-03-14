using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;

namespace Installer
{
    /// <summary>
    /// RepairWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InstallWindow : Window, INotifyPropertyChanged
    {
        public static string datadir = Environment.GetEnvironmentVariable("LocalAppData") + "\\RegsvrRepair\\";
        public static string sysdir = Environment.GetFolderPath(Environment.SpecialFolder.System);
        public static string sysdirx86 = Environment.GetFolderPath(Environment.SpecialFolder.SystemX86);
        public static string bindir = System.AppDomain.CurrentDomain.BaseDirectory + @"bin\";


        public InstallWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public InstallWindow(string path)
        {
            InitializeComponent();
            this.DataContext = this;
            this.path = path;
        }

        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                this.RaisePropertyChanged("Message");
            }
        }
        private string info;

        public string Info
        {
            get { return info; }
            set
            {
                info = value;
                this.RaisePropertyChanged("Info");
            }
        }

        private string path;

       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(Go);
            t.Start();
        }

        private void Fail()
        {
            this.Dispatcher.Invoke(() => {
                buttonclose.Visibility = Visibility.Visible;
                textstate.Text = "安装失败";
                progressbar1.IsIndeterminate = false;
            });
        }

        private void Success(string str)
        {
            this.Dispatcher.Invoke(() => {
                buttonclose.Visibility = Visibility.Visible;
                textstate.Text = str;
                progressbar1.IsIndeterminate = false;
            });
        }

        private void Go()
        {
            Info = "校验目录";
            DirectoryInfo dir;
            try
            {
                Directory.CreateDirectory(path);
                dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    Message = "创建目录失败";
                    Fail();
                    return;
                }
            }
            catch(Exception ex)
            {
                Message = "创建目录失败";
                Fail();
                return;
            }

            Info = "复制文件";
            try
            {
                CopyDirectory(bindir, path);
            }
            catch(Exception ex)
            {
                Message = "复制文件失败：" + ex.Message;
                Fail();
                return;
            }

            Info = "注册协议";
            try
            {
                RegisterUriScheme("lab", path + "\\" + "MyWebLab.exe");
            }
            catch (Exception ex)
            {
                Message = "注册协议失败";
                Fail();
                return;
            }
            Info = "任务完成";
            Success("安装完成");
        }

        public static void CopyDirectory(string srcPath, string destPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos(); 
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)
                    {
                        if (!Directory.Exists(destPath + "\\" + i.Name))
                        {
                            Directory.CreateDirectory(destPath + "\\" + i.Name);
                        }
                        CopyDirectory(i.FullName, destPath + "\\" + i.Name);
                    }
                    else
                    {
                        File.Copy(i.FullName, destPath + "\\" + i.Name, true);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void RegisterUriScheme(string scheme, string applicationPath)
        {
            string filename = Path.GetFileName(applicationPath);
            using (var schemeKey = Registry.ClassesRoot.CreateSubKey(scheme, writable: true))
            {
                schemeKey.SetValue("", "URL:Lab Protocol");
                schemeKey.SetValue("URL Protocol", "");

                using (var defaultIconKey = schemeKey.CreateSubKey("DefaultIcon"))
                {
                    defaultIconKey.SetValue("", filename + ",1");
                }
                using (var shellKey = schemeKey.CreateSubKey("shell"))
                using (var openKey = shellKey.CreateSubKey("open"))
                using (var commandKey = openKey.CreateSubKey("command"))
                {
                    commandKey.SetValue("", string.Format("\"{0}\" \"%1\"", applicationPath));
                }
            }
        }

        private void buttonclose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
