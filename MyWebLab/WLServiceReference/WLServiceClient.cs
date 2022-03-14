using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace USTCORi.WebLabClient.WLServiceReference
{
    // Token: 0x0200000A RID: 10
    [DebuggerStepThrough]
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public class WLServiceClient : ClientBase<IWLService>, IWLService
    {
        // Token: 0x0600003D RID: 61 RVA: 0x00002CB5 File Offset: 0x00000EB5
        public WLServiceClient()
        {
        }

        // Token: 0x0600003E RID: 62 RVA: 0x00002CC0 File Offset: 0x00000EC0
        public WLServiceClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00002CCC File Offset: 0x00000ECC
        public WLServiceClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        // Token: 0x06000040 RID: 64 RVA: 0x00002CD9 File Offset: 0x00000ED9
        public WLServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        // Token: 0x06000041 RID: 65 RVA: 0x00002CE6 File Offset: 0x00000EE6
        public WLServiceClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        // Token: 0x06000042 RID: 66 RVA: 0x00002CF4 File Offset: 0x00000EF4
        public string DoService(RequestInfo requestInfo)
        {
            return base.Channel.DoService(requestInfo);
        }
    }
}
