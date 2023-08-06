using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region user_model
    
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string userid { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string[] roleSet { get; set; }
        public long index { get; set; }
    }

    public class USER_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "操作失败： 安装包名称重复！";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
    }

    #endregion

    public class UserModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static JqDataTableInfo GetUserListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<User> filteredCompanies;

            var alllist = GetUserList();

            filteredCompanies = alllist;

            var displayedUnit = filteredCompanies.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedUnit
                         select new[] {
                             Convert.ToString(c.index),
                             Convert.ToString(c.name),
                             Convert.ToString(c.userid),
                             Convert.ToString(c.phone),
                             Convert.ToString(c.role),
                             Convert.ToString(c.id)
                         };
            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = alllist.Count();
            rst.aaData = result;

            return rst;
        }

        public static IEnumerable<User> GetUserList()
        {
            List<User> retList = null;            

            long shopId = CommonModel.GetCurrentUserShopId();
            retList = db.tbl_users
                .Where(p => p.deleted == 0 && p.shop_id == shopId && p.role.Contains("admin") == false)
                .OrderByDescending(p => p.id)
                .Select(row => new User
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    userid = Convert.ToString(row.userid),
                    password = Convert.ToString(row.password),
                    phone = Convert.ToString(row.phone),
                    role = Convert.ToString(row.role),
                    roleSet = GetRoleSet(row.role)
                }).ToList();

            long index = 1;
            foreach (User usrItem in retList)
            {
                if (usrItem.role != null)
                {
                    string[] roleSet = usrItem.role.Split(',');
                    string convertRole = "";
                    string chineseName = "";
                    foreach (string strItem in roleSet)
                    {
                        switch (strItem)
                        {
                            case "buying":
                                chineseName = "进货";
                                break;
                            case "sale":
                                chineseName = "销售";
                                break;
                            case "store":
                                chineseName = "仓库";
                                break;
                            case "account":
                                chineseName = "财务";
                                break;
                            case "admin":
                                chineseName = "经销商";
                                break;
                        }
                        convertRole += "," + chineseName;
                    }
                    if (convertRole.CompareTo("") == 0)
                        usrItem.role = "";
                    else
                        usrItem.role = convertRole.Substring(1);
                }
                else
                {
                    usrItem.role = "";
                }
                usrItem.index = index;
                index++;
            }
            return retList;
        }

        public static IEnumerable<User> GetStoreManList()
        {
            List<User> retList = null;            
            long userId = CommonModel.GetCurrentUserShopId();
            retList = db.tbl_users
                .Where(p => p.deleted == 0 && p.role.Contains("store") == true && p.shop_id == userId)
                .Select(row => new User
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    userid = Convert.ToString(row.userid),
                    password = Convert.ToString(row.password),
                    phone = Convert.ToString(row.phone),
                    role = Convert.ToString(row.role),
                    roleSet = GetRoleSet(row.role)
                }).ToList();

            return retList;
        }

        public static string[] GetRoleSet(string role)
        {
            if (role != null)
                return role.Split(',');
            else
            {
                string[] roleSet = { "" };
                return roleSet;
            }

        }
        public static User GetUserInfo(long id)
        {
            User retInfo = null;            

            retInfo = db.tbl_users
                .Where(p => p.deleted == 0 && p.id == id)
                .Select(row => new User
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    userid = Convert.ToString(row.userid),
                    phone = Convert.ToString(row.phone),
                    password = Convert.ToString(row.password),
                    role = Convert.ToString(row.role),
                    roleSet = GetRoleSet(row.role)
                }).SingleOrDefault();

            
            return retInfo;
        }

        public string UpdateUserItem(long uid, string user_name, string user_id, string user_phone,string user_password, string user_role)
        {
            try
            {
                tbl_user edititem = (from m in db.tbl_users
                                     where m.deleted == 0 && m.id == uid
                                     select m).FirstOrDefault();
                if (edititem != null)
                {
                    edititem.name = user_name;
                    edititem.userid = user_id;
                    edititem.phone = user_phone;
                    edititem.password = user_password;
                    edititem.regtime = DateTime.Now;
                    edititem.role = user_role;
                }

                db.SubmitChanges();

                return USER_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "UpdateItem()", e.ToString());
                return USER_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public string InsertUserItem(string user_name, string user_id, string user_phone,string user_password, string user_role)
        {           
            try
            {
                tbl_user newitem = new tbl_user();

                newitem.name = user_name;
                newitem.userid = user_id;
                newitem.phone = user_phone;
                newitem.password = user_password;
                newitem.role = user_role;
                newitem.shop_id = CommonModel.GetCurrentUserShopId();
                newitem.regtime = DateTime.Now;
                db.tbl_users.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return USER_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "InsertItem()", e.ToString());
                return USER_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public bool RegisterUser(string name, string userid, string password, string role)
        {
            try
            {                
                tbl_user newitem = new tbl_user();

                newitem.name = name;
                newitem.userid = userid;
                newitem.password = password;
                newitem.role = role;
                newitem.regtime = DateTime.Now;
                newitem.deleted = 0;

                db.tbl_users.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "RegisterUser()", e.ToString());
                return false;
            }

        }

        public bool UserIdUniqueCheck(string userid)
        {
            try
            {
                var edititem = (from m in db.tbl_users
                                where m.deleted == 0 && m.userid == userid
                                select m).FirstOrDefault();
                if (edititem == null)
                {
                    var editshop = (from m in db.tbl_shops
                                    where m.deleted == 0 && m.userid == userid && m.pass != 2
                                    select m).FirstOrDefault();
                    if (editshop == null)
                    {
                        var adminitem = (from m in db.tbl_admins
                                         where m.deleted == 0 && m.userid == userid
                                         select m).FirstOrDefault();
                        if (adminitem == null)
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
                CommonModel.WriteLogFile("UserModel", "UserIdUniqueCheck()", e.ToString());
                return false;
            }
        }

        public bool DeleteUser(long id)
        {            
            var delitem = (from m in db.tbl_users
                           where m.deleted == 0 && m.id == id
                           select m).FirstOrDefault();

            if (delitem != null)
            {
                delitem.deleted = 1;
                db.SubmitChanges();
            }

            return true;
        }

        public static string GetPhoneForUsername(string username)
        {
            tbl_user userInfo = (from m in db.tbl_users
                                 where m.deleted == 0 && m.userid == username
                                 select m).FirstOrDefault();
            if (userInfo != null)
                return userInfo.phone;
            else
                return "";
        }

        public static string GetUsernameForPhone(string phone)
        {
            tbl_user userInfo = (from m in db.tbl_users
                                 where m.deleted == 0 && m.phone == phone
                                 select m).FirstOrDefault();
            if (userInfo != null)
                return userInfo.userid;
            else
                return "";
        }

        public static long GetUserId(string name, string phone)
        {
            tbl_user userInfo = (from m in db.tbl_users
                                 where m.deleted == 0 && m.userid == name && m.phone == phone
                                 select m).FirstOrDefault();
            if (userInfo != null)
                return userInfo.id;
            else
                return 0;
        }

        public static bool ChangePassword(long id, string new_pass)
        {
            tbl_user user = (from m in db.tbl_users
                             where m.deleted == 0 && m.id == id
                             select m).FirstOrDefault();

            if (user != null)
            {
                user.password = new_pass;
                db.SubmitChanges();
                return true;
            }
            return false;
        }

        public static tbl_user GetUserInfoFull(long id)
        {
            return (from m in db.tbl_users
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
        }

        public static bool DeleteUsersForShopId(long shop_id)
        {
            try
            {
                var items = from m in db.tbl_users
                            where m.deleted == 0 && m.shop_id == shop_id && m.role != "admin"
                            select m;
                foreach (var item in items) {
                    item.deleted = 1;
                    db.SubmitChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "DeleteUsersForShopId()", e.ToString());
                return false;
            }
        }

        public static tbl_user GetDelUser(string username, string password)
        {
            tbl_user userInfo = (from m in db.tbl_users
                                 where m.deleted == 1 && m.userid == username && m.password == password
                                 select m).FirstOrDefault();
            if (userInfo != null)
                return userInfo;
            else
                return null;
        }

        public static string GetUserNameForId(long id) 
        {
            var item = (from m in db.tbl_users
                            where m.deleted == 0 && m.id == id
                            select m).FirstOrDefault();
            if(item != null)
                return item.name;
            else
                return "";
        }
    }
}