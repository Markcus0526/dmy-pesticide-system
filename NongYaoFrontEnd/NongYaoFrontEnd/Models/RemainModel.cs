using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region remain_model
    public class RemainInfo
    {
        public long id;
        public long catalog_id;
        public long standard_id;
        public string largenumber;
        public long quantity;
        public string product_date;
        public decimal? origin_price;
        public long? supply_id;
    }
    public class RemainCatalogInfo
    {
        public string catalog_num { get; set; }
        public string name { get; set; }
        public string usingname { get; set; }
        public string standard { get; set; }
        public long standard_id { get; set; }
        public string unit { get; set; }
        public string supply { get; set; }
        public string productdate { get; set; }
        public int avail_date { get; set; }
        public string largenumber { get; set; }
        public decimal price { get; set; }
        public long quantity { get; set; }
        public long? kind { get; set; }
    }

    public class ManageAvailInfo
    {
        public string catalog_name { get; set; }
        public string register_id { get; set; }
        public string largenumber { get; set; }
        public long quantity { get; set; }
        public string ticketnum { get; set; }
        public long standard_id { get; set; }
        public DateTime product_date { get; set; }
        public int avail_date { get; set; }
        public string product_area { get; set; }
        public long supply_id { get; set; }
    }

    public class REMAIN_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "重复";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
        public const string REMAIN_INSUFFICIENT = "库存不充足";
        public const string DUPLICATE_LARGENUMBER = "该产品批号的生产日期不正确";
        public const string OVER_AVAILDATE = "过期有效期";
        public const string NO_BUYING_CATALOG = "没有>采购农药";
        public const string NO_SALE_CATALOG = "没有销售农药";
        public const string ERROR_SALECNT = "销售数量不正确";
    }

    #endregion

    public class RemainModel
    {
        private NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static List<tbl_catalog> GetRemainCatalogList(long shop_id, long store_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            var countInfo = (from m in db.tbl_remains
                          where m.deleted == 0 && m.store_id == store_id
                          group m by new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber} into grouping
                          select new
                          {
                              catalog_id = grouping.Key.catalog_id,
                              standard_id = grouping.Key.standard_id,
                              largenumber = grouping.Key.largenumber,
                              quantity = (from l in grouping
                                          orderby l.id descending
                                          select l.quantity).FirstOrDefault()

                          }).ToList();

            var cataloginfo = (from m in countInfo
                                   group m by m.catalog_id into grouping
                                   select new {
                                    catalog_id = grouping.Key,
                                    quantity = (from l in grouping
                                          orderby l.quantity descending
                                          select l.quantity).Sum()
                                   });

            byte level = CommonModel.GetCurrentUserLevel();
            var retlist = (from m in cataloginfo
                           from l in db.tbl_catalogs
                           where m.catalog_id == l.id && m.quantity > 0 && l.deleted == 0 && l.pass == 1 && l.level <= level
                           select l).ToList();
            return retlist;
        }

        public static List<string> GetRemainCatalogLargenumberList(long shop_id, long store_id, long catalog_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.store_id == store_id && m.shop_id == shop_id && m.catalog_id == catalog_id
                    select m.largenumber).Distinct().ToList();
        }

        public static List<StandardInfo> GetRemainCatalogStandardList(long shop_id, long store_id, long catalog_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            var list = (from m in db.tbl_remains
                        where m.deleted == 0 && m.store_id == store_id && m.shop_id == shop_id && m.catalog_id == catalog_id
                        select new
                        {
                            standard_id = m.standard_id
                        }).Distinct().ToList();

            return (from m in list
                    from l in db.tbl_standards
                    where m.standard_id == l.id && l.deleted == 0
                    select new StandardInfo
                    {
                        id = l.id,
                        standard = l.quantity + (l.mass == 0 ? "克" : "毫升") + "/" + UnitModel.GetUnitNameById(l.unit_id)
                    }).ToList();
        }

        public static List<StandardInfo> GetRemainCatalogLargenumberStandardList(long shop_id, long store_id, long catalog_id, string largenumber)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            var list = (from m in db.tbl_remains
                        where m.deleted == 0 && m.store_id == store_id && m.shop_id == shop_id &&
                            m.catalog_id == catalog_id && m.largenumber == largenumber
                        select new
                        {
                            standard_id = m.standard_id
                        }).Distinct().ToList();

            return (from m in list
                    from l in db.tbl_standards
                    where m.standard_id == l.id && l.deleted == 0
                    select new StandardInfo
                    {
                        id = l.id,
                        standard = l.quantity + (l.mass == 0 ? "克" : "毫升") + "/" + UnitModel.GetUnitNameById(l.unit_id)
                    }).ToList();
        }

        public static List<string> GetRemainCatalogStandardLargenumberList(long shop_id, long store_id, long catalog_id, long standard_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.store_id == store_id && m.shop_id == shop_id &&
                        m.catalog_id == catalog_id && m.standard_id == standard_id
                    select m.largenumber).Distinct().ToList();
        }

        //public static long GetRemainCatalogInStore(long store_id, long catalog_id)
        //{
        //    tbl_remain lastitem = db.tbl_remains
        //        .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == store_id)
        //        .OrderByDescending(m => m.id)
        //        .FirstOrDefault();
        //    if (lastitem != null)
        //        return (long)lastitem.quantity;
        //    else
        //        return 0;
        //}

        public static tbl_remain GetRemainCatalogInStore(long shop_id, long store_id, long catalog_id,
            long standard_id, string largenumber)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            tbl_remain lastitem = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.catalog_id == catalog_id && m.store_id == store_id &&
                    m.standard_id == standard_id && m.largenumber == largenumber)
                .OrderByDescending(m => m.id)
                .FirstOrDefault();

            return lastitem;
        }
        public static tbl_remain GetRemainCatalogOrigin(long shop_id, long store_id, long catalog_id, long standard_id, string largenumber)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            tbl_remain lastitem = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.catalog_id == catalog_id && m.store_id == store_id &&
                    m.standard_id == standard_id && m.largenumber == largenumber)
                .OrderByDescending(m => m.id)
                .FirstOrDefault();

            return lastitem;
        }

        public static List<RemainCatalogInfo> GetRemainCatalogInfoListFromStore(long storeId, long nonyaotype)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();
            List<RemainCatalogInfo> retList = null;

            var remainlist = new List<RemainInfo>();
            var remainlist1 = new List<RemainInfo>();
            long total = 0;

            if (storeId == 0)
            {
                remainlist1 = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id)
                .GroupBy(m => new { store_id = m.store_id, catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                .Select(g => new RemainInfo
                {
                    id = g.Key.store_id,
                    catalog_id = g.Key.catalog_id,
                    standard_id = g.Key.standard_id,
                    largenumber = g.Key.largenumber,
                    supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                    product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                    origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                    quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
                }).ToList();

                remainlist = remainlist1.GroupBy(m => new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                    .Select(g => new RemainInfo
                    {                       
                        catalog_id = g.Key.catalog_id,
                        standard_id = g.Key.standard_id,
                        largenumber = g.Key.largenumber,
                        supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                        product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                        origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                        quantity = g.Sum(m => m.quantity)
                    }).ToList();
                foreach (var m in remainlist)
                {
                    total += m.quantity;
                }
                
            }
            else
            {
                remainlist = db.tbl_remains
                .Where(m => m.deleted == 0 && m.store_id == storeId)
                .GroupBy(m => new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                .Select(g => new RemainInfo
                {
                    catalog_id = g.Key.catalog_id,
                    standard_id = g.Key.standard_id,
                    largenumber = g.Key.largenumber,
                    supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                    product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                    origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                    quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
                }).ToList();
                
            }

            if (nonyaotype == 0)
            {
                retList = (from m in remainlist
                           from l in db.tbl_catalogs
                           where m.catalog_id == l.id
                           select new RemainCatalogInfo
                           {
                               catalog_num = l.catalog_num,
                               name = l.name,
                               usingname = l.usingname,
                               standard = CommonModel.GetStandardStringWithoutUnitFromId(m.standard_id),
                               unit = CommonModel.GetStandardUnitFromId(m.standard_id),
                               supply = SupplyModel.GetSupplyNameFromId(m.supply_id),
                               productdate = String.Format("{0:yyyy/MM/dd}", m.product_date),
                               avail_date = (int)l.avail_date,
                               largenumber = m.largenumber,
                               price = (decimal)m.origin_price,
                               quantity = m.quantity,
                               kind = l.kind
                           }).ToList();
            }
            else
            {
                tbl_nongyao nongitem = db.tbl_nongyaos
                    .Where(m => m.id == nonyaotype)
                    .FirstOrDefault();

              //  if (nongitem.parentid > 0)
             //   {
                    retList = (from m in remainlist
                               from l in db.tbl_catalogs
                               where m.catalog_id == l.id && l.kind == nonyaotype
                               select new RemainCatalogInfo
                               {
                                   catalog_num = l.catalog_num,
                                   name = l.name,
                                   usingname = l.usingname,
                                   standard = CommonModel.GetStandardStringWithoutUnitFromId(m.standard_id),
                                   unit = CommonModel.GetStandardUnitFromId(m.standard_id),
                                   supply = SupplyModel.GetSupplyNameFromId(m.supply_id),
                                   productdate = String.Format("{0:yyyy/MM/dd}", m.product_date),
                                   avail_date = (int)l.avail_date,
                                   largenumber = m.largenumber,
                                   price = (decimal)m.origin_price,
                                   quantity = m.quantity,
                                   kind = l.kind
                               }).ToList();

            }

            return retList;
        }

        public static decimal GetTotalRemainCatalogCount()
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();
            List<RemainCatalogInfo> retList = null;

            var remainlist = new List<RemainInfo>();
            var remainlist1 = new List<RemainInfo>();
            decimal total = 0;

                remainlist1 = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id)
                .GroupBy(m => new { store_id = m.store_id, catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                .Select(g => new RemainInfo
                {
                    id = g.Key.store_id,
                    catalog_id = g.Key.catalog_id,
                    standard_id = g.Key.standard_id,
                    largenumber = g.Key.largenumber,
                    supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                    product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                    origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                    quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
                }).ToList();

                remainlist = remainlist1.GroupBy(m => new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                    .Select(g => new RemainInfo
                    {
                        catalog_id = g.Key.catalog_id,
                        standard_id = g.Key.standard_id,
                        largenumber = g.Key.largenumber,
                        supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                        product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                        origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                        quantity = g.Sum(m => m.quantity)
                    }).ToList();
                foreach (var m in remainlist)
                {

                    total += m.quantity * StandardModel.GetStandardQuantity(m.standard_id) / 1000;
                }

                return total;
        }

        public string RefreshTotalRemainCatalogCount(long storeId, long nonyaotype)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();
            List<RemainCatalogInfo> retList = null;

            var remainlist = new List<RemainInfo>();
            var remainlist1 = new List<RemainInfo>();
            decimal total = 0;

            if (storeId == 0)
            {
                remainlist1 = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id)
                .GroupBy(m => new { store_id = m.store_id, catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                .Select(g => new RemainInfo
                {
                    id = g.Key.store_id,
                    catalog_id = g.Key.catalog_id,
                    standard_id = g.Key.standard_id,
                    largenumber = g.Key.largenumber,
                    supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                    product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                    origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                    quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
                }).ToList();

                remainlist = remainlist1.GroupBy(m => new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                    .Select(g => new RemainInfo
                    {
                        catalog_id = g.Key.catalog_id,
                        standard_id = g.Key.standard_id,
                        largenumber = g.Key.largenumber,
                        supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                        product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                        origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                        quantity = g.Sum(m => m.quantity)
                    }).ToList();
            }
            else
            {
                remainlist = db.tbl_remains
               .Where(m => m.deleted == 0 && m.store_id == storeId)
               .GroupBy(m => new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
               .Select(g => new RemainInfo
               {
                   catalog_id = g.Key.catalog_id,
                   standard_id = g.Key.standard_id,
                   largenumber = g.Key.largenumber,
                   supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                   product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                   origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                   quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
               }).ToList();
            }

            if (nonyaotype == 0)
            {
                retList = (from m in remainlist
                           from l in db.tbl_catalogs
                           where m.catalog_id == l.id
                           select new RemainCatalogInfo
                           {
                               catalog_num = l.catalog_num,
                               name = l.name,
                               usingname = l.usingname,
                               standard_id=m.standard_id,
                               standard = CommonModel.GetStandardStringWithoutUnitFromId(m.standard_id),
                               unit = CommonModel.GetStandardUnitFromId(m.standard_id),
                               supply = SupplyModel.GetSupplyNameFromId(m.supply_id),
                               productdate = String.Format("{0:yyyy/MM/dd}", m.product_date),
                               avail_date = (int)l.avail_date,
                               largenumber = m.largenumber,
                               price = (decimal)m.origin_price,
                               quantity = m.quantity,
                               kind = l.kind
                           }).ToList();
            }
            else
            {
                tbl_nongyao nongitem = db.tbl_nongyaos
                    .Where(m => m.id == nonyaotype)
                    .FirstOrDefault();

                //  if (nongitem.parentid > 0)
                //   {
                retList = (from m in remainlist
                           from l in db.tbl_catalogs
                           where m.catalog_id == l.id && l.kind == nonyaotype
                           select new RemainCatalogInfo
                           {
                               catalog_num = l.catalog_num,
                               name = l.name,
                               usingname = l.usingname,
                               standard_id = m.standard_id,
                               standard = CommonModel.GetStandardStringWithoutUnitFromId(m.standard_id),
                               unit = CommonModel.GetStandardUnitFromId(m.standard_id),
                               supply = SupplyModel.GetSupplyNameFromId(m.supply_id),
                               productdate = String.Format("{0:yyyy/MM/dd}", m.product_date),
                               avail_date = (int)l.avail_date,
                               largenumber = m.largenumber,
                               price = (decimal)m.origin_price,
                               quantity = m.quantity,
                               kind = l.kind
                           }).ToList();

            }

            foreach (var m in retList)
            {

                total += m.quantity * StandardModel.GetStandardQuantity(m.standard_id) / 1000;
            }

            return total.ToString();
        }

        public static decimal GetTotalPriceCatalogCount()
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();
            List<RemainCatalogInfo> retList = null;

            var remainlist = new List<RemainInfo>();
            var remainlist1 = new List<RemainInfo>();
            decimal total = 0;

            remainlist1 = db.tbl_remains
            .Where(m => m.deleted == 0 && m.shop_id == shop_id)
            .GroupBy(m => new { store_id = m.store_id, catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
            .Select(g => new RemainInfo
            {
                id = g.Key.store_id,
                catalog_id = g.Key.catalog_id,
                standard_id = g.Key.standard_id,
                largenumber = g.Key.largenumber,
                supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
            }).ToList();

            remainlist = remainlist1.GroupBy(m => new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                .Select(g => new RemainInfo
                {
                    catalog_id = g.Key.catalog_id,
                    standard_id = g.Key.standard_id,
                    largenumber = g.Key.largenumber,
                    supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                    product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                    origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                    quantity = g.Sum(m => m.quantity)
                }).ToList();
            foreach (var m in remainlist)
            {

                total += m.quantity * (decimal)m.origin_price;
            }

            return total;
        }

        public string RefreshGetTotalPriceCatalogCount(long storeId, long nonyaotype)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();
            List<RemainCatalogInfo> retList = null;

            var remainlist = new List<RemainInfo>();
            var remainlist1 = new List<RemainInfo>();
            decimal total = 0;

            if (storeId == 0)
            {
                remainlist1 = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == shop_id)
                .GroupBy(m => new { store_id = m.store_id, catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                .Select(g => new RemainInfo
                {
                    id = g.Key.store_id,
                    catalog_id = g.Key.catalog_id,
                    standard_id = g.Key.standard_id,
                    largenumber = g.Key.largenumber,
                    supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                    product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                    origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                    quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
                }).ToList();

                remainlist = remainlist1.GroupBy(m => new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                    .Select(g => new RemainInfo
                    {
                        catalog_id = g.Key.catalog_id,
                        standard_id = g.Key.standard_id,
                        largenumber = g.Key.largenumber,
                        supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                        product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                        origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                        quantity = g.Sum(m => m.quantity)
                    }).ToList();
            }

            else
            {
                remainlist = db.tbl_remains
               .Where(m => m.deleted == 0 && m.store_id == storeId)
               .GroupBy(m => new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
               .Select(g => new RemainInfo
               {
                   catalog_id = g.Key.catalog_id,
                   standard_id = g.Key.standard_id,
                   largenumber = g.Key.largenumber,
                   supply_id = g.OrderByDescending(m => m.id).FirstOrDefault().supply_id,
                   product_date = String.Format("{0:yyyy/MM/dd}", g.OrderByDescending(m => m.id).FirstOrDefault().product_date),
                   origin_price = g.OrderByDescending(m => m.id).FirstOrDefault().origin_price,
                   quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
               }).ToList();
            }

            if (nonyaotype == 0)
            {
                retList = (from m in remainlist
                           from l in db.tbl_catalogs
                           where m.catalog_id == l.id
                           select new RemainCatalogInfo
                           {
                               catalog_num = l.catalog_num,
                               name = l.name,
                               usingname = l.usingname,
                               standard = CommonModel.GetStandardStringWithoutUnitFromId(m.standard_id),
                               unit = CommonModel.GetStandardUnitFromId(m.standard_id),
                               supply = SupplyModel.GetSupplyNameFromId(m.supply_id),
                               productdate = String.Format("{0:yyyy/MM/dd}", m.product_date),
                               avail_date = (int)l.avail_date,
                               largenumber = m.largenumber,
                               price = (decimal)m.origin_price,
                               quantity = m.quantity,
                               kind = l.kind
                           }).ToList();
            }
            else
            {
                tbl_nongyao nongitem = db.tbl_nongyaos
                    .Where(m => m.id == nonyaotype)
                    .FirstOrDefault();

                //  if (nongitem.parentid > 0)
                //   {
                retList = (from m in remainlist
                           from l in db.tbl_catalogs
                           where m.catalog_id == l.id && l.kind == nonyaotype
                           select new RemainCatalogInfo
                           {
                               catalog_num = l.catalog_num,
                               name = l.name,
                               usingname = l.usingname,
                               standard = CommonModel.GetStandardStringWithoutUnitFromId(m.standard_id),
                               unit = CommonModel.GetStandardUnitFromId(m.standard_id),
                               supply = SupplyModel.GetSupplyNameFromId(m.supply_id),
                               productdate = String.Format("{0:yyyy/MM/dd}", m.product_date),
                               avail_date = (int)l.avail_date,
                               largenumber = m.largenumber,
                               price = (decimal)m.origin_price,
                               quantity = m.quantity,
                               kind = l.kind
                           }).ToList();

            }

            foreach (var m in retList)
            {

                total += m.quantity * (decimal)m.price;
            }

            return total.ToString();
        }

        public bool CheckRemain(    // store_id는 고려하지 않는다
            long catalog_id,
            long store_id,
            decimal count,
            long standard_id,
            string largenumber)
        {
            tbl_remain lastitem = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == store_id
                        && m.standard_id == standard_id && m.largenumber == largenumber)
                .OrderByDescending(m => m.id)
                .FirstOrDefault();
            if (lastitem != null && lastitem.quantity >= count)
                return true;
            else
                return false;
        }

        public bool CheckDuplicateLargenumber(
            long catalog_id,
            long store_id,
            long standard_id,
            string largenumber,
            string product_date)
        {
            var item = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && //m.store_id == store_id &&
                        m.standard_id == standard_id && m.largenumber == largenumber && 
                        m.product_date.Date != Convert.ToDateTime(product_date).Date)
                .OrderByDescending(m => m.id)
                .FirstOrDefault();
            if (item != null)
                return false;
            else
            {
                //var item1 = db.tbl_remains
                //.Where(m => m.deleted == 0 && m.catalog_id == catalog_id && //m.store_id == store_id &&
                //        m.standard_id == standard_id && m.largenumber != largenumber &&
                //        m.product_date.Date == Convert.ToDateTime(product_date).Date)
                //.OrderByDescending(m => m.id)
                //.FirstOrDefault();

                //if (item1 != null)
                //    return false;
                //else
                    return true;
            }
        }

        public string InsertRemain(
            long catalog_id,
            long standard_id,
            string largenumber,
            long store_id,
            decimal price,
            int count,
            long supply_id,
            DateTime product_date,
            decimal origin_price,
            byte type
            )
        {
            try
            {
                tbl_remain lastproductitem = db.tbl_remains
                    .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == store_id
                        && m.standard_id == standard_id && m.largenumber == largenumber)
                    .OrderByDescending(m => m.id)
                    .FirstOrDefault();
                long quantity = 0;
                if (lastproductitem != null)
                    quantity = lastproductitem.quantity;

                tbl_remain lastitem = db.tbl_remains
                    .Where(m => m.deleted == 0 /*&& m.catalog_id == catalog_id*/ && m.store_id == store_id)
                    .OrderByDescending(m => m.id)
                    .FirstOrDefault();
                long total_quantity = 0;
                if (lastitem != null)
                    total_quantity = lastitem.total_quantity;

                tbl_remain newitem = new tbl_remain();

                newitem.catalog_id = catalog_id;
                newitem.standard_id = standard_id;
                newitem.largenumber = largenumber;
                newitem.store_id = store_id;
                newitem.buy_price = price;
                newitem.catalog_cnt = count;
                newitem.sum_price = price * count;
                newitem.type = type;
                newitem.supply_id = supply_id;
                if (product_date != null)
                    newitem.product_date = product_date;
                if (origin_price != null)
                    newitem.origin_price = origin_price;

                switch (type)
                {
                    case 0:
                    case 3:
                        newitem.quantity = quantity + count;
                        newitem.total_quantity = total_quantity + count;
                        break;
                    case 1:
                    case 2:
                        newitem.quantity = quantity - count;
                        newitem.total_quantity = total_quantity - count;
                        break;
                }
                newitem.shop_id = CommonModel.GetCurrentUserShopId();
                newitem.regtime = DateTime.Now;
                newitem.user_id = CommonModel.GetCurrentUserId();

                db.tbl_remains.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return REMAIN_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("RemainModel", "InsertRemain()", e.ToString());
                return REMAIN_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public static bool DeleteRemainsForShopId(long shop_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            try
            {
                var items = from m in db.tbl_remains
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

        public static long GetBuyingTotalCount(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && m.type == 0 && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                        select m).ToList().Sum(m => m.catalog_cnt);
        }

        public static decimal GetBuyingTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && m.type == 0 && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.sum_price);
        }

        public static long GetBuyingBackTotalCount(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && m.type == 1 && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.catalog_cnt);
        }

        public static decimal GetBuyingBackTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && m.type == 1 && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.sum_price);
        }

        public static long GetSellTotalCount(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && m.type == 2 && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.catalog_cnt);
        }

        public static decimal GetSellTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && m.type == 2 && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.sum_price);
        }

        public static long GetSellBackTotalCount(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && m.type == 3 && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.catalog_cnt);
        }

        public static decimal GetSellBackTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && m.type == 3 && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.sum_price);
        }

        public static long GetRemainTotalCount(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            var list = (from m in db.tbl_remains
                        where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                        group m by new { store_id = m.store_id, catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber } into g
                        select new
                        {
                            quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity,
                            origin_price = g.OrderByDescending(l => l.id).FirstOrDefault().origin_price
                        }).ToList();

            return list.Sum(m => m.quantity);
        }

        public static decimal GetRemainTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            var list = (from m in db.tbl_remains
                        where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                        group m by new { store_id = m.store_id, catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber } into g
                        select new
                        {
                            total_quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity,
                            origin_price = g.OrderByDescending(l => l.id).FirstOrDefault().origin_price
                        }).ToList();

            return list.Sum(m => m.total_quantity * m.origin_price);
        }

        public static decimal GetSellOriginTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && m.type == 2 && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                        select m).ToList().Sum(m => m.catalog_cnt * m.origin_price);
        }

        public List<ManageAvailInfo> GetManageAvailList(long store_id, byte status, int remaindays)
        {
            var list1 = (from m in db.tbl_remains
                             where m.deleted == 0 && m.store_id == store_id
                             group m by new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber } into grouping
                             select new
                             {
                                 catalog_id = grouping.Key.catalog_id,
                                 standard_id = grouping.Key.standard_id,
                                 largenumber = grouping.Key.largenumber,
                                 quantity = (from l in grouping
                                             orderby l.id descending
                                             select l.quantity).FirstOrDefault(),
                                 product_date = (from l in grouping
                                                 orderby l.id descending
                                                 select l.product_date).FirstOrDefault(),
                                 supply_id = (from l in grouping
                                              orderby l.id descending
                                              select l.supply_id).FirstOrDefault(),

                             }).ToList();

            var list2 = (from m in list1
                        from l in db.tbl_buyings
                        where l.catalog_id == m.catalog_id && l.store_id == store_id && l.standard_id == m.standard_id && l.largenumber == m.largenumber &&
                            l.deleted == 0
                        select new
                        {
                            catalog_id = m.catalog_id,
                            standard_id = m.standard_id,
                            largenumber = m.largenumber,
                            quantity = m.quantity,
                            product_date = m.product_date,
                            supply_id = m.supply_id,
                            ticket_id = l.ticket_id
                        }).ToList();

            DateTime today = DateTime.Now;
            List<ManageAvailInfo> retlist = null;
            if (status == 0)
            {
                retlist = (from m in list2
                           from l in db.tbl_tickets
                           from k in db.tbl_catalogs
                           where m.ticket_id == l.id && l.deleted == 0 && l.type == 0
                            && m.catalog_id == k.id && k.deleted == 0
                            && (today - m.product_date).TotalDays >= k.avail_date * 30
                           select new ManageAvailInfo
                           {
                               catalog_name = k.name,
                               register_id = k.register_id,
                               largenumber = m.largenumber,
                               quantity = m.quantity,
                               ticketnum = l.ticketnum,
                               standard_id = m.standard_id,
                               product_date = m.product_date,
                               avail_date = (int)k.avail_date,
                               product_area = k.product_area,
                               supply_id = m.supply_id
                           }).ToList();
            }
            else if (status == 1)
            {
                retlist = (from m in list2
                           from l in db.tbl_tickets
                           from k in db.tbl_catalogs
                           where m.ticket_id == l.id && l.deleted == 0 && l.type == 0
                            && m.catalog_id == k.id && k.deleted == 0
                            && (today - m.product_date).TotalDays <= k.avail_date * 30 && (today - m.product_date).TotalDays >= k.avail_date * 30 - remaindays
                           select new ManageAvailInfo
                           {
                               catalog_name = k.name,
                               register_id = k.register_id,
                               largenumber = m.largenumber,
                               quantity = m.quantity,
                               ticketnum = l.ticketnum,
                               standard_id = m.standard_id,
                               product_date = m.product_date,
                               avail_date = (int)k.avail_date,
                               product_area = k.product_area,
                               supply_id = m.supply_id
                           }).ToList();
            }
            else
            {
                try
                {
                    retlist = (from m in list2
                               from l in db.tbl_tickets
                               from k in db.tbl_catalogs
                               where m.ticket_id == l.id && l.deleted == 0 && l.type == 0
                                && m.catalog_id == k.id && k.deleted == 0
                               select new ManageAvailInfo
                               {
                                   catalog_name = k.name,
                                   register_id = k.register_id,
                                   largenumber = m.largenumber,
                                   quantity = m.quantity,
                                   ticketnum = l.ticketnum,
                                   standard_id = m.standard_id,
                                   product_date = m.product_date,
                                   avail_date = (int)k.avail_date,
                                   product_area = k.product_area,
                                   supply_id = m.supply_id
                               }).ToList();
                }
                catch (Exception ex)
                {
                    string ss = ex.Message;
                    string sss = ss;
                }
            }

            return retlist;
        }

        public JqDataTableInfo GetManageAvailListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long store_id, byte status, int remaindays)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<ManageAvailInfo> filteredCompanies;

            List<ManageAvailInfo> alllist = GetManageAvailList(store_id, status, remaindays);

            //Check whether the companies should be filtered by keyword
            //if (!string.IsNullOrEmpty(param.sSearch))
            //{
            //    //Used if particulare columns are filtered 
            //    var nameFilter = Convert.ToString(Request["sSearch_1"]);

            //    //Optionally check whether the columns are searchable at all 
            //    var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

            //    filteredCompanies = alllist
            //       .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            //}
            //else
            //{
                filteredCompanies = alllist;
            //}
            
            //var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            //var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<tbl_salelist, long> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.id :
            //                                               0);

            //var sortDirection = Request["sSortDir_0"]; // asc or desc
            //if (sortDirection == "asc")
            //    filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            //else
            //    filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }
            
            DateTime today = DateTime.Now;
            
            var result = from c in displayedCompanies
                         select new[] { 
                (today - c.product_date).TotalDays >= c.avail_date * 30 ? "已过期" : "未过期",
                c.catalog_name,
                c.register_id,
                c.largenumber,
                //(today - c.product_date).TotalDays >= c.avail_date * 30 ? Convert.ToString((int)((today - c.product_date).TotalDays) - c.avail_date * 30) : "",
                String.Format("{0:yyyy-MM-dd}", c.product_date.AddMonths(c.avail_date)),
                Convert.ToString(c.quantity),
                c.ticketnum,
                StandardModel.GetStandardDescForId(c.standard_id),
                String.Format("{0:yyyy-MM-dd}", c.product_date),
                Convert.ToString(c.avail_date) + "个月",
                c.product_area,
                SupplyModel.GetSupplyPhoneForId(c.supply_id)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

    }
}
