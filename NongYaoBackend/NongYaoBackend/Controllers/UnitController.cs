using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NongYaoBackend.Models.Library;
using NongYaoBackend.Models;

namespace NongYaoBackend.Controllers
{
    public class UnitController : Controller
    {
        //
        // GET: /Area/

        public ActionResult ManageUnit()
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
            ViewBag.role = role;
            return View();
        }

        public JsonResult RetrieveUnitList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = UnitModel.GetListDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitUnit(string name, string id)
        {
            UnitModel aModel = new UnitModel();
            string rst;
            long uid = 0;
            try
            {
                uid = long.Parse(id);
            }
            catch (System.Exception ex)
            {

            }
            rst = aModel.CheckUnit(name, uid);
            if (rst == "")
            {
                if (id == null || id.Length == 0)
                    rst = aModel.InsertUnit(name);
                else
                    rst = aModel.UpdateUnit(uid, name);
            }           

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteUnit(string id)
        {
            long uid = 0;
            try
            {
                uid = long.Parse(id);
            }
            catch (System.Exception ex)
            {

            }

            UnitModel aModel = new UnitModel();
            bool rst = aModel.DeleteUnit(uid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

    }
}
