using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Hosting;


using System.IO;

using NongYaoBackend.Models.Library;
using NongYaoBackend.Models.Library;

namespace NongYaoBackend.Models
{
    
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "原密码")]
        public string OldPassword { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        //[System.Web.Mvc.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "用户名 ")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码 ")]
        public string Password { get; set; }

        [Display(Name = "记住密码")]
        public string RememberMe { get; set; }
    }

    public class RegisterModel
    {
        NongYaoModelDataContext db = new NongYaoModelDataContext();

        public bool RegisterUser(string name, string userid, string password, string phonenum)
        {
            try
            {
                if (RegisterUniqueCheck(userid, 0))
                {
                    tbl_admin newitem = new tbl_admin();

                    newitem.name = name;
                    newitem.userid = userid;
                    newitem.password = password;
                    newitem.phone = phonenum;
                    newitem.regtime = DateTime.Now;
                    newitem.deleted = 0;

                    db.tbl_admins.InsertOnSubmit(newitem);
                    db.SubmitChanges();

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("RegisterModel", "RegisterUser()", e.ToString());
                return false;
            }

        }

        public bool RegisterUniqueCheck(string userid, long id)
        {
            try
            {
                var edititem = (from m in db.tbl_admins
                                where m.deleted == 0 && m.userid == userid && m.id != id
                                select m).FirstOrDefault();
                if (edititem == null) {

                    var useritem = (from m in db.tbl_users
                                    where m.deleted == 0 && m.userid == userid
                                    select m).FirstOrDefault();
                    if (useritem == null)
                    {
                        var shopitem = (from m in db.tbl_shops
                                        where m.deleted == 0 && m.userid == userid && m.pass != 2
                                        select m).FirstOrDefault();
                        if (shopitem == null)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;
                }
                else
                    return false;
                
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("RegisterModel", "RegisterUniqueCheck()", e.ToString());
                return false;
            }

        }

        public bool AddUser(string userid, string password, string username, long city, long district, string unit_name, string connect, byte roleinfo)
        {
            try
            {
                if (RegisterUniqueCheck(userid, 0))
                {
                    tbl_admin newitem = new tbl_admin();

                    newitem.name = username;
                    newitem.userid = userid;
                    //newitem.password = CommonModel.GetMD5Hash(password);
                    newitem.password = password;
                    newitem.phone = connect;
                    newitem.unitname = unit_name;
                    newitem.regtime = DateTime.Now;

                    if (roleinfo == 0)  //super role
                    {
                        newitem.role = 1;
                        newitem.regionid = city;
                    }
                    else if (roleinfo == 1) // city role
                    {
                        newitem.role = 2;
                        newitem.regionid = district;
                    }
                    newitem.deleted = 0;

                    db.tbl_admins.InsertOnSubmit(newitem);
                    db.SubmitChanges();

                    return true;
                }
                else
                    return false;

            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("AccountModel", "AddUser()", e.ToString());
                return false;
            }
        }

        public bool UpdateUser(long uid, string userid, string password, string username, long city, long district, string unit_name, string connect, byte roleinfo)
        {
            try
            {
                if (RegisterUniqueCheck(userid, uid))
                {
                    tbl_admin edititem = (from m in db.tbl_admins
                                          where m.id == uid && m.deleted == 0
                                          select m).FirstOrDefault();
                    if (edititem != null)
                    {

                        edititem.name = username;
                        edititem.userid = userid;
                        //if(password != edititem.password)
                        //    edititem.password = CommonModel.GetMD5Hash(password);
                        edititem.password = password;
                        edititem.phone = connect;
                        edititem.regtime = DateTime.Now;
                        edititem.unitname = unit_name;
                        if (roleinfo == 0)  //super role
                        {
                            //edititem.role = 1;
                            edititem.regionid = city;
                        }
                        else if (roleinfo == 1) // city role
                        {
                            //edititem.role = 2;
                            edititem.regionid = district;
                        }
                        edititem.deleted = 0;

                        db.SubmitChanges();

                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("AccountModel", "AddUser()", e.ToString());
                return false;
            }
        }
    }

    public class UserTableInfo
    {
        public long uid { get; set; }
        public string region { get; set; }
        public string unitname { get; set; }
        public string name { get; set; }
    }

    public class AccountModel
    {
        NongYaoModelDataContext db = new NongYaoModelDataContext();
        public static tbl_admin GetUserInfoById(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            return (from m in db.tbl_admins
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
        }
        public static long GetCurrentUserRegionId(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            tbl_admin admin_info = (from m in db.tbl_admins
                                    where m.deleted == 0 && m.id == id
                                    select m).FirstOrDefault();
            if (admin_info != null)
            {
                return (long)admin_info.regionid;
            }
            return 0;
        }
        public bool DeleteUser(string id)
        {
            try
            {
                long aid = Convert.ToInt64(id);
                var edititem = (from m in db.tbl_admins
                                where m.deleted == 0 && m.id == aid
                                select m).FirstOrDefault();
                if (edititem != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    edititem.deleted = 1;
                    db.SubmitChanges();
                    db.Transaction.Commit();
                    return true;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("SalesmanModel", "UpdateSalesman()", e.ToString());
                db.Transaction.Rollback();
                return false;
            }

            return false;
        }
        public static long GetCurrentUserUpperRegionId(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            tbl_admin admin_info = (from m in db.tbl_admins
                                    where m.deleted == 0 && m.id == id
                                    select m).FirstOrDefault();
            
            if (admin_info != null)
            {
                tbl_region region_info = (from m in db.tbl_regions
                                          where m.deleted == 0 && m.id == admin_info.regionid
                                          select m).FirstOrDefault();
                if (region_info != null)
                {
                    tbl_region upperregion = (from m in db.tbl_regions
                                              where m.deleted == 0 && m.id == region_info.parentid
                                              select m).FirstOrDefault();
                    if (upperregion != null)
                    {
                        return upperregion.id;
                    }
                }
            }
            return 0;
        }
        public static string GetUserRegionName(long id)
        {
            string ret = "";
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            tbl_admin admin_info =  (from m in db.tbl_admins
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
            if (admin_info != null)
            {
                long regionid = (long)admin_info.regionid;
                ret = AreaModel.GetRegionNameById(regionid);
            }
            return ret;
        }
        public static string GetDistrictBaseName(long id)
        {
            string ret = "";
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            tbl_admin admin_info = (from m in db.tbl_admins
                                    where m.deleted == 0 && m.id == id
                                    select m).FirstOrDefault();
            if (admin_info != null)
            {
                long regionid = (long)admin_info.regionid;
                ret = AreaModel.GetUpperRegionNameById(regionid);
            }
            return ret;
        }

        public JqDataTableInfo GetUserTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<UserTableInfo> userInfo;
            List<String[]> res = new List<String[]>();
            byte role = CommonModel.GetCurrentUserRole();
            long uid = CommonModel.GetCurrentUserId();
            long regionid = AreaModel.GetRegionIdByUserId(uid);
            userInfo = db.tbl_admins
                .Where(p => p.deleted == 0 && p.role == role + 1)
                .Join(db.tbl_regions, m=>m.regionid, l=>l.id , (m, l) => new {adminInfo = m, regionInfo = l})
                .Where(k=>(role == 1 && k.regionInfo.parentid == regionid) || role == 0)
                .Select(row => new UserTableInfo
                {
                    uid = row.adminInfo.id,
                    name = row.adminInfo.name,
                    unitname = row.adminInfo.unitname,
                    region = row.regionInfo.name
                }).ToList();

            var displayedSalesman = userInfo.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var i = 0;
            foreach (UserTableInfo c in displayedSalesman)
            {
                i = i + 1;
                var tmp = new[] { Convert.ToString(i), Convert.ToString(c.region), Convert.ToString(c.unitname), Convert.ToString(c.name), Convert.ToString(c.uid) };
                res.Add(tmp);
            }

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = userInfo.Count();
            rst.iTotalDisplayRecords = userInfo.Count();
            rst.aaData = res;
            return rst;
        }

        public int UpdateFailedCount(string addr, bool isFaild)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

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

        public tbl_admin ValidateUser(string username, string password)
        {
            tbl_admin userObj = GetUserObjByUserNameOrMailAddr(username, password);

            if (userObj != null)
                return userObj;
            return null;
        }

        public tbl_admin GetUserObjByUserNameOrMailAddr(string userName, string passWord)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            try
            {
                tbl_admin userinfo = (from m in db.tbl_admins
                                      where (m.userid.ToLower() == userName.ToLower() && m.password == passWord && m.deleted == 0)
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

        /*public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }*/

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public tbl_admin GetAdminInfo(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            return (from m in db.tbl_admins
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
        }

        public bool ChangePassword(long id, string new_pass)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            tbl_admin admin = (from m in db.tbl_admins
                             where m.deleted == 0 && m.id == id
                             select m).FirstOrDefault();

            if (admin != null)
            {
                admin.password = new_pass;
                db.SubmitChanges();

                return true;
            }

            return false;
        }
    }
}
