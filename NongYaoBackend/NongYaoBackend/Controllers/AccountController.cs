using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using NongYaoBackend.Models;
using NongYaoBackend.Models.Library;
using System.Net;
using System.IO;
using System.Xml;



namespace NongYaoBackend.Controllers
{
    public class AccountController : Controller
    {

        private AccountModel accountModel = new AccountModel();

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            ViewBag.isUsingCaptcha = "none";

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ConsiderSalesman", "Salesman");
            }

            return View("LogOn");
        }
        
        public ActionResult ManageAccount()
        {
            if (User.Identity.Name.Length > 5)
            {
                ViewBag.username = User.Identity.Name.Substring(0, 5) + "...";
            }
            else
            {
                ViewBag.username = User.Identity.Name;
            }
            ViewBag.role = CommonModel.GetCurrentUserRole();
            if (ViewBag.role == 0) // superrole
            {
                ViewBag.region = AreaModel.GetCityList();
                return View();
            }
            else if (ViewBag.role == 1) //cityrole
            {
                long uid = CommonModel.GetCurrentUserId();
                long cityid = AreaModel.GetRegionIdByUserId(uid);
                ViewBag.region = AreaModel.GetDistrictListByCityId(cityid);
                return View();
            }
            else
            {
                return View("error");
            }
            
            
        }

        public JsonResult RetrieveUserInfo(long id)
        {
            if (id > 0)
            {
                var rst = AccountModel.GetUserInfoById(id);
                var ret = Json(new { userdata = rst }, JsonRequestBehavior.AllowGet);
                return ret;
            }
            return null;
        }
        [AjaxOnly]
        public JsonResult RetrieveUser(JQueryDataTableParamModel param)
        {
            AccountModel aModel = new AccountModel();
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = aModel.GetUserTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public ActionResult SubmitUser(string uid, string userid, string password, string username, string city,string district, string unit_name, string connect)
        {
            RegisterModel aModel = new RegisterModel();
            bool rst = true;
            long l_city = Convert.ToInt64(city);
            long l_district = Convert.ToInt64(district);
            byte b_roleinfo = CommonModel.GetCurrentUserRole();

            if (uid == "")
                rst = aModel.AddUser(userid, password, username, l_city, l_district, unit_name, connect, b_roleinfo);
            else
            {
                long l_uid = Convert.ToInt64(uid);
                rst = aModel.UpdateUser(l_uid, userid, password, username, l_city, l_district, unit_name, connect, b_roleinfo);
            }
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteUser(string id)
        {
            AccountModel sModel = new AccountModel();
            bool rst = true;

            rst = sModel.DeleteUser(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public bool IsTrialVersion()
        {
            return false;
            /*
            string xmlstr = GetYahooCurrTime();

            try
            {
                if (String.IsNullOrEmpty(xmlstr))
                {
                    return true;
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstr);

                XmlNodeList tnode = doc.DocumentElement.GetElementsByTagName("Timestamp");
                string currtime = tnode[0].InnerText;

                long currtimestamp = long.Parse(currtime);

                TimeSpan tspan = (new DateTime(2014, 10, 1, 0, 0, 0) - new DateTime(1970, 1, 1, 0, 0, 0, 0));

                if (currtimestamp > tspan.TotalSeconds)
                {
                    return true;
                }

            }
            catch
            {

            }

            return false;
            */
        }
        public static string GetYahooCurrTime()
        {
            string reqUrl = "http://developer.yahooapis.com/TimeService/V1/getTime?appid=YahooDemo";
            string responseBody = null;
            string contentType = "application/text";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(reqUrl);
            req.Method = "GET";
            req.ContentType = contentType;

            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch
            {
                return null;
            }

            Stream respStream = resp.GetResponseStream();
            if (respStream != null)
            {
                responseBody = new StreamReader(respStream).ReadToEnd();
                return responseBody;
            }

            return null;
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string captcha, string isUsingCaptcha, string returnUrl)
        {
            string addr = Request.UserHostAddress;

            if (isUsingCaptcha.CompareTo("inline") == 0)
            {
                if (captcha.ToUpper() != HttpContext.Session["captcha"].ToString())
                {
                    ViewBag.isUsingCaptcha = "inline";

                    ModelState.AddModelError("modelerror", "您输入的验证码有误!");
                    return View("LogOn", model);
                }
                else
                    accountModel.UpdateFailedCount(addr, false);
            }

            string userRole = "";
            
            ViewBag.curdate = DateTime.Today.ToString("yyyy年MM月dd日");
            ViewBag.isUsingCaptcha = "none";

            if (IsTrialVersion() == true)
            {
                ModelState.AddModelError("modelerror", "系统到期了，已经过了测试期限!");
                return View("LogOn", model);
            }

            if (ModelState.IsValid)
            {
                var userInfo = accountModel.ValidateUser(model.UserName, model.Password);
                if (userInfo != null)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                userInfo.name,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(1440),
                                model.RememberMe == "on" ? true : false,
                                //adminrole.role + "|" + userInfo.uid + "|" + userInfo.realname + "|" + userInfo.imgpath,
                                "admin|" + userInfo.id + "|" + userInfo.name,
                                FormsAuthentication.FormsCookiePath);

                    // Encrypt the ticket.
                    string encTicket = FormsAuthentication.Encrypt(ticket);

                    // Create the cookie.
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("ConsiderSalesman", "Salesman");
                    }
                }
                else
                {
                    ModelState.AddModelError("modelerror", "帐号或密码错误，请重新输入");

                    int faildcount = accountModel.UpdateFailedCount(addr, true);
                    if (faildcount >= 3)
                        ViewBag.isUsingCaptcha = "inline";
                }

                return View("LogOn", model);
            }

            else
            {
                ModelState.AddModelError("modelerror", "帐号或密码错误，请重新输入");
            }

            // If we got this far, something failed, redisplay form
            return View("LogOn", model);
        }

        public CaptchaResult GetCaptcha()
        {
            Captcha captcha = new Captcha();
            string captchaText = captcha.GenerateRandomText();

            HttpContext.Session.Add("captcha", captchaText);
            return new CaptchaResult(captchaText);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("LogOn", "Account");
        }

        //
        // GET: /Account/Register

        //public ActionResult RegisterCont()
        //{
        //    FormsAuthentication.SignOut();
        //    return RedirectToAction("Register", "Account");
            
        //}
        
        //public ActionResult Register()
        //{
        //    return View();
        //}
        ////
        //// POST: /Account/Register

        //[HttpPost]
        //public JsonResult Register(string name, string userid, string password, string rpassword, string phonenum)
        //{
        //    RegisterModel aModel = new RegisterModel();

        //    bool rst;
        //    rst = aModel.RegisterUser(name, userid, password, phonenum);

        //    return Json(rst, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult RegisterUniqueCheck(string userid)
        {
            RegisterModel aModel = new RegisterModel();

            bool rst;
            rst = aModel.RegisterUniqueCheck(userid, 0);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            long uid = CommonModel.GetCurrentUserId();
            ViewBag.old_pass = accountModel.GetAdminInfo(uid).password;

            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            long uid = CommonModel.GetCurrentUserId();
            ViewBag.old_pass = accountModel.GetAdminInfo(uid).password;

            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    tbl_admin admin = accountModel.GetAdminInfo(uid);
                    if (admin.password == model.OldPassword)
                        changePasswordSucceeded = accountModel.ChangePassword(uid, model.NewPassword);
                    else
                        changePasswordSucceeded = false;
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("LogOff");
                }
                else
                {
                    ModelState.AddModelError("modelerror", "原密码不正确或新密码有错误");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        /*public JsonResult ChangePassword(string old_pass, string new_pass, string retype_pass)
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
        }*/

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
