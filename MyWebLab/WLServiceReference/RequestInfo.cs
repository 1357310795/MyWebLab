using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace USTCORi.WebLabClient.WLServiceReference
{
    // Token: 0x02000007 RID: 7
    [DataContract(Name = "RequestInfo", Namespace = "http://www.ustcori.com/2009/09")]
    [DebuggerStepThrough]
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [Serializable]
    public class RequestInfo : IExtensibleDataObject, INotifyPropertyChanged
    {
        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000030 RID: 48 RVA: 0x00002B00 File Offset: 0x00000D00
        // (set) Token: 0x06000031 RID: 49 RVA: 0x00002B18 File Offset: 0x00000D18
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

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000032 RID: 50 RVA: 0x00002B24 File Offset: 0x00000D24
        // (set) Token: 0x06000033 RID: 51 RVA: 0x00002B3C File Offset: 0x00000D3C
        [DataMember]
        public string ActionName
        {
            get
            {
                return this.ActionNameField;
            }
            set
            {
                if (!object.ReferenceEquals(this.ActionNameField, value))
                {
                    this.ActionNameField = value;
                    this.RaisePropertyChanged("ActionName");
                }
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000034 RID: 52 RVA: 0x00002B70 File Offset: 0x00000D70
        // (set) Token: 0x06000035 RID: 53 RVA: 0x00002B88 File Offset: 0x00000D88
        [DataMember]
        public string BizName
        {
            get
            {
                return this.BizNameField;
            }
            set
            {
                if (!object.ReferenceEquals(this.BizNameField, value))
                {
                    this.BizNameField = value;
                    this.RaisePropertyChanged("BizName");
                }
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000036 RID: 54 RVA: 0x00002BBC File Offset: 0x00000DBC
        // (set) Token: 0x06000037 RID: 55 RVA: 0x00002BD4 File Offset: 0x00000DD4
        [DataMember]
        public string Params
        {
            get
            {
                return this.ParamsField;
            }
            set
            {
                if (!object.ReferenceEquals(this.ParamsField, value))
                {
                    this.ParamsField = value;
                    this.RaisePropertyChanged("Params");
                }
            }
        }

        // Token: 0x14000002 RID: 2
        // (add) Token: 0x06000038 RID: 56 RVA: 0x00002C08 File Offset: 0x00000E08
        // (remove) Token: 0x06000039 RID: 57 RVA: 0x00002C44 File Offset: 0x00000E44
        public event PropertyChangedEventHandler PropertyChanged;

        // Token: 0x0600003A RID: 58 RVA: 0x00002C80 File Offset: 0x00000E80
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Token: 0x0400003C RID: 60
        [NonSerialized]
        private ExtensionDataObject extensionDataField;

        // Token: 0x0400003D RID: 61
        [OptionalField]
        private string ActionNameField;

        // Token: 0x0400003E RID: 62
        [OptionalField]
        private string BizNameField;

        // Token: 0x0400003F RID: 63
        [OptionalField]
        private string ParamsField;
    }
}
