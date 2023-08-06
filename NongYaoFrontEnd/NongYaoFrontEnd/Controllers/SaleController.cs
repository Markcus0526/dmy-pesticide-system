using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NongYaoFrontEnd.Models;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Controllers
{
    public class SaleController : Controller
    {
        //
        // GET: /Sale/
        [Authorize(Roles = "admin,sale")]
        public ActionResult Index()
        {
            string tabnum = Request.QueryString["tabnum"];
            if (tabnum == null || tabnum.Length == 0)
                tabnum = "1";
            ViewData["tabnum"] = tabnum;

            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["userrole"] = CommonModel.GetCurrentUserRole();
            ViewData["userName"] = CommonModel.GetCurrentUserName();

            return View();
        }

        [Authorize(Roles = "admin,sale")]
        public ActionResult Sale()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            long shop_id = CommonModel.GetCurrentUserShopId();
            ViewData["ticketnum"] = TicketModel.MakeCurrentTicketNumber(2);
            ViewData["shoppername"] = ShopModel.GetShopNameForId(shop_id);
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            ViewData["username"] = CommonModel.GetCurrentUserName();
            ViewData["othershoppers"] = ShopModel.GetShopListExcept(shop_id);
            List<tbl_store> stores = StoreModel.GetStoreList(shop_id);
            ViewData["stores"] = stores;

            if (stores.Count() > 0)
            {
                var catalogs = RemainModel.GetRemainCatalogList(shop_id, stores.ElementAt(0).id);
                ViewData["catalogs"] = catalogs;

                if (catalogs.Count() > 0)
                {
                    tbl_catalog catalog0 = catalogs.ElementAt(0);
                    tbl_store store0 = stores.ElementAt(0);
                    ViewData["catalog_num0"] = catalog0.catalog_num;
                    ViewData["product_area0"] = catalog0.product_area;
                    ViewData["avail_date0"] = catalog0.avail_date;
                    var standards0 = RemainModel.GetRemainCatalogStandardList(shop_id, store0.id, catalog0.id);
                    ViewData["standards0"] = standards0;
                    if (standards0.Count() > 0)
                    {
                        var largenumbers0 = RemainModel.GetRemainCatalogStandardLargenumberList(shop_id, store0.id, catalog0.id, standards0.ElementAt(0).id);
                        ViewData["largenumbers0"] = largenumbers0;
                        if (largenumbers0.Count() > 0)
                        {
                            var remain0 = RemainModel.GetRemainCatalogInStore(shop_id, store0.id, catalog0.id, standards0.ElementAt(0).id, largenumbers0.ElementAt(0));
                            ViewData["remain0"] = remain0.quantity;
                            ViewData["product_date0"] = String.Format("{0:yyyy-MM-dd}", remain0.product_date);
                        }
                    }
                }
            }

            return View();
        }

        [Authorize(Roles = "admin,sale")]
        public ActionResult Back()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            long shop_id = CommonModel.GetCurrentUserShopId();
            ViewData["ticketnum"] = TicketModel.MakeCurrentTicketNumber(3);
            ViewData["shoppername"] = ShopModel.GetShopNameForId(shop_id);
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            ViewData["username"] = CommonModel.GetCurrentUserName();
            ViewData["othershoppers"] = ShopModel.GetSaleShopList(shop_id);
            var stores = StoreModel.GetStoreList(shop_id);
            ViewData["stores"] = stores;

            var catalogs = CatalogModel.GetCatalogList(shop_id);
            ViewData["catalogs"] = catalogs;

            if (catalogs.Count() > 0 && stores.Count() > 0)
            {
                tbl_catalog catalog0 = catalogs.ElementAt(0);
                tbl_store store0 = stores.ElementAt(0);
                ViewData["catalog_num0"] = catalog0.catalog_num;
                ViewData["product_area0"] = catalog0.product_area;
                ViewData["avail_date0"] = catalog0.avail_date;
                var standards0 = RemainModel.GetRemainCatalogStandardList(shop_id, store0.id, catalog0.id);
                ViewData["standards0"] = standards0;
                if (standards0.Count() > 0)
                {
                    var largenumbers0 = RemainModel.GetRemainCatalogStandardLargenumberList(shop_id, store0.id, catalog0.id, standards0.ElementAt(0).id);
                    ViewData["largenumbers0"] = largenumbers0;
                    if (largenumbers0.Count() > 0)
                    {
                        var remain0 = RemainModel.GetRemainCatalogInStore(shop_id, store0.id, catalog0.id, standards0.ElementAt(0).id, largenumbers0.ElementAt(0));
                        ViewData["remain0"] = remain0.quantity;
                        ViewData["product_date0"] = String.Format("{0:yyyy-MM-dd}", remain0.product_date);
                    }
                }
            }

            return View();
        }

        [HttpPost]
        [AjaxOnly]
        [Authorize(Roles = "admin,sale")]
        public JsonResult SubmitTicket(string ticketnum, byte customer_type, string customer_name, string customer_phone, long? othershop, byte paytype, long store,
            byte type, decimal sellmoney, decimal sellchange,
            long[] catalog_id, long[] standard_id, string[] largenumber, decimal[] price, int[] count)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            TicketModel aModel = new TicketModel();
            RemainModel rModel = new RemainModel();
            EarnModel eModel = new EarnModel();
            MoneyModel mmModel = new MoneyModel();
            string rst = "";
            int check_sale = 0;

            int cnt = catalog_id.Length;

            /////////////////////////////////////////////
            if (othershop == null) othershop = 0;
            /////////////////////////////////////////////

            //byte bcustomer_type = 0;
            //try { bcustomer_type = byte.Parse(customer_type); }
            //catch (System.Exception ex) { }

            //long lothershop = 0;
            //try { lothershop = long.Parse(othershop); }
            //catch (System.Exception ex) { }

            //byte bpaytype = 0;
            //try { bpaytype = byte.Parse(paytype); }
            //catch (System.Exception ex) { }

            //byte btype = 0;
            //try { btype = byte.Parse(type); }
            //catch (System.Exception ex) { }

            long customer_id = 0;
            if (customer_type == 0)
                customer_id = CustomerModel.GetUserId(customer_name, customer_phone);

            //byte bselltype = 0;
            //try { bselltype = byte.Parse(selltype); }
            //catch (System.Exception ex) { }

            //decimal dsellchange = 0;
            //try { dsellchange = decimal.Parse(sellchange); }
            //catch (System.Exception ex) { }

            //long[] lstore_id = new long[catalog_name.Length];
            //long[] lcatalog_id = new long[catalog_name.Length];
            //decimal[] dprice = new decimal[catalog_name.Length];
            //int[] icount = new int[catalog_name.Length];
            for (int i = 0; i < cnt; i++)
            {
            //    lstore_id[i] = 0;
            //    try { lstore_id[i] = long.Parse(store[i]); }
            //    catch (System.Exception ex) { }

            //    lcatalog_id[i] = 0;
            //    try { lcatalog_id[i] = long.Parse(catalog_name[i]); }
            //    catch (System.Exception ex) { }

            //    dprice[i] = 0;
            //    try { dprice[i] = decimal.Parse(price[i]); }
            //    catch (System.Exception ex) { }

            //    icount[i] = 0;
            //    try { icount[i] = int.Parse(count[i]); }
            //    catch (System.Exception ex) { }

                if (type == 2 && !rModel.CheckRemain(catalog_id[i], store, count[i], standard_id[i], largenumber[i]))
                {
                    rst = REMAIN_SUBMITSTATUS.REMAIN_INSUFFICIENT;
                    break;
                }

                if (type == 3)
                {
                    long customer = 0;
                    if (customer_type == 1)
                        customer = (long)othershop;
                    else
                        customer = customer_id;
                    check_sale = aModel.CheckSale(customer_type, customer, catalog_id[i], standard_id[i], largenumber[i], count[i]);
                    if (check_sale == 0)
                    {
                        rst = REMAIN_SUBMITSTATUS.NO_SALE_CATALOG;
                        break;
                    }
                    else if (check_sale == 2)
                    {
                        rst = REMAIN_SUBMITSTATUS.ERROR_SALECNT;
                        break;
                    }
                }
            }



            if (rst == "")
            {
                decimal totalPrice = 0;
                for (int i = 0; i < cnt; i++)
                    totalPrice += price[i] * count[i];

                if (type == 3)
                {
                    if (paytype == 0 || paytype == 2)
                        sellmoney = totalPrice;
                }

                rst = aModel.InsertSellTicket(ticketnum, customer_type, customer_id, (long)othershop, paytype, store, totalPrice, sellmoney, type);
                if (rst == "")
                {
                    tbl_ticket ticket = aModel.GetTicketInfo(ticketnum);
                    for (int i = 0; i < cnt; i++)
                    {
                        rst = aModel.InsertSale(ticket.id, store, catalog_id[i], standard_id[i], largenumber[i], 
                            price[i], count[i]);
                        if (rst == "")
                        {
                            DateTime oproduct_date = DateTime.MinValue;
                            decimal oprice = 0;
                            long osupply = 0;
                            long shop_id = CommonModel.GetCurrentUserShopId();
                            var oitem = RemainModel.GetRemainCatalogOrigin(shop_id, store, catalog_id[i], standard_id[i], largenumber[i]);
                            if (oitem != null)
                            {
                                oproduct_date = (DateTime)oitem.product_date;
                                oprice = (long)oitem.origin_price;
                                osupply = (long)oitem.supply_id;
                            }

                            rst = rModel.InsertRemain(catalog_id[i], standard_id[i], largenumber[i], store, 
                                price[i], count[i], osupply, oproduct_date,oprice, type);
                            //if (type == 3)
                            {
                                var shopInfo = ShopModel.GetShopInfo(CommonModel.GetCurrentUserShopId());
                                if (shopInfo != null)
                                    rst = aModel.InsertSalestatistics(shopInfo.region, catalog_id[i], CatalogModel.GetNongyaoIdOfCatalog(catalog_id[i]), (type == 2 ? count[i] : -count[i]), standard_id[i], largenumber[i]);
                            }
                        }
                    }
                    
                    if (paytype == 0 || paytype == 2)
                    {
                        if (sellmoney > 0)
                            eModel.InsertEarn(ticket.id, sellmoney, (byte)(type == 2 ? 1 : 0), paytype, (type == 2 ? "销售开单" : "销售退货"));
                        if (sellchange > 0)
                        {
                            mmModel.InsertChange(sellchange);
                        }
                         decimal payment = totalPrice - sellchange - sellmoney;
                         if (payment > 0)
                               mmModel.InsertCredit((customer_type == 1 ? (long)othershop : customer_id), (byte)(type == 2 ? 1 : 0), payment, (byte)(type == 2 ? 2 : 3), ticket.id, customer_type);
                        
                    }

                  /*  if (type == 2 && sellmoney == 1 && sellchange != 0)
                    {
                        MoneyModel mModel = new MoneyModel();
                        mModel.SubmitStatistics(0, (byte)(sellchange >= 0 ? 1 : 0), paytype, (decimal)(sellchange >= 0 ? sellchange : -sellchange),
                            "销售的找零", CommonModel.GetCurrentUserId(), CommonModel.GetCurrentUserShopId());                    
                    }*/
                    if (paytype == 1)
                    {
                        mmModel.InsertCredit((customer_type == 1 ? (long)othershop : customer_id), (byte)(type == 2 ? 1 : 0), totalPrice, (byte)(type == 2 ? 2 : 3), ticket.id, customer_type);
                    }
                    

                }
                else if (rst == TicketModel.TICKET_SUBMITSTATUS.DUPLICATE_NAME)
                {
                    string newTicketnum = TicketModel.MakeCurrentTicketNumber(type);
                    return Json(new { error = rst, ticketnum = newTicketnum }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxOnly]
        [Authorize(Roles = "admin,sale")]
        public JsonResult GetUsernameOrPhone(string search, string isphone)
        {
            string rst;

            if (isphone == "false")
                rst = CustomerModel.GetPhoneForUsername(search);
            else
                rst = CustomerModel.GetUsernameForPhone(search);

            return Json(new { isphone = isphone, result = rst }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxOnly]
        [Authorize(Roles = "admin,sale")]
        public JsonResult SubmitUser(string customer_name, string customer_phone, string isinsert)
        {
            string rst = "";
            CustomerModel aModel = new CustomerModel();
            if (isinsert == "1")
            {
                rst = aModel.InsertCustomer(customer_name, customer_phone);
            }
            else
            {
                long uid = CustomerModel.GetUserId(customer_name, customer_phone);
                if (uid == 0)
                    rst = "no";
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //[AjaxOnly]
        //[Authorize(Roles = "admin,sale")]
        //public JsonResult GetCatalogsInStore(string store, string num)
        //{
        //    long lstore = 0;
        //    try { lstore = long.Parse(store); }
        //    catch (System.Exception ex) { }

        //    long shop_id = CommonModel.GetCurrentUserShopId();
        //    List<tbl_catalog> cataloglist = RemainModel.GetRemainCatalogList(shop_id, lstore);
        //    /*string standard0 = "";
        //    string remain0 = "";
        //    if (retlist.Count > 0)
        //    {
        //        tbl_catalog firstitem = retlist.ElementAt(0);
        //        standard0 = Convert.ToString(firstitem.standard) + UnitModel.GetUnitInfo((long)firstitem.unit).unit;
        //        remain0 = Convert.ToString(RemainModel.GetRemainCatalogInStore(lstore, firstitem.id));
        //    }*/
        //    var retlist = cataloglist.Select(m => new
        //    {
        //        id = m.id,
        //        name = m.name,
        //        catalog_num = m.catalog_num,
        //        standard = m.standard + UnitModel.GetUnitInfo((long)m.unit).unit,
        //        remain = RemainModel.GetRemainCatalogInStore(lstore, m.id)
        //    });

        //    return Json(new { num = num, data = retlist/*, standard0 = standard0, remain0 = remain0*/ }, JsonRequestBehavior.AllowGet);
        //}

        [AjaxOnly]
        [Authorize(Roles = "admin,sale")]
        public JsonResult GetCatalogInfoAndStandardList(long store_id, long catalog_id, int num)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var info = CatalogModel.GetCatalogInfo(catalog_id);
            var rst = RemainModel.GetRemainCatalogStandardList(shop_id, store_id, catalog_id);

            return Json(new { num = num, info = info, data = rst }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Authorize(Roles = "admin,sale")]
        public JsonResult GetLargenumberList(long store_id, long catalog_id, long standard_id, int num)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var rst = RemainModel.GetRemainCatalogStandardLargenumberList(shop_id, store_id, catalog_id, standard_id);

            return Json(new { num = num, data = rst }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Authorize(Roles = "admin,sale")]
        public JsonResult GetCatalogRemainInfo(long store_id, long catalog_id, long standard_id, string largenumber, int num)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var item = RemainModel.GetRemainCatalogInStore(shop_id, store_id, catalog_id, standard_id, largenumber);

            if (item != null)
            {
                var info = new RemainInfo
                {
                    id = item.id,
                    catalog_id = item.catalog_id,
                    standard_id = item.standard_id,
                    largenumber = item.largenumber,
                    quantity = item.quantity,
                    product_date = String.Format("{0:yyyy-MM-dd}", (DateTime)item.product_date),
                    origin_price = (decimal)item.origin_price
                };

                return Json(new { num = num, info = info }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { num = num, info = new RemainInfo() }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "admin,sale")]
        public ActionResult Salehistory()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

            return View();
        }

        [Authorize(Roles = "admin,sale")]
        public JsonResult RetrieveSalehistoryList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            byte type = 4;
            try { type = byte.Parse(Request.QueryString["type"]); }
            catch (Exception e) { }

            DateTime startdate = Convert.ToDateTime(Convert.ToString(DateTime.Now.Year) + "-" + Convert.ToString(DateTime.Now.Month - 1) + "-" +Convert.ToString(DateTime.Now.Day));
            try { startdate = DateTime.Parse(Request.QueryString["startdate"]); }
            catch (Exception e) { }

            DateTime enddate = DateTime.Now;
            try { enddate = DateTime.Parse(Request.QueryString["enddate"]); }
            catch (Exception e) { }

            string search = Request.QueryString["search"];
            if (search == null) search = "";

            TicketModel ticketModel = new TicketModel();
            JqDataTableInfo rst = ticketModel.GetTicketListDataTable(param, Request.QueryString, rootUri, type, startdate, enddate, search);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,sale")]
        public JsonResult RetrieveTicketSaleList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            long ticket_id = 0;
            try { ticket_id = long.Parse(Request.QueryString["ticketid"]); }
            catch (Exception e) { }

            TicketModel ticketModel = new TicketModel();
            JqDataTableInfo rst = ticketModel.GetTicketSaleListDataTable(param, Request.QueryString, rootUri, ticket_id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
