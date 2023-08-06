using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region shop_model
    public class Shop
    {
        public long id { get; set; }
        public string permitid { get; set; }
        public string name { get; set; }
        public string nickname { get; set; }
        public long region { get; set; }
        public string addr { get; set; }
        public string username { get; set; }
        public string mobile_phone { get; set; }
        public string phone { get; set; }
        public string mailaddr { get; set; }
        public string qqnum { get; set; }
        public byte level { get; set; }
        public byte pass { get; set; }
        public string managername { get; set; }
        public string userid { get; set; }
        public string password { get; set; }
        public DateTime? regtime { get; set; }
        public string uid { get; set; }
        public string law_man { get; set; }
        public string notice { get; set; }
        public string technical_manager { get; set; }
        public DateTime? changetime { get; set; }
    }

    public class SHOP_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "操作失败： 经销商名称重复！";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
    }
    #endregion

    public class ShopModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static bool IsConsistent(string name)
        {
            tbl_shop item = (from m in db.tbl_shops
                                where m.deleted == 0 && m.name == name
                                select m).FirstOrDefault();
            if (item != null)
                return false;
            else
                return
                    true;
        }

        public static string RegisterShop(string permitid, string shopname, string nickname, string region, string addr, string username, string mobile_phone, string phone, string mailaddr, string qqnum, byte level, string manager_name, string userid, string password, string lon, string lat)
        {
            try
            {
                if (IsConsistent(shopname))
                {
                    tbl_shop newitem = new tbl_shop();

                    newitem.permit_id = permitid;
                    newitem.name = shopname;
                    newitem.nickname = nickname;
                    newitem.region = long.Parse(region);
                    newitem.addr = addr;
                    newitem.username = username;
                    newitem.mobile_phone = mobile_phone;
                    newitem.phone = phone;
                    newitem.mailaddr = mailaddr;
                    newitem.qqnum = qqnum;
                    newitem.level = level;
                    newitem.manager_name = manager_name;
                    newitem.userid = userid;
                    newitem.password = password;
                    newitem.pass = 0;
                    newitem.longitude = Convert.ToDecimal(lon);
                    newitem.latitude = Convert.ToDecimal(lat);
                    newitem.regtime = DateTime.Now;
                    newitem.changetime = DateTime.Now;
                    newitem.law_man = "";
                    newitem.deleted = 0;

                    db.tbl_shops.InsertOnSubmit(newitem);

                    db.SubmitChanges();

                    return SHOP_SUBMITSTATUS.SUCCESS_SUBMIT;
                }
                else
                {
                    return SHOP_SUBMITSTATUS.DUPLICATE_NAME;
                }
                
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("ShopModel", "RegisterShop()", e.ToString());
                return SHOP_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public static bool UpdateShop(long shop_id, string nickname, long region, string addr, string username, string mobile_phone, string phone, string mailaddr, string qqnum)
        {
            try
            {
                tbl_shop edititem = (from m in db.tbl_shops
                                     where m.deleted == 0 && m.id == shop_id
                                     select m).FirstOrDefault();
                if (edititem != null)
                {
                    edititem.nickname = nickname;
                    edititem.region = region;
                    edititem.addr = addr;
                    edititem.username = username;
                    edititem.mobile_phone = mobile_phone;
                    edititem.phone = phone;
                    edititem.mailaddr = mailaddr;
                    edititem.qqnum = qqnum;
                }

                db.SubmitChanges();

                return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("ShopModel", "UpdateShop()", e.ToString());
                return false;
            }
        }

        public static Shop GetShopInfo(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            Shop retInfo = null;            

            retInfo = db.tbl_shops
                .Where(p => p.deleted == 0 && p.id == id && p.pass == 1)
                .Select(row => new Shop
                {
                    id = Convert.ToInt64(row.id),
                    permitid = Convert.ToString(row.permit_id),
                    name = Convert.ToString(row.name),
                    nickname = Convert.ToString(row.nickname),
                    region = Convert.ToInt64(row.region),
                    addr = Convert.ToString(row.addr),
                    username = Convert.ToString(row.username),
                    mobile_phone = Convert.ToString(row.mobile_phone),
                    phone = Convert.ToString(row.phone),
                    mailaddr = Convert.ToString(row.mailaddr),
                    qqnum = Convert.ToString(row.qqnum),
                    level = Convert.ToByte(row.level),
                    pass = Convert.ToByte(row.pass),
                    managername = Convert.ToString(row.manager_name),
                    userid = Convert.ToString(row.userid),
                    password = Convert.ToString(row.password),
                    regtime = Convert.ToDateTime(row.regtime),
                    uid = Convert.ToString(row.uid),
                    law_man = Convert.ToString(row.law_man),
                    notice = Convert.ToString(row.notice),
                    technical_manager = Convert.ToString(row.technical_manager),
                    changetime = Convert.ToDateTime(row.changetime)
                }).SingleOrDefault();

            return retInfo;
        }

        public static List<tbl_shop> GetShopListExcept(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            return (from m in db.tbl_shops
                    where m.deleted == 0 && m.id != id && m.pass == 1
                    select m).ToList();
        }

        public static List<tbl_shop> GetSaleShopList(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            List<tbl_ticket> ticket_list = (from m in db.tbl_tickets
                                            where m.shop_id == id && m.deleted == 0 && m.type == 2 && m.customer_type == 1
                                            select m).ToList();
            List<tbl_shop> other = new List<tbl_shop>();

            foreach (tbl_ticket m in ticket_list)
            {
                tbl_shop shop = (from l in db.tbl_shops
                                 where l.id == m.customer_id && l.deleted == 0
                                 select l).FirstOrDefault();
                if (shop != null)
                    other.Add(shop);
            }

            return other;
        }

        public static string GetShopNameForId(long id)
        {
            var item = GetShopInfo(id);
            if (item != null)
                return item.name;
            else
                return "";
        }

        public static string GetShopPhoneForId(long id)
        {
            var item = GetShopInfo(id);
            if (item != null)
                return item.phone;
            else
                return "";
        }
    }
}