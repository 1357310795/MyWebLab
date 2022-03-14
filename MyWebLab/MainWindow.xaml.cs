using Easy.MessageHub;
using MyWebLab.Hack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MyWebLab
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IMessageHub AddReportManager = new MessageHub();
        public static IMessageHub DeleteReportManager = new MessageHub();
        public static IMessageHub CloseManager = new MessageHub();
        public MainWindow()
        {
            InitializeComponent();
            MainListBox.ItemsSource = ReportList.reports;
            AddReportManager.Subscribe<ReportModel>(AddReport);
            DeleteReportManager.Subscribe<ReportModel>(DeleteReport);
            CloseManager.Subscribe<object>(CloseExp);
        }

        public void AddReport(ReportModel report)
        {
            this.Dispatcher.Invoke(() => {
                ReportList.reports.Add(report);
            });
        }

        public void DeleteReport(ReportModel report)
        {
            this.Dispatcher.Invoke(() => {
                ReportList.reports.Remove(report);
            });
        }

        public void CloseExp(object o)
        {
            this.Dispatcher.Invoke(() => {
                foreach (Window w in App.Current.Windows)
                    if (!(w is MainWindow))
                        w.Close();
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
            Application.Current.Shutdown();
        }
    }
}
