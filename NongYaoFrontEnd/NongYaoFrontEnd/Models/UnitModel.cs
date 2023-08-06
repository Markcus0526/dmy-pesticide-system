using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region unit model
    public class UnitInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public long index { get; set; }
    }
    #endregion
    public class UnitModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static JqDataTableInfo GetListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<UnitInfo> filteredCompanies;

            var alllist = GetUnitList();
            filteredCompanies = alllist;

            var displayedUnit = filteredCompanies.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedUnit
                         select new[] {
                             Convert.ToString(c.index),
                             Convert.ToString(c.name),
                             Convert.ToString(c.unit),
                             Convert.ToString(c.id)
                         };

            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = alllist.Count();
            rst.aaData = result;

            return rst;
        }

        public static IEnumerable<UnitInfo> GetUnitList()
        {
            List<UnitInfo> retList = null;
            
            retList = db.tbl_units
                .Where(p => p.deleted == 0)
                .Select(row => new UnitInfo
                    {
                        id = Convert.ToString(row.id),
                        name = Convert.ToString(row.name),
                        unit = Convert.ToString(row.unit)
                    }).ToList();
            long i = 1;
            foreach (UnitInfo m in retList)
            {
                m.index = i;
                i++;
            }
            return retList;
        }

        public static List<tbl_unit> GetUnitListFull()
        {
            return (from m in db.tbl_units
                    where m.deleted == 0
                    select m).ToList();
        }
        
        public static tbl_unit GetUnitInfo(long id)
        {
            return (from m in db.tbl_units
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
        }

        public bool InsertUnit(string name, string unit)
        {
            try
            {
                tbl_unit newitem = new tbl_unit();

                newitem.name = name;
                newitem.unit = unit;
                newitem.regtime = DateTime.Now;
                newitem.deleted = 0;

                db.tbl_units.InsertOnSubmit(newitem);
                db.SubmitChanges();

                return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UnitModel", "InsertUnit()", e.ToString());
                return false;
            }

        }

        public bool UpdateUnit(long id, string name, string unit)
        {
            try
            {
                var edititem = (from m in db.tbl_units
                                where m.deleted == 0 && m.id == id
                                select m).FirstOrDefault();
                if (edititem != null)
                {
                    edititem.name = name;
                    edititem.unit = unit;
                    db.SubmitChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UnitModel", "UpdateUnit()", e.ToString());
                return false;
            }

            return false;
        }

        public bool CheckUnit(string unit, long id)
        {
            try
            {
                tbl_unit unit_data = (from m in db.tbl_units
                                 where m.unit == unit && m.id != id && m.deleted == 0
                                 select m).FirstOrDefault();
                if (unit_data == null)
                    return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UnitModel", "CheckUnit", e.ToString());
                return false;
            }

            return false;
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

        public static string GetUnitNameById(long id)
        {
            tbl_unit item = (from m in db.tbl_units
                             where m.deleted == 0 && m.id == id
                             select m).FirstOrDefault();
            if (item != null)
                return item.name;
            else
                return "";
        }
    }
}