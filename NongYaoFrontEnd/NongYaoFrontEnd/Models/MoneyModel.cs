using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region Models
    public class CreditTableInfo
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
        public string price { get; set; }
        public string name { get; set; }
    }

    public class CreditDetailInfo
    {
        public long id { get; set; }
        public string date { get; set; }
        public string ticketnum { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public decimal price { get; set; }
        public string username { get; set; }
        public string jingbanren { get; set; }
    }

    public class StatisticsTableInfo
    {
        public long id { get; set; }
        public DateTime date { get; set; }
        public byte salemethod { get; set; }
        public decimal price { get; set; }
        public string reason { get; set; }
    }

    public class StatDialogInfo
    {
        public long id { get; set; }
        public byte type { get; set; }
        public byte paytype { get; set; }
        public decimal price { get; set; }
        public string reason { get; set; }
    }

    public class BankTableInfo
    {
        public string ticket_num { get; set; }
        public DateTime date { get; set; }
        public decimal price { get; set; }
        public byte type { get; set; }
    }

    public class FinanceTableInfo
    {
        public DateTime date { get; set; }
        public decimal saleincome { get; set; }
        public decimal otherincome { get; set; }
        public decimal saleorigin { get; set; }
        public decimal otherout { get; set; }
        public decimal smallchange { get; set; }
        public decimal profit { get; set; }
    }

    public class FinanceDetailInfo
    {
        public string desc { get; set; }
        public decimal price { get; set; }
        public long user_id { get; set; }
        public string reason { get; set; }
    }
    #endregion

    public class MoneyModel
    {
        public NongYaoModelDataContext db = new NongYaoModelDataContext();

        public bool SubmitRealpay(long id, DateTime date, string cName, string cPhone, byte type, byte paytype, decimal price, decimal smallchange, string reason, long userid, long shopid)
        {
            long customer_id = 0;
            tbl_customerlist customerInfo = (from m in db.tbl_customerlists
                                            where m.deleted == 0 && m.name == cName && m.phone == cPhone
                                            select m).FirstOrDefault();
            
            if(customerInfo == null)
            {
                CustomerModel cModel = new CustomerModel();
                cModel.InsertCustomer(cName, cPhone);
                customer_id = db.tbl_customerlists
                    .Where(m => m.deleted == 0 && m.name == cName && m.phone == cPhone)
                    .Select(m => m.id)
                    .FirstOrDefault();
            }
            else
                customer_id = customerInfo.id;

            tbl_realpayment newitem = new tbl_realpayment
            {
                shop_id = shopid,
                regtime = DateTime.Now,
                userid = userid,
                customer_id = customer_id, //temp
                type = type,
                paytype = paytype,
                paymoney = price,
                change = smallchange,
                etc = reason,
                deleted = 0
            };
            db.tbl_realpayments.InsertOnSubmit(newitem);
            db.SubmitChanges();

            EarnModel eModel = new EarnModel();
            eModel.InsertEarn(id, price - smallchange, type, paytype, reason);
            
            return true;
        }

        public bool DeleteCredit(long id)
        {
            //db.Connection.Open();
           // db.Transaction = db.Connection.BeginTransaction();
            tbl_payment temp = (from m in db.tbl_payments
                                where m.id == id
                                select m).FirstOrDefault();
            temp.deleted = 1;
            db.SubmitChanges();
           // db.Transaction.Commit();
           // db.Connection.Close();
            return true;
        }

        public JqDataTableInfo GetCreditTable(JQueryDataTableParamModel param, NameValueCollection Request, DateTime start_date, DateTime end_date, string customer_name, byte type)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
           
            List<String[]> res = new List<String[]>();
            long shop_id = CommonModel.GetCurrentUserShopId();
         
            List<tbl_payment> creditInfo_tmp = (from m in db.tbl_payments
                                            where m.deleted == 0 && ((DateTime)m.regtime).Date <= end_date.Date && ((DateTime)m.regtime).Date >= start_date.Date && m.shop_id == shop_id && (type == 2 || (type !=2 && m.type == type))
                                            orderby m.regtime descending
                                            select m).ToList();

            foreach (tbl_payment eachCredit in creditInfo_tmp)
            {
                string name = "";
                if (eachCredit.ticket_type == 0 || eachCredit.ticket_type == 1) // if type is buy name is supply name~
                {
                    long supply_id = eachCredit.customer_id;
                    tbl_supply supplyInfo = (from m in db.tbl_supplies
                                             where m.deleted == 0 && m.id == supply_id
                                             select m).FirstOrDefault();
                    if(supplyInfo != null)
                    {
                        name = supplyInfo.name;
                    }
                }
                else
                {
                    if (eachCredit.customer_type == 0) //
                    {
                        long customer_id = eachCredit.customer_id;
                        tbl_customerlist customerInfo = (from m in db.tbl_customerlists
                                                         where m.deleted == 0 && m.id == customer_id
                                                         select m).FirstOrDefault();
                        if (customerInfo != null)
                        {
                            name = customerInfo.name;
                        }
                    }
                    else
                    {
                        long customer_id = eachCredit.customer_id;
                        tbl_shop shopInfo = (from m in db.tbl_shops
                                                where m.deleted == 0 && m.id == customer_id
                                                select m).FirstOrDefault();
                        if (shopInfo != null)
                        {
                            name = shopInfo.name;
                        }
                    }
                    
                }
                if (customer_name != "")
                {
                    if (name.ToLower().Contains(customer_name.ToLower()) == false)
                        continue;
                }
                decimal credit_value = 0;
                credit_value = (decimal)eachCredit.price;
                
                string str_value = "";
                if(eachCredit.type == 0) //out
                    str_value = "应付: " + Convert.ToString(credit_value) + "元";
                else
                    str_value = "应收: " + Convert.ToString(credit_value) + "元";
                var tmp = new[] { String.Format("{0:yyyy-MM-dd}", eachCredit.regtime.Date), name, str_value, Convert.ToString(eachCredit.id) };
                res.Add(tmp);
            }
            var res1 = res.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = res.Count();
            rst.iTotalDisplayRecords = res.Count();
            rst.aaData = res1;
            
            return rst;
        }

        public JqDataTableInfo GetAddHistoryTable(JQueryDataTableParamModel param, NameValueCollection Request, DateTime start_date, DateTime end_date, string customer_name, byte type)
        {
            JqDataTableInfo rst = new JqDataTableInfo();

            List<String[]> res = new List<String[]>();
            long shop_id = CommonModel.GetCurrentUserShopId();

            List<tbl_realpayment> creditInfo_tmp = (from m in db.tbl_realpayments
                                            where m.deleted == 0 && ((DateTime)m.regtime).Date <= end_date.Date && ((DateTime)m.regtime).Date >= start_date.Date && m.shop_id == shop_id && (type ==2 || type != 2 && m.type == type)
                                            select m).OrderByDescending(m => m.regtime).ToList();
            List<tbl_realpayment> creditInfo = creditInfo_tmp;
            if(customer_name != "")
            {
                creditInfo = creditInfo_tmp.Join(db.tbl_customerlists, m => m.customer_id, l => l.id, (m, l) => new { creditInfo = m, customInfo = l })
                    .Where(m => m.customInfo.name.Contains(customer_name)).Select(l => l.creditInfo).ToList();
            }
            foreach (tbl_realpayment eachCredit in creditInfo)
            {
                string name = "";
                //if (eachCredit.type == 0) // if type is buy name is supply name~
                //{
                //    long supply_id = eachCredit.customer_id;
                //    tbl_supply supplyInfo = (from m in db.tbl_supplies
                //                             where m.deleted == 0 && m.id == supply_id
                //                             select m).FirstOrDefault();
                //    if (supplyInfo != null)
                //    {
                //        name = supplyInfo.name;
                //    }
                //}
                //else
                {
                    long customer_id = eachCredit.customer_id;
                    tbl_customerlist customerInfo = (from m in db.tbl_customerlists
                                                     where m.deleted == 0 && m.id == customer_id
                                                     select m).FirstOrDefault();
                    if (customerInfo != null)
                    {
                        name = customerInfo.name;
                    }
                }

                decimal credit_value = 0;
                credit_value = (decimal)eachCredit.paymoney;
                decimal smallchange_value = 0;
                smallchange_value = (decimal)eachCredit.change;

                string str_value = "";
                if (eachCredit.type == 1) //out
                    str_value = "实收: " + Convert.ToString(credit_value) + "元";
                else
                    str_value = "实付: " + Convert.ToString(credit_value) + "元";
                var tmp = new[] { String.Format("{0:yyyy-MM-dd}", eachCredit.regtime.Date), name, str_value, Convert.ToString(smallchange_value) + "元", "" };
                res.Add(tmp);
            }
            var res1 = res.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = res.Count();
            rst.iTotalDisplayRecords = res.Count();
            rst.aaData = res1;

            return rst;
        }
        public JqDataTableInfo GetDetailCreditTable(JQueryDataTableParamModel param, NameValueCollection Request, long credit_id)
        {
            JqDataTableInfo rst = new JqDataTableInfo();

            List<String[]> res = new List<String[]>();
            CreditDetailInfo detailInfo = new CreditDetailInfo();
            tbl_payment payInfo = (from m in db.tbl_payments
                                   where m.deleted == 0 && m.id == credit_id
                                   select m).FirstOrDefault();
            if (payInfo != null)
            {
                detailInfo.date = String.Format("{0:yyyy-MM-dd}", payInfo.regtime.Date);

                if(payInfo.ticket_type == 0)
                    detailInfo.content = "采购进货";
                else if(payInfo.ticket_type == 1)
                    detailInfo.content = "采购退货";
                else if(payInfo.ticket_type == 2)
                    detailInfo.content = "销售开单";
                else if(payInfo.ticket_type == 3)
                    detailInfo.content = "销售退货";
                
                if (payInfo.type == 1)
                    detailInfo.type = "应收";
                else
                    detailInfo.type = "应付";

                detailInfo.price = payInfo.price;
                detailInfo.ticketnum = db.tbl_tickets
                    .Where(m => m.deleted == 0 && m.id == payInfo.ticket_id)
                    .Select(m => m.ticketnum).FirstOrDefault();

                var tmp = new[] { detailInfo.date, detailInfo.ticketnum, detailInfo.content, detailInfo.type, Convert.ToString(detailInfo.price), detailInfo.jingbanren, "" };
                res.Add(tmp);
            }
            
            var res1 = res.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = res.Count();
            rst.iTotalDisplayRecords = res.Count();
            rst.aaData = res1;
            return rst;
        }
        
        //public CreditDetailInfo GetOneCredit(long id)
        //{
        //    CreditDetailInfo detailInfo = new CreditDetailInfo();
        //    tbl_payment payInfo = (from m in db.tbl_payments
        //                               where m.deleted == 0 && m.id == id
        //                               select m).FirstOrDefault();
        //    if(payInfo != null)
        //    {
        //        detailInfo.date = String.Format("{0:yyyy-MM-dd}", payInfo.regtime.Date);
        //        detailInfo.content = "采购进货"; //temp
        //        if (payInfo.type == 0)
        //            detailInfo.type = "应收";
        //        else
        //            detailInfo.type = "应付";
        //        detailInfo.price = payInfo.price;
                
        //        if (payInfo.ticket_type == 0 || payInfo.ticket_type == 1)
        //        {
        //            detailInfo.ticketnum = db.tbl_buyings
        //                .Where(m => m.deleted == 0 && m.id == payInfo.ticket_id)
        //                .Join(db.tbl_tickets, m => m.ticket_id, l => l.id, (m, l) => new { payInfo = m, ticketInfo = l })
        //                .Select(m => m.ticketInfo.ticketnum).FirstOrDefault();
        //        }
        //        else
        //        {
        //            detailInfo.ticketnum = db.tbl_salelists
        //                .Where(m => m.deleted == 0 && m.id == payInfo.ticket_id)
        //                .Join(db.tbl_tickets, m => m.ticket_id, l => l.id, (m, l) => new { payInfo = m, ticketInfo = l })
        //                .Select(m => m.ticketInfo.ticketnum).FirstOrDefault();
        //        }
        //    }
        //    //CreditDetailInfo info = db.tbl_payments
        //    //    .Where(p => p.deleted == 0 && p.id == id)
        //    //    .Join(db.tbl_customerlists, m => m.customer_id, l => l.id, (m, l) => new { crinfo = m, customer = l })
        //    //    .Select(row => new CreditDetailInfo
        //    //    {
        //    //        id = row.crinfo.id,
        //    //        date = Convert.ToString(row.crinfo.regtime),
        //    //        ticketnum = row.crinfo.ti
        //    //        username = row.customer.name,
        //    //        handphonenum = row.customer.phone,
        //    //        price = (decimal)row.crinfo.price,
        //    //        type = row.crinfo.type,
        //    //    }).FirstOrDefault();

        //    return detailInfo;
        //}

        //GetStatisticsTable
        public JqDataTableInfo GetStatisticsTable(JQueryDataTableParamModel param, NameValueCollection Request, byte search_type, DateTime start_date, DateTime end_date)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            List<StatisticsTableInfo> statInfo = new List<StatisticsTableInfo>();
            List<String[]> res = new List<String[]>();
            long shop_id = CommonModel.GetCurrentUserShopId();
            if (search_type == 2)
            {
                statInfo = db.tbl_otherpays
                    .Where(p => p.deleted == 0 && p.shop_id == shop_id && p.regtime.Date >= start_date.Date && p.regtime.Date <= end_date.Date)
                    .OrderByDescending(p => p.id)
                    .Select(row => new StatisticsTableInfo

                    {
                        id = row.id,
                        date = (DateTime)row.regtime,
                        salemethod = row.type,
                        price = (decimal)row.price,
                        reason = row.reason
                    }).ToList();
            }
            else
            {
                statInfo = db.tbl_otherpays
                    .Where(p => p.deleted == 0 && p.type == search_type && p.shop_id == shop_id && p.regtime.Date >= start_date.Date && p.regtime.Date <= end_date.Date)
                    .OrderByDescending(p => p.id)
                    .Select(row => new StatisticsTableInfo

                    {
                        id = row.id,
                        date = (DateTime)row.regtime,
                        salemethod = row.type,
                        price = (decimal)row.price,
                        reason = row.reason
                    }).ToList();
            }

            foreach (StatisticsTableInfo item in statInfo)
            {
                var tmp = new[] { 
                    String.Format("{0:yyyy-MM-dd}", item.date), 
                    Convert.ToString(((item.salemethod == 1) ? "收入" : "支出")), 
                    Convert.ToString(item.price + "元"), 
                    Convert.ToString(item.reason), 
                    Convert.ToString(item.id) };
                res.Add(tmp);
            }

            var res1 = res.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = res.Count();
            rst.iTotalDisplayRecords = res.Count();
            rst.aaData = res1;
            return rst;
        }

        public StatDialogInfo GetOneStatistics(long id)
        {
            StatDialogInfo info = db.tbl_otherpays
                .Where(p => p.deleted == 0 && p.id == id)
                //.Join(db.tbl_customerlists, m => m.customer_id, l => l.id, (m, l) => new { crinfo = m, customer = l })
                .Select(row => new StatDialogInfo
                {
                    id = row.id,
                    type = row.type,
                    paytype = row.paytype,
                    price = (decimal)row.price,
                    reason = row.reason
                }).FirstOrDefault();
            return info;
        }
        public bool DeleteStatistics(long id)
        {
            //db. .Open();
            //db.Transaction = db.Connection.BeginTransaction();
            tbl_otherpay temp = (from m in db.tbl_otherpays
                                where m.id == id
                                select m).FirstOrDefault();
            temp.deleted = 1;
            db.SubmitChanges();
            //db.Transaction.Commit();
         //   db.Connection.Close();
            return true;
        }
        public bool SubmitStatistics(long id, byte type, byte paytype, decimal price, string reason, long userid, long shopid)
        {
            
            tbl_otherpay temp = (from m in db.tbl_otherpays
                                 where m.id == id
                                select m).FirstOrDefault();
            
            if (temp == null) // this is insert case
            {
                long nuserRegisterId = 0;
                List<tbl_otherpay> paylist = (from m in db.tbl_otherpays
                                                select m).ToList();
                foreach (tbl_otherpay u in paylist)
                {
                    if (nuserRegisterId < u.id)
                        nuserRegisterId = u.id;

                }
                nuserRegisterId++;
                tbl_otherpay newpay = new tbl_otherpay
                {
                    id = nuserRegisterId,
                    userid = userid,
                    type = type,
                    paytype = paytype,
                    price = price,
                    reason = reason,
                    shop_id = shopid,
                    regtime = DateTime.Now,
                    deleted = 0,
                };
                db.tbl_otherpays.InsertOnSubmit(newpay);
            }
            else  //this is update case
            {
                temp.type = type;
                temp.paytype = paytype;
                temp.price = price;                
                temp.reason = reason;
                temp.deleted = 0;   
            }

            db.SubmitChanges();
            return true;
        }

        public static decimal GetTotalShouZhi(int type, DateTime start_date, DateTime end_date)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();
            var paylist = db.tbl_otherpays
                    .Where(p => p.deleted == 0 && p.type == type && p.shop_id == shop_id && p.regtime.Date >= start_date.Date && p.regtime.Date <= end_date.Date)
                    .Select(p => new
                    {
                        price = (p.price == null) ? 0 : (decimal)p.price
                    });
           decimal rst = 0;
           if(paylist.Count() > 0)
               rst = paylist.Sum(p => p.price);

            return rst;
        }        

        //public JqDataTableInfo GetBankTable(JQueryDataTableParamModel param, NameValueCollection Request,DateTime start_date,DateTime end_date)
        //{
            
        //    JqDataTableInfo rst = new JqDataTableInfo();
        //    List<BankTableInfo> bankInfo = new List<BankTableInfo>();
        //    List<String[]> res = new List<String[]>();
        //    /*
        //    bankInfo = db.tbl_banklists
        //        .OrderByDescending(p => p.id)
        //        .Where(p => p.deleted == 0 && p.regtime>=start_date && p.regtime <=end_date)
        //        .Join(db.tbl_tickets, m=>m.ticket_id , l=>l.id, (m,l)=>new{bank = m, ticket=l})
        //        .Select(row => new BankTableInfo

        //        {
        //            ticket_num = row.ticket.ticketnum,
        //            date = (DateTime)row.bank.regtime,
        //            price = (decimal)row.bank.price,
        //            type = row.bank.type
        //        }).ToList();

        //    foreach (BankTableInfo item in bankInfo)
        //    {
        //        var income = item.price;
        //        var export = item.price;
        //        if (item.type == 1)
        //        {
        //            income = item.price;
        //            export = 0;
        //        }
        //        else
        //        {
        //            export = item.price;
        //            income = 0;
        //        }
        //        var tmp = new[] { Convert.ToString(item.ticket_num), String.Format("{0:yyyy-MM-dd}",item.date), Convert.ToString(income), Convert.ToString(export)};
        //        res.Add(tmp);
        //    }*/
        //    var res1 = res.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();

        //    rst.sEcho = param.sEcho;
        //    rst.iTotalRecords = res.Count();
        //    rst.iTotalDisplayRecords = res.Count();
        //    rst.aaData = res1;
        //    return rst;
        //}

        public JqDataTableInfo GetFinanceTable(JQueryDataTableParamModel param, NameValueCollection Request, DateTime start_date, DateTime end_date)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            JqDataTableInfo rst = new JqDataTableInfo();
            
            List<String[]> res = new List<String[]>();
            long shop_id = CommonModel.GetCurrentUserShopId();
            for (DateTime cur_date = end_date; cur_date >= start_date; cur_date = cur_date.AddDays(-1))
            {
                FinanceTableInfo financeInfo = new FinanceTableInfo();
                List<tbl_remain> remaininfo = (from m in db.tbl_remains
                                               where m.deleted == 0 && ((DateTime)m.regtime).Date == cur_date.Date && m.shop_id == shop_id
                                               select m).ToList();
                decimal saleincome = 0, saleorigin = 0;
                for (var i = 0; i < remaininfo.Count(); i++)
                {
                    if (remaininfo[i].type == 2 || remaininfo[i].type == 3)
                    {
                        if (remaininfo[i].type == 2)
                        {
                            saleincome += (decimal)remaininfo[i].sum_price;     //sell
                            saleorigin += remaininfo[i].origin_price * remaininfo[i].catalog_cnt;
                        }
                        else if (remaininfo[i].type == 3)
                        {
                            saleincome -= (decimal)remaininfo[i].sum_price;     //sellback
                            saleorigin -= remaininfo[i].origin_price * remaininfo[i].catalog_cnt;
                        }
                    }
                }
                
                decimal otherincome = (from m in db.tbl_otherpays
                                       where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id && m.type == 1
                                       select m).ToList().Sum(m => (decimal)m.price);
                decimal otherout = (from m in db.tbl_otherpays
                                    where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id && m.type == 0
                                    select m).ToList().Sum(m => (decimal)m.price);

                decimal smallchange = (from m in db.tbl_changes
                                       where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id
                                       select m).ToList().Sum(m => (decimal)m.change);

                financeInfo.date = cur_date;
                financeInfo.saleincome = saleincome;
                financeInfo.otherincome = otherincome;
                financeInfo.saleorigin = saleorigin;
                financeInfo.otherout = otherout;
                financeInfo.smallchange = smallchange;
                financeInfo.profit = financeInfo.saleincome - financeInfo.saleorigin + otherincome - otherout - smallchange;
                if (financeInfo.saleincome != 0 || financeInfo.saleorigin != 0 || financeInfo.otherincome != 0 || financeInfo.otherout != 0 || financeInfo.smallchange != 0)
                {
                    var tmp = new[] { 
                        String.Format("{0:yyyy-MM-dd}", financeInfo.date), 
                        Convert.ToString(financeInfo.saleincome) + "元", 
                        Convert.ToString(financeInfo.otherincome) + "元", 
                        Convert.ToString(financeInfo.saleorigin) + "元", 
                        Convert.ToString(financeInfo.otherout) + "元", 
                        Convert.ToString(financeInfo.smallchange) + "元", 
                        Convert.ToString(financeInfo.profit) + "元", 
                        String.Format("{0:yyyy-MM-dd}", financeInfo.date) };
                    res.Add(tmp);
                }
            }
            

            var res1 = res.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            
            rst.sEcho = param.sEcho;
            rst.iTotalRecords = res.Count();
            rst.iTotalDisplayRecords = res.Count();
            rst.aaData = res1;
            return rst;
        }

        public List<FinanceDetailInfo> GetFinanceDetailList(DateTime detaildate)
        {
            List<FinanceDetailInfo> retlist = new List<FinanceDetailInfo>();

            long shop_id = CommonModel.GetCurrentUserShopId();
            var sales = (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && (m.type == 2 || m.type == 3) && m.regtime.Date == detaildate.Date
                    group m by m.user_id into g
                    select new FinanceDetailInfo {
                        desc = "销售收入",
                        price = g.ToList().Sum(m => (m.type == 2) ? m.sum_price : -m.sum_price),
                        user_id = g.Key,
                        reason = "",
                    }).ToList();
            retlist = retlist.Union(sales).ToList();

            var saleorigins = (from m in db.tbl_remains
                    where m.deleted == 0 && m.shop_id == shop_id && (m.type == 2 || m.type == 3) && m.regtime.Date == detaildate.Date
                    group m by m.user_id into g
                    select new FinanceDetailInfo {
                        desc = "销售成本",
                        price = g.ToList().Sum(m => (m.type == 2) ? m.catalog_cnt * m.origin_price : -m.catalog_cnt * m.origin_price),
                        user_id = g.Key,
                        reason = "",
                    }).ToList();
            retlist = retlist.Union(saleorigins).ToList();

            var otherpays = (from m in db.tbl_otherpays
                            where m.deleted == 0 && m.regtime.Date == detaildate.Date && m.shop_id == shop_id
                            select new FinanceDetailInfo
                            {
                                desc =  m.type == 1 ? "其他收入" : "其他费用",
                                price = m.price,
                                user_id = m.userid,
                                reason = m.reason,
                            }).ToList();
            retlist = retlist.Union(otherpays).ToList();

            var smallchanges = (from m in db.tbl_changes
                             where m.deleted == 0 && m.regtime.Date == detaildate.Date && m.shop_id == shop_id
                             select new FinanceDetailInfo
                             {
                                 desc = "找零金额",
                                 price = (decimal)m.change,
                                 user_id = m.userid,
                                 reason = "",
                             }).ToList();
            retlist = retlist.Union(smallchanges).ToList();


            return retlist;
        }

        public JqDataTableInfo GetFinanceDetailTable(JQueryDataTableParamModel param, NameValueCollection Request, DateTime detaildate)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<FinanceDetailInfo> filteredCompanies;

            List<FinanceDetailInfo> alllist = GetFinanceDetailList(detaildate);

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

            //var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            //var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<tbl_salelist, long> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.id :
            //                                               0);

            //var sortDirection = Request["sSortDir_0"]; // asc or desc
            //if (sortDirection == "asc")
            //    filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            //else
            //    filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart);
            if (param.iDisplayLength > 0)
            {
                displayedCompanies = displayedCompanies.Take(param.iDisplayLength);
            }

            int i = 1;
            var result = from c in displayedCompanies
                         select new[] { 
                Convert.ToString(i ++),
                c.desc,
                String.Format("{0:0.00元}", c.price),
                UserModel.GetUserNameForId(c.user_id),
                c.reason
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }
        

        public string GetFinanceTotal(JQueryDataTableParamModel param, NameValueCollection Request, DateTime start_date, DateTime end_date)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            decimal profit = 0,total = 0;
            try
            {
                for (DateTime cur_date = start_date; cur_date <= end_date; cur_date = cur_date.AddDays(1))
                {
                    List<tbl_remain> remaininfo = (from m in db.tbl_remains
                                                   where m.deleted == 0 && ((DateTime)m.regtime).Date == cur_date.Date && m.shop_id == shop_id
                                                   select m).ToList();
                    decimal saleincome = 0, saleorigin = 0;
                    for (var i = 0; i < remaininfo.Count(); i++)
                    {
                        if (remaininfo[i].type == 2 || remaininfo[i].type == 3)
                        {
                            if (remaininfo[i].type == 2)
                            {
                                saleincome += (decimal)remaininfo[i].sum_price;     //sell
                                saleorigin += remaininfo[i].origin_price * remaininfo[i].catalog_cnt;
                            }
                            else if (remaininfo[i].type == 3)
                            {
                                saleincome -= (decimal)remaininfo[i].sum_price;     //sellback
                                saleorigin -= remaininfo[i].origin_price * remaininfo[i].catalog_cnt;
                            }
                        }
                    }

                    decimal otherincome = (from m in db.tbl_otherpays
                                           where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id && m.type == 1
                                           select m).ToList().Sum(m => (decimal)m.price);
                    decimal otherout = (from m in db.tbl_otherpays
                                        where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id && m.type == 0
                                        select m).ToList().Sum(m => (decimal)m.price);

                    decimal smallchange = (from m in db.tbl_changes
                                           where m.deleted == 0 && m.regtime.Date == cur_date.Date && m.shop_id == shop_id
                                           select m).ToList().Sum(m => (decimal)m.change);                   

                    profit = saleincome + otherincome - saleorigin - otherout - smallchange;
                    total += profit;
                }
            }
            catch (Exception ex)
            {
                CommonModel.WriteLogFile("UserModel", "DeleteForShopId()", ex.ToString());
                return "";
            }
           
            
            return Convert.ToString(total);
        }

        public static bool DeletePaymentsForShopId(long shop_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            try
            {
                var items = from m in db.tbl_payments
                            where m.deleted == 0 && m.shop_id == shop_id
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
                CommonModel.WriteLogFile("UserModel", "DeleteForShopId()", e.ToString());
                return false;
            }
        }

        public static bool DeleteOtherpaysForShopId(long shop_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            try
            {
                var items = from m in db.tbl_otherpays
                            where m.deleted == 0 && m.shop_id == shop_id
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
                CommonModel.WriteLogFile("UserModel", "DeleteForShopId()", e.ToString());
                return false;
            }
        }

        public static bool DeleteBankingsForShopId(long shop_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            /*
            try
            {
                var items = from m in db.tbl_banklists
                            where m.deleted == 0 && m.shop_id == shop_id
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
                CommonModel.WriteLogFile("UserModel", "DeleteForShopId()", e.ToString());
                return false;
            }*/
            return true;
        }

        public static decimal GetShoyruTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_payments
                    where m.deleted == 0 && m.type == 1 && m.shop_id == shop_id && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.price);
        }

        public static decimal GetZhichuTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_payments
                    where m.deleted == 0 && m.type == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => m.price);
        }

        public static decimal GetSmallchangeTotalPrice(DateTime startdate, DateTime enddate)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();

            return (from m in db.tbl_changes
                    where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate.Date && m.regtime.Date <= enddate.Date
                    select m).ToList().Sum(m => (decimal)m.change);

            //return (from m in db.tbl_otherpays
            //        where m.deleted == 0 && m.type == 0 && m.shop_id == shop_id && m.reason == "销售的找零" && m.regtime.Date >= startdate && m.regtime.Date <= enddate
            //        select m).ToList().Sum(m => m.price);
        }

        public bool InsertCredit(long supply_id, byte type, decimal price, byte ticket_type, long ticket_id, byte customer_type)
        {
            //db.Connection.Open();
            //db.Transaction = db.Connection.BeginTransaction();

               
            long nuserRegisterId = 0;
            
            tbl_payment newpay = new tbl_payment
            {
                customer_id = supply_id,
                 userid = CommonModel.GetCurrentUserId(),
                 type = type,
                 price = price,
                 shop_id = CommonModel.GetCurrentUserShopId(),
                 ticket_type = ticket_type,
                 ticket_id = ticket_id,
                 regtime = DateTime.Now,
                 customer_type = customer_type,
                 deleted = 0,
            };
            db.tbl_payments.InsertOnSubmit(newpay);
            db.SubmitChanges();
            //db.Transaction.Commit();
            //db.Connection.Close();
            return true;
        }

        public bool InsertChange(decimal change)
        {
            tbl_change item = new tbl_change();

            item.shop_id = CommonModel.GetCurrentUserShopId();
            item.userid = CommonModel.GetCurrentUserId();
            item.regtime = DateTime.Now;
            item.change = change;
            item.deleted = 0;

            db.tbl_changes.InsertOnSubmit(item);
            db.SubmitChanges();

            return true;
        }

        public long GetLastInsertedId()
        {
            var item = (from m in db.tbl_otherpays
                        where m.deleted == 0
                        orderby m.id descending
                        select m).FirstOrDefault();

            if (item != null)
                return item.id;
            else
                return 0;
        }
    }

}