using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NongYaoBackend.Models;
using NongYaoBackend.Models.Library;

namespace NongYaoBackend.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Area/
        static long m_country = 0;
        static string m_searchWord = "";
        static byte m_searchpass = 3;

        public ActionResult ConsiderProduct()
        {
            if (User.Identity.Name.Length > 5)
            {
                ViewBag.username = User.Identity.Name.Substring(0, 5) + "...";
            }
            else
            {
                ViewBag.username = User.Identity.Name;
            }
            ViewBag.Message = "ConsiderProduct";
            ViewBag.role = CommonModel.GetCurrentUserRole();
            if (ViewBag.role == 1)
            {
                long cur_id = CommonModel.GetCurrentUserId();
                long city_id = AccountModel.GetCurrentUserRegionId(cur_id);
                ViewBag.curCityId = city_id;
                ViewBag.curDistrictList = AreaModel.GetDistrictListByCityId(city_id);
                m_country = city_id;
            }
            else if (ViewBag.role == 2)
            {
                long cur_id = CommonModel.GetCurrentUserId();
                long district_id = AccountModel.GetCurrentUserRegionId(cur_id);
                ViewBag.curDistrictId = district_id;
                ViewBag.curDistrictName = AreaModel.GetRegionNameById(district_id);
                m_country = district_id;
            }
            else
            {
                m_country = 0;
            }

            ViewBag.cityregion = AreaModel.GetCityList();

            ViewBag.units = UnitModel.GetUnitListFull();
            TypeModel aModel = new TypeModel();
            //ViewBag.kinds = aModel.GetNongyaoNotParent();
            List<tbl_nongyao> middelTypes = aModel.GetNongyaoAtParent(0);
            ViewBag.middlekinds = middelTypes;
            ViewBag.lastkinds = aModel.GetNongyaoAtParent(middelTypes);

            return View();
        }

        [AjaxOnly]
        public JsonResult RetrieveCatalogList(JQueryDataTableParamModel param)
        {
            ProductModel aModel = new ProductModel();
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            m_searchWord = Request.QueryString["search"];
            if (m_searchWord == null) m_searchWord = "";
            String pass = Request.QueryString["pass"];
            m_searchpass = 3;
            try { m_searchpass = byte.Parse(pass); }
            catch (Exception e) { }

            JqDataTableInfo rst = aModel.GetCatalogDataTable(param, Request.QueryString, rootUri, m_country, m_searchWord, m_searchpass);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxOnly]
        public JsonResult SubmitCatalog(string uid, string catalog_num, string nickname, string barcode, string shape, string material, string content, string avail_date, string product, string level, string kind,
            string product_area, string description, string imgpath, string reason, string pass, string usingname)
        {
            ProductModel aModel = new ProductModel();
            bool rst = false;

            long id = 0;
            try { id = long.Parse(uid); }
            catch (System.Exception ex) { }

            byte blevel = (byte)(level == "on" ? 1 : 0);

            int ikind = 0;
            try { ikind = int.Parse(kind); }
            catch (System.Exception ex) { }

            byte ipass = 0;
            try { ipass = byte.Parse(pass); }
            catch (System.Exception ex) { }

            int iavail_date = 0;
            try { iavail_date = byte.Parse(avail_date); }
            catch (System.Exception ex) { }
            
            decimal d_content = 0;
            try { d_content = decimal.Parse(content); }
            catch (System.Exception ex) { }

            if (id != 0)
                rst = aModel.UpdateCatalog(id, catalog_num, nickname, barcode, shape, material, d_content, iavail_date, product, blevel, ikind, 
                    product_area, description, imgpath, reason, ipass, usingname);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteCatalog(long id)
        {
            ProductModel aModel = new ProductModel();
            bool rst = aModel.DeleteCatalog(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public string FilterCatalogList(string country, string searchWord, string pass)
        {

            if (searchWord == null)
                searchWord = "";
            if (country == null)
                country = "";
            m_country = Convert.ToInt64(country);
            m_searchWord = searchWord;
            m_searchpass = Convert.ToByte(pass);

            return "";
        }
        public JsonResult CheckUniqueUsingname(string check_val, long id)
        {
            bool ret = ProductModel.IsUniqueUsingname(check_val, id);

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetBarcode(string precode, string barcode, string making)
        {
            string bin;
            if (making == "1")
                bin = BarCodeToHTML.get39code(precode);
            else
                bin = barcode;
            string image = BarCodeToHTML.get39image(precode, bin, 2, 60);

            return Json(new { bin = bin, image = image }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProductInfo(long id)
        {
            ProductModel aModel = new ProductModel();
            var item = aModel.GetCatalogInfo(id);

            return Json(new { 
                data = item, 
                username = UserModel.GetUserName((long)item.userid), 
                regtime = String.Format("{0:yyyy-MM-dd HH:mm:ss}", item.regtime) }, JsonRequestBehavior.AllowGet);
        }
    }
}
