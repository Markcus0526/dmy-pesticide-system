using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;

namespace NongYaoFrontEnd.Models
{

    #region Models

    public class LogOnModel
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [DisplayName("用户名:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("密码:")]
        public string Password { get; set; }

        [DisplayName("记住密码")]
        public string RememberMe { get; set; }
    }

    #endregion



    public class Foreign_Servant
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "{0}至少为{1}位.";
        private readonly int _minCharacters = 1;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }

    public class AccountModel
    {
        private NongYaoModelDataContext db = new NongYaoModelDataContext();

        public int UpdateFailedCount(string addr, bool isFaild)
        {
            if (isFaild)
            {
                if (HttpContext.Current.Session[addr] == null)
                {
                    HttpContext.Current.Session.Add(addr, 1);

                    return 1;
                }
                else
                {
                    int faildcount = (int)HttpContext.Current.Session[addr];
                    faildcount++;
                    HttpContext.Current.Session[addr] = faildcount;

                    return faildcount;
                }
            }
            else
            {
                if (HttpContext.Current.Session[addr] != null)
                    HttpContext.Current.Session[addr] = 0;
            }
            
            return -1;
        }

        public tbl_user ValidateUser(string username, string password)
        {
            tbl_user userObj = GetUserObjByUserNameOrMailAddr(username, password);
            
            if (userObj != null)
                return userObj;
            return null;
        }

        public tbl_user GetUserObjByUserNameOrMailAddr(string userName, string passWord)
        {            
            try
            {
                tbl_user userinfo = (from m in db.tbl_users
                                      where (
                                        (m.userid.ToLower() == userName.ToLower() &&
                                        m.password == passWord && m.deleted == 0))
                                      select m).FirstOrDefault();

                if (userinfo != null)
                {
                    return userinfo;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile(this.GetType().Name, "GetUserObjByUserNameOrMailAddr()", e.ToString());
            }
            return null;
        }

        public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

}
