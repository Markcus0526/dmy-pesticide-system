using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Linq;
using System.ServiceModel.Web;
using NongYaoService.ServiceModel;
using NongYaoService.ServiceDB;
using NongYaoService.Json;

namespace NongYaoService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService
    {
        NongYaoServiceResponser _serviceResponser = new NongYaoServiceResponser();

        public NongYaoResponseData Login(string userid, string password)
        {
            return _serviceResponser.Login(userid, password);
        }

        public NongYaoResponseData GetBankCashLogList(long shop_id, string start_date, string end_date, int pagenum)
        {
            return _serviceResponser.GetBankCashLogList(shop_id, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetInOutLog(long shop_id, string start_date, string end_date, int pagenum)
        {
            return _serviceResponser.GetInOutLog(shop_id, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetInOutTotalProfit(long shop_id, string start_date, string end_date)
        {
            return _serviceResponser.GetInOutTotalProfit(shop_id, start_date, end_date);
        }

        public NongYaoResponseData GetUnitList()
        {
            return _serviceResponser.GetUnitList();
        }

        //public NongYaoResponseData GetNongYaoKindList()
        //{
        //    return _serviceResponser.GetNongYaoKindList();
        //}

        public NongYaoResponseData GetRegionList()
        {
            return _serviceResponser.GetRegionList();
        }

        public NongYaoResponseData GetStoreList(long shop_id)
        {
            return _serviceResponser.GetStoreList(shop_id);
        }

        public NongYaoResponseData GetCustomerList(long shop_id)
        {
            return _serviceResponser.GetCustomerList(shop_id);
        }

        public NongYaoResponseData GetCustomerInfo(long shop_id, string name, string phone)
        {
            return _serviceResponser.GetCustomerInfo(shop_id, name, phone);
        }

        public NongYaoResponseData GetSupplyList(long shop_id)
        {
            return _serviceResponser.GetSupplyList(shop_id);
        }

        public NongYaoResponseData GetUserList(long shop_id, string role)
        {
            return _serviceResponser.GetUserList(shop_id, role);
        }

        public NongYaoResponseData GetShopInfo(long shop_id, string uid)
        {
            return _serviceResponser.GetShopInfo(shop_id, uid);
        }

        public NongYaoResponseData GetShopUserDetailList(long shop_id, int pagenum)
        {
            return _serviceResponser.GetShopUserDetailList(shop_id, pagenum);
        }

        public NongYaoResponseData GetStoreDetailList(long shop_id)
        {
            return _serviceResponser.GetStoreDetailList(shop_id);
        }

        public NongYaoResponseData GetCatalogRemainList(long shop_id, long catalog_id)
        {
            return _serviceResponser.GetCatalogRemainList(shop_id, catalog_id);
        }

        public NongYaoResponseData GetCatalogRemainListWithBarcode(long shop_id, string barcode)
        {
            return _serviceResponser.GetCatalogRemainListWithBarcode(shop_id, barcode);
        }

        public NongYaoResponseData GetOtherPayList(long shop_id, byte type,
            string start_date,
            string end_date, int pagenum)
        {
            return _serviceResponser.GetOtherPayList(shop_id, type, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetTicketNumber(long shop_id, int type)
        {
            return _serviceResponser.GetTicketNumber(shop_id, type);
        }

        public NongYaoResponseData GetCatalogInfoFromBarcode(
            long shop_id, 
            string barcode)
        {
            return _serviceResponser.GetCatalogInfoFromBarcode(shop_id, barcode);
        }

        public NongYaoResponseData GetCatalogListFromStore(long shop_id, long store_id, string search_name)
        {
            return _serviceResponser.GetCatalogListFromStore(shop_id, store_id, search_name);
        }

        public NongYaoResponseData GetPaymentLog(
         long shop_id,
            string customer_search,
         byte type,
            string start_date,
            string end_date,
         int pagenum)
        {
            return _serviceResponser.GetPaymentLog(shop_id, customer_search, type, start_date, end_date, pagenum);
        }

        public NongYaoResponseData EditShopInfo(
            long uid,
            long shop_id,
            string nickname,
            string address,
            long region,
            string username,
            string mobile_phone,
            string phone)
        {
            return _serviceResponser.EditShopInfo( uid, shop_id, nickname, address, region, username, mobile_phone, phone);
        }

        public NongYaoResponseData EditShopPositionInfo(
            long uid,
            long shop_id,
            decimal longitude,
            decimal latitude)
        {
            return _serviceResponser.EditShopPositionInfo(uid, shop_id, longitude, latitude);
        }

        public NongYaoResponseData AddShopUser(
            long shop_id,
            string name,
            string userid,
            string password,
            string phone,
            string role)
        {
            return _serviceResponser.AddShopUser( shop_id, name, userid, password, phone, role);
        }

        public NongYaoResponseData EditShopUser(
            long shop_id,
            long uid,
            string name,
            string userid,
            string password,
            string phone,
            string role)
        {
            return _serviceResponser.EditShopUser(shop_id, uid, name, userid, password, phone, role);
        }

        public NongYaoResponseData DelShopUser(
            long shop_id,
            long uid)
        {
            return _serviceResponser.DelShopUser(shop_id, uid);
        }

        public NongYaoResponseData AddStore(
             long shop_id,
             string storename,
             string uid)
        {
            return _serviceResponser.AddStore(shop_id, storename, uid);
        }

        public NongYaoResponseData EditStore(
            long shop_id,
            long store_id,
            string storename,
            string uid)
        {
            return _serviceResponser.EditStore(shop_id, store_id, storename, uid);
        }

        public NongYaoResponseData DelStore(
            long shop_id,
            long store_id)
        {
            return _serviceResponser.DelStore(shop_id, store_id);
        }

        public NongYaoResponseData BuyingCatalog(
            long shop_id,
            long uid,
            string ticketnum,
            long store_id,
            long supply_id,
            int paytype,
            long catalogcount,
            string cataloglist)
        {
            return _serviceResponser.BuyingCatalog(shop_id, uid, ticketnum, store_id, supply_id, paytype, catalogcount, cataloglist);
        }

        public NongYaoResponseData SaleCatalog(
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
            return _serviceResponser.SaleCatalog(shop_id, uid, ticketnum, customer_name, customer_phone, store_id, paytype, catalogcount, cataloglist, sellmoney, sellchange);
        }

        public NongYaoResponseData RejectCatalog(
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
            return _serviceResponser.RejectCatalog(shop_id, uid, ticketnum, store_id, customer_name, customer_phone, paytype, catalogcount, cataloglist);
        }

        public NongYaoResponseData MovingCatalog(
           long shop_id,
           long uid,          
           long count,
           string cataloglist)
        {
            return _serviceResponser.MovingCatalog(shop_id, uid, count, cataloglist);
        }

        public NongYaoResponseData AddCatalogUsingLog(
           long shop_id,
            long uid,
           long catalogcount,
           string cataloglist)
        {
            return _serviceResponser.AddCatalogUsingLog(shop_id, uid, catalogcount, cataloglist);
        }

        public NongYaoResponseData AddPaymentLog(
            long shop_id,
            long uid,
            long customer_id,           
            decimal price,
            byte type,
            byte paytype,
            decimal change,
            string etc)
        {
            return _serviceResponser.AddPaymentLog(shop_id, uid, customer_id, price, type, paytype, change, etc);
        }

        public NongYaoResponseData EditPaymentLog(
            long shop_id,
            long uid,
            long payment_id,
            long customer_id,            
            decimal price,
            byte type)
        {
            return _serviceResponser.EditPaymentLog(shop_id, uid, payment_id, customer_id, price, type);
        }

        public NongYaoResponseData DelPaymentLog(
            long shop_id,
            long uid,
            long payment_id)
        {
            return _serviceResponser.DelPaymentLog(shop_id, uid, payment_id);
        }

        public NongYaoResponseData AddOtherpayLog(
            long shop_id,
            long uid,
            decimal price,
            byte type,
            byte paytype,
            string reason)
        {
            return _serviceResponser.AddOtherpayLog(shop_id, uid, price, type, paytype, reason);
        }

        public NongYaoResponseData EditOtherpayLog(
            long shop_id,
            long uid,
            long otherpay_id,
            decimal price,
            byte type,
            byte paytype,
            string reason)
        {
            return _serviceResponser.EditOtherpayLog(shop_id, uid, otherpay_id, price, type, paytype, reason);
        }

        public NongYaoResponseData DelOtherpayLog(
          long shop_id,
          long uid,
          long otherpay_id)
        {
            return _serviceResponser.DelOtherpayLog(shop_id, uid, otherpay_id);
        }

        public NongYaoResponseData RequestAddCatalog(
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
            return _serviceResponser.RequestAddCatalog(uid, shop_id, register_id, permit_id, sample_id, catalog_usingname, catalog_nickname, product, shape, material, content, product_area, level, description, image);
        }

        public NongYaoResponseData GetRemainCatalogStandardList(
           long shop_id,
            long store_id,
            long catalog_id)
        {
            return _serviceResponser.GetRemainCatalogStandardList(shop_id, store_id, catalog_id);
        }

        public NongYaoResponseData GetLargenumberList(
           long shop_id,
            long store_id,
            long catalog_id,
            long standard_id)
        {
            return _serviceResponser.GetLargenumberList(shop_id, store_id, catalog_id, standard_id);
        }

        public NongYaoResponseData SaleHistory(
           long shop_id,
            byte type,
            string start,
            string end,
            string search,
            int pagenum)
        {
            return _serviceResponser.SaleHistory(shop_id, type, start, end, search, pagenum);
        }

        public NongYaoResponseData SaleDetail(
           long shop_id,
            long ticket_id)
        {
            return _serviceResponser.SaleDetail(shop_id, ticket_id);
        }

        public NongYaoResponseData GetCatalogInfoFromBarcodeAndStore(
           string barcode,
            long store_id)
        {
            return _serviceResponser.GetCatalogInfoFromBarcodeAndStore(barcode, store_id);
        }

        public NongYaoResponseData GetQuantityFromCatalogAndStandard(
             string largenumber,
            long store_id,
            long catalog_id,
            long standard_id)
        {
            return _serviceResponser.GetQuantityFromCatalogAndStandard(largenumber, store_id, catalog_id, standard_id);
        }

        public NongYaoResponseData GetCatalogInfoFromIdAndStore(
            long catalog_id,
            long store_id)
        {
            return _serviceResponser.GetCatalogInfoFromIdAndStore(catalog_id, store_id);
        }
        public NongYaoResponseData GetInOutLogDetail(
           long shop_id,
            string date)
        {
            return _serviceResponser.GetInOutLogDetail(shop_id, date);
        }

        public NongYaoResponseData GetPaymentDetailLog(
           long shop_id,
            long payment_id)
        {
            return _serviceResponser.GetPaymentDetailLog(shop_id, payment_id);
        }

        public NongYaoResponseData RealPaymentList(
           long shop_id,
            string customer_name,
            byte type,
            string start_date,
            string end_date,
            int pagenum)
        {
            return _serviceResponser.RealPaymentList(shop_id, customer_name, type, start_date, end_date, pagenum);
        }

        public NongYaoResponseData MoneybankList(
           long shop_id,
            string start_date,
            string end_date,
            int pagenum)
        {
            return _serviceResponser.MoneybankList(shop_id, start_date, end_date, pagenum);
        }

        public NongYaoResponseData MoneybankInfo(
           long shop_id,
            byte paytype,
            string date)
        {
            return _serviceResponser.MoneybankInfo(shop_id, paytype, date);
        }

        public NongYaoResponseData BuyingDetailInfo(
           long shop_id,
            string start_date,
            string end_date)
        {
            return _serviceResponser.BuyingDetailInfo(shop_id, start_date, end_date);
        }

        public NongYaoResponseData SaleDetailInfo(
           long shop_id,
            string start_date,
            string end_date)
        {
            return _serviceResponser.SaleDetailInfo(shop_id, start_date, end_date);
        }

        public NongYaoResponseData StoreMovingDetailInfo(
           long shop_id,
            string start_date,
            string end_date)
        {
            return _serviceResponser.StoreMovingDetailInfo(shop_id, start_date, end_date);
        }

        public NongYaoResponseData SpendingDetailInfo(
           long shop_id,
            string start_date,
            string end_date)
        {
            return _serviceResponser.SpendingDetailInfo(shop_id, start_date, end_date);
        }

        public NongYaoResponseData RemainDetailInfo(
           long shop_id,
            string start_date,
            string end_date)
        {
            return _serviceResponser.RemainDetailInfo(shop_id, start_date, end_date);
        }

        public NongYaoResponseData PayingDetailInfo(
           long shop_id,
            string start_date,
            string end_date)
        {
            return _serviceResponser.PayingDetailInfo(shop_id, start_date, end_date);
        }

        public NongYaoResponseData MoneyDetailInfo(
           long shop_id,
            string start_date,
            string end_date)
        {
            return _serviceResponser.MoneyDetailInfo(shop_id, start_date, end_date);
        }

        public NongYaoResponseData GetShopInfoFromNickname(
            string nickname,
            long region_id)
        {
            return _serviceResponser.GetShopInfoFromNickname(nickname, region_id);
        }

        public NongYaoResponseData GetLastRegionList(
            int role,
            long region_id)
        {
            return _serviceResponser.GetLastRegionList(role, region_id);
        }

        public NongYaoResponseData GetShopStatistic(
           long region_id,
            byte level,
            string search_key,
            long shop_id,
            string start_date,
            string end_date,
            int pagenum)
        {
            return _serviceResponser.GetShopStatistic(region_id, level, search_key, shop_id, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetNongyaoKindList()
        {
            return _serviceResponser.GetNongyaoKindList();
        }

        public NongYaoResponseData GetCatalogInfoFromNickname(
            string nickname,
            long nongyao_id)
        {
            return _serviceResponser.GetCatalogInfoFromNickname(nickname, nongyao_id);
        }

        public NongYaoResponseData GetCatalogStatistics(
            long region_id,
            long nongyao_id,
            long catalog_id,
            string search_key,
            string start_date,
            string end_date,
            int pagenum)
        {
            return _serviceResponser.GetCatalogStatistics(region_id, nongyao_id, catalog_id, search_key, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetCatalogDetailStatistics(
            long catalog_id,
            long standard_id,
            long region_id,
            string start_date,
            string end_date)
        {
            return _serviceResponser.GetCatalogDetailStatistics(catalog_id, standard_id, region_id, start_date, end_date);
        }

        public NongYaoResponseData GetStatistics(
            long year)
        {
            return _serviceResponser.GetStatistics(year);
        }

        public NongYaoResponseData GetBarGraph(
            long year)
        {
            return _serviceResponser.GetBarGraph(year);
        }

        public NongYaoResponseData GetLineGraph(
            long year)
        {
            return _serviceResponser.GetLineGraph(year);
        }

        public NongYaoResponseData GetPieGraph(
            long year)
        {
            return _serviceResponser.GetPieGraph(year);
        }

        public NongYaoResponseData CheckCatalogRegisterId(
            string register_id, long shop_id)
        {
            return _serviceResponser.CheckCatalogRegisterId(register_id, shop_id);
        }

        public NongYaoResponseData GetNickname(
            string name)
        {
            return _serviceResponser.GetNickname(name);
        }

        public NongYaoResponseData GetNewVersion(string version)
        {
            return _serviceResponser.GetNewVersion(version);
        }
    }
}
