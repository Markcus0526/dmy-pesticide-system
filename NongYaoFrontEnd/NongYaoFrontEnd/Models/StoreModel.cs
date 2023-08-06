using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region store_model
    
    public class Store
    {
        public string id { get; set; }
        public string name { get; set; }
        public string storeManager { get; set; }
        public long index { get; set; }
    }

    public class STORE_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "操作失败： 安装包名称重复！";
        public const string ERROR_NOCATALOG = "没有商品";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "数量超过！";
    }


    public class StockInfo
    {
        public long id { get; set; }
        public long store_id { get; set; }
        public long catalog_id { get; set; }
        public long standard_id { get; set; }
        public string largenumber { get; set; }
        public long? stock { get; set; }
    }

    #endregion
    public class StoreModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static JqDataTableInfo GetStoreListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<Store> filteredCompanies;

            var alllist = GetStoreList();

            filteredCompanies = alllist;

            var displayedUnit = filteredCompanies.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedUnit
                         select new[] {
                             Convert.ToString(c.index),
                             Convert.ToString(c.name),
                             Convert.ToString(c.storeManager),                             
                             Convert.ToString(c.id)
                         };
            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = alllist.Count();
            rst.aaData = result;

            return rst;
        }

        public static IEnumerable<Store> GetStoreList()
        {
            List<Store> retList = null;            
            long shopId = CommonModel.GetCurrentUserShopId();
            retList = db.tbl_stores
                .Where(p => p.deleted == 0 && p.shop_id == shopId)
                .OrderByDescending(p => p.id)
                .Select(row => new Store
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    storeManager = GetStoreManager(row.uid)
                }).ToList();
            long index = 1;
            foreach (Store strItem in retList)
            {
                strItem.index = index;
                index++;
            }
            return retList;
        }

        public static IEnumerable<Store> GetUsersStoreList(long uid)
        {
            List<Store> retList = null;            

            retList = db.tbl_stores
                .Where(p => p.deleted == 0 && p.uid.Contains(uid.ToString()))
                .Select(row => new Store
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name)
                }).ToList();
            return retList;
        }

        public static string GetStoreManager(string id)
        {
            string[] ids = id.Split(',');
            string user = "";
            foreach (string id_data in ids)
            {
                User userInfo = db.tbl_users
                                .Where(p => p.deleted == 0 && p.id == long.Parse(id_data))
                                .Select(row => new User
                                {
                                    id = Convert.ToString(row.id),
                                    name = Convert.ToString(row.name)
                                }).FirstOrDefault();

                if (userInfo != null)
                    user = user + userInfo.name + ",";                
            }
            if (user.Length > 0)
                user = user.Substring(0, user.Length - 1);
            return user;            
        }

        public static Store GetStoreInfo(long id)
        {
            Store retInfo = null;           

            retInfo = db.tbl_stores
                .Where(p => p.deleted == 0 && p.id == id)
                .Select(row => new Store
                {
                    id = Convert.ToString(row.id),
                    name = Convert.ToString(row.name),
                    storeManager = Convert.ToString(row.uid)
                }).SingleOrDefault();

            return retInfo;
        }

        public string UpdateStoreItem(long store_id, string store_name, string userId)
        {               
            try
            {
                tbl_store edititem = (from m in db.tbl_stores
                                      where m.deleted == 0 && m.id == store_id
                                      select m).FirstOrDefault();
                if (edititem != null)
                {
                    edititem.name = store_name;
                    edititem.uid = userId;
                    edititem.regtime = DateTime.Now;
                }

                db.SubmitChanges();

                return STORE_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("StoreModel", "UpdateItem()", e.ToString());
                return STORE_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public string InsertStoreItem(string store_name, string userId)
        {
            try
            {
                
                tbl_store newitem = new tbl_store();

                newitem.name = store_name;
                newitem.uid = userId;
                newitem.shop_id = CommonModel.GetCurrentUserShopId();
                newitem.regtime = DateTime.Now;
                db.tbl_stores.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return STORE_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("StoreModel", "InsertItem()", e.ToString());
                return STORE_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public bool DeleteStore(long id)
        {            
            var delitem = (from m in db.tbl_stores
                           where m.deleted == 0 && m.id == id
                           select m).FirstOrDefault();

            if (delitem != null)
            {
                delitem.deleted = 1;
                db.SubmitChanges();
            }

            return true;
        }

        public static List<tbl_store> GetStoreList(long shop_id)
        {
            List<tbl_store> storeList = (from m in db.tbl_stores
                                         where m.deleted == 0 && m.shop_id == shop_id
                                         select m).ToList();
            return storeList;
        }


#region StoreMove 
        public string SaveStoreMove(string[] catalog_id, string[] standard_id, string[] largenumber, string[] start_store, string[] end_store, string[] count)
        {
            long catalogId;
            long start_storeid;
            long standardId;
            string largenum;
            long bCount;
            long end_storeid;
            bool exStockList;
            bool exStockList1;
            List<StockInfo> stockList = new List<StockInfo>();
            for (var i = 0; i < catalog_id.Length; i++)
            {
                exStockList = false;
                exStockList1 = false;
                catalogId = Convert.ToInt64(catalog_id[i]);
                standardId = Convert.ToInt64(standard_id[i]);
                largenum = largenumber[i];
                start_storeid = Convert.ToInt64(start_store[i]);
                bCount = Convert.ToInt64(count[i]);
                end_storeid = Convert.ToInt64(end_store[i]);

                tbl_remain rmInfo = (from m in db.tbl_remains
                                     where m.deleted == 0 && m.store_id == start_storeid && m.catalog_id == catalogId && m.standard_id == standardId && m.largenumber == largenum
                                     orderby m.regtime descending
                                     select m).FirstOrDefault();
                tbl_remain rmInfo1 = (from m in db.tbl_remains
                                      where m.deleted == 0 && m.store_id == end_storeid && m.catalog_id == catalogId && m.standard_id == standardId && m.largenumber == largenum
                                      orderby m.regtime descending
                                      select m).FirstOrDefault();
                foreach (var stkItem in stockList)
                {
                    if (rmInfo != null && stkItem.id == rmInfo.id)
                    {
                        exStockList = true;
                        stkItem.stock -= bCount;
                    }
                    else if (rmInfo != null && stkItem.store_id == rmInfo.store_id && stkItem.catalog_id == rmInfo.catalog_id && stkItem.standard_id == rmInfo.standard_id && stkItem.largenumber == rmInfo.largenumber)
                    {
                        exStockList = true;
                        stkItem.stock -= bCount;
                    }
                    if (rmInfo1 != null && stkItem.id == rmInfo1.id)
                    {
                        exStockList1 = true;
                        stkItem.stock += bCount;
                    }
                    else if (rmInfo1 != null && stkItem.store_id == rmInfo1.store_id && stkItem.catalog_id == rmInfo1.catalog_id && stkItem.standard_id == rmInfo1.standard_id && stkItem.largenumber == rmInfo1.largenumber)
                    {
                        exStockList1 = true;
                        stkItem.stock += bCount;
                    }
                }
                if (exStockList == false)
                {
                    StockInfo stockInfo = new StockInfo();
                    stockInfo.id = rmInfo.id;
                    stockInfo.store_id = (long)rmInfo.store_id;
                    stockInfo.catalog_id = (long)rmInfo.catalog_id;
                    stockInfo.standard_id = (long)rmInfo.standard_id;
                    stockInfo.largenumber = rmInfo.largenumber;
                    stockInfo.stock = rmInfo.quantity - bCount;
                    stockList.Add(stockInfo);
                }
                if (exStockList1 == false)
                {
                    StockInfo stockInfo1 = new StockInfo();
                    if (rmInfo1 != null)
                    {
                        stockInfo1.id = rmInfo1.id;
                        stockInfo1.store_id = (long)rmInfo1.store_id;
                        stockInfo1.catalog_id = (long)rmInfo1.catalog_id;
                        stockInfo1.standard_id = (long)rmInfo1.standard_id;
                        stockInfo1.largenumber = rmInfo1.largenumber;
                        stockInfo1.stock = rmInfo1.quantity + bCount;
                        stockList.Add(stockInfo1);
                    }
                    else
                    {
                        stockInfo1.store_id = end_storeid;
                        stockInfo1.catalog_id = catalogId;
                        stockInfo1.standard_id = standardId;
                        stockInfo1.largenumber = largenum;
                        stockInfo1.stock = bCount;
                        stockList.Add(stockInfo1);
                    }
                }
            }

            foreach (var sktItem in stockList)
            {
                if (sktItem.stock < 0)
                    return STORE_SUBMITSTATUS.ERROR_SUBMIT;
            }

            long store1_catalog_count = 0;
            long store2_catalog_count = 0;

            for (var i = 0; i < catalog_id.Length; i++)
            {
                exStockList = false;
                catalogId = Convert.ToInt64(catalog_id[i]);
                standardId = Convert.ToInt64(standard_id[i]);
                largenum = largenumber[i];
                start_storeid = Convert.ToInt64(start_store[i]);
                bCount = Convert.ToInt64(count[i]);
                end_storeid = Convert.ToInt64(end_store[i]);
                tbl_remain rmInfo1 = (from m in db.tbl_remains
                                      where m.deleted == 0 && m.store_id == start_storeid && m.catalog_id == catalogId && m.standard_id == standardId && m.largenumber == largenum
                                      orderby m.regtime descending
                                      select m).FirstOrDefault();
                tbl_remain rmInfo2 = (from m in db.tbl_remains
                                      where m.deleted == 0 && m.store_id == end_storeid && m.catalog_id == catalogId && m.standard_id == standardId && m.largenumber == largenum
                                      orderby m.regtime descending
                                      select m).FirstOrDefault();
                tbl_remain rmInfo3 = (from m in db.tbl_remains
                                      where m.deleted == 0
                                      orderby m.regtime descending
                                      select m).FirstOrDefault();
                if (rmInfo1 != null)
                {
                    store1_catalog_count = (long)rmInfo1.quantity;
                    
                    tbl_remain newremain1 = new tbl_remain
                    {
                        catalog_id = catalogId,
                        standard_id = standardId,
                        largenumber = largenum,
                        store_id = start_storeid,
                        buy_price = 0,
                        catalog_cnt = -(int)bCount,
                        sum_price = 0,
                        quantity = store1_catalog_count - bCount,
                        total_quantity = rmInfo3.total_quantity,
                        type = 4, //storemove
                        shop_id = CommonModel.GetCurrentUserShopId(),
                        regtime = DateTime.Now,
                        deleted = 0,
                        supply_id = rmInfo1.supply_id,
                        product_date = rmInfo1.product_date,
                        origin_price = rmInfo1.origin_price,
                        user_id = CommonModel.GetCurrentUserId()
                    };

                    if (rmInfo2 != null)
                        store2_catalog_count = (long)rmInfo2.quantity;
                    tbl_remain newremain2 = new tbl_remain
                    {
                        catalog_id = catalogId,
                        standard_id = standardId,
                        largenumber = largenum,
                        store_id = end_storeid,
                        buy_price = 0,
                        catalog_cnt = (int)bCount,
                        sum_price = 0,
                        quantity = store2_catalog_count + bCount,
                        total_quantity = rmInfo3.total_quantity,
                        type = 4, //storemove
                        shop_id = CommonModel.GetCurrentUserShopId(),
                        regtime = DateTime.Now,
                        deleted = 0,
                        supply_id = rmInfo1.supply_id,
                        product_date = rmInfo1.product_date,
                        origin_price = rmInfo1.origin_price,
                        user_id = CommonModel.GetCurrentUserId()
                    };
                    db.tbl_remains.InsertOnSubmit(newremain1);
                    db.tbl_remains.InsertOnSubmit(newremain2);

                    tbl_storemoving addItem = new tbl_storemoving
                    {
                        sending_id = start_storeid,
                        recving_id = end_storeid,
                        catalog_id = catalogId,
                        standard_id = standardId,
                        largenumber = largenum,
                        catalog_cnt = (int)bCount,
                        shop_id = CommonModel.GetCurrentUserShopId(),
                        regtime = DateTime.Now,
                        deleted = 0,
                    };
                    db.tbl_storemovings.InsertOnSubmit(addItem);


                    db.SubmitChanges();
                }
                else
                {
                    return STORE_SUBMITSTATUS.ERROR_NOCATALOG;
                }
            }
            return STORE_SUBMITSTATUS.SUCCESS_SUBMIT;
        }
#endregion

        public string SaveSpendInStore(string[] catalog_id, string[] standard_id, string[] largenumber, string[] start_store, string[] count, string[] spendreason)
        {
            long store1_catalog_count = 0;
            long catalogId;
            long standardId;
            string largenum;
            long store_id;
            long bCount;
            string spendReason;
            /*
            for (var i = 0; i < catalog_id.Length; i++)
            {
                catalogId = Convert.ToInt64(catalog_id[i]);
                standardId = Convert.ToInt64(standard_id[i]);
                largenum = largenumber[i];
                store_id = Convert.ToInt64(start_store[i]);
                bCount = Convert.ToInt64(count[i]);
                spendReason = spendreason[i];

                tbl_remain rmInfo = (from m in db.tbl_remains
                                      where m.deleted == 0 && m.store_id == store_id && m.catalog_id == catalogId && m.standard_id == standardId && m.largenumber == largenum
                                      orderby m.regtime descending
                                      select m).FirstOrDefault();
                if (rmInfo != null)
                {
                    if (rmInfo.quantity - bCount < 0)
                        return STORE_SUBMITSTATUS.ERROR_SUBMIT;
                }
                
            }
            */
            bool exStockList;
            List<StockInfo> stockList = new List<StockInfo>();
            for (var i = 0; i < catalog_id.Length; i++)
            {
                exStockList = false;
                catalogId = Convert.ToInt64(catalog_id[i]);
                standardId = Convert.ToInt64(standard_id[i]);
                largenum = largenumber[i];
                store_id = Convert.ToInt64(start_store[i]);
                bCount = Convert.ToInt64(count[i]);

                tbl_remain rmInfo = (from m in db.tbl_remains
                                     where m.deleted == 0 && m.store_id == store_id && m.catalog_id == catalogId && m.standard_id == standardId && m.largenumber == largenum
                                     orderby m.regtime descending
                                     select m).FirstOrDefault();
                foreach (var stkItem in stockList)
                {
                    if (rmInfo != null && stkItem.id == rmInfo.id)
                    {
                        exStockList = true;
                        stkItem.stock -= bCount;
                    }
                    else if (rmInfo != null && stkItem.store_id == rmInfo.store_id && stkItem.catalog_id == rmInfo.catalog_id && stkItem.standard_id == rmInfo.standard_id && stkItem.largenumber == rmInfo.largenumber)
                    {
                        exStockList = true;
                        stkItem.stock -= bCount;
                    }
                }
                if (exStockList == false)
                {
                    StockInfo stockInfo1 = new StockInfo();
                    if (rmInfo != null)
                    {
                        stockInfo1.id = rmInfo.id;
                        stockInfo1.store_id = (long)rmInfo.store_id;
                        stockInfo1.catalog_id = (long)rmInfo.catalog_id;
                        stockInfo1.standard_id = (long)rmInfo.standard_id;
                        stockInfo1.largenumber = rmInfo.largenumber;
                        stockInfo1.stock = rmInfo.quantity - bCount;
                        stockList.Add(stockInfo1);
                    }
                    else
                    {
                        stockInfo1.store_id = store_id;
                        stockInfo1.catalog_id = catalogId;
                        stockInfo1.standard_id = standardId;
                        stockInfo1.largenumber = largenum;
                        stockInfo1.stock = bCount;
                        stockList.Add(stockInfo1);
                    }
                }
            }

            foreach (var sktItem in stockList)
            {
                if (sktItem.stock < 0)
                    return STORE_SUBMITSTATUS.ERROR_SUBMIT;
            }

            for (var i = 0; i < catalog_id.Length; i++)
            {
                catalogId = Convert.ToInt64(catalog_id[i]);
                standardId = Convert.ToInt64(standard_id[i]);
                largenum = largenumber[i];
                store_id = Convert.ToInt64(start_store[i]);
                bCount = Convert.ToInt64(count[i]);
                spendReason = spendreason[i];
                tbl_remain rmInfo1 = (from m in db.tbl_remains
                                      where m.deleted == 0 && m.store_id == store_id && m.catalog_id == catalogId && m.standard_id == standardId && m.largenumber == largenum
                                      orderby m.regtime descending
                                      select m).FirstOrDefault();
                if (rmInfo1 != null)
                {
                    store1_catalog_count = (long)rmInfo1.quantity;

                    tbl_remain rmInfo2 = (from m in db.tbl_remains
                                          where m.deleted == 0
                                          orderby m.regtime descending
                                          select m).FirstOrDefault();
                    
                    tbl_remain newremain1 = new tbl_remain
                    {
                        catalog_id = catalogId,
                        standard_id = standardId,
                        largenumber = largenum,
                        store_id = store_id,
                        buy_price = 0,
                        catalog_cnt = -(int)bCount,
                        sum_price = 0,
                        quantity = store1_catalog_count - bCount,
                        total_quantity = rmInfo2.total_quantity,
                        type = 5, //storespend
                        shop_id = CommonModel.GetCurrentUserShopId(),
                        regtime = DateTime.Now,
                        deleted = 0,
                        supply_id = rmInfo1.supply_id,
                        product_date = rmInfo1.product_date,
                        origin_price = rmInfo1.origin_price,
                        user_id = CommonModel.GetCurrentUserId()
                    };
                    
                    db.tbl_remains.InsertOnSubmit(newremain1);
                    
                    tbl_usinglist addItem = new tbl_usinglist
                    {
                        store_id = store_id,
                        catalog_id = catalogId,
                        standard_id = standardId,
                        largenumber = largenum,
                        catalog_cnt = (int)bCount,
                        reason = spendReason,
                        shop_id = CommonModel.GetCurrentUserShopId(),
                        userid = CommonModel.GetCurrentUserId(),
                        regtime = DateTime.Now,
                        deleted = 0,
                    };
                    db.tbl_usinglists.InsertOnSubmit(addItem);

                    db.SubmitChanges();
                }
                else
                {
                    return STORE_SUBMITSTATUS.ERROR_NOCATALOG;
                }
            }
            return STORE_SUBMITSTATUS.SUCCESS_SUBMIT;
        }

        public List<tbl_nongyao> GetNongyaoAtParent(int i)
        {
            return (from m in db.tbl_nongyaos
                    where m.deleted == 0 && m.parentid == i
                    select m).ToList();
        }

        public List<string> GetNames(List<tbl_nongyao> list)
        {
            return (from m in list
                    select m.name).ToList();
        }

        public List<List<string>> GetNongyaoAtParentToString(List<tbl_nongyao> parentids)
        {
            List<List<string>> list = new List<List<string>>();
            for (int i = 0; i < parentids.Count; i++)
            {
                tbl_nongyao parent = parentids.ElementAt(i);
                List<tbl_nongyao> children = (from m in db.tbl_nongyaos
                                              where m.deleted == 0 && m.parentid == parent.id
                                              select m).ToList();
                list.Add(GetNames(children));
            }

            return list;
        }

        public static bool DeleteStoresForShopId(long shop_id)
        {
            try
            {
                var items = from m in db.tbl_stores
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

        public static bool DeleteStoremovingsForShopId(long shop_id)
        {
            try
            {
                var items = from m in db.tbl_storemovings
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

        public static bool DeleteUsingsForShopId(long shop_id)
        {
            try
            {
                var items = from m in db.tbl_usinglists
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

        public static string GetStoreNameForId(long id)
        {
            var item = (from m in db.tbl_stores
                        where m.deleted == 0 && m.id == id
                        select m).FirstOrDefault();
            if (item != null)
                return item.name;
            else
                return "";
        }

        public static long GetMainStoreId()
        {
            long shop_id = CommonModel.GetCurrentUserShopId();            

            var item = (from m in db.tbl_stores
                        where m.deleted == 0 && m.shop_id == shop_id
                        orderby m.id
                            select m).FirstOrDefault();
            if (item != null)
                return item.id;
            else
                return 0;
        }

        public static long GetTotalCountIntoMain(DateTime startdate, DateTime enddate)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            long main = GetMainStoreId();

            return (from m in db.tbl_storemovings
                    where m.deleted == 0 && m.shop_id == shop_id && m.recving_id == main && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.catalog_cnt);
        }

        public static long GetTotalCountOutfromMain(DateTime startdate, DateTime enddate)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            long main = GetMainStoreId();

            return (from m in db.tbl_storemovings
                    where m.deleted == 0 && m.shop_id == shop_id && m.sending_id == main && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.catalog_cnt);
        }

        public static long GetUsingLossTotalCount(DateTime startdate, DateTime enddate)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            
            return (from m in db.tbl_usinglists
                    where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.catalog_cnt);
        }

        public static decimal GetUsingLossTotalPrice(DateTime startdate, DateTime enddate)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();

            var list = (from m in db.tbl_usinglists
                        from l in db.tbl_remains
                        where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date &&
                            l.deleted == 0 && m.store_id == l.store_id && m.catalog_id == l.catalog_id && m.standard_id == l.standard_id && m.largenumber == l.largenumber
                        select new
                        {
                            id = m.id,
                            buying_id = l.id,
                            catalog_cnt = m.catalog_cnt,
                            price = l.origin_price
                        }).ToList();

            //var list = (from m in db.tbl_usinglists
            //            from l in db.tbl_buyings
            //            from k in db.tbl_tickets
            //            where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate &&
            //                l.deleted == 0 && m.store_id == l.store_id && m.catalog_id == l.catalog_id && m.standard_id == l.standard_id && m.largenumber == l.largenumber &&
            //                k.deleted == 0 && l.ticket_id == k.id && k.type == 0
            //            select new
            //            {
            //                id = m.id,
            //                buying_id = l.id,
            //                catalog_cnt = m.catalog_cnt,
            //                price = l.price
            //            }).ToList();

            return (from m in list
                        group m by m.id into g
                        select new {
                            id = g.Key,
                            catalog_cnt = g.OrderByDescending(l => l.buying_id).FirstOrDefault().catalog_cnt,
                            price = g.OrderByDescending(l => l.buying_id).FirstOrDefault().price
                        }).Sum(m => m.catalog_cnt * m.price);
        }
    }
}
