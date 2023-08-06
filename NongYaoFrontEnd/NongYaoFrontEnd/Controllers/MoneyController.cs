using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NongYaoFrontEnd.Models;
using NongYaoFrontEnd.Models.Library;
namespace NongYaoFrontEnd.Controllers
{
    public class MoneyController : Controller
    {
        //
        // GET: /Money/
        static string m_searchWord = "";
        static byte m_searchType = 2;   //全都

        static byte m_stat_searchType = 2;

        static DateTime m_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" +Convert.ToString(DateTime.Now.Day));
        static DateTime m_end_date = DateTime.Now;

        static DateTime f_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
        static DateTime f_end_date = DateTime.Now;
        static string c_customer_name = "";
        static byte c_type = 2;

        static DateTime a_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
        static DateTime a_end_date = DateTime.Now;
        static string a_customer_name = "";
        static byte a_type = 2;
        static long credit_id = 0;

        [Authorize(Roles = "admin,account")]
        public ActionResult Index()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["userrole"] = CommonModel.GetCurrentUserRole();
            ViewData["userName"] = CommonModel.GetCurrentUserName();
            return View();
        }

        [Authorize(Roles = "admin,account")]
        public ActionResult Finance()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            m_searchWord = "";
            m_searchType = 2;   //全都

            m_stat_searchType = 2;

            m_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" +Convert.ToString(DateTime.Now.Day));
            m_end_date = DateTime.Now;

            f_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            f_end_date = DateTime.Now;
            ViewData["rootUri"] = rootUri;
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

            return View();
        }

        [Authorize(Roles = "admin,account")]
        public ActionResult Credit()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            m_searchWord = "";
            m_searchType = 2;   //全都

            m_stat_searchType = 2;

            m_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            m_end_date = DateTime.Now;

            f_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            f_end_date = DateTime.Now;
            ViewData["rootUri"] = rootUri;
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            long user_id = CommonModel.GetCurrentUserId();
            ViewData["username"] = UserModel.GetUserNameForId(user_id);

            return View();
        }

        [Authorize(Roles = "admin,account")]
        public ActionResult Statistics()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            m_searchWord = "";
            m_searchType = 2;   //全都

            m_stat_searchType = 2;

            m_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            m_end_date = DateTime.Now;

            f_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            f_end_date = DateTime.Now;
            ViewData["rootUri"] = rootUri;
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            ViewData["total_shouru"] = MoneyModel.GetTotalShouZhi(1, f_start_date, f_end_date);
            ViewData["total_zhichu"] = MoneyModel.GetTotalShouZhi(0, f_start_date, f_end_date);



            return View();
        }

        [Authorize(Roles = "admin,account")]
        public ActionResult Bank()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            ViewData["enddatelimit"] = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewData["startdatelimit"] = String.Format("{0:#}-01-01", DateTime.Now.Year);

            m_searchWord = "";
            m_searchType = 2;   //全都

            m_stat_searchType = 2;

            m_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            m_end_date = DateTime.Now;

            f_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            f_end_date = DateTime.Now;
            ViewData["rootUri"] = rootUri;

            return View();
        }

        [Authorize(Roles = "admin,account")]
        public ActionResult BossReport()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            ViewData["rootUri"] = rootUri;

            return View();
        }

        public JsonResult RetrieveBossReport(string startdate, string enddate)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            DateTime start = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            try { start = DateTime.Parse(startdate); }
            catch (Exception e) { }

            DateTime end = DateTime.Now;
            try { end = DateTime.Parse(enddate); }
            catch (Exception e) { }

            var buying_count = RemainModel.GetBuyingTotalCount(start, end);
            var buying_price = RemainModel.GetBuyingTotalPrice(start, end);
            var buying_back_count = RemainModel.GetBuyingBackTotalCount(start, end);
            var buying_back_price = RemainModel.GetBuyingBackTotalPrice(start, end);

            var sell_count = RemainModel.GetSellTotalCount(start, end);
            var sell_price = RemainModel.GetSellTotalPrice(start, end);
            var sell_back_count = RemainModel.GetSellBackTotalCount(start, end);
            var sell_back_price = RemainModel.GetSellBackTotalPrice(start, end);

            var moving_out_count = StoreModel.GetTotalCountIntoMain(start, end);
            var moving_in_count = StoreModel.GetTotalCountOutfromMain(start, end);

            var spending_loss_count = StoreModel.GetUsingLossTotalCount(start, end);
            var spending_loss_price = StoreModel.GetUsingLossTotalPrice(start, end);
            var spending_more_count = TicketModel.GetReceivingMoreTotalCount(start, end);
            var spending_more_price = TicketModel.GetReceivingMoreTotalPrice(start, end);

            var remain_count = RemainModel.GetRemainTotalCount(start, end);
            var remain_price = RemainModel.GetRemainTotalPrice(start, end);

            var paying_in = MoneyModel.GetShoyruTotalPrice(start, end);
            var paying_out = MoneyModel.GetZhichuTotalPrice(start, end);

            var money_sell = sell_price;
            var money_buying = RemainModel.GetSellOriginTotalPrice(start, end);
            var money_smallchange = MoneyModel.GetSmallchangeTotalPrice(start, end);

            return Json(new { buying_count = buying_count, buying_price = buying_price, buying_back_count = buying_back_count, buying_back_price = buying_back_price, 
                sell_count = sell_count, sell_price = sell_price, sell_back_count = sell_back_count, sell_back_price = sell_back_price, 
                moving_out_count = moving_out_count, moving_in_count = moving_in_count, 
                spending_loss_count = spending_loss_count, spending_loss_price = spending_loss_price, spending_more_count = spending_more_count, spending_more_price = spending_more_price, 
                remain_count = remain_count, remain_price = remain_price, 
                paying_in = paying_in, paying_out = paying_out,
                money_sell = money_sell, money_buying = money_buying, money_smallchange = money_smallchange
            }, JsonRequestBehavior.AllowGet);
        }

#region Credit Part
        [HttpPost]
        [Authorize(Roles = "admin,account")]
        public JsonResult SubmitRealPay(string id,string date, string cName, string cPhone, string type, string paytype, string price, string smallchange, string reason)
        {
            MoneyModel sModel = new MoneyModel();
            bool rst = true;
            long bId = 0;
            if (id.CompareTo("") != 0)
            {
                bId = Convert.ToInt64(id);
            }
            DateTime bDate = DateTime.Now;
            byte bType = Convert.ToByte(type);
            byte bPaytype = Convert.ToByte(paytype);
            
            
            if(date.CompareTo("") != 0)
            {
               bDate = Convert.ToDateTime(date);
            }

            long userid = CommonModel.GetCurrentUserId();
            long shopid = CommonModel.GetCurrentUserShopId();

            rst = sModel.SubmitRealpay(bId,bDate, cName, cPhone, bType,bPaytype, Convert.ToDecimal(price), Convert.ToDecimal(smallchange), reason, userid,shopid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,account")]
        public JsonResult DeleteCredit(string id)
        {
            long bid = 0;
            if (id.CompareTo("") != 0)
            {
                bid = Convert.ToInt64(id);
            }
            if (bid > 0)
            {
                MoneyModel sModel = new MoneyModel();
                var rst = sModel.DeleteCredit(bid);
                var ret = Json(rst, JsonRequestBehavior.AllowGet);
                return ret;
            }
            return null;
        }

        [Authorize(Roles = "admin,account")]
        public JsonResult RetrieveCredit(JQueryDataTableParamModel param)
        {
            MoneyModel mModel = new MoneyModel();
            JqDataTableInfo rst = mModel.GetCreditTable(param, Request.QueryString, f_start_date, f_end_date, c_customer_name, c_type);
            return Json(rst, JsonRequestBehavior.AllowGet);            
        }

        [Authorize(Roles = "admin,account")]
        public JsonResult RetrieveDetailCredit(JQueryDataTableParamModel param)
        {
            MoneyModel mModel = new MoneyModel();
            JqDataTableInfo rst = mModel.GetDetailCreditTable(param, Request.QueryString, credit_id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,account")]
        public JsonResult RetrieveAddHistory(JQueryDataTableParamModel param)
        {
            MoneyModel mModel = new MoneyModel();
            JqDataTableInfo rst = mModel.GetAddHistoryTable(param, Request.QueryString, a_start_date, a_end_date, a_customer_name, a_type);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,account")]
        public string changeDetailId(string id)
        {
            credit_id = Convert.ToInt64(id);
            return "";
        }
        [Authorize(Roles = "admin,account")]
        public string FilterCredit(DateTime start_date, DateTime end_date, string customer_name, string paytype)
        {
            c_customer_name = customer_name;
            c_type = Convert.ToByte(paytype);
            f_start_date = start_date;
            f_end_date = end_date;
            //RetrieveSalesman(m_queryParam);
            return "";
        }
        public string CleanFilterCredit()
        {
            c_customer_name = "";
            c_type = 2;
            f_start_date = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            f_end_date = DateTime.Now;
            return "";
        }
        public string FilterAddHistory(DateTime start_date, DateTime end_date, string customer_name, string paytype)
        {
            a_customer_name = customer_name;
            a_type = Convert.ToByte(paytype);
            a_start_date = start_date;
            a_end_date = end_date;
            //RetrieveSalesman(m_queryParam);
            return "";
        }
#endregion
#region Statistics Part
        [Authorize(Roles = "admin,account")]
        public JsonResult RetrieveStatistics(JQueryDataTableParamModel param)
        {
            MoneyModel mModel = new MoneyModel();
            JqDataTableInfo rst = mModel.GetStatisticsTable(param, Request.QueryString, m_stat_searchType, f_start_date, f_end_date);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        //RetrieveEditStatistics
        [Authorize(Roles = "admin,account")]
        public JsonResult RetrieveEditStatistics(String id)
        {
            long stat_id = Convert.ToInt64(id);
            if (stat_id > 0)
            {
                MoneyModel sModel = new MoneyModel();
                var rst = sModel.GetOneStatistics(stat_id);
                var ret = Json(new { statInfo = rst }, JsonRequestBehavior.AllowGet);
                return ret;
            }
            return null;
        }

        [Authorize(Roles = "admin,account")]
        public JsonResult DeleteStatistics(String id)
        {
            long stat_id = Convert.ToInt64(id);
            if (stat_id > 0)
            {
                MoneyModel sModel = new MoneyModel();
                var rst = sModel.DeleteStatistics(stat_id);
                var ret = Json(rst, JsonRequestBehavior.AllowGet);
                return ret;
            }
            return null;
        }

        [HttpPost]
        [Authorize(Roles = "admin,account")]
        public JsonResult SubmitStatistics(string stat_id,string stat_type, string stat_paytype, string statprice, string statreason)
        {
            MoneyModel sModel = new MoneyModel();
            bool rst = true;
            long bstat_id = 0;
            if (stat_id.CompareTo("") != 0)
            {
                bstat_id = Convert.ToInt64(stat_id);
            }
            DateTime bDate = DateTime.Now;
            byte bType = Convert.ToByte(stat_type); 
            byte bPayType = Convert.ToByte(stat_paytype);
            decimal bPrice = Convert.ToDecimal(statprice);
            
            long userid = CommonModel.GetCurrentUserId();
            long shopid = CommonModel.GetCurrentUserShopId();
            rst = sModel.SubmitStatistics(bstat_id, bType, bPayType, bPrice, statreason, userid, shopid);

            if (rst)
            {
                EarnModel earnModel = new EarnModel();
                if (bstat_id == 0)
                {
                    long otherpay_id = sModel.GetLastInsertedId();
                    earnModel.InsertOtherpayEarn(otherpay_id, bPrice, bType, bPayType, (bType == 1) ? "其他收入" : "其他费用");
                }
                else
                {
                    earnModel.UpdateOtherpayEarn(bstat_id, bPrice, bType, bPayType, (bType == 1) ? "其他收入" : "其他费用");
                }
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,account")]
        public JsonResult FilterStat(string search_stat_type, string start_date, string end_date)
        {
            m_stat_searchType = Convert.ToByte(search_stat_type);
            f_start_date = Convert.ToDateTime(start_date);
            f_end_date = Convert.ToDateTime(end_date);

            decimal shouru = MoneyModel.GetTotalShouZhi(1, f_start_date, f_end_date);
            decimal zhichu = MoneyModel.GetTotalShouZhi(0, f_start_date, f_end_date);

            return Json(new { shouru = shouru, zhichu = zhichu }, JsonRequestBehavior.AllowGet);
        }
        
#endregion
#region Bank Part
        [Authorize(Roles = "admin,account")]
        public JsonResult RetrieveMoneyBank(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            DateTime startdate = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" + Convert.ToString(DateTime.Now.Day));
            try { startdate = DateTime.Parse(Request.QueryString["startdate"]); }
            catch (Exception e) { }

            DateTime enddate = DateTime.Now;
            try { enddate = DateTime.Parse(Request.QueryString["enddate"]); }
            catch (Exception e) { }

            EarnModel eModel = new EarnModel();
            JqDataTableInfo rst = eModel.GetMoneyBankDataTable(param, Request.QueryString, rootUri, startdate, enddate);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,account")]
        public JsonResult RetrieveMoneyBankDetail(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            byte paytype = 4;
            try { paytype = byte.Parse(Request.QueryString["paytype"]); }
            catch (Exception e) { }

            byte moneytype = 2;
            try { moneytype = byte.Parse(Request.QueryString["moneytype"]); }
            catch (Exception e) { }

            DateTime detaildate = DateTime.Now;
            try { detaildate = DateTime.Parse(Request.QueryString["detaildate"]); }
            catch (Exception e) { }

            EarnModel eModel = new EarnModel();
            JqDataTableInfo rst = eModel.GetMoneyBankDetailDataTable(param, Request.QueryString, rootUri, detaildate, paytype, moneytype);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,account")]
        public string FilterBank(string start_date, string end_date)
        {
            m_start_date = Convert.ToDateTime(start_date);
            m_end_date = Convert.ToDateTime(end_date);

            return "";
        }
#endregion
#region Finance Part

        [Authorize(Roles = "admin,account")]
        public JsonResult RetrieveFinance(JQueryDataTableParamModel param)
        {
            MoneyModel mModel = new MoneyModel();
            JqDataTableInfo rst = mModel.GetFinanceTable(param, Request.QueryString, f_start_date, f_end_date);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,account")]
        public JsonResult RetrieveFinanceDetail(JQueryDataTableParamModel param)
        {
            DateTime detaildate = DateTime.Now;
            try { if (Request.QueryString["detaildate"] != null) detaildate = Convert.ToDateTime(Request.QueryString["detaildate"]); }
            catch(Exception e) { }

            MoneyModel mModel = new MoneyModel();
            JqDataTableInfo rst = mModel.GetFinanceDetailTable(param, Request.QueryString, detaildate);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,account")]
        public JsonResult financeTotal(JQueryDataTableParamModel param)
        {
            MoneyModel mModel = new MoneyModel();
            string rst = mModel.GetFinanceTotal(param, Request.QueryString, f_start_date, f_end_date);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,account")]
        public string filterFinance(string start_date,string end_date)
        {
            f_start_date = Convert.ToDateTime(start_date);
            f_end_date = Convert.ToDateTime(end_date);
            //RetrieveSalesman(m_queryParam);
            return "";
        }
#endregion
    }
}
