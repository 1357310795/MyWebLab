using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace USTCORi.WebLabClient.FileTransferReference
{
    // Token: 0x02000021 RID: 33
    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class FileTransferClient : ClientBase<IFileTransfer>, IFileTransfer
    {
        // Token: 0x060000FF RID: 255 RVA: 0x000094EA File Offset: 0x000076EA
        public FileTransferClient()
        {
        }

        // Token: 0x06000100 RID: 256 RVA: 0x000094F5 File Offset: 0x000076F5
        public FileTransferClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        // Token: 0x06000101 RID: 257 RVA: 0x00009501 File Offset: 0x00007701
        public FileTransferClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        // Token: 0x06000102 RID: 258 RVA: 0x0000950E File Offset: 0x0000770E
        public FileTransferClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        // Token: 0x06000103 RID: 259 RVA: 0x0000951B File Offset: 0x0000771B
        public FileTransferClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        // Token: 0x06000104 RID: 260 RVA: 0x00009528 File Offset: 0x00007728
        public void UploadFile(string fileName, string path, byte[] content, bool append)
        {
            base.Channel.UploadFile(fileName, path, content, append);
        }

        // Token: 0x06000105 RID: 261 RVA: 0x0000953C File Offset: 0x0000773C
        public string[] DownloadAllPic(string path)
        {
            return base.Channel.DownloadAllPic(path);
        }

        // Token: 0x06000106 RID: 262 RVA: 0x0000955C File Offset: 0x0000775C
        public string DownLoadFile(string path)
        {
            return base.Channel.DownLoadFile(path);
        }
    }
}
