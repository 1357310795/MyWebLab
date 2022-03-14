using System;
using System.Diagnostics;
using USTCORi.WebLabClient.DataModel;

namespace USTCORi.WebLabClient.RuningInfo
{
    // Token: 0x02000024 RID: 36
    public class RunningInfoGlobalVariable
    {
        // Token: 0x17000038 RID: 56
        // (get) Token: 0x06000114 RID: 276 RVA: 0x00009BA8 File Offset: 0x00007DA8
        // (set) Token: 0x06000115 RID: 277 RVA: 0x00009BC0 File Offset: 0x00007DC0
        public bool NeedConfirmShutdown
        {
            get
            {
                return this.needConfirmShutdown;
            }
            set
            {
                this.needConfirmShutdown = value;
            }
        }

        // Token: 0x17000039 RID: 57
        // (get) Token: 0x06000116 RID: 278 RVA: 0x00009BCC File Offset: 0x00007DCC
        // (set) Token: 0x06000117 RID: 279 RVA: 0x00009BE4 File Offset: 0x00007DE4
        public LABINFO RunningExperiment
        {
            get
            {
                return this.runningExperiment;
            }
            set
            {
                this.runningExperiment = value;
            }
        }

        /// <summary>
        /// 实验进程
        /// </summary>
        public Process RunningProcess
        {
            get
            {
                return this.runningpro;
            }
            set
            {
                this.runningpro = value;
            }
        }
        
        public void ShutdownLab()
        {
            if (this.runningpro != null)
            {
                if (!this.runningpro.HasExited)
                {
                    this.runningpro.Kill();
                }
                this.runningpro = null;
                this.runningExperiment = null;
            }
        }

        // Token: 0x040000CB RID: 203
        private bool needConfirmShutdown;

        // Token: 0x040000CC RID: 204
        private LABINFO runningExperiment;

        // Token: 0x040000CD RID: 205
        private Process runningpro;
    }
}
