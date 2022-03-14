using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace USTCORi.WebLabClient.WLServiceReference
{
    // Token: 0x02000008 RID: 8
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(Namespace = "http://www.ustcori.com/2009/09", ConfigurationName = "WLServiceReference.IWLService")]
    public interface IWLService
    {
        // Token: 0x0600003C RID: 60
        [OperationContract(Action = "http://www.ustcori.com/2009/09/IWLService/DoService", ReplyAction = "http://www.ustcori.com/2009/09/IWLService/DoServiceResponse")]
        string DoService(RequestInfo requestInfo);
    }
}
