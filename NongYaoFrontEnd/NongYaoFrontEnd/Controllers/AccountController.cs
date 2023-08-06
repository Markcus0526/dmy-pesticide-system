using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;
using NongYaoFrontEnd.Models;
using NongYaoFrontEnd.Models.Library;
using System.Xml;

namespace NongYaoFrontEnd.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogOn(string returnUrl)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["isUsingCaptcha"] = "none";
            ViewData["Message"] = "";

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "First");
            }

            return View("LogOn");
        }

        private AccountModel accountModel = new AccountModel();
        private double fLng = 0.0f, fLat = 0.0f;
        private double FIX_LNG = 122.250431f;
        private double FIX_LAT = 43.659046f;

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

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string captcha, string isUsingCaptcha, string returnUrl)
        {
            string addr = Request.UserHostAddress;

            if (isUsingCaptcha.CompareTo("inline") == 0)
            {
                if (captcha.ToUpper() != HttpContext.Session["captcha"].ToString())
                {
                    ViewData["Message"] = "您输入的验证码有误!";
                    ViewData["isUsingCaptcha"] = "inline";

                    ModelState.AddModelError("modelerror", "您输入的验证码有误!");
                    return View("LogOn", model);
                }
                else
                    accountModel.UpdateFailedCount(addr, false);
            }

            string userRole = "";

            ViewData["rootUri"] = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["base_url"] = ViewData["rootUri"] + "Account";

            ViewData["curdate"] = DateTime.Today.ToString("yyyy年MM月dd日");
            ViewData["isUsingCaptcha"] = "none";
            ViewData["Message"] = "";

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
                    ViewData["Message"] = "";

                    var shopInfo = ShopModel.GetShopInfo((long)userInfo.shop_id);
                    if (shopInfo == null)
                    {
                        ModelState.AddModelError("modelerror", "帐号的经销商没有存在");
                        return View("LogOn", model);
                    }

                    long city_id = RegionModel.GetCityIdForRegionId(shopInfo.region);

                    accountModel.UpdateFailedCount(addr, false);
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                userInfo.name,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(1440),
                                model.RememberMe == "on" ? true: false,
                                userInfo.role + "|" + userInfo.id + "|" + userInfo.name + "|" + userInfo.shop_id + "|" + shopInfo.level + "|" + city_id,
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
                        return RedirectToAction("Index", "First");
                    }
                }
                else
                {
                    var deluser = UserModel.GetDelUser(model.UserName, model.Password);

                    if (deluser != null)
                        ModelState.AddModelError("modelerror", "你帐号删除了");
                    else
                        ModelState.AddModelError("modelerror", "帐号或密码错误，请重新输入");

                    int faildcount = accountModel.UpdateFailedCount(addr, true);
                    if (faildcount >= 3)
                        ViewData["isUsingCaptcha"] = "inline";
                }

                return View("LogOn", model);
            }
            else
            {
                ModelState.AddModelError("modelerror", "帐号或密码错误，请重新输入");
            }

            return View("LogOn", model);
        }

        public CaptchaResult GetCaptcha()
        {
            Captcha captcha = new Captcha();
            string captchaText = captcha.GenerateRandomText();

            HttpContext.Session.Add("captcha", captchaText);
            return new CaptchaResult(captchaText);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("LogOn", "Account");
        }

        public ActionResult Register()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;

            List<Region> citylist = (List<Region>)RegionModel.GetCityList();
            long initcityid = 0;
            if (citylist.Count > 0)
                initcityid = citylist.ElementAt(0).id;

            GetLngLatWithIP();

            ViewData["cityList"] = citylist;
            ViewData["districtList"] = RegionModel.GetDistrictList(initcityid);
            ViewData["fixlng"] = 122.249902;
            ViewData["fixlat"] = 43.65994;

            return View("Register");
        }

        public JsonResult GetDistrictList(string cityid)
        {
            long l_cityid = Convert.ToInt64(cityid);
            var rst = RegionModel.GetDistrictList(l_cityid);
            var ret = Json(new { districtdata = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }

        public JsonResult GetPinyinCode(string originStr)
        {
            var rst = CommonModel.GetPinyinCode(originStr);
            var ret = Json(new { pinyincode = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }

        [HttpPost]
        public JsonResult UserIdUniqueCheck(string userid)
        {
            UserModel aModel = new UserModel();

            bool rst;
            if (userid.Equals("")) 
                rst = false;
            else
                rst = aModel.UserIdUniqueCheck(userid);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegisterShop(string userid, string password, string rpassword, string manager_name, string permitid,
            string shopname, string nickname, string cityregion, string districtregion, string addr, string username, string mobile_phone, 
            string phone, string mailaddr, string qqnum, string level, string lon, string lat)
        {

            bool rst;

            byte level1 = 0;
            if (level != null)
                level1 = 1;

            UserModel aModel = new UserModel();                        
            rst = aModel.UserIdUniqueCheck(userid);

            if (rst)
            {
                string rst1 = ShopModel.RegisterShop(permitid, shopname, nickname, districtregion, addr, username, mobile_phone, phone, mailaddr, qqnum, level1, manager_name, userid, password, lon, lat);
                if (rst1.Equals(""))
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public void GetLngLatWithIP()
        {
            string reqUrl = "http://api.map.baidu.com/location/ip?ak=B9c5b113ec6dcbccb4540870728af498&coor=bd09ll";
            string responseBody = null;
            string contentType = "application/json";

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
                return;
            }

            Stream respStream = resp.GetResponseStream();
            if (respStream != null)
            {
                responseBody = new StreamReader(respStream).ReadToEnd();
            }

            dynamic dynJson = JsonConvert.DeserializeObject(responseBody);                        

            try
            {
                string strLng = dynJson.content.point.x;
                fLng = double.Parse(strLng);
            }
            catch
            {
                fLng = FIX_LNG;
            }

            try
            {
                string strLat = dynJson.content.point.y;
                fLat = double.Parse(strLat);
            }
            catch
            {
                fLat = FIX_LAT;
            }

            return;
        }
    }
}
