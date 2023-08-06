using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Web;
using NongYaoService.ServiceLibrary;

namespace NongYaoService.ServiceModel
{
    #region data models
    [DataContract]
    public class UnitInfo
    {
        [DataMember(Name = "unit_id", Order = 1)]
        public long bid { get; set; }

        [DataMember(Name = "unit", Order = 2)]
        public string unit { get; set; }
    }

    [DataContract]
    public class NongYaoKindInfo
    {
        [DataMember(Name = "id", Order = 1)]
        public long id { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string name { get; set; }
    }

    [DataContract]
    public class RegionInfo
    {
        [DataMember(Name = "region_id", Order = 1)]
        public long bid { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string name { get; set; }
    }

    [DataContract]
    public class StoreInfo
    {
        [DataMember(Name = "store_id", Order = 1)]
        public long bid { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string name { get; set; }
    }

    [DataContract]
    public class CustomerInfo
    {
        [DataMember(Name = "customer_id", Order = 1)]
        public long customer_id { get; set; }
        [DataMember(Name = "name", Order = 2)]
        public string name { get; set; }
        [DataMember(Name = "phone", Order = 3)]
        public string phone { get; set; }
    }


    [DataContract]
    public class SupplyInfo
    {
        [DataMember(Name = "supply_id", Order = 1)]
        public long bid { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string name { get; set; }
    }

    [DataContract]
    public class UserInfo
    {
        [DataMember(Name = "uid", Order = 1)]
        public long bid { get; set; }

        [DataMember(Name = "name", Order = 2)]
        public string name { get; set; }
    }

    [DataContract]
    public class ShopInfo
    {
        [DataMember(Name = "name", Order = 1)]
        public string name { get; set; }
        [DataMember(Name = "nickname", Order = 2)]
        public string nickname { get; set; }
        [DataMember(Name = "address", Order = 3)]
        public string addr { get; set; }
        [DataMember(Name = "region", Order = 4)]
        public long region { get; set; }
        [DataMember(Name = "username", Order = 5)]
        public string username { get; set; }
        [DataMember(Name = "mobile_phone", Order = 6)]
        public string mobile_phone { get; set; }
        [DataMember(Name = "phone", Order = 7)]
        public string phone { get; set; }
    }

    [DataContract]
    public class ShopUserDetailInfo
    {
        [DataMember(Name = "uid", Order = 1)]
        public long uid { get; set; }
        [DataMember(Name = "userid", Order = 2)]
        public string userid { get; set; }
        [DataMember(Name = "username", Order = 3)]
        public string username { get; set; }
        [DataMember(Name = "phone", Order = 4)]
        public string phone { get; set; }
        [DataMember(Name = "role", Order = 5)]
        public string role { get; set; }
        [DataMember(Name = "password", Order = 6)]
        public string password { get; set; }
    }

    [DataContract]
    public class StoreDetailInfo
    {
        [DataMember(Name = "store_id", Order = 1)]
        public long store_id { get; set; }
        [DataMember(Name = "name", Order = 2)]
        public string name { get; set; }
        [DataMember(Name = "uid", Order = 3)]
        public string uid { get; set; }
        [DataMember(Name = "uname", Order = 4)]
        public string uname { get; set; }
    }

    [DataContract]
    public class CatalogRemainListInfo
    {
        [DataMember(Name = "store_id", Order = 1)]
        public long store_id { get; set; }
        [DataMember(Name = "catalog_num", Order = 2)]  
        public string catalog_num { get; set; }
        [DataMember(Name = "storename", Order = 3)]
        public string storename { get; set; }
        [DataMember(Name = "standard_id", Order = 4)]
        public long standard_id { get; set; }
        [DataMember(Name = "standard_name", Order = 5)]
        public string standard_name { get; set; }
        [DataMember(Name = "quantity", Order = 6)]
        public long quantity { get; set; }
        [DataMember(Name = "catalog_name", Order = 7)]
        public string catalog_name { get; set; }
        [DataMember(Name = "largenumber", Order = 8)]
        public string largenumber { get; set; }
    }

    [DataContract]
    public class OtherPayListInfo
    {
        [DataMember(Name = "otherpay_id", Order = 1)]
        public long otherpay_id { get; set; }
        [DataMember(Name = "date", Order = 2)]
        public string date { get; set; }
        [DataMember(Name = "price", Order = 3)]
        public decimal price { get; set; }
        [DataMember(Name = "type", Order = 4)]
        public byte type { get; set; }
        [DataMember(Name = "reason", Order = 5)]
        public string reason { get; set; }
    }

    [DataContract]
    public class PaymentLogInfo
    {
        [DataMember(Name = "payment_id", Order = 1)]
        public long payment_id { get; set; }
        [DataMember(Name = "date", Order = 2)]
        public string date { get; set; }
        [DataMember(Name = "price", Order = 3)]
        public decimal price { get; set; }
        [DataMember(Name = "type", Order = 4)]
        public byte type { get; set; }
        [DataMember(Name = "customer_name", Order = 5)]
        public string customer_name { get; set; }
        [DataMember(Name = "customer_phone", Order = 6)]
        public string customer_phone { get; set; }
        [DataMember(Name = "customer_id", Order = 7)]
        public long customer_id { get; set; }
    }

    [DataContract]
    public class CatalogRemainInfo
    {
        [DataMember(Name = "catalog_id", Order = 1)]
        public long catalog_id { get; set; }
        [DataMember(Name = "catalog_num", Order = 2)]
        public string catalog_num { get; set; }
        [DataMember(Name = "catalog_name", Order = 3)]
        public string catalog_name { get; set; }
        //[DataMember(Name = "quantity", Order = 4)]
        //public long quantity { get; set; }
    }

    [DataContract]
    public class BankTableInfo
    {
        [DataMember(Name = "ticket_num", Order = 1)]
        public string ticket_num { get; set; }
        [DataMember(Name = "date", Order = 2)]
        public string date { get; set; }
        [DataMember(Name = "price", Order = 3)]
        public decimal price { get; set; }
        [DataMember(Name = "type", Order = 4)]
        public byte type { get; set; }
    }

    [DataContract]
    public class StandardInfo
    {
        [DataMember(Name = "standard_id", Order = 1)]
        public long standard_id { get; set; }
        [DataMember(Name = "standard", Order = 2)]
        public string standard { get; set; }
    }

    [DataContract]
    public class SaleHistoryInfo
    {
         [DataMember(Name = "ticket_id", Order = 1)]
        public long ticket_id { get; set; }
         [DataMember(Name = "ticket_num", Order = 2)]
         public string ticket_num { get; set; }
         [DataMember(Name = "saledate", Order = 3)]
         public string saledate { get; set; }
         [DataMember(Name = "customer_name", Order = 4)]
         public string customer_name { get; set; }
         [DataMember(Name = "totalmoney", Order = 5)]
         public decimal totalmoney { get; set; }
         [DataMember(Name = "realmoney", Order = 6)]
         public decimal realmoney { get; set; }
         [DataMember(Name = "type", Order = 7)]
         public string type { get; set; }
    }

    [DataContract]
    public class SaleDetailInfo
    {
        [DataMember(Name = "catalog_name", Order = 1)]
        public string catalog_name { get; set; }
        [DataMember(Name = "standard", Order = 2)]
        public string standard { get; set; }
        [DataMember(Name = "price", Order = 3)]
        public decimal price { get; set; }
        [DataMember(Name = "count", Order = 4)]
        public int count { get; set; }
        [DataMember(Name = "total", Order = 5)]
        public decimal total { get; set; }
    }

    [DataContract]
    public class StandardQuantityInfo
    {
        [DataMember(Name = "standard_id", Order = 1)]
        public long standard_id { get; set; }
        [DataMember(Name = "standard_string", Order = 2)]
        public string standard_string { get; set; }
        [DataMember(Name = "LargenumberQuantityInfo", Order = 3)]
        public List<LargenumberQuantityInfo> data { get; set; }
    }

    [DataContract]
    public class LargenumberQuantityInfo
    {
        [DataMember(Name = "largenumber", Order = 1)]
        public string largenumber { get; set; }
        [DataMember(Name = "quantity", Order = 2)]
        public long quantity { get; set; }
    }

    [DataContract]
    public class InOutLogInfo
    {
        [DataMember(Name = "date", Order = 1)]
        public string date { get; set; }
        [DataMember(Name = "saleincome", Order = 2)]
        public decimal saleincome { get; set; }
        [DataMember(Name = "othericome", Order = 3)]
        public decimal othericome { get; set; }
        [DataMember(Name = "saleorigin", Order = 4)]
        public decimal saleorigin { get; set; }
        [DataMember(Name = "otheroutcome", Order = 5)]
        public decimal otheroutcome { get; set; }
        [DataMember(Name = "change", Order = 6)]
        public decimal change { get; set; }
        [DataMember(Name = "profit", Order = 7)]
        public decimal profit { get; set; }
    }

    [DataContract]
    public class InOutLogDetailInfo
    {
        [DataMember(Name = "no", Order = 1)]
        public int no { get; set; }
        [DataMember(Name = "description", Order = 2)]
        public string description { get; set; }
        [DataMember(Name = "money", Order = 3)]
        public decimal money { get; set; }
        [DataMember(Name = "username", Order = 4)]
        public string username { get; set; }
        [DataMember(Name = "reason", Order = 5)]
        public string reason { get; set; }
    }


    [DataContract]
    public class FinanceDetailInfo
    {
        [DataMember(Name = "desc", Order = 1)]
        public string desc { get; set; }
        [DataMember(Name = "price", Order = 2)]
        public decimal price { get; set; }
        [DataMember(Name = "user_id", Order = 3)]
        public long user_id { get; set; }
        [DataMember(Name = "reason", Order = 4)]
        public string reason { get; set; }
    }

    [DataContract]
    public class PaymentDetailLogInfo
    {
        [DataMember(Name = "payment_id", Order = 1)]
        public long payment_id { get; set; }
        [DataMember(Name = "date", Order = 2)]
        public string date { get; set; }
        [DataMember(Name = "ticket_num", Order = 3)]
        public string ticket_num { get; set; }
        [DataMember(Name = "content", Order = 4)]
        public string content { get; set; }
        [DataMember(Name = "type", Order = 5)]
        public byte type { get; set; }
        [DataMember(Name = "price", Order = 6)]
        public decimal price { get; set; }
        [DataMember(Name = "reason", Order = 7)]
        public string reason { get; set; }
    }

    [DataContract]
    public class RealPaymentListInfo
    {
        [DataMember(Name = "date", Order = 1)]
        public string date { get; set; }
        [DataMember(Name = "customer_name", Order = 2)]
        public string customer_name { get; set; }
        [DataMember(Name = "type", Order = 3)]
        public byte type { get; set; }
        [DataMember(Name = "price", Order = 4)]
        public decimal price { get; set; }
        [DataMember(Name = "change", Order = 5)]
        public decimal change { get; set; }
        [DataMember(Name = "etc", Order = 6)]
        public string etc { get; set; }
    }

    [DataContract]
    public class MoneybankListInfo
    {
        [DataMember(Name = "date", Order = 1)]
        public string date { get; set; }
        [DataMember(Name = "money", Order = 2)]
        public decimal money { get; set; }
        [DataMember(Name = "bank", Order = 3)]
        public decimal bank { get; set; }
        [DataMember(Name = "sum", Order = 4)]
        public decimal sum { get; set; }
    }

    [DataContract]
    public class MoneybankDetailInfo
    {
        [DataMember(Name = "ticket_id", Order = 1)]
        public long ticket_id { get; set; }
        [DataMember(Name = "paytype", Order = 2)]
        public byte paytype { get; set; }
        [DataMember(Name = "type", Order = 3)]
        public byte type { get; set; }
        [DataMember(Name = "price", Order = 4)]
        public decimal price { get; set; }
        [DataMember(Name = "reason", Order = 5)]
        public string reason { get; set; }
        [DataMember(Name = "ticket_num", Order = 6)]
        public string ticket_num { get; set; }
    }

    [DataContract]
    public class ShopStatisticsInfo
    {
        [DataMember(Name = "region_name", Order = 1)]
        public string region_name { get; set; }
        [DataMember(Name = "shop_id", Order = 2)]
        public long shop_id { get; set; }
        [DataMember(Name = "shop_name", Order = 3)]
        public string shop_name { get; set; }
        [DataMember(Name = "shop_lawman", Order = 4)]
        public string shop_lawman { get; set; }
        [DataMember(Name = "sale_count", Order = 5)]
        public decimal sale_count { get; set; }
        [DataMember(Name = "remain_count", Order = 6)]
        public decimal remain_count { get; set; }
    }

    [DataContract]
    public class CatalogStatisticsInfo
    {
        [DataMember(Name = "catalog_id", Order = 1)]
        public long catalog_id { get; set; }
        [DataMember(Name = "catalog_name", Order = 2)]
        public string catalog_name { get; set; }
        [DataMember(Name = "product", Order = 3)]
        public string product { get; set; }
        [DataMember(Name = "standard", Order = 4)]
        public string standard { get; set; }
        [DataMember(Name = "standard_id", Order = 5)]
        public long standard_id { get; set; }
        [DataMember(Name = "remain_count", Order = 6)]
        public decimal remain_count { get; set; }
        [DataMember(Name = "sale_count", Order = 7)]
        public decimal sale_count { get; set; }
    }

    [DataContract]
    public class CatalogStatisticsDetailInfo
    {
        [DataMember(Name = "largenumber", Order = 1)]
        public string largenumber { get; set; }
        [DataMember(Name = "product_date", Order = 2)]
        public string product_date { get; set; }
        [DataMember(Name = "avail_date", Order = 3)]
        public int avail_date { get; set; }
        [DataMember(Name = "remain_count", Order = 4)]
        public long remain_count { get; set; }
        [DataMember(Name = "sale_count", Order = 5)]
        public long sale_count { get; set; }
        [DataMember(Name = "total_count", Order = 6)]
        public long total_count { get; set; }
        [DataMember(Name = "shop_name", Order = 7)]
        public string shop_name { get; set; }
    }

    [DataContract]
    public class RegionBarGraphInfo
    {
        [DataMember(Name = "region_name", Order = 1)]
        public string region_name { get; set; }
        [DataMember(Name = "sale_count", Order = 2)]
        public long sale_count { get; set; }
        [DataMember(Name = "remain_count", Order = 3)]
        public long remain_count { get; set; }
    }

    [DataContract]
    public class MonthLineGraphInfo
    {
        [DataMember(Name = "month", Order = 1)]
        public int month { get; set; }
        [DataMember(Name = "sale_count", Order = 2)]
        public long sale_count { get; set; }
    }

    [DataContract]
    public class NongyaoPieGraphInfo
    {
        [DataMember(Name = "nongyao_name", Order = 1)]
        public string nongyao_name { get; set; }
        [DataMember(Name = "sale_count", Order = 2)]
        public long sale_count { get; set; }
    }
    #endregion

    public class DBNongYao
    {
        private int page_per_count = 10;

        #region basedata Operation

        /************* Get data for DB *************/
        public NongYaoResponseData GetUnitList(ServiceDBDataContext db)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<UnitInfo> retData = new List<UnitInfo>();
                List<tbl_unit> unitlist = (from m in db.tbl_units
                                           where m.deleted == 0
                                           select m).ToList();
             
                foreach (tbl_unit unit in unitlist)
                {
                    UnitInfo newitem = new UnitInfo();
                    newitem.bid = unit.id;
                    newitem.unit = DBCommon.SNN(unit.name);
                    retData.Add(newitem);
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retData.Count(),
                    data = retData,
                };
                       
             }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        //public NongYaoResponseData GetNongYaoKindList(ServiceDBDataContext db)
        //{
        //    NongYaoResponseData result = new NongYaoResponseData();

        //    try
        //    {
        //        List<NongYaoKindInfo> retData = new List<NongYaoKindInfo>();
        //        List<tbl_nongyao> nongyaolist = (from m in db.tbl_nongyaos
        //                                   where m.deleted == 0
        //                                   select m).ToList();

        //        foreach (tbl_nongyao nongyao in nongyaolist)
        //        {
        //            NongYaoKindInfo newitem = new NongYaoKindInfo();
        //            newitem.id = nongyao.id;
        //            newitem.name = nongyao.name;
        //            retData.Add(newitem);
        //        }

        //        result.Result = NONGYAOERROR.ERR_SUCCESS;
        //        result.Data = new
        //        {
        //            count = retData.Count(),
        //            data = retData,
        //        };

        //    }
        //    catch (System.Exception ex)
        //    {
        //        NongYaoCommon.LogErrors(ex.ToString());
        //        result.Result = NONGYAOERROR.ERR_FAILURE;
        //    }
        //    return result;
        //}

        public NongYaoResponseData GetRegionList(ServiceDBDataContext db)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<RegionInfo> retData = new List<RegionInfo>();
                List<tbl_region> regionlist = (from m in db.tbl_regions
                                           where m.deleted == 0 && m.parentid != 0
                                           select m).ToList();
               
                foreach (tbl_region region in regionlist)
                {
                    RegionInfo newitem = new RegionInfo();
                    newitem.bid = region.id;
                    newitem.name = DBCommon.SNN(region.name);
                    retData.Add(newitem);
                }
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retData.Count(),
                    data = retData,
                };
              
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetStoreList(ServiceDBDataContext db, long shop_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<StoreInfo> retData = new List<StoreInfo>();
                List<tbl_store> storelist = (from m in db.tbl_stores
                                             where m.shop_id == shop_id && m.deleted == 0
                                               select m).ToList();
                
                foreach (tbl_store store in storelist)
                {
                    StoreInfo newitem = new StoreInfo();                       
                    newitem.bid = store.id;
                    newitem.name = DBCommon.SNN(store.name);
                    retData.Add(newitem);                     
                }
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retData.Count(),
                    data = retData,
                };
               
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetCustomerList(ServiceDBDataContext db, long shop_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<tbl_customerlist> customerlist = (from m in db.tbl_customerlists
                                                        where m.deleted == 0 && m.shop_id == shop_id
                                                        select m).ToList();

                List<CustomerInfo> retData = new List<CustomerInfo>();
                foreach (tbl_customerlist customer in customerlist)
                {
                    retData.Add(new CustomerInfo { 
                        customer_id = customer.id,
                        name = DBCommon.SNN(customer.name),
                        phone = DBCommon.SNN(customer.phone)
                    });
                }
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retData.Count(),
                    data = retData,
                };

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetCustomerInfo(ServiceDBDataContext db, long shop_id, string name, string phone)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_customerlist customer = new tbl_customerlist();

                if (name != "") {
                    customer = (from m in db.tbl_customerlists
                                where m.shop_id == shop_id && m.deleted == 0 && m.name == name
                                select m).FirstOrDefault();
                    if (customer != null)
                    {
                        name = customer.name;
                        phone = customer.phone;
                    }
                    else
                    {
                        name = "";
                        phone = "";
                    }                    

                }
                else if (phone != "")
                {
                    customer = (from m in db.tbl_customerlists
                                where m.shop_id == shop_id && m.deleted == 0 && m.phone == phone
                                select m).FirstOrDefault();
                    if (customer != null)
                    {
                        name = customer.name;
                        phone = customer.phone;
                    }
                    else
                    {
                        name = "";
                        phone = "";
                    }  
                }

                
                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                    result.Data = new
                    {
                        data = new
                        {
                            name =  name,
                            phone = phone
                        },
                    };
                
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetSupplyList(ServiceDBDataContext db, long shop_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<SupplyInfo> retData = new List<SupplyInfo>();
                List<tbl_supply> supplylist = (from m in db.tbl_supplies
                                               where m.shop_id == shop_id && m.deleted == 0
                                             select m).ToList();
              
                foreach (tbl_supply supply in supplylist)
                {
                    SupplyInfo newitem = new SupplyInfo();                      
                    newitem.bid = supply.id;
                    newitem.name = DBCommon.SNN(supply.name);
                    retData.Add(newitem);
                }
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retData.Count(),
                    data = retData,
                };              
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetUserList(ServiceDBDataContext db, long shop_id, string role)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                if (role == null)
                {
                    List<UserInfo> retData = new List<UserInfo>();
                    List<tbl_user> userlist = (from m in db.tbl_users
                                               where m.shop_id == shop_id && m.deleted == 0 && m.role != "admin"
                                                   select m).ToList();
                
                    foreach (tbl_user user in userlist)
                    {
                        UserInfo newitem = new UserInfo();  
                        newitem.bid = user.id;
                        newitem.name = DBCommon.SNN(user.name);
                        retData.Add(newitem);
                    }

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                    result.Data = new
                    {
                        count = retData.Count(),
                        data = retData,
                    }; 
                }
                else
                {
                    List<UserInfo> retData = new List<UserInfo>();
                    List<tbl_user> userlist = (from m in db.tbl_users
                                               where (m.shop_id == shop_id && m.deleted == 0 && (m.role.Contains(role) == true))
                                               select m).ToList();

                    foreach (tbl_user user in userlist)
                    {
                        UserInfo newitem = new UserInfo();
                        newitem.bid = user.id;
                        newitem.name = DBCommon.SNN(user.name);
                        retData.Add(newitem);
                    }

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                    result.Data = new
                    {
                        count = retData.Count(),
                        data = retData,
                    }; 
                }
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetShopInfo(ServiceDBDataContext db, long shop_id, string uid)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {                
                tbl_shop shop = (from m in db.tbl_shops
                                 where m.id == shop_id && m.pass == 1 && m.deleted == 0
                                 select m).FirstOrDefault();

                if (shop != null)
                {
                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                    result.Data = new
                    {
                        name = shop.name,
                        nickname = DBCommon.SNN(shop.nickname),
                        addr = DBCommon.SNN(shop.addr),
                        region = (long)shop.region,
                        username = DBCommon.SNN(shop.username),
                        mobile_phone = DBCommon.SNN(shop.mobile_phone),
                        phone = DBCommon.SNN(shop.phone),
                        longitude = shop.longitude != null ? shop.longitude : 0,
                        latitude = shop.latitude != null ? shop.latitude : 0,
                        permit_id = DBCommon.SNN(shop.permit_id)
                    };
                }
                else
                {
                    result.Result = NONGYAOERROR.ERR_FAILURE;
                }
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetShopUserDetailList(ServiceDBDataContext db, long shop_id, int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();
          
            try
            {
                List<ShopUserDetailInfo> retDatas = db.tbl_users
                    .Where(m => m.shop_id == shop_id && m.role.Contains("admin") == false && m.deleted == 0)
                    .Skip((pagenum - 1) * page_per_count).Take(page_per_count)
                    .Select(row => new ShopUserDetailInfo
                    {
                        uid = row.id,
                        username = DBCommon.SNN(row.name),
                        userid = row.userid,
                        phone = DBCommon.SNN(row.phone),
                        role = DBCommon.SNN(row.role),
                        password = DBCommon.SNN(row.password)
                    }).ToList();
                             
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retDatas.Count(),
                    data = retDatas,
                };               
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetStoreDetailList(ServiceDBDataContext db, long shop_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<StoreDetailInfo> retDatas = db.tbl_stores
                    .Where(m => m.shop_id == shop_id && m.deleted == 0)                    
                    .Select( row => new StoreDetailInfo
                    {
                        store_id = row.id,
                        name = DBCommon.SNN(row.name),                      
                        uid = row.uid,                       
                    }).ToList();

                foreach (StoreDetailInfo item in retDatas)
                {
                    string[] ids = item.uid.Split(',');
                    string user = "";
                    foreach (string id_data in ids)
                    {
                        tbl_user userInfo = (from m in db.tbl_users
                                             where m.deleted == 0 && m.id == long.Parse(id_data)
                                             select m).FirstOrDefault();
                                        

                        if (userInfo != null)
                            user = user + userInfo.name + ", ";
                    }
                    if (user.Length > 0)
                        user = user.Substring(0, user.Length - 2);
                    item.uname = user;
                }
           
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retDatas.Count(),
                    data = retDatas,
                };              
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetCatalogRemainList(ServiceDBDataContext db, 
            long shop_id, 
            long catalog_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                           where m.deleted == 0 && m.id == catalog_id
                                           select m).FirstOrDefault();

                string catalog_num = "", catalog_name = "";
                if (catalogitem != null)
                {
                    catalog_num = catalogitem.catalog_num;
                    catalog_name = catalogitem.name;
                }

                var alllist = db.tbl_remains
                                   .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.shop_id == shop_id)
                                   .GroupBy(m => new { store_id = m.store_id, standard_id = m.standard_id, largenumber = m.largenumber })
                                    .Select(g => new
                                    {
                                        store_id = g.Key.store_id,
                                        standard_id = g.Key.standard_id,
                                        largenumber = g.Key.largenumber,
                                        quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
                                    }).ToList()
                                    .GroupBy(m => new { store_id = m.store_id, standard_id = m.standard_id, largenumeber = m.largenumber })
                                    .Select(g => new
                                    {
                                        store_id = g.Key.store_id,
                                        standard_id = g.Key.standard_id,
                                        largenumber = g.Key.largenumeber,
                                        quantity = g.Sum(l => l.quantity)
                                    }).ToList();

                List<CatalogRemainListInfo> retdata = new List<CatalogRemainListInfo>();
                foreach (var item in alllist)
                {
                    tbl_store storeitem = (from m in db.tbl_stores
                                           where m.deleted == 0 && m.id == item.store_id
                                           select m).FirstOrDefault();
                    string storename = "";
                    if (storeitem != null)
                        storename = storeitem.name;

                    string standard = "";
                    tbl_standard standarditem = (from m in db.tbl_standards
                                                 where m.deleted == 0 && m.id == item.standard_id
                                                 select m).FirstOrDefault();
                    if (standarditem != null)
                    {
                        tbl_unit unititem = (from m in db.tbl_units
                                             where m.deleted == 0 && m.id == standarditem.unit_id
                                             select m).FirstOrDefault();
                        if (unititem != null)
                            standard = standarditem.quantity + (standarditem.mass == 0 ? "克" : "毫升") + "/" + unititem.name;
                    }

                    retdata.Add(new CatalogRemainListInfo
                    {
                        store_id = item.store_id,
                        catalog_num = catalog_num,
                        storename = storename,
                        standard_id = item.standard_id,    
                        largenumber = DBCommon.SNN(item.largenumber),
                        catalog_name = catalog_name,
                        standard_name = standard,
                        quantity = item.quantity
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count,
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetCatalogRemainListWithBarcode(ServiceDBDataContext db, long shop_id, string barcode)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                if (barcode == "")
                {
                    result = GetCatalogRemainListAll(db, shop_id);
                }
                else
                {
                tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                       where m.deleted == 0 && m.register_id == barcode
                                       select m).FirstOrDefault();
                if(catalogitem != null)
                {
                    result = GetCatalogRemainList(db, shop_id, catalogitem.id);
                }
                else
                {
                    result.Result = NONGYAOERROR.ERR_NOCATALOG;
                }
            }
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetCatalogRemainListAll(ServiceDBDataContext db,
            long shop_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<tbl_catalog> catalogitem_list = (from m in db.tbl_catalogs
                                           where m.deleted == 0
                                           select m).ToList();

                List<CatalogRemainListInfo> retdata = new List<CatalogRemainListInfo>();

                foreach (tbl_catalog catalogitem in catalogitem_list)
                {
                    string catalog_num = "", catalog_name = "";
                    if (catalogitem != null)
                    {
                        catalog_num = catalogitem.catalog_num;
                        catalog_name = catalogitem.name;
                    }

                    var alllist = db.tbl_remains
                                       .Where(m => m.deleted == 0 && m.catalog_id == catalogitem.id && m.shop_id == shop_id)
                                       .GroupBy(m => new { store_id = m.store_id, standard_id = m.standard_id, largenumber = m.largenumber })
                                        .Select(g => new
                                        {
                                            store_id = g.Key.store_id,
                                            standard_id = g.Key.standard_id,
                                            largenumber = g.Key.largenumber,
                                            quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
                                        }).ToList()
                                        .GroupBy(m => new { store_id = m.store_id, standard_id = m.standard_id, largenumeber = m.largenumber })
                                        .Select(g => new
                                        {
                                            store_id = g.Key.store_id,
                                            standard_id = g.Key.standard_id,
                                            largenumber = g.Key.largenumeber,
                                            quantity = g.Sum(l => l.quantity)
                                        }).ToList();

                    
                    foreach (var item in alllist)
                    {
                        tbl_store storeitem = (from m in db.tbl_stores
                                               where m.deleted == 0 && m.id == item.store_id
                                               select m).FirstOrDefault();
                        string storename = "";
                        if (storeitem != null)
                            storename = storeitem.name;

                        string standard = "";
                        tbl_standard standarditem = (from m in db.tbl_standards
                                                     where m.deleted == 0 && m.id == item.standard_id
                                                     select m).FirstOrDefault();
                        if (standarditem != null)
                        {
                            tbl_unit unititem = (from m in db.tbl_units
                                                 where m.deleted == 0 && m.id == standarditem.unit_id
                                                 select m).FirstOrDefault();
                            if (unititem != null)
                                standard = standarditem.quantity + (standarditem.mass == 0 ? "克" : "毫升") + "/" + unititem.name;
                        }
                        if (item.quantity > 0)
                        {
                            retdata.Add(new CatalogRemainListInfo
                            {
                                store_id = item.store_id,
                                catalog_num = catalog_num,
                                storename = storename,
                                standard_id = item.standard_id,
                                largenumber = DBCommon.SNN(item.largenumber),
                                catalog_name = catalog_name,
                                standard_name = standard,
                                quantity = item.quantity
                            });
                        }                        
                    }
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count,
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetOtherPayList(ServiceDBDataContext db, 
            long shop_id, 
            byte type, 
            string start_date,
            string end_date,
            int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                var alllist = db.tbl_otherpays
                    .Where(p => p.deleted == 0 && p.shop_id == shop_id && p.regtime.Date >= startdate && p.regtime.Date <= enddate &&
                            (type == 2 || (type != 2 && p.type == type)))
                    .OrderByDescending(p => p.id)
                    .Skip((pagenum - 1) * page_per_count).Take(page_per_count)
                    .ToList();

                List<OtherPayListInfo> retdata = new List<OtherPayListInfo>();
                foreach (var item in alllist)
                {
                    retdata.Add(new OtherPayListInfo
                    {
                        otherpay_id = item.id,
                        date = String.Format("{0:yyyy-MM-dd}", item.regtime),
                        price = item.price,
                        type = item.type,
                        reason = DBCommon.SNN(item.reason)
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetTicketNumber(ServiceDBDataContext db, long shop_id, int type)
        {
            NongYaoResponseData result = new NongYaoResponseData();
            string[] sale_type = { "_IN_", "_IB_", "_OT_", "_OB_" };

            try
            {

                tbl_shop shop = (from m in db.tbl_shops
                                 where m.id == shop_id && m.deleted == 0
                                 select m).FirstOrDefault();
               
                List<tbl_ticket> ticketlist = (from m in db.tbl_tickets
                                               where m.ticketnum.Contains(shop.nickname + sale_type[type] + string.Format("{0:yyyyMMdd}", DateTime.Now) + "_") == true && m.deleted == 0
                                               select m).ToList();
               
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    data = (shop.nickname).ToUpper() + sale_type[type] + string.Format("{0:yyyyMMdd}", DateTime.Now) + "_" + string.Format("{0:000000}", ticketlist.Count + 1),
                  date = string.Format("{0:yyyy-MM-dd}", DateTime.Now), 
                };
                
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetCatalogInfoFromBarcode(ServiceDBDataContext db,
            long shop_id,
            string barcode)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_shop shop = (from m in db.tbl_shops
                                 where m.deleted == 0 && m.id == shop_id
                                 select m).FirstOrDefault();
                long region_id = 0;
                if (shop != null)
                {
                    tbl_region item = (from m in db.tbl_regions
                                       where m.deleted == 0 && m.id == shop.region
                                       select m).FirstOrDefault();
                    if (item != null)
                    {
                        if (item.parentid == 0)
                            region_id = item.id;
                        else
                            region_id = (long)item.parentid;
                    }
                }

                
                tbl_catalog catalog = (from m in db.tbl_catalogs
                                       where m.register_id == barcode && m.deleted == 0 && m.pass == 1 && ((m.level == 0 && m.region_id == region_id) || (shop.level == 1 && m.level == 1 && m.shop_id == shop_id))
                                       select m).FirstOrDefault();

                if (catalog != null)
                {
                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                    result.Data = new
                    {
                        catalog_id = catalog.id,
                        catalog_name = DBCommon.SNN(catalog.name),
                        catalog_num = DBCommon.SNN(catalog.catalog_num),
                        avail_date = catalog.avail_date
                    };
                }
                else
                    result.Result = NONGYAOERROR.ERR_NOCATALOG;
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetCatalogListFromStore(ServiceDBDataContext db, 
            long shop_id, 
            long store_id,
            string search_name)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                if (!string.IsNullOrEmpty(search_name))
                    search_name = search_name.ToLower();

                var alllist = db.tbl_remains
                                   .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.store_id == store_id)
                                   .GroupBy(m => new { catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber })
                                    .Select(g => new
                                    {
                                        catalog_id = g.Key.catalog_id,
                                        standard_id = g.Key.standard_id,
                                        largenumber = g.Key.largenumber,
                                        quantity = g.OrderByDescending(m => m.id).FirstOrDefault().quantity
                                    }).ToList()
                                    .GroupBy(m => m.catalog_id)
                                    .Select(g => new
                                    {
                                        catalog_id = g.Key,
                                        quantity = g.Sum(l => l.quantity)
                                    }).ToList();

                List<CatalogRemainInfo> retdata = new List<CatalogRemainInfo>();
                foreach (var item in alllist)
                {         
                   tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                 where m.id == item.catalog_id && m.deleted == 0
                                 select m).FirstOrDefault();

                   string catalog_num = "", catalog_name = "", catalog_nickname = "", catalog_registerid = "";
                   if (catalogitem != null)
                    {
                        catalog_num = catalogitem.catalog_num;
                        catalog_name = catalogitem.name;
                        catalog_nickname = catalogitem.nickname;
                        catalog_registerid = catalogitem.register_id;
                    }

                   if (catalog_nickname.ToLower().Contains(search_name) || catalog_registerid.ToLower().Contains(search_name))
                   {
                       retdata.Add(new CatalogRemainInfo
                       {
                           catalog_id = item.catalog_id,
                           catalog_num = catalog_num,
                           catalog_name = catalog_name/*,
                           quantity = item.quantity*/
                       });
                   }
                }      

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count,
                    data = retdata,
                };              
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }        
                        
        public NongYaoResponseData GetInOutLog(ServiceDBDataContext db, long shop_id, string start_date, string end_date, int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;
                decimal total_profit = 0, profit = 0;
                List<InOutLogInfo> retdata = new List<InOutLogInfo>();
                for (DateTime cur_date = enddate; cur_date >= startdate; cur_date = cur_date.AddDays(-1))
                {
                    List<tbl_remain> remaininfo = (from m in db.tbl_remains
                                                   where m.deleted == 0 && ((DateTime)m.regtime).Date == cur_date.Date && m.shop_id == shop_id
                                                   select m).ToList();
                    decimal saleincome = 0, saleorigin = 0;
                    for (var i = 0; i < remaininfo.Count(); i++)
                    {
                        if (remaininfo[i].type == 2 || remaininfo[i].type == 3)
                        {
                            if (remaininfo[i].type == 2)
                            {
                                saleincome += (decimal)remaininfo[i].sum_price;     //sell
                                saleorigin += remaininfo[i].origin_price * remaininfo[i].catalog_cnt;
                            }
                            else if (remaininfo[i].type == 3)
                            {
                                saleincome -= (decimal)remaininfo[i].sum_price;     //sellback
                                saleorigin -= remaininfo[i].origin_price * remaininfo[i].catalog_cnt;
                            }
                        }
                    }

                    decimal otherincome = (from m in db.tbl_otherpays
                                           where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id && m.type == 1
                                           select m).ToList().Sum(m => (decimal)m.price);
                    decimal otherout = (from m in db.tbl_otherpays
                                        where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id && m.type == 0
                                        select m).ToList().Sum(m => (decimal)m.price);

                    decimal smallchange = (from m in db.tbl_changes
                                           where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id
                                           select m).ToList().Sum(m => (decimal)m.change);

                    profit = saleincome - saleorigin + otherincome - otherout - smallchange;
                    if (saleincome != 0 || saleorigin != 0 || otherincome != 0 || otherout != 0 || smallchange != 0)
                    {
                        retdata.Add(new InOutLogInfo
                        {
                            date = String.Format("{0:yyyy-MM-dd}", cur_date),
                            saleincome = saleincome,
                            othericome = otherincome,
                            saleorigin = saleorigin,
                            otheroutcome = otherout,
                            change = smallchange,
                            profit = profit
                        });
                    }

                    total_profit += profit;
                }

                retdata = retdata.Skip((pagenum - 1) * page_per_count).Take(page_per_count).ToList();
               
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    total_profit = total_profit,
                    count = retdata.Count,
                    data = retdata,
                };

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetInOutTotalProfit(ServiceDBDataContext db, long shop_id, string start_date, string end_date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;
                decimal total = 0, profit = 0;
                for (DateTime cur_date = startdate; cur_date <= enddate; cur_date = cur_date.AddDays(1))
                {
                    List<tbl_remain> remaininfo = (from m in db.tbl_remains
                                                   where m.deleted == 0 && ((DateTime)m.regtime).Date == cur_date.Date && m.shop_id == shop_id
                                                   select m).ToList();
                    decimal saleincome = 0, saleorigin = 0;
                    for (var i = 0; i < remaininfo.Count(); i++)
                    {
                        if (remaininfo[i].type == 2 || remaininfo[i].type == 3)
                        {
                            if (remaininfo[i].type == 2)
                            {
                                saleincome += (decimal)remaininfo[i].sum_price;     //sell
                                saleorigin += remaininfo[i].origin_price * remaininfo[i].catalog_cnt;
                            }
                            else if (remaininfo[i].type == 3)
                            {
                                saleincome -= (decimal)remaininfo[i].sum_price;     //sellback
                                saleorigin -= remaininfo[i].origin_price * remaininfo[i].catalog_cnt;
                            }
                        }
                    }

                    decimal otherincome = (from m in db.tbl_otherpays
                                           where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id && m.type == 1
                                           select m).ToList().Sum(m => (decimal)m.price);
                    decimal otherout = (from m in db.tbl_otherpays
                                        where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id && m.type == 0
                                        select m).ToList().Sum(m => (decimal)m.price);

                    decimal smallchange = (from m in db.tbl_changes
                                           where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id
                                           select m).ToList().Sum(m => (decimal)m.change);

                    profit = saleincome + otherincome - saleorigin - otherout - smallchange;
                    total += profit;
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    totalprofit = total,
                };

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }
                   
        public NongYaoResponseData GetBankCashLogList(ServiceDBDataContext db, long shop_id, string start_date, string end_date, int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;             

                //List<BankTableInfo> retDatas = db.tbl_banklists
                //    .Where(p => p.deleted == 0 && ((DateTime)p.regtime).Date >= startdate.Date && ((DateTime)p.regtime).Date <= enddate.Date)
                //    .OrderByDescending(m => m.id)
                //    .Join(db.tbl_tickets, m => m.ticket_id, l => l.id, (m, l) => new {bank = m, ticket = l})
                //    .Skip((pagenum - 1) * page_per_count).Take(page_per_count)
                //    .Select(row => new BankTableInfo
                //    {
                //        ticket_num = row.ticket.ticketnum,
                //        date = String.Format("{0:yyyy-MM-dd}", (DateTime)row.bank.regtime),                        
                //        price = (decimal)row.bank.price,
                //        type = row.bank.type
                //    }).ToList();
         
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                //result.Data = new
                //{
                //    count = retDatas.Count(),
                //    data = retDatas
                //};

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetPaymentLog(
            ServiceDBDataContext db,
            long shop_id,
            string customer_search,
            byte type,
            string start_date,
            string end_date,
            int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                List<tbl_payment> alllist = (from m in db.tbl_payments
                                                    where m.deleted == 0 && ((DateTime)m.regtime).Date <= enddate && ((DateTime)m.regtime).Date >= startdate && m.shop_id == shop_id && 
                                                    (type == 2 || (type != 2 && m.type == type)) && m.customer_type == 0
                                                    orderby m.regtime descending
                                                    select m).ToList();

                if (!string.IsNullOrEmpty(customer_search))
                    customer_search = customer_search.ToLower();

                List<PaymentLogInfo> retdata = new List<PaymentLogInfo>();
                foreach (var item in alllist)
                {
                    string customer_name = "", customer_phone = "";
                    if (item.ticket_type == 0 || item.ticket_type == 1)
                    {
                        long supply_id = item.customer_id;
                        tbl_supply supplyInfo = (from m in db.tbl_supplies
                                                 where m.deleted == 0 && m.id == supply_id
                                                 select m).FirstOrDefault();
                        if (supplyInfo != null)
                        {
                            customer_name = supplyInfo.name;
                            customer_phone = supplyInfo.contact_mobilephone;
                        }
                    }
                    else
                    {
                        if (item.customer_type == 0)
                        {
                            long customer_id = item.customer_id;
                            tbl_customerlist customerInfo = (from m in db.tbl_customerlists
                                                             where m.deleted == 0 && m.id == customer_id
                                                             select m).FirstOrDefault();
                            if (customerInfo != null)
                            {
                                customer_name = customerInfo.name;
                                customer_phone = customerInfo.phone;
                            }
                        }
                        else
                        {
                            long customer_id = item.customer_id;
                            tbl_shop shopInfo = (from m in db.tbl_shops
                                                 where m.deleted == 0 && m.id == customer_id
                                                 select m).FirstOrDefault();
                            if (shopInfo != null)
                            {
                                customer_name = shopInfo.name;
                                customer_phone = shopInfo.mobile_phone;
                            }
                        }
                    }
                    /*tbl_customerlist customeritem = (from m in db.tbl_customerlists
                                                     where m.deleted == 0 && m.id == item.customer_id
                                                     select m).FirstOrDefault();
                    if (customeritem != null) 
                    {
                        customer_name = customeritem.name;
                        customer_phone = customeritem.phone;
                    }*/

                    if (customer_name.ToLower().Contains(customer_search))
                    {
                        retdata.Add(new PaymentLogInfo
                        {
                            payment_id = item.id,
                            date = String.Format("{0:yyyy-MM-dd}", item.regtime),
                            price = item.price,
                            type = item.type,
                            customer_name = customer_name,
                            customer_phone = customer_phone
                        });
                    }
                }

                retdata = retdata.Skip((pagenum - 1) * page_per_count).Take(page_per_count).ToList();

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }


        /************ Post data for DB ************/
        public NongYaoResponseData EditShopInfo(
            ServiceDBDataContext db, 
            long uid,
            long shop_id,
            string nickname,
            string address,
            long region,
            string username,
            string mobile_phone,
            string phone)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_shop shop = (from m in db.tbl_shops
                                     where m.id == shop_id && m.deleted == 0
                                     select m).FirstOrDefault();
                if (shop != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    shop.nickname = nickname;
                    shop.addr = address;
                    shop.region = region;
                    shop.username = username;
                    shop.mobile_phone = mobile_phone;
                    shop.phone = phone;

                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;  
                }
                else
                    result.Result = NONGYAOERROR.ERR_FAILURE;     
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData EditShopPositionInfo(
            ServiceDBDataContext db,
            long uid,
            long shop_id,
            decimal longitude,
            decimal latitude)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_shop shop = (from m in db.tbl_shops
                                 where m.id == shop_id && m.deleted == 0
                                 select m).FirstOrDefault();
                if (shop != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    shop.longitude = longitude;
                    shop.latitude = latitude;
                    
                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else
                    result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData AddShopUser(
            ServiceDBDataContext db, 
            long shop_id,
            string name,
            string userid,
            string password,
            string phone,
            string role)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                long nshopUserRegisterId = 0;
                byte unique_flag = 1;

                List<tbl_user> userList = (from m in db.tbl_users
                                           select m).ToList();
               
                foreach (tbl_user u in userList)
                {
                    if (nshopUserRegisterId < u.id)
                        nshopUserRegisterId = u.id;
                    if (u.userid == userid)
                    {
                        unique_flag = 0;
                        break;
                    }
                }
                nshopUserRegisterId++;

                if(unique_flag == 1)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    tbl_user newuser = new tbl_user
                    {
                        id = nshopUserRegisterId,
                        shop_id = shop_id,
                        userid = userid,
                        password = password,
                        phone = phone,
                        role = role,
                        name = name,
                        regtime = DateTime.Now,
                        deleted = 0,
                    };
                    db.tbl_users.InsertOnSubmit(newuser);
                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                    result.Data = new
                    {
                        uid = nshopUserRegisterId,
                    };
                }
                else
                    result.Result = NONGYAOERROR.ERR_DUPLICATEUSERID;
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }
       
        public NongYaoResponseData EditShopUser(
            ServiceDBDataContext db,
            long shop_id,
            long uid,
            string name,
            string userid,
            string password,
            string phone,
            string role)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                byte unique_flag = 1;

                List<tbl_user> userList = (from m in db.tbl_users
                                           select m).ToList();

                foreach (tbl_user u in userList)
                {
                    if ((u.userid == userid) && (u.id != uid))
                    {
                        unique_flag = 0;
                        break;
                    }
                }
               
                tbl_user user = (from m in db.tbl_users
                                 where m.id == uid && m.deleted == 0
                                 select m).FirstOrDefault();
                if ( (unique_flag == 1) && (user != null))
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    user.shop_id = shop_id;
                    user.userid = userid;
                    user.password = password;
                    user.phone = phone;
                    user.role = role;
                    user.name = name;

                    db.SubmitChanges();
                    db.Transaction.Commit();
                   
                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else if(unique_flag == 0)
                    result.Result = NONGYAOERROR.ERR_DUPLICATEUSERID;                
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }
       
        public NongYaoResponseData DelShopUser(
            ServiceDBDataContext db,
            long shop_id,
            long uid)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_user user = (from m in db.tbl_users
                                 where m.id == uid && m.shop_id == shop_id && m.deleted == 0
                                 select m).FirstOrDefault();
                if (user != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    user.deleted = 1;

                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else
                    result.Result = NONGYAOERROR.ERR_FAILURE;                
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData AddStore(
           ServiceDBDataContext db,
           long shop_id,
           string storename,
           string uid)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                long nstoreRegisterId = 0;
                byte unique_flag = 1;

                List<tbl_store> storeList = (from m in db.tbl_stores
                                           select m).ToList();

                foreach (tbl_store s in storeList)
                {
                    if (nstoreRegisterId < s.id)
                        nstoreRegisterId = s.id;
                    if (s.name == storename)
                    {
                        unique_flag = 0;
                        break;
                    }
                }
                nstoreRegisterId++;

                if (unique_flag == 1)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();
                    tbl_store newstore = new tbl_store
                    {
                        id = nstoreRegisterId,
                        name = storename,
                        uid = uid,
                        shop_id = shop_id,
                        regtime = DateTime.Now,
                        deleted = 0,
                    };
                    db.tbl_stores.InsertOnSubmit(newstore);
                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                    result.Data = new
                    {
                        store_id = nstoreRegisterId,
                    };
                }
                else
                    result.Result = NONGYAOERROR.ERR_DUPLICATEUSERID;
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData EditStore(
            ServiceDBDataContext db,
            long shop_id,
            long store_id,
            string storename,
            string uid)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                byte unique_flag = 1;

                List<tbl_store> storeList = (from m in db.tbl_stores
                                           select m).ToList();

                foreach (tbl_store s in storeList)
                {
                    if ((s.name == storename) && (s.id != store_id))
                    {
                        unique_flag = 0;
                        break;
                    }
                }

                tbl_store store = (from m in db.tbl_stores
                                 where m.id == store_id && m.deleted == 0
                                 select m).FirstOrDefault();
                if ((unique_flag == 1) && (store != null))
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    store.name = storename;
                    store.uid = uid;

                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else if (unique_flag == 0)
                    result.Result = NONGYAOERROR.ERR_DUPLICATEUSERID;
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData DelStore(
            ServiceDBDataContext db,
            long shop_id,
            long store_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_store store = (from m in db.tbl_stores
                                 where m.id == store_id && m.shop_id == shop_id && m.deleted == 0
                                 select m).FirstOrDefault();
                if (store != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    store.deleted = 1;

                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else
                    result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }           
        
        public NongYaoResponseData BuyingCatalog(
            ServiceDBDataContext db,
            long shop_id,
            long uid,
            string ticketnum,
            long store_id,
            long supply_id,
            int paytype,
            long catalogcount,
            string cataloglist)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();

                string[] catalogitems = cataloglist.Split('@');
                int cnt = catalogitems.Length-1;
                long[] catalog_id = new long[cnt];
                decimal[] standard = new decimal[cnt];
                byte[] mass = new byte[cnt];
                long[] unit_id = new long[cnt];
                string[] largenumber = new string[cnt];
                DateTime[] product_date = new DateTime[cnt];
                decimal[] price = new decimal[cnt];
                int[] count = new int[cnt];
                decimal total_price = 0;
                for (int i = 0; i < cnt; i++)
                {
                    string item = catalogitems[i];
                    //if (item == "")
                    //    break;
                    string[] item_detail = item.Split(',');

                    catalog_id[i] = Convert.ToInt64(item_detail[0]);
                    largenumber[i] = item_detail[1];
                    product_date[i] = Convert.ToDateTime(item_detail[2]);
                    standard[i] = Convert.ToDecimal(item_detail[3]);
                    mass[i] = Convert.ToByte(item_detail[4]);
                    unit_id[i] = Convert.ToInt64(item_detail[5]);
                    price[i] = Convert.ToDecimal(item_detail[6]);
                    count[i] = Convert.ToInt32(item_detail[7]);

                    total_price += (price[i] * count[i]);
                }

                int duplicate_largenumber = -1;
                /////////////////////////////////////////////////////////////////
                DateTime today = DateTime.Now;
                for (int i = 0; i < cnt; i++)
                {
                    tbl_catalog catalog = (from m in db.tbl_catalogs
                                           where m.id == catalog_id[i] && m.deleted == 0
                                           select m).FirstOrDefault();

                    if (catalog != null)
                    {
                        if ((today - product_date[i]).TotalDays >= catalog.avail_date * 30)
                        {
                            result.Result = NONGYAOERROR.ERR_OVER_AVAILDATE;
                            db.Connection.Close();
                            return result;
                        }
                    }                     
                }

                long[] standard_id = new long[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    tbl_standard standarditem = (from m in db.tbl_standards
                                         where m.deleted == 0 && m.quantity == standard[i] && m.mass == mass[i] && m.unit_id == unit_id[i]
                                         select m).FirstOrDefault();
                    if (standarditem != null)
                    {
                        standard_id[i] = standarditem.id;

                        var item = db.tbl_remains
                                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && //m.store_id == store_id &&
                                m.standard_id == standard_id[i] && m.largenumber == largenumber[i] &&
                                m.product_date.Date != product_date[i].Date)
                            .OrderByDescending(m => m.id)
                            .FirstOrDefault();
                        if (item != null)
                        {
                            duplicate_largenumber = i;
                            break;
                        }                     
                    }
                    else
                    {
                        tbl_standard newitem = new tbl_standard();

                        newitem.quantity = standard[i];
                        newitem.mass = mass[i];
                        newitem.unit_id = unit_id[i];

                        db.tbl_standards.InsertOnSubmit(newitem);
                        db.SubmitChanges();

                        tbl_standard laststandard = (from m in db.tbl_standards
                                                     where m.deleted == 0
                                                     orderby m.id descending
                                                     select m).FirstOrDefault();
                        standard_id[i] = laststandard.id;
                    }
                }

                if(duplicate_largenumber >= 0) {
                    result.Result = NONGYAOERROR.ERR_DUPLICATE_LARGENUMBER;
                    result.Data = new
                    {
                        index = duplicate_largenumber
                    };
                }
                else
                {
                    /////////////////////////////////////////////////////////////////
                    tbl_ticket ticketitem = (from m in db.tbl_tickets
                                             where m.deleted == 0 && m.ticketnum == ticketnum
                                             select m).FirstOrDefault();

                    if (ticketitem != null)
                    {
                        NongYaoResponseData newticket = GetTicketNumber(db, shop_id, 0);
                        result.Result = NONGYAOERROR.ERR_TICKETNUMUSED;
                        result.Data = new
                        {
                            ticket_num = newticket.Data,
                        };
                    }
                    else
                    {
                        /////////////////////////////////////////////////////////////////                        
                        tbl_ticket newticket = new tbl_ticket
                        {
                            ticketnum = ticketnum,
                            supply_id = supply_id,
                            userid = uid,
                            regtime = DateTime.Now,
                            paytype = paytype,
                            customer_type = 1,
                            customer_id = 0,
                            store_id = store_id,
                            shop_id = shop_id,
                            total_price = total_price,
                            real_price = total_price,
                            type = 0
                        };
                        db.tbl_tickets.InsertOnSubmit(newticket);
                        db.SubmitChanges();

                        tbl_ticket lastticket = (from m in db.tbl_tickets
                                                 where m.deleted == 0
                                                 orderby m.id descending
                                                 select m).FirstOrDefault();

                        for (int i = 0; i < cnt; i++)
                        {
                            tbl_buying newbuy = new tbl_buying
                            {
                                ticket_id = lastticket.id,
                                catalog_id = catalog_id[i],
                                standard_id = standard_id[i],
                                largenumber = largenumber[i],
                                product_date = product_date[i],
                                price = price[i],
                                quantity = count[i],
                                store_id = store_id,
                                shop_id = shop_id,
                            };
                            db.tbl_buyings.InsertOnSubmit(newbuy);

                            tbl_remain lastremainitem = db.tbl_remains
                                    .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id && m.shop_id == shop_id
                                        && m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                                    .OrderByDescending(m => m.id)
                                    .FirstOrDefault();
                            long quantity = 0;
                            if (lastremainitem != null)
                                quantity = lastremainitem.quantity;

                            tbl_remain lastremainitem_total = db.tbl_remains
                                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id)
                                .OrderByDescending(m => m.id)
                                .FirstOrDefault();
                            long total_quantity = 0;
                            if (lastremainitem_total != null)
                                total_quantity = lastremainitem_total.total_quantity;

                            tbl_remain newremain = new tbl_remain
                            {
                                catalog_id = catalog_id[i],
                                standard_id = standard_id[i],
                                largenumber = largenumber[i],
                                store_id = store_id,
                                buy_price = price[i],
                                catalog_cnt = count[i],
                                sum_price = price[i] * count[i],
                                type = 0,
                                supply_id = supply_id,
                                product_date = product_date[i],
                                origin_price = price[i],
                                quantity = quantity + count[i],
                                total_quantity = total_quantity + count[i],
                                shop_id = shop_id,
                                regtime = DateTime.Now,
                                user_id = uid
                            };
                            db.tbl_remains.InsertOnSubmit(newremain);
                        }

                        //////////////////////////////////////////////////////////////////////////
                        if (paytype == 0 || paytype == 2)
                        {
                            decimal money_profit = 0, bank_profit = 0;
                            tbl_earnlist lastearn = (from m in db.tbl_earnlists
                                                     where m.deleted == 0 && m.shop_id == shop_id
                                                     orderby m.id descending
                                                     select m).FirstOrDefault();

                            if (paytype == 0)
                            {
                                money_profit = (decimal)lastearn.money_profit - total_price;
                                bank_profit = (decimal)lastearn.bank_profit;
                            }
                            else if (paytype == 2)
                            {
                                money_profit = (decimal)lastearn.money_profit;
                                bank_profit = (decimal)lastearn.bank_profit - total_price;
                            }

                            tbl_earnlist newearnlist = new tbl_earnlist
                            {
                                ticket_id = lastticket.id,
                                price = total_price,
                                type = 0,
                                paytype = (byte)paytype,
                                reason = "采购进货",
                                userid = uid,
                                shop_id = shop_id,
                                regtime = DateTime.Now,
                                money_profit = money_profit,
                                bank_profit = bank_profit
                            };
                            db.tbl_earnlists.InsertOnSubmit(newearnlist);

                        }
                        else if (paytype == 1)
                        {
                            tbl_payment newpayment = new tbl_payment
                            {
                                customer_id = supply_id,
                                userid = uid,
                                type = 0,
                                price = total_price,
                                shop_id = shop_id,
                                ticket_type = 0,
                                ticket_id = lastticket.id,
                                regtime = DateTime.Now,
                                customer_type = 0,
                                deleted = 0,
                            };
                            db.tbl_payments.InsertOnSubmit(newpayment);
                        }

                        db.SubmitChanges();
                        db.Transaction.Commit();


                        result.Result = NONGYAOERROR.ERR_SUCCESS;
                        result.Data = new
                        {
                            ticket_id = lastticket.id
                        };
                    } //duplicate_ticketitem   
                } // duplicate_largenumber                
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }
        
        public NongYaoResponseData SaleCatalog(
            ServiceDBDataContext db,
            long shop_id,
            long uid,
            string ticketnum,
            string customer_name,
            string customer_phone,
            long store_id,
            int paytype,
            long catalogcount,
            string cataloglist,
            decimal sellmoney,
            decimal sellchange)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();

                string[] catalogitems = cataloglist.Split('@');
                int cnt = catalogitems.Length-1;
                long[] catalog_id = new long[cnt];
                long[] standard_id = new long[cnt];
                string[] largenumber = new string[cnt];
                decimal[] price = new decimal[cnt];
                int[] count = new int[cnt];
                decimal total_price = 0;
                for (int i = 0; i < cnt; i ++)
                {
                    string item = catalogitems[i];
                    string[] item_detail = item.Split(',');
                    catalog_id[i] = Convert.ToInt64(item_detail[0]);
                    standard_id[i] = Convert.ToInt64(item_detail[1]);
                    largenumber[i] = item_detail[2];
                    price[i] = Convert.ToDecimal(item_detail[3]);
                    count[i] = Convert.ToInt32(item_detail[4]);
                    total_price += (price[i] * count[i]);
                }

                tbl_customerlist customeritem = (from m in db.tbl_customerlists
                                             where m.deleted == 0 && m.name == customer_name && m.phone == customer_phone && m.shop_id == shop_id
                                              select m).FirstOrDefault();
                long customer_id = 0;
                if(customeritem != null)
                {
                    customer_id = customeritem.id;
                }
                if (customeritem == null)
                {    
                    tbl_customerlist newcustomer = new tbl_customerlist
                    {
                        name = customer_name,
                        phone = customer_phone,
                        regtime = DateTime.Now,
                        shop_id = shop_id,
                        deleted = 0,
                    };
                    db.tbl_customerlists.InsertOnSubmit(newcustomer);
                    db.SubmitChanges();

                    tbl_customerlist lastcustomer = (from m in db.tbl_customerlists
                                                     where m.deleted == 0
                                                     orderby m.id descending
                                                     select m).FirstOrDefault();
                    customer_id = lastcustomer.id;
                }

                ///////////////////////////////////////////////////////////////////////////
                int remain_insufficient = -1;
                for (int i = 0; i < cnt; i++)
                {
                    tbl_remain lastremainitem_check = db.tbl_remains
                            .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id
                                    && m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                            .OrderByDescending(m => m.id)
                            .FirstOrDefault();
                    if (lastremainitem_check == null || lastremainitem_check.quantity < count[i])
                    {
                        remain_insufficient = i;
                        break;
                    }

                }

                if (remain_insufficient >= 0)
                {
                    result.Result = NONGYAOERROR.ERR_REMAIN_INSUFFICIENT;
                    result.Data = new
                    {
                        index = remain_insufficient,
                    };
                }
                else
                {
                    /////////////////////////////////////////////////////////////////////////
                    tbl_ticket ticketitem = (from m in db.tbl_tickets
                                             where m.deleted == 0 && m.ticketnum == ticketnum
                                             select m).FirstOrDefault();
                    if (ticketitem != null)
                    {
                        NongYaoResponseData newticket = GetTicketNumber(db, shop_id, 2);
                        result.Result = NONGYAOERROR.ERR_TICKETNUMUSED;
                        result.Data = new
                        {
                            ticket_num = newticket.Data,
                        };
                    }
                    else
                    {
                        //////////////////////////////////////////////////////////////////////////////
                        tbl_ticket newticket = new tbl_ticket
                        {
                            ticketnum = ticketnum,
                            userid = uid,
                            regtime = DateTime.Now,
                            paytype = paytype,
                            customer_type = 0,
                            customer_id = customer_id,
                            type = 2,
                            store_id = store_id,
                            total_price = total_price,
                            real_price = sellmoney,
                            shop_id = shop_id,
                            deleted = 0
                        };
                        db.tbl_tickets.InsertOnSubmit(newticket);
                        db.SubmitChanges();

                        tbl_ticket lastticket = (from m in db.tbl_tickets
                                                 where m.deleted == 0
                                                 orderby m.id descending
                                                 select m).FirstOrDefault();

                        /////////////////////////////////////////////////////////////////////////////////
                        for (int i = 0; i < cnt; i++)
                        {
                            tbl_salelist newsale = new tbl_salelist
                            {
                                ticket_id = lastticket.id,
                                catalog_id = catalog_id[i],
                                standard_id = standard_id[i],
                                largenumber = largenumber[i],
                                catalog_price = price[i],
                                catalog_cnt = count[i],
                                shop_id = shop_id
                            };
                            db.tbl_salelists.InsertOnSubmit(newsale);

                            //////////////////////////////////////////////////////////////
                            DateTime oproduct_date = DateTime.MinValue;
                            decimal oprice = 0;
                            long osupply = 0;
                            tbl_remain originitem = db.tbl_remains
                                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.catalog_id == catalog_id[i] && m.store_id == store_id && m.shop_id == shop_id &&
                                    m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                                .OrderByDescending(m => m.id)
                                .FirstOrDefault();
                            if (originitem != null)
                            {
                                oproduct_date = (DateTime)originitem.product_date;
                                oprice = (long)originitem.origin_price;
                                osupply = (long)originitem.supply_id;
                            }

                            tbl_remain lastremainitem = db.tbl_remains
                                        .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id 
                                            && m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                                        .OrderByDescending(m => m.id)
                                        .FirstOrDefault();
                            long quantity = 0;
                            if (lastremainitem != null)
                                quantity = lastremainitem.quantity;

                            tbl_remain lastremainitem_total = db.tbl_remains
                                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id)
                                .OrderByDescending(m => m.id)
                                .FirstOrDefault();
                            long total_quantity = 0;
                            if (lastremainitem_total != null)
                                total_quantity = lastremainitem_total.total_quantity;

                            tbl_remain newremain = new tbl_remain
                            {
                                catalog_id = catalog_id[i],
                                standard_id = standard_id[i],
                                largenumber = largenumber[i],
                                store_id = store_id,
                                buy_price = price[i],
                                catalog_cnt = count[i],
                                sum_price = price[i] * count[i],
                                type = 2,
                                supply_id = osupply,
                                product_date = oproduct_date,
                                origin_price = oprice,
                                quantity = quantity - count[i],
                                total_quantity = total_quantity - count[i],
                                shop_id = shop_id,
                                regtime = DateTime.Now,
                                user_id = uid
                            };
                            db.tbl_remains.InsertOnSubmit(newremain);

                            ////////////////////////////////////////////////////////////////
                            long region_id = 0;
                            tbl_shop shopitem = (from m in db.tbl_shops
                                                 where m.deleted == 0 && m.id == shop_id
                                                 select m).FirstOrDefault();
                            if (shopitem != null)
                                region_id = (long)shopitem.region;

                            long nongyao_id = 0;
                            tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                                       where m.deleted == 0 && m.id == catalog_id[i]
                                                       select m).FirstOrDefault();
                            if (catalogitem != null)
                                nongyao_id = (long)catalogitem.kind;

                            long total_quantity_statistics = 0;
                            tbl_salestatistic lastsalestatistics = db.tbl_salestatistics
                                .Where(m => m.deleted == 0)
                                .OrderByDescending(m => m.id)
                                .FirstOrDefault();
                            if (lastsalestatistics != null)
                                total_quantity_statistics = lastsalestatistics.total_quantity;

                            tbl_salestatistic newsalestatistics = new tbl_salestatistic
                            {
                                region_id = region_id,
                                catalog_id = catalog_id[i],
                                nongyao_id = nongyao_id,
                                user_id = uid,
                                shop_id = shop_id,
                                regtime = DateTime.Now,
                                standard_id = standard_id[i],
                                largenumber = largenumber[i],
                                quantity = count[i],
                                total_quantity = total_quantity_statistics + count[i]
                            };

                            db.tbl_salestatistics.InsertOnSubmit(newsalestatistics);

                        }

                        if (paytype == 0 || paytype == 2)
                        {
                            /////////////////////////////////////////////////////////
                            if (sellmoney > 0)
                            {
                                decimal money_profit = 0, bank_profit = 0;
                                tbl_earnlist lastearn = (from m in db.tbl_earnlists
                                                         where m.deleted == 0 && m.shop_id == shop_id
                                                         orderby m.id descending
                                                         select m).FirstOrDefault();

                                if (paytype == 0)
                                {
                                    money_profit = (decimal)lastearn.money_profit + total_price;
                                    bank_profit = (decimal)lastearn.bank_profit;
                                }
                                else if (paytype == 2)
                                {
                                    money_profit = (decimal)lastearn.money_profit;
                                    bank_profit = (decimal)lastearn.bank_profit + total_price;
                                }

                                tbl_earnlist newearnlist = new tbl_earnlist
                                {
                                    ticket_id = lastticket.id,
                                    price = sellmoney,
                                    type = 1,
                                    paytype = (byte)paytype,
                                    reason = "销售开单",
                                    userid = uid,
                                    shop_id = shop_id,
                                    regtime = DateTime.Now,
                                    money_profit = money_profit,
                                    bank_profit = bank_profit
                                };
                                db.tbl_earnlists.InsertOnSubmit(newearnlist);
                            }

                            ///////////////////////////////////////////////////////////////////
                            if (sellchange > 0)
                            {
                                tbl_change newchange = new tbl_change
                                {
                                    shop_id = shop_id,
                                    userid = uid,
                                    regtime = DateTime.Now,
                                    change = sellchange
                                };

                                db.tbl_changes.InsertOnSubmit(newchange);

                                decimal payment = total_price - sellchange - sellmoney;
                                if (payment > 0)
                                {
                                    tbl_payment newpayment = new tbl_payment
                                    {
                                        customer_id = customer_id,
                                        userid = uid,
                                        type = 1,
                                        price = payment,
                                        shop_id = shop_id,
                                        ticket_type = 2,
                                        ticket_id = lastticket.id,
                                        regtime = DateTime.Now,
                                        customer_type = 0
                                    };
                                    db.tbl_payments.InsertOnSubmit(newpayment);
                                }
                            }
                        }
                        else if (paytype == 1)
                        {
                            tbl_payment newpayment = new tbl_payment
                            {
                                customer_id = customer_id,
                                userid = uid,
                                type = 1,
                                price = total_price,
                                shop_id = shop_id,
                                ticket_type = 2,
                                ticket_id = lastticket.id,
                                regtime = DateTime.Now,
                                customer_type = 0
                            };
                            db.tbl_payments.InsertOnSubmit(newpayment);
                        }

                        db.SubmitChanges();
                        db.Transaction.Commit();

                        result.Result = NONGYAOERROR.ERR_SUCCESS;
                        result.Data = new
                        {
                            ticket_id = lastticket.id
                        };
                    } // duplicate ticketnum
                } // remain_insufficient                
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData RejectCatalog(
         ServiceDBDataContext db,
         long shop_id,
         long uid,
         string ticketnum,
         long store_id,
         string customer_name,
         string customer_phone,
         int paytype,
         long catalogcount,
         string cataloglist)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();

                string[] catalogitems = cataloglist.Split('@');
                int cnt = catalogitems.Length-1;
                long[] catalog_id = new long[cnt];
                long[] standard_id = new long[cnt];
                string[] largenumber = new string[cnt];
                decimal[] price = new decimal[cnt];
                int[] count = new int[cnt];
                decimal total_price = 0;
                for (int i = 0; i < cnt; i++)
                {
                    string item = catalogitems[i];
                    string[] item_detail = item.Split(',');
                    catalog_id[i] = Convert.ToInt64(item_detail[0]);
                    standard_id[i] = Convert.ToInt64(item_detail[1]);
                    largenumber[i] = item_detail[2];
                    price[i] = Convert.ToDecimal(item_detail[3]);
                    count[i] = Convert.ToInt32(item_detail[4]);
                    total_price += (price[i] * count[i]);
                }


                tbl_customerlist customeritem = (from m in db.tbl_customerlists
                                                 where m.deleted == 0 && m.name == customer_name && m.phone == customer_phone && m.shop_id == shop_id
                                                 select m).FirstOrDefault();
                long customer_id = 0;
                if (customeritem == null)
                {
                    result.Result = NONGYAOERROR.ERR_NOCUSTOMER;
                }
                else
                {
                    customer_id = customeritem.id;

                    List<tbl_ticket> sale_list = (from m in db.tbl_tickets
                                                  where m.customer_type == 0 && m.type == 2 && m.customer_id == customer_id && m.deleted == 0
                                                  select m).ToList();

                    int rst = 0;
                    for (int i = 0; i < cnt; i++)
                    {                       

                        int sale_count = 0;

                        foreach (tbl_ticket m in sale_list)
                        {
                            List<tbl_salelist> buying_data = (from l in db.tbl_salelists
                                                              where l.ticket_id == m.id && l.deleted == 0
                                                              select l).ToList();
                            foreach (tbl_salelist p in buying_data)
                            {
                                if (p.catalog_id == catalog_id[i] && p.standard_id == standard_id[i] && p.largenumber == largenumber[i])
                                {
                                    rst = 1;
                                    sale_count += (int)p.catalog_cnt;
                                }
                            }
                            if (rst == 1)
                                break;
                        }

                        if (rst == 1)
                        {
                            if (sale_count < count[i])
                            {
                                rst = 2;
                                break;
                            }
                        }
                        else
                            break;
                    }

                    if (rst == 0)
                    {
                        result.Result = NONGYAOERROR.ERR_NO_SALECATALOG;
                    }
                    else if (rst == 2)
                    {
                        result.Result = NONGYAOERROR.ERR_SALECATALOGOVERFLOW;
                    }
                    else
                    {
                        /////////////////////////////////////////////////////////////////////////
                        tbl_ticket ticketitem = (from m in db.tbl_tickets
                                                 where m.deleted == 0 && m.ticketnum == ticketnum
                                                 select m).FirstOrDefault();
                        if (ticketitem != null)
                        {
                            NongYaoResponseData newticket = GetTicketNumber(db, shop_id, 3);
                            result.Result = NONGYAOERROR.ERR_TICKETNUMUSED;
                            result.Data = new
                            {
                                ticket_num = newticket.Data,
                            };
                        }
                        else
                        {
                            decimal sellmoney = total_price;

                            //////////////////////////////////////////////////////////////////////////////
                            tbl_ticket newticket = new tbl_ticket
                            {
                                ticketnum = ticketnum,
                                userid = uid,
                                regtime = DateTime.Now,
                                paytype = paytype,
                                customer_type = 0,
                                customer_id = customer_id,
                                type = 3,
                                store_id = store_id,
                                total_price = total_price,
                                real_price = sellmoney,
                                shop_id = shop_id,
                                deleted = 0
                            };
                            db.tbl_tickets.InsertOnSubmit(newticket);
                            db.SubmitChanges();

                            tbl_ticket lastticket = (from m in db.tbl_tickets
                                                     where m.deleted == 0
                                                     orderby m.id descending
                                                     select m).FirstOrDefault();

                            /////////////////////////////////////////////////////////////////////////////////
                            for (int i = 0; i < cnt; i++)
                            {
                                tbl_salelist newsale = new tbl_salelist
                                {
                                    ticket_id = lastticket.id,
                                    catalog_id = catalog_id[i],
                                    standard_id = standard_id[i],
                                    largenumber = largenumber[i],
                                    catalog_price = price[i],
                                    catalog_cnt = count[i],
                                    shop_id = shop_id
                                };
                                db.tbl_salelists.InsertOnSubmit(newsale);

                                //////////////////////////////////////////////////////////////
                                DateTime oproduct_date = DateTime.MinValue;
                                decimal oprice = 0;
                                long osupply = 0;
                                tbl_remain originitem = db.tbl_remains
                                    .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.catalog_id == catalog_id[i] && m.store_id == store_id &&
                                        m.standard_id == standard_id[i] && m.largenumber == largenumber[i]/* && m.type == 0*/)
                                    .OrderByDescending(m => m.id)
                                    .FirstOrDefault();
                                if (originitem != null)
                                {
                                    oproduct_date = (DateTime)originitem.product_date;
                                    oprice = (long)originitem.origin_price;
                                    osupply = (long)originitem.supply_id;
                                }

                                tbl_remain lastremainitem = db.tbl_remains
                                            .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id &&
                                                m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                                            .OrderByDescending(m => m.id)
                                            .FirstOrDefault();
                                long quantity = 0;
                                if (lastremainitem != null)
                                    quantity = lastremainitem.quantity;

                                tbl_remain lastremainitem_total = db.tbl_remains
                                    .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id)
                                    .OrderByDescending(m => m.id)
                                    .FirstOrDefault();
                                long total_quantity = 0;
                                if (lastremainitem_total != null)
                                    total_quantity = lastremainitem_total.total_quantity;

                                tbl_remain newremain = new tbl_remain
                                {
                                    catalog_id = catalog_id[i],
                                    standard_id = standard_id[i],
                                    largenumber = largenumber[i],
                                    store_id = store_id,
                                    buy_price = price[i],
                                    catalog_cnt = count[i],
                                    sum_price = price[i] * count[i],
                                    type = 3,
                                    supply_id = osupply,
                                    product_date = oproduct_date,
                                    origin_price = oprice,
                                    quantity = quantity + count[i],
                                    total_quantity = total_quantity + count[i],
                                    shop_id = shop_id,
                                    regtime = DateTime.Now,
                                    user_id = uid
                                };
                                db.tbl_remains.InsertOnSubmit(newremain);

                                ////////////////////////////////////////////////////////////////
                                long region_id = 0;
                                tbl_shop shopitem = (from m in db.tbl_shops
                                                     where m.deleted == 0 && m.id == shop_id
                                                     select m).FirstOrDefault();
                                if (shopitem != null)
                                    region_id = (long)shopitem.region;

                                long nongyao_id = 0;
                                tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                                           where m.deleted == 0 && m.id == catalog_id[i]
                                                           select m).FirstOrDefault();
                                if (catalogitem != null)
                                    nongyao_id = (long)catalogitem.kind;

                                long total_quantity_statistics = 0;
                                tbl_salestatistic lastsalestatistics = db.tbl_salestatistics
                                    .Where(m => m.deleted == 0)
                                    .OrderByDescending(m => m.id)
                                    .FirstOrDefault();
                                if (lastsalestatistics != null)
                                    total_quantity_statistics = lastsalestatistics.total_quantity;

                                tbl_salestatistic newsalestatistics = new tbl_salestatistic
                                {
                                    region_id = region_id,
                                    catalog_id = catalog_id[i],
                                    nongyao_id = nongyao_id,
                                    user_id = uid,
                                    shop_id = shop_id,
                                    regtime = DateTime.Now,
                                    standard_id = standard_id[i],
                                    largenumber = largenumber[i],
                                    quantity = -count[i],
                                    total_quantity = total_quantity_statistics - count[i]
                                };

                                db.tbl_salestatistics.InsertOnSubmit(newsalestatistics);

                            }

                            if (paytype == 0 || paytype == 2)
                            {
                                /////////////////////////////////////////////////////////
                                decimal money_profit = 0, bank_profit = 0;
                                tbl_earnlist lastearn = (from m in db.tbl_earnlists
                                                         where m.deleted == 0
                                                         orderby m.id descending
                                                         select m).FirstOrDefault();

                                if (paytype == 0)
                                {
                                    money_profit = (decimal)lastearn.money_profit - total_price;
                                    bank_profit = (decimal)lastearn.bank_profit;
                                }
                                else if (paytype == 2)
                                {
                                    money_profit = (decimal)lastearn.money_profit;
                                    bank_profit = (decimal)lastearn.bank_profit - total_price;
                                }

                                tbl_earnlist newearnlist = new tbl_earnlist
                                {
                                    ticket_id = lastticket.id,
                                    price = sellmoney,
                                    type = 0,
                                    paytype = (byte)paytype,
                                    reason = "销售退货",
                                    userid = uid,
                                    shop_id = shop_id,
                                    regtime = DateTime.Now,
                                    money_profit = money_profit,
                                    bank_profit = bank_profit
                                };
                                db.tbl_earnlists.InsertOnSubmit(newearnlist);
                            }
                            else if (paytype == 1)
                            {
                                tbl_payment newpayment = new tbl_payment
                                {
                                    customer_id = customer_id,
                                    userid = uid,
                                    type = 0,
                                    price = total_price,
                                    shop_id = shop_id,
                                    ticket_type = 3,
                                    ticket_id = lastticket.id,
                                    regtime = DateTime.Now,
                                    customer_type = 0
                                };
                                db.tbl_payments.InsertOnSubmit(newpayment);
                            }

                            db.SubmitChanges();
                            db.Transaction.Commit();

                            result.Result = NONGYAOERROR.ERR_SUCCESS;
                            result.Data = new
                            {
                                ticket_id = lastticket.id
                            };
                    }
                    

                   
                    } // duplicate ticketnum    
                } // nocustomer                           
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData MovingCatalog(
            ServiceDBDataContext db,
            long shop_id,
            long uid,                  
            long count,
            string cataloglist)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                string[] catalogitems = cataloglist.Split('@');
                int cnt = catalogitems.Length - 1;
                long[] startstore_id = new long[cnt];
                long[] endstore_id = new long[cnt];
                long[] catalog_id = new long[cnt];
                long[] standard_id = new long[cnt];
                string[] largenumber = new string[cnt];
                int[] movecount = new int[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    string item = catalogitems[i];
                    //if (item == "")
                    //    break;
                    string[] item_detail = item.Split(',');

                    startstore_id[i] = Convert.ToInt64(item_detail[0]);
                    endstore_id[i] = Convert.ToInt64(item_detail[1]);
                    catalog_id[i] = Convert.ToInt64(item_detail[2]);
                    standard_id[i] = Convert.ToInt64(item_detail[3]);
                    largenumber[i] = item_detail[4];
                    movecount[i] = Convert.ToInt32(item_detail[5]);
                }

                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();             

                ///////////////////////////////////////////////////////////////////////////
                int remain_insufficient = -1;
                for (int i = 0; i < cnt; i++)
                {
                    tbl_remain lastremainitem_check = db.tbl_remains
                            .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == startstore_id[i]
                                    && m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                            .OrderByDescending(m => m.id)
                            .FirstOrDefault();
                    if (lastremainitem_check == null || lastremainitem_check.quantity < movecount[i])
                    {
                        remain_insufficient = i;
                        break;
                    }

                }

                if (remain_insufficient >= 0)
                {
                    result.Result = NONGYAOERROR.ERR_REMAIN_INSUFFICIENT;
                    result.Data = new
                    {
                        index = remain_insufficient,
                    };
                }
                else
                {
                    /////////////////////////////////////////////////////////////////////////
                    for (int i = 0; i < cnt; i++)
                    {
                        tbl_storemoving newstoremovingitem = new tbl_storemoving
                        {
                            sending_id = startstore_id[i],
                            recving_id = endstore_id[i],
                            catalog_id = catalog_id[i],
                            standard_id = standard_id[i],
                            largenumber = largenumber[i],
                            catalog_cnt = movecount[i],
                            shop_id = shop_id,
                            regtime = DateTime.Now,
                            deleted = 0,
                        };
                        db.tbl_storemovings.InsertOnSubmit(newstoremovingitem);

                        //////////////////////////////////////////////////////////////////////////////
                        tbl_remain lastremainitem_total = db.tbl_remains
                            .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i])
                            .OrderByDescending(m => m.id)
                            .FirstOrDefault();
                        long total_quantity = 0;
                        if (lastremainitem_total != null)
                            total_quantity = lastremainitem_total.total_quantity;

                        tbl_remain lastremainitem_send = db.tbl_remains
                                        .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == startstore_id[i]
                                            && m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                                        .OrderByDescending(m => m.id)
                                        .FirstOrDefault();
                        long quantity_send = 0;
                        if (lastremainitem_send != null)
                            quantity_send = lastremainitem_send.quantity;

                        tbl_remain newremain_send = new tbl_remain
                        {
                            catalog_id = catalog_id[i],
                            standard_id = standard_id[i],
                            largenumber = largenumber[i],
                            store_id = startstore_id[i],
                            buy_price = 0,
                            catalog_cnt = -movecount[i],
                            sum_price = 0,
                            quantity = quantity_send - movecount[i],
                            total_quantity = total_quantity,
                            type = 4, //storemove
                            shop_id = shop_id,
                            regtime = DateTime.Now,
                            deleted = 0,
                            supply_id = lastremainitem_send.supply_id,
                            product_date = lastremainitem_send.product_date,
                            origin_price = lastremainitem_send.origin_price,
                            user_id = uid
                        };
                        db.tbl_remains.InsertOnSubmit(newremain_send);

                        ////////////////////////////////////////////////////////////////////////////
                        tbl_remain lastremainitem_recv = db.tbl_remains
                                        .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == endstore_id[i]
                                            && m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                                        .OrderByDescending(m => m.id)
                                        .FirstOrDefault();
                        long quantity_recv = 0;
                        if (lastremainitem_recv != null)
                            quantity_recv = lastremainitem_recv.quantity;
                        tbl_remain newremain_recv = new tbl_remain
                        {
                            catalog_id = catalog_id[i],
                            standard_id = standard_id[i],
                            largenumber = largenumber[i],
                            store_id = endstore_id[i],
                            buy_price = 0,
                            catalog_cnt = movecount[i],
                            sum_price = 0,
                            quantity = quantity_recv + movecount[i],
                            total_quantity = total_quantity,
                            type = 4, //storemove
                            shop_id = shop_id,
                            regtime = DateTime.Now,
                            deleted = 0,
                            supply_id = lastremainitem_send.supply_id,
                            product_date = lastremainitem_send.product_date,
                            origin_price = lastremainitem_send.origin_price,
                            user_id = uid
                        };
                        db.tbl_remains.InsertOnSubmit(newremain_recv);                       
                    }
                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                    db.SubmitChanges();
                    db.Transaction.Commit();
                }
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }
        
        public NongYaoResponseData AddCatalogUsingLog(
            ServiceDBDataContext db,
            long shop_id,
            long uid,
            long catalogcount,
            string cataloglist)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                string[] catalogitems = cataloglist.Split('@');
                int cnt = catalogitems.Length - 1;
                long[] store_id = new long[cnt];
                long[] catalog_id = new long[cnt];
                long[] standard_id = new long[cnt];
                string[] largenumber = new string[cnt];
                int[] count = new int[cnt];
                string[] reason = new string[cnt];
                for (int i = 0; i < cnt; i++)
                {
                    string item = catalogitems[i];
                    //if (item == "")
                    //    break;
                    string[] item_detail = item.Split(',');

                    store_id[i] = Convert.ToInt64(item_detail[0]);
                    catalog_id[i] = Convert.ToInt64(item_detail[1]);
                    standard_id[i] = Convert.ToInt64(item_detail[2]);
                    largenumber[i] = item_detail[3];
                    count[i] = Convert.ToInt32(item_detail[4]);
                    reason[i] = item_detail[5];
                }

                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();             

                ///////////////////////////////////////////////////////////////////////////
                int remain_insufficient = -1;
                for (int i = 0; i < cnt; i++)
                {
                    tbl_remain lastremainitem_check = db.tbl_remains
                            .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id[i]
                                    && m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                            .OrderByDescending(m => m.id)
                            .FirstOrDefault();
                    if (lastremainitem_check == null || lastremainitem_check.quantity < count[i])
                    {
                        remain_insufficient = i;
                        break;
                    }
                }

                if (remain_insufficient >= 0)
                {
                    result.Result = NONGYAOERROR.ERR_REMAIN_INSUFFICIENT;
                    result.Data = new
                    {
                        index = remain_insufficient,
                    };
                }
                else
                {
                    /////////////////////////////////////////////////////////////////////////
                    for (int i = 0; i < cnt; i++)
                    {
                        tbl_usinglist newusingitem = new tbl_usinglist
                        {
                            store_id = store_id[i],
                            catalog_id = catalog_id[i],
                            standard_id = standard_id[i],
                            largenumber = largenumber[i],
                            catalog_cnt = count[i],
                            reason = reason[i],
                            shop_id = shop_id,
                            userid = uid,
                            regtime = DateTime.Now,
                            deleted = 0,
                        };
                        db.tbl_usinglists.InsertOnSubmit(newusingitem);

                        ///////////////////////////////////////////////////////////////////////
                        tbl_remain lastremainitem_total = db.tbl_remains
                            .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id[i])
                            .OrderByDescending(m => m.id)
                            .FirstOrDefault();
                        long total_quantity = 0;
                        if (lastremainitem_total != null)
                            total_quantity = lastremainitem_total.total_quantity;

                        tbl_remain lastremainitem = db.tbl_remains
                                        .Where(m => m.deleted == 0 && m.catalog_id == catalog_id[i] && m.store_id == store_id[i]
                                            && m.standard_id == standard_id[i] && m.largenumber == largenumber[i])
                                        .OrderByDescending(m => m.id)
                                        .FirstOrDefault();
                        long quantity = 0;
                        if (lastremainitem != null)
                            quantity = lastremainitem.quantity;

                        tbl_remain newremain = new tbl_remain
                        {
                            catalog_id = catalog_id[i],
                            standard_id = standard_id[i],
                            largenumber = largenumber[i],
                            store_id = store_id[i],
                            buy_price = 0,
                            catalog_cnt = -count[i],
                            sum_price = 0,
                            quantity = quantity - count[i],
                            total_quantity = total_quantity,
                            type = 5, //storespend
                            shop_id = shop_id,
                            regtime = DateTime.Now,
                            deleted = 0,
                            supply_id = lastremainitem.supply_id,
                            product_date = lastremainitem.product_date,
                            origin_price = lastremainitem.origin_price,
                            user_id = uid
                        };
                        db.tbl_remains.InsertOnSubmit(newremain);
                    }

                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }                        
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData AddPaymentLog(
            ServiceDBDataContext db,
            long shop_id,
            long uid,
            long customer_id,           
            decimal price,
            byte type,
            byte paytype,
            decimal change,
            string etc)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {              
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();

                tbl_realpayment newitem = new tbl_realpayment
                {
                    shop_id = shop_id,
                    regtime = DateTime.Now,
                    userid = uid,
                    customer_id = customer_id, 
                    type = type,
                    paytype = paytype,
                    paymoney = price,
                    change = change,
                    etc = etc,
                    deleted = 0
                };
                db.tbl_realpayments.InsertOnSubmit(newitem);

                /////////////////////////////////////////////////////////
                decimal total_price = price - change;
                decimal money_profit = 0, bank_profit = 0;
                tbl_earnlist lastearn = (from m in db.tbl_earnlists
                                         where m.deleted == 0 && m.shop_id == shop_id
                                         orderby m.id descending
                                         select m).FirstOrDefault();

                if (paytype == 0)
                {
                    money_profit = (decimal)lastearn.money_profit - total_price;
                    bank_profit = (decimal)lastearn.bank_profit;
                }
                else if (paytype == 2)
                {
                    money_profit = (decimal)lastearn.money_profit;
                    bank_profit = (decimal)lastearn.bank_profit - total_price;
                }

                tbl_earnlist newearnlist = new tbl_earnlist
                {
                    ticket_id = 0,
                    price = total_price,
                    type = type,
                    paytype = paytype,
                    reason = etc,
                    userid = uid,
                    shop_id = shop_id,
                    regtime = DateTime.Now,
                    money_profit = money_profit,
                    bank_profit = bank_profit
                };
                db.tbl_earnlists.InsertOnSubmit(newearnlist);

                db.SubmitChanges();
                db.Transaction.Commit();

                result.Result = NONGYAOERROR.ERR_SUCCESS;                           
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData EditPaymentLog(
            ServiceDBDataContext db,
            long shop_id,
            long uid,
            long payment_id,
            long customer_id,            
            decimal price,
            byte type)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {    
                tbl_payment payment = (from m in db.tbl_payments
                                 where m.id == payment_id && m.deleted == 0
                                 select m).FirstOrDefault();
                if (payment != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    payment.shop_id = shop_id;
                    payment.userid = uid;
                    payment.customer_id = customer_id;
                    payment.price = price;
                    payment.type = type;                 

                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else
                    result.Result = NONGYAOERROR.ERR_FAILURE;        
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData DelPaymentLog(
            ServiceDBDataContext db,
            long shop_id,
            long uid,
            long payment_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_payment payment = (from m in db.tbl_payments
                                       where m.id == payment_id && m.shop_id == shop_id && m.userid == uid && m.deleted == 0
                                       select m).FirstOrDefault();
                if (payment != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    payment.deleted = 1;

                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else
                    result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }
       
        public NongYaoResponseData AddOtherpayLog(
            ServiceDBDataContext db,
            long shop_id,
            long uid,
            decimal price,
            byte type,
            byte paytype,
            string reason)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {              
                db.Connection.Open();
                db.Transaction = db.Connection.BeginTransaction();

                tbl_otherpay newotherpay = new tbl_otherpay
                {
                    userid = uid,
                    type = type,
                    paytype = paytype,
                    price = price,
                    reason = reason,
                    shop_id = shop_id,
                    regtime = DateTime.Now,
                    deleted = 0,
                };
                db.tbl_otherpays.InsertOnSubmit(newotherpay);
                db.SubmitChanges();

                //////////////////////////////////////////////////////////
                tbl_otherpay lastotherpay = (from m in db.tbl_otherpays
                            where m.deleted == 0
                            orderby m.id descending
                            select m).FirstOrDefault();
                long otherpay_id = 0;
                if (lastotherpay != null)
                    otherpay_id = lastotherpay.id;

                decimal money_profit = 0, bank_profit = 0;
                tbl_earnlist lastearn = (from m in db.tbl_earnlists
                                         where m.deleted == 0 && m.shop_id == shop_id
                                         orderby m.id descending
                                         select m).FirstOrDefault();

                if (paytype == 0)
                {
                    if(type == 0)
                        money_profit = (decimal)lastearn.money_profit - price;
                    else
                        money_profit = (decimal)lastearn.money_profit + price;
                    bank_profit = (decimal)lastearn.bank_profit;
                }
                else if (paytype == 2)
                {
                    money_profit = (decimal)lastearn.money_profit;
                    if(type == 0)
                        bank_profit = (decimal)lastearn.bank_profit - price;
                    else
                        bank_profit = (decimal)lastearn.bank_profit + price;
                }

                tbl_earnlist newearnlist = new tbl_earnlist
                {
                    otherpay_id = otherpay_id,
                    price = price,
                    type = type,
                    paytype = paytype,
                    reason = (type == 1 ? "其他收入" : "其他费用"),
                    userid = uid,
                    shop_id = shop_id,
                    regtime = DateTime.Now,
                    money_profit = money_profit,
                    bank_profit = bank_profit
                };
                db.tbl_earnlists.InsertOnSubmit(newearnlist);

                db.SubmitChanges();
                db.Transaction.Commit();

                result.Result = NONGYAOERROR.ERR_SUCCESS;                        
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData EditOtherpayLog(
            ServiceDBDataContext db,
            long shop_id,
            long uid,
            long otherpay_id,
            decimal price,
            byte type,
            byte paytype,
            string reason)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_otherpay otherpayitem = (from m in db.tbl_otherpays
                                        where m.id == otherpay_id && m.deleted == 0
                                       select m).FirstOrDefault();
                if (otherpayitem != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    otherpayitem.price = price;
                    otherpayitem.type = type;
                    otherpayitem.paytype = paytype;
                    otherpayitem.reason = reason;

                    ////////////////////////////////////////////////////
                    tbl_earnlist earnitem = (from m in db.tbl_earnlists
                                             where m.deleted == 0 && m.otherpay_id == otherpay_id && m.shop_id == shop_id
                                             select m).FirstOrDefault();
                    if (earnitem != null)
                    {
                        earnitem.type = type;
                        earnitem.paytype = paytype;
                        earnitem.reason = reason;

                        decimal prev_money_profit = (decimal)earnitem.money_profit, prev_bank_profit = (decimal)earnitem.bank_profit;
                        decimal money_profit = 0, bank_profit = 0;
                        tbl_earnlist lastearnitemthan = (from m in db.tbl_earnlists
                                                         where m.deleted == 0 && m.shop_id == shop_id && m.id < earnitem.id
                                                        orderby m.id descending
                                                        select m).FirstOrDefault();
                        if (lastearnitemthan != null)
                        {
                            money_profit = (decimal)lastearnitemthan.money_profit;
                            bank_profit = (decimal)lastearnitemthan.bank_profit;
                        }
                        if (paytype == 0)
                        {
                            if (type == 0)
                                earnitem.money_profit = money_profit - price;
                            else
                                earnitem.money_profit = money_profit + price;
                            earnitem.bank_profit = bank_profit;
                        }
                        else if (paytype == 2)
                        {
                            if (type == 0)
                                earnitem.bank_profit = bank_profit - price;
                            else
                                earnitem.bank_profit = bank_profit + price;
                            earnitem.money_profit = money_profit;
                        }
                        earnitem.price = price;

                        db.SubmitChanges();

                        if (earnitem.money_profit != prev_money_profit || earnitem.bank_profit != prev_bank_profit)
                        {
                            decimal money_diff = (decimal)earnitem.money_profit - prev_money_profit;
                            decimal bank_diff = (decimal)earnitem.bank_profit - prev_bank_profit;

                            var itemlist = (from m in db.tbl_earnlists
                                            where m.deleted == 0 && m.shop_id == shop_id && m.id > earnitem.id
                                            select m).ToList();
                            foreach (tbl_earnlist item in itemlist)
                            {
                                item.money_profit = item.money_profit + money_diff;
                                item.bank_profit = item.bank_profit + bank_diff;

                                db.SubmitChanges();
                            }
                        }
                    }
                    else
                        result.Result = NONGYAOERROR.ERR_FAILURE;


                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else
                    result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData DelOtherpayLog(
           ServiceDBDataContext db,
           long shop_id,
           long uid,
           long otherpay_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_otherpay payment = (from m in db.tbl_otherpays
                                        where m.id == otherpay_id && m.shop_id == shop_id && m.userid == uid && m.deleted == 0
                                        select m).FirstOrDefault();
                if (payment != null)
                {
                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    payment.deleted = 1;

                    db.SubmitChanges();
                    db.Transaction.Commit();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else
                    result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }
         
        public NongYaoResponseData RequestAddCatalog(
            ServiceDBDataContext db,
            long uid,
            long shop_id,
            string register_id,
            string permit_id,
            string sample_id,
            string catalog_usingname,
            string catalog_nickname,
            string product,
            string shape,
            string material,
            decimal content,
            string product_area,
            byte level,
            string description,
            string image)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                  tbl_catalog lastcatalog = (from m in db.tbl_catalogs    
                                                   orderby m.id descending
                                                     select m).FirstOrDefault();

                  long nNewCatalogRegisterId = 1;
                if(lastcatalog != null)
                    nNewCatalogRegisterId = lastcatalog.id;
                nNewCatalogRegisterId++;

                tbl_shop shop = (from m in db.tbl_shops
                                 where m.id == shop_id && m.deleted == 0
                                 select m).FirstOrDefault();

                long region_id = 0;

                if (shop != null)
                {     
                   
                        tbl_region item = (from m in db.tbl_regions
                                           where m.deleted == 0 && m.id == shop.region
                                           select m).FirstOrDefault();
                        if (item != null)
                        {
                            if (item.parentid == 0)
                                region_id = item.id;
                            else
                                region_id = (long)item.parentid;
                        }
                   

                    tbl_catalog catalog = (from m in db.tbl_catalogs
                                           where m.deleted == 0 && m.register_id == register_id && m.region_id == region_id
                                           select m).FirstOrDefault();

                    if (catalog != null)
                    {
                        result.Result = NONGYAOERROR.ERR_DUPLICATEREGISTERID;
                        db.Connection.Close();
                        return result;
                    }
                    tbl_catalog catalog_item = (from m in db.tbl_catalogs
                                                where m.deleted == 0 && m.name == catalog_usingname && m.region_id == region_id
                                                select m).FirstOrDefault();
                    if (catalog_item != null)
                    {
                        result.Result = NONGYAOERROR.ERR_DUPLICATEUSERNAME;
                        db.Connection.Close();
                        return result;
                    }
                }
                
                

                String szPath = NongYaoCommon.saveImage(image, nNewCatalogRegisterId);
                    if (szPath == "")
                    {
                        result.Result = NONGYAOERROR.ERR_NO_IMAGE;
                    }

                    db.Connection.Open();
                    db.Transaction = db.Connection.BeginTransaction();

                    tbl_catalog newcatalog = new tbl_catalog
                    {
                        id = nNewCatalogRegisterId,
                        shop_id = shop_id,
                        register_id = register_id,
                        permit_id = permit_id,
                        sample_id = sample_id,
                        name = catalog_usingname,
                        usingname = catalog_usingname,
                        nickname = catalog_nickname,
                        product = product,
                        shape = shape,
                        material = material,
                        content = content,
                        product_area = product_area,
                        level = level,
                        description = description,
                        userid = uid,
                        image = szPath,
                        regtime = DateTime.Now,
                        region_id = region_id,
                        pass = 0,
                        deleted = 0
                    };
                    db.tbl_catalogs.InsertOnSubmit(newcatalog);

                    db.SubmitChanges();
                    db.Transaction.Commit();
                   
                result.Result = NONGYAOERROR.ERR_SUCCESS; 
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                db.Transaction.Rollback();
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            finally
            {
                db.Connection.Close();
            }
            return result;
        }

        public NongYaoResponseData GetRemainCatalogStandardList(
            ServiceDBDataContext db,
            long shop_id,
            long store_id,
            long catalog_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                            where m.deleted == 0 && m.id == catalog_id
                                            select m).FirstOrDefault();

                var list = (from m in db.tbl_remains
                            where m.deleted == 0 && (store_id == 0 || (store_id != 0 && m.store_id == store_id)) && m.shop_id == shop_id && m.catalog_id == catalog_id
                            select new
                            {
                                standard_id = m.standard_id
                            }).Distinct().ToList();

                List<tbl_standard> standdardlist = (from m in list
                        from l in db.tbl_standards
                        where m.standard_id == l.id && l.deleted == 0
                        select l).ToList();

                List<StandardInfo> standarddata = new List<StandardInfo>();

                foreach (tbl_standard standarditem in standdardlist)
                {
                    tbl_unit unititem = (from m in db.tbl_units
                                         where m.deleted == 0 && m.id == standarditem.unit_id
                                         select m).FirstOrDefault();
                    string unitname = "";
                    if (unititem != null)
                        unitname = unititem.name;

                    standarddata.Add(new StandardInfo { 
                        standard_id = standarditem.id,
                        standard = standarditem.quantity + (standarditem.mass == 0 ? "克" : "毫升") + "/" + unitname
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    catalog_id = catalog_id,
                    catalog_name = DBCommon.SNN(catalogitem.name),
                    standard_count = standarddata.Count(),
                    standard_data = standarddata
                };
                       
             }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetLargenumberList(
            ServiceDBDataContext db,
            long shop_id,
            long store_id,
            long catalog_id,
            long standard_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<string> largenumberlist = (from m in db.tbl_remains
                 where m.deleted == 0 && (store_id == 0 || (store_id != 0 && m.store_id == store_id)) && m.shop_id == shop_id &&
                     m.catalog_id == catalog_id && m.standard_id == standard_id
                 select m.largenumber).Distinct().ToList();                

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = largenumberlist.Count(),
                    data = largenumberlist
                };

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData SaleHistory(
            ServiceDBDataContext db,
            long shop_id,
            byte type,
            string start,
            string end,
            string search,
            int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start).Date;
                DateTime enddate = Convert.ToDateTime(end).Date;
                if (!string.IsNullOrEmpty(search))
                    search = search.ToLower();

                List<tbl_ticket> alllist = db.tbl_tickets
                        .Where(m => m.deleted == 0 && m.shop_id == shop_id &&
                            m.regtime.Date >= startdate && m.regtime.Date <= enddate && (type == 4 || ( m.type != 4 && m.type == type)))
                        .OrderByDescending(m => m.id)
                        .ToList();

                List<SaleHistoryInfo> retdata = new List<SaleHistoryInfo>();
                foreach (tbl_ticket item in alllist)
                {
                    string customer_name = "";
                    if (item.customer_type == 0)
                    {
                        tbl_customerlist customeritem = (from m in db.tbl_customerlists
                                    where m.deleted == 0 && m.id == item.customer_id
                                    select m).FirstOrDefault();
                        if (customeritem != null)
                            customer_name = customeritem.name;
                    }
                    else
                    {
                        tbl_shop shopitem =(from m in db.tbl_shops
                                                where m.deleted == 0 && m.id == item.customer_id
                                                select m).FirstOrDefault();
                        if (shopitem != null)
                            customer_name = shopitem.name;
                    }

                    if ((item.ticketnum != null && item.ticketnum.ToLower().Contains(search)) || customer_name.ToLower().Contains(search))
                    {
                        retdata.Add(new SaleHistoryInfo
                        {
                            ticket_id = item.id,
                            ticket_num = item.ticketnum,
                            customer_name = customer_name,
                            saledate = String.Format("{0:yyyy-MM-dd}", item.regtime),
                            totalmoney = (decimal)item.total_price,
                            realmoney = (decimal)item.real_price,
                            type = (item.type == 2)?"销售开单":"退货"
                        });
                    }
                }

                //retdata.Skip((pagenum - 1) * page_per_count).Take(page_per_count);

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata
                };

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData SaleDetail(
            ServiceDBDataContext db,
            long shop_id,
            long ticket_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                string customer_name = "";
                tbl_ticket ticketitem = (from m in db.tbl_tickets
                                             where m.deleted == 0 && m.id == ticket_id
                                             select m).FirstOrDefault();
                string ticketnum = "";
                if(ticketitem != null) {
                    ticketnum = ticketitem.ticketnum;

                    if (ticketitem.customer_type == 0)
                    {
                        tbl_customerlist customeritem = (from m in db.tbl_customerlists
                                                         where m.deleted == 0 && m.id == ticketitem.customer_id
                                                         select m).FirstOrDefault();
                        if (customeritem != null)
                            customer_name = customeritem.name;
                    }
                    else
                    {
                        tbl_shop shopitem = (from m in db.tbl_shops
                                             where m.deleted == 0 && m.id == ticketitem.customer_id
                                             select m).FirstOrDefault();
                        if (shopitem != null)
                            customer_name = shopitem.name;
                    }
                }                

                List<tbl_salelist> alllist = db.tbl_salelists
                    .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.ticket_id == ticket_id)
                    .OrderByDescending(m => m.id)
                    .ToList();

                List<SaleDetailInfo> retdata = new List<SaleDetailInfo>();
                foreach (tbl_salelist item in alllist)
                {
                    string catalog_name = "";
                    tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                        where m.deleted == 0 && m.id == item.catalog_id
                                        select m).FirstOrDefault();
                    if (catalogitem != null)
                            catalog_name = catalogitem.name;

                    string standard = "";
                    tbl_standard standarditem = (from m in db.tbl_standards
                                     where m.deleted == 0 && m.id == item.standard_id
                                     select m).FirstOrDefault();
                    if (standarditem != null) {
                        tbl_unit unititem = (from m in db.tbl_units
                                                 where m.deleted == 0 && m.id == standarditem.unit_id
                                                 select m).FirstOrDefault();
                        if(unititem != null)
                            standard = standarditem.quantity + (standarditem.mass == 0 ? "克" : "毫升") + "/" + unititem.name;
                    }

                    retdata.Add(new SaleDetailInfo {
                        catalog_name = catalog_name,
                        standard = standard,
                        price = (decimal)item.catalog_price,
                        count = (int)item.catalog_cnt,
                        total = (decimal)item.catalog_price * (int)item.catalog_cnt
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    ticketnum = ticketnum,
                    customer_name = customer_name,
                    count = retdata.Count(),
                    data = retdata
                };

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetCatalogInfoFromBarcodeAndStore(
            ServiceDBDataContext db,
            string barcode,
            long store_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                long catalog_id = 0;
                string catalog_name = "";
                string catalog_num = "";

                tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                           where m.deleted == 0 && m.register_id == barcode
                                           select m).FirstOrDefault();
                if (catalogitem != null)
                {
                    catalog_id = catalogitem.id;
                    catalog_name = catalogitem.name;
                    catalog_num = catalogitem.catalog_num;
                }

                var alllist = (from m in db.tbl_remains
                               where m.deleted == 0 && m.store_id == store_id && m.catalog_id == catalog_id
                               group m by new { standard_id = m.standard_id, largenumber = m.largenumber } into g
                               select new
                               {
                                   standard_id = g.Key.standard_id,
                                   largenumber = g.Key.largenumber,
                                   quantity = g.OrderByDescending(m => m.id).Select(m => m.quantity).FirstOrDefault()
                               }).ToList();

                var standardlist = alllist.Select(m => new
                {
                    standard_id = m.standard_id
                }).Distinct().ToList();
                List<StandardQuantityInfo> retdata = new List<StandardQuantityInfo>();
                foreach (var item in standardlist)
                {
                    string standard = "";
                    tbl_standard standarditem = (from m in db.tbl_standards
                                                 where m.deleted == 0 && m.id == item.standard_id
                                                 select m).FirstOrDefault();
                    if (standarditem != null)
                    {
                        tbl_unit unititem = (from m in db.tbl_units
                                             where m.deleted == 0 && m.id == standarditem.unit_id
                                             select m).FirstOrDefault();
                        if (unititem != null)
                            standard = standarditem.quantity + (standarditem.mass == 0 ? "克" : "毫升") + "/" + unititem.name;
                    }

                    retdata.Add(new StandardQuantityInfo
                    {
                        standard_id = item.standard_id,
                        standard_string = standard,
                        data = alllist.Where(m => m.standard_id == item.standard_id)
                                .Select(m => new LargenumberQuantityInfo { 
                                    largenumber = m.largenumber,
                                    quantity = m.quantity
                                }).ToList()
                    });
                }

                if (retdata.Count() == 0)
                    result.Result = NONGYAOERROR.ERR_NOCATALOG;
                else
                {
                    result.Result = NONGYAOERROR.ERR_SUCCESS;

                    result.Data = new
                    {
                        catalog_id = catalog_id,
                        catalog_name = catalog_name,
                        catalog_num = catalog_num,
                        count = retdata.Count(),
                        data = retdata
                    };
                }

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetQuantityFromCatalogAndStandard(
            ServiceDBDataContext db,
            string largenumber,
            long store_id,
            long catalog_id,
            long standard_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
              
                long quantity = (from m in db.tbl_remains
                               where m.deleted == 0 && m.store_id == store_id && m.catalog_id == catalog_id && m.standard_id == standard_id && m.largenumber == largenumber
                               orderby m.id descending
                               select m.quantity).FirstOrDefault();                               
             
                result.Result = NONGYAOERROR.ERR_SUCCESS;

                result.Data = new
                {
                    quantity = quantity
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetCatalogInfoFromIdAndStore(
           ServiceDBDataContext db,
           long catalog_id,
           long store_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                
                string catalog_name = "";
                tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                           where m.deleted == 0 && m.id == catalog_id
                                           select m).FirstOrDefault();
                if (catalogitem != null)
                {
                    catalog_id = catalogitem.id;
                    catalog_name = catalogitem.name;
                }

                var alllist = (from m in db.tbl_remains
                               where m.deleted == 0 && m.store_id == store_id && m.catalog_id == catalog_id
                               group m by new { standard_id = m.standard_id, largenumber = m.largenumber } into g
                               select new
                               {
                                   standard_id = g.Key.standard_id,
                                   largenumber = g.Key.largenumber,
                                   quantity = g.OrderByDescending(m => m.id).Select(m => m.quantity).FirstOrDefault()
                               }).ToList();

                var standardlist = alllist.Select(m => new
                {
                    standard_id = m.standard_id
                }).Distinct().ToList();
                List<StandardQuantityInfo> retdata = new List<StandardQuantityInfo>();
                foreach (var item in standardlist)
                {
                    string standard = "";
                    tbl_standard standarditem = (from m in db.tbl_standards
                                                 where m.deleted == 0 && m.id == item.standard_id
                                                 select m).FirstOrDefault();
                    if (standarditem != null)
                    {
                        tbl_unit unititem = (from m in db.tbl_units
                                             where m.deleted == 0 && m.id == standarditem.unit_id
                                             select m).FirstOrDefault();
                        if (unititem != null)
                            standard = standarditem.quantity + (standarditem.mass == 0 ? "克" : "毫升") + "/" + unititem.name;
                    }

                    retdata.Add(new StandardQuantityInfo
                    {
                        standard_id = item.standard_id,
                        standard_string = standard,
                        data = alllist.Where(m => m.standard_id == item.standard_id)
                                .Select(m => new LargenumberQuantityInfo
                                {
                                    largenumber = m.largenumber,
                                    quantity = m.quantity
                                }).ToList()
                    });
                }

                if (retdata.Count() == 0)
                    result.Result = NONGYAOERROR.ERR_NOCATALOG;
                else
                {
                    result.Result = NONGYAOERROR.ERR_SUCCESS;

                    result.Data = new
                    {
                        catalog_id = catalog_id,
                        catalog_name = catalog_name,
                        count = retdata.Count(),
                        data = retdata
                    };
                }

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetInOutLogDetail(ServiceDBDataContext db, long shop_id, string date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime detaildate = Convert.ToDateTime(date);
                List<FinanceDetailInfo> alllist = new List<FinanceDetailInfo>();

                var sales = (from m in db.tbl_remains
                             where m.deleted == 0 && m.shop_id == shop_id && (m.type == 2 || m.type == 3) && m.regtime.Date == detaildate.Date
                             group m by m.user_id into g
                             select new FinanceDetailInfo
                             {
                                 desc = "销售收入",
                                 price = g.ToList().Sum(m => (m.type == 2) ? m.sum_price : -m.sum_price),
                                 user_id = g.Key,
                                 reason = "",
                             }).ToList();
                alllist = alllist.Union(sales).ToList();

                var saleorigins = (from m in db.tbl_remains
                                   where m.deleted == 0 && m.shop_id == shop_id && (m.type == 2 || m.type == 3) && m.regtime.Date == detaildate.Date
                                   group m by m.user_id into g
                                   select new FinanceDetailInfo
                                   {
                                       desc = "销售成本",
                                       price = g.ToList().Sum(m => (m.type == 2) ? m.catalog_cnt * m.origin_price : -m.catalog_cnt * m.origin_price),
                                       user_id = g.Key,
                                       reason = "",
                                   }).ToList();
                alllist = alllist.Union(saleorigins).ToList();

                var otherpays = (from m in db.tbl_otherpays
                                 where m.deleted == 0 && m.regtime.Date == detaildate.Date && m.shop_id == shop_id
                                 select new FinanceDetailInfo
                                 {
                                     desc = m.type == 1 ? "其他收入" : "其他费用",
                                     price = m.price,
                                     user_id = m.userid,
                                     reason = m.reason,
                                 }).ToList();
                alllist = alllist.Union(otherpays).ToList();

                var smallchanges = (from m in db.tbl_changes
                                    where m.deleted == 0 && m.regtime.Date == detaildate.Date && m.shop_id == shop_id
                                    select new FinanceDetailInfo
                                    {
                                        desc = "找零金额",
                                        price = (decimal)m.change,
                                        user_id = m.userid,
                                        reason = "",
                                    }).ToList();
                alllist = alllist.Union(smallchanges).ToList();

               // alllist = alllist.Skip((pagenum-1)*page_per_count).Take(page_per_count).ToList();

                int i = 1;
                List<InOutLogDetailInfo> retdata = new List<InOutLogDetailInfo>();
                foreach (var item in alllist)
                {
                    string username = "";
                    tbl_user useritem = (from m in db.tbl_users
                                         where m.deleted == 0 && m.id == item.user_id
                                         select m).FirstOrDefault();
                    if (useritem != null)
                        username = useritem.name;

                    retdata.Add(new InOutLogDetailInfo { 
                        no = i++,
                        description = DBCommon.SNN(item.desc),
                        money = item.price,
                        username = username,
                        reason = DBCommon.SNN(item.reason)
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count,
                    data = retdata,
                };

            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }

        public NongYaoResponseData GetPaymentDetailLog(
           ServiceDBDataContext db,
           long shop_id,
           long payment_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_payment payInfo = (from m in db.tbl_payments
                                       where m.deleted == 0 && m.id == payment_id
                                       select m).FirstOrDefault();

                List<PaymentDetailLogInfo> retdata = new List<PaymentDetailLogInfo>();
                if (payInfo != null)
                {
                    string content = "";
                    if (payInfo.ticket_type == 0)
                        content = "采购进货";
                    else if (payInfo.ticket_type == 1)
                        content = "采购退货";
                    else if (payInfo.ticket_type == 2)
                        content = "销售开单";
                    else if (payInfo.ticket_type == 3)
                        content = "销售退货";
                    
                    string ticketnum = "";
                    tbl_ticket ticketitem = db.tbl_tickets
                        .Where(m => m.deleted == 0 && m.id == payInfo.ticket_id)
                        .FirstOrDefault();
                    if (ticketitem != null)
                        ticketnum = ticketitem.ticketnum;

                    retdata.Add(new PaymentDetailLogInfo { 
                        payment_id = payInfo.id,
                        date = String.Format("{0:yyyy-MM-dd}", payInfo.regtime),
                        ticket_num = ticketnum,
                        content = content,
                        type = payInfo.type,
                        price = payInfo.price,
                        reason = DBCommon.SNN(payInfo.reason)
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData RealPaymentList(
           ServiceDBDataContext db,
           long shop_id,
           string customer_name,
           byte type,
            string start_date,
            string end_date,
            int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;
                if (!string.IsNullOrEmpty(customer_name))
                    customer_name = customer_name.ToLower();

                List<tbl_realpayment> creditInfo_tmp = (from m in db.tbl_realpayments
                                                        where m.deleted == 0 && ((DateTime)m.regtime).Date <= enddate.Date && ((DateTime)m.regtime).Date >= startdate.Date && m.shop_id == shop_id && 
                                                            (type == 2 || type != 2 && m.type == type)
                                                        select m).OrderByDescending(m => m.regtime).ToList();
                List<tbl_realpayment> creditInfo = creditInfo_tmp;
                if (customer_name != "")
                {
                    creditInfo = creditInfo_tmp.Join(db.tbl_customerlists, m => m.customer_id, l => l.id, (m, l) => new { creditInfo = m, customInfo = l })
                        .Where(m => m.customInfo.name.Contains(customer_name)).Select(l => l.creditInfo).ToList();
                }

                List<RealPaymentListInfo> retdata = new List<RealPaymentListInfo>();
                foreach (tbl_realpayment eachCredit in creditInfo)
                {
                    string name = "";
                    //if (eachCredit.type == 0) // if type is buy name is supply name~
                    //{
                    //    long supply_id = eachCredit.customer_id;
                    //    tbl_supply supplyInfo = (from m in db.tbl_supplies
                    //                             where m.deleted == 0 && m.id == supply_id
                    //                             select m).FirstOrDefault();
                    //    if (supplyInfo != null)
                    //    {
                    //        name = supplyInfo.name;
                    //    }
                    //}
                    //else
                    {
                        long customer_id = eachCredit.customer_id;
                        tbl_customerlist customerInfo = (from m in db.tbl_customerlists
                                                         where m.deleted == 0 && m.id == customer_id
                                                         select m).FirstOrDefault();
                        if (customerInfo != null)
                        {
                            name = customerInfo.name;
                        }
                    }

                    decimal credit_value = 0;
                    credit_value = (decimal)eachCredit.paymoney;
                    decimal smallchange_value = 0;
                    smallchange_value = (decimal)eachCredit.change;

                    string str_value = "";
                    if (eachCredit.type == 1) //out
                        str_value = "实收: " + Convert.ToString(credit_value) + "元";
                    else
                        str_value = "实付: " + Convert.ToString(credit_value) + "元";

                    if (name.ToLower().Contains(customer_name))
                    {
                        retdata.Add(new RealPaymentListInfo
                        {
                            date = String.Format("{0:yyyy-MM-dd}", eachCredit.regtime),
                            customer_name = name,
                            type = eachCredit.type,
                            price = eachCredit.paymoney,
                            change = eachCredit.change,
                            etc = DBCommon.SNN(eachCredit.etc)
                        });
                    }
                }

                retdata = retdata.Skip((pagenum - 1) * page_per_count).Take(page_per_count).ToList();

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData MoneybankList(
           ServiceDBDataContext db,
           long shop_id,
            string start_date,
            string end_date,
            int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                var retdata = (from m in db.tbl_earnlists
                           where m.deleted == 0 && m.regtime.Date >= startdate && m.regtime.Date <= enddate && m.shop_id == shop_id
                           group m by m.regtime.Date into g
                           select new MoneybankListInfo
                           {
                               date = String.Format("{0:yyyy-MM-dd}", g.Key),
                               money = (decimal)g.OrderByDescending(l => l.id).FirstOrDefault().money_profit,
                               bank = (decimal)g.OrderByDescending(l => l.id).FirstOrDefault().bank_profit,
                               sum = (decimal)g.OrderByDescending(l => l.id).Select(l => l.money_profit + l.bank_profit).FirstOrDefault() 
                           }).ToList();

                retdata = retdata.Skip((pagenum - 1) * page_per_count).Take(page_per_count).ToList();
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData MoneybankInfo(
           ServiceDBDataContext db,
           long shop_id,
            byte paytype,
            string date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime detaildate = Convert.ToDateTime(date);

                var retdata = (from m in db.tbl_earnlists
                               where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date == detaildate.Date && 
                            (paytype == 4 || (paytype != 4 && m.paytype == paytype) && m.shop_id == shop_id)
                       select new MoneybankDetailInfo
                       {
                           ticket_id = m.ticket_id != null ? (long)m.ticket_id : 0,
                           paytype = m.paytype,
                           type = m.type,
                           price = m.price,
                           reason = DBCommon.SNN(m.reason)
                       }).ToList();
                foreach (MoneybankDetailInfo m in retdata)
                {
                   string ticket_num = (from l in db.tbl_tickets
                                    where l.id == m.ticket_id
                                    select l.ticketnum).FirstOrDefault();
                   if (ticket_num == null) ticket_num = "";
                   m.ticket_num = ticket_num;
                }
                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData BuyingDetailInfo(
           ServiceDBDataContext db,
           long shop_id,
            string start_date,
            string end_date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                long buying_count = (from m in db.tbl_remains
                                     where m.deleted == 0 && m.shop_id == shop_id && m.type == 0 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                     select m).ToList().Sum(m => m.catalog_cnt);
                decimal buying_price = (from m in db.tbl_remains
                                        where m.deleted == 0 && m.shop_id == shop_id && m.type == 0 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                        select m).ToList().Sum(m => m.sum_price);
                long buying_back_count = (from m in db.tbl_remains
                                          where m.deleted == 0 && m.shop_id == shop_id && m.type == 1 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                          select m).ToList().Sum(m => m.catalog_cnt);
                decimal buying_back_price = (from m in db.tbl_remains
                                             where m.deleted == 0 && m.shop_id == shop_id && m.type == 1 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                             select m).ToList().Sum(m => m.sum_price);

                var retdata = new { 
                    buying_count = buying_count,   // 采购数量
                    buying_price = buying_price,    // 采购金额
                    buying_back_count = buying_back_count, // 退货数量
                    buying_back_price = buying_back_price, // 退货金额
                    buying_totalcount = buying_count - buying_back_count, // 总数量
                    buying_totalprice = buying_price - buying_back_price // 总金额
                };

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData SaleDetailInfo(
           ServiceDBDataContext db,
           long shop_id,
            string start_date,
            string end_date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                long sell_count = (from m in db.tbl_remains
                                   where m.deleted == 0 && m.shop_id == shop_id && m.type == 2 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                   select m).ToList().Sum(m => m.catalog_cnt);
                decimal sell_price = (from m in db.tbl_remains
                                      where m.deleted == 0 && m.shop_id == shop_id && m.type == 2 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                      select m).ToList().Sum(m => m.sum_price);
                long sell_back_count = (from m in db.tbl_remains
                                        where m.deleted == 0 && m.shop_id == shop_id && m.type == 3 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                        select m).ToList().Sum(m => m.catalog_cnt);
                decimal sell_back_price = (from m in db.tbl_remains
                                           where m.deleted == 0 && m.shop_id == shop_id && m.type == 3 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                           select m).ToList().Sum(m => m.sum_price);

                var retdata = new
                {
                    sell_count = sell_count,   
                    sell_price = sell_price,   
                    sell_back_count = sell_back_count,
                    sell_back_price = sell_back_price, 
                    sell_totalcount = sell_count - sell_back_count, 
                    sell_totalprice = sell_price - sell_back_price 
                };

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData StoreMovingDetailInfo(
           ServiceDBDataContext db,
           long shop_id,
            string start_date,
            string end_date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                long main = 0;
                tbl_store storeitem = (from m in db.tbl_stores
                                              where m.deleted == 0 && m.shop_id == shop_id
                                              orderby m.id
                                              select m).FirstOrDefault();
                if (storeitem != null)
                    main = storeitem.id;

                long moving_out_count = (from m in db.tbl_storemovings
                                         where m.deleted == 0 && m.shop_id == shop_id && m.sending_id == main && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                         select m).ToList().Sum(m => m.catalog_cnt);
                long moving_in_count = (from m in db.tbl_storemovings
                                        where m.deleted == 0 && m.shop_id == shop_id && m.recving_id == main && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                        select m).ToList().Sum(m => m.catalog_cnt);

                var retdata = new
                {
                    moving_out_count = moving_out_count,
                    moving_in_count = moving_in_count,
                };

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData SpendingDetailInfo(
           ServiceDBDataContext db,
           long shop_id,
            string start_date,
            string end_date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                long spending_loss_count = (from m in db.tbl_usinglists
                                            where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                            select m).ToList().Sum(m => m.catalog_cnt);
                var alllist = (from m in db.tbl_usinglists
                            from l in db.tbl_remains
                            where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate &&
                                l.deleted == 0 && m.store_id == l.store_id && m.catalog_id == l.catalog_id && m.standard_id == l.standard_id && m.largenumber == l.largenumber
                            select new
                            {
                                id = m.id,
                                buying_id = l.id,
                                catalog_cnt = m.catalog_cnt,
                                price = l.origin_price
                            }).ToList();
                decimal spending_loss_price = (from m in alllist
                                               group m by m.id into g
                                               select new
                                               {
                                                   id = g.Key,
                                                   catalog_cnt = g.OrderByDescending(l => l.buying_id).FirstOrDefault().catalog_cnt,
                                                   price = g.OrderByDescending(l => l.buying_id).FirstOrDefault().price
                                               }).Sum(m => m.catalog_cnt * m.price);
                long spending_more_count = (from m in db.tbl_tickets
                                            from l in db.tbl_buyings
                                            where m.deleted == 0 && m.type == 0 && m.paytype == 3 && m.shop_id == shop_id && m.id == l.ticket_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                            select new
                                            {
                                                id = m.id,
                                                price = l.price,
                                                quantity = l.quantity
                                            }).ToList().Sum(m => m.quantity);
                decimal spending_more_price = (from m in db.tbl_tickets
                                               from l in db.tbl_buyings
                                               where m.deleted == 0 && m.type == 0 && m.paytype == 3 && m.shop_id == shop_id && m.id == l.ticket_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                               select new
                                               {
                                                   id = m.id,
                                                   price = l.price,
                                                   quantity = l.quantity
                                               }).ToList().Sum(m => m.price * m.quantity);

                var retdata = new
                {
                    spending_loss_count = spending_loss_count,
                    spending_loss_price = spending_loss_price,
                    spending_more_count = spending_more_count,
                    spending_more_price = spending_more_price,
                };

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData RemainDetailInfo(
           ServiceDBDataContext db,
           long shop_id,
            string start_date,
            string end_date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                long remain_count = (from m in db.tbl_remains
                                         where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                         group m by new { store_id = m.store_id, catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber } into g
                                         select new
                                         {
                                             total_quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity,
                                             origin_price = g.OrderByDescending(l => l.id).FirstOrDefault().origin_price
                                         }).ToList().Sum(m => m.total_quantity);
                decimal remain_price = (from m in db.tbl_remains
                                        where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                        group m by new { store_id = m.store_id, catalog_id = m.catalog_id, standard_id = m.standard_id, largenumber = m.largenumber } into g
                                        select new
                                        {
                                            total_quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity,
                                            origin_price = g.OrderByDescending(l => l.id).FirstOrDefault().origin_price
                                        }).ToList().Sum(m => m.total_quantity * m.origin_price);

                var retdata = new
                {
                    remain_count = remain_count,
                    remain_price = remain_price,
                };

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData PayingDetailInfo(
           ServiceDBDataContext db,
           long shop_id,
            string start_date,
            string end_date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                decimal paying_in = (from m in db.tbl_otherpays
                                     where m.deleted == 0 && m.type == 1 && m.shop_id == shop_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                     select m).ToList().Sum(m => m.price);
                decimal paying_out = (from m in db.tbl_otherpays
                                      where m.deleted == 0 && m.type == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                      select m).ToList().Sum(m => m.price);

                var retdata = new
                {
                    paying_in = paying_in,
                    paying_out = paying_out,
                };

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData MoneyDetailInfo(
           ServiceDBDataContext db,
           long shop_id,
            string start_date,
            string end_date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                decimal money_sell = (from m in db.tbl_remains
                                      where m.deleted == 0 && m.shop_id == shop_id && m.type == 2 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                      select m).ToList().Sum(m => m.sum_price);
                decimal money_buying = (from m in db.tbl_remains
                                        where m.deleted == 0 && m.shop_id == shop_id && m.type == 2 && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                        select m).ToList().Sum(m => m.catalog_cnt * m.origin_price);
                decimal money_smallchange = (from m in db.tbl_changes
                                        where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                                        select m).ToList().Sum(m => (decimal)m.change);

                var retdata = new
                {
                    money_sell = money_sell,
                    money_buying = money_buying,
                    money_profit = money_sell - money_buying - money_smallchange
                };

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetShopInfoFromNickname(
           ServiceDBDataContext db,
            string nickname,
            long region_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                if(!string.IsNullOrEmpty(nickname))
                    nickname = nickname.ToLower();
                var retdata = db.tbl_shops
                .Where(m => m.deleted == 0 && m.pass == 1 && (m.permit_id.Contains(nickname) || m.nickname.Contains(nickname)))
                .Join(db.tbl_regions, m => m.region, l => l.id, (m, l) => new { shopInfo = m, regionInfo = l })
                .Where(p => (p.regionInfo.parentid == region_id) || (p.regionInfo.parentid != 0 && p.regionInfo.id == region_id))
                .Select(l => new 
                {
                    shop_id = l.shopInfo.id,
                    shop_name = DBCommon.SNN(l.shopInfo.name),
                }).ToList();

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetLastRegionList(
           ServiceDBDataContext db,
            int role,
            long region_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                var retdata = (from m in db.tbl_regions
                               where m.deleted == 0 && ( (role == 0 && m.parentid == 0) || (role == 1 && m.parentid == region_id) || (role == 2 && m.id == region_id) )
                               select new { 
                                region_id = m.id,
                                region_name = m.name
                               }).ToList();

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetShopStatistic(
           ServiceDBDataContext db,
           long region_id,
            byte level,
            string search_key,
            long shop_id,
            string start_date,
            string end_date,
            int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;
                if (!string.IsNullOrEmpty(search_key))
                    search_key = search_key.ToLower();

                List<long> regionList = new List<long>();

                regionList = db.tbl_regions
                    .Where(m => m.deleted == 0 && ((region_id == 0 && m.parentid > 0) || (region_id != 0 && (m.parentid == region_id || m.id == region_id))))
                    .Select(m => m.id)
                    .ToList();

                var alllist = db.tbl_shops
                .Where(m => m.deleted == 0 && m.pass == 1 && regionList.Contains((long)m.region) &&
                    ((shop_id == 0) || (shop_id != 0 && m.id == shop_id)) && 
                    ((level == 0) || (level != 0 && m.level == level)))
                    .ToList();

                List<ShopStatisticsInfo> retdata = new List<ShopStatisticsInfo>();
                foreach (var item in alllist)
                {
                    if (shop_id == 0)
                    {
                        if (!(item.name != null && item.name.ToLower().Contains(search_key)) && !(item.nickname != null && item.nickname.ToLower().Contains(search_key)))
                            continue;
                    }

                    string region_name = "";
                    tbl_region regionitem = (from m in db.tbl_regions
                                                 where m.deleted == 0 && m.id == item.region
                                                 select m).FirstOrDefault();
                    if(regionitem != null)
                        region_name = regionitem.name;

                    /*long remain_count = 0;
                    var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.shop_id == item.id && m.regtime >= startdate.Date && m.regtime <= enddate.Date)
                .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                .Select(g => new
                {
                    quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                });
                    if (remainList.Count() != 0)
                    {
                        remain_count = remainList.Sum(m => m.quantity);
                    }

                    long sale_count = 0;
                    long saleVal = 0;
                    long salebackVal = 0;

                    var saleList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.shop_id == item.id && m.type == 2 && m.regtime >= startdate && m.regtime <= enddate)
                        .Select(l => l.catalog_cnt);
                    if (saleList.Count() != 0)
                    {
                        saleVal = saleList.Sum();
                    }

                    var salebackList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.shop_id == item.id && m.type == 3 && m.regtime >= startdate && m.regtime <= enddate)
                        .Select(l => l.catalog_cnt);
                    if (salebackList.Count() != 0)
                    {
                        salebackVal = salebackList.Sum();
                    }
                    sale_count = saleVal - salebackVal;*/

                    decimal retVal = 0;
                    var remainList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.shop_id == item.id && m.regtime >= startdate.Date && m.regtime <= enddate.Date)
                        .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                        .Select(g => new
                        {
                            standard_id = g.Key.standard_id,
                            quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                        }).ToList();

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
                    decimal remain_count = retVal;

                    retVal = 0;
                    decimal saleVal = 0;
                    decimal salebackVal = 0;

                    var saleList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.shop_id == item.id && m.type == 2 && m.regtime >= startdate && m.regtime <= enddate)
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
                        .Where(m => m.deleted == 0 && m.shop_id == item.id && m.type == 3 && m.regtime >= startdate && m.regtime <= enddate)
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
                    decimal sale_count = retVal;

                    retdata.Add(new ShopStatisticsInfo
                    {
                        region_name = region_name,
                        shop_id = item.id,
                        shop_name = DBCommon.SNN(item.name),
                        shop_lawman = DBCommon.SNN(item.law_man),
                        sale_count = sale_count,
                        remain_count = remain_count
                    });
                }

                retdata = retdata.Skip((pagenum - 1) * page_per_count).Take(page_per_count).ToList();

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetNongyaoKindList(ServiceDBDataContext db)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                var retdata = (from m in db.tbl_nongyaos
                               where m.deleted == 0 && m.parentid == 0
                               select new { 
                                nongyao_id = m.id,
                                nongyao_name = DBCommon.SNN(m.name)
                               }).ToList();

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetCatalogInfoFromNickname(ServiceDBDataContext db,
            string nickname,
            long nongyao_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                if (!string.IsNullOrEmpty(nickname))
                    nickname = nickname.ToLower();

                var retdata = db.tbl_catalogs
                            .Where(m => m.deleted == 0 && m.pass == 1 &&
                                ((m.nickname != null && m.nickname.ToLower().Contains(nickname)) || (m.permit_id != null && m.permit_id.ToLower().Contains(nickname))) &&
                                ((nongyao_id == 0 && m.kind > 0) || (nongyao_id != 0 && m.kind == nongyao_id)))
                            .Select(l => new 
                            {
                                catalog_id = l.id,
                                catalog_name = DBCommon.SNN(l.name),
                            }).ToList();

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetCatalogStatistics(ServiceDBDataContext db,
            long region_id,
            long nongyao_id,
            long catalog_id,
            string search_key,
            string start_date,
            string end_date,
            int pagenum)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                var alllist =  (catalog_id != 0) ?
                    db.tbl_remains
                    .Where(m => m.deleted == 0)
                    .Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Where(m => m.catInfo.pass == 1 && m.catInfo.id == catalog_id)
                    .Join(db.tbl_shops, m => m.remainInfo.shop_id, l => l.id, (m, l) => new { catInfo = m, shopInfo = l })
                    .Join(db.tbl_regions, m => m.shopInfo.region, l => l.id, (m, l) => new { catInfo = m, regionInfo = l })
                    .Where(p => ((region_id == 0 && p.regionInfo.parentid > 0) || (region_id > 0 && ((p.regionInfo.parentid == region_id) || (p.regionInfo.parentid != 0 && p.regionInfo.id == region_id)))))
                    .GroupBy(m => new { m.catInfo.catInfo.catInfo.id, m.catInfo.catInfo.remainInfo.standard_id })
                    .Select(g => new 
                    {
                        catalog_id = g.FirstOrDefault().catInfo.catInfo.catInfo.id,
                        name = g.FirstOrDefault().catInfo.catInfo.catInfo.name,
                        nickname = g.FirstOrDefault().catInfo.catInfo.catInfo.nickname,
                        nongyaoname = g.FirstOrDefault().catInfo.catInfo.catInfo.name,
                        product = g.FirstOrDefault().catInfo.catInfo.catInfo.product,
                        standard_id = (int)g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id
                    }).ToList() :
                   db.tbl_remains
                    .Where(m => m.deleted == 0)
                    .Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                    .Where(m => (m.catInfo.pass == 1 && ((nongyao_id == 0 && m.catInfo.kind > 0) || (nongyao_id != 0 && m.catInfo.kind == nongyao_id))))
                    .Join(db.tbl_shops, m => m.remainInfo.shop_id, l => l.id, (m, l) => new { catInfo = m, shopInfo = l })
                    .Join(db.tbl_regions, m => m.shopInfo.region, l => l.id, (m, l) => new { catInfo = m, regionInfo = l })
                    .Where(p => ((region_id == 0 && p.regionInfo.parentid > 0) || (region_id > 0 && ((p.regionInfo.parentid == region_id) || (p.regionInfo.parentid != 0 && p.regionInfo.id == region_id)))))
                    .GroupBy(m => new { m.catInfo.catInfo.catInfo.id, m.catInfo.catInfo.remainInfo.standard_id })
                    .Select(g => new 
                    {
                        catalog_id = g.FirstOrDefault().catInfo.catInfo.catInfo.id,
                        name = g.FirstOrDefault().catInfo.catInfo.catInfo.name,
                        nickname = g.FirstOrDefault().catInfo.catInfo.catInfo.nickname,
                        nongyaoname = g.FirstOrDefault().catInfo.catInfo.catInfo.name,
                        product = g.FirstOrDefault().catInfo.catInfo.catInfo.product,                        
                        standard_id = (int)g.FirstOrDefault().catInfo.catInfo.remainInfo.standard_id                        
                    }).ToList();

                List<CatalogStatisticsInfo> retdata = new List<CatalogStatisticsInfo>();
                foreach (var item in alllist)
                {
                    if (catalog_id == 0)
                    {
                        if (!string.IsNullOrEmpty(search_key))
                            search_key = search_key.ToLower();
                        if (!(item.name != null && item.name.ToLower().Contains(search_key)) && !(item.nickname != null && item.nickname.ToLower().Contains(search_key)))
                            continue;
                    }

                    string standard = "";
                    tbl_standard standarditem = (from m in db.tbl_standards
                                                 where m.deleted == 0 && m.id == item.standard_id
                                                 select m).FirstOrDefault();
                    if (standarditem != null)
                    {
                        tbl_unit unititem = (from m in db.tbl_units
                                             where m.deleted == 0 && m.id == standarditem.unit_id
                                             select m).FirstOrDefault();
                        if (unititem != null)
                            standard = standarditem.quantity + (standarditem.mass == 0 ? "克" : "毫升") + "/" + unititem.name;
                    }

                    /*long remain_count = 0;
                    var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == item.catalog_id && m.standard_id == item.standard_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate)
                .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                .Select(g => new
                {
                    quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                });
                    if (remainList.Count() != 0)
                    {
                        remain_count = remainList.Sum(m => m.quantity);
                    }

                    long sale_count = 0;
                    long saleVal = 0;
                    long salebackVal = 0;

                    var saleList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.catalog_id == item.catalog_id && m.standard_id == item.standard_id && m.type == 2 && m.regtime >= startdate && m.regtime <= enddate)
                        .Select(l => l.catalog_cnt);
                    if (saleList.Count() != 0)
                    {
                        saleVal = saleList.Sum();
                    }

                    var salebackList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.catalog_id == item.catalog_id && m.standard_id == item.standard_id && m.type == 3 && m.regtime >= startdate && m.regtime <= enddate)
                        .Select(l => l.catalog_cnt);
                    if (salebackList.Count() != 0)
                    {
                        salebackVal = salebackList.Sum();
                    }
                    sale_count = saleVal - salebackVal;*/

                    decimal retVal = 0;
                    var remainList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.catalog_id == item.catalog_id && m.standard_id == item.standard_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate)
                        .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                        .Select(g => new
                        {
                            standard_id = g.Key.standard_id,
                            quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                        });

                    foreach (var m in remainList)
                    {
                        var standarditem1 = db.tbl_standards
                            .Where(l => l.deleted == 0 && l.id == m.standard_id)
                            .FirstOrDefault();
                        decimal standard1 = 0;
                        if (standarditem1 != null)
                            standard1 = standarditem1.quantity;
                        retVal += m.quantity * standard1 / 1000;
                    }
                    decimal remain_count = retVal;

                    retVal = 0;
                    decimal saleVal = 0;
                    decimal salebackVal = 0;

                    var saleList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.catalog_id == item.catalog_id && m.standard_id == item.standard_id && m.type == 2 && m.regtime >= startdate && m.regtime <= enddate)
                        .Select(m => new
                        {
                            standard_id = m.standard_id,
                            quantity = m.catalog_cnt
                        });
                    foreach (var m in saleList)
                    {
                        var standarditem1 = db.tbl_standards
                            .Where(l => l.deleted == 0 && l.id == m.standard_id)
                            .FirstOrDefault();
                        decimal standard1 = 0;
                        if (standarditem1 != null)
                            standard1 = standarditem1.quantity;
                        saleVal += m.quantity * standard1 / 1000;
                    }

                    var salebackList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.catalog_id == item.catalog_id && m.standard_id == item.standard_id && m.type == 3 && m.regtime >= startdate && m.regtime <= enddate)
                        .Select(m => new
                        {
                            standard_id = m.standard_id,
                            quantity = m.catalog_cnt
                        });
                    foreach (var m in salebackList)
                    {
                        var standarditem1 = db.tbl_standards
                            .Where(l => l.deleted == 0 && l.id == m.standard_id)
                            .FirstOrDefault();
                        decimal standard1 = 0;
                        if (standarditem1 != null)
                            standard1 = standarditem1.quantity;
                        salebackVal += m.quantity * standard1 / 1000;
                    }
                    retVal = saleVal - salebackVal;
                    decimal sale_count = retVal;

                    retdata.Add(new CatalogStatisticsInfo { 
                        catalog_id = item.catalog_id,
                        catalog_name = DBCommon.SNN(item.name),
                        product = item.product,
                        standard = standard,
                        standard_id = item.standard_id,
                        remain_count = remain_count,
                        sale_count = sale_count
                    });
                }

                retdata = retdata.Skip((pagenum - 1) * page_per_count).Take(page_per_count).ToList();

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetCatalogDetailStatistics(ServiceDBDataContext db,
            long catalog_id,
            long standard_id,
            long region_id,
            string start_date,
            string end_date)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                DateTime startdate = Convert.ToDateTime(start_date).Date;
                DateTime enddate = Convert.ToDateTime(end_date).Date;

                List<long> regionList = new List<long>();

                regionList = db.tbl_regions
                    .Where(m => m.deleted == 0 && ((region_id == 0 && m.parentid > 0) || (region_id != 0 && (m.parentid == region_id || m.id == region_id))))
                    .Select(m => m.id)
                    .ToList();

                var alllist = db.tbl_remains
                .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.standard_id == standard_id)
                .Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                .GroupBy(m => new { m.remainInfo.catalog_id, m.remainInfo.store_id, m.remainInfo.standard_id, m.remainInfo.largenumber })
                .Select(g => new 
                {
                    uid = g.FirstOrDefault().catInfo.id,
                    largenumber = g.FirstOrDefault().remainInfo.largenumber,//g.large,
                    product_date = Convert.ToString(g.FirstOrDefault().catInfo.regtime),
                    avail_date = (int)g.FirstOrDefault().catInfo.avail_date,
                    shop_id = (long)g.FirstOrDefault().catInfo.shop_id,
                    store_id = g.FirstOrDefault().remainInfo.store_id
                }).OrderBy(l => l.uid).ToList();

                List<CatalogStatisticsDetailInfo> retdata = new List<CatalogStatisticsDetailInfo>();
                foreach (var item in alllist)
                {
                    string shop_name = "";
                    tbl_shop shopitem = (from m in db.tbl_shops
                                where m.deleted == 0 && m.id == item.shop_id
                                         select m).FirstOrDefault();

                    if ( !regionList.Contains((long)shopitem.region) )
                        continue;

                    if (shopitem != null)
                        shop_name = shopitem.name;

                    long remain_count = 0;
                    tbl_remain remainInfo = (from m in db.tbl_remains
                                             where m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == item.store_id && m.standard_id == standard_id && m.largenumber == item.largenumber && m.regtime >= startdate && m.regtime <= enddate
                                             orderby m.id descending
                                             select m).FirstOrDefault();
                    if (remainInfo != null)
                    {
                        remain_count = remainInfo.quantity;
                    }

                    long sale_count = 0;
                    long saleVal = 0;
                    long salebackVal = 0;

                    var saleList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == item.store_id && m.standard_id == standard_id && m.largenumber == item.largenumber && m.type == 2 && m.regtime >= startdate && m.regtime <= enddate)
                        .Select(l => l.catalog_cnt);
                    if (saleList.Count() != 0)
                    {
                        saleVal = saleList.Sum();
                    }

                    var salebackList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.catalog_id == catalog_id && m.store_id == item.store_id && m.standard_id == standard_id && m.largenumber == item.largenumber && m.type == 3 && m.regtime >= startdate && m.regtime <= enddate)
                        .Select(l => l.catalog_cnt);
                    if (salebackList.Count() != 0)
                    {
                        salebackVal = salebackList.Sum();
                    }
                    sale_count = saleVal - salebackVal;

                    retdata.Add(new CatalogStatisticsDetailInfo { 
                        largenumber = item.largenumber,
                        product_date = String.Format("{0:yyyy-MM-dd}", item.product_date),
                        avail_date = item.avail_date,
                        remain_count = remain_count,
                        sale_count = sale_count,
                        total_count = remain_count + sale_count,
                        shop_name = shop_name
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetStatistics(ServiceDBDataContext db,
            long year)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                long yearVal = year;

                /*long remain_count = 0, sale_count = 0;
                var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.regtime.Year <= yearVal)
                .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                .Select(g => new
                {
                    quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                });
                if (remainList.Count() != 0)
                    remain_count = remainList.Sum(m => m.quantity);

                long saleCount = 0;
                List<tbl_remain> remainInfo = (from m in db.tbl_remains
                                               where m.deleted == 0 && m.regtime.Year == yearVal && m.type == 2
                                               select m).ToList();
                if (remainInfo.Count() != 0)
                {
                    saleCount = remainInfo.Sum(k => k.catalog_cnt);
                }
                long salebackCount = 0;
                List<tbl_remain> remainInfo1 = (from m in db.tbl_remains
                                                where m.deleted == 0 && m.regtime.Year == yearVal && m.type == 3
                                                select m).ToList();
                if (remainInfo1.Count() != 0)
                {
                    salebackCount = remainInfo1.Sum(k => k.catalog_cnt);
                }
                sale_count = saleCount - salebackCount;*/

                var remainList = db.tbl_remains
                .Where(m => m.deleted == 0 && m.regtime.Year <= yearVal)
                .GroupBy(m => new { m.catalog_id, m.store_id, m.standard_id, m.largenumber })
                .Select(g => new
                {
                    standard_id = g.Key.standard_id,
                    quantity = g.OrderByDescending(l => l.id).FirstOrDefault().quantity
                });
                //if (remainList.Count() != 0)
                //    retValue.remainCount = remainList.Sum(m => m.quantity);
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
                decimal remain_count = remainQuantity;

                //long saleCount = 0;
                List<tbl_remain> remainInfo = (from m in db.tbl_remains
                                               where m.deleted == 0 && m.regtime.Year == yearVal && m.type == 2
                                               select m).ToList();
                //if (remainInfo.Count() != 0)
                //{
                //    saleCount = remainInfo.Sum(k => k.catalog_cnt);
                //}

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

                //long salebackCount = 0;
                List<tbl_remain> remainInfo1 = (from m in db.tbl_remains
                                                where m.deleted == 0 && m.regtime.Year == yearVal && m.type == 3
                                                select m).ToList();
                //if (remainInfo1.Count() != 0)
                //{
                //    salebackCount = remainInfo1.Sum(k => k.catalog_cnt);
                //}

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
                decimal sale_count = saleQuantity - salebackQuantity;

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    remain_count = remain_count,
                    sale_count = sale_count,
                    total_count = (remain_count + sale_count)
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetBarGraph(ServiceDBDataContext db,
            long year)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<tbl_region> alllist = (from m in db.tbl_regions
                                            where m.deleted == 0 && m.parentid != 0
                                            select m).ToList();

                List<RegionBarGraphInfo> retdata = new List<RegionBarGraphInfo>();
                foreach (var item in alllist)
                {
                    long region_id = item.id;

                    long sale_count = 0;
                    decimal retVal = 0;
                    decimal saleVal = 0;
                    decimal salebackVal = 0;

                    var saleList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Year == year)
                        .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { remainInfo = m, shop_info = l })
                        .Where(k => k.shop_info.region == region_id)
                        .Select(m => new
                        {
                            standard_id = m.remainInfo.standard_id,
                            quantity = m.remainInfo.catalog_cnt
                        });
                    /*if (saleList.Count() != 0)
                    {
                        saleVal = saleList.Sum();
                    }*/
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
                        .Select(m => new
                        {
                            standard_id = m.remainInfo.standard_id,
                            quantity = m.remainInfo.catalog_cnt
                        });
                    /*if (salebackList.Count() != 0)
                    {
                        salebackVal = salebackList.Sum();
                    }*/
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
                    sale_count = (long)Math.Round(retVal);

                    long remain_count = 0;
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
                    retVal = 0;
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
                    remain_count = (long)Math.Round(retVal);
                    /*if (remainList.Count() != 0)
                    {
                        remain_count = remainList.Sum(m => m.quantity);
                    }*/

                    retdata.Add(new RegionBarGraphInfo { 
                        region_name = DBCommon.SNN(item.name),
                        sale_count = sale_count,
                        remain_count = remain_count
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetLineGraph(ServiceDBDataContext db,
            long year)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<MonthLineGraphInfo> retdata = new List<MonthLineGraphInfo>();
                for (int Month = 1; Month <= 12; Month++)
                {
                    long sale_count = 0;
                    decimal retVal = 0;
                    decimal saleVal = 0;
                    decimal salebackVal = 0;

                    var saleList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Year == year && m.regtime.Month == Month)
                        .Select(m => new { 
                            standard_id = m.standard_id,
                            quantity = m.catalog_cnt
                        }).ToList();
                    /*if (saleList.Count() != 0)
                    {
                        saleVal = saleList.Sum();
                    }*/
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
                    /*if (salebackList.Count() != 0)
                    {
                        salebackVal = salebackList.Sum();
                    }*/
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
                    sale_count = (long)Math.Round(retVal);

                    retdata.Add(new MonthLineGraphInfo { 
                        month = Month,
                        sale_count = sale_count
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetPieGraph(ServiceDBDataContext db,
            long year)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                List<tbl_nongyao> alllist = (from m in db.tbl_nongyaos
                                              where m.deleted == 0 && m.parentid == 0
                                              select m).ToList();

                List<NongyaoPieGraphInfo> retdata = new List<NongyaoPieGraphInfo>();
                foreach (var item in alllist)
                {
                    long type_id = item.id;

                    long sale_count = 0;
                    long saleVal = 0;
                    long salebackVal = 0;

                    var saleList = db.tbl_remains
                        .Where(m => m.deleted == 0 && m.type == 2 && m.regtime.Year == year)
                        .Join(db.tbl_catalogs, m => m.catalog_id, l => l.id, (m, l) => new { remainInfo = m, catInfo = l })
                        .Join(db.tbl_nongyaos, m => m.catInfo.kind, l => l.id, (m, l) => new { remainInfo = m, nongyaoInfo = l })
                        .Where(k => k.nongyaoInfo.parentid == type_id || k.nongyaoInfo.id == type_id)
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
                    sale_count = saleVal - salebackVal;

                    retdata.Add(new NongyaoPieGraphInfo { 
                        nongyao_name = DBCommon.SNN(item.name),
                        sale_count = sale_count
                    });
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    count = retdata.Count(),
                    data = retdata,
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData CheckCatalogRegisterId(ServiceDBDataContext db,
            string register_id, long shop_id)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                tbl_shop shop = (from m in db.tbl_shops
                                 where m.id == shop_id && m.deleted == 0
                                 select m).FirstOrDefault();

                long region_id = 0;
                

                if (shop != null)
                {
                    tbl_region item = (from m in db.tbl_regions
                                       where m.deleted == 0 && m.id == shop.region
                                       select m).FirstOrDefault();
                    if (item != null)
                    {
                        if (item.parentid == 0)
                            region_id = item.id;
                        else
                            region_id = (long)item.parentid;
                    }
                }

                tbl_catalog catalogitem = (from m in db.tbl_catalogs
                                           where m.deleted == 0 && m.register_id == register_id && m.region_id == region_id
                                           select m).FirstOrDefault();

                int check = 0, pass = 0;
                if (catalogitem != null)
                {
                    check = 1;
                    pass = catalogitem.pass;
                }

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    check = check,
                    pass = pass
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        public NongYaoResponseData GetNickname(ServiceDBDataContext db,
            string name)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            try
            {
                string nickname = DBCommon.GetPinyinCode(name);

                result.Result = NONGYAOERROR.ERR_SUCCESS;
                result.Data = new
                {
                    nickname = nickname
                };
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;

        }

        #endregion

        #region Version Management
        public int AvailableNewVersion(string oldVersion, string newVersion)
        {
            int pos = oldVersion.LastIndexOf(".");

            int oldMajor = 0, oldMinor = 0;
            if (pos > 0)
            {
                oldMajor = Convert.ToInt32(oldVersion.Substring(0, pos));
                oldMinor = Convert.ToInt32(oldVersion.Substring(pos + 1, oldVersion.Length - pos - 1));
            }
            else
                return 0;

            pos = newVersion.LastIndexOf(".");
            int newMajor = 0, newMinor = 0;
            if (pos > 0)
            {
                newMajor = Convert.ToInt32(newVersion.Substring(0, pos));
                newMinor = Convert.ToInt32(newVersion.Substring(pos + 1, newVersion.Length - pos - 1));
            }
            else
                return 0;

            if (oldMajor < newMajor)
                return 1;
            else if (oldMajor == newMajor)
            {
                if (oldMinor < newMinor)
                    return 1;
                else
                    return 0;
            }
            else
                return 0;
        }

        public NongYaoResponseData GetNewVersion(string version)
        {
            NongYaoResponseData retData = new NongYaoResponseData();
            ServiceDBDataContext context = new ServiceDBDataContext();

            int nNewVersion = 0;
            string serverVersion = string.Empty, serverURL = string.Empty;

            try
            {
                tbl_version verItem = (from m in context.tbl_versions
                                        select m).First();

                serverVersion = verItem.andver;
                serverURL = verItem.andurl;

                if (serverVersion != null)
                {
                    nNewVersion = AvailableNewVersion(version, serverVersion);

                    retData.Result = NONGYAOERROR.ERR_SUCCESS;
                }
                else
                {
                    retData.Result = NONGYAOERROR.ERR_FAILURE;
                }
            }
            catch (System.Exception ex)
            {
                retData.Result = NONGYAOERROR.ERR_FAILURE;
            }

            if (nNewVersion == 1)
                retData.Data = serverURL;
            else
                retData.Data = string.Empty;

            return retData;
        }
        #endregion
    }
}