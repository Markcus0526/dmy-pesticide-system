using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region Models
    public class EARN_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "重复";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
        public const string OTHERPAY_NOEXIST = "其他财务情报没存在";
    }

    public class MoneyBankInfo
    {
        public DateTime date { get; set; }
        public decimal? money_profit { get; set; }
        public decimal? bank_profit { get; set; }
        public decimal? money_in { get; set; }
        public decimal? money_out { get; set; }
    }

    public class MoneyBankDetailInfo
    {
        public long id { get; set; }
        public long ticket_id { get; set; }
        public byte paytype { get; set; }
        public byte type { get; set; }
        public decimal price { get; set; }
        public string reason { get; set; }
    }
    #endregion

    public class EarnModel
    {
        NongYaoModelDataContext db = new NongYaoModelDataContext();

        public string InsertEarn(
            long ticket_id,
            decimal price,
            byte type,
            byte paytype,
            string reason
            )
        {
            try
            {
                tbl_earnlist newitem = new tbl_earnlist();
                if(ticket_id != 0)
                    newitem.ticket_id = ticket_id;
                newitem.price = price;
                newitem.type = type;
                newitem.paytype = paytype;
                newitem.reason = reason;
                newitem.userid = CommonModel.GetCurrentUserId();
                newitem.shop_id = CommonModel.GetCurrentUserShopId();
                newitem.regtime = DateTime.Now;

                if (paytype == 0)
                {
                    decimal money_profit = GetLastMoneyProfit();
                    if (type == 0 /*&& type == 3*/)
                        money_profit -= price;
                    else
                        money_profit += price;
                    newitem.money_profit = money_profit;
                    newitem.bank_profit = GetLastBankProfit();
                }
                else if (paytype == 2)
                {
                    decimal bank_profit = GetLastBankProfit();
                    if (type == 0 /*&& type == 3*/)
                        bank_profit -= price;
                    else
                        bank_profit += price;
                    newitem.bank_profit = bank_profit;
                    newitem.money_profit = GetLastMoneyProfit();
                }

                db.tbl_earnlists.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return EARN_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TicketModel", "InsertBank()", e.ToString());
                return EARN_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public string InsertOtherpayEarn(
            long otherpay_id,
            decimal price,
            byte type,
            byte paytype,
            string reason
            )
        {
            try
            {
                tbl_earnlist newitem = new tbl_earnlist();

                newitem.otherpay_id = otherpay_id;
                newitem.price = price;
                newitem.type = type;
                newitem.paytype = paytype;
                newitem.reason = reason;
                newitem.userid = CommonModel.GetCurrentUserId();
                newitem.shop_id = CommonModel.GetCurrentUserShopId();
                newitem.regtime = DateTime.Now;

                if (paytype == 0)
                {
                    decimal money_profit = GetLastMoneyProfit();
                    if (type == 0/* && type == 3*/)
                        money_profit -= price;
                    else
                        money_profit += price;
                    newitem.money_profit = money_profit;
                    newitem.bank_profit = GetLastBankProfit();
                }
                else if (paytype == 2)
                {
                    decimal bank_profit = GetLastBankProfit();
                    if (type == 0/* && type == 3*/)
                        bank_profit -= price;
                    else
                        bank_profit += price;
                    newitem.bank_profit = bank_profit;
                    newitem.money_profit = GetLastMoneyProfit();
                }

                db.tbl_earnlists.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return EARN_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TicketModel", "InsertBank()", e.ToString());
                return EARN_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public string UpdateOtherpayEarn(
            long otherpay_id,
            decimal price,
            byte type,
            byte paytype,
            string reason
            )
        {
            try
            {
                tbl_earnlist edititem = (from m in db.tbl_earnlists
                                         where m.deleted == 0 && m.otherpay_id == otherpay_id
                                         select m).FirstOrDefault();

                if (edititem != null)
                {
                    edititem.type = type;
                    edititem.paytype = paytype;
                    edititem.reason = reason;

                    decimal prev_money_profit = (decimal)edititem.money_profit, prev_bank_profit = (decimal)edititem.bank_profit;

                    if (paytype == 0)
                    {
                        decimal money_profit = GetLastMoneyProfitThan(edititem.id);
                        if (type == 0 && type == 3)
                            money_profit -= price;
                        else
                            money_profit += price;
                        edititem.money_profit = money_profit;
                        edititem.bank_profit = GetLastBankProfitThan(edititem.id);
                    }
                    else if (paytype == 2)
                    {
                        decimal bank_profit = GetLastBankProfitThan(edititem.id);
                        if (type == 0 && type == 3)
                            bank_profit -= price;
                        else
                            bank_profit += price;
                        edititem.bank_profit = bank_profit;
                        edititem.money_profit = GetLastMoneyProfitThan(edititem.id);
                    }
                    edititem.price = price;

                    db.SubmitChanges();

                    if (edititem.money_profit != prev_money_profit || edititem.bank_profit != prev_bank_profit)
                    {
                        decimal money_diff = (decimal)edititem.money_profit - prev_money_profit;
                        decimal bank_diff = (decimal)edititem.bank_profit - prev_bank_profit;
                        UpdateProfitFromId(edititem.id, money_diff, bank_diff);
                    }

                    return EARN_SUBMITSTATUS.SUCCESS_SUBMIT;
                }
                else
                {
                    return EARN_SUBMITSTATUS.OTHERPAY_NOEXIST;
                }                
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TicketModel", "InsertBank()", e.ToString());
                return EARN_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public decimal GetLastMoneyProfit()
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var item = (from m in db.tbl_earnlists
                        where m.deleted == 0 && m.shop_id == shop_id
                        orderby m.id descending
                        select m).FirstOrDefault();
            if (item != null && item.money_profit != null)
                return (decimal)item.money_profit;
            else
                return 0;
        }

        public decimal GetLastBankProfit()
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var item = (from m in db.tbl_earnlists
                        where m.deleted == 0 && m.shop_id == shop_id
                        orderby m.id descending
                        select m).FirstOrDefault();
            if (item != null && item.bank_profit != null)
                return (decimal)item.bank_profit;
            else
                return 0;
        }

        public decimal GetLastMoneyProfitThan(long id)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var item = (from m in db.tbl_earnlists
                        where m.deleted == 0 && m.shop_id == shop_id && m.id < id
                        orderby m.id descending
                        select m).FirstOrDefault();
            if (item != null && item.money_profit != null)
                return (decimal)item.money_profit;
            else
                return 0;
        }

        public decimal GetLastBankProfitThan(long id)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var item = (from m in db.tbl_earnlists
                        where m.deleted == 0 && m.shop_id == shop_id && m.id < id
                        orderby m.id descending
                        select m).FirstOrDefault();
            if (item != null && item.bank_profit != null)
                return (decimal)item.bank_profit;
            else
                return 0;
        }

        public List<MoneyBankInfo> GetMoneyBankList(DateTime startdate, DateTime enddate)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            var ret = (from m in db.tbl_earnlists
                       where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date >= startdate && m.regtime.Date <= enddate
                       group m by m.regtime.Date into g
                       select new MoneyBankInfo { 
                        date = g.Key,
                        money_profit = g.OrderByDescending(l => l.id).FirstOrDefault().money_profit,
                        bank_profit = g.OrderByDescending(l => l.id).FirstOrDefault().bank_profit,
                        money_in = g.Where(m => m.type == 1).Sum(m => m.price),
                        money_out = g.Where(m => m.type == 0).Sum(m => m.price)
                       }).ToList();

            return ret;
        }

        public JqDataTableInfo GetMoneyBankDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, DateTime startdate, DateTime enddate)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<MoneyBankInfo> filteredCompanies;

            List<MoneyBankInfo> alllist = GetMoneyBankList(startdate, enddate);
            filteredCompanies = alllist;

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


            //var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            //var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<TicketInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.ticketnum :
            //                                               "");

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

            var result = from c in displayedCompanies
                         select new[] { 
                String.Format("{0:yyyy-MM-dd}",c.date),
                Convert.ToString(c.money_profit),
                Convert.ToString(c.bank_profit),
                Convert.ToString((c.money_profit != null ? c.money_profit : 0) + (c.bank_profit != null ? c.bank_profit : 0)),
                "",
                Convert.ToString(c.money_in != null ? c.money_in : 0),
                Convert.ToString(c.money_out != null ? c.money_out : 0)
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public List<MoneyBankDetailInfo> GetMoneyBankDetailList(DateTime detaildate, byte paytype, byte moneytype)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            List<MoneyBankDetailInfo> ret = null;
            if (paytype == 4)
            {
                ret = (from m in db.tbl_earnlists
                       where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date == detaildate.Date && (moneytype == 2 || (moneytype != 2 && m.type == moneytype))
                       select new MoneyBankDetailInfo
                       {
                           id = m.id,
                           ticket_id = m.ticket_id != null ? (long)m.ticket_id : 0,
                           paytype = m.paytype,
                           type = m.type,
                           price = m.price,
                           reason = m.reason
                       }).ToList();
            }
            else
            {
                ret = (from m in db.tbl_earnlists
                       where m.deleted == 0 && m.shop_id == shop_id && m.regtime.Date == detaildate.Date && m.paytype == paytype && (moneytype == 2 || (moneytype != 2 && m.type == moneytype))
                       select new MoneyBankDetailInfo
                       {
                           id = m.id,
                           ticket_id = m.ticket_id != null ? (long)m.ticket_id : 0,
                           paytype = m.paytype,
                           type = m.type,
                           price = m.price,
                           reason = m.reason
                       }).ToList();
            }

            return ret;
        }

        public JqDataTableInfo GetMoneyBankDetailDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, DateTime detaildate, byte paytype, byte moneytype)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<MoneyBankDetailInfo> filteredCompanies;

            List<MoneyBankDetailInfo> alllist = GetMoneyBankDetailList(detaildate, paytype, moneytype);
            filteredCompanies = alllist;

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


            //var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            //var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            //Func<TicketInfo, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.ticketnum :
            //                                               "");

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

            var result = from c in displayedCompanies
                         select new[] { 
                TicketModel.GetTicketNumById(c.ticket_id),
                (c.paytype == 0) ? "现金收入" : "银行收入",
                (c.type == 0) ? "支出" : "收入",
                Convert.ToString(c.price),
                c.reason
            };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }

        public string UpdateProfitFromId(long id, decimal money_diff, decimal bank_diff)
        {
            long shop_id = CommonModel.GetCurrentUserShopId();
            try
            {
                var itemlist = (from m in db.tbl_earnlists
                                         where m.deleted == 0 && m.shop_id == shop_id && m.id > id
                                         select m).ToList();
                foreach (tbl_earnlist item in itemlist)
                {
                    item.money_profit = item.money_profit + money_diff;
                    item.bank_profit = item.bank_profit + bank_diff;

                    db.SubmitChanges();
                }

                return EARN_SUBMITSTATUS.SUCCESS_SUBMIT;                
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TicketModel", "InsertBank()", e.ToString());
                return EARN_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }
    }
}
