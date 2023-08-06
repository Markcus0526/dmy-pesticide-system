using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NongYaoBackend.Models;
using NongYaoBackend.Models.Library;


namespace NongYaoBackend.Controllers
{
    public class SalesmanController : Controller
    {
        //
        // GET: /Area/
        static long m_regionid = 0;
        static string m_searchWord = "";
        static byte m_searchpass = 2;   //全都
        [Authorize]
        public ActionResult ConsiderSalesman()
        {
            
            if (User.Identity.Name.Length > 5)
            {
                ViewBag.username = User.Identity.Name.Substring(0, 5) + "...";
            }
            else
            {
                ViewBag.username = User.Identity.Name;
            }
            if(User.Identity.Name == "2")
            {
                ViewBag.username = " 管理人同志";
            }
            //ViewBag.region = AreaModel.GetRegionList();
            ViewBag.role = CommonModel.GetCurrentUserRole();
            if (ViewBag.role == 1)
            {
                long cur_id = CommonModel.GetCurrentUserId();
                long city_id = AccountModel.GetCurrentUserRegionId(cur_id);
                ViewBag.curCityId = city_id;
                ViewBag.curDistrictList = AreaModel.GetDistrictListByCityId(city_id);
                m_regionid = city_id;
            }
            else if (ViewBag.role == 2)
            {
                long cur_id = CommonModel.GetCurrentUserId();
                long district_id = AccountModel.GetCurrentUserRegionId(cur_id);
                ViewBag.curDistrictId = district_id;
                ViewBag.curDistrictName = AreaModel.GetRegionNameById(district_id);
                m_regionid = district_id;
            }
            else
            {
                m_regionid = 0;
            }
            ViewBag.cityregion = AreaModel.GetCityList();
            return View();
        }


        public JsonResult RetrieveSalesman(JQueryDataTableParamModel param)
        {
            SalesmanModel sModel = new SalesmanModel();
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            m_searchWord = Request.QueryString["search"];
            if (m_searchWord == null)  m_searchWord = "";
            String pass = Request.QueryString["pass"];
            m_searchpass = 3;
            try { m_searchpass = byte.Parse(pass); }
            catch (Exception e) { }

            JqDataTableInfo rst = sModel.GetSalesmanTable(param, Request.QueryString, rootUri, m_regionid, m_searchWord, m_searchpass);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RetrieveSalesmanInfo(long id)
        {
            if (id > 0)
            {
                SalesmanModel sModel = new SalesmanModel();
                var rst = sModel.GetSalesmanData(id);

                var ret = Json(new { salesmandata = rst }, JsonRequestBehavior.AllowGet);
                return ret;
            }
                
            return null;
        }
        
        [HttpPost]
        public JsonResult SubmitSalesman(string uid, string shop_name, string nickname, string cityregion, string districtregion, string addr,
            string username, string faren, string mobile_phone, string phone, string qqnum,
            string mailaddr, string level, string longitude, string latitude, string technical_manager, string notice,string pass,string aid)
        {
            SalesmanModel sModel = new SalesmanModel();
            bool rst = true;

            byte blevel = (byte)(level == "on" ? 1 : 0);
            long l_cityregion = 0; 
            long l_dregion = 0;
            long region = 0;
            if(cityregion != "")
                l_cityregion = Convert.ToInt64(cityregion);
            if (districtregion != "")
                l_dregion = Convert.ToInt64(districtregion);
            if (l_dregion != 0)
                region = l_dregion;
            else
                region = l_cityregion;
            rst = sModel.UpdateSalesman(aid, uid, shop_name, nickname, region, addr, username, faren, mobile_phone,
                phone, qqnum, mailaddr, blevel, longitude, latitude, technical_manager, notice, pass);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteSalesman(string id)
        {
            SalesmanModel sModel = new SalesmanModel();
            bool rst = true;

            rst = sModel.DeleteSalesman(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public string FilterSalesmanTable(string countryid, string searchWord, string pass)
        {
            
            if (searchWord == null)
                searchWord = "";
            m_regionid = Convert.ToInt64(countryid);
            m_searchWord = searchWord;
            m_searchpass = Convert.ToByte(pass);
            //RetrieveSalesman(m_queryParam);
            return "";
        }
        public JsonResult CheckRole(long region_id)
        {
            long myid = CommonModel.GetCurrentUserId();
            byte my_role = CommonModel.GetCurrentUserRole();
            long myregion = AccountModel.GetCurrentUserRegionId(myid);
            bool rst = false;
            if (my_role == 0)
                rst = true;
            if (myregion == region_id)
                rst = true;
            if (my_role == 1)
                rst = AreaModel.IsUpperRegion(myregion, region_id);

            var ret = Json(new { roledata = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult GetDistrictList(string cityid)
        {
            long l_cityid = Convert.ToInt64(cityid);
            var rst = AreaModel.GetDistrictListByCityId(l_cityid);
            var ret = Json(new { districtdata = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }
    }
}
