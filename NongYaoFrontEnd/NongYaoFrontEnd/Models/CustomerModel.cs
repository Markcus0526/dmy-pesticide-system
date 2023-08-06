using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region customer_model
    public class Customer_Data
    {
        public string id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public DateTime? regtime { get; set; }
        public long index { get; set; }
    }

    public class CUSTOMER_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "操作失败： 安装包名称重复！";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
    }

    #endregion

    public class CustomerModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static bool isCustomer(string cName, string cPhone)
        {            
            tbl_customerlist customer = (from m in db.tbl_customerlists
                                         where m.deleted == 0 && m.name == cName && m.phone == cPhone
                                         select m).FirstOrDefault();
            if (customer == null)
                return false;
            else
                return true;
        }

        public static JqDataTableInfo GetCustomerListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, string searchWord)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<Customer_Data> filteredCompanies;

            var alllist = GetCustomerFilterList(searchWord);

            filteredCompanies = alllist;

            var displayedUnit = filteredCompanies.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedUnit
                         select new[] {
                             Convert.ToString(c.index),
                             Convert.ToString(c.name),
                             Convert.ToString(c.phone),
                             Convert.ToString(c.id)
                         };
            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = alllist.Count();
            rst.aaData = result;

            return rst;
        }

        public static IEnumerable<Customer_Data> GetCustomerList()
        {
            IEnumerable<Customer_Data> retList = null;
            
            long shopId = CommonModel.GetCurrentUserShopId();
            retList = db.tbl_customerlists
                .Where(p => p.deleted == 0 && p.shop_id == shopId)
                .OrderByDescending(p => p.id)
                .Select(row => new Customer_Data
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    phone = Convert.ToString(row.phone),
                    regtime = Convert.ToDateTime(row.regtime)
                }).ToList();

            return retList;
        }

        public static IEnumerable<Customer_Data> GetCustomerFilterList(string searchWord)
        {
            IEnumerable<Customer_Data> retList = null;
            
            if (searchWord.CompareTo("") == 0)
                retList = GetCustomerList();
            else
            {
                long shopId = CommonModel.GetCurrentUserShopId();
                retList = db.tbl_customerlists
                            .Where(p => p.deleted == 0 && p.name.Contains(searchWord) == true && p.shop_id == shopId)
                            .OrderByDescending(p => p.id)
                            .Select(row => new Customer_Data
                            {
                                id = Convert.ToString(row.id),
                                name = Convert.ToString(row.name),
                                phone = Convert.ToString(row.phone),
                                regtime = Convert.ToDateTime(row.regtime)
                            }).ToList();
            }
            long index = 1;
            foreach (Customer_Data cstItem in retList)
            {
                cstItem.index = index;
                index++;
            }

            return retList;
        }

        public static Customer_Data GetCustomerInfo(long id)
        {
            Customer_Data retInfo = null;
            
            retInfo = db.tbl_customerlists
                .Where(p => p.deleted == 0 && p.id == id)
                .Select(row => new Customer_Data
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    phone = Convert.ToString(row.phone),
                    regtime = Convert.ToDateTime(row.regtime)
                }).SingleOrDefault();

            return retInfo;
        }

        public string UpdateCustomerItem(long uid, string name, string phone)
        {
            try
            {
                tbl_customerlist edititem = (from m in db.tbl_customerlists
                                             where m.deleted == 0 && m.id == uid
                                             select m).FirstOrDefault();
                if (edititem != null)
                {
                    edititem.name = name;
                    edititem.phone = phone;
                    edititem.regtime = DateTime.Now;
                }

                db.SubmitChanges();

                return CUSTOMER_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("CustomerModel", "UpdateItem()", e.ToString());
                return CUSTOMER_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public bool DeleteCustomer(long id)
        {            
            var delitem = (from m in db.tbl_customerlists
                           where m.deleted == 0 && m.id == id
                           select m).FirstOrDefault();

            if (delitem != null)
            {
                delitem.deleted = 1;
                db.SubmitChanges();
            }

            return true;
        }

        public string InsertCustomer(string username, string phone)
        {
            try
            {
                tbl_customerlist newitem = new tbl_customerlist();

                newitem.name = username;
                newitem.phone = phone;
                newitem.regtime = DateTime.Now;
                newitem.shop_id = CommonModel.GetCurrentUserShopId();
                newitem.deleted = 0;

                db.tbl_customerlists.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return USER_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "InsertItem()", e.ToString());
                return USER_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public static string GetPhoneForUsername(string username)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            tbl_customerlist userInfo = (from m in db.tbl_customerlists
                                 where m.deleted == 0 && m.name == username && m.shop_id == shop_id
                                 select m).FirstOrDefault();
            if (userInfo != null)
                return userInfo.phone;
            else
                return "";
        }

        public static string GetUsernameForPhone(string phone)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();

            tbl_customerlist userInfo = (from m in db.tbl_customerlists
                                 where m.deleted == 0 && m.phone == phone && m.shop_id == shop_id
                                 select m).FirstOrDefault();
            if (userInfo != null)
                return userInfo.name;
            else
                return "";
        }

        public static long GetUserId(string name, string phone)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            tbl_customerlist userInfo = (from m in db.tbl_customerlists
                                 where m.deleted == 0 && m.name == name && m.phone == phone && m.shop_id == shop_id
                                 select m).FirstOrDefault();
            if (userInfo != null)
                return userInfo.id;
            else
                return 0;
        }

        public static string GetCustomerNameForId(long id)
        {
            var item = (from m in db.tbl_customerlists
                        where m.deleted == 0 && m.id == id
                        select m).FirstOrDefault();
            if (item != null)
                return item.name;
            else
                return "";
        }

        public static string GetCustomerPhoneForId(long id)
        {
            var item = (from m in db.tbl_customerlists
                        where m.deleted == 0 && m.id == id
                        select m).FirstOrDefault();
            if (item != null)
                return item.phone;
            else
                return "";
        }
    }
}