using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NongYaoBackend.Models
{
    public class AREA_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "操作失败： 地区重复！";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
    }

    public class AreaModel
    {
        NongYaoModelDataContext db = new NongYaoModelDataContext();
            
        public static List<tbl_region> GetRegionList()
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            List<tbl_region> ret = (from m in db.tbl_regions
                                    where m.deleted == 0
                                    select m).ToList();

            return ret;
        }

        public List<string> GetNames(List<tbl_region> list)
        {
            return (from m in list
                    select m.name).ToList();
        }

        public List<string> GetRegionListToString()
        {

            List<string> ret = (from m in db.tbl_regions
                                    where m.deleted == 0
                                    select m.name).ToList();

            return ret;
        }

        public List<string> GetCityRegionListToString()
        {
            List<string> ret = (from m in db.tbl_regions
                                where m.deleted == 0 && m.parentid == 0 && m.parentid == 0 //1:city
                                select m.name).ToList();
            return ret;
        }

        public List<string> GetDistrictRegionListToString(long cityid)
        {
            List<string> ret = (from m in db.tbl_regions
                                where m.deleted == 0 && m.parentid == cityid && m.parentid != 0 //1:city
                                select m.name).ToList();
            return ret;
        }

        public tbl_region GetRegionItem(int id)
        {
            return (from m in db.tbl_regions
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
        }
        
        public tbl_region GetRegionItem(string name)
        {
            return (from m in db.tbl_regions
                    where m.deleted == 0 && m.name == name
                    select m).FirstOrDefault();
        }
        public static bool IsUpperRegion(long region_id1,long region_id2)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            tbl_region region_info = (from m in db.tbl_regions
                          where m.deleted == 0 && m.id == region_id2
                          select m).FirstOrDefault();
            if (region_info != null)
            {
                if (region_info.parentid == region_id1)
                    return true;
            }
            return false;
        }
        public static List<tbl_region> GetCityList()
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            return (from m in db.tbl_regions
                    where m.deleted == 0 && m.parentid == 0 //city
                    select m).ToList();
        }
        
        public static List<tbl_region> GetDistrictList()
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            return (from m in db.tbl_regions
                    where m.deleted == 0 && m.parentid != 0 //district
                    select m).ToList();
        }
        public static List<tbl_region> GetDistrictListByCityId(long cityid)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            return (from m in db.tbl_regions
                    where m.deleted == 0 && m.parentid != 0 && m.parentid == cityid //district
                    select m).ToList();
        }
        public static string GetRegionNameById(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            string retValue = "";
            tbl_region regioninfo = (from m in db.tbl_regions
                                     where m.deleted == 0 && m.id == id
                                     select m).FirstOrDefault();
            if (regioninfo != null)
            {
                retValue = regioninfo.name;
            }
            return retValue;
        }
        public static long GetRegionIdByUserId(long userid)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            
            tbl_admin userInfo = (from m in db.tbl_admins
                                  where m.deleted == 0 && m.id == userid
                                  select m).FirstOrDefault();
            if (userInfo == null)
                return 0;

            tbl_region regioninfo = (from m in db.tbl_regions
                                     where m.deleted == 0 && m.id == userInfo.regionid
                                     select m).FirstOrDefault();
            if (regioninfo == null)
                return 0;

            return regioninfo.id;
        }
        public static string GetUpperRegionNameById(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            string retValue = "";
            tbl_region regioninfo = (from m in db.tbl_regions
                                     where m.deleted == 0 && m.id == id
                                     select m).FirstOrDefault();
            tbl_region upper_region = (from m in db.tbl_regions
                                       where m.deleted == 0 && m.id == regioninfo.parentid
                                       select m).FirstOrDefault();
            if (upper_region != null)
            {
                retValue = upper_region.name;
            }
            return retValue;
        }

        public static long GetUpperRegionIdById(long id)
        {
            if (id == 0)
                return 0;

            NongYaoModelDataContext db = new NongYaoModelDataContext();
            long retValue = 0;
            tbl_region regioninfo = (from m in db.tbl_regions
                                     where m.deleted == 0 && m.id == id
                                     select m).FirstOrDefault();
            tbl_region upper_region = (from m in db.tbl_regions
                                       where m.deleted == 0 && m.id == regioninfo.parentid
                                       select m).FirstOrDefault();
            if (regioninfo != null && upper_region != null)
            {
                retValue = upper_region.id;
            }
            return retValue;
        }

        public bool IsConsistent(string name, long parentid)
        {
            tbl_region item = (from m in db.tbl_regions
                                where m.deleted == 0 && m.name == name && m.parentid == parentid
                                select m).FirstOrDefault();
            if (item != null)
                return false;
            else
                return
                    true;
        }

        public string InsertRegion(string name, byte role, long cityid)
        {
            try
            {
                if (IsConsistent(name, cityid))
                {
                    tbl_region newitem = new tbl_region();

                    newitem.name = name;
                    newitem.regtime = DateTime.Now;
                    newitem.deleted = 0;
                    if (role == 0)
                    {
                        newitem.parentid = 0;
                        //newitem.regioninfo = 1;
                    }
                    else if (role == 1)
                    {
                        newitem.parentid = cityid;
                        //newitem.regioninfo = 2;
                    }

                    db.tbl_regions.InsertOnSubmit(newitem);
                    db.SubmitChanges();

                    return AREA_SUBMITSTATUS.SUCCESS_SUBMIT;
                }
                else
                {
                    return AREA_SUBMITSTATUS.DUPLICATE_NAME;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("AreaModel", "InsertRegion()", e.ToString());
                return AREA_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public string UpdateRegion(string name, string originname, byte role, long parentid)
        {
            try
            {
                if (name != originname && IsConsistent(name, parentid))
                {
                    var edititem = (from m in db.tbl_regions
                                    where m.deleted == 0 && m.name == originname
                                    select m).FirstOrDefault();
                    if (edititem != null)
                    {
                        edititem.name = name;
                        db.SubmitChanges();

                        return AREA_SUBMITSTATUS.SUCCESS_SUBMIT;
                    }
                }
                else if (name == originname)
                {
                    return AREA_SUBMITSTATUS.SUCCESS_SUBMIT;
                }
                else
                {
                    return AREA_SUBMITSTATUS.DUPLICATE_NAME;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("AreaModel", "UpdateRegion()", e.ToString());
                return AREA_SUBMITSTATUS.ERROR_SUBMIT;
            }

            return AREA_SUBMITSTATUS.ERROR_SUBMIT;
        }

        public bool DeleteRegion(string name)
        {
            try
            {
                long myid = CommonModel.GetCurrentUserId();
                long regionid = AccountModel.GetCurrentUserRegionId(myid);
                long cityid = GetUpperRegionIdById(regionid);


                if (regionid == 0)
                {
                    var delitem = (from m in db.tbl_regions
                                   where m.deleted == 0 && m.name == name && m.parentid == 0
                                   select m).FirstOrDefault();

                    var delchilditems = (from m in db.tbl_regions
                                         where m.deleted == 0 && m.parentid == delitem.id
                                         select m).ToList();

                    if (delchilditems.Count() != 0)
                    {
                        foreach (var childitem in delchilditems)
                        {
                            var delchilduser = (from m in db.tbl_admins
                                                where m.deleted == 0 && m.regionid == childitem.id
                                                select m).ToList();

                            if (delchilduser.Count() != 0)
                            {
                                foreach (var deluseritem in delchilduser)
                                {
                                    deluseritem.deleted = 1;
                                }
                            }
                            if (childitem != null)
                            {
                                childitem.deleted = 1;
                            }
                        }
                    }

                    if (delitem != null)
                    {
                        delitem.deleted = 1;
                    }
                }
                else
                {
                    var delitem = (from m in db.tbl_regions
                                   where m.deleted == 0 && m.name == name && m.parentid == regionid
                                   select m).FirstOrDefault();

                    var deluser = (from m in db.tbl_admins
                                   where m.deleted == 0 && m.regionid == delitem.id
                                   select m).ToList();
                    if (deluser.Count() != 0)
                    {
                        foreach (var item in deluser)
                        {
                            item.deleted = 1;
                        }
                    }

                    if (delitem != null)
                    {
                        delitem.deleted = 1;
                    }
                }
                
                db.SubmitChanges();

                return true;
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("AreaModel", "DeleteRegion()", e.ToString());
                return false;
            }
        } 

    }
}
