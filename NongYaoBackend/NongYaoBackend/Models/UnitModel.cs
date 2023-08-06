using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Specialized;
using NongYaoBackend.Models.Library;

namespace NongYaoBackend.Models
{
    #region unit model
    public class Unit
    {
        public string id { get; set; }
        public string name { get; set; }
        public long index { get; set; }
    }
    
    public class UNIT_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "操作失败： 单位重复！";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
    }

    #endregion
    public class UnitModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static JqDataTableInfo GetListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<Unit> filteredCompanies;

            var alllist = GetUnitList();
            filteredCompanies = alllist;

            var displayedUnit = filteredCompanies.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedUnit
                         select new[] {
                             Convert.ToString(c.index),
                             Convert.ToString(c.name),
                             Convert.ToString(c.id)
                         };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = alllist.Count();
            rst.aaData = result;

            return rst;
        }

        public static IEnumerable<Unit> GetUnitList()
        {
            List<Unit> retList = null;
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            retList = db.tbl_units
                .Where(p => p.deleted == 0)
                .Select(row => new Unit
                    {
                        id = Convert.ToString(row.id),
                        name = Convert.ToString(row.name),
                    }).ToList();
            long i = 1;
            foreach (Unit m in retList)
            {
                m.index = i;
                i++;
            }
            return retList;
        }

        public static List<tbl_unit> GetUnitListFull()
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            return (from m in db.tbl_units
                    where m.deleted == 0
                    select m).ToList();
        }

        public string InsertUnit(string name)
        {
            try
            {
                tbl_unit newitem = new tbl_unit();

                newitem.name = name;
                newitem.regtime = DateTime.Now;
                newitem.deleted = 0;

                db.tbl_units.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return UNIT_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UnitModel", "InsertUnit()", e.ToString());
                return UNIT_SUBMITSTATUS.ERROR_SUBMIT;
            }

        }

        public string UpdateUnit(long id, string name)
        {
            try
            {
                var edititem = (from m in db.tbl_units
                                where m.deleted == 0 && m.id == id
                                select m).FirstOrDefault();
                if (edititem != null)
                {
                    edititem.name = name;
                    db.SubmitChanges();

                    return UNIT_SUBMITSTATUS.SUCCESS_SUBMIT;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UnitModel", "UpdateUnit()", e.ToString());
                return UNIT_SUBMITSTATUS.ERROR_SUBMIT;
            }

            return UNIT_SUBMITSTATUS.ERROR_SUBMIT;
        }

        public string CheckUnit(string name, long id)
        {
            try
            {
                tbl_unit unit_data = (from m in db.tbl_units
                                 where m.name == name && m.id != id && m.deleted == 0
                                 select m).FirstOrDefault();
                if (unit_data == null)
                    return UNIT_SUBMITSTATUS.SUCCESS_SUBMIT;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UnitModel", "CheckUnit", e.ToString());
                return UNIT_SUBMITSTATUS.ERROR_SUBMIT;
            }

            return UNIT_SUBMITSTATUS.DUPLICATE_NAME;
        }

        public bool DeleteUnit(long id)
        {
            try
            {
                var delitem = (from m in db.tbl_units
                               where m.deleted == 0 && m.id == id
                               select m).FirstOrDefault();

                if (delitem != null)
                {
                    delitem.deleted = 1;
                    db.SubmitChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UnitModel", "DeleteUnit()", e.ToString());
                return false;
            }

            return false;
        } 

    }
}