using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NongYaoFrontEnd.Models
{
    public class StandardInfo
    {
        public long id { get; set; }
        public string standard { get; set; }
    }


    public class StandardModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();        

        public static long GetOrInsertStandardWith(decimal quantity, byte mass, long unit_id)
        {
            try
            {
                tbl_standard item = (from m in db.tbl_standards
                                     where m.deleted == 0 && m.quantity == quantity && m.mass == mass && m.unit_id == unit_id
                                     select m).FirstOrDefault();
                if (item != null)
                {
                    return item.id;
                } else {
                    tbl_standard newitem = new tbl_standard();

                    newitem.quantity = quantity;
                    newitem.mass = mass;
                    newitem.unit_id = unit_id;

                    db.tbl_standards.InsertOnSubmit(newitem);
                    db.SubmitChanges();

                    return GetLastInsertedId();
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("ShopModel", "UpdateShop()", e.ToString());
                return 0;
            }
        }

        public static tbl_standard GetStandardInfo(long id)
        {
            NongYaoModelDataContext context = new NongYaoModelDataContext();
            return context.tbl_standards
                .Where(m => m.deleted == 0 && m.id == id)
                .FirstOrDefault();
        }

        public static long GetLastInsertedId()
        {
            tbl_standard item = db.tbl_standards
                .Where(m => m.deleted == 0)
                .OrderByDescending(m => m.id)
                .FirstOrDefault();
            if (item != null)
                return item.id;
            else
                return 0;
        }

        public static string GetStandardDescForId(long id)
        {
            var item = (from m in db.tbl_standards
                                     where m.deleted == 0 && m.id == id
                                     select m).FirstOrDefault();
            if (item != null)
                return item.quantity + (item.mass == 0 ? "克" : "毫升") + "/" + UnitModel.GetUnitNameById(item.unit_id);
            else
                return "";
        }

        public static decimal GetStandardQuantity(long id)
        {
            var item = GetStandardInfo(id);
            if (item != null)
                return item.quantity;
            else
                return 0;
        }
    }
}
