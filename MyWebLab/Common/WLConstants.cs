using MyWebLab;
using System;

namespace USTCORi.WebLabClient.Common
{
    // Token: 0x02000002 RID: 2
    public class WLConstants
    {
        // Token: 0x04000001 RID: 1
        public const string SYJJ_FILENAME = "/实验简介.htm";

        // Token: 0x04000002 RID: 2
        public const string SYYL_FILENAME = "/实验原理.htm";

        // Token: 0x04000003 RID: 3
        public const string SYZD_FILENAME = "/实验指导.htm";

        // Token: 0x04000004 RID: 4
        public const string SYZDS_FILENAME = "/实验指导书.chm";

        // Token: 0x04000005 RID: 5
        public const string SYNR_FILENAME = "/实验内容.htm";

        // Token: 0x04000006 RID: 6
        public const string SYYQ_FILENAME = "/实验仪器.htm";

        // Token: 0x04000007 RID: 7
        public const string SYYS_FILENAME = "/实验演示.htm";

        // Token: 0x04000008 RID: 8
        public const string SYTP_FILENAME = "/实验图片.png";

        // Token: 0x04000009 RID: 9
        public static readonly string BIZEXCEPTION_NAME = "USTCORI.CommonLib.ExceptionHandling.BizException";

        // Token: 0x0400000A RID: 10
        public static readonly string BLL_NAMESPACE = "USTCORi.WebLab.BLL.";

        // Token: 0x0400000B RID: 11
        public static readonly string SERVICE_ADDRESS = "http://" + App.runningInfo.Const.ServerIP + "/";

        // Token: 0x0400000C RID: 12
        public static readonly string SESSION_KEY_EXCEPTION = "SESSION异常";

        // Token: 0x0400000D RID: 13
        public static readonly string ERROR_PAGE_URL = "/Common/ErrorPage.xaml";

        // Token: 0x0400000E RID: 14
        public static readonly string DEFAULT_PAGE_URL = "/USTCORi.WebLab.WebUITestPage.html";

        // Token: 0x0400000F RID: 15
        public static readonly int FILE_LABCLIENT = 0;

        // Token: 0x04000010 RID: 16
        public static readonly int FILE_LAB = 1;

        // Token: 0x04000011 RID: 17
        public static readonly int FILE_INSTRUCTION_TYPE = 2;

        // Token: 0x04000012 RID: 18
        public static readonly int FILE_LAB_SHOWONLINE = 3;

        // Token: 0x04000013 RID: 19
        public static readonly int FILE_IMAGE_TYPE = 4;

        // Token: 0x04000014 RID: 20
        public static readonly int BLOCK = 1000000;

        // Token: 0x04000015 RID: 21
        public static readonly string FILE_LABCLIENT_PATH = "FileUpload/labClient/";

        // Token: 0x04000016 RID: 22
        public static readonly string FILE_LAB_PATH = "/FileUpload/lab/";

        // Token: 0x04000017 RID: 23
        public static readonly string FILE_INSTRUCTION_TYPE_PATH = "FileUpload/instruction/";

        // Token: 0x04000018 RID: 24
        public static readonly string FILE_LAB_SHOWONLINE_PATH = "FileUpload/showonline/";

        // Token: 0x04000019 RID: 25
        public static readonly string FILE_IMAGE_TYPE_PATH = "FileUpload/image/";

        // Token: 0x0400001A RID: 26
        public static readonly string LAB_SOURCES_PATH = "FileUpload/LabSources/";

        // Token: 0x0400001B RID: 27
        public static readonly string ROLE_ADMIN = "1";

        // Token: 0x0400001C RID: 28
        public static readonly string ROLE_STUDENT = "2";
    }
}
