using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NongYaoFrontEnd.Models;

namespace NongYaoFrontEnd.Controllers
{
    public class FirstController : Controller
    {
        //
        // GET: /First/

        public ActionResult Index()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["userrole"] = CommonModel.GetCurrentUserRole();
            ViewData["userName"] = CommonModel.GetCurrentUserName();
            ViewData["header"] = "First";

            ViewData["passedCatalogs"] = CatalogModel.GetPassedCatalog();
            ViewData["unpassedCatalogs"] = CatalogModel.GetUnpassedCatalog();

            return View();
        }

    }
}
