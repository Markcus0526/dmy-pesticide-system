using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NongYaoBackend.Models;
using NongYaoBackend.Models.Library;

namespace NongYaoBackend.Controllers
{
    public class AreaController : Controller
    {
        //
        // GET: /Area/

        public ActionResult ManageArea()
        {
            if (User.Identity.Name.Length > 5)
            {
                ViewBag.username = User.Identity.Name.Substring(0, 5) + "...";
            }
            else
            {
                ViewBag.username = User.Identity.Name;
            }
            byte role = CommonModel.GetCurrentUserRole();
            if (role == 0) // super role
            {
                ViewBag.AreaBaseName = "地区";
            }
            else if (role == 1) // city role
            {
                long myid = CommonModel.GetCurrentUserId();
                ViewBag.AreaBaseName = AccountModel.GetUserRegionName(myid); 
            }
            else if (role == 2) // district role
            {
                long myid = CommonModel.GetCurrentUserId();
                ViewBag.AreaBaseName = AccountModel.GetDistrictBaseName(myid); 
            }
            ViewBag.role = role;
            return View();
        }

        [AjaxOnly]
        public JsonResult RetrieveRegionList()
        {
            AreaModel aModel = new AreaModel();
            List<string> regions = new List<string>();
            byte role = CommonModel.GetCurrentUserRole();
            if (role == 0)
            {
                regions = aModel.GetCityRegionListToString();
            }
            else if (role == 1)
            {
                long myid = CommonModel.GetCurrentUserId();
                long cityid = AccountModel.GetCurrentUserRegionId(myid);
                if(cityid != 0)
                    regions = aModel.GetDistrictRegionListToString(cityid);
            }
            else if (role == 2)
            {
                long myid = CommonModel.GetCurrentUserId();
                long uppercityid = AccountModel.GetCurrentUserUpperRegionId(myid);
                if (uppercityid != 0)
                    regions = aModel.GetDistrictRegionListToString(uppercityid);
            }

            return Json(new { region = regions}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitRegion(string name, string origin)
        {
            string rst = "";
            byte role = CommonModel.GetCurrentUserRole();
            if (role == 0 || role == 1)
            {
                AreaModel aModel = new AreaModel();

                long myid = CommonModel.GetCurrentUserId();
                long cityid = AccountModel.GetCurrentUserRegionId(myid);

                if (origin == null || origin.Length == 0)
                    rst = aModel.InsertRegion(name, role, cityid);
                else
                    rst = aModel.UpdateRegion(name, origin, role, cityid);
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteRegion(string name, string type)
        {
            AreaModel aModel = new AreaModel();
            bool rst = aModel.DeleteRegion(name);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
