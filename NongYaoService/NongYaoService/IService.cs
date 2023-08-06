using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Linq;
using System.ServiceModel.Web;
using NongYaoService.ServiceModel;
using Newtonsoft.Json;

namespace NongYaoService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService
    {
        // TODO: Add your service operations here
        #region API_AUTH

        #endregion

        #region API_NONGYAOSERVICE
        [OperationContract]
        [WebGet(
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json)]
        NongYaoResponseData Login(String userid, String password);

        [WebGet, OperationContract]
        NongYaoResponseData GetBankCashLogList(long shop_id, string start_date, string end_date, int pagenum);

       [WebGet, OperationContract]
        NongYaoResponseData GetInOutLog(long shop_id, string start_date, string end_date, int pagenum);

       [WebGet, OperationContract]
       NongYaoResponseData GetInOutTotalProfit(long shop_id, string start_date, string end_date);

        [WebGet, OperationContract]
        NongYaoResponseData GetUnitList();

        //[WebGet, OperationContract]
        //NongYaoResponseData GetNongYaoKindList();

        [WebGet, OperationContract]
        NongYaoResponseData GetRegionList();

        [WebGet, OperationContract]
        NongYaoResponseData GetStoreList(long shop_id);

        [WebGet, OperationContract]
        NongYaoResponseData GetCustomerList(long shop_id);

        [WebGet, OperationContract]
        NongYaoResponseData GetCustomerInfo(long shop_id, string name, string phone);

        [WebGet, OperationContract]
        NongYaoResponseData GetSupplyList(long shop_id);

        [WebGet, OperationContract]
        NongYaoResponseData GetUserList(long shop_id, string role);

        [WebGet, OperationContract]
        NongYaoResponseData GetShopInfo(long shop_id, string uid);

        [WebGet, OperationContract]
        NongYaoResponseData GetShopUserDetailList(long shop_id, int pagenum);

        [WebGet, OperationContract]
        NongYaoResponseData GetStoreDetailList(long shop_id);

        [WebGet, OperationContract]
        NongYaoResponseData GetCatalogRemainList(long shop_id, long catalog_id);

        [WebGet, OperationContract]
        NongYaoResponseData GetCatalogRemainListWithBarcode(long shop_id, string barcode);

        [WebGet, OperationContract]
        NongYaoResponseData GetOtherPayList(long shop_id, byte type,
            string start_date,
            string end_date, int pagenum);

        [WebGet, OperationContract]
        NongYaoResponseData GetTicketNumber(long shop_id, int type);

        [WebGet, OperationContract]
        NongYaoResponseData GetCatalogInfoFromBarcode(
            long shop_id, 
            string barcode);

        [WebGet, OperationContract]
        NongYaoResponseData GetCatalogListFromStore(
            long shop_id, 
            long store_id,
            string search_name);

        [WebGet, OperationContract]
        NongYaoResponseData GetPaymentLog(
            long shop_id,
            string customer_search,
            byte type,
            string start_date,
            string end_date,
            int pagenum
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData EditShopInfo(
            long uid,
            long shop_id,            
            string nickname,
            string address,
            long region,
            string username,
            string mobile_phone,
            string phone
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData EditShopPositionInfo(
            long uid,
            long shop_id,
            decimal longitude,
            decimal latitude
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData AddShopUser(
            long shop_id,
            string name,
            string userid,
            string password,
            string phone,
            string role
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData EditShopUser(
            long shop_id,
            long uid,
            string name,
            string userid,
            string password,
            string phone,
            string role
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData DelShopUser(
            long shop_id,
            long uid           
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData AddStore(
            long shop_id,
            string storename,
            string uid            
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData EditStore(
            long shop_id,
            long store_id,
            string storename,           
            string uid
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData DelStore(
            long shop_id,
            long store_id
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json)]       
        //[WebGet, OperationContract]
        NongYaoResponseData BuyingCatalog(
            long shop_id,
            long uid,
            string ticketnum,
            long store_id,
            long supply_id,
            int paytype,
            long catalogcount,
            string cataloglist
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
         NongYaoResponseData SaleCatalog(
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
            decimal sellchange
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData RejectCatalog(
            long shop_id,
            long uid,
            string ticketnum,
            long store_id,
            string customer_name,
            string customer_phone,
            int paytype,
            long catalogcount,
            string cataloglist
        );

        [OperationContract]
       [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json)]
     
        NongYaoResponseData MovingCatalog(
            long shop_id,
            long uid,           
            long count,
            string cataloglist
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData AddCatalogUsingLog(
            long shop_id,
            long uid,
            long catalogcount,
            string cataloglist
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData AddPaymentLog(
            long shop_id,
            long uid,
            long customer_id,            
            decimal price,
            byte type,
            byte paytype,
            decimal change,
            string etc
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData EditPaymentLog(
            long shop_id,
            long uid,
            long payment_id,
            long customer_id,           
            decimal price,
            byte type
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData DelPaymentLog(
            long shop_id,
            long uid,
            long payment_id
        );
        
        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData AddOtherpayLog(
            long shop_id,
            long uid,
            decimal price,
            byte type,
            byte paytype,
            string reason
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData EditOtherpayLog(
            long shop_id,
            long uid,
            long otherpay_id,
            decimal price,
            byte type,
            byte paytype,
            string reason
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData DelOtherpayLog(
            long shop_id,
            long uid,
            long otherpay_id
        );

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
        NongYaoResponseData RequestAddCatalog(
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
            string image
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetRemainCatalogStandardList(
            long shop_id,
            long store_id,
            long catalog_id
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetLargenumberList(
            long shop_id,
            long store_id,
            long catalog_id,
            long standard_id
        );

        [WebGet, OperationContract]
        NongYaoResponseData SaleHistory(
            long shop_id,
            byte type,
            string start,
            string end,
            string search,
            int pagenum
        );

        [WebGet, OperationContract]
        NongYaoResponseData SaleDetail(
            long shop_id,
            long ticket_id
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetCatalogInfoFromBarcodeAndStore(
            string barcode,
            long store_id
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetCatalogInfoFromIdAndStore(
            long catalog_id,
            long store_id
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetQuantityFromCatalogAndStandard(
            string largenumber,
            long store_id,
            long catalog_id,
            long standard_id            
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetInOutLogDetail(
            long shop_id,
            string date          
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetPaymentDetailLog(
            long shop_id,
            long payment_id
        );

        [WebGet, OperationContract]
        NongYaoResponseData RealPaymentList(
            long shop_id,
            string customer_name,
            byte type,
            string start_date,
            string end_date,
            int pagenum
        );

        [WebGet, OperationContract]
        NongYaoResponseData MoneybankList(
            long shop_id,
            string start_date,
            string end_date,
            int pagenum
        );

        [WebGet, OperationContract]
        NongYaoResponseData MoneybankInfo(
            long shop_id,
            byte paytype,
            string date
        );

        [WebGet, OperationContract]
        NongYaoResponseData BuyingDetailInfo(
            long shop_id,
            string start_date,
            string end_date
        );

        [WebGet, OperationContract]
        NongYaoResponseData SaleDetailInfo(
            long shop_id,
            string start_date,
            string end_date
        );

        [WebGet, OperationContract]
        NongYaoResponseData StoreMovingDetailInfo(
            long shop_id,
            string start_date,
            string end_date
        );

        [WebGet, OperationContract]
        NongYaoResponseData SpendingDetailInfo(
            long shop_id,
            string start_date,
            string end_date
        );

        [WebGet, OperationContract]
        NongYaoResponseData RemainDetailInfo(
            long shop_id,
            string start_date,
            string end_date
        );

        [WebGet, OperationContract]
        NongYaoResponseData PayingDetailInfo(
            long shop_id,
            string start_date,
            string end_date
        );

        [WebGet, OperationContract]
        NongYaoResponseData MoneyDetailInfo(
            long shop_id,
            string start_date,
            string end_date
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetShopInfoFromNickname(
            string nickname,
            long region_id
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetLastRegionList(
            int role,
            long region_id
        );

        [WebGet, OperationContract]
        NongYaoResponseData GetShopStatistic(
            long region_id,
            byte level,
            string search_key,
            long shop_id,
            string start_date,
            string end_date,
            int pagenum);

        [WebGet, OperationContract]
        NongYaoResponseData GetNongyaoKindList();

        [WebGet, OperationContract]
        NongYaoResponseData GetCatalogInfoFromNickname(
            string nickname,
            long nongyao_id);

        [WebGet, OperationContract]
        NongYaoResponseData GetCatalogStatistics(
            long region_id,
            long nongyao_id,
            long catalog_id,
            string search_key,
            string start_date,
            string end_date,
            int pagenum);

        [WebGet, OperationContract]
        NongYaoResponseData GetCatalogDetailStatistics(
            long catalog_id,
            long standard_id,
            long region_id,
            string start_date,
            string end_date);

        [WebGet, OperationContract]
        NongYaoResponseData GetStatistics(
            long year);

        [WebGet, OperationContract]
        NongYaoResponseData GetBarGraph(
            long year);

        [WebGet, OperationContract]
        NongYaoResponseData GetLineGraph(
            long year);

        [WebGet, OperationContract]
        NongYaoResponseData GetPieGraph(
            long year);

        [WebGet, OperationContract]
        NongYaoResponseData CheckCatalogRegisterId(
            string register_id,
            long shop_id);

        [WebGet, OperationContract]
        NongYaoResponseData GetNickname(
            string name);
        #endregion

        #region Version Management
        [WebGet, OperationContract]
        NongYaoResponseData GetNewVersion( string version );
        #endregion
    }

}
