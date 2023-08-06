using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NongYaoFrontEnd.Models
{
    #region region_model
    public class Region
    {
        public long id { get; set; }
        public string name { get; set; }
        public DateTime? regtime { get; set; }
        public long parentid { get; set; }
    }
    #endregion

    public class RegionModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static string getRegionName(long? regionid)
        {           
            tbl_region regionInfo = (from m in db.tbl_regions
                                     where m.deleted == 0 && m.id == regionid
                                     select m).FirstOrDefault();
            if (regionInfo == null)
                return "";
            return regionInfo.name;
        }

        public static IEnumerable<Region> GetCityList()
        {
            List<Region> retList = null;

            retList = db.tbl_regions
                .Where(p => p.deleted == 0 && p.parentid == 0)
                .Select(row => new Region
                {
                    id = Convert.ToInt64(row.id),
                    name = Convert.ToString(row.name),
                    regtime = Convert.ToDateTime(row.regtime),
                    parentid = Convert.ToInt64(row.parentid)
                }).ToList();

            return retList;
        }

        public static IEnumerable<Region> GetDistrictList(long? cityid)
        {
            List<Region> retList = null;

            if (cityid == null)
                return null;

            retList = db.tbl_regions
                .Where(p => p.deleted == 0 && p.parentid == cityid)
                .Select(row => new Region
                {
                    id = Convert.ToInt64(row.id),
                    name = Convert.ToString(row.name),
                    regtime = Convert.ToDateTime(row.regtime),
                    parentid = Convert.ToInt64(row.parentid)
                }).ToList();

            return retList;
        }

        public static long GetParentId(long? districtid)
        {
            tbl_region regionInfo = (from m in db.tbl_regions
                                     where m.deleted == 0 && m.id == districtid
                                     select m).FirstOrDefault();
            if (regionInfo == null)
                return 0;

            return (long)regionInfo.parentid;
        }

        public static long GetCityIdForRegionId(long region_id)
        {
            tbl_region item = (from m in db.tbl_regions
                                     where m.deleted == 0 && m.id == region_id
                                     select m).FirstOrDefault();
            if (region_id != null)
            {
                if (item.parentid == 0)
                    return item.id;
                else
                    return (long)item.parentid;
            }

            return 0;
        }
    }
}