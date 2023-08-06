using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NongYaoFrontEnd.Models;
using NongYaoFrontEnd.Models.Library;


namespace NongYaoFrontEnd.Controllers
{
    public class SupplyController : Controller
    {
        //
        // GET: /Supply/
        [Authorize(Roles = "admin,buying")]
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

        [Authorize(Roles = "admin,buying")]
        public ActionResult Supply()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            long shop_id = CommonModel.GetCurrentUserShopId();
            ViewData["ticketnum"] = TicketModel.MakeCurrentTicketNumber(0);
            ViewData["supplys"] = SupplyModel.GetSupplyList(shop_id);
            ViewData["username"] = CommonModel.GetCurrentUserName();
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            ViewData["curdateymd"] = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewData["stores"] = StoreModel.GetStoreList(shop_id);

            var catalogs = CatalogModel.GetCatalogList(shop_id);
            ViewData["catalogs"] = catalogs;
            ViewData["units"] = UnitModel.GetUnitListFull();

            if(catalogs.Count() > 0) {
                tbl_catalog catalog0 = catalogs.ElementAt(0);
                ViewData["register_id0"] = catalog0.register_id;
                ViewData["product_area0"] = catalog0.product_area;
                ViewData["avail_date0"] = catalog0.avail_date;
            }

            return View();
        }

        [Authorize(Roles = "admin,buying")]
        public ActionResult Back()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            long shop_id = CommonModel.GetCurrentUserShopId();
            ViewData["ticketnum"] = TicketModel.MakeCurrentTicketNumber(1);
            ViewData["supplys"] = SupplyModel.GetSupplyList(shop_id);
            ViewData["username"] = CommonModel.GetCurrentUserName();
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            ViewData["curdateymd"] = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            List<tbl_store> stores = StoreModel.GetStoreList(shop_id);
            ViewData["stores"] = stores;

            var catalogs = CatalogModel.GetCatalogList(shop_id);
            ViewData["catalogs"] = catalogs;
            ViewData["units"] = UnitModel.GetUnitListFull();

            if (catalogs.Count() > 0 && stores.Count() > 0)
            {
                tbl_catalog catalog0 = catalogs.ElementAt(0);
                tbl_store store0 = stores.ElementAt(0);
                var largenumbers0 = RemainModel.GetRemainCatalogLargenumberList(shop_id, store0.id, catalog0.id);
                ViewData["largenumbers0"] = largenumbers0;
                ViewData["register_id0"] = catalog0.register_id;
                if (largenumbers0.Count() > 0)
                    ViewData["standards0"] = RemainModel.GetRemainCatalogLargenumberStandardList(shop_id, store0.id, catalog0.id, largenumbers0.ElementAt(0));
            }

            return View();
        }
        
        [HttpPost]
        [AjaxOnly]
        [Authorize(Roles = "admin,buying")]
        public JsonResult SubmitTicket(string ticketnum, long supply, byte paytype, long store, byte type,
            long[] catalog_id, decimal[] standard, int[] mass, long[] unit_id, long[] standard_id,
            string[] largenumber, string[] product_date, decimal[] price, int[] count)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            TicketModel aModel = new TicketModel();
            RemainModel rModel = new RemainModel();
            EarnModel eModel = new EarnModel();
            MoneyModel mModel = new MoneyModel();
            string rst = "";
            
            int cnt = catalog_id.Length;

            int avail_date = 0;
            CatalogModel cModel = new CatalogModel();

            int check_buying = 0;
            

            //long lsupply = 0;
            //try { lsupply = long.Parse(supply); }
            //catch (System.Exception ex) { }

            //byte bpaytype = 0;
            //try { bpaytype = byte.Parse(paytype); }
            //catch (System.Exception ex) { }

            //long lstore = 0;
            //try { lstore = long.Parse(store); }
            //catch (System.Exception ex) { }

            //byte btype = 0;
            //try { btype = byte.Parse(type); }
            //catch (System.Exception ex) { }

            //long[] lstore_id = new long[cnt];
            //long[] lcatalog_id = new long[cnt];
            //decimal[] dprice = new decimal[cnt];
            //int[] icount = new int[cnt];

            if(type == 0)
                standard_id = new long[cnt];
            for (int i = 0; i < cnt; i++)
            {
                //lcatalog_id[i] = 0;
                //try { lcatalog_id[i] = long.Parse(catalog_id[i]); }
                //catch (System.Exception ex) { }

                //dprice[i] = 0;
                //try { dprice[i] = decimal.Parse(price[i]); }
                //catch (System.Exception ex) { }

                //icount[i] = 0;
                //try { icount[i] = int.Parse(count[i]); }
                //catch (System.Exception ex) { }

                if(type == 0)
                standard_id[i] = StandardModel.GetOrInsertStandardWith(standard[i], (byte)mass[i], unit_id[i]);

                if(type == 1 && !rModel.CheckRemain(catalog_id[i], store, count[i], standard_id[i], largenumber[i])) {
                    rst = REMAIN_SUBMITSTATUS.REMAIN_INSUFFICIENT;
                    break;
                }
                else if (type == 0 && !rModel.CheckDuplicateLargenumber(catalog_id[i], store, standard_id[i], largenumber[i], product_date[i]))
                {
                    rst = "第" + (i + 1) + "产品 - 批号:" + largenumber[i] + ",生产日期:" + product_date[i] + ", " + REMAIN_SUBMITSTATUS.DUPLICATE_LARGENUMBER;
                    break;
                }

                avail_date = 0;
                if (type == 0) 
                {
                    avail_date = cModel.GetCatalogAvailDateForId(catalog_id[i]);
                    DateTime dproduct_date = Convert.ToDateTime(product_date[i]);
                    if (DateTime.Compare(dproduct_date.AddDays(avail_date * 30), DateTime.Now) < 0)
                    {
                        rst = "第" + (i + 1) + "产品 - 批号:" + largenumber[i] + ",生产日期:" + product_date[i] + ", " + REMAIN_SUBMITSTATUS.OVER_AVAILDATE;
                        break;
                    }
                }
                if (type == 1)
                {
                    check_buying = aModel.CheckBuying(supply, catalog_id[i], standard_id[i], largenumber[i], store, count[i]);
                    if (check_buying == 0)
                    {
                        rst = "第" + (i + 1) + "产品 - 批号:" + largenumber[i] + ", " + REMAIN_SUBMITSTATUS.NO_BUYING_CATALOG;
                        break;
                    }
                    
                }
            }

            if (rst == "")
            {
                decimal totalPrice = 0;
                for (int i = 0; i < cnt; i++)
                    totalPrice += price[i] * count[i];

                rst = aModel.InsertBuyTicket(ticketnum, supply, paytype, store, totalPrice, totalPrice, type);
                if (rst == "")
                {
                    tbl_ticket ticket = aModel.GetTicketInfo(ticketnum);
                    for (int i = 0; i < cnt; i++)
                    {
                        rst = aModel.InsertBuying(ticket.id, catalog_id[i], standard_id[i], largenumber[i],
                            (product_date != null ? product_date[i] : null), price[i], count[i], store);
                        if (rst == "")
                        {
                            DateTime oproduct_date = DateTime.MinValue;
                            decimal oprice = 0;
                            if (type == 0)
                            {
                                oproduct_date = Convert.ToDateTime(product_date[i]);
                                oprice = price[i];                                
                            }
                            else
                            {
                                long shop_id = CommonModel.GetCurrentUserShopId();
                                var oitem = RemainModel.GetRemainCatalogOrigin(shop_id, store, catalog_id[i], standard_id[i], largenumber[i]);
                                if (oitem != null)
                                {
                                    oproduct_date = (DateTime)oitem.product_date;
                                    oprice = (long)oitem.origin_price;
                                }
                            }

                            rst = rModel.InsertRemain(catalog_id[i], standard_id[i], largenumber[i], store, price[i], count[i], supply, oproduct_date, oprice, type);
                        }
                    }
                    if (paytype == 0 || paytype == 2)
                        eModel.InsertEarn(ticket.id, totalPrice, (byte)(type == 0 ? 0 : 1), paytype, (type == 0 ? "采购进货" : "采购退货"));
                    if (paytype == 1)
                        mModel.InsertCredit(supply, (byte)(type == 0 ? 0 : 1), totalPrice, type, ticket.id, 0);                   

                }
                else if (rst == TicketModel.TICKET_SUBMITSTATUS.DUPLICATE_NAME)
                {
                    string newTicketnum = TicketModel.MakeCurrentTicketNumber(type);
                    return Json(new { error = rst, ticketnum = newTicketnum}, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }  

        [Authorize(Roles = "admin,buying")]
        public JsonResult GetCurrentTicketNumber(byte type)
        {
            string ticketnum = TicketModel.MakeCurrentTicketNumber(type);

            return Json(ticketnum, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxOnly]
        [Authorize(Roles = "admin,buying")]
        public JsonResult GetCatalogsInStore(string store)
        {
            long lstore = 0;
            try { lstore = long.Parse(store); }
            catch (System.Exception ex) { }

            long shop_id = CommonModel.GetCurrentUserShopId();
            List<tbl_catalog> retlist = RemainModel.GetRemainCatalogList(shop_id, lstore);

            return Json(retlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxOnly]
        [Authorize(Roles = "admin,buying")]
        public JsonResult GetCatalogsInStore_1(string store, int num)
        {
            long lstore = 0;
            try { lstore = long.Parse(store); }
            catch (System.Exception ex) { }

            long shop_id = CommonModel.GetCurrentUserShopId();
            List<tbl_catalog> retlist = RemainModel.GetRemainCatalogList(shop_id, lstore);

            return Json(new { num = num, data = retlist }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Authorize(Roles = "admin,buying")]
        public JsonResult GetCatalogInfo(long id, int num)
        {
            var rst = CatalogModel.GetCatalogInfo(id);

            return Json(new { num = num, data = rst }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Authorize(Roles = "admin,buying")]
        public JsonResult SearchCatalogList(string search, int num)
        {
            var rst = CatalogModel.SearchCatalogList(search);

            return Json(new { num = num, data = rst }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Authorize(Roles = "admin,buying")]
        public JsonResult GetCatalogInfoAndLargenumberList(long store_id, long catalog_id, int num)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var info = CatalogModel.GetCatalogInfo(catalog_id);            
            var rst = RemainModel.GetRemainCatalogLargenumberList(shop_id, store_id, catalog_id);

            return Json(new { num = num, info = info, data = rst }, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Authorize(Roles = "admin,buying")]
        public JsonResult GetStandardList(long store_id, long catalog_id, string largenumber, int num)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var rst = RemainModel.GetRemainCatalogLargenumberStandardList(shop_id, store_id, catalog_id, largenumber);

            return Json(new { num = num, data = rst }, JsonRequestBehavior.AllowGet);
        }
    }
}
