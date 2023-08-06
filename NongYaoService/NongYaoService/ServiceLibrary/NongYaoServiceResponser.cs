using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NongYaoService.ServiceModel;
using NongYaoService.ServiceLibrary;


namespace NongYaoService.ServiceDB
{
    public class NongYaoServiceResponser
    {
        #region Fields and Properties
        public ServiceDBDataContext _dbcontext = new ServiceDBDataContext();

        private DBUser _dbUser = new DBUser();
        private DBNongYao _dbNongYao = new DBNongYao();
        #endregion

        #region API Functions
        public NongYaoResponseData Login(string userid, string password)
        {
            return _dbUser.Login(_dbcontext, userid, password);
        }

        public NongYaoResponseData GetBankCashLogList(long shop_id, string start_date, string end_date, int pagenum)
        {
            return _dbNongYao.GetBankCashLogList(_dbcontext, shop_id, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetInOutLog(long shop_id, string start_date, string end_date, int pagenum)
        {
            return _dbNongYao.GetInOutLog(_dbcontext, shop_id, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetInOutTotalProfit(long shop_id, string start_date, string end_date)
        {
            return _dbNongYao.GetInOutTotalProfit(_dbcontext, shop_id, start_date, end_date);
        }

        public NongYaoResponseData GetUnitList()
        {
            return _dbNongYao.GetUnitList(_dbcontext);
        }

        //public NongYaoResponseData GetNongYaoKindList()
        //{
        //    return _dbNongYao.GetNongYaoKindList(_dbcontext);
        //}

        public NongYaoResponseData GetRegionList()
        {
            return _dbNongYao.GetRegionList(_dbcontext);
        }

        public NongYaoResponseData GetStoreList(long shop_id)
        {
            return _dbNongYao.GetStoreList(_dbcontext, shop_id);
        }
        
        public NongYaoResponseData GetCustomerList(long shop_id)
        {
            return _dbNongYao.GetCustomerList(_dbcontext, shop_id);
        }

        public NongYaoResponseData GetCustomerInfo(long shop_id, string name, string phone)
        {
            return _dbNongYao.GetCustomerInfo(_dbcontext, shop_id, name, phone);
        }

        public NongYaoResponseData GetSupplyList(long shop_id)
        {
            return _dbNongYao.GetSupplyList(_dbcontext, shop_id);
        }

        public NongYaoResponseData GetUserList(long shop_id, string role)
        {
            return _dbNongYao.GetUserList(_dbcontext, shop_id, role);
        }

        public NongYaoResponseData GetShopInfo(long shop_id, string uid)
        {
            return _dbNongYao.GetShopInfo(_dbcontext, shop_id, uid);
        }

        public NongYaoResponseData GetShopUserDetailList(long shop_id, int pagenum)
        {
            return _dbNongYao.GetShopUserDetailList(_dbcontext, shop_id, pagenum);
        }

        public NongYaoResponseData GetStoreDetailList(long shop_id)
        {
            return _dbNongYao.GetStoreDetailList(_dbcontext, shop_id);
        }

        public NongYaoResponseData GetCatalogRemainList(long shop_id, long catalog_id)
        {
            return _dbNongYao.GetCatalogRemainList(_dbcontext, shop_id, catalog_id);
        }

        public NongYaoResponseData GetCatalogRemainListWithBarcode(long shop_id, string barcode)
        {
            return _dbNongYao.GetCatalogRemainListWithBarcode(_dbcontext, shop_id, barcode);
        }

        public NongYaoResponseData GetOtherPayList(long shop_id, byte type,
            string start_date,
            string end_date, int pagenum)
        {
            return _dbNongYao.GetOtherPayList(_dbcontext, shop_id, type, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetTicketNumber(long shop_id, int type)
        {
            return _dbNongYao.GetTicketNumber(_dbcontext, shop_id, type);
        }

        public NongYaoResponseData GetCatalogInfoFromBarcode(
            long shop_id,
            string barcode)
        {
            return _dbNongYao.GetCatalogInfoFromBarcode(_dbcontext, shop_id, barcode);
        }

        public NongYaoResponseData GetCatalogListFromStore(long shop_id, long store_id, string search_name)
        {
            return _dbNongYao.GetCatalogListFromStore(_dbcontext, shop_id, store_id, search_name);
        }

        public NongYaoResponseData GetPaymentLog(
            long shop_id,
            string customer_search,
            byte type,
            string start_date,
            string end_date,
            int pagenum)
        {
            return _dbNongYao.GetPaymentLog(_dbcontext, shop_id, customer_search, type, start_date, end_date, pagenum);
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
            NongYaoResponseData ret = new NongYaoResponseData();

            long userid = _dbUser.CheckUser(_dbcontext, uid);

            if (userid >= 0)
            {
                try
                {
                    ret = _dbNongYao.EditShopInfo(_dbcontext, uid, shop_id, nickname, address, region, username, mobile_phone, phone);
                }
                catch (System.Exception ex)
                {
                    NongYaoCommon.LogErrors(ex.ToString());
                    ret.Result = NONGYAOERROR.ERR_FAILURE;
                }
                return ret;
            }
            else
            {
                ret.Result = NONGYAOERROR.ERR_FAILURE;
            }            
            return ret;
        }

        public NongYaoResponseData EditShopPositionInfo(
           long uid,
           long shop_id,
           decimal longitude,
           decimal latitude)
        {
            NongYaoResponseData ret = new NongYaoResponseData();

            long userid = _dbUser.CheckUser(_dbcontext, uid);

            if (userid >= 0)
            {
                try
                {
                    ret = _dbNongYao.EditShopPositionInfo(_dbcontext, uid, shop_id, longitude, latitude);
                }
                catch (System.Exception ex)
                {
                    NongYaoCommon.LogErrors(ex.ToString());
                    ret.Result = NONGYAOERROR.ERR_FAILURE;
                }
                return ret;
            }
            else
            {
                ret.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return ret;
        }

        public NongYaoResponseData AddShopUser(
            long shop_id,
            string name,
            string userid,
            string password,
            string phone,
            string role)
        {          
           return _dbNongYao.AddShopUser(_dbcontext, shop_id, name, userid, password, phone, role);
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
            return _dbNongYao.EditShopUser(_dbcontext, shop_id, uid, name, userid, password, phone, role);
        }

        public NongYaoResponseData DelShopUser(
            long shop_id,
            long uid)
        {
            return _dbNongYao.DelShopUser(_dbcontext, shop_id, uid);
        }

        public NongYaoResponseData AddStore(
            long shop_id,
            string storename,
            string uid)
        {
            return _dbNongYao.AddStore(_dbcontext, shop_id, storename, uid);
        }

        public NongYaoResponseData EditStore(
            long shop_id,
            long store_id,
            string storename,
            string uid)
        {
            return _dbNongYao.EditStore(_dbcontext, shop_id, store_id,storename, uid);
        }

        public NongYaoResponseData DelStore(
            long shop_id,
            long store_id)
        {
            return _dbNongYao.DelStore(_dbcontext, shop_id, store_id);
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
            return _dbNongYao.BuyingCatalog(_dbcontext, shop_id, uid, ticketnum, store_id, supply_id, paytype, catalogcount, cataloglist);
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
            return _dbNongYao.SaleCatalog(_dbcontext, shop_id, uid, ticketnum, customer_name, customer_phone, store_id, paytype, catalogcount, cataloglist, sellmoney, sellchange);
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
            return _dbNongYao.RejectCatalog(_dbcontext, shop_id, uid, ticketnum, store_id, customer_name, customer_phone, paytype, catalogcount, cataloglist);
        }

        public NongYaoResponseData MovingCatalog(
            long shop_id,
            long uid,         
            long count,
            string cataloglist)
        {
            return _dbNongYao.MovingCatalog(_dbcontext, shop_id, uid, count, cataloglist);
        }

        public NongYaoResponseData AddCatalogUsingLog(
            long shop_id,
            long uid,
            long catalogcount,
            string cataloglist)
        {
            return _dbNongYao.AddCatalogUsingLog(_dbcontext, shop_id, uid, catalogcount, cataloglist);
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
            return _dbNongYao.AddPaymentLog(_dbcontext, shop_id, uid, customer_id, price, type, paytype, change, etc);
        }

        public NongYaoResponseData EditPaymentLog(
            long shop_id,
            long uid,
            long payment_id,
            long customer_id,           
            decimal price,
            byte type)
        {
            return _dbNongYao.EditPaymentLog(_dbcontext, shop_id, uid, payment_id, customer_id, price, type);
        }

        public NongYaoResponseData DelPaymentLog(           
            long shop_id,
            long uid,
            long payment_id)
        {
            return _dbNongYao.DelPaymentLog(_dbcontext, shop_id, uid, payment_id);        
        }

        public NongYaoResponseData AddOtherpayLog(
            long shop_id,
            long uid,
            decimal price,
            byte type,
            byte paytype,
            string reason)
        {
            return _dbNongYao.AddOtherpayLog(_dbcontext, shop_id, uid, price, type, paytype, reason);
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
            return _dbNongYao.EditOtherpayLog(_dbcontext, shop_id, uid, otherpay_id, price, type, paytype, reason);
        }

        public NongYaoResponseData DelOtherpayLog(
           long shop_id,
           long uid,
           long otherpay_id)
        {
            return _dbNongYao.DelOtherpayLog(_dbcontext, shop_id, uid, otherpay_id);
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
            return _dbNongYao.RequestAddCatalog(_dbcontext, uid, shop_id, register_id, permit_id, sample_id, catalog_usingname, catalog_nickname, product, shape, material, content, product_area, level, description, image);
        }

        public NongYaoResponseData GetRemainCatalogStandardList(
            long shop_id,
            long store_id,
            long catalog_id)
        {
            return _dbNongYao.GetRemainCatalogStandardList(_dbcontext, shop_id, store_id, catalog_id);
        }

        public NongYaoResponseData GetLargenumberList(
            long shop_id,
            long store_id,
            long catalog_id,
            long standard_id)
        {
            return _dbNongYao.GetLargenumberList(_dbcontext, shop_id, store_id, catalog_id, standard_id);
        }

        public NongYaoResponseData SaleHistory(
            long shop_id,
            byte type,
            string start,
            string end,
            string search,
            int pagenum)
        {
            return _dbNongYao.SaleHistory(_dbcontext, shop_id, type, start, end, search, pagenum);
        }

        public NongYaoResponseData SaleDetail(
            long shop_id,
            long ticket_id)
        {
            return _dbNongYao.SaleDetail(_dbcontext, shop_id, ticket_id);
        }

        public NongYaoResponseData GetCatalogInfoFromBarcodeAndStore(
            string barcode,
            long store_id)
        {
            return _dbNongYao.GetCatalogInfoFromBarcodeAndStore(_dbcontext, barcode, store_id);
        }

        public NongYaoResponseData GetQuantityFromCatalogAndStandard(
            string largenumber,
            long store_id,
            long catalog_id,
            long standard_id)
        {
            return _dbNongYao.GetQuantityFromCatalogAndStandard(_dbcontext, largenumber, store_id, catalog_id, standard_id);
        }

        public NongYaoResponseData GetCatalogInfoFromIdAndStore(
            long catalog_id,
                long store_id)
        {
            return _dbNongYao.GetCatalogInfoFromIdAndStore(_dbcontext, catalog_id, store_id);
        }

        public NongYaoResponseData GetInOutLogDetail(
            long shop_id,
            string date)
        {
            return _dbNongYao.GetInOutLogDetail(_dbcontext, shop_id, date);
        }

        public NongYaoResponseData GetPaymentDetailLog(
            long shop_id,
            long payment_id)
        {
            return _dbNongYao.GetPaymentDetailLog(_dbcontext, shop_id, payment_id);
        }

        public NongYaoResponseData RealPaymentList(
            long shop_id,
            string customer_name,
            byte type,
            string start_date,
            string end_date,
            int pagenum)
        {
            return _dbNongYao.RealPaymentList(_dbcontext, shop_id, customer_name, type, start_date, end_date, pagenum);
        }

        public NongYaoResponseData MoneybankList(
            long shop_id,
            string start_date,
            string end_date,
            int pagenum)
        {
            return _dbNongYao.MoneybankList(_dbcontext, shop_id, start_date, end_date, pagenum);
        }

        public NongYaoResponseData MoneybankInfo(
            long shop_id,
            byte paytype,
            string date)
        {
            return _dbNongYao.MoneybankInfo(_dbcontext, shop_id, paytype, date);
        }

        public NongYaoResponseData BuyingDetailInfo(
            long shop_id,
            string start_date,
            string end_date)
        {
            return _dbNongYao.BuyingDetailInfo(_dbcontext, shop_id, start_date, end_date);
        }

        public NongYaoResponseData SaleDetailInfo(
            long shop_id,
            string start_date,
            string end_date)
        {
            return _dbNongYao.SaleDetailInfo(_dbcontext, shop_id, start_date, end_date);
        }

        public NongYaoResponseData StoreMovingDetailInfo(
            long shop_id,
            string start_date,
            string end_date)
        {
            return _dbNongYao.StoreMovingDetailInfo(_dbcontext, shop_id, start_date, end_date);
        }

        public NongYaoResponseData SpendingDetailInfo(
            long shop_id,
            string start_date,
            string end_date)
        {
            return _dbNongYao.SpendingDetailInfo(_dbcontext, shop_id, start_date, end_date);
        }

        public NongYaoResponseData RemainDetailInfo(
            long shop_id,
            string start_date,
            string end_date)
        {
            return _dbNongYao.RemainDetailInfo(_dbcontext, shop_id, start_date, end_date);
        }

        public NongYaoResponseData PayingDetailInfo(
            long shop_id,
            string start_date,
            string end_date)
        {
            return _dbNongYao.PayingDetailInfo(_dbcontext, shop_id, start_date, end_date);
        }

        public NongYaoResponseData MoneyDetailInfo(
            long shop_id,
            string start_date,
            string end_date)
        {
            return _dbNongYao.MoneyDetailInfo(_dbcontext, shop_id, start_date, end_date);
        }

        public NongYaoResponseData GetShopInfoFromNickname(
            string nickname,
            long region_id)
        {
            return _dbNongYao.GetShopInfoFromNickname(_dbcontext, nickname, region_id);
        }

        public NongYaoResponseData GetLastRegionList(
            int role,
            long region_id)
        {
            return _dbNongYao.GetLastRegionList(_dbcontext, role, region_id);
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
            return _dbNongYao.GetShopStatistic(_dbcontext, region_id, level, search_key, shop_id, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetNongyaoKindList()
        {
            return _dbNongYao.GetNongyaoKindList(_dbcontext);
        }

        public NongYaoResponseData GetCatalogInfoFromNickname(
            string nickname,
            long nongyao_id)
        {
            return _dbNongYao.GetCatalogInfoFromNickname(_dbcontext, nickname, nongyao_id);
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
            return _dbNongYao.GetCatalogStatistics(_dbcontext, region_id, nongyao_id, catalog_id, search_key, start_date, end_date, pagenum);
        }

        public NongYaoResponseData GetCatalogDetailStatistics(
            long catalog_id,
            long standard_id,
            long region_id,
            string start_date,
            string end_date)
        {
            return _dbNongYao.GetCatalogDetailStatistics(_dbcontext, catalog_id, standard_id, region_id, start_date, end_date);
        }

        public NongYaoResponseData GetStatistics(
            long year)
        {
            return _dbNongYao.GetStatistics(_dbcontext, year);
        }

        public NongYaoResponseData GetBarGraph(
            long year)
        {
            return _dbNongYao.GetBarGraph(_dbcontext, year);
        }

        public NongYaoResponseData GetLineGraph(
            long year)
        {
            return _dbNongYao.GetLineGraph(_dbcontext, year);
        }

        public NongYaoResponseData GetPieGraph(
            long year)
        {
            return _dbNongYao.GetPieGraph(_dbcontext, year);
        }

        public NongYaoResponseData CheckCatalogRegisterId(
            string register_id, long shop_id)
        {
            return _dbNongYao.CheckCatalogRegisterId(_dbcontext, register_id, shop_id);
        }

        public NongYaoResponseData GetNickname(
            string name)
        {
            return _dbNongYao.GetNickname(_dbcontext, name);
        }

        #endregion

        #region Version Management
        public NongYaoResponseData GetNewVersion(string version)
        {
            return _dbNongYao.GetNewVersion(version);
        }
        #endregion
    }
}