using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json;

namespace USTCORi.WebLabClient.Common
{
    // Token: 0x0200001A RID: 26
    public class CMMUtils
    {
        // Token: 0x060000D6 RID: 214 RVA: 0x00008744 File Offset: 0x00006944
        public static string JsonSerialize(object obj)
        {
            return JavaScriptConvert.SerializeObject(obj);
        }

        // Token: 0x060000D7 RID: 215 RVA: 0x0000875C File Offset: 0x0000695C
        public static object JsonDeserialize(string jsonString)
        {
            return JavaScriptConvert.DeserializeObject(jsonString);
        }

        // Token: 0x060000D8 RID: 216 RVA: 0x00008774 File Offset: 0x00006974
        public static object JsonDeserialize(string jsonString, Type dataType)
        {
            return JavaScriptConvert.DeserializeObject(jsonString, dataType);
        }

        // Token: 0x060000D9 RID: 217 RVA: 0x00008790 File Offset: 0x00006990
        public static T JsonDeserialize<T>(string jsonString)
        {
            return JavaScriptConvert.DeserializeObject<T>(jsonString);
        }

        // Token: 0x060000DA RID: 218 RVA: 0x000087A8 File Offset: 0x000069A8
        public static string GetFilePathByType(int iFileType)
        {
            string result;
            if (iFileType == WLConstants.FILE_LABCLIENT)
            {
                result = WLConstants.FILE_LABCLIENT_PATH;
            }
            else if (iFileType == WLConstants.FILE_LAB)
            {
                result = WLConstants.FILE_LAB_PATH;
            }
            else if (iFileType == WLConstants.FILE_INSTRUCTION_TYPE)
            {
                result = WLConstants.FILE_INSTRUCTION_TYPE_PATH;
            }
            else if (iFileType == WLConstants.FILE_LAB_SHOWONLINE)
            {
                result = WLConstants.FILE_LAB_SHOWONLINE_PATH;
            }
            else if (iFileType == WLConstants.FILE_IMAGE_TYPE)
            {
                result = WLConstants.FILE_IMAGE_TYPE_PATH;
            }
            else
            {
                result = "";
            }
            return result;
        }

        // Token: 0x060000DB RID: 219 RVA: 0x00008838 File Offset: 0x00006A38
        public static RichTextBox GenRichTextBox(string text, bool canEdit)
        {
            RichTextBox rta = new RichTextBox();
            DocumentPersister.ParseSavedDocument(text, rta.Document.Blocks);
            if (canEdit)
            {
                rta.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                rta.IsReadOnly = false;
                rta.IsTabStop = true;
                rta.Background = (rta.BorderBrush = new SolidColorBrush(Colors.White));
            }
            else
            {
                rta.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                rta.IsTabStop = false;
                rta.IsReadOnly = true;
                rta.BorderThickness = new Thickness(0.0);
                rta.Background = (rta.BorderBrush = new SolidColorBrush(Color.FromArgb(byte.MaxValue, 237, 242, 249)));
            }
            return rta;
        }

        // Token: 0x060000DC RID: 220 RVA: 0x00008900 File Offset: 0x00006B00
        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)Delegate.Combine(ServicePointManager.ServerCertificateValidationCallback, new RemoteCertificateValidationCallback(CMMUtils.RemoteCertificateValidate));
        }

        // Token: 0x060000DD RID: 221 RVA: 0x00008924 File Offset: 0x00006B24
        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            return true;
        }
    }
}
