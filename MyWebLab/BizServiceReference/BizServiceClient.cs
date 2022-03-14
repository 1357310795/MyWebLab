using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace USTCORi.WebLabClient.BizServiceReference
{
    // Token: 0x02000010 RID: 16
    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class BizServiceClient : ClientBase<IBizService>, IBizService
    {
        // Token: 0x06000066 RID: 102 RVA: 0x00003171 File Offset: 0x00001371
        public BizServiceClient()
        {
        }

        // Token: 0x06000067 RID: 103 RVA: 0x0000317C File Offset: 0x0000137C
        public BizServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        // Token: 0x06000068 RID: 104 RVA: 0x00003188 File Offset: 0x00001388
        public BizServiceClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        // Token: 0x06000069 RID: 105 RVA: 0x00003195 File Offset: 0x00001395
        public BizServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        // Token: 0x0600006A RID: 106 RVA: 0x000031A2 File Offset: 0x000013A2
        public BizServiceClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        // Token: 0x0600006B RID: 107 RVA: 0x000031B0 File Offset: 0x000013B0
        public SvcResponse DoService(SvcRequest request)
        {
            return base.Channel.DoService(request);
        }

        // Token: 0x0600006C RID: 108 RVA: 0x000031D0 File Offset: 0x000013D0
        public int UploadFile(string LabName, string FileName, byte[] Content, bool Append, string Key)
        {
            return base.Channel.UploadFile(LabName, FileName, Content, Append, Key);
        }

        // Token: 0x0600006D RID: 109 RVA: 0x000031F4 File Offset: 0x000013F4
        public int UpdateLabRecord(string UserID, int LabID, string LabName, int RecordID, double Score, string FileName, string Key)
        {
            return base.Channel.UpdateLabRecord(UserID, LabID, LabName, RecordID, Score, FileName, Key);
        }

        // Token: 0x0600006E RID: 110 RVA: 0x0000321C File Offset: 0x0000141C
        public int UploadRecordFile(string FileName, byte[] Content)
        {
            return base.Channel.UploadRecordFile(FileName, Content);
        }

        // Token: 0x0600006F RID: 111 RVA: 0x0000323C File Offset: 0x0000143C
        public int AddDiscuss(int labid, string userid, string fileName)
        {
            return base.Channel.AddDiscuss(labid, userid, fileName);
        }
    }
}
