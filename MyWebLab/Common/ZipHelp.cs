using System;
using System.IO;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;

namespace USTCORi.WebLabClient.Common
{
    // Token: 0x0200002B RID: 43
    public class ZipHelp
    {
        // Token: 0x06000167 RID: 359 RVA: 0x0000B1FC File Offset: 0x000093FC
        public static void ZipFile(string strFile, string strZip)
        {
            if (strFile[strFile.Length - 1] != Path.DirectorySeparatorChar)
            {
                strFile += Path.DirectorySeparatorChar;
            }
            if (File.Exists(strZip))
            {
                File.Delete(strZip);
            }
            ZipOutputStream zos = new ZipOutputStream(File.Create(strZip));
            zos.SetLevel(6);
            ZipHelp.zip(strFile, zos, strFile);
            zos.Finish();
            zos.Close();
        }

        // Token: 0x06000168 RID: 360 RVA: 0x0000B278 File Offset: 0x00009478
        private static void zip(string strFile, ZipOutputStream zos, string staticFile)
        {
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strFile);
            foreach (string file in filenames)
            {
                if (Directory.Exists(file))
                {
                    ZipHelp.zip(file, zos, staticFile);
                }
                else
                {
                    FileStream fs = File.OpenRead(file);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    string tempfile = file.Substring(staticFile.LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(tempfile);
                    entry.DateTime = DateTime.Now;
                    entry.Size = fs.Length;
                    fs.Close();
                    crc.Reset();
                    crc.Update(buffer);
                    entry.Crc = crc.Value;
                    zos.PutNextEntry(entry);
                    zos.Write(buffer, 0, buffer.Length);
                }
            }
        }

        // Token: 0x06000169 RID: 361 RVA: 0x0000B378 File Offset: 0x00009578
        public static string UnZipFile(string TargetFile, string fileDir)
        {
            string rootFile = "";
            string result;
            try
            {
                ZipInputStream s = new ZipInputStream(File.OpenRead(TargetFile.Trim()));
                string path = fileDir;
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    string rootDir = Path.GetDirectoryName(theEntry.Name);
                    if (rootDir.IndexOf("\\") >= 0)
                    {
                        rootDir = rootDir.Substring(0, rootDir.IndexOf("\\") + 1);
                    }
                    string dir = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);
                    if (dir != " ")
                    {
                        if (!Directory.Exists(fileDir + "\\" + dir))
                        {
                            path = fileDir + "\\" + dir;
                            Directory.CreateDirectory(path);
                        }
                    }
                    else if (dir == " " && fileName != "")
                    {
                        path = fileDir;
                        rootFile = fileName;
                    }
                    else if (dir != " " && fileName != "")
                    {
                        if (dir.IndexOf("\\") > 0)
                        {
                            path = fileDir + "\\" + dir;
                        }
                    }
                    if (dir == rootDir)
                    {
                        path = fileDir + "\\" + rootDir;
                    }
                    if (fileName != string.Empty)
                    {
                        FileStream streamWriter = File.Create(path + "\\" + fileName);
                        byte[] data = new byte[2048];
                        for (;;)
                        {
                            int size = s.Read(data, 0, data.Length);
                            if (size <= 0)
                            {
                                break;
                            }
                            streamWriter.Write(data, 0, size);
                        }
                        streamWriter.Close();
                    }
                }
                s.Close();
                result = rootFile;
            }
            catch (Exception ex)
            {
                result = "1; " + ex.Message;
            }
            return result;
        }
    }
}
