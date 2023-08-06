using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    public class TicketModel
    {
        #region ticket_model

        public class TICKET_SUBMITSTATUS
        {
            public const string DUPLICATE_NAME = "重复";
            public const string SUCCESS_SUBMIT = "";
            public const string ERROR_SUBMIT = "操作失败";
        }

        public class TicketInfo
        {
            public long id { get; set; }
            public string ticketnum { get; set; }
            public DateTime regtime { get; set; }
            public string customer_name { get; set; }
            public string contact_phone { get; set; }
            public decimal total_price { get; set; }
            public decimal real_price { get; set; }
            public string username { get; set; }
            public byte type { get; set; }
        }

        #endregion

        private NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static int GetCurrentTickerNumber(byte type, string prefix)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            return (from m in db.tbl_tickets
                    where m.deleted == 0 && m.regtime.Date == DateTime.Today && m.ticketnum.Contains(prefix) //m.type == type
                    select m).Count() + 1;
        }

        public static string MakeCurrentTicketNumber(byte type)
        {
            string retstr = "";
            long shop_id = CommonModel.GetCurrentUserShopId();
            string name = ShopModel.GetShopInfo(shop_id).nickname.ToUpper();
            retstr += name; // name.Length >= 2 ? name.Substring(0, 2) : name;

            switch (type)
            {
                case 0:
                    retstr += "_IN_";
                    break;
                case 1:
                    retstr += "_IB_";
                    break;
                case 2:
                    retstr += "_OT_";
                    break;
                case 3:
                    retstr += "_OB_";
                    break;
            }

            retstr += String.Format("{0:yyyyMMdd}", DateTime.Today);
            retstr += "_";

            retstr += String.Format("{0:000000}", GetCurrentTickerNumber(type, retstr));

            return retstr;
        }

        public bool IsConsistent(string ticketnum)
        {
            var item = (from m in db.tbl_tickets
                        where m.deleted == 0 && m.ticketnum == ticketnum
                        select m).FirstOrDefault();
            if (item != null)
                return false;
            else
                return true;
        }

        public string InsertBuyTicket(
            string ticketnum,
            long supply_id,
            byte paytype,
            long store_id,
            decimal total_price,
            decimal real_price,
            byte type)
        {
            try
            {
                if (IsConsistent(ticketnum))
                {
                    tbl_ticket newitem = new tbl_ticket();

                    newitem.ticketnum = ticketnum;
                    newitem.supply_id = supply_id;
                    newitem.userid = CommonModel.GetCurrentUserId();
                    newitem.regtime = DateTime.Now;
                    newitem.paytype = paytype;
                    newitem.customer_type = 1;
                    //newitem.customer_id = CommonModel.GetCurrentUserShopId();
                    newitem.store_id = store_id;
                    newitem.shop_id = CommonModel.GetCurrentUserShopId();
                    newitem.type = type;
                    newitem.total_price = total_price;
                    newitem.real_price = real_price;

                    db.tbl_tickets.InsertOnSubmit(newitem);
                    db.SubmitChanges();

                    return TICKET_SUBMITSTATUS.SUCCESS_SUBMIT;
                }
                else
                    return TICKET_SUBMITSTATUS.DUPLICATE_NAME;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TicketModel", "InsertTicket()", e.ToString());
                return TICKET_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public string InsertSellTicket(
            string ticketnum,
            byte customer_type,
            long customer_id,
            long shop_id,
            byte paytype,
            long store_id,
            decimal total_price,
            decimal real_price,
            byte type)
        {
            try
            {
                if (IsConsistent(ticketnum))
                {
                    tbl_ticket newitem = new tbl_ticket();

                    newitem.ticketnum = ticketnum;
                    newitem.userid = CommonModel.GetCurrentUserId();
                    newitem.regtime = DateTime.Now;
                    newitem.paytype = paytype;
                    newitem.customer_type = customer_type;
                    if(customer_type == 0)
                        newitem.customer_id = customer_id;
                    else
                        newitem.customer_id = shop_id;
                    newitem.shop_id = CommonModel.GetCurrentUserShopId();
                    newitem.type = type;
                    newitem.store_id = store_id;
                    newitem.total_price = total_price;
                    newitem.real_price = real_price;

                    db.tbl_tickets.InsertOnSubmit(newitem);
                    db.SubmitChanges();

                    return TICKET_SUBMITSTATUS.SUCCESS_SUBMIT;
                }
                else
                    return TICKET_SUBMITSTATUS.DUPLICATE_NAME;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TicketModel", "InsertTicket()", e.ToString());
                return TICKET_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public tbl_ticket GetTicketInfo(string ticketnum)
        {
            return (from m in db.tbl_tickets
                    where m.deleted == 0 && m.ticketnum == ticketnum
                    select m).FirstOrDefault();
        }

        public string InsertBuying(
            long ticket_id,
            long catalog_id,
            long standard_id,
            string largenumber,
            string product_date,
            decimal price,
            int quantity,
            long store_id)
        {
            try
            {
                tbl_buying newitem = new tbl_buying();

                newitem.ticket_id = ticket_id;
                newitem.catalog_id = catalog_id;
                newitem.standard_id = standard_id;
                newitem.largenumber = largenumber;
                if(product_date != null)
                    newitem.product_date = Convert.ToDateTime(product_date);
                newitem.price = price;
                newitem.quantity = quantity;
                newitem.store_id = store_id;
                newitem.shop_id = CommonModel.GetCurrentUserShopId();

                db.tbl_buyings.InsertOnSubmit(newitem);
                db.SubmitChanges();
                
                return TICKET_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TicketModel", "InsertBuying()", e.ToString());
                return TICKET_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public string InsertSale(
            long ticket_id,
            long store_id,
            long catalog_id,
            long standard_id,
            string largenumber,
            decimal price,
            int count)
        {
            try
            {
                tbl_salelist newitem = new tbl_salelist();

                newitem.ticket_id = ticket_id;

				//newitem.regtime = DateTime.Now;
                //newitem.store_id = store_id;
                //newitem.standard = standard;

                newitem.catalog_id = catalog_id;
                newitem.standard_id = standard_id;
                newitem.largenumber = largenumber;
                newitem.catalog_price = price;
                newitem.catalog_cnt = count;
                newitem.shop_id = CommonModel.GetCurrentUserShopId();

                db.tbl_salelists.InsertOnSubmit(newitem);
                db.SubmitChanges();
                
                return TICKET_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TicketModel", "InsertSale()", e.ToString());
                return TICKET_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        //public string InsertBank(
        //    decimal price,
        //    byte type,
        //    long ticket_id
        //    )
        //{
        //    try
        //    {
        //        tbl_banklist newitem = new tbl_banklist();

        //        newitem.userid = CommonModel.GetCurrentUserId();
        //        newitem.price = price;
        //        newitem.type = type;
        //        newitem.ticket_id = ticket_id;
        //        newitem.shop_id = CommonModel.GetCurrentUserShopId();
        //        newitem.regtime = DateTime.Now;
        //        newitem.deleted = 0;

        //        db.tbl_banklists.InsertOnSubmit(newitem);
        //        db.SubmitChanges();
                
        //        return TICKET_SUBMITSTATUS.SUCCESS_SUBMIT;
        //    }
        //    catch (Exception e)
        //    {
        //        CommonModel.WriteLogFile("TicketModel", "InsertBank()", e.ToString());
        //        return TICKET_SUBMITSTATUS.ERROR_SUBMIT;
        //    }
        //}

        public string InsertSalestatistics(
            long region_id,
            long catalog_id,
            long nongyao_id,
            long quantity,
            long standard_id,
            string largenumber
            )
        {
            try
            {
                tbl_salestatistic newitem = new tbl_salestatistic();

                newitem.region_id = region_id;
                newitem.catalog_id = catalog_id;
                newitem.nongyao_id = nongyao_id;
                newitem.user_id = CommonModel.GetCurrentUserId();
                newitem.shop_id = CommonModel.GetCurrentUserShopId();
                newitem.regtime = DateTime.Now;
                newitem.standard_id = standard_id;
                newitem.largenumber = largenumber;
                newitem.quantity = quantity;

                var lastitem = db.tbl_salestatistics
                    .Where(m => m.deleted == 0)
                    .OrderByDescending(m => m.id)
                    .FirstOrDefault();
                long total_quantity = 0;
                if (lastitem != null)
                    total_quantity = lastitem.total_quantity;
                newitem.total_quantity = total_quantity + quantity;

                db.tbl_salestatistics.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return TICKET_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TicketModel", "InsertSalestatistics()", e.ToString());
                return TICKET_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public static bool DeleteBuyingForTicketId(long ticket_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            try
            {
                var items = from m in db.tbl_buyings
                            where m.deleted == 0 && m.ticket_id == ticket_id
                            select m;
                foreach (var item in items)
                {
                    item.deleted = 1;
                    db.SubmitChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "DeleteBuyingForTicketId()", e.ToString());
                return false;
            }
        }

        public static bool DeleteSaleForTicketId(long ticket_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            try
            {
                var items = from m in db.tbl_salelists
                            where m.deleted == 0 && m.ticket_id == ticket_id
                            select m;
                foreach (var item in items)
                {
                    item.deleted = 1;
                    db.SubmitChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "DeleteBuyingForTicketId()", e.ToString());
                return false;
            }
        }

        public static bool DeleteTicketsForShopId(long shop_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            try
            {
                var items = from m in db.tbl_tickets
                            where m.deleted == 0 && m.shop_id == shop_id
                            select m;
                foreach (var item in items)
                {
                    item.deleted = 1;
                    db.SubmitChanges();

                    DeleteBuyingForTicketId(item.id);
                    DeleteSaleForTicketId(item.id);
                }

                return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "DeleteForShopId()", e.ToString());
                return false;
            }
        }

        public List<tbl_ticket> GetTicketList(byte type, DateTime startdate, DateTime enddate)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            if (type == 4)
            {
                return db.tbl_tickets
                    .Where(m => m.deleted == 0 && m.shop_id == shop_id &&
                        m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date && (m.type == 2 || m.type == 3))
                    .OrderByDescending(m => m.id)
                    .ToList();
            }
            else
            {
                return db.tbl_tickets
                    .Where(m => m.deleted == 0 && m.shop_id == shop_id &&
                        m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date && m.type == type)
                    .OrderByDescending(m => m.id)
                    .ToList();
            }
        }

        public JqDataTableInfo GetTicketListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri,
            byte type, DateTime startdate, DateTime enddate, string search)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<TicketInfo> filteredCompanies;

            List<tbl_ticket> alllist = GetTicketList(type, startdate, enddate);

            //Check whether the companies should be filtered by keyword
            //if (!string.IsNullOrEmpty(search))
            //{
            //    //Used if particulare columns are filtered 
            //    var nameFilter = Convert.ToString(Request["sSearch_1"]);

            //    //Optionally check whether the columns are searchable at all 
            //    var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

            //    filteredCompanies = alllist
            //       .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            //}
            //else
            //{
            //    filteredCompanies = alllist;
            //}

            if (!string.IsNullOrEmpty(search))
                search = search.ToLower();
            filteredCompanies = (from m in alllist
                                 select new TicketInfo
                                 {
                                     id = m.id,
                                     ticketnum = m.ticketnum,
                                     regtime = m.regtime,
                                     customer_name = (m.customer_type == 0) ? CustomerModel.GetCustomerNameForId((long)m.customer_id) : ShopModel.GetShopNameForId((long)m.customer_id),
                                     contact_phone = (m.customer_type == 0) ? CustomerModel.GetCustomerPhoneForId((long)m.customer_id) : ShopModel.GetShopPhoneForId((long)m.customer_id),
                                     total_price = (decimal)m.total_price,
                                     real_price = (decimal)m.real_price,
                                     username = UserModel.GetUserNameForId(m.userid),
                                     type = m.type
                                 }).ToList()
                                 .Where(m => m.ticketnum.ToLower().Contains(search) || m.customer_name.ToLower().Contains(search))
                                 .ToList();


            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<TicketInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.ticketnum :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }

            var result = from c in displayedCompanies
                         select new[] { 
                c.type == 2 ? "销售开单" : "退货", 
                c.ticketnum,
                String.Format("{0:yyyy-MM-dd}", c.regtime),
                c.customer_name, 
                c.contact_phone,
                c.type == 2? Convert.ToString(c.total_price): "-" + Convert.ToString(c.total_price),
                c.type == 2? Convert.ToString(c.real_price): "-" + Convert.ToString(c.real_price),
                c.username,
                Convert.ToString(c.id)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public List<tbl_salelist> GetTicketSaleList(long ticket_id)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            return db.tbl_salelists
                .Where(m => m.deleted == 0 && m.shop_id == shop_id && m.ticket_id == ticket_id)
                .OrderByDescending(m => m.id)
                .ToList();
        }

        public JqDataTableInfo GetTicketSaleListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long ticket_id)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_salelist> filteredCompanies;

            List<tbl_salelist> alllist = GetTicketSaleList(ticket_id);

            //Check whether the companies should be filtered by keyword
            //if (!string.IsNullOrEmpty(param.sSearch))
            //{
            //    //Used if particulare columns are filtered 
            //    var nameFilter = Convert.ToString(Request["sSearch_1"]);

            //    //Optionally check whether the columns are searchable at all 
            //    var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

            //    filteredCompanies = alllist
            //       .Where(c => isNameSearchable && c.title.ToLower().Contains(param.sSearch.ToLower()));
            //}
            //else
            //{
                filteredCompanies = alllist;
            //}
            
            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<tbl_salelist, long> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.id :
                                                           0);

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }

            var ticketitem = GetTicketInfoById(ticket_id);
            string store = "";
            if(ticketitem != null)
                store = StoreModel.GetStoreNameForId((long)ticketitem.store_id);

            var result = from c in displayedCompanies
                         select new[] { 
                CatalogModel.GetCatalogNumForId(c.catalog_id), 
                store,
                CatalogModel.GetCatalogNameForId(c.catalog_id),
                CatalogModel.GetCatalogRegisteridForId(c.catalog_id),
                CatalogModel.GetCatalogProductareaForId(c.catalog_id),
                c.largenumber,
                StandardModel.GetStandardDescForId(c.standard_id),
                Convert.ToString(c.catalog_price),
                Convert.ToString(c.catalog_cnt),
                Convert.ToString(c.catalog_price * c.catalog_cnt)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public static tbl_ticket GetTicketInfoById(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            return (from m in db.tbl_tickets
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
        }

        public static string GetTicketNumById(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            var item = (from m in db.tbl_tickets
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
            if (item != null)
                return item.ticketnum;
            else
                return "";
        }

        public static long GetReceivingMoreTotalCount(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            var list = (from m in db.tbl_tickets
                        from l in db.tbl_buyings
                        where m.deleted == 0 && m.type == 0 && m.paytype == 3 && m.shop_id == shop_id && m.id == l.ticket_id && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                        select new
                        {
                            id = m.id,
                            price = l.price,
                            quantity = l.quantity
                        }).ToList();

            return list.Sum(m => m.quantity);
        }

        public static decimal GetReceivingMoreTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            var list = (from m in db.tbl_tickets
                        from l in db.tbl_buyings
                        where m.deleted == 0 && m.type == 0 && m.paytype == 3 && m.shop_id == shop_id && m.id == l.ticket_id && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                        select new
                        {
                            id = m.id,
                            price = l.price,
                            quantity = l.quantity
                        }).ToList();

            return list.Sum(m => m.price * m.quantity);
        }

        public int CheckBuying(long supply, long catalog_id, long standard_id, string largenumber, long store_id, int count)
        {
            int rst = 0;

            List<tbl_ticket> buying_list = (from m in db.tbl_tickets
                                            where m.supply_id == supply && m.type == 0 && m.deleted == 0
                                            select m).ToList();
            
            foreach (tbl_ticket m in buying_list)
            {
                List<tbl_buying> buying_data = (from l in db.tbl_buyings
                                                where l.ticket_id == m.id && l.deleted == 0
                                                select l).ToList();
                foreach (tbl_buying p in buying_data)
                {
                    if (p.catalog_id == catalog_id && p.standard_id == standard_id && p.largenumber == largenumber)
                    {
                        rst = 1;
                        break;
                    }                    
                }
                if (rst == 1)
                    break;
            }


            return rst;
        }

        public int CheckSale(byte customer_type, long customer_id, long catalog_id, long standard_id, string largenumber, int count)
        {
            int rst = 0;

            List<tbl_ticket> sale_list = (from m in db.tbl_tickets
                                            where m.customer_type == customer_type && m.type == 2 && m.customer_id == customer_id && m.deleted == 0
                                            select m).ToList();

            int sale_count = 0;

            foreach (tbl_ticket m in sale_list)
            {
                List<tbl_salelist> buying_data = (from l in db.tbl_salelists
                                                where l.ticket_id == m.id && l.deleted == 0
                                                select l).ToList();
                foreach (tbl_salelist p in buying_data)
                {
                    if (p.catalog_id == catalog_id && p.standard_id == standard_id && p.largenumber == largenumber)
                    {
                        rst = 1;
                        sale_count += (int)p.catalog_cnt;
                    }
                }
                if (rst == 1)
                    break;
            }

            if (rst == 1)
            {
                if (sale_count < count)
                    rst = 2;
            }

            return rst;
        }
    }
}
