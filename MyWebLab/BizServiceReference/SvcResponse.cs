using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace USTCORi.WebLabClient.BizServiceReference
{
    // Token: 0x0200000C RID: 12
    [DebuggerStepThrough]
    [KnownType(typeof(ResponseStatus))]
    [DataContract(Name = "SvcResponse", Namespace = "http://www.ustcori.com/2009/10")]
    [KnownType(typeof(SvcRequest))]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [KnownType(typeof(Dictionary<string, object>))]
    [Serializable]
    public class SvcResponse : IExtensibleDataObject, INotifyPropertyChanged
    {
        // Token: 0x17000014 RID: 20
        // (get) Token: 0x06000051 RID: 81 RVA: 0x00002F18 File Offset: 0x00001118
        // (set) Token: 0x06000052 RID: 82 RVA: 0x00002F30 File Offset: 0x00001130
        [Browsable(false)]
        public ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        // Token: 0x17000015 RID: 21
        // (get) Token: 0x06000053 RID: 83 RVA: 0x00002F3C File Offset: 0x0000113C
        // (set) Token: 0x06000054 RID: 84 RVA: 0x00002F54 File Offset: 0x00001154
        [DataMember]
        public object Data
        {
            get
            {
                return this.DataField;
            }
            set
            {
                if (!object.ReferenceEquals(this.DataField, value))
                {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }

        // Token: 0x17000016 RID: 22
        // (get) Token: 0x06000055 RID: 85 RVA: 0x00002F88 File Offset: 0x00001188
        // (set) Token: 0x06000056 RID: 86 RVA: 0x00002FA0 File Offset: 0x000011A0
        [DataMember]
        public string DataString
        {
            get
            {
                return this.DataStringField;
            }
            set
            {
                if (!object.ReferenceEquals(this.DataStringField, value))
                {
                    this.DataStringField = value;
                    this.RaisePropertyChanged("DataString");
                }
            }
        }

        // Token: 0x17000017 RID: 23
        // (get) Token: 0x06000057 RID: 87 RVA: 0x00002FD4 File Offset: 0x000011D4
        // (set) Token: 0x06000058 RID: 88 RVA: 0x00002FEC File Offset: 0x000011EC
        [DataMember]
        public string Message
        {
            get
            {
                return this.MessageField;
            }
            set
            {
                if (!object.ReferenceEquals(this.MessageField, value))
                {
                    this.MessageField = value;
                    this.RaisePropertyChanged("Message");
                }
            }
        }

        // Token: 0x17000018 RID: 24
        // (get) Token: 0x06000059 RID: 89 RVA: 0x00003020 File Offset: 0x00001220
        // (set) Token: 0x0600005A RID: 90 RVA: 0x00003038 File Offset: 0x00001238
        [DataMember]
        public int RecordCount
        {
            get
            {
                return this.RecordCountField;
            }
            set
            {
                if (!this.RecordCountField.Equals(value))
                {
                    this.RecordCountField = value;
                    this.RaisePropertyChanged("RecordCount");
                }
            }
        }

        // Token: 0x17000019 RID: 25
        // (get) Token: 0x0600005B RID: 91 RVA: 0x0000306C File Offset: 0x0000126C
        // (set) Token: 0x0600005C RID: 92 RVA: 0x00003084 File Offset: 0x00001284
        [DataMember]
        public ResponseStatus Status
        {
            get
            {
                return this.StatusField;
            }
            set
            {
                if (!this.StatusField.Equals(value))
                {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }

        // Token: 0x14000004 RID: 4
        // (add) Token: 0x0600005D RID: 93 RVA: 0x000030C4 File Offset: 0x000012C4
        // (remove) Token: 0x0600005E RID: 94 RVA: 0x00003100 File Offset: 0x00001300
        public event PropertyChangedEventHandler PropertyChanged;

        // Token: 0x0600005F RID: 95 RVA: 0x0000313C File Offset: 0x0000133C
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Token: 0x04000047 RID: 71
        [NonSerialized]
        private ExtensionDataObject extensionDataField;

        // Token: 0x04000048 RID: 72
        [OptionalField]
        private object DataField;

        // Token: 0x04000049 RID: 73
        [OptionalField]
        private string DataStringField;

        // Token: 0x0400004A RID: 74
        [OptionalField]
        private string MessageField;

        // Token: 0x0400004B RID: 75
        [OptionalField]
        private int RecordCountField;

        // Token: 0x0400004C RID: 76
        [OptionalField]
        private ResponseStatus StatusField;
    }
}
