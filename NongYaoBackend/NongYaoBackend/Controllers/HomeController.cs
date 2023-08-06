using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NongYaoBackend.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult ConsiderSalesman()
        {
            ViewBag.Message = "ConsiderSalesman";

            return View();
        }

        public ActionResult ConsiderProduct()
        {
            ViewBag.Message = "ConsiderProduct";
            return View();
        }

        public ActionResult ManageAgrochemicalType()
        {
            ViewBag.Message = "ManageAgrochemicalType";
            return View();
        }

        public ActionResult ManageArea()
        {
            ViewBag.Message = "ManageArea";
            return View();
        }

        public ActionResult ManageUnit()
        {
            ViewBag.Message = "ManageUnit";
            return View();
        }

        public ActionResult Statistics()
        {
            ViewBag.Message = "Statistics";
            return View();
        }
    }
}
