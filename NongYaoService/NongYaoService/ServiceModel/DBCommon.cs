using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Text;

namespace NongYaoService.ServiceModel
{
    #region Error constants
    public enum NONGYAOERROR
    {
        ERR_SUCCESS = 0, 
        ERR_FAILURE = 100,   
        ERR_DUPLICATEUSERID = 101,
        ERR_DUPLICATESTORENAME = 102,
        ERR_NOUSER = 103,
        ERR_TICKETNUMUSED = 104,
        ERR_NO_IMAGE = 105,
        ERR_CATALOGOVERFLOW = 106,
        ERR_NOCATALOG = 107,
        ERR_NOCUSTOMER = 108,
        ERR_SALECATALOGOVERFLOW = 109,
        ERR_NO_SALECATALOG = 110,
        ERR_OVER_AVAILDATE = 111,
        ERR_DUPLICATEUSERNAME = 112,
        ERR_DUPLICATEREGISTERID = 113,
        ERR_TRIAL_VERSION = -1,

        ERR_DUPLICATE_LARGENUMBER = 200,     //"该产品批号的生产日期不正确";
        ERR_REMAIN_INSUFFICIENT =  201 //"库存不充足";
    }
  
    #endregion

    #region [DataContract] - Common
    [DataContract]
    [KnownType(typeof(List<tbl_user>))]
    /*[KnownType(typeof(List<ApkGroupInfo>))]
    [KnownType(typeof(List<ApkInfo>))]*/

    [Newtonsoft.Json.JsonObject(MemberSerialization = Newtonsoft.Json.MemberSerialization.OptIn)]
    public class NongYaoResponseData
    {
        [DataMember(Name = "SVCC_RET", Order = 1)]
        public NONGYAOERROR Result { get; set; }

        object retdata = new object();
        [DataMember(Name = "SVCC_DATA", Order = 2), Newtonsoft.Json.JsonProperty]
        public object Data
        {
            get { return retdata; }
            set { retdata = value; }
        }

        String token = "";
        [DataMember(Name = "SVCC_TOKEN", Order = 3)]
        public String Token
        {
            get { return token; }
            set { token = value; }
        }

        String baseurl = ConfigurationManager.AppSettings["ServerRootUri"];
        [DataMember(Name = "SVCC_BASEURL", Order = 4)]
        public String BaseUrl
        {
            get { return baseurl; }
            set { baseurl = value; }
        }
    }
    #endregion
    public class DBCommon
    {

        public static string GetPinyinCode(string unicodeString)
        {
            int i = 0;
            ushort key = 0;
            string strResult = string.Empty;
            //创建两个不同的encoding对象
            Encoding unicode = Encoding.Unicode;
            //创建GBK码对象
            Encoding gbk = Encoding.GetEncoding(936);
            //将unicode字符串转换为字节
            byte[] unicodeBytes = unicode.GetBytes(unicodeString);
            //再转化为GBK码
            byte[] gbkBytes = Encoding.Convert(unicode, gbk, unicodeBytes);
            while (i < gbkBytes.Length)
            {
                //如果为数字\字母\其他ASCII符号
                if (gbkBytes[i] <= 127)
                {
                    strResult = strResult + (char)gbkBytes[i];
                    i += 1;
                }
                #region 否则生成汉字拼音简码,取拼音首字母
                else
                {
                    key = (ushort)(gbkBytes[i] * 256 + gbkBytes[i + 1]);
                    if (key >= '\uB0A1' && key <= '\uB0C4')
                    {
                        strResult = strResult + "A";
                    }
                    else if (key >= '\uB0C5' && key <= '\uB2C0')
                    {
                        strResult = strResult + "B";
                    }
                    else if (key >= '\uB2C1' && key <= '\uB4ED')
                    {
                        strResult = strResult + "C";
                    }
                    else if (key >= '\uB4EE' && key <= '\uB6E9')
                    {
                        strResult = strResult + "D";
                    }
                    else if (key >= '\uB6EA' && key <= '\uB7A1')
                    {
                        strResult = strResult + "E";
                    }
                    else if (key >= '\uB7A2' && key <= '\uB8C0')
                    {
                        strResult = strResult + "F";
                    }
                    else if (key >= '\uB8C1' && key <= '\uB9FD')
                    {
                        strResult = strResult + "G";
                    }
                    else if (key >= '\uB9FE' && key <= '\uBBF6')
                    {
                        strResult = strResult + "H";
                    }
                    else if (key >= '\uBBF7' && key <= '\uBFA5')
                    {
                        strResult = strResult + "J";
                    }
                    else if (key >= '\uBFA6' && key <= '\uC0AB')
                    {
                        strResult = strResult + "K";
                    }
                    else if (key >= '\uC0AC' && key <= '\uC2E7')
                    {
                        strResult = strResult + "L";
                    }
                    else if (key >= '\uC2E8' && key <= '\uC4C2')
                    {
                        strResult = strResult + "M";
                    }
                    else if (key >= '\uC4C3' && key <= '\uC5B5')
                    {
                        strResult = strResult + "N";
                    }
                    else if (key >= '\uC5B6' && key <= '\uC5BD')
                    {
                        strResult = strResult + "O";
                    }
                    else if (key >= '\uC5BE' && key <= '\uC6D9')
                    {
                        strResult = strResult + "P";
                    }
                    else if (key >= '\uC6DA' && key <= '\uC8BA')
                    {
                        strResult = strResult + "Q";
                    }
                    else if (key >= '\uC8BB' && key <= '\uC8F5')
                    {
                        strResult = strResult + "R";
                    }
                    else if (key >= '\uC8F6' && key <= '\uCBF9')
                    {
                        strResult = strResult + "S";
                    }
                    else if (key >= '\uCBFA' && key <= '\uCDD9')
                    {
                        strResult = strResult + "T";
                    }
                    else if (key >= '\uCDDA' && key <= '\uCEF3')
                    {
                        strResult = strResult + "W";
                    }
                    else if (key >= '\uCEF4' && key <= '\uD188')
                    {
                        strResult = strResult + "X";
                    }
                    else if (key >= '\uD1B9' && key <= '\uD4D0')
                    {
                        strResult = strResult + "Y";
                    }
                    else if (key >= '\uD4D1' && key <= '\uD7F9')
                    {
                        strResult = strResult + "Z";
                    }
                    #region Special character process
                    else if (key == 60578)
                    {
                        strResult = strResult + "L";
                    }
                    else if (key == 57281)
                    {
                        strResult = strResult + "B";
                    }
                    else if (key == 57559)
                    {
                        strResult = strResult + "M";
                    }
                    else if (key == 58875)
                    {
                        strResult = strResult + "Y";
                    }
                    #endregion
                    else
                    {
                        strResult = strResult + "?";
                    }
                    i += 2;
                }

                #endregion
            }

            return strResult;
        }

        public static string SNN(string s) //StringNotNull
        {
            return (s != null) ? s : "";
        }
    }
}