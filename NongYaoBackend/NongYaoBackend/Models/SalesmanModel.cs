using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using System.Web.Hosting;
using System.IO;

using NongYaoBackend.Models.Library;

namespace NongYaoBackend.Models
{
#region PackageModels
    public class SalesmanData
    {
        public long id { get; set; }
        public string uid { get; set; }
        public string permit_id{ get; set; }
        public string name{ get; set; }
        public string nickname{ get; set; }
        public long cityregion { get; set; }
        public long districtregion{ get; set; }
        public string addr{ get; set; }
        public string regtime{ get; set; }
        public string changetime{ get; set; }
        public string username{ get; set; }
        public string faren{ get; set; }
        public string mobile_phone{ get; set; }
        public string phone{ get; set; }
        public string qqnum{ get; set; }
        public string mailaddr{ get; set; }
        public byte pass{ get; set; }
        public byte level{ get; set; }
        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
        public string technical_manager{ get; set; }
        public string notice{ get; set; }

    }
    public class SalesmanTableInfo
    {
        public long id { get; set; }
        public string     uid { get; set; }
        public string   name { get; set; }
        public string   region { get; set; }
        public string   addr { get; set; }
        public string   username { get; set; }
        public byte     level { get; set; }
        public byte     pass { get; set; }
    }
    public class SalesmanIDInfo
    {
        public long uid { get; set; }
        public string name { get; set; }
    }
#endregion
    public class SalesmanModel
    {
        NongYaoModelDataContext db = new NongYaoModelDataContext();
        public JqDataTableInfo GetSalesmanTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long regionid, string search_word, byte search_pass)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<SalesmanTableInfo> salesmanInfo;
            List<String[]> res = new List<String[]>();

            
            if (search_pass == 3)
            {
                if (regionid == 0)
                {
                    salesmanInfo = db.tbl_shops
                        .Where(p => p.deleted == 0 && p.name.Contains(search_word) == true)
                        .Join(db.tbl_regions, m => m.region, l => l.id, (m, l) => new { shopInfo = m, regionInfo = l })
                        .Select(row => new SalesmanTableInfo
                        {
                            id = row.shopInfo.id,
                            uid = row.shopInfo.uid,
                            name = row.shopInfo.name,
                            region = row.regionInfo.name,
                            addr = row.shopInfo.addr,
                            username = row.shopInfo.username,
                            level = row.shopInfo.level,
                            pass = row.shopInfo.pass,
                        }).OrderByDescending(m => m.id).ToList();
                }
                else
                {
                    
                        salesmanInfo = db.tbl_shops
                            .Where(p => p.deleted == 0 && p.name.Contains(search_word) == true)
                            .Join(db.tbl_regions, m => m.region, l => l.id, (m, l) => new { shopInfo = m, regionInfo = l })
                            .Where(p => (p.regionInfo.parentid == regionid) || (p.regionInfo.parentid != 0 && p.regionInfo.id == regionid))
                            .Select(row => new SalesmanTableInfo
                            {
                                id = row.shopInfo.id,
                                uid = row.shopInfo.uid,
                                name = row.shopInfo.name,
                                region = row.regionInfo.name,
                                addr = row.shopInfo.addr,
                                username = row.shopInfo.username,
                                level = row.shopInfo.level,
                                pass = row.shopInfo.pass,
                            }).OrderByDescending(m => m.id).ToList();
                }
            }
            else
            {
                if (regionid == 0)
                {
                    salesmanInfo = db.tbl_shops
                    .Where(p => p.deleted == 0 && p.name.Contains(search_word) == true && p.pass == search_pass)
                    .Join(db.tbl_regions, m => m.region, l => l.id, (m, l) => new { shopInfo = m, regionInfo = l })
                    .Select(row => new SalesmanTableInfo
                    {
                        id = row.shopInfo.id,
                        uid = row.shopInfo.uid,
                        name = row.shopInfo.name,
                        region = row.regionInfo.name,
                        addr = row.shopInfo.addr,
                        username = row.shopInfo.username,
                        level = row.shopInfo.level,
                        pass = row.shopInfo.pass,
                    }).OrderByDescending(m => m.id).ToList();
                }
                else
                {
                    salesmanInfo = db.tbl_shops
                    .Where(p => p.deleted == 0 && p.name.Contains(search_word) == true && p.pass == search_pass)
                    .Join(db.tbl_regions, m => m.region, l => l.id, (m, l) => new { shopInfo = m, regionInfo = l })
                    .Where(p => (p.regionInfo.parentid == regionid) || (p.regionInfo.parentid != 0 && p.regionInfo.id == regionid))
                    .Select(row => new SalesmanTableInfo
                    {
                        id = row.shopInfo.id,
                        uid = row.shopInfo.uid,
                        name = row.shopInfo.name,
                        region = row.regionInfo.name,
                        addr = row.shopInfo.addr,
                        username = row.shopInfo.username,
                        level = row.shopInfo.level,
                        pass = row.shopInfo.pass,
                    }).OrderByDescending(m => m.id).ToList();                
                }
                
            }
            var displayedSalesman = salesmanInfo.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            foreach (SalesmanTableInfo c in displayedSalesman)
            {
                var tmp = new[] { (c.uid != null ? c.uid : "未定"), c.name, c.region, c.addr, c.username, Convert.ToString(c.level), Convert.ToString(c.pass), Convert.ToString(c.id) };
                res.Add(tmp);
            }
            rst.sEcho = param.sEcho;
            rst.iTotalRecords = salesmanInfo.Count();
            rst.iTotalDisplayRecords = salesmanInfo.Count();
            rst.aaData = res;
            return rst;
        }
        public SalesmanData GetSalesmanData(long id)
        {
            byte role = CommonModel.GetCurrentUserRole();
            SalesmanData salesmandata = (from m in db.tbl_shops
                                         where m.id == id && m.deleted == 0
                                         select new SalesmanData
                                         {
                                             id = m.id,
                                             uid = (m.uid != null ? m.uid : "未定"),
                                             permit_id = m.permit_id,
                                             name = m.name,
                                             nickname = m.nickname,
                                             cityregion = AreaModel.GetUpperRegionIdById((long)m.region),
                                             districtregion = (long)m.region,
                                             addr = m.addr,
                                             regtime =  m.regtime != null ? String.Format("{0:yyyy-MM-dd}", m.regtime) : "",
                                             changetime = String.Format("{0:yyyy-MM-dd}", DateTime.Now),
                                             username = m.username,
                                             faren = m.law_man,
                                             mobile_phone = m.mobile_phone,
                                             phone = m.phone,
                                             level = m.level,
                                             longitude = (m.longitude == null ? 0 : (decimal)m.longitude),
                                             latitude = (m.latitude == null ? 0 : (decimal)m.latitude),
                                             qqnum = m.qqnum,
                                             mailaddr = m.mailaddr,
                                             pass= m.pass,
                                             technical_manager = m.technical_manager,
                                             notice = m.notice,
                                         }).FirstOrDefault();
            
            return salesmandata;
        }
        public bool UpdateSalesman(string aid, string uid, string name, string nickname, long region, string addr,
            string username, string faren, string mobile_phone, string phone, string qqnum,
            string mailaddr, byte level, string longitude, string latitude, string technical_manager, string notice, string pass)
        {
            try
            {
                long shop_id = Convert.ToInt64(aid); 
                var edititem = (from m in db.tbl_shops
                                where m.deleted == 0 && m.id == shop_id
                                select m).FirstOrDefault();
                if (edititem != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    if (Convert.ToByte(pass) == 1)
                        edititem.uid = uid;
                    edititem.name = name;
                    edititem.nickname = nickname;
                    edititem.region = region;
                    edititem.addr = addr;
                    edititem.changetime = DateTime.Now; 
                    edititem.username = username;
                    edititem.law_man = faren;
                    edititem.mobile_phone = mobile_phone;
                    edititem.phone = phone;
                    edititem.qqnum = qqnum;
                    edititem.mailaddr = mailaddr;
                    edititem.level = level;
                    edititem.longitude = Convert.ToDecimal(longitude);
                    edititem.latitude = Convert.ToDecimal(latitude);
                    edititem.pass = Convert.ToByte(pass);
                    edititem.technical_manager = technical_manager;
                    edititem.notice = notice;
                    
                    //part of insert user table//
                    if (edititem.pass == 1)
                    {
                        long nuserRegisterId = 0;
                        List<tbl_user> userlist = (from m in db.tbl_users
                                                   select m).ToList();
                        foreach (tbl_user u in userlist)
                        {
                            if (nuserRegisterId < u.id)
                                nuserRegisterId = u.id;

                        }
                        nuserRegisterId++;
                        tbl_user newuser = new tbl_user
                        {
                            id = nuserRegisterId,
                            name = edititem.username,
                            userid = edititem.userid,
                            password = edititem.password,
                            phone = edititem.phone,
                            role = "admin",
                            shop_id = edititem.id,
                            regtime = DateTime.Now,
                        };
                        db.tbl_users.InsertOnSubmit(newuser);
                    }
                    db.SubmitChanges();
                    db.Transaction.Commit();
                    /////////////////////////////

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
        public bool DeleteSalesman(string id)
        {
            try
            {
                long aid = Convert.ToInt64(id);
                var edititem = (from m in db.tbl_shops
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

        public List<SalesmanIDInfo> GetSalesmanID(long region_id)
        {
            List<SalesmanIDInfo> ret = new List<SalesmanIDInfo>();
            List<tbl_shop> shop_info = (from m in db.tbl_shops
                                        where m.region == region_id
                                        select m).ToList();
            foreach (var item in shop_info)
            {
                SalesmanIDInfo sitem = new SalesmanIDInfo
                {
                   name = item.name,
                   uid = item.id
                };
                
                ret.Add(sitem);
            }
            return ret;
        }

        public static string GetShopArea(long shop_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            string retVal = "";
            tbl_shop catInfo = (from m in db.tbl_shops
                                   where m.deleted == 0 && m.id == shop_id
                                   select m).FirstOrDefault();
            if (catInfo != null)
            {
                tbl_region regInfo = (from m in db.tbl_regions
                                      where m.deleted == 0 && m.id == catInfo.region
                                      select m).FirstOrDefault();
                if (regInfo != null)
                    retVal = regInfo.name;
            }
            return retVal;
        }

        public static string GetShopName(long shop_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            string retVal = "";
            tbl_shop catInfo = (from m in db.tbl_shops
                                where m.deleted == 0 && m.id == shop_id
                                select m).FirstOrDefault();
            if (catInfo != null)
            {
                retVal = catInfo.name;
            }
            return retVal;
        }
    }
}