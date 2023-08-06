using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NongYaoFrontEnd.Models;
using NongYaoFrontEnd.Models.Library;
namespace NongYaoFrontEnd.Controllers
{
    public class StoreController : Controller
    {
        //
        // GET: /Store/
        public static long m_storId;
        public static string m_nongyaoName = "";

        [Authorize(Roles = "admin,store")]
        public ActionResult Index()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["userrole"] = CommonModel.GetCurrentUserRole();
            ViewData["userName"] = CommonModel.GetCurrentUserName();
            return View();
        }

        [Authorize(Roles = "admin,store")]
        public ActionResult Inventory()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            m_storId = 0;
            m_nongyaoName = "";
            ViewData["rootUri"] = rootUri;
            ViewData["StoreNameList"] = StoreModel.GetStoreList(CommonModel.GetCurrentUserShopId());

            ViewData["total_remain"] = RemainModel.GetTotalRemainCatalogCount();
            ViewData["total_price"] = RemainModel.GetTotalPriceCatalogCount();

            return View();
        }

        [Authorize(Roles = "admin,store")]
        public ActionResult Moving()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            long shop_id = CommonModel.GetCurrentUserShopId();
            
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
                    var standards0 = RemainModel.GetRemainCatalogStandardList(shop_id, store0.id, catalog0.id);
                    ViewData["standards0"] = standards0;
                    if (standards0.Count() > 0)
                    {
                        var largenumbers0 = RemainModel.GetRemainCatalogStandardLargenumberList(shop_id, store0.id, catalog0.id, standards0.ElementAt(0).id);
                        ViewData["largenumbers0"] = largenumbers0;
                    }
                }
            }
            
            return View();
        }

        [Authorize(Roles = "admin,store")]
        public ActionResult Spending()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            long shop_id = CommonModel.GetCurrentUserShopId();

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
                    var standards0 = RemainModel.GetRemainCatalogStandardList(shop_id, store0.id, catalog0.id);
                    ViewData["standards0"] = standards0;
                    if (standards0.Count() > 0)
                    {
                        var largenumbers0 = RemainModel.GetRemainCatalogStandardLargenumberList(shop_id, store0.id, catalog0.id, standards0.ElementAt(0).id);
                        ViewData["largenumbers0"] = largenumbers0;
                    }
                }
            }
            return View();
        }


        [HttpPost]
        [AjaxOnly]
        [Authorize(Roles = "admin,store")]
        public JsonResult SubmitStoreMove(string[] start_store, string[] catalog_id, string[] standard_id, string[] largenumber, string[] end_store, string[] count)
        {
            string rst = "";
            StoreModel sModel = new StoreModel();
            
            //rst = sModel.SaveStoreMove(nongyao_id, start_storeid, end_storeid, bCount);
            rst = sModel.SaveStoreMove(catalog_id, standard_id, largenumber, start_store, end_store, count);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxOnly]
        [Authorize(Roles = "admin,store")]
        public JsonResult SubmitSpendInStore(string[] start_store, string[] catalog_id, string[] standard_id, string[] largenumber, string[] spendreason, string[] count)
        {
            string rst = "";
            StoreModel sModel = new StoreModel();

            rst = sModel.SaveSpendInStore(catalog_id, standard_id, largenumber, start_store, count, spendreason);
             
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        [Authorize(Roles = "admin,store")]
        public JsonResult RetrieveAgrochemicalType()
        {
            StoreModel aModel = new StoreModel();
            List<tbl_nongyao> middelTypes = aModel.GetNongyaoAtParent(0);
            List<String> middle = aModel.GetNames(middelTypes);
            List<List<String>> last = aModel.GetNongyaoAtParentToString(middelTypes);
            return Json(new { middle = middle, last = last }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult RetrieveCatalogList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            JqDataTableInfo rst = CatalogModel.GetCatalogListDataTable(param, Request.QueryString, rootUri, m_storId, m_nongyaoName);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RetrievePrintCatalogList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            //JqDataTableInfo rst = CatalogModel.GetCatalogListDataTable(param, Request.QueryString, rootUri, m_storId, m_nongyaoName);
            //JqDataTableInfo rst = new JqDataTableInfo();

            long nongyao_id = CommonModel.GetNongYaoKindFromName(m_nongyaoName);
            var alllist = RemainModel.GetRemainCatalogInfoListFromStore(m_storId, nongyao_id);

            var result = from c in alllist
                         select new[] {
                             Convert.ToString(c.catalog_num),
                             Convert.ToString(c.name),
                             Convert.ToString(c.usingname),
                             Convert.ToString(c.standard),
                             Convert.ToString(c.unit),
                             Convert.ToString(c.supply),
                             Convert.ToString(c.productdate),
                             Convert.ToString(c.avail_date) + "个月",
                             Convert.ToString(c.largenumber),
                             Convert.ToString(c.price),
                             Convert.ToString(c.quantity),
                             Convert.ToString(c.price * c.quantity)
                         };

            return Json(new 
            {
                sEcho = param.sEcho,
                iTotalRecords = alllist.Count(),
                iTotalDisplayRecords = alllist.Count(),
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public JsonResult RefreshTotal_remain()
        {
            long nongyao_id = CommonModel.GetNongYaoKindFromName(m_nongyaoName);
            var rst = new RemainModel().RefreshTotalRemainCatalogCount(m_storId, nongyao_id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RefreshTotal_price()
        {
            long nongyao_id = CommonModel.GetNongYaoKindFromName(m_nongyaoName);
            var rst = new RemainModel().RefreshGetTotalPriceCatalogCount(m_storId, nongyao_id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "admin,store")]
        public JsonResult GetCatalogNames(long storeid)
        {
            /*
            var rst = CatalogModel.GetCatalogInfoList(storeid);
            var ret = Json(new { caData = rst }, JsonRequestBehavior.AllowGet);
            return ret;
             */
            return null;
        }

        [Authorize(Roles = "admin,store")]
        public string InventByStore(string storeId,string nongyaoName)
        {
            long id = 0;
            try
            {
                id = long.Parse(storeId);
            }
            catch (System.Exception ex)
            {
            }
            m_storId = id;
            if (nongyaoName.CompareTo("农药类别") == 0)
                m_nongyaoName = "";
            else
                m_nongyaoName = nongyaoName;
            return "";
        }

        [Authorize(Roles = "admin,store")]
        public ActionResult ManageAvail()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            long shop_id = CommonModel.GetCurrentUserShopId();
            ViewData["stores"] = StoreModel.GetStoreList(shop_id);
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

            return View();
        }

        [Authorize(Roles = "admin,store")]
        public JsonResult RetrieveManageAvail(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            long store_id = 0;
            try { store_id = long.Parse(Request.QueryString["store"]); }
            catch (Exception e) { }

            byte status = 2;
            try { status = byte.Parse(Request.QueryString["status"]); }
            catch (Exception e) { }

            int remaindays = 7;
            try { remaindays = int.Parse(Request.QueryString["remaindays"]); }
            catch (Exception e) { }

            RemainModel remainModel = new RemainModel();
            JqDataTableInfo rst = remainModel.GetManageAvailListDataTable(param, Request.QueryString, rootUri, store_id, status, remaindays);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

    }
}
