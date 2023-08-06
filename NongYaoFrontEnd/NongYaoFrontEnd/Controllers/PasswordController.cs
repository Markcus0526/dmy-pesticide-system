using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NongYaoFrontEnd.Models;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Controllers
{
    public class PasswordController : Controller
    {
        //
        // GET: /Password/

        public ActionResult Index()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["userrole"] = CommonModel.GetCurrentUserRole();
            ViewData["userName"] = CommonModel.GetCurrentUserName();

            long uid = CommonModel.GetCurrentUserId();
            ViewData["password"] = UserModel.GetUserInfo(uid).password;

            return View();
        }

        public JsonResult ChangePassword(string old_pass, string new_pass, string retype_pass)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            bool rst = true;

            long uid = CommonModel.GetCurrentUserId();

            User user = UserModel.GetUserInfo(uid);

            if (user.password != old_pass)
            {
                rst = false;
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            rst = UserModel.ChangePassword(uid, new_pass);     
            
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

    }
}
