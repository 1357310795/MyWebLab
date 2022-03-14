using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace USTCORi.WebLabClient.BizServiceReference
{
    // Token: 0x0200000E RID: 14
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(Namespace = "http://www.ustcori.com/2009/10", ConfigurationName = "BizServiceReference.IBizService")]
    public interface IBizService
    {
        // Token: 0x06000061 RID: 97
        [OperationContract(Action = "http://www.ustcori.com/2009/10/IBizService/DoService", ReplyAction = "http://www.ustcori.com/2009/10/IBizService/DoServiceResponse")]
        SvcResponse DoService(SvcRequest request);

        // Token: 0x06000062 RID: 98
        [OperationContract(Action = "http://www.ustcori.com/2009/10/IBizService/UploadFile", ReplyAction = "http://www.ustcori.com/2009/10/IBizService/UploadFileResponse")]
        int UploadFile(string LabName, string FileName, byte[] Content, bool Append, string Key);

        // Token: 0x06000063 RID: 99
        [OperationContract(Action = "http://www.ustcori.com/2009/10/IBizService/UpdateLabRecord", ReplyAction = "http://www.ustcori.com/2009/10/IBizService/UpdateLabRecordResponse")]
        int UpdateLabRecord(string UserID, int LabID, string LabName, int RecordID, double Score, string FileName, string Key);

        // Token: 0x06000064 RID: 100
        [OperationContract(Action = "http://www.ustcori.com/2009/10/IBizService/UploadRecordFile", ReplyAction = "http://www.ustcori.com/2009/10/IBizService/UploadRecordFileResponse")]
        int UploadRecordFile(string FileName, byte[] Content);

        // Token: 0x06000065 RID: 101
        [OperationContract(Action = "http://www.ustcori.com/2009/10/IBizService/AddDiscuss", ReplyAction = "http://www.ustcori.com/2009/10/IBizService/AddDiscussResponse")]
        int AddDiscuss(int labid, string userid, string fileName);
    }
}
