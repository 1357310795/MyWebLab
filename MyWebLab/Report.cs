using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using USTCORi.WebLabClient.BizServiceReference;

namespace USTCORi.WebLabClient
{
    // Token: 0x02000012 RID: 18
    public class Report
    {
        // Token: 0x1700001A RID: 26
        // (get) Token: 0x06000074 RID: 116 RVA: 0x0000325C File Offset: 0x0000145C
        // (set) Token: 0x06000075 RID: 117 RVA: 0x00003273 File Offset: 0x00001473
        public Type ReturnType { get; set; }

        // Token: 0x1700001B RID: 27
        // (get) Token: 0x06000076 RID: 118 RVA: 0x0000327C File Offset: 0x0000147C
        // (set) Token: 0x06000077 RID: 119 RVA: 0x00003293 File Offset: 0x00001493
        public string BizCode { get; set; }

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000078 RID: 120 RVA: 0x0000329C File Offset: 0x0000149C
        // (set) Token: 0x06000079 RID: 121 RVA: 0x000032B3 File Offset: 0x000014B3
        public string MethodName { get; set; }

        // Token: 0x0600007A RID: 122 RVA: 0x000032BC File Offset: 0x000014BC
        public Report()
        {
            this.bizClient = new BizServiceClient();
            this.request = new SvcRequest();
            this.request.Parameters = new Dictionary<string, object>();
        }

        // Token: 0x0600007B RID: 123 RVA: 0x000032F0 File Offset: 0x000014F0
        public SvcResponse Run()
        {
            this.request.BizCode = this.BizCode;
            this.request.MethodName = this.MethodName;
            SvcResponse response = this.bizClient.DoService(this.request);
            if (this.ReturnType != null)
            {
                response.Data = JavaScriptConvert.DeserializeObject(response.DataString, this.ReturnType);
            }
            return response;
        }

        // Token: 0x0600007C RID: 124 RVA: 0x00003364 File Offset: 0x00001564
        public void SetParameter(string key, object value)
        {
            string stringValue = JavaScriptConvert.SerializeObject(value);
            if (!this.request.Parameters.ContainsKey(key))
            {
                this.request.Parameters.Add(key, stringValue);
            }
            else
            {
                this.request.Parameters[key] = stringValue;
            }
        }

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x0600007D RID: 125 RVA: 0x000033BC File Offset: 0x000015BC
        // (set) Token: 0x0600007E RID: 126 RVA: 0x000033D3 File Offset: 0x000015D3
        public EventHandler<EventArgs> BeforeStart { get; set; }

        // Token: 0x04000052 RID: 82
        private BizServiceClient bizClient;

        // Token: 0x04000053 RID: 83
        private SvcRequest request;
    }
}
