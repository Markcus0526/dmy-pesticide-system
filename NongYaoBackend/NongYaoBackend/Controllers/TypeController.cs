using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NongYaoBackend.Models;
using NongYaoBackend.Models.Library;

namespace NongYaoBackend.Controllers
{
    public class TypeController : Controller
    {
        //
        // GET: /Area/

        public ActionResult ManageAgrochemicalType()
        {
            if (User.Identity.Name.Length > 5)
            {
                ViewBag.username = User.Identity.Name.Substring(0, 5) + "...";
            }
            else
            {
                ViewBag.username = User.Identity.Name;
            }
            TypeModel aModel = new TypeModel();
            List<tbl_nongyao> middelTypes = aModel.GetNongyaoAtParent(0);
            ViewBag.middleTypes = aModel.GetNames(middelTypes);
            ViewBag.lastTypes = aModel.GetNongyaoAtParentToString(middelTypes);
            ViewBag.role = CommonModel.GetCurrentUserRole();

            return View();
        }

        [AjaxOnly]
        public JsonResult RetrieveAgrochemicalType()
        {
            TypeModel aModel = new TypeModel();
            List<tbl_nongyao> middelTypes = aModel.GetNongyaoAtParent(0);
            List<String> middle = aModel.GetNames(middelTypes);
            List<List<String>> last= aModel.GetNongyaoAtParentToString(middelTypes);

            return Json(new {middle = middle, last = last}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SubmitType(string name, string type, string origin)
        {
            TypeModel aModel = new TypeModel();
            byte role = CommonModel.GetCurrentUserRole();
            string rst;
            if (origin == null || origin.Length == 0)
            {
                if (role == 0 || role == 1) //super or city role
                {
                    rst = aModel.InsertType(name, type);
                }
                else
                {
                    rst = TYPE_SUBMITSTATUS.ERROR_SUBMIT;
                }
            }
            else
            {
                string[] s = origin.Split(new char[] { '|' });
                if ((s.Length == 2) && role == 0) //only super role
                   rst = aModel.UpdateType(name, type, s[0], s[1]);
                else
                    rst = TYPE_SUBMITSTATUS.ERROR_SUBMIT;
            }

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteType(string name, string type)
        {
            bool rst = false;
            if (CommonModel.GetCurrentUserRole() == 0)  //only super role
            {
                TypeModel aModel = new TypeModel();
                rst = aModel.DeleteType(name, type);
            }
            else
                rst = false;

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
