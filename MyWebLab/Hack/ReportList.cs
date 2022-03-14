using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MyWebLab.Hack
{
    public delegate CommonResult DoReportDelegate(object args);

    public class ReportList
    {
        public static ObservableCollection<ReportModel> reports = new ObservableCollection<ReportModel>();
    }

    public class ReportModel : BindableBase
    {
        private string expName;
        public string ExpName
        {
            get { return expName; }
            set
            {
                expName = value;
                this.RaisePropertyChanged("ExpName");
            }
        }

        private string taskName;
        public string TaskName
        {
            get { return taskName; }
            set
            {
                taskName = value;
                this.RaisePropertyChanged("TaskName");
            }
        }

        private ReportState state;
        public ReportState State
        {
            get { return state; }
            set
            {
                state = value;
                this.RaisePropertyChanged("State");
            }
        }

        private string result;
        public string Result
        {
            get { return result; }
            set
            {
                result = value;
                this.RaisePropertyChanged("Result");
            }
        }

        public object args;
        public DoReportDelegate DoReportMethod;
        public BackgroundWorker bgw;

        public DelegateCommand runcommand { get; set; }
        public DelegateCommand closecommand { get; set; }
        public DelegateCommand cancelcommand { get; set; }

        public ReportModel(string expName, string taskName, object args, DoReportDelegate doReportMethod)
        {
            ExpName = expName;
            TaskName = taskName;
            this.args = args;
            DoReportMethod = doReportMethod;
            runcommand = new DelegateCommand(Run);
            closecommand = new DelegateCommand(Close);
            cancelcommand = new DelegateCommand(Cancel);
            State = ReportState.Waiting;
        }

        public void Run()
        {
            switch (State)
            {
                case ReportState.Cancelled:
                    MessageBox.Show("已取消的任务无法运行", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return ;
                case ReportState.Done:
                    if (MessageBox.Show("任务已完成，您确定要重新进行？\n注意：这可能有潜在的风险！", "提示", MessageBoxButton.YesNo, MessageBoxImage.Information)!=MessageBoxResult.Yes)
                        return ;
                    break;
                case ReportState.Error:
                    return ;
                case ReportState.Running:
                    MessageBox.Show("任务正在运行！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return ;
                case ReportState.Waiting:
                    break ;
            }
            State = ReportState.Running;
            if (bgw == null)
            {
                bgw = new BackgroundWorker();
                bgw.DoWork += Bgw_DoWork;
            }
            bgw.RunWorkerAsync();
        }

        private void Bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            var res = DoReportMethod.Invoke(args);
            Result = res.result;
            if (res.success)
                State = ReportState.Done;
            else
                State = ReportState.Error;
        }

        public void Close()
        {
            if (State == ReportState.Running)
            {
                MessageBox.Show("任务正在运行，无法删除！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MainWindow.DeleteReportManager.Publish(this);
        }

        public void Cancel()
        {
            switch (State)
            {
                case ReportState.Cancelled:
                    return;
                case ReportState.Done:
                    return;
                case ReportState.Error:
                    break;
                case ReportState.Running:
                    MessageBox.Show("任务正在运行！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                case ReportState.Waiting:
                    break;
            }
            State = ReportState.Cancelled;
        }
    }

    public enum ReportState
    {
        Waiting, Running, Done, Cancelled, Error
    }

    public class CommonResult
    {
        public bool success;
        public string result;

        public CommonResult() { }
        public CommonResult(bool success, string result)
        {
            this.success = success;
            this.result = result;
        }
    }

}
