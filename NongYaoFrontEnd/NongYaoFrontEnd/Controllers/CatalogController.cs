using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using NongYaoFrontEnd.Models.Library;
using NongYaoFrontEnd.Models;

namespace NongYaoFrontEnd.Controllers
{
    
    public class CatalogController : Controller
    {
        //
        // GET: /Catalog/
        
        public ActionResult Index()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["userrole"] = CommonModel.GetCurrentUserRole();            
            ViewData["unitList"] = UnitModel.GetUnitList();
            ViewData["nongyaoList"] = CatalogModel.GetNongYaoList();
            ViewData["curdate"] = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            ViewData["userName"] = CommonModel.GetCurrentUserName();
            return View();
        }

        public JsonResult SubmitCatalog(string ctlg_register, string ctlg_permit, string ctlg_sample, string ctlg_name, string ctlg_nickname, string ctlg_product,
            string ctlg_shape, string ctlg_material, string ctlg_content, string ctlg_level, string ctlg_productarea, string ctlg_description, string imgpath)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            var rst1 = CatalogModel.GetCatalogPassInfo(ctlg_register);
            string rst = "";

            if (rst1 != -1)
            {
                rst = CATALOG_SUBMITSTATUS.DUPLICATE_REGISTER;
            }
            else
            {
                rst = CatalogModel.InsertCatalog(ctlg_register, ctlg_permit, ctlg_sample, ctlg_name, ctlg_nickname, CommonModel.GetCurrentUserId(),
                                        ctlg_product, ctlg_shape, ctlg_material, ctlg_content, ctlg_level, ctlg_productarea, ctlg_description, imgpath);
            }            

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckCatalogPass(string ctl_register)
        {
            var rst = CatalogModel.GetCatalogPassInfo(ctl_register);
            var ret = Json(new { pass = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }
    }
}
