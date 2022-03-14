using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace USTCORi.WebLabClient.FileTransferReference
{
    // Token: 0x0200001F RID: 31
    [ServiceContract(ConfigurationName = "FileTransferReference.IFileTransfer")]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public interface IFileTransfer
    {
        // Token: 0x060000FC RID: 252
        [OperationContract(Action = "http://tempuri.org/IFileTransfer/UploadFile", ReplyAction = "http://tempuri.org/IFileTransfer/UploadFileResponse")]
        void UploadFile(string fileName, string path, byte[] content, bool append);

        // Token: 0x060000FD RID: 253
        [OperationContract(Action = "http://tempuri.org/IFileTransfer/DownloadAllPic", ReplyAction = "http://tempuri.org/IFileTransfer/DownloadAllPicResponse")]
        string[] DownloadAllPic(string path);

        // Token: 0x060000FE RID: 254
        [OperationContract(Action = "http://tempuri.org/IFileTransfer/DownLoadFile", ReplyAction = "http://tempuri.org/IFileTransfer/DownLoadFileResponse")]
        string DownLoadFile(string path);
    }
}
