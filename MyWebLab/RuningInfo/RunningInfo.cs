using System;

namespace USTCORi.WebLabClient.RuningInfo
{
    // Token: 0x02000015 RID: 21
    public class RunningInfo
    {
        // Token: 0x060000BF RID: 191 RVA: 0x00006F63 File Offset: 0x00005163
        public RunningInfo()
        {
            this.rConst = new RuningInfoConst();
            this.rGlobal = new RunningInfoGlobalVariable();
        }

        // Token: 0x1700002C RID: 44
        // (get) Token: 0x060000C0 RID: 192 RVA: 0x00006F84 File Offset: 0x00005184
        public RunningInfoGlobalVariable Global
        {
            get
            {
                return this.rGlobal;
            }
        }

        // Token: 0x1700002D RID: 45
        // (get) Token: 0x060000C1 RID: 193 RVA: 0x00006F9C File Offset: 0x0000519C
        public RuningInfoConst Const
        {
            get
            {
                return this.rConst;
            }
        }

        // Token: 0x040000A1 RID: 161
        private RuningInfoConst rConst;

        // Token: 0x040000A2 RID: 162
        private RunningInfoGlobalVariable rGlobal;
    }
}
