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
    public class ShopStatInfo
    {
        public long uid;
        public string permit_id;
        public string area;
        public string shop_name;
        public string law_man;
        public string phone_num;
        public long remain_count;
        public long sale_count;
        public decimal remain_quantity;
        public decimal sale_quantity;
        public decimal lon;
        public decimal lat;
    }
    public class CatalogStandardList
    {
        public long catalog_id;
        public long standard_id;
    }
    public class NongyaoStatInfo
    {
        public long uid;
        public string permit_id;
        public string name;
        public string product;
        public string standard;
        public int standard_id;
        public string largenumber;
        public long remain_count;
        public long sale_count;
        public decimal remain_quantity;
        public decimal sale_quantity;
    }
    public class DetailNongyaoStatInfo
    {
        public long uid;
        public string product_number;
        public string product_date;
        public int avail_date;
        public long remain_count;
        public long sale_count;
        public long sum_count;
        public string largenumber;
        public string area;
        public string salesman;
    }
    public class SalesmanListInfo
    {
        public long uid;
        public string name;
    }
    public class YearSaleCount
    {
        public long remainCount;
        public long allCount;
        public long saleCount;
        public decimal remainQuantity;
        public decimal allQuantity;
        public decimal saleQuantity;
    }
    public class AreaSaleCountInfo
    {
        public string area { get; set; }
        public long saleCount { get; set; }
        public long remainCount { get; set; }
    }
    public class MonthSaleCountInfo
    {
        public string Month { get; set; }
        public long saleCount { get; set; }
    }
    public class TypeSaleCountInfo
    {
        public string type { get; set; }
        public long saleCount { get; set; }
    }
    public class DateSaleCountInfo
    {
        public string area { get; set; }
        public long saleCount { get; set; }
    }
    public class StatisticsTableInfo
    {
        public long id { get; set; }
        public string shopname { get; set; }
        public string shopphone { get; set; }
        public long salecount { get; set; }
    }
    public class StatisticsModel
    {
        NongYaoModelDataContext db = new NongYaoModelDataContext();

#region ShopPart
        public JqDataTableInfo GetShopTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long shopRegionId, long shopId, DateTime shopStartTime, DateTime shopEndTime, byte level)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            List<String[]> res = new List<String[]>();
            List<ShopStatInfo> shopstatinfo = db.tbl_shops
                .Where(m=> m.deleted == 0 && m.pass == 1 && ((shopId == 0) || (shopId != 0 && m.id == shopId)) && ((level == 0) || (level != 0 && m.level == level)))
                .Join(db.tbl_regions, m => m.region, l => l.id, (m, l) => new { shopInfo = m, regionInfo = l })
                .Where(m => ((shopRegionId == 0 && m.regionInfo.parentid > 0 ) || (shopRegionId != 0 && ((m.regionInfo.parentid == shopRegionId) || (m.regionInfo.parentid != 0 && m.regionInfo.id == shopRegionId)))) )
                .Select(g => new ShopStatInfo{
                    uid = g.shopInfo.id,
                    permit_id = g.shopInfo.permit_id,
                    area = AreaModel.GetRegionNameById((long)g.shopInfo.region),
                    shop_name = g.shopInfo.name,
                    law_man = g.shopInfo.law_man,
                    phone_num = g.shopInfo.phone,
                    remain_count = GetShopRemainCount(g.shopInfo.id, shopStartTime, shopEndTime),
                    sale_count = GetShopSaleCount(g.shopInfo.id, shopStartTime, shopEndTime),
                    remain_quantity = GetShopRemainQuantity(g.shopInfo.id, shopStartTime, shopEndTime),
                    sale_quantity = GetShopSaleQuantity(g.shopInfo.id, shopStartTime, shopEndTime),
                    lon = (decimal)(g.shopInfo.longitude == null ? decimal.Parse("122.250431") : g.shopInfo.longitude),
                    lat = (decimal)(g.shopInfo.latitude == null ? decimal.Parse("43.659046") : g.shopInfo.latitude)
                }).ToList();

            var displayedSalesman = shopstatinfo.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            foreach (ShopStatInfo item in displayedSalesman)
            {
                var tmp = new[] {   Convert.ToString(item.permit_id), 
                                    Convert.ToString(item.area), 
                                    Convert.ToString(item.shop_name), 
                                    Convert.ToString(item.law_man), 
                                    Convert.ToString(item.phone_num), 
                                    //Convert.ToString(item.remain_count), 
                                    //Convert.ToString(item.sale_count), 
                                    Convert.ToString(item.remain_quantity) + "Kg", 
                                    Convert.ToString(item.sale_quantity) + "Kg", 
                                    Convert.ToString(item.uid),
                                    Convert.ToString(item.lon),
                                    Convert.ToString(item.lat)
                };
                res.Add(tmp);
            }

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = shopstatinfo.Count();
            rst.iTotalDisplayRecords = shopstatinfo.Count();
            rst.aaData = res;
            return rst;
        }

        public List<ShopStatInfo> GetShopData(long shopRegionId, long shopId, DateTime shopStartTime, DateTime shopEndTime, byte level)
        {
            List<ShopStatInfo> shopstatinfo = db.tbl_shops
                .Where(m => m.deleted == 0 && ((shopRegionId == 0) || (shopRegionId != 0 && m.region == shopRegionId)) && ((shopId == 0) || (shopId != 0 && m.id == shopId)) && ((level == 0) || (level != 0 && m.level == level)))
                .Select(g => new ShopStatInfo
                {
                    uid = g.id,
                    permit_id = g.permit_id,
                    area = AreaModel.GetRegionNameById((long)g.region),
                    shop_name = g.name,
                    law_man = g.law_man,
                    phone_num = g.phone,
                    remain_count = GetShopRemainCount(g.id, shopStartTime, shopEndTime),
                    sale_count = GetShopSaleCount(g.id, shopStartTime, shopEndTime)
                }).ToList();
            return shopstatinfo;
        }//for export excel...

        public long GetShopRemainCount(long shop_id, DateTime start_time, DateTime end_time)
        {
            long retVal = 0;
            var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                .Select(g => new
                {
                    quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                });
            if (remainList.Count() != 0)
            {
                retVal = remainList.Sum(m => m.quantity);
            }
            return retVal;
        }
        public long GetShopSaleCount(long shop_id, DateTime start_time, DateTime end_time)
        {
            long retVal = 0;
            long saleVal = 0;
            long salebackVal = 0;

            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.type == 2 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(l => l.catalog_cnt);
            if (saleList.Count() != 0)
            {
                saleVal = saleList.Sum();
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.type == 3 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(l => l.catalog_cnt);
            if (salebackList.Count() != 0)
            {
                salebackVal = salebackList.Sum();
            }
            retVal = saleVal - salebackVal;
            return retVal;
        }

        public decimal GetShopRemainQuantity(long shop_id, DateTime start_time, DateTime end_time)
        {
            decimal retVal = 0;
            var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                .Select(g => new
                {
                    standard_id = g.Key.standard_id,
                    quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                }).ToList();

            foreach(var m in remainList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if(standarditem != null)
                    standard = standarditem.quantity;
                retVal += m.quantity * standard / 1000;
            }
            return retVal;
        }
        public decimal GetShopSaleQuantity(long shop_id, DateTime start_time, DateTime end_time)
        {
            decimal retVal = 0;
            decimal saleVal = 0;
            decimal salebackVal = 0;

            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.type == 2 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(m => new { 
                    standard_id = m.standard_id,
                    quantity = m.catalog_cnt 
                }).ToList();
            foreach (var m in saleList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                saleVal += m.quantity * standard / 1000;
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.type == 3 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(m => new
                {
                    standard_id = m.standard_id,
                    quantity = m.catalog_cnt
                }).ToList();
            foreach (var m in salebackList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                salebackVal += m.quantity * standard / 1000;
            }
            retVal = saleVal - salebackVal;

            return retVal;
        }

        public List<SalesmanListInfo> GetSalesmanListFromPinyin(string basepinyin, long regionid)
        {
            if (regionid > 0)
            {
                List<SalesmanListInfo> slInfo = db.tbl_shops
                    .Where(m => m.deleted == 0 && m.pass == 1 && (m.permit_id.Contains(basepinyin) || m.nickname.Contains(basepinyin)))
                    .Join(db.tbl_regions, m => m.region, l => l.id, (m, l) => new { shopInfo = m, regionInfo = l })
                    .Where(p => (p.regionInfo.parentid == regionid) || (p.regionInfo.parentid != 0 && p.regionInfo.id == regionid))
                    .Select(l => new SalesmanListInfo
                    {
                        uid = l.shopInfo.id,
                        name = l.shopInfo.name,
                    }).ToList();
                return slInfo;
            }
            else
            {
                List<SalesmanListInfo> slInfo = db.tbl_shops
                    .Where(m => m.deleted == 0 && (m.permit_id.Contains(basepinyin) || m.nickname.Contains(basepinyin)))
                    .Select(l => new SalesmanListInfo
                    {
                        uid = l.id,
                        name = l.name,
                    }).ToList();
                return slInfo;
            }
        }

        public bool DeleteShop(string id)
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
                CommonModel.WriteLogFile("StatisticsModel", "DeleteShop()", e.ToString());
                db.Transaction.Rollback();
                return false;
            }

            return false;
        }
#endregion
#region Nongyao Part
        public JqDataTableInfo GetNongyaoTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long nongyaoRegionId, long nongyaoType, long nongyaoId, DateTime nongyaoStartTime, DateTime nongyaoEndTime)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            List<String[]> res = new List<String[]>();
            List<CatalogStandardList> catStdList = new List<CatalogStandardList>(); 
            List<NongyaoStatInfo> nongyaostatinfo = new List<NongyaoStatInfo>();
            
            if(nongyaoId != 0)

                nongyaostatinfo = db.tbl_remains
                    .Where(m=> m.deleted == 0)
                    .Join(db.tbl_catalogs, m=>m.catalog_id, l=>l.id, (m,l)=>new {remainInfo = m, catInfo = l})
                    .Where(m => m.catInfo.pass == 1 && m.catInfo.id == nongyaoId)
                    .Join(db.tbl_shops, m=>m.remainInfo.shop_id , l=>l.id , (m,l) => new{catInfo = m, shopInfo = l})
                    .Join(db.tbl_regions, m => m.shopInfo.region, l => l.id, (m, l) => new { catInfo = m, regionInfo = l })
                    .Where(p => ((nongyaoRegionId == 0 && p.regionInfo.parentid > 0) || (nongyaoRegionId > 0 && ((p.regionInfo.parentid == nongyaoRegionId) || (p.regionInfo.parentid != 0 && p.regionInfo.id == nongyaoRegionId)))))
                    .GroupBy(m => new { m.catInfo.catInfo.catInfo.id, m.catInfo.catInfo.remainInfo.standard_id })
                    .Select(g => new NongyaoStatInfo
                    {
                        uid = g.FirstOrDefault().catInfo.catInfo.catInfo.id,
                        permit_id = g.FirstOrDefault().catInfo.catInfo.catInfo.register_id,
                        name = g.FirstOrDefault().catInfo.catInfo.catInfo.name,
                        product = g.FirstOrDefault().catInfo.catInfo.catInfo.product,
                        standard = Convert.ToString(ProductModel.GetStandardText(g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id)),    //temp
                        standard_id = (int)g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id,
                        remain_count = GetNongyaoRemainCount(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime),
                        sale_count = GetNongyaoSaleCount(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime)
                    }).ToList();
            else
                
                nongyaostatinfo = db.tbl_remains
                    .Where(m => m.deleted == 0)
                    .Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Where(m => (m.catInfo.pass == 1 && ((nongyaoType == 0 && m.catInfo.kind > 0) || (nongyaoType != 0 && m.catInfo.kind == nongyaoType))))
                    .Join(db.tbl_shops, m => m.remainInfo.shop_id, l => l.id, (m, l) => new { catInfo = m, shopInfo = l })
                    .Join(db.tbl_regions, m => m.shopInfo.region, l => l.id, (m, l) => new { catInfo = m, regionInfo = l })
                    .Where(p => ((nongyaoRegionId == 0 && p.regionInfo.parentid > 0) || (nongyaoRegionId > 0 && ((p.regionInfo.parentid == nongyaoRegionId) || (p.regionInfo.parentid != 0 && p.regionInfo.id == nongyaoRegionId)))))
                    .GroupBy(m => new { m.catInfo.catInfo.catInfo.id, m.catInfo.catInfo.remainInfo.standard_id })
                    .Select(g => new NongyaoStatInfo
                    {
                        uid = g.FirstOrDefault().catInfo.catInfo.catInfo.id,
                        permit_id = g.FirstOrDefault().catInfo.catInfo.catInfo.register_id,
                        name = g.FirstOrDefault().catInfo.catInfo.catInfo.name,
                        product = g.FirstOrDefault().catInfo.catInfo.catInfo.product,
                        standard = Convert.ToString(ProductModel.GetStandardText(g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id)),    //temp
                        standard_id = (int)g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id,
                        remain_count = GetNongyaoRemainCount(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime),
                        sale_count = GetNongyaoSaleCount(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime),
                        remain_quantity = GetNongyaoRemainQuantity(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime),
                        sale_quantity = GetNongyaoSaleQuantity(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime)
                    }).ToList();

            
            var displayedNongyao = nongyaostatinfo.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            foreach (NongyaoStatInfo item in displayedNongyao)
            {
                var tmp = new[] { 
                    Convert.ToString(item.permit_id), 
                    Convert.ToString(item.name),
                    Convert.ToString(item.product), 
                    Convert.ToString(item.standard), 
                    Convert.ToString(item.remain_quantity) + "Kg", 
                    Convert.ToString(item.sale_quantity) + "Kg", 
                    Convert.ToString(item.uid), 
                    Convert.ToString(item.standard_id)
                };
                res.Add(tmp);
            }

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = nongyaostatinfo.Count();
            rst.iTotalDisplayRecords = nongyaostatinfo.Count();
            rst.aaData = res;
            return rst;
        }

        public List<NongyaoStatInfo> GetNongyaoData(long nongyaoRegionId, long nongyaoType, long nongyaoId, DateTime nongyaoStartTime, DateTime nongyaoEndTime)
        {
            List<NongyaoStatInfo> nongyaostatinfo = new List<NongyaoStatInfo>();
            if (nongyaoId != 0)
                nongyaostatinfo = db.tbl_remains
                    .Where(m => m.deleted == 0)
                    .Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Where(m => m.catInfo.pass == 1 && ( (nongyaoId == 0 && m.catInfo.kind > 0) || (nongyaoId != 0 && m.catInfo.kind == nongyaoId)))
                    .Join(db.tbl_shops, m => m.remainInfo.shop_id, l => l.id, (m, l) => new { catInfo = m, shopInfo = l })
                    .Where(k => (nongyaoRegionId == 0) || ((nongyaoRegionId != 0) && k.shopInfo.region == nongyaoRegionId))
                    .Join(db.tbl_nongyaos, m => m.catInfo.catInfo.kind, l => l.id, (m, l) => new { catInfo = m, nongInfo = l })
                    .GroupBy(m => new { m.catInfo.catInfo.catInfo.id, m.catInfo.catInfo.remainInfo.standard_id })
                    .Select(g => new NongyaoStatInfo
                    {
                        uid = g.FirstOrDefault().catInfo.catInfo.catInfo.id,
                        permit_id = g.FirstOrDefault().catInfo.catInfo.catInfo.permit_id,
                        name = g.FirstOrDefault().nongInfo.name,
                        product = g.FirstOrDefault().catInfo.catInfo.catInfo.product,
                        standard = Convert.ToString(ProductModel.GetStandardText(g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id)),    //temp
                        standard_id = (int)g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id,
                        remain_count = GetNongyaoRemainCount(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime),
                        sale_count = GetNongyaoSaleCount(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime)
                    }).OrderBy(l => l.uid).ToList();
            else
                nongyaostatinfo = db.tbl_remains
                    .Where(m => m.deleted == 0)
                    .Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Where(m => (nongyaoId == 0) || (nongyaoId != 0 && m.catInfo.kind == nongyaoId))
                    .Join(db.tbl_shops, m => m.remainInfo.shop_id, l => l.id, (m, l) => new { catInfo = m, shopInfo = l })
                    .Where(k => (nongyaoRegionId == 0) || ((nongyaoRegionId != 0) && k.shopInfo.region == nongyaoRegionId))
                    .Join(db.tbl_nongyaos, m => m.catInfo.catInfo.kind, l => l.id, (m, l) => new { catInfo = m, nongInfo = l })
                    .Where(l => l.nongInfo.parentid == nongyaoType)
                    .GroupBy(m => new { m.catInfo.catInfo.catInfo.id, m.catInfo.catInfo.remainInfo.standard_id })
                    .Select(g => new NongyaoStatInfo
                    {
                        uid = g.FirstOrDefault().catInfo.catInfo.catInfo.id,
                        permit_id = g.FirstOrDefault().catInfo.catInfo.catInfo.permit_id,
                        name = g.FirstOrDefault().nongInfo.name,
                        product = g.FirstOrDefault().catInfo.catInfo.catInfo.product,
                        standard = Convert.ToString(ProductModel.GetStandardText(g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id)),    //temp
                        standard_id = (int)g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id,
                        remain_count = GetNongyaoRemainCount(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime),
                        sale_count = GetNongyaoSaleCount(g.FirstOrDefault().catInfo.catInfo.remainInfo.catalog_id, g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id, nongyaoStartTime, nongyaoEndTime)
                    }).OrderBy(l => l.uid).ToList();

            return nongyaostatinfo;
        }
        public List<DetailNongyaoStatInfo> GetDetailNongyaoData(long catalog_id, DateTime nongyaoStartTime, DateTime nongyaoEndTime)
        {

            int standard_id = 0;
            tbl_catalog catInfo = (from m in db.tbl_catalogs
                                   where m.id == catalog_id && m.deleted == 0
                                   select m).FirstOrDefault();
            if (catInfo != null)
            {
                standard_id = (int)catInfo.standard;
            }
            List<DetailNongyaoStatInfo> nongyaostatdetailinfo = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.standard_id == standard_id)
                .Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                .GroupBy(m => new { m.remainInfo.catalog_id, m.remainInfo.store_id, m.remainInfo.standard_id, m.remainInfo.largenumber })
                .Select(g => new DetailNongyaoStatInfo
                {
                    uid = g.FirstOrDefault().catInfo.id,
                    product_number = g.FirstOrDefault().remainInfo.largenumber,//g.large,
                    product_date = Convert.ToString(g.FirstOrDefault().catInfo.regtime),
                    avail_date = (int)g.FirstOrDefault().catInfo.avail_date,
                    largenumber = g.FirstOrDefault().remainInfo.largenumber,
                    remain_count = GetDetailNongyaoRemainCount(catalog_id, g.FirstOrDefault().remainInfo.store_id, standard_id, g.FirstOrDefault().remainInfo.largenumber, nongyaoStartTime, nongyaoEndTime),
                    sale_count = GetDetailNongyaoSaleCount(catalog_id, g.FirstOrDefault().remainInfo.store_id, standard_id, g.FirstOrDefault().remainInfo.largenumber, nongyaoStartTime, nongyaoEndTime),
                    sum_count = GetDetailNongyaoSumCount(catalog_id, g.FirstOrDefault().remainInfo.store_id, standard_id, g.FirstOrDefault().remainInfo.largenumber, nongyaoStartTime, nongyaoEndTime),
                    area = SalesmanModel.GetShopArea((long)g.FirstOrDefault().catInfo.shop_id),
                    salesman = SalesmanModel.GetShopName((long)g.FirstOrDefault().catInfo.shop_id),
                }).OrderBy(l => l.uid).ToList();

            return nongyaostatdetailinfo;
        }
        public JqDataTableInfo GetDetailNongyaoTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long catalog_id, long standard_id, DateTime nongyaoStartTime, DateTime nongyaoEndTime)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            List<String[]> res = new List<String[]>();
            
            List<DetailNongyaoStatInfo> nongyaostatdetailinfo = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.standard_id == standard_id)
                .Join(db.tbl_catalogs, m=>m.catalog_id, l=>l.id, (m,l)=>new {remainInfo = m, catInfo = l})
                .GroupBy(m => new { m.remainInfo.catalog_id, m.remainInfo.store_id ,m.remainInfo.standard_id, m.remainInfo.largenumber})
                .Select(g => new DetailNongyaoStatInfo
                {
                    uid = g.FirstOrDefault().catInfo.id,
                    product_number = g.FirstOrDefault().remainInfo.largenumber,//g.large,
                    product_date = Convert.ToString(g.FirstOrDefault().catInfo.regtime),
                    avail_date = (int)g.FirstOrDefault().catInfo.avail_date,
                    largenumber = g.FirstOrDefault().remainInfo.largenumber,
                    remain_count = GetDetailNongyaoRemainCount(catalog_id, g.FirstOrDefault().remainInfo.store_id, standard_id, g.FirstOrDefault().remainInfo.largenumber, nongyaoStartTime, nongyaoEndTime),
                    sale_count = GetDetailNongyaoSaleCount(catalog_id, g.FirstOrDefault().remainInfo.store_id, standard_id, g.FirstOrDefault().remainInfo.largenumber, nongyaoStartTime, nongyaoEndTime),
                    sum_count = GetDetailNongyaoSumCount(catalog_id, g.FirstOrDefault().remainInfo.store_id, standard_id, g.FirstOrDefault().remainInfo.largenumber, nongyaoStartTime, nongyaoEndTime),
                    area = SalesmanModel.GetShopArea((long)g.FirstOrDefault().catInfo.shop_id),
                    salesman = SalesmanModel.GetShopName((long)g.FirstOrDefault().catInfo.shop_id),
                }).OrderBy(l=>l.uid).ToList();

            var displayedNongyao = nongyaostatdetailinfo.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            foreach (DetailNongyaoStatInfo item in displayedNongyao)
            {
                var tmp = new[] { Convert.ToString(item.product_number), Convert.ToString(item.product_date), Convert.ToString(item.avail_date), Convert.ToString(item.remain_count), Convert.ToString(item.sale_count), Convert.ToString(item.sum_count),Convert.ToString(item.area), Convert.ToString(item.salesman), Convert.ToString(item.uid) };
                res.Add(tmp);
            }

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = nongyaostatdetailinfo.Count();
            rst.iTotalDisplayRecords = nongyaostatdetailinfo.Count();
            rst.aaData = res;
            return rst;
        }
        public long GetNongyaoRemainCount(long catalog_id, long standard_id, DateTime start_time, DateTime end_time)
        {
            long retVal = 0;
            var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.standard_id == standard_id && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                .Select(g => new
                {
                    quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                });
            if (remainList.Count() != 0)
            {
                retVal = remainList.Sum(m => m.quantity);
            }
            return retVal;
        }
        public long GetNongyaoSaleCount(long catalog_id, long standard_id, DateTime start_time, DateTime end_time)
        {
            long retVal = 0;
            long saleVal = 0;
            long salebackVal = 0;

            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.standard_id == standard_id && m.type == 2 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(l => l.catalog_cnt);
            if (saleList.Count() != 0)
            {
                saleVal = saleList.Sum();
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.standard_id == standard_id && m.type == 3 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(l => l.catalog_cnt);
            if (salebackList.Count() != 0)
            {
                salebackVal = salebackList.Sum();
            }
            retVal = saleVal - salebackVal;
            return retVal;
        }
        public decimal GetNongyaoRemainQuantity(long catalog_id, long standard_id, DateTime start_time, DateTime end_time)
        {
            decimal retVal = 0;
            var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.standard_id == standard_id && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                .Select(g => new
                {
                    standard_id = g.Key.standard_id,
                    quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                });

            foreach (var m in remainList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                retVal += m.quantity * standard / 1000;
            }
            return retVal;
        }
        public decimal GetNongyaoSaleQuantity(long catalog_id, long standard_id, DateTime start_time, DateTime end_time)
        {
            decimal retVal = 0;
            decimal saleVal = 0;
            decimal salebackVal = 0;

            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.standard_id == standard_id && m.type == 2 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(m => new { 
                    standard_id = m.standard_id,
                    quantity = m.catalog_cnt
                });
            foreach (var m in saleList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                saleVal += m.quantity * standard / 1000;
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.standard_id == standard_id && m.type == 3 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(m => new { 
                    standard_id = m.standard_id,
                    quantity = m.catalog_cnt
                });
            foreach (var m in salebackList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                salebackVal += m.quantity * standard / 1000;
            }
            retVal = saleVal - salebackVal;
            return retVal;
        }
        public long GetDetailNongyaoSumCount(long catalog_id, long store_id, long standard_id, string large_number, DateTime start_time, DateTime end_time)
        {
            long retVal = 0;
            long remainCount = 0;
            long saleCount = 0;
            tbl_remain remainInfo = (from m in db.tbl_remains
                                     where m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == store_id && m.standard_id == standard_id && m.largenumber == large_number && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date
                                     orderby m.id descending
                                     select m).FirstOrDefault();
            if (remainInfo != null)
            {
                remainCount = remainInfo.quantity;
            }

            long saleVal = 0;
            long salebackVal = 0;

            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == store_id && m.standard_id == standard_id && m.largenumber == large_number && m.type == 2 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(l => l.catalog_cnt);
            if (saleList.Count() != 0)
            {
                saleVal = saleList.Sum();
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == store_id && m.standard_id == standard_id && m.largenumber == large_number && m.type == 3 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(l => l.catalog_cnt);
            if (salebackList.Count() != 0)
            {
                salebackVal = salebackList.Sum();
            }
            saleCount = saleVal - salebackVal;
            retVal = remainCount + saleCount;
            return retVal;
        }
        public long GetDetailNongyaoRemainCount(long catalog_id, long store_id, long standard_id, string large_number, DateTime start_time, DateTime end_time)
        {
            long retVal = 0;
            
            tbl_remain remainInfo = (from m in db.tbl_remains
                                     where m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == store_id && m.standard_id == standard_id && m.largenumber == large_number && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date
                          orderby m.id descending
                          select m).FirstOrDefault();
            if(remainInfo != null)
            {
                retVal = remainInfo.quantity;
            }

            return retVal;
        }
        public long GetDetailNongyaoSaleCount(long catalog_id, long store_id, long standard_id, string large_number, DateTime start_time, DateTime end_time)
        {
            long retVal = 0;
            long saleVal = 0;
            long salebackVal = 0;

            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == store_id && m.standard_id == standard_id && m.largenumber == large_number && m.type == 2 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(l => l.catalog_cnt);
            if (saleList.Count() != 0)
            {
                saleVal = saleList.Sum();
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == store_id && m.standard_id == standard_id && m.largenumber == large_number && m.type == 3 && m.regtime.Date >= start_time.Date && m.regtime.Date <= end_time.Date)
                .Select(l => l.catalog_cnt);
            if (salebackList.Count() != 0)
            {
                salebackVal = salebackList.Sum();
            }
            retVal = saleVal - salebackVal;
            return retVal;
        }

        public List<NongyaoListInfo> GetNongyaoListFromPinyin(string basepinyin, long nongType)
        {
            List<NongyaoListInfo> slInfo = db.tbl_catalogs
                .Where(m => m.deleted == 0 && m.pass == 1 && (m.nickname.Contains(basepinyin) || m.permit_id.Contains(basepinyin)) && ( ( nongType == 0 && m.kind > 0 ) || ( nongType > 0 && m.kind == nongType)))
                .Select(l => new NongyaoListInfo
                {
                    uid = l.id,
                    name = l.name,
                    usingname = (l.usingname != null)?l.usingname:"",
                }).ToList();
            return slInfo;
        }
#endregion
        public long GetSaleCount(String area, byte type)
        {
            long ret_quantity = 0;
            long region_id = (from m in db.tbl_regions
                              where m.name == area
                              select m).FirstOrDefault().id;
            List<tbl_nongyao> nongyao_list = (from m in db.tbl_nongyaos
                                              where m.parentid == type
                                              select m).ToList();
            foreach (tbl_nongyao item in nongyao_list)
            {
                ret_quantity += (long)(from m in db.tbl_salestatistics
                                 where m.region_id == region_id && m.nongyao_id == item.id
                                       select m).FirstOrDefault().quantity;
            }
            return ret_quantity;
        }
        public long GetSaleCount(DateTime time, byte type)
        {
            long ret_quantity = 0;
            
            List<tbl_nongyao> nongyao_list = (from m in db.tbl_nongyaos
                                              where m.parentid == type
                                              select m).ToList();
            foreach (tbl_nongyao item in nongyao_list)
            {
                ret_quantity += (long)(from m in db.tbl_salestatistics
                                       where m.regtime == time  && m.nongyao_id == item.id
                                       select m).FirstOrDefault().quantity;
            }
            return ret_quantity;
        }
        public long GetNongyaoRemainCountByRegion(long region_id,int year)
        {
            long retVal = 0;
            var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.regtime.Year <= year)
                .Join(db.tbl_shops, m=>m.shop_id, l=>l.id, (m,l)=>new{remainInfo = m, shop_info = l})
                .Where(k=>k.shop_info.region == region_id)
                .GroupBy(m => new {m.remainInfo.catalog_id, m.remainInfo.store_id, m.remainInfo.standard_id, m.remainInfo.largenumber })
                .Select(g => new
                {
                    quantity = g.OrderByDescending(l => l.remainInfo.id).FirstOrDefault().remainInfo.quantity
                });
            if (remainList.Count() != 0)
            {
                retVal = remainList.Sum(m => m.quantity);
            }
            return retVal;
        }
        public long GetNongyaoSaleCountByRegion(long region_id,int year)
        {
            long retVal = 0;
            long saleVal = 0;
            long salebackVal = 0;

            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Year == year)
                .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                .Where(k=>k.shop_info.region == region_id)
                .Select(l => l.remainInfo.catalog_cnt);
            if (saleList.Count() != 0)
            {
                saleVal = saleList.Sum();
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.type == 3 && m.regtime.Year == year)
                .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                .Where(k => k.shop_info.region == region_id)
                .Select(l => l.remainInfo.catalog_cnt);
            if (salebackList.Count() != 0)
            {
                salebackVal = salebackList.Sum();
            }
            retVal = saleVal - salebackVal;
            return retVal;
        }
        public decimal GetNongyaoRemainQuantityByRegion(long region_id, int year)
        {
            decimal retVal = 0;
            var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.regtime.Year <= year)
                .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                .Where(k => k.shop_info.region == region_id)
                .GroupBy(m => new { m.remainInfo.catalog_id, m.remainInfo.store_id, m.remainInfo.standard_id, m.remainInfo.largenumber })
                .Select(g => new
                {
                    standard_id = g.Key.standard_id,
                    quantity = g.OrderByDescending(l => l.remainInfo.id).FirstOrDefault().remainInfo.quantity
                });
            foreach (var m in remainList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                retVal += m.quantity * standard / 1000;
            }

            return retVal;
        }
        public decimal GetNongyaoSaleQuantityByRegion(long region_id, int year)
        {
            decimal retVal = 0;
            decimal saleVal = 0;
            decimal salebackVal = 0;

            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Year == year)
                .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                .Where(k => k.shop_info.region == region_id)
                .Select(m => new { 
                    standard_id = m.remainInfo.standard_id,
                    quantity = m.remainInfo.catalog_cnt
                });
            foreach (var m in saleList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                saleVal += m.quantity * standard / 1000;
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.type == 3 && m.regtime.Year == year)
                .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                .Where(k => k.shop_info.region == region_id)
                .Select(m => new { 
                    standard_id = m.remainInfo.standard_id,
                    quantity = m.remainInfo.catalog_cnt
                });
            foreach (var m in salebackList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                salebackVal += m.quantity * standard / 1000;
            }
            retVal = saleVal - salebackVal;

            return retVal;
        }

        public long GetNongyaoRemainCountByDetail(long region_id, DateTime startTime, DateTime endTime, long type_id, long nongyao_id)
        {
            long retVal = 0;
            IQueryable<long> remainList;
            if(nongyao_id != 0)
                remainList = db.tbl_remains
                    .Where(m => m.deleted == 0 && m.regtime.Date <= endTime.Date && m.regtime.Date >= startTime.Date)
                    .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                    .Where(k => k.shop_info.region == region_id)
                    .Join(db.tbl_catalogs, m => m.remainInfo.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Where(k => k.catInfo.kind == nongyao_id)
                    .GroupBy(m => new {m.remainInfo.remainInfo.catalog_id, m.remainInfo.remainInfo.store_id, m.remainInfo.remainInfo.standard_id, m.remainInfo.remainInfo.largenumber })
                    .Select(g => g.OrderByDescending(l => l.remainInfo.remainInfo.id).FirstOrDefault().remainInfo.remainInfo.quantity
                    );
            else
                remainList = db.tbl_remains
                    .Where(m => m.deleted == 0 && m.regtime.Date <= endTime.Date && m.regtime.Date >= startTime.Date)
                    .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                    .Where(k => k.shop_info.region == region_id)
                    .Join(db.tbl_catalogs, m => m.remainInfo.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Join(db.tbl_nongyaos, m => m.catInfo.kind, l => l.id, (m, l) => new { remainInfo = m, nongyaoInfo = l })
                    .Where(k => k.nongyaoInfo.parentid == type_id)
                    .GroupBy(m => new { m.remainInfo.remainInfo.remainInfo.catalog_id, m.remainInfo.remainInfo.remainInfo.store_id, m.remainInfo.remainInfo.remainInfo.standard_id, m.remainInfo.remainInfo.remainInfo.largenumber })
                    .Select(g => g.OrderByDescending(l => l.remainInfo.remainInfo.remainInfo.id).FirstOrDefault().remainInfo.remainInfo.remainInfo.quantity
                    );
            if (remainList.Count() != 0)
            {
                retVal = remainList.Sum();
            }
            return retVal;
        }
        public long GetNongyaoSaleCountByDetail(long region_id, DateTime startTime, DateTime endTime, long type_id, long nongyao_id)
        {
            long retVal = 0;
            long saleVal = 0;
            long salebackVal = 0;
            IQueryable<int> saleList;
            IQueryable<int> salebackList;
            if(nongyao_id != 0)
                saleList = db.tbl_remains
                    .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Date <= endTime.Date && m.regtime.Date >= startTime.Date)
                    .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                    .Where(k => k.shop_info.region == region_id)
                    .Join(db.tbl_catalogs, m => m.remainInfo.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Where(k => k.catInfo.kind == nongyao_id)
                    .Select(l => l.remainInfo.remainInfo.catalog_cnt);
            else
                saleList = db.tbl_remains
                    .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Date <= endTime.Date && m.regtime.Date >= startTime.Date)
                    .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                    .Where(k => k.shop_info.region == region_id)
                    .Join(db.tbl_catalogs, m => m.remainInfo.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Join(db.tbl_nongyaos, m => m.catInfo.kind, l => l.id, (m, l) => new { remainInfo = m, nongyaoInfo = l })
                    .Where(k => k.nongyaoInfo.parentid == type_id)
                    .Select(l => l.remainInfo.remainInfo.remainInfo.catalog_cnt);
            if (saleList.Count() != 0)
            {
                saleVal = saleList.Sum();
            }

            if (nongyao_id != 0)
                salebackList = db.tbl_remains
                    .Where(m => m.deleted == 0 && m.type == 3 && m.regtime.Date <= endTime.Date && m.regtime.Date >= startTime.Date)
                    .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                    .Where(k => k.shop_info.region == region_id)
                    .Join(db.tbl_catalogs, m => m.remainInfo.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Where(k => k.catInfo.kind == nongyao_id)
                    .Select(l => l.remainInfo.remainInfo.catalog_cnt);
            else
                salebackList = db.tbl_remains
                    .Where(m => m.deleted == 0 && m.type == 3 && m.regtime.Date <= endTime.Date && m.regtime.Date >= startTime.Date)
                    .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                    .Where(k => k.shop_info.region == region_id)
                    .Join(db.tbl_catalogs, m => m.remainInfo.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Join(db.tbl_nongyaos, m => m.catInfo.kind, l => l.id, (m, l) => new { remainInfo = m, nongyaoInfo = l })
                    .Where(k => k.nongyaoInfo.parentid == type_id)
                    .Select(l => l.remainInfo.remainInfo.remainInfo.catalog_cnt);
            if (salebackList.Count() != 0)
            {
                salebackVal = salebackList.Sum();
            }
            retVal = saleVal - salebackVal;
            return retVal;
        }
        public decimal GetNongyaoSaleCountByMonth(int year, int Month)
        {
            decimal retVal = 0;
            decimal saleVal = 0;
            decimal salebackVal = 0;

            var saleList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Year == year && m.regtime.Month == Month)
                        .Select(m => new
                        {
                            standard_id = m.standard_id,
                            quantity = m.catalog_cnt
                        }).ToList();
            foreach (var m in saleList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                saleVal += m.quantity * standard / 1000;
            }
            var salebackList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.type == 3 && m.regtime.Year == year && m.regtime.Month == Month)
                        .Select(m => new
                        {
                            standard_id = m.standard_id,
                            quantity = m.catalog_cnt
                        });
            foreach (var m in salebackList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                salebackVal += m.quantity * standard / 1000;
            }
            retVal = saleVal - salebackVal;

            /* Fix Me
            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Year == year && m.regtime.Month == Month)
                .Select(l => l.catalog_cnt);
            if (saleList.Count() != 0)
            {
                saleVal = saleList.Sum();
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.type == 3 && m.regtime.Year == year && m.regtime.Month == Month)
                .Select(l => l.catalog_cnt);
            if (salebackList.Count() != 0)
            {
                salebackVal = salebackList.Sum();
            }
            retVal = saleVal - salebackVal;
             * */
            return retVal;
        }
        public long GetNongyaoSaleCountByType(int year, long type_id)
        {
            long retVal = 0;
            long saleVal = 0;
            long salebackVal = 0;

            var saleList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Year == year)
                .Join(db.tbl_catalogs, m=>m.catalog_id, l=>l.id, (m,l)=>new {remainInfo = m, catInfo = l})
                .Join(db.tbl_nongyaos, m=>m.catInfo.kind, l=>l.id, (m,l)=>new {remainInfo = m, nongyaoInfo = l})
                .Where(k=>k.nongyaoInfo.parentid == type_id || k.nongyaoInfo.id == type_id)
                .Select(l => l.remainInfo.remainInfo.catalog_cnt);
            if (saleList.Count() != 0)
            {
                saleVal = saleList.Sum();
            }

            var salebackList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.type == 3 && m.regtime.Year == year)
                .Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                .Join(db.tbl_nongyaos, m => m.catInfo.kind, l => l.id, (m, l) => new { remainInfo = m, nongyaoInfo = l })
                .Where(k => k.nongyaoInfo.parentid == type_id)
                .Select(l => l.remainInfo.remainInfo.catalog_cnt);
            if (salebackList.Count() != 0)
            {
                salebackVal = salebackList.Sum();
            }
            retVal = saleVal - salebackVal;
            return retVal;
        }
        public List<AreaSaleCountInfo> GetAreaSaleCount(string year,int scrollCount)
        {
            List<AreaSaleCountInfo> retValue = new List<AreaSaleCountInfo>();
            List<tbl_region> regions = (from m in db.tbl_regions
                                       where m.deleted == 0 && m.parentid != 0
                                       select m).ToList();
            var regions_scroll = regions.Skip(scrollCount).Take(10);
            foreach (tbl_region region in regions_scroll)
            {
                AreaSaleCountInfo additem = new AreaSaleCountInfo();
                List<tbl_nongyao> nongyao_list = new List<tbl_nongyao>();
                int year_val = Convert.ToInt32(year);
                //long sale_quantity = 0;
                //long remain_quantity = 0;
                //sale_quantity = GetNongyaoSaleCountByRegion(region.id, year_val);
                //remain_quantity = GetNongyaoRemainCountByRegion(region.id, year_val);
                decimal sale_quantity = 0;
                decimal remain_quantity = 0;
                sale_quantity = GetNongyaoSaleQuantityByRegion(region.id, year_val);
                remain_quantity = GetNongyaoRemainQuantityByRegion(region.id, year_val);
                
                additem.area = region.name;
                //additem.saleCount = sale_quantity;
                //additem.remainCount = remain_quantity;
                additem.saleCount = (long)Math.Round(sale_quantity);
                additem.remainCount = (long)Math.Round(remain_quantity);
                retValue.Add(additem);
            }
            return retValue;
        }

        public List<AreaSaleCountInfo> GetDetailAreaSaleCount(DateTime startTime, DateTime endTime, long type_id, long nongyao_id, int scrollCount)
        {
            List<AreaSaleCountInfo> retValue = new List<AreaSaleCountInfo>();
            List<tbl_region> regions = (from m in db.tbl_regions
                                        where m.deleted == 0 && m.parentid != 0
                                        select m).ToList();
            var regions_scroll = regions.Skip(scrollCount).Take(10);
            foreach (tbl_region region in regions_scroll)
            {
                long sale_quantity = 0;
                long remain_quantity = 0;
                AreaSaleCountInfo additem = new AreaSaleCountInfo();
                List<tbl_nongyao> nongyao_list = new List<tbl_nongyao>();
                
                sale_quantity = GetNongyaoSaleCountByDetail(region.id, startTime, endTime, type_id, nongyao_id);
                remain_quantity = GetNongyaoRemainCountByDetail(region.id, startTime, endTime, type_id, nongyao_id);

                additem.area = region.name;
                additem.saleCount = sale_quantity;
                additem.remainCount = remain_quantity;
                retValue.Add(additem);
            }
            return retValue;
        }

        public List<MonthSaleCountInfo> GetMonthSaleCount(string year)
        {
            List<MonthSaleCountInfo> retValue = new List<MonthSaleCountInfo>();
            List<tbl_region> regions = (from m in db.tbl_regions
                                        where m.deleted == 0
                                        select m).ToList();
            for (int i = 1; i <= 12; i++ )
            {
                decimal sale_quantity = 0;
                MonthSaleCountInfo additem = new MonthSaleCountInfo();
                List<tbl_nongyao> nongyao_list = new List<tbl_nongyao>();
                int year_val = Convert.ToInt32(year);
                sale_quantity = GetNongyaoSaleCountByMonth(year_val, i);

                additem.Month = i + "月";
                additem.saleCount = (long)Math.Round(sale_quantity);
                retValue.Add(additem);
            }
            return retValue;
        }
        public List<TypeSaleCountInfo> GetTypeSaleCount(string year)
        {
            List<TypeSaleCountInfo> retValue = new List<TypeSaleCountInfo>();
            List<tbl_nongyao> nongyaos = (from m in db.tbl_nongyaos
                                        where m.deleted == 0 && m.parentid == 0
                                        select m).ToList();
            foreach (var item in nongyaos)
            {
                long sale_quantity = 0;
                TypeSaleCountInfo additem = new TypeSaleCountInfo();
                List<tbl_nongyao> nongyao_list = new List<tbl_nongyao>();
                int year_val = Convert.ToInt32(year);
                sale_quantity = GetNongyaoSaleCountByType(year_val, item.id);

                additem.type = item.name;
                additem.saleCount = sale_quantity;
                retValue.Add(additem);
            }
            return retValue;
        }
        public YearSaleCount GetYearSaleCount(string year)
        {
            YearSaleCount retValue = new YearSaleCount();
            int yearVal = Convert.ToInt32(year);
            var remainList = db.tbl_remains
                .Where(m=>m.deleted == 0 && m.regtime.Year <= yearVal)
                .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                .Select(g => new
                {
                    standard_id = g.Key.standard_id,
                    quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                });
            if (remainList.Count() != 0)
                retValue.remainCount = remainList.Sum(m => m.quantity);
            decimal remainQuantity = 0;
            foreach (var m in remainList)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                remainQuantity += m.quantity * standard / 1000;
            }
            retValue.remainQuantity = remainQuantity;

            long saleCount = 0;
            List<tbl_remain> remainInfo = (from m in db.tbl_remains
                                           where m.deleted == 0 && m.regtime.Year == yearVal && m.type == 2
                                           select m).ToList();
            if(remainInfo.Count() != 0)
            { 
                saleCount = remainInfo.Sum(k => k.catalog_cnt);
            }

            decimal saleQuantity = 0;
            foreach (var m in remainInfo)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                saleQuantity += m.catalog_cnt * standard / 1000;
            }

            long salebackCount = 0;
            List<tbl_remain> remainInfo1 = (from m in db.tbl_remains
                                           where m.deleted == 0 && m.regtime.Year == yearVal && m.type == 3
                                           select m).ToList();
            if (remainInfo1.Count() != 0)
            {
                salebackCount = remainInfo1.Sum(k => k.catalog_cnt);
            }
            
            decimal salebackQuantity = 0;
            foreach (var m in remainInfo1)
            {
                var standarditem = db.tbl_standards
                    .Where(l => l.deleted == 0 && l.id == m.standard_id)
                    .FirstOrDefault();
                decimal standard = 0;
                if (standarditem != null)
                    standard = standarditem.quantity;
                salebackQuantity += m.catalog_cnt * standard / 1000;
            }

            retValue.saleQuantity = saleQuantity - salebackQuantity;
            retValue.allQuantity = retValue.remainQuantity + retValue.saleQuantity;

            retValue.saleCount = saleCount - salebackCount;
            retValue.allCount = retValue.remainCount + retValue.saleCount;

            return retValue;
        }
        public List<AreaSaleCountInfo> GetTaiTypeCount(long area_id, DateTime start_date, DateTime end_date, long nongyao_id)
        {
            List<AreaSaleCountInfo> retValue = new List<AreaSaleCountInfo>();
            List<tbl_nongyao> tmp_nongyaos = (from m in db.tbl_nongyaos
                                        where m.deleted == 0 && m.parentid == 0
                                        select m).ToList();
            
            foreach (tbl_nongyao item in tmp_nongyaos)
            {
                List<tbl_nongyao> nongyaos = (from m in db.tbl_nongyaos
                                              where m.deleted == 0 && m.parentid == item.id
                                              select m).ToList();
                long ret_quantity = 0;
                AreaSaleCountInfo additem = new AreaSaleCountInfo();
                foreach (tbl_nongyao item1 in nongyaos)
                {
                    List<tbl_salestatistic> q_items = db.tbl_salestatistics
                            .Where(m => m.region_id == area_id && m.nongyao_id == item1.id && ((DateTime)m.regtime).Date >= start_date.Date && ((DateTime)m.regtime).Date <= end_date.Date && m.deleted == 0)
                            .Select(row => row).ToList();
                    ret_quantity += (long)q_items.Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { stat = m, catalog = l })
                        .Sum(m => m.stat.quantity * m.catalog.standard);
                }
                
                additem.area = item.name;
                additem.saleCount = ret_quantity;
                retValue.Add(additem);
            }
            
            return retValue;
        }
        public List<AreaSaleCountInfo> GetXiaoTypeCount(long area_id, DateTime start_date, DateTime end_date, long nongyao_id)
        {
            List<AreaSaleCountInfo> retValue = new List<AreaSaleCountInfo>();
            List<tbl_nongyao> tmp_nongyaos = (from m in db.tbl_nongyaos
                                              where m.deleted == 0 && m.parentid == nongyao_id
                                              select m).ToList();

            foreach (tbl_nongyao item in tmp_nongyaos)
            {
                List<tbl_nongyao> nongyaos = (from m in db.tbl_nongyaos
                                              where m.deleted == 0 && m.parentid == item.id
                                              select m).ToList();
                long ret_quantity = 0;
                AreaSaleCountInfo additem = new AreaSaleCountInfo();
                
                List<tbl_salestatistic> q_items = db.tbl_salestatistics
                        .Where(m => m.region_id == area_id && m.nongyao_id == item.id && ((DateTime)m.regtime).Date >= start_date.Date && ((DateTime)m.regtime).Date <= end_date.Date && m.deleted == 0)
                        .Select(row => row).ToList();
                ret_quantity = (long)q_items.Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { stat = m, catalog = l })
                    .Sum(m => m.stat.quantity * m.catalog.standard);

                additem.area = item.name;
                additem.saleCount = ret_quantity;
                retValue.Add(additem);
            }

            return retValue;
        }

        public List<DateSaleCountInfo> GetDateSaleCount(long area_id,long shop_id, byte type,long nongyao_id, DateTime start_date,DateTime end_date)
        {
            List<DateSaleCountInfo> retValue = new List<DateSaleCountInfo>();
            List<tbl_region> regions = (from m in db.tbl_regions
                                        where m.deleted == 0
                                        select m).ToList();
            DateTime c_start_date = start_date;
            TimeSpan diff = end_date - start_date;
            if (diff.Days > 30)
            {
                c_start_date = end_date.AddDays(-30);
            }
            for (DateTime cur_date = c_start_date; cur_date<=end_date; cur_date = cur_date.AddDays(1))
            {
                long ret_quantity = 0;
                DateSaleCountInfo additem = new DateSaleCountInfo();
                List<tbl_nongyao> nongyao_list = new List<tbl_nongyao>();
                if (type == 0)
                {
                    List<tbl_salestatistic> q_items = db.tbl_salestatistics
                        .Where(m => m.region_id == area_id && ((shop_id == 0) ? true : m.shop_id == shop_id) && ((DateTime)m.regtime).Date == cur_date.Date && m.deleted == 0)
                        .Join(db.tbl_nongyaos, m => m.nongyao_id, l => l.id, (m, l) => new { stat = m, nongyao = l })
                        .Where(m => m.nongyao.deleted == 0)
                        .Select(row=>row.stat).ToList();
                    ret_quantity = (long)q_items.Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { stat = m, catalog = l })
                        .Sum(m => m.stat.quantity * m.catalog.standard);
                    
                }
                else if (type == 1)
                {
                    List<tbl_salestatistic> q_items = db.tbl_salestatistics
                        .Where(m => m.region_id == area_id && ((shop_id == 0) ? true : m.shop_id == shop_id) && ((DateTime)m.regtime).Date == cur_date.Date && m.deleted == 0)
                        .Join(db.tbl_nongyaos, m => m.nongyao_id, l => l.id, (m, l) => new { stat = m, nongyao = l })
                        .Where(m => m.nongyao.deleted == 0 && m.nongyao.parentid == 0)
                        .Select(row => row.stat).ToList();
                    ret_quantity = (long)q_items.Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { stat = m, catalog = l })
                        .Sum(m => m.stat.quantity * m.catalog.standard);
                }
                else if (type == 2)
                {
                    List<tbl_salestatistic> q_items = db.tbl_salestatistics
                        .Where(m => m.region_id == area_id && ((shop_id == 0) ? true : m.shop_id == shop_id) && ((DateTime)m.regtime).Date == cur_date.Date && m.deleted == 0)
                        .Join(db.tbl_nongyaos, m => m.nongyao_id, l => l.id, (m, l) => new { stat = m, nongyao = l })
                        .Where(m => m.nongyao.deleted == 0 && m.nongyao.parentid != 0)
                        .Select(row => row.stat).ToList();
                    ret_quantity = (long)q_items.Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { stat = m, catalog = l })
                        .Sum(m => m.stat.quantity * m.catalog.standard);
                }
                    
                
                additem.area = String.Format("{0:yyyy-MM-dd}",cur_date);
                additem.saleCount = ret_quantity;
                retValue.Add(additem);
            }
            
            return retValue;
        }

        public JqDataTableInfo GetStatisticsTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long region_id, long salesman_id, DateTime start_time, DateTime end_time, byte nongyao_type, long nongyao_id)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            List<String[]> res = new List<String[]>();
            IEnumerable<StatisticsTableInfo> stInfo = new List<StatisticsTableInfo>();
            
            DateTime start_date = start_time;
            DateTime end_date = end_time;
            if (nongyao_type == 0) //quandou
            {
                var stInfo1 = db.tbl_salestatistics
                .Where(m => m.deleted == 0 && ((DateTime)m.regtime).Date >= start_date.Date && ((DateTime)m.regtime).Date <= end_date.Date && ((region_id == 0) ? true : m.region_id == region_id) && ((salesman_id == 0) ? true : m.shop_id == salesman_id))
                .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { stat_info = m, shop = l })
                .Join(db.tbl_nongyaos, m => m.stat_info.nongyao_id, l => l.id, (m, l) => new { stat_info = m, nongyao = l })
                
                .Select(row => new StatisticsTableInfo
                {
                    id = row.stat_info.stat_info.id,
                    shopname = row.stat_info.shop.name,
                    shopphone = row.stat_info.shop.phone,
                    salecount = row.stat_info.stat_info.quantity != null ? (long)row.stat_info.stat_info.quantity : 0,
                }).ToList();
                stInfo = stInfo1.GroupBy(m => m.shopname)
                    .Select(g => new StatisticsTableInfo
                    {
                        shopname = g.Key,
                        id = g.Select(l => l.id).FirstOrDefault(),
                        shopphone = g.Select(l => l.shopphone).FirstOrDefault(),
                        salecount = g.Sum(l => l.salecount)
                    }).ToList();
            }
            else if (nongyao_type == 1) //taifenliao
            {
                var stInfo1 = db.tbl_salestatistics
                .Where(m => m.deleted == 0 && ((DateTime)m.regtime).Date >= start_date.Date && ((DateTime)m.regtime).Date <= end_date.Date && ((region_id == 0) ? true : m.region_id == region_id) && ((salesman_id == 0) ? true : m.shop_id == salesman_id))
                .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { stat_info = m, shop = l })
                .Join(db.tbl_nongyaos, m => m.stat_info.nongyao_id, l => l.id, (m, l) => new { stat_info = m, nongyao = l })
                .Where(m=> m.nongyao.parentid == nongyao_id)
                .Select(row => new StatisticsTableInfo
                {
                    id = row.stat_info.stat_info.id,
                    shopname = row.stat_info.shop.name,
                    shopphone = row.stat_info.shop.phone,
                    salecount = row.stat_info.stat_info.quantity != null ? (long)row.stat_info.stat_info.quantity : 0,
                }).ToList();
                stInfo = stInfo1.GroupBy(m => m.shopname)
                    .Select(g => new StatisticsTableInfo
                    {
                        shopname = g.Key,
                        id = g.Select(l => l.id).FirstOrDefault(),
                        shopphone = g.Select(l => l.shopphone).FirstOrDefault(),
                        salecount = g.Sum(l => l.salecount)
                    }).ToList();
            }
            else if (nongyao_type == 2) //xiaofenliao
            {
                var stInfo1 = db.tbl_salestatistics
                .Where(m => m.deleted == 0 && ((DateTime)m.regtime).Date >= start_date.Date && ((DateTime)m.regtime).Date <= end_date.Date && ((region_id == 0) ? true : m.region_id == region_id) && ((salesman_id == 0) ? true : m.shop_id == salesman_id))
                .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { stat_info = m, shop = l })
                .Join(db.tbl_nongyaos, m => m.stat_info.nongyao_id, l => l.id, (m, l) => new { stat_info = m, nongyao = l })
                .Where(m => m.nongyao.id == nongyao_id)
                .Select(row => new StatisticsTableInfo
                {
                    id = row.stat_info.stat_info.id,
                    shopname = row.stat_info.shop.name,
                    shopphone = row.stat_info.shop.phone,
                    salecount = row.stat_info.stat_info.quantity != null ? (long)row.stat_info.stat_info.quantity : 0,
                }).ToList();
                stInfo = stInfo1.GroupBy(m => m.shopname)
                    .Select(g => new StatisticsTableInfo
                    {
                        shopname = g.Key,
                        id = g.Select(l => l.id).FirstOrDefault(),
                        shopphone = g.Select(l => l.shopphone).FirstOrDefault(),
                        salecount = g.Sum(l => l.salecount)
                    }).ToList();
            }

            var displayedSalesman = stInfo.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            int i = 0;
            foreach (StatisticsTableInfo item in displayedSalesman)
            {
                var tmp = new[] { Convert.ToString(++i)/*Convert.ToString(item.id)*/, Convert.ToString(item.shopname), Convert.ToString(item.shopphone), Convert.ToString(item.salecount) };
                res.Add(tmp);
            }

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = res.Count();
            rst.iTotalDisplayRecords = res.Count();
            rst.aaData = res;
            return rst;
        }

    }
}