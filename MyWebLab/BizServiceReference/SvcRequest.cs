using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace USTCORi.WebLabClient.BizServiceReference
{
    // Token: 0x0200000B RID: 11
    [GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name = "SvcRequest", Namespace = "http://www.ustcori.com/2009/10")]
    [DebuggerStepThrough]
    [Serializable]
    public class SvcRequest : IExtensibleDataObject, INotifyPropertyChanged
    {
        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000043 RID: 67 RVA: 0x00002D14 File Offset: 0x00000F14
        // (set) Token: 0x06000044 RID: 68 RVA: 0x00002D2C File Offset: 0x00000F2C
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

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x06000045 RID: 69 RVA: 0x00002D38 File Offset: 0x00000F38
        // (set) Token: 0x06000046 RID: 70 RVA: 0x00002D50 File Offset: 0x00000F50
        [DataMember]
        public string BizCode
        {
            get
            {
                return this.BizCodeField;
            }
            set
            {
                if (!object.ReferenceEquals(this.BizCodeField, value))
                {
                    this.BizCodeField = value;
                    this.RaisePropertyChanged("BizCode");
                }
            }
        }

        // Token: 0x17000011 RID: 17
        // (get) Token: 0x06000047 RID: 71 RVA: 0x00002D84 File Offset: 0x00000F84
        // (set) Token: 0x06000048 RID: 72 RVA: 0x00002D9C File Offset: 0x00000F9C
        [DataMember]
        public bool EnableCache
        {
            get
            {
                return this.EnableCacheField;
            }
            set
            {
                if (!this.EnableCacheField.Equals(value))
                {
                    this.EnableCacheField = value;
                    this.RaisePropertyChanged("EnableCache");
                }
            }
        }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x06000049 RID: 73 RVA: 0x00002DD0 File Offset: 0x00000FD0
        // (set) Token: 0x0600004A RID: 74 RVA: 0x00002DE8 File Offset: 0x00000FE8
        [DataMember]
        public string MethodName
        {
            get
            {
                return this.MethodNameField;
            }
            set
            {
                if (!object.ReferenceEquals(this.MethodNameField, value))
                {
                    this.MethodNameField = value;
                    this.RaisePropertyChanged("MethodName");
                }
            }
        }

        // Token: 0x17000013 RID: 19
        // (get) Token: 0x0600004B RID: 75 RVA: 0x00002E1C File Offset: 0x0000101C
        // (set) Token: 0x0600004C RID: 76 RVA: 0x00002E34 File Offset: 0x00001034
        [DataMember]
        public Dictionary<string, object> Parameters
        {
            get
            {
                return this.ParametersField;
            }
            set
            {
                if (!object.ReferenceEquals(this.ParametersField, value))
                {
                    this.ParametersField = value;
                    this.RaisePropertyChanged("Parameters");
                }
            }
        }

        // Token: 0x14000003 RID: 3
        // (add) Token: 0x0600004D RID: 77 RVA: 0x00002E68 File Offset: 0x00001068
        // (remove) Token: 0x0600004E RID: 78 RVA: 0x00002EA4 File Offset: 0x000010A4
        public event PropertyChangedEventHandler PropertyChanged;

        // Token: 0x0600004F RID: 79 RVA: 0x00002EE0 File Offset: 0x000010E0
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Token: 0x04000041 RID: 65
        [NonSerialized]
        private ExtensionDataObject extensionDataField;

        // Token: 0x04000042 RID: 66
        [OptionalField]
        private string BizCodeField;

        // Token: 0x04000043 RID: 67
        [OptionalField]
        private bool EnableCacheField;

        // Token: 0x04000044 RID: 68
        [OptionalField]
        private string MethodNameField;

        // Token: 0x04000045 RID: 69
        [OptionalField]
        private Dictionary<string, object> ParametersField;
    }
}
