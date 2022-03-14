using MyWebLab;
using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;

namespace USTCORi.WebLabClient.Common
{
    // Token: 0x02000027 RID: 39
    public static class WcfServiceClientFactory<TServiceClient, TService> where TServiceClient : ClientBase<TService>, TService where TService : class
    {
        // Token: 0x06000137 RID: 311 RVA: 0x0000AB84 File Offset: 0x00008D84
        public static TServiceClient CreateServiceClient()
        {
            string typeName = typeof(TService).Name.Substring(1);
            string serviceAddress = "../" + typeName + ".svc";
            return WcfServiceClientFactory<TServiceClient, TService>.CreateServiceClient(serviceAddress);
        }

        // Token: 0x06000138 RID: 312 RVA: 0x0000ABC4 File Offset: 0x00008DC4
        public static TServiceClient CreateServiceClient(string serviceAddress)
        {
            EndpointAddress endpointAddr = new EndpointAddress(new Uri(new Uri("http://" + App.runningInfo.Const.ServerIP, UriKind.RelativeOrAbsolute), serviceAddress), new AddressHeader[0]);
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.OpenTimeout = new TimeSpan(0, 1, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 1, 0);
            binding.SendTimeout = new TimeSpan(0, 1, 0);
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.TransferMode = TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;
            binding.MaxReceivedMessageSize = 500000L;
            binding.MaxBufferSize = 500000;
            binding.MaxBufferPoolSize = 500000L;
            binding.AllowCookies = false;
            binding.MessageEncoding = WSMessageEncoding.Text;
            binding.TextEncoding = Encoding.UTF8;
            binding.ReaderQuotas.MaxStringContentLength = 100000;
            binding.ReaderQuotas.MaxDepth = 50;
            binding.ReaderQuotas.MaxArrayLength = 100000;
            binding.ReaderQuotas.MaxBytesPerRead = 100000;
            ConstructorInfo ctor = typeof(TServiceClient).GetConstructor(new Type[]
            {
                typeof(Binding),
                typeof(EndpointAddress)
            });
            return (TServiceClient)((object)ctor.Invoke(new object[]
            {
                binding,
                endpointAddr
            }));
        }
    }
}
