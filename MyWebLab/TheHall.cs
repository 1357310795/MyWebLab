using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Xml.Linq;
using MyWebLab;
using PackageLibrary;
using Unity3DWPFConnection;
using USTCORi.WebLabClient.BizServiceReference;
using USTCORi.WebLabClient.Common;
using USTCORi.WebLabClient.DataModel;
using USTCORi.WebLabClient.FileTransferReference;
using USTCORi.WebLabClient.Views.DownloadWindow;

namespace USTCORi.WebLabClient
{
    public partial class TheHall : Window
    {
        #region Fields
        public double StudentScore;

        private string destFileName = string.Empty;
        public LABINFO lab;
        public string path;
        public string userid;
        public string userType;
        private XElement downXml;
        private string downXmlAdd;

        public Report SetLabStartTask;
        public Report SetLabEndTask;
        public Report UserCountControlTask;
        public Report JFTask;
        public Report UnZipFileTask;
        public Report InsertLabRecordTask;

        public int RunningID;

        public LABTIMERECORD labTime;

        public int ucID = 0;

        private Package pack;

        private XElement paperElement;
        private XElement xeXml = null;
        private XElement Currentpara = null;

        private string LabMsg = "";
        private string CurrentStep = "";
        public string StudentID = "";
        public string StudentName = "";
        public string LabName = "";
        public string LabTypeName = "";
        public string FinishPercent = "";
        
        public string ZipFilePath;
        public string UpDicPath;

        private FileTransferClient FileTransferClient;
        private FileInfoManager file;

        public IList<LabRecord> recordList = new List<LabRecord>();

        public new string Name;
        public static string temppath = Environment.GetEnvironmentVariable("TEMP");
        #endregion

        #region Constructors
        public TheHall(LABINFO lab, string path, string userid, string userType)
        {
            this.InitializeComponent();
            this.lab = lab;
            this.path = path;
            this.userid = userid;
            this.userType = userType;
            this.FileTransferClient = WcfServiceClientFactory<FileTransferClient, IFileTransfer>.CreateServiceClient();
        }
        #endregion

        #region Common
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(new HwndSourceHook(this.WndProc));
            if (!Unity3DWPF.Instance.IsCreated)
            {
                Unity3DWPF.Instance.CreatePipe();
            }
            Unity3DWPF.Instance.StartConnect();
            Unity3DWPF.Instance.MessageReceived += this.Instance_MessageReceived;
        }
        public void ExitApp()
        {
            UserCountControl ucControl = new UserCountControl();
            ucControl.ID = this.ucID;
            ucControl.OperType = 1;
            DoReportCloseApp(ucControl);
            ShutdownAllProcess();
        }
        private static void ShutdownAllProcess()
        {
            App.runningInfo.Global.ShutdownLab();
            Unity3DWPF.Instance.ReleaseConnect();
            Environment.Exit(0);
            Application.Current.Shutdown();
        }
        #endregion

        #region Start
        public void Begin()
        {
            if (this.UserCountControlTask == null)
            {
                this.UserCountControlTask = new Report();
                this.UserCountControlTask.BizCode = "UstcOri.BLL.BLLUserCountControl";
                this.UserCountControlTask.MethodName = "ucControlMethod";
                this.UserCountControlTask.ReturnType = typeof(int);
            }
            UserCountControl ucControl = new UserCountControl();
            ucControl.UserID = this.userid;
            ucControl.ServiceID = "开始实验";
            ucControl.OperType = 0;
            ucControl.LabID = this.lab.LabID;
            this.UserCountControlTask.SetParameter("ucControl", ucControl);
            try
            {
                SvcResponse response = this.UserCountControlTask.Run();
                if (string.IsNullOrEmpty(response.Message))
                {
                    if (response.Data != null)
                    {
                        this.ucID = (int)response.Data;
                        if (this.ucID > 0)
                        {
                            this.CheckIsDownloaded();
                        }
                        else
                        {
                            MessageBox.Show("服务器正忙，请稍后重试！", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            this.ExitApp();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器连接出错，请稍后重试！", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.ExitApp();
            }
        }

        public void CheckIsDownloaded()
        {
            this.downXmlAdd = this.path + "Download";
            if (!Directory.Exists(this.downXmlAdd))
            {
                Directory.CreateDirectory(this.downXmlAdd);
            }
            if (!File.Exists(this.downXmlAdd + "\\download.xml"))
            {
                this.downXml = new XElement("root");
                this.downXml.Save(this.downXmlAdd + "\\download.xml");
            }
            else
            {
                this.downXml = XElement.Load(this.downXmlAdd + "\\download.xml");
            }
            IEnumerable<XElement> experiments = from n in this.downXml.Elements("Experiment")
                                                where (int)n.Attribute("ID") == this.lab.LabID
                                                select n;
            bool isDown;
            if (experiments.Count() != 0)
            {
                if (((DateTime)experiments.First().Attribute("UpTime")).Equals(this.lab.UpTime))
                {
                    string labpath = this.path + "DownLoad\\" + this.lab.LABTYPENAME + "\\" + this.lab.LABNAME + ".lab";
                    FileInfo file = new FileInfo(labpath);
                    isDown = file.Exists;
                }
                else
                {
                    isDown = false;
                }
            }
            else
            {
                isDown = false;
            }
            this.StartLab(isDown);
        }

        public void StartLab(bool isDown)
        {
            if (!isDown)
            {
                DownloadLab();
                return;
            }

            if (CheckIsRunning())
            {
                return;
            }

            Thread newWindowThread = new Thread(new ThreadStart(this.CreateCounterWindowThread));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.Start();


            this.labTime = new LABTIMERECORD();
            this.labTime.Mark = "1";
            this.labTime.USERID = App.runningInfo.Const.UserName;
            this.labTime.LABID = this.lab.LabID;
            this.LabName = this.lab.LABNAME;
            this.LabTypeName = this.lab.LABTYPENAME;

            DoReportLabStart();
            if (!DoReportJF())
                return;
            
            try
            {
                string labpath = this.path + "DownLoad\\" + this.lab.LABTYPENAME + "\\" + this.lab.LABNAME + ".lab";
                FileInfo file = new FileInfo(labpath);
                if (!file.Exists)
                {
                    MessageBox.Show("文件未找到", "ERROR", MessageBoxButton.OK, MessageBoxImage.Hand);
                    this.ExitApp();
                    return;
                }
                this.pack = new Package(file);
                this.pack.SetDirectoryToUnPack(temppath + "\\lab");
                this.pack.Run();

                //CreateUSERINFOBIN(TempFolder);

                Process ap = new Process();
                ap.StartInfo.FileName = temppath + "\\lab\\Main.exe";
                ap.StartInfo.WorkingDirectory = temppath + "\\lab\\";
                DateTime currentDateTime = DateTime.Now;
                string key = "USTCOri" + this.userid + this.lab.LABNAME + (Convert.ToDateTime(currentDateTime.ToString()) - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds.ToString();
                string md5Key = "";
                MD5 md5 = MD5.Create();
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(key));
                for (int i = 0; i < s.Length; i++)
                {
                    md5Key += s[i].ToString("X");
                }
                if (this.lab.INTRODUTION == "1")
                {
                    ap.StartInfo.Arguments = string.Concat(new string[]
                    {
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(this.userid)),
                        "|",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(this.lab.LabID.ToString())),
                        "|",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(this.lab.LABNAME)),
                        "|",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(this.RunningID.ToString())),
                        "|",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(md5Key)),
                        "|",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(currentDateTime.ToString())),
                        "|",
                        Convert.ToBase64String(Encoding.UTF8.GetBytes(App.runningInfo.Const.ServerIP))
                    });
                }
                ap.Exited += this.ap_Exited;
                ap.EnableRaisingEvents = true;
                ap.Start();
                App.runningInfo.Global.RunningExperiment = this.lab;
                App.runningInfo.Global.RunningProcess = ap;

                //DistroyFile(TempFonder, ap);
            }
            catch (Exception ex1)
            {
                MessageBox.Show("文件打开出错" + ex1.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Hand);
                //try
                //{
                //    File.Delete(temppath + "\\lab\\Main.exe");
                //    Directory.Delete(temppath + "\\lab", true);
                //}
                //catch (Exception ex2)
                //{
                //    this.ExitApp();
                //    return;
                //}
                this.ExitApp();
            }
        }

        private void CreateUSERINFOBIN(string TempFolder)
        {
            Directory.CreateDirectory(TempFolder + "\\Start");
            try
            {
                File.Copy(this.path + "Resources\\USERINFO.BIN", TempFolder + "\\Start\\USERINFO.BIN", true);
            }
            catch (Exception)
            {
                MessageBox.Show("未找到合法用户文件！");
                this.ExitApp();
            }
        }

        private void DistroyFile(string TempFonder, Process ap)
        {
            Thread.Sleep(3000);
            if (File.Exists(ap.StartInfo.FileName))
            {
                this.destFileName = TempFonder + "\\TS_" + DateTime.Now.Ticks.ToString() + ".tmp";
                File.Move(ap.StartInfo.FileName, this.destFileName);
                FileStream fs = new FileStream(this.destFileName, FileMode.Open, FileAccess.Read);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Random rnd = new Random();
                for (int i = 260; i < 10240; i++)
                {
                    bytes[i] = Convert.ToByte(rnd.Next(0, 255));
                }
                fs = new FileStream(ap.StartInfo.FileName, FileMode.Create, FileAccess.ReadWrite);
                fs.Write(bytes, 0, bytes.Length);
                fs.Flush();
                fs.Close();
            }
        }

        private bool CheckIsRunning()
        {
            Process[] myProcesses = Process.GetProcesses();
            foreach (Process myProcess in myProcesses)
            {
                if (myProcess.ProcessName.Trim() == "Main" || myProcess.ProcessName.Trim() == "wowexec")
                {
                    MessageBox.Show("已经有一个实验程序正在运行,如要开始新实验,请先结束当前实验", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    this.ExitApp();
                    return true;
                }
            }
            Process running = App.runningInfo.Global.RunningProcess;
            if (running != null && !running.HasExited)
            {
                MessageBox.Show("已经有一个实验程序正在运行,如要开始新实验,请先结束当前实验", "警告", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.ExitApp();
                return true;
            }
            return false;
        }

        private void DownloadLab()
        {
            try
            {
                DownloadWindow download = new DownloadWindow();
                download.Topmost = true;
                download.SetExperiment(this.lab, this, this.path);
                download.ShowDialog();
                if (download.DialogResult == true)
                {
                    this.StartLab(true);
                }
                else
                {
                    this.ExitApp();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("下载实验失败", "ERROR", MessageBoxButton.OK, MessageBoxImage.Hand);
                this.ExitApp();
            }
        }

        private void CreateCounterWindowThread()
        {
            ShowWindow show = new ShowWindow();
            show.Topmost = true;
            show.start();
            show.Show();
            Dispatcher.Run();
        }
        #endregion

        #region After
        private void ap_Exited(object sender, EventArgs e)
        {
            labTime = new LABTIMERECORD();
            labTime.Mark = "2";
            labTime.LABID = this.lab.LabID;
            labTime.USERID = App.runningInfo.Const.UserName;
            labTime.ID = this.RunningID;

            string ReviewXmlPath = "", OpConfigPath = "";
            bool flag = false;
            if (File.Exists(temppath + "\\lab\\SimExpConfig\\SimExpConfig.xml"))
            {
                ReviewXmlPath = temppath + "\\lab\\SimExpConfig\\SimExpConfig.xml";
                OpConfigPath = temppath + "\\lab\\SimExpConfig\\OpConfig.xml";
                flag = true;
            }
            else if (File.Exists(temppath + "\\lab\\SimExpConfig\\ExamConfig.xml"))
            {
                ReviewXmlPath = temppath + "\\lab\\SimExpConfig\\ExamConfig.xml";
                OpConfigPath = temppath + "\\lab\\SimExpConfig\\OpConfig.xml";
                flag = true;
            }
            else if (File.Exists(temppath + "\\lab\\Main_Data\\StreamingAssets\\ExamConfig.xml"))
            {
                ReviewXmlPath = temppath + "\\lab\\Main_Data\\StreamingAssets\\ExamConfig.xml";
                OpConfigPath = temppath + "\\lab\\Main_Data\\StreamingAssets\\OpConfig.xml";
                flag = true;
            }
            else
            {
                labTime.LabDateUrl = "";
                flag = false;
            }

            if (flag)
            {
                this.ReviewXml(ReviewXmlPath);
                this.paperElement.Save(OpConfigPath);
                FileInfoManager filem = this.GetStreamOfContent(OpConfigPath);
                string path = "Upload\\LabDate\\" + this.lab.LABNAME;
                DateTime time = DateTime.Now;
                string date = string.Concat(new string[]
                {
                    time.Year.ToString(),
                    time.Month.ToString(),
                    time.Day.ToString(),
                    time.Hour.ToString(),
                    time.Minute.ToString(),
                    time.Second.ToString()
                });
                string filename = App.runningInfo.Const.UserName + "_" + date + ".xml";
                this.UploadDateXml(filem, path, filename);
                labTime.LabDateUrl = path + "\\" + filename;
                labTime.Score = this.StudentScore;
            }

            DoReportLabEnd();
            //try
            //{
            //    File.Delete(temppath + "\\lab\\Main.exe");
            //    Directory.Delete(temppath + "\\lab", true);
            //    File.Delete(this.destFileName);
            //}
            //catch (Exception ex)
            //{
            //    return;
            //}
            this.ExitApp();
        }

        public void ReviewXml(string path)
        {
            try
            {
                this.paperElement = XElement.Load(path);
                IEnumerable<XElement> queslist = this.paperElement.Element("Content").Elements("Question");
                foreach (XElement item in queslist)
                {
                    XElement checkPointElement = item.Element("CheckPoint");
                    IEnumerable<XElement> list_TestTarget = checkPointElement.Elements("TestTarget");
                    string questionGainScore = "0";
                    double questionScore = 0.0;
                    foreach (XElement testTarget in list_TestTarget)
                    {
                        string testTargetGainScore = "0";
                        this.GetPerTestTarget(testTarget, ref testTargetGainScore);
                        if (testTargetGainScore == "LostWholeQuestion")
                        {
                            questionGainScore = "LostWholeQuestion";
                        }
                        else if (testTargetGainScore == "LostWholeCheckPoint")
                        {
                            questionScore += 0.0;
                        }
                        else
                        {
                            questionScore += double.Parse(testTargetGainScore);
                        }
                    }
                    if (questionGainScore != "LostWholeQuestion")
                    {
                        questionGainScore = questionScore.ToString();
                        this.StudentScore += questionScore;
                    }
                    else
                    {
                        this.StudentScore += 0.0;
                    }
                    string ETotal = item.Element("Score").Element("Total").Value;
                    if (string.IsNullOrEmpty(ETotal))
                    {
                        questionGainScore = "0";
                        this.StudentScore += 0.0;
                    }
                    else
                    {
                        double tscore = double.Parse(ETotal);
                        if (Math.Abs(tscore) < Math.Pow(10.0, -6.0))
                        {
                            questionGainScore = "0";
                            this.StudentScore += 0.0;
                        }
                    }
                    item.Element("Score").Element("RealScore").SetValue(questionGainScore);
                }
            }
            catch (Exception ee)
            {
            }
        }

        private void GetPerTestTarget(XElement testTarget, ref string testTargetGainScore)
        {
            XElement paraGroup = testTarget.Element("Group");
            IEnumerable<XElement> list_Para = paraGroup.Elements("Para");
            double testTargetScore = 0.0;
            foreach (XElement para in list_Para)
            {
                string paraGainScore = "0";
                this.GetPerPara(para, ref paraGainScore);
                if (paraGainScore == "LostWholeCheckPoint" || paraGainScore == "LostWholeQuestion")
                {
                    testTargetGainScore = paraGainScore;
                }
                else
                {
                    testTargetScore += double.Parse(paraGainScore);
                }
            }
            if (testTargetGainScore != "LostWholeCheckPoint" && testTargetGainScore != "LostWholeQuestion")
            {
                testTargetGainScore = testTargetScore.ToString();
            }
            XElement RealScore = testTarget.Element("RealScore");
            if (RealScore != null)
            {
                RealScore.SetValue(testTargetGainScore);
            }
            else
            {
                testTarget.Add(new XElement("RealScore", testTargetGainScore));
            }
        }

        private void GetPerPara(XElement para, ref string paraGainScore)
        {
            string paraShowName = para.Element("Script").Value.Replace("&lt;", "<").Replace("&gt;", ">");
            string paraVarType = para.Element("VarType").Value;
            string RealResult = para.Element("RealResult").Value;
            string StdResult = para.Element("StdResult").Value;
            if (!(paraVarType.Trim().ToUpper() == "ANSWER"))
            {
                if (paraVarType.Trim().ToUpper() == "TLIST")//TLIST里面有subPara
                {
                    try
                    {
                        string[,] realRets = this.DoTListResult(RealResult);
                        string[,] stdRets = this.DoTListResult(StdResult);
                        IEnumerable<XElement> list_SubPara = para.Elements("Para");
                        double paraTotalScore = 0.0;
                        int i = 0;
                        foreach (XElement subPara in list_SubPara)
                        {
                            string subVarType = subPara.Element("VarType").Value;
                            int start = stdRets.GetLowerBound(1);
                            int end = stdRets.GetUpperBound(1);
                            for (int j = start + 1; j <= end; j++)
                            {
                                string RealResultToShow = "";
                                string subParaGainScore = this.GainScore(realRets[i, j], stdRets[i, j], subVarType, subPara, ref RealResultToShow);
                                if (subParaGainScore == "LostWholeQuestion" || subParaGainScore == "LostWholeCheckPoint")
                                {
                                    paraGainScore = subParaGainScore;
                                    break;
                                }
                                paraTotalScore += double.Parse(subParaGainScore);
                            }
                            if (paraGainScore == "LostWholeQuestion" || paraGainScore == "LostWholeCheckPoint")
                            {
                                break;
                            }
                            i++;
                        }
                        if (paraGainScore != "LostWholeQuestion" && paraGainScore != "LostWholeCheckPoint")
                        {
                            paraGainScore = paraTotalScore.ToString();
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    string RealResultToShow = "";
                    RealResultToShow = RealResult;
                    paraGainScore = this.GainScore(RealResult, StdResult, paraVarType, para, ref RealResultToShow);
                }
            }
            string ShowparaGainScore = paraGainScore;
            XElement ParaAdd = para.Element("ParaAdd");
            if (ParaAdd != null)
            {
                string paraTotalScore2 = para.Element("TotalScore").Value;
                string paraAddName = ParaAdd.Element("Name").Value;
                string paraAddValue = ParaAdd.Element("ParaChangeValue").Value;
                if (paraAddValue == "")
                {
                    paraAddValue = "0";
                }
                string paraAddVarType = ParaAdd.Element("VarType").Value.Trim().ToUpper();
                string ShowPart = ParaAdd.Element("ShowPart").Value;
                string ShowWrong = ParaAdd.Element("ShowWrong").Value;
                if (!(paraGainScore == "LostWholeQuestion") && !(paraGainScore == "LostWholeCheckPoint"))
                {
                    if (paraAddVarType == "DOUBLE")
                    {
                        double NewparaAddValue = double.Parse(paraAddValue);
                        double TempMax = double.Parse(paraTotalScore2) * NewparaAddValue;
                        double Temp = double.Parse(paraGainScore);
                        if (Temp < 1E-08)
                        {
                            ShowparaGainScore = paraGainScore;
                        }
                        else
                        {
                            if (Temp > TempMax)
                            {
                                Temp = TempMax;
                            }
                            paraGainScore = Temp.ToString("F1");
                            if (double.Parse(paraAddValue) < 1E-07)
                            {
                                ShowparaGainScore = paraGainScore + "   ,   " + ShowWrong;
                            }
                            else if (double.Parse(paraAddValue) < 0.999999 && double.Parse(paraAddValue) > 1E-07)
                            {
                                ShowparaGainScore = paraGainScore + "   ,   " + ShowPart;
                            }
                        }
                    }
                }
            }
            XElement RealScore = para.Element("RealScore");
            if (RealScore != null)
            {
                RealScore.SetValue(ShowparaGainScore);
            }
            else
            {
                para.Add(new XElement("RealScore", ShowparaGainScore));
            }
        }

        private string GainScore(string StudentAnswerStr, string StdAnswerStr, string varType, XElement paraElement, ref string RealResultToShow)
        {
            try
            {
                XElement judgeRuleElement = paraElement.Element("JudgeRule");
                if (judgeRuleElement == null)
                {
                    return "0";
                }
                varType = varType.Trim().ToLower();
                if (varType == "bool" || varType == "string")
                {
                    XElement rightRuleElement = judgeRuleElement.Element("RightRule");
                    XElement wrongRuleElement = judgeRuleElement.Element("WrongRule");
                    bool isEuqal = StudentAnswerStr.Trim().ToLower() == StdAnswerStr.Trim().ToLower();
                    string gainPoint;
                    if (isEuqal)
                    {
                        gainPoint = rightRuleElement.Element("Score").Value;
                        RealResultToShow = this.AddStudentAnswerShow(paraElement, rightRuleElement);
                    }
                    else
                    {
                        gainPoint = wrongRuleElement.Element("Score").Value;
                        RealResultToShow = this.AddStudentAnswerShow(paraElement, wrongRuleElement);
                    }
                    if (varType == "string" && RealResultToShow == "")
                    {
                        RealResultToShow = StudentAnswerStr;
                    }
                    return gainPoint;
                }
                if (varType == "int" || varType == "double")
                {
                    if (string.IsNullOrEmpty(StudentAnswerStr) || string.IsNullOrEmpty(StdAnswerStr))
                    {
                        return "0";
                    }
                    List<XElement> allMatchRules = new List<XElement>();
                    double StuAnswerDemical = double.Parse(StudentAnswerStr);
                    double StdAnswerDemical = double.Parse(StdAnswerStr);
                    IEnumerable<XElement> list_ruleElement = judgeRuleElement.Elements("Rule");
                    if (list_ruleElement == null)
                    {
                        return "0";
                    }
                    foreach (XElement ruleElement in list_ruleElement)
                    {
                        if (CheckRule(StuAnswerDemical, StdAnswerDemical, ruleElement))
                            allMatchRules.Add(ruleElement);
                    }
                    if (allMatchRules.Count == 0)
                    {
                        return "0";
                    }
                    string maxScore = "";
                    XElement outRule;
                    try
                    {
                        maxScore = this.CalcMaxScore(allMatchRules, out outRule);
                        XElement TotalS = paraElement.Element("TotalScore");
                        if (TotalS != null && TotalS.Value == "0")
                        {
                            maxScore = Math.Pow(10.0, -5.0).ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        maxScore = this.CalcMaxScore(allMatchRules, out outRule);
                    }
                    RealResultToShow = this.AddStudentAnswerShow(paraElement, outRule);
                    if (RealResultToShow == "")
                    {
                        RealResultToShow = StuAnswerDemical.ToString();
                    }
                    return maxScore;
                }
            }
            catch
            {
                return "0";
            }
            return "0";
        }

        private bool CheckRule(double StuAnswerDemical, double StdAnswerDemical, XElement ruleElement)
        {
            return true;
            double stdValue = double.Parse(ruleElement.Element("StdValue").Value);
            if (stdValue == 0.0)
            {
                stdValue = 1E-11;
            }
            XElement rangeElement = ruleElement.Element("Range");
            double min = double.Parse(rangeElement.Element("Min").Value);
            double max = double.Parse(rangeElement.Element("Max").Value);
            double diffStd = StuAnswerDemical - stdValue * StdAnswerDemical;
            string MatchType = rangeElement.Element("Type").Value;
            double MinValue = 1E-07;
            if (MatchType.Trim() == "绝对值")//绝对误差
            {
                if (diffStd >= min - MinValue && diffStd <= max + MinValue)
                {
                    return true;
                }
            }
            else//相对误差
            {
                bool tempflag = false;
                if (Math.Abs(StdAnswerDemical) <= MinValue)
                {
                    StdAnswerDemical = 1E-11;
                    tempflag = true;
                }
                double realRation = StuAnswerDemical / StdAnswerDemical;
                double diffPercent = (realRation - stdValue) / stdValue;
                if (diffPercent >= min - MinValue && diffPercent <= max + MinValue)
                {
                    return true;
                }
                else if (tempflag && Math.Abs(StuAnswerDemical) <= MinValue)
                {
                    return true;
                }
            }

            return false;
        }

        private string AddStudentAnswerShow(XElement paraElement, XElement ruleElement)
        {
            string result;
            if (paraElement == null)
            {
                result = "";
            }
            else if (ruleElement == null)
            {
                result = "";
            }
            else
            {
                XElement StdResultShowInfoElement = paraElement.Element("StdResultShowInfo");
                if (StdResultShowInfoElement == null || StdResultShowInfoElement.Value == "原值")
                {
                    result = "";
                }
                else
                {
                    XElement RuleShowInfoElement = ruleElement.Element("RuleShowInfo");
                    if (RuleShowInfoElement == null)
                    {
                        result = "";
                    }
                    else
                    {
                        XElement srs = paraElement.Element("StudentResultShowInfo");
                        if (srs == null)
                        {
                            paraElement.Add(new XElement("StudentResultShowInfo", RuleShowInfoElement.Value));
                        }
                        else
                        {
                            srs.Value = RuleShowInfoElement.Value;
                        }
                        result = RuleShowInfoElement.Value;
                    }
                }
            }
            return result;
        }

        private string CalcMaxScore(List<XElement> allMatchRules, out XElement outRule)
        {
            IEnumerable<XElement> rules = from r in allMatchRules
                                          where r.Element("Score").Value == "LostWholeQuestion"
                                          select r;
            IEnumerable<XElement> rules2 = from r in allMatchRules
                                           where r.Element("Score").Value == "LostWholeCheckPoint"
                                           select r;
            IEnumerable<XElement> rules3 = from r in allMatchRules
                                           where r.Element("Score").Value != "LostWholeQuestion" && r.Element("Score").Value != "LostWholeCheckPoint"
                                           select r;
            string result;
            if (rules3 != null && rules3.Count<XElement>() != 0)
            {
                double maxScore = (from r in rules3
                                   select double.Parse(r.Element("Score").Value)).Max();
                outRule = (from r in rules3
                           where double.Parse(r.Element("Score").Value) == maxScore
                           select r).Single<XElement>();
                result = Math.Round(maxScore, 2).ToString();
            }
            else if (rules2 != null && rules2.Count<XElement>() != 0)
            {
                outRule = rules2.ToList<XElement>()[0];
                result = "LostWholeCheckPoint";
            }
            else if (rules != null && rules.Count<XElement>() != 0)
            {
                outRule = rules.ToList<XElement>()[0];
                result = "LostWholeQuestion";
            }
            else
            {
                outRule = null;
                result = "0";
            }
            return result;
        }

        private string[,] DoTListResult(string result)
        {
            string[] Rets = result.Split(new char[]
            {
                ';'
            });
            string[][] arr = new string[Rets.Length][];
            for (int i = 0; i < Rets.Length; i++)
            {
                if (Rets[i].Contains("(") && Rets[i].Contains(")"))
                {
                    string Ret = Rets[i].Substring(1, Rets[i].Length - 2);
                    arr[i] = Ret.Split(new char[]
                    {
                        ','
                    });
                }
            }
            string[,] _arr = new string[arr[0].Length, arr.Length];
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr[i].Length; j++)
                {
                    _arr[j, i] = arr[i][j];
                }
            }
            return _arr;
        }
        #endregion

        #region Report
        private bool DoReportJF()
        {
            string JFResult = "计费过程出现异常";
            if (this.JFTask == null)
            {
                this.JFTask = new Report();
                this.JFTask.BizCode = "UstcOri.BLL.BLLLabClent";
                this.JFTask.MethodName = "JFIsAccess";
                this.JFTask.ReturnType = typeof(string);
            }
            this.JFTask.SetParameter("UserID", App.runningInfo.Const.UserName);
            this.JFTask.SetParameter("LabName", this.lab.LABNAME);
            this.JFTask.SetParameter("SysID", 1);
            try
            {
                SvcResponse response = this.JFTask.Run();
                if (string.IsNullOrEmpty(response.Message))
                {
                    if (response.Data != null)
                    {
                        JFResult = (string)response.Data;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            if (!string.IsNullOrEmpty(JFResult))
            {
                MessageBox.Show("计费失败：" + JFResult, "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.ExitApp();
                return false;
            }
            return true;
        }

        private void DoReportLabStart()
        {
            if (this.SetLabStartTask == null)
            {
                this.SetLabStartTask = new Report();
                this.SetLabStartTask.BizCode = "UstcOri.BLL.BLLLabClent";
                this.SetLabStartTask.MethodName = "SetLabTimeRecord";
                this.SetLabStartTask.ReturnType = typeof(int);
            }
            this.SetLabStartTask.SetParameter("labTime", this.labTime);
            try
            {
                SvcResponse response = this.SetLabStartTask.Run();
                if (string.IsNullOrEmpty(response.Message))
                {
                    if (response.Data != null)
                    {
                        this.RunningID = (int)response.Data;
                        if (this.RunningID <= 0)
                        {
                            MessageBox.Show("服务器连接出错，此次操作记录无效", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            this.ExitApp();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器连接出错，此次操作记录无效", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.ExitApp();
            }
        }

        private void DoReportLabEnd()
        {
            if (this.SetLabEndTask == null)
            {
                this.SetLabEndTask = new Report();
                this.SetLabEndTask.BizCode = "UstcOri.BLL.BLLLabClent";
                this.SetLabEndTask.MethodName = "SetLabTimeRecord";
                this.SetLabEndTask.ReturnType = typeof(int);
            }
            this.SetLabEndTask.SetParameter("labTime", labTime);
            try
            {
                SvcResponse response = this.SetLabEndTask.Run();
                if (string.IsNullOrEmpty(response.Message))
                {
                    if (response.Data != null)
                    {
                        this.RunningID = (int)response.Data;
                        if (this.RunningID <= 0)
                        {
                            MessageBox.Show("服务器链接出错，此次操作记录无效", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器连接出错，此次操作记录无效", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DoReportCloseApp(UserCountControl ucControl)
        {
            if (this.UserCountControlTask == null)
            {
                this.UserCountControlTask = new Report();
                this.UserCountControlTask.BizCode = "UstcOri.BLL.BLLUserCountControl";
                this.UserCountControlTask.MethodName = "ucControlMethod";
                this.UserCountControlTask.ReturnType = typeof(int);
            }
            this.UserCountControlTask.SetParameter("ucControl", ucControl);
            try
            {
                SvcResponse response = this.UserCountControlTask.Run();
                if (string.IsNullOrEmpty(response.Message))
                {
                    if (response.Data != null)
                    {
                        int ucid = (int)response.Data;
                    }
                }
                else
                {
                    MessageBox.Show(response.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器连接出错，请稍后重试！", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DoReportUnzipFile(string newFile)
        {
            if (this.UnZipFileTask == null)
            {
                this.UnZipFileTask = new Report();
                this.UnZipFileTask.BizCode = "UstcOri.BLL.BLLLabClent";
                this.UnZipFileTask.MethodName = "UnZipFile";
                this.UnZipFileTask.ReturnType = typeof(string);
            }
            this.UnZipFileTask.SetParameter("zipFilePath", newFile);
            try
            {
                SvcResponse response = this.UnZipFileTask.Run();
                if (string.IsNullOrEmpty(response.Message))
                {
                    if (response.Data == null)
                    {
                        this.UpdateDate();
                    }
                    else
                    {
                        MessageBox.Show("操作步骤保存失败", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("操作步骤保存失败", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void DoReportInsertLabRecord()
        {
            if (this.InsertLabRecordTask == null)
            {
                this.InsertLabRecordTask = new Report();
                this.InsertLabRecordTask.BizCode = "UstcOri.BLL.BLLLabClent";
                this.InsertLabRecordTask.MethodName = "InsertLabRecord";
                this.InsertLabRecordTask.ReturnType = typeof(int);
            }
            this.InsertLabRecordTask.SetParameter("recordList", this.recordList);
            try
            {
                SvcResponse response = this.InsertLabRecordTask.Run();
                if (string.IsNullOrEmpty(response.Message))
                {
                    if (response.Data != null)
                    {
                        int result = (int)response.Data;
                        if (result <= 0)
                        {
                            MessageBox.Show("服务器链接出错，数据更新失败", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("服务器连接出错，数据更新失败", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }
        #endregion

        #region Window Message & Timer
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 74)
            {
                string[] labmsglist = ((COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(COPYDATASTRUCT))).lpData.Split(new char[]
                {
                    '|'
                });
                string labpath = temppath + "\\lab\\";
                string loadxmlpath = labpath + labmsglist[1];
                string imagedicth = labpath + labmsglist[2];
                this.ZipFilePath = imagedicth;
                this.UpDicPath = imagedicth;
                this.StudentID = App.runningInfo.Const.UserName;
                if (labmsglist[0] == "USTCORI_SUBMIT")
                {
                    UploadSteps(loadxmlpath);
                }
            }
            return IntPtr.Zero;
        }

        private void Instance_MessageReceived(string msg)
        {
            HallLetter hallletter = HallLetter.DeCodeMessage(msg);
            if (hallletter == null)
                return;
            string key = hallletter.Key;
            if (key == null)
                return;
            if (key == "USTCORI_SHOWSELF")
            {
                this.LabMsg = hallletter.Value;
                if (!string.IsNullOrEmpty(this.LabMsg))
                {
                    string[] labmsglist = this.LabMsg.Split('|');
                    string labpath = temppath + "\\lab\\";
                    string loadxmlpath = labpath + labmsglist[1];
                    string imagedicth = labpath + "Main_Data\\StreamingAssets\\" + labmsglist[2];
                    this.ZipFilePath = imagedicth;
                    this.UpDicPath = imagedicth;
                    this.StudentID = App.runningInfo.Const.UserName;
                    if (labmsglist[0] == "USTCORI_SUBMIT")
                    {
                        UploadSteps(loadxmlpath);
                    }
                    HallLetter msgsend = new HallLetter("USTCORI_SHUTDOWNEXP", "USTCORI_SAVED");
                    if (Unity3DWPF.Instance.IsCreated && Unity3DWPF.Instance.IsConnected)
                    {
                        Unity3DWPF.Instance.SendMessage(HallLetter.EnCodeMessage(msgsend));
                    }
                    base.Dispatcher.Invoke(new Action(delegate ()
                    {
                        DispatcherTimer closetimer = new DispatcherTimer
                        {
                            Interval = new TimeSpan(0, 0, 0, 5)
                        };
                        closetimer.Tick += Closetimer1_Tick;
                        closetimer.Start();
                    }), new object[0]);
                }
            }
            else if (key == "SAVE_DATA")
            {
                this.LabMsg = hallletter.Value;
                if (!string.IsNullOrEmpty(this.LabMsg))
                {
                    string[] labmsglist = this.LabMsg.Split(new char[]
                    {
                                    '|'
                    });
                    string labpath = temppath + "\\lab\\";
                    string loadxmlpath = labpath + labmsglist[1];
                    string imagedicth = labpath + "Main_Data\\StreamingAssets\\" + labmsglist[2];
                    this.ZipFilePath = imagedicth;
                    this.UpDicPath = imagedicth;
                    this.StudentID = App.runningInfo.Const.UserName;
                    if (labmsglist[0] == "USTCORI_SUBMIT")
                    {
                        UploadSteps(loadxmlpath);
                    }
                }
            }
            else if (key == "USTCORI_INITCOMPLETE")
            {
                Unity3DWPF.Instance.StartConnect();
                base.Dispatcher.Invoke(new Action(delegate ()
                {
                    DispatcherTimer INITCOMPLETEtimer = new DispatcherTimer
                    {
                        Interval = new TimeSpan(0, 0, 0, 1)
                    };
                    INITCOMPLETEtimer.Tick += this.InitCompleteTimer_Tick;
                    INITCOMPLETEtimer.Start();
                }), new object[0]);
            }
            else if (key == "USTCORI_SHUTDOWNEXP")
            {
                HallLetter msgsend = new HallLetter("USTCORI_SHUTDOWNEXP", "USTCORI_SHUTDOWNEXP");
                if (Unity3DWPF.Instance.IsCreated && Unity3DWPF.Instance.IsConnected)
                {
                    Unity3DWPF.Instance.SendMessage(HallLetter.EnCodeMessage(msgsend));
                }
                base.Dispatcher.Invoke(new Action(delegate ()
                {
                    DispatcherTimer closetimer = new DispatcherTimer
                    {
                        Interval = new TimeSpan(0, 0, 0, 5)
                    };
                    closetimer.Tick += this.closetimer_Tick;
                    closetimer.Start();
                }), new object[0]);
            }
        }

        private void Closetimer1_Tick(object sender, EventArgs e)
        {
            var closetimer = (DispatcherTimer)sender;
            if (closetimer != null) closetimer.Stop();
            ap_Exited(null, new EventArgs());
        }

        public void InitCompleteTimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            HallLetter msgsend = new HallLetter("USTCORI_USERTYPE", this.userType);
            if (Unity3DWPF.Instance.IsCreated && Unity3DWPF.Instance.IsConnected)
            {
                Unity3DWPF.Instance.SendMessage(HallLetter.EnCodeMessage(msgsend));
            }
        }

        private void closetimer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            Process[] myProcesses = Process.GetProcesses();
            foreach (Process myProcess in myProcesses)
            {
                if (myProcess.ProcessName.Trim() == "Main")
                {
                    myProcess.Kill();
                }
            }
        }
        #endregion

        #region Upload
        private FileInfoManager GetStreamOfContent(string writePath)
        {
            FileInfoManager file = new FileInfoManager();
            FileInfo fileIn = new FileInfo(writePath);
            try
            {
                if (!fileIn.Exists)
                {
                    MessageBox.Show("要上传的文件不存在！");
                }
                else
                {
                    Stream stream = File.OpenRead(writePath);
                    file.Size = (double)(stream.Length / 1024L);
                    file.Contents = new List<byte[]>();
                    while (stream.Position > -1L && stream.Position < stream.Length)
                    {
                        int block;
                        if (stream.Length - stream.Position >= 1000000L)
                        {
                            block = 1000000;
                        }
                        else
                        {
                            block = (int)(stream.Length - stream.Position);
                        }
                        byte[] fileBytes = new byte[block];
                        stream.Read(fileBytes, 0, block);
                        file.Contents.Add(fileBytes);
                    }
                    stream.Close();
                    file.Sent = (double)(file.Contents[0].Length / 1024);
                    file.Name = fileIn.Name;
                }
            }
            catch
            {
                MessageBox.Show("上传文件失败！");
            }
            return file;
        }

        public void UploadDateXml(FileInfoManager file, string targetpath, string filename)
        {
            if (file.Size >= 0.0)
            {
                for (int i = 0; i < file.Contents.Count; i++)
                {
                    if (i > 0)
                    {
                        this.FileTransferClient.UploadFile(filename, targetpath, file.Contents[i], true);
                    }
                    else
                    {
                        this.FileTransferClient.UploadFile(filename, targetpath, file.Contents[i], false);
                    }
                }
            }
        }

        private void UploadSteps(string loadxmlpath)
        {
            this.xeXml = XElement.Load(loadxmlpath);
            ZipHelp.ZipFile(this.UpDicPath, this.ZipFilePath + ".zip");
            this.file = this.GetStreamOfContent(this.ZipFilePath + ".zip");

            XElement LabType = this.xeXml.Element("Title");
            if (LabType != null)
            {
                XElement LabTypeName = LabType.Element("Name");
                if (LabTypeName != null)
                {
                    this.Name = LabTypeName.Value.ToString();
                }
                else
                {
                    this.Name = this.LabName;
                }
            }
            string UpToImg = string.Concat(new string[]
            {
                "Upload\\LabImage\\",
                this.Name,
                "\\",
                this.StudentID,
                "\\"
            });
            if (this.file.Size >= 0.0)
            {
                for (int i = 0; i < this.file.Contents.Count; i++)
                {
                    if (i > 0)
                    {
                        this.FileTransferClient.UploadFile(this.file.Name, UpToImg, this.file.Contents[i], true);
                    }
                    else
                    {
                        this.FileTransferClient.UploadFile(this.file.Name, UpToImg, this.file.Contents[i], false);
                    }
                }
                this.UnZip(this.file.Name, UpToImg);
            }
        }

        public void UnZip(string FileName, string UpTo)
        {
            string newFile = UpTo + FileName;
            DoReportUnzipFile(newFile);
        }

        public void UpdateDate()
        {
            try
            {
                XElement LabType = this.xeXml.Element("Title");
                if (LabType != null)
                {
                    XElement LabTypeName = LabType.Element("Name");
                    if (LabTypeName != null)
                    {
                        this.Name = LabTypeName.Value.ToString();
                    }
                    else
                    {
                        this.Name = this.LabName;
                    }
                }
                XElement Group = this.xeXml.Element("Group");
                if (Group != null)
                {
                    IEnumerable<XElement> xeList = Group.Elements("TestTarget");
                    foreach (XElement item in xeList)
                    {
                        XElement Title = item.Element("Title");
                        if (Title != null)
                        {
                            XElement TotalPercent = Title.Element("Completion");
                            if (TotalPercent != null)
                            {
                                try
                                {
                                    if (Convert.ToDouble(TotalPercent.Value) > 0.0)
                                    {
                                        this.FinishPercent = TotalPercent.Value.ToString();
                                        this.Currentpara = item.Element("CurrentSimExp");
                                        if (this.Currentpara != null)
                                        {
                                            XElement step = this.Currentpara.Element("Step");
                                            if (step != null)
                                            {
                                                if (step.Attribute("NO") != null)
                                                {
                                                    this.CurrentStep = step.Attribute("NO").Value;
                                                }
                                            }
                                        }
                                        LabRecord para = new LabRecord();
                                        para.UserID = App.runningInfo.Const.UserName;
                                        para.UserName = App.runningInfo.Const.Name;
                                        para.LabID = App.runningInfo.Global.RunningExperiment.LabID;
                                        para.LabName = this.Name;
                                        para.CurrentStep = this.CurrentStep;
                                        para.CurrentStepXml = this.Currentpara.ToString();
                                        para.StepXml = item.ToString();
                                        para.FinishPercent = Convert.ToDouble(TotalPercent.Value);
                                        XElement LabTitle = item.Element("Title");
                                        if (LabTitle != null)
                                        {
                                            XElement LabContentID = LabTitle.Element("ID");
                                            if (LabContentID != null)
                                            {
                                                para.LabContentID = LabContentID.Value.ToString();
                                            }
                                            XElement LabContent = LabTitle.Element("Name");
                                            if (LabContent != null)
                                            {
                                                para.LabContent = LabContent.Value.ToString();
                                            }
                                        }
                                        this.recordList.Add(para);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
                if (this.recordList.Count > 0)
                {
                    DoReportInsertLabRecord();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("服务器连接出错，数据更新失败！", "ERROR", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        #endregion
    }

    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;

        public int cbData;

        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }
}
