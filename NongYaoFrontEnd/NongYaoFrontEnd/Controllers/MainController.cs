using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NongYaoFrontEnd.Models.Library;
using NongYaoFrontEnd.Models;
namespace NongYaoFrontEnd.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/
        static string m_searchCustomer = "";

        static string m_searchSupply = "";

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            ViewData["userrole"] = CommonModel.GetCurrentUserRole();
            ViewData["userName"] = CommonModel.GetCurrentUserName();

            string init = Request.QueryString["init"];
            if (init != null && init == "1")
            {
                long shop_id = CommonModel.GetCurrentUserShopId();
                SupplyModel.DeleteSupplysForShopId(shop_id);
                UserModel.DeleteUsersForShopId(shop_id);
                StoreModel.DeleteStoresForShopId(shop_id);
                StoreModel.DeleteStoremovingsForShopId(shop_id);
                StoreModel.DeleteUsingsForShopId(shop_id);
                TicketModel.DeleteTicketsForShopId(shop_id);
                RemainModel.DeleteRemainsForShopId(shop_id);
              //  CatalogModel.DeleteCatalogsForShopId(shop_id);
                MoneyModel.DeletePaymentsForShopId(shop_id);
                MoneyModel.DeleteOtherpaysForShopId(shop_id);
                MoneyModel.DeleteBankingsForShopId(shop_id);
            }

            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult ShopInfo()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            
            Shop shop_info = ShopModel.GetShopInfo(CommonModel.GetCurrentUserShopId());
            ViewData["shopInfo"] = shop_info;

            long cityid = RegionModel.GetParentId(shop_info.region);
            ViewData["cityList"] = RegionModel.GetCityList();
            ViewData["districtList"] = RegionModel.GetDistrictList(cityid);

            ViewData["cityid"] = cityid;

            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult CustomerInfo()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult SupplyInfo()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            //IEnumerable<Region> rgnList = RegionModel.GetRegionList();;
            //ViewData["regionList"] = rgnList;
            //ViewData["regionFirst"] = rgnList.ElementAt(0).id;
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult UserInfo()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            return View();
        }

        [Authorize(Roles = "admin")]
        public ActionResult StoreInfo()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            ViewData["rootUri"] = rootUri;
            IEnumerable<User> userList = UserModel.GetStoreManList();
            ViewData["storeMan"] = userList;
            if (userList == null || userList.Count() == 0)
                ViewData["storeManFirst"] = "";
            else
                ViewData["storeManFirst"] = userList.ElementAt(0).id;
            return View();
        }

        [Authorize(Roles = "admin")]
        public JsonResult SubmitMain(string shop_id, string shopname, string nickname, string cityregion, string districtregion, string addr, string username, string mobile_phone, string phone, string mailaddr, string qqnum)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            long shopId = 0;
            try { shopId = long.Parse(shop_id); }
            catch (System.Exception ex) { }

            long regionId = 0;
            try { regionId = long.Parse(districtregion); }
            catch (System.Exception ex) { }

            bool rst = ShopModel.UpdateShop(shopId, nickname, regionId, addr, username, mobile_phone, phone, mailaddr, qqnum);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        //Managing Customer//
        ///////////
        public JsonResult RetrieveCustomerList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            JqDataTableInfo rst = CustomerModel.GetCustomerListDataTable(param, Request.QueryString, rootUri, m_searchCustomer);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectedCustomer(long id)
        {
            var rst = CustomerModel.GetCustomerInfo(id);
            return Json(new { custom = rst }, JsonRequestBehavior.AllowGet);
        }

        public string FilterCustomer(string searchWord)
        {
            m_searchCustomer = searchWord;
            return "";
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult SubmitEditCustomer(string customer_id, string customer_name, string customer_phone)
        {
            long id = 0;
            string rst;
            try
            {
                id = long.Parse(customer_id);
            }
            catch (System.Exception ex)
            {
            }
            CustomerModel customer = new CustomerModel();
            rst = customer.UpdateCustomerItem(id, customer_name, customer_phone);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult DeleteCustomer(long id)
        {
            CustomerModel customer = new CustomerModel();
            bool rst = customer.DeleteCustomer(id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        
        ///////
        //Managing Supply
        public JsonResult RetrieveSupplyList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
           
            JqDataTableInfo rst = SupplyModel.GetSupplyListDataTable(param, Request.QueryString, rootUri, m_searchSupply);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public string FilterSupply(string searchWord)
        {
            m_searchSupply = searchWord;
            return "";
        }

        public JsonResult GetSelectedSupply(long id)
        {
            var rst = SupplyModel.GetSupplyInfo(id);
            return Json(new { custom = rst }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult SubmitEditSupply(string supply_id, string supply_name, string supply_nickname, string supply_region,string supply_addr, string supply_contactname,
                                            string supply_contactmobile, string supply_phone, string supply_qqnum)
        {
            long id = 0;
            string rst;
            try
            {
                id = long.Parse(supply_id);
            }
            catch (System.Exception ex)
            {
            }

            SupplyModel supply = new SupplyModel();

            if (id > 0)
            {
                rst = supply.UpdateSupplyItem(id, supply_name, supply_nickname, supply_region, supply_addr, supply_contactname,
                                            supply_contactmobile, supply_phone, supply_qqnum);
            }
            else
            {
                rst = supply.InsertSupplyItem(supply_name, supply_nickname, supply_region, supply_addr, supply_contactname,
                                            supply_contactmobile, supply_phone, supply_qqnum);
            }
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult DeleteSupply(long id)
        {
            SupplyModel supply = new SupplyModel();

            bool rst = supply.DeleteSupply(id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        ///////
        //Managing User
        public JsonResult RetrieveUserList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = UserModel.GetUserListDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectedUser(long id)
        {
            var rst = UserModel.GetUserInfo(id);
            return Json(new { custom = rst }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult SubmitEditUser(string uid, string user_name, string user_id, string user_password, string user_phone, 
                                    string buying, string sale, string store, string account)
        {
            long id = 0;
            string rst;
            try
            {
                id = long.Parse(uid);
            }
            catch (System.Exception ex)
            {
            }
            string role = "";
            if (buying != null)
                role = "buying";
            if (sale != null)
            {
                if (role.CompareTo("") == 0)
                    role = "sale";
                else
                    role = role + ",sale";
            }
            if (store != null)
            {
                if (role.CompareTo("") == 0)
                    role = "store";
                else
                    role = role + ",store";
            }
            if (account != null)
            {
                if (role.CompareTo("") == 0)
                    role = "account";
                else
                    role = role + ",account";
            }
            UserModel user = new UserModel();
            if (id > 0)
            {
                rst = user.UpdateUserItem(id, user_name, user_id,user_phone, user_password,role);
            }
            else
            {
                rst = user.InsertUserItem(user_name, user_id, user_phone,user_password, role);
            }
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult DeleteUser(long id)
        {
            UserModel customer = new UserModel();
            bool rst = customer.DeleteUser(id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }


        ///////
        //Managing Store
        public JsonResult RetrieveStoreList(JQueryDataTableParamModel param)
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

            JqDataTableInfo rst = StoreModel.GetStoreListDataTable(param, Request.QueryString, rootUri);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSelectedStore(long id)
        {
            var rst = StoreModel.GetStoreInfo(id);
            return Json(new { custom = rst }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult SubmitEditStore(string store_id, string store_name, string store_manager)
        {
            long id = 0;
            string store_data = Request.Form["store_manager"].ToString();
            string rst;
            try
            {
                id = long.Parse(store_id);
            }
            catch (System.Exception ex)
            {
            }

           /* long userId = 0;
            try
            {
                userId = long.Parse(store_manager);
            }
            catch (System.Exception ex)
            {
            }*/

            StoreModel store = new StoreModel();

            if (id > 0)
            {
                rst = store.UpdateStoreItem(id, store_name, store_data);
            }
            else
            {
                rst = store.InsertStoreItem(store_name, store_data);
            }
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public JsonResult DeleteStore(long id)
        {
            StoreModel customer = new StoreModel();
            bool rst = customer.DeleteStore(id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
