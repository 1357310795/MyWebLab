using System;
using System.Collections.Generic;

namespace USTCORi.WebLabClient.Common
{
    // Token: 0x02000004 RID: 4
    public class FileInfoManager
    {
        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000009 RID: 9 RVA: 0x000021AC File Offset: 0x000003AC
        // (set) Token: 0x0600000A RID: 10 RVA: 0x000021C3 File Offset: 0x000003C3
        public string Name { get; set; }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x0600000B RID: 11 RVA: 0x000021CC File Offset: 0x000003CC
        // (set) Token: 0x0600000C RID: 12 RVA: 0x000021E3 File Offset: 0x000003E3
        public double Size { get; set; }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x0600000D RID: 13 RVA: 0x000021EC File Offset: 0x000003EC
        // (set) Token: 0x0600000E RID: 14 RVA: 0x00002203 File Offset: 0x00000403
        public double Sent { get; set; }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600000F RID: 15 RVA: 0x0000220C File Offset: 0x0000040C
        // (set) Token: 0x06000010 RID: 16 RVA: 0x00002223 File Offset: 0x00000423
        public string FileLength { get; set; }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x06000011 RID: 17 RVA: 0x0000222C File Offset: 0x0000042C
        // (set) Token: 0x06000012 RID: 18 RVA: 0x00002243 File Offset: 0x00000443
        public string FileHashCode { get; set; }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000013 RID: 19 RVA: 0x0000224C File Offset: 0x0000044C
        // (set) Token: 0x06000014 RID: 20 RVA: 0x00002263 File Offset: 0x00000463
        public List<byte[]> Contents { get; set; }
    }
}
