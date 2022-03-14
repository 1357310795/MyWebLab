using System;
using System.Text;
using USTCORi.WebLabClient.BizServiceReference;

namespace USTCORi.WebLabClient.RuningInfo
{
    // Token: 0x02000029 RID: 41
    public class RuningInfoConst
    {
        // Token: 0x1700004B RID: 75
        // (get) Token: 0x0600013C RID: 316 RVA: 0x0000ADB4 File Offset: 0x00008FB4
        public string Version
        {
            get
            {
                return this.version;
            }
        }

        // Token: 0x1700004C RID: 76
        // (get) Token: 0x0600013D RID: 317 RVA: 0x0000ADCC File Offset: 0x00008FCC
        public string SchoolUser
        {
            get
            {
                return this.allowedUser;
            }
        }

        // Token: 0x1700004D RID: 77
        // (get) Token: 0x0600013E RID: 318 RVA: 0x0000ADE4 File Offset: 0x00008FE4
        public BizServiceClient ServiceClient
        {
            get
            {
                return this.serviceClient;
            }
        }

        // Token: 0x1700004E RID: 78
        // (get) Token: 0x0600013F RID: 319 RVA: 0x0000ADFC File Offset: 0x00008FFC
        public string UserName
        {
            get
            {
                return this.userName;
            }
        }

        // Token: 0x1700004F RID: 79
        // (get) Token: 0x06000140 RID: 320 RVA: 0x0000AE14 File Offset: 0x00009014
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        // Token: 0x17000050 RID: 80
        // (get) Token: 0x06000141 RID: 321 RVA: 0x0000AE2C File Offset: 0x0000902C
        public string IsShowPhysic
        {
            get
            {
                return this.isShowPhysic;
            }
        }

        // Token: 0x17000051 RID: 81
        // (get) Token: 0x06000142 RID: 322 RVA: 0x0000AE44 File Offset: 0x00009044
        public string ConfigName
        {
            get
            {
                string appName = AppDomain.CurrentDomain.FriendlyName;
                appName = appName.Substring(0, appName.Length - 4);
                return appName + ".xml";
            }
        }

        // Token: 0x17000052 RID: 82
        // (get) Token: 0x06000143 RID: 323 RVA: 0x0000AE80 File Offset: 0x00009080
        public string DownloadExperimentPath
        {
            get
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                return basePath + this.downloadExperimentPath;
            }
        }

        // Token: 0x17000053 RID: 83
        // (get) Token: 0x06000144 RID: 324 RVA: 0x0000AEAC File Offset: 0x000090AC
        public string DownloadExperimentXmlPath
        {
            get
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                return basePath + this.downloadExperimentXmlPath;
            }
        }

        // Token: 0x17000054 RID: 84
        // (get) Token: 0x06000145 RID: 325 RVA: 0x0000AED8 File Offset: 0x000090D8
        public string DownloadUpdataPath
        {
            get
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                return basePath + this.downloadUpdataPath;
            }
        }

        // Token: 0x17000055 RID: 85
        // (get) Token: 0x06000146 RID: 326 RVA: 0x0000AF04 File Offset: 0x00009104
        public string ServerIP
        {
            get
            {
                string s = this.serviceClient.Endpoint.Address.Uri.ToString();
                s = s.Substring(7);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '/')
                    {
                        break;
                    }
                    sb.Append(s[i]);
                }
                return sb.ToString();
            }
        }

        // Token: 0x06000147 RID: 327 RVA: 0x0000AF82 File Offset: 0x00009182
        public void SetVersion(string s)
        {
            this.version = s;
        }

        // Token: 0x06000148 RID: 328 RVA: 0x0000AF8C File Offset: 0x0000918C
        public void SetIsShowPhysic(string s)
        {
            this.isShowPhysic = s;
        }

        // Token: 0x06000149 RID: 329 RVA: 0x0000AF96 File Offset: 0x00009196
        public void SetShoolUser(string s)
        {
            this.allowedUser = s;
        }

        // Token: 0x0600014A RID: 330 RVA: 0x0000AFA0 File Offset: 0x000091A0
        public void SetClient(BizServiceClient c)
        {
            this.serviceClient = c;
        }

        // Token: 0x0600014B RID: 331 RVA: 0x0000AFAA File Offset: 0x000091AA
        public void SetUserName(string s)
        {
            this.userName = s;
        }

        // Token: 0x0600014C RID: 332 RVA: 0x0000AFB4 File Offset: 0x000091B4
        public void SetName(string s)
        {
            this.name = s;
        }

        // Token: 0x0600014D RID: 333 RVA: 0x0000AFBE File Offset: 0x000091BE
        public void SetDownloadExperimentPath(string s)
        {
            this.downloadExperimentPath = s;
        }

        // Token: 0x0600014E RID: 334 RVA: 0x0000AFC8 File Offset: 0x000091C8
        public void SetDownloadExperimentXmlPath(string s)
        {
            this.downloadExperimentXmlPath = s;
        }

        // Token: 0x0600014F RID: 335 RVA: 0x0000AFD2 File Offset: 0x000091D2
        public void SetDownloadUpdataPath(string s)
        {
            this.downloadUpdataPath = s;
        }

        // Token: 0x040000DF RID: 223
        private string version;

        // Token: 0x040000E0 RID: 224
        private BizServiceClient serviceClient;

        // Token: 0x040000E1 RID: 225
        private string allowedUser;

        // Token: 0x040000E2 RID: 226
        private string userName;

        // Token: 0x040000E3 RID: 227
        private string name;

        // Token: 0x040000E4 RID: 228
        private string password;

        // Token: 0x040000E5 RID: 229
        private string downloadExperimentPath;

        // Token: 0x040000E6 RID: 230
        private string downloadExperimentXmlPath;

        // Token: 0x040000E7 RID: 231
        private string downloadUpdataPath;

        // Token: 0x040000E8 RID: 232
        private string isShowPhysic;
    }
}
