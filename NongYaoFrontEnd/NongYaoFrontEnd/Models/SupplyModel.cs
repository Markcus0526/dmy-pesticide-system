using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region supply_model
    public class Supply_Data
    {
        public string id { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public string addr { get; set; }
        public string region { get; set; }
        public string contact_name { get; set; }
        public string contact_mobilephone { get; set; }
        public string contact_phone { get; set; }
        public string qqnum { get; set; }
        public long index { get; set; }
    }

    public class SUPPLY_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "操作失败： 安装包名称重复！";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
    }
    #endregion

    public class SupplyModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();        

        public static JqDataTableInfo GetSupplyListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, string searchWord)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<Supply_Data> filteredCompanies;

            var alllist = GetSupplyFilterList(searchWord);

            filteredCompanies = alllist;

            var displayedUnit = filteredCompanies.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedUnit
                         select new[] {
                             Convert.ToString(c.index),
                             Convert.ToString(c.name),
                             Convert.ToString(c.region),
                             Convert.ToString(c.addr),
                             Convert.ToString(c.contact_name),
                             Convert.ToString(c.contact_mobilephone),
                             Convert.ToString(c.contact_phone),
                             Convert.ToString(c.id)
                         };
            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = alllist.Count();
            rst.aaData = result;

            return rst;
        }

        public static IEnumerable<Supply_Data> GetSupplyList()
        {
            IEnumerable<Supply_Data> retList = null;            
            long shopId = CommonModel.GetCurrentUserShopId();

            retList = db.tbl_supplies
                .Where(p => p.deleted == 0 && p.shop_id == shopId)
                .OrderByDescending(p => p.id)
                .Select(row => new Supply_Data
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    nickname = Convert.ToString(row.nickname),
                    addr = Convert.ToString(row.addr),
                    region = Convert.ToString(row.region),
                    contact_name = Convert.ToString(row.contact_name),
                    contact_phone = Convert.ToString(row.contact_phone),
                    contact_mobilephone = Convert.ToString(row.contact_mobilephone),
                    qqnum = Convert.ToString(row.qqnum)
                }).ToList();

            return retList;
        }

        public static IEnumerable<Supply_Data> GetSupplyFilterList(string searchWord)
        {
            IEnumerable<Supply_Data> retList = null;            
            if (searchWord.CompareTo("") == 0)
                retList = GetSupplyList();
            else
            {
                long shopId = CommonModel.GetCurrentUserShopId();
                retList = db.tbl_supplies
                .Where(p => p.deleted == 0 && p.name.Contains(searchWord) == true && p.shop_id == shopId)
                .OrderByDescending(p => p.id)
                .Select(row => new Supply_Data
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    nickname = Convert.ToString(row.nickname),
                    addr = Convert.ToString(row.addr),
                    region = Convert.ToString(row.region),
                    contact_name = Convert.ToString(row.contact_name),
                    contact_phone = Convert.ToString(row.contact_phone),
                    contact_mobilephone = Convert.ToString(row.contact_mobilephone),
                    qqnum = Convert.ToString(row.qqnum)
                }).ToList();
            }
            long index = 1;
            foreach (Supply_Data splItem in retList)
            {
                splItem.index = index;
                index++;
            }
            return retList;
        }

        public static Supply_Data GetSupplyInfo(long id)
        {
            Supply_Data retInfo = null;            

            retInfo = db.tbl_supplies
                .Where(p => p.deleted == 0 && p.id == id)
                .Select(row => new Supply_Data
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    nickname = Convert.ToString(row.nickname),
                    addr = Convert.ToString(row.addr),
                    region = Convert.ToString(row.region),
                    contact_name = Convert.ToString(row.contact_name),
                    contact_phone = Convert.ToString(row.contact_phone),
                    contact_mobilephone = Convert.ToString(row.contact_mobilephone),
                    qqnum = Convert.ToString(row.qqnum)
                }).SingleOrDefault();

            return retInfo;
        }

        public string UpdateSupplyItem(long supply_id, string supply_name, string supply_nickname, string region, string supply_addr, string supply_contactname,
                                            string supply_contactmobile, string supply_phone, string supply_qqnum)
        {
            try
            {
                tbl_supply edititem = (from m in db.tbl_supplies
                                       where m.deleted == 0 && m.id == supply_id
                                       select m).FirstOrDefault();
                if (edititem != null)
                {
                    edititem.name = supply_name;
                    edititem.nickname = supply_nickname;

                    edititem.region = region;
                    edititem.addr = supply_addr;
                    edititem.contact_name = supply_contactname;
                    edititem.contact_mobilephone = supply_contactmobile;
                    edititem.contact_phone = supply_phone;
                    edititem.regtime = DateTime.Now;
                    edititem.qqnum = supply_qqnum;
                }

                db.SubmitChanges();

                return SUPPLY_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("SupplyModel", "UpdateItem()", e.ToString());
                return SUPPLY_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public string InsertSupplyItem(string supply_name, string supply_nickname, string region, string supply_addr, string supply_contactname,
                                            string supply_contactmobile, string supply_phone, string supply_qqnum)
        {           

            try
            {
                tbl_supply newitem = new tbl_supply();

                newitem.name = supply_name;
                newitem.nickname = supply_nickname;
                newitem.region = region;
                newitem.addr = supply_addr;
                newitem.contact_name = supply_contactname;
                newitem.contact_mobilephone = supply_contactmobile;
                newitem.contact_phone = supply_phone;
                newitem.qqnum = supply_qqnum;
                newitem.shop_id = CommonModel.GetCurrentUserShopId();
                newitem.regtime = DateTime.Now;
                db.tbl_supplies.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return SUPPLY_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("SupplyModel", "InsertItem()", e.ToString());
                return SUPPLY_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public bool DeleteSupply(long id)
        {            
            var delitem = (from m in db.tbl_supplies
                           where m.deleted == 0 && m.id == id
                           select m).FirstOrDefault();

            if (delitem != null)
            {
                delitem.deleted = 1;
                db.SubmitChanges();
            }

            return true;
        }

        public static List<tbl_supply> GetSupplyList(long shop_id)
        {
            return (from m in db.tbl_supplies
                        where m.deleted == 0 && m.shop_id == shop_id
                        select m).ToList();
        }

        public static string GetSupplyNameFromId(long? supply_id)
        {
            tbl_supply item = (from m in db.tbl_supplies
                    where m.deleted == 0 && m.id == supply_id
                    select m).FirstOrDefault();

            if (item == null)
                return "";
            else
                return item.name;
        }

        public static bool DeleteSupplysForShopId(long shop_id)
        {
            try
            {
                var items = from m in db.tbl_supplies
                            where m.deleted == 0 && m.shop_id == shop_id
                            select m;
                foreach (var item in items)
                {
                    item.deleted = 1;
                    db.SubmitChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "DeleteForShopId()", e.ToString());
                return false;
            }
        }

        public static string GetSupplyPhoneForId(long id)
        {
            var item = (from m in db.tbl_supplies
                        where m.deleted == 0 && m.id == id
                        select m).FirstOrDefault();
            if (item != null)
                return item.contact_mobilephone;
            else
                return "";
        }
    }
}
