using System;
using System.CodeDom.Compiler;
using System.Runtime.Serialization;

namespace USTCORi.WebLabClient.BizServiceReference
{
    // Token: 0x0200000D RID: 13
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "ResponseStatus", Namespace = "http://www.ustcori.com/2009/10")]
    public enum ResponseStatus
    {
        // Token: 0x0400004F RID: 79
        [EnumMember]
        Success,
        // Token: 0x04000050 RID: 80
        [EnumMember]
        BadRequest,
        // Token: 0x04000051 RID: 81
        [EnumMember]
        UnknownFailure
    }
}
