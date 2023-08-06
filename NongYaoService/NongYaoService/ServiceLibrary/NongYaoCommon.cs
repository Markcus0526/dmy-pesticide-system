using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;

namespace NongYaoService.ServiceLibrary
{
    public class NongYaoCommon
    {
        #region Fields and properties
        private static String strErrorFileName = "NongYaoError.log";
        public static String strAuthSalt = "NongYao1234567890987654321";
        #endregion

        #region Public methods
        public static void LogErrors(String pMessage)
        {
            try
            {
                //StreamWriter w = File.AppendText(System.Environment.CurrentDirectory + "\\" + strErrorFileName);
                StreamWriter w = File.AppendText("C:\\" + strErrorFileName);
                w.WriteLine("{0}--{1}--{2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), pMessage);
                w.Flush();
                w.Close();
            }
            catch
            {
                //LogErrors(ex.ToString());
            }
        }

        public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            //            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value + strAuthSalt));
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static String GetParentDir(String szPath)
        {
            String szParentDir = szPath;

            szParentDir = szParentDir.Replace("\\", "/");
            int nIndex = szParentDir.LastIndexOf("/");

            if (nIndex > 0)
                szParentDir = szParentDir.Substring(0, nIndex);

            return szParentDir;
        }

        public static String saveImage(String szImageData, long bid)
        {
            if (szImageData == String.Empty || szImageData == null)
                return String.Empty;

            String basePath = WebConfigurationManager.AppSettings["UploadPath"];
         /*   if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);*/

            String szFullPath = String.Empty, szNewPath = String.Empty;

            szNewPath = "Content/uploads/image/" +
                        bid.ToString() + "_" +
                        DateTime.Now.ToString("yyyy_MM") + "/" +
                        DateTime.Now.ToString("dd") + "/" +
                        DateTime.Now.ToString("yyyyMMddHHmmss") +
                        NongYaoCommon.GenRndNumSeries(2) + ".png";


            szFullPath = basePath + szNewPath;

            try
            {
                byte[] imgData = Convert.FromBase64String(szImageData);

                String szTmpPath = szFullPath.Replace("/", "\\");
                String szDir = NongYaoCommon.GetParentDir(szTmpPath);

                if (!File.Exists(szDir))
                    Directory.CreateDirectory(szDir);

                File.WriteAllBytes(szTmpPath, imgData);
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.Message);
                szNewPath = "";
            }

            return szNewPath;
        }

        public static String GenRndNumSeries(int size)
        {
            Random _rng = new Random();
            string _chars = "0123456789";
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
                buffer[i] = _chars[_rng.Next(_chars.Length)];

            return new string(buffer);
        }    
          
        #endregion
    }
}