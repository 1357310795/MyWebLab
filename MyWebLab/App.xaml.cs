using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using Unity3DWPFConnection;
using USTCORi.WebLabClient;
using USTCORi.WebLabClient.BizServiceReference;
using USTCORi.WebLabClient.DataModel;
using USTCORi.WebLabClient.RuningInfo;
using USTCORi.WebLabClient.WLServiceReference;

namespace MyWebLab
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static RunningInfo runningInfo;

        protected override void OnStartup(StartupEventArgs e)
        {
            this.InitInfo(AppDomain.CurrentDomain.BaseDirectory);
            string path = AppDomain.CurrentDomain.BaseDirectory;
            var l = new List<string>();
            l.Add("");
            var args = e.Args;//l.ToArray();
            
            if (args.Length == 0)
            {
                this.ExitApp();
                return;
            }
            //MessageBox.Show(e.Args[0].ToString());
            string[] arr = DecodeArgs(args);
            string diff = arr[0];
            string labname = arr[1];
            string ip = arr[2];
            string port = arr[3];
            string userid = arr[4];
            string userType = "3";
            if (arr.Length > 7)
            {
                userType = arr[7];
            }
            App.runningInfo.Const.SetUserName(userid);

            UpdateConfig(path, ip, port);

            this.InitServiceClient();
            Report findLabTask;
            findLabTask = new Report();
            findLabTask.BizCode = "UstcOri.BLL.BLLLabClent";
            findLabTask.MethodName = "FindByLABID";
            findLabTask.ReturnType = typeof(IList<LABINFO>);
            findLabTask.SetParameter("LabID", labname);
            try
            {
                SvcResponse response = findLabTask.Run();
                if (string.IsNullOrEmpty(response.Message))
                {
                    if (response.Data != null)
                    {
                        IList<LABINFO> lablist = response.Data as IList<LABINFO>;
                        if (lablist.Count > 0)
                        {
                            TheHall hall = new TheHall(lablist[0], path, userid, userType);
                            hall.Begin();
                        }
                        else
                        {
                            MessageBox.Show("未找到相应的实验项目！");
                            this.ExitApp();
                        }
                    }
                    else
                    {
                        MessageBox.Show("获取信息时发生错误！", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        this.ExitApp();
                    }
                }
                else
                {
                    MessageBox.Show(response.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                    this.ExitApp();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                this.ExitApp();
            }
        }

        private string[] DecodeArgs(string[] args)
        {
            string arg = args[0];
            string decode = "";
            try
            {
                string cs = arg.Split(':')[arg.Split(':').Length - 1];
                string s = cs.Substring(2, cs.Length - 3);
                byte[] bytes = Convert.FromBase64String(s);
                decode = Encoding.UTF8.GetString(bytes);
            }
            catch (Exception exe)
            {
                MessageBox.Show("参数解析失败！" + exe.Message);
                this.ExitApp();
            }
            return decode.Split('/');
        }

        private static void UpdateConfig(string path, string ip, string port)
        {
            string configfile = AppDomain.CurrentDomain.FriendlyName;
            XElement config = XElement.Load(path + configfile + ".config");
            //XElement config = XElement.Load(path + "settings" + ".config");
            XElement serviceModel = config.Element("system.serviceModel");
            XElement clientxml = serviceModel.Element("client");
            IEnumerable<XElement> endpoints = clientxml.Elements("endpoint");
            foreach (XElement item in endpoints)
            {
                if (item.Attribute("address").Value.Contains("BizService"))
                {
                    if (port.ToLower() == "false" || port.ToLower() == "undefined")
                    {
                        item.Attribute("address").Value = "http://" + ip + "/BizService.svc";
                    }
                    else
                    {
                        item.Attribute("address").Value = string.Concat(new string[]
                        {
                                "http://",
                                ip,
                                ":",
                                port,
                                "/BizService.svc"
                        });
                    }
                }
                else if (item.Attribute("address").Value.Contains("WLService"))
                {
                    if (port.ToLower() == "false" || port.ToLower() == "undefined")
                    {
                        item.Attribute("address").Value = "http://" + ip + "/WLService.svc";
                    }
                    else
                    {
                        item.Attribute("address").Value = string.Concat(new string[]
                        {
                                "http://",
                                ip,
                                ":",
                                port,
                                "/WLService.svc"
                        });
                    }
                }
                else if (port.ToLower() == "false" || port.ToLower() == "undefined")
                {
                    item.Attribute("address").Value = "http://" + ip + "/FileTransfer.svc";
                }
                else
                {
                    item.Attribute("address").Value = string.Concat(new string[]
                    {
                            "http://",
                            ip,
                            ":",
                            port,
                            "/FileTransfer.svc"
                    });
                }
            }
            config.Save(path + "WebLabClient.exe.config");
        }

        public void ExitApp()
        {
            App.runningInfo.Global.ShutdownLab();
            Unity3DWPF.Instance.ReleaseConnect();
            Environment.Exit(0);
            Application.Current.Shutdown();
        }

        private void InitInfo(string path)
        {
            App.runningInfo = new RunningInfo();
            App.runningInfo.Const.SetShoolUser("No School");
            App.runningInfo.Const.SetVersion("V2.01.0914");
            App.runningInfo.Const.SetDownloadExperimentPath(path + "Download\\");
            App.runningInfo.Const.SetDownloadExperimentXmlPath(path + "Download\\download.xml");
            App.runningInfo.Const.SetDownloadUpdataPath(path + "Download\\Updata\\");
            App.runningInfo.Global.NeedConfirmShutdown = true;
        }

        private void InitServiceClient()
        {
            BizServiceClient bizclient = new BizServiceClient();
            WLServiceClient client = new WLServiceClient();
            App.runningInfo.Const.SetClient(bizclient);
        }

    }
}
