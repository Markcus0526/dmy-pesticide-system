using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
    #region catalog_model
    public class NongYaoInfo
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class CatalogInfo
    {
        public string id { get; set; }
        public string catalog_num { get; set; }
        public string register_id { get; set; }
        public string name { get; set; }
        public string usingname { get; set; }
        public long? kind { get; set; }
        public int avail_date { get; set; }
    }

    public class UnpassedCatalogInfo
    {
        public long id { get; set; }
        public string name { get; set; }
        public string reason { get; set; }
    }
    #endregion
    public class CATALOG_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "操作失败： 农药名称重复！";
        public const string DUPLICATE_PIHAO = "操作失败： 农药批号重复！";
        public const string DUPLICATE_REGISTER = "操作失败： 农药登记证号重复！";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
    }

    public class CatalogModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static JqDataTableInfo GetCatalogListDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long storeId, string nongyaoName)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<RemainCatalogInfo> filteredCompanies;

            long nongyao_id = CommonModel.GetNongYaoKindFromName(nongyaoName);
            var alllist = RemainModel.GetRemainCatalogInfoListFromStore(storeId, nongyao_id);

            filteredCompanies = alllist;

            var displayedUnit = filteredCompanies.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            var result = from c in displayedUnit
                         select new[] {
                             Convert.ToString(c.catalog_num),
                             Convert.ToString(c.name),
                             Convert.ToString(c.usingname),
                             Convert.ToString(c.standard),
                             Convert.ToString(c.unit),
                             Convert.ToString(c.supply),
                             Convert.ToString(c.productdate),
                             Convert.ToString(c.avail_date) + "个月",
                             Convert.ToString(c.largenumber),
                             Convert.ToString(c.price),
                             Convert.ToString(c.quantity),
                             Convert.ToString(c.price * c.quantity)
                         };
            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = alllist.Count();
            rst.aaData = result;

            return rst;
        }
        /*
        public static List<RemainCatalogInfo> GetInventoryList(long storeId, string nongyaoName)
        {
            
            List<RemainCatalogInfo> retList = new List<RemainCatalogInfo>();
            List<CatalogInfo> tempList = new List<CatalogInfo>();
            if (storeId == 0)
            {
                var storeList = StoreModel.GetStoreList(CommonModel.GetCurrentUserShopId());
                foreach (tbl_store item in storeList)
                {
                    IEnumerable<CatalogInfo> ctlgList = GetCatalogInfoList(Convert.ToInt64(item.id));
                    tempList.AddRange(ctlgList);
                }

                int removeCount = 0;
                for (int i = 0; i < tempList.Count; i++)
                {
                    for (int j = 0; j < tempList.Count; j++)
                    {
                        if (tempList[i] != tempList[j])
                        {
                            if (tempList[i].id == tempList[j].id)
                            {
                                long value = Convert.ToInt64(tempList[i].quantity);
                                value += Convert.ToInt64(tempList[j].quantity);
                                tempList[i].quantity = value.ToString();
                                tempList.Remove(tempList[j]);
                                removeCount++;
                                j--;
                            }
                        }
                    }
                    if (removeCount>0)
                        i--;
                    removeCount = 0;
                }
            }
            else
            {
                tempList = GetCatalogInfoList(storeId);
            }

            if (nongyaoName.CompareTo("") != 0)
            {
                tbl_nongyao nongyaoInfo = (from m in db.tbl_nongyaos
                                           where m.deleted == 0 && m.name == nongyaoName
                                       select m).FirstOrDefault();
                if (nongyaoInfo == null) return retList;
                List<tbl_nongyao> nongyaoList = (from m in db.tbl_nongyaos
                                                 where m.deleted == 0 && m.parentid == nongyaoInfo.id
                                       select m).ToList();
                if (nongyaoList == null || nongyaoList.Count() == 0)
                {
                    for (int i = 0; i < retList.Count; i++)
                    {
                        if (nongyaoInfo.id != retList[i].kind)
                        {
                            retList.Remove(retList[i]);
                            i--;
                        }
                    }
                }
                else
                {
                    bool flag = false;
                    for (int i = 0; i < retList.Count; i++)
                    {
                        foreach (var item in nongyaoList)
                        {
                            if (item.id == retList[i].kind)
                            {
                                flag = true;
                            }
                        }
                        if (flag == false)
                        {
                            retList.Remove(retList[i]);
                            i--;
                        }
                        flag = false;
                    }
                }
            }
            for (var i = 0; i < retList.Count(); i++)
            {
                if (Convert.ToInt64(retList[i].quantity) == 0)
                {
                    retList.Remove(retList[i]);
                }
            }
            return retList;
        }

        public static List<RemainCatalogInfo> GetCatalogInfoList(long storeId)
        {
            List<RemainCatalogInfo> retList = null;
            var cataloglist = db.tbl_remains
                 .Where(p => p.deleted == 0 && storeId == p.store_id)
                 .OrderByDescending(p => p.id)
                 .Select(m => new 
                 {
                     catalog_id = m.catalog_id,
                     standard_id = m.standard_id,
                     largenumber = m.largenumber
                 }).Distinct();

            var remainList = new List<RemainInfo>();
            foreach (var item in cataloglist)
            {
                RemainInfo remainInfo = db.tbl_remains
                .Where(p => p.deleted == 0 && item.standard_id == p.standard_id && item.largenumber == p.largenumber && item.catalog_id == p.catalog_id && storeId == p.store_id)
                .OrderByDescending(p => p.id)
                .Select(m => new RemainInfo
                {
                    catalog_id = m.catalog_id,
                    standard_id = m.standard_id,
                    largenumber = m.largenumber,
                    quantity = m.quantity
                }).FirstOrDefault();
                if (remainInfo != null && remainInfo.quantity != 0)
                    remainList.Add(remainInfo);
            }
            
            retList = db.tbl_catalogs
                .Where(p => p.deleted == 0)
                .Join(cataloglist, m => m.id, l => l.catalog_id, (m, l) => new RemainCatalogInfo
                {
                    id = m.id,
                    catalog_num = Convert.ToString(m.catalog_num),
                    name = Convert.ToString(m.name),
                    usingname = Convert.ToString(m.usingname),
                    kind = m.kind,
                }).ToList();

            List<CatalogInfo> list = new List<CatalogInfo>();
            foreach (var rtItem in retList)
            {
                foreach (var remainItem in remainList)
                {
                    if (remainItem.catalog_id.ToString().CompareTo(rtItem.id) == 0)
                    {
                        rtItem.quantity = remainItem.quantity.ToString();
                        list.Add(rtItem);
                        break;
                    }
                }                
            }

            return list;
        }
        */
        public static IEnumerable<NongYaoInfo> GetNongYaoList()
        {
            return db.tbl_nongyaos
                .Where(p => p.deleted == 0 && p.parentid != 0)
                .Select(row => new NongYaoInfo
                {
                    id = Convert.ToString(row.id),
                    name = row.name
                }).ToList();
        }

        public static List<tbl_catalog> GetCatalogList(long shop_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            byte level = CommonModel.GetCurrentUserLevel();
            long region_id = CommonModel.GetCurrentUserRegionCity();
            tbl_shop shop = (from m in db.tbl_shops
                             where m.deleted == 0 && m.id == shop_id && m.pass == 1
                             select m).FirstOrDefault();


            return (from m in db.tbl_catalogs
                    where m.deleted == 0 && m.pass == 1 && ((m.level == 0 && m.region_id == region_id) || (shop.level == 1 && m.level == 1 && m.shop_id == shop_id))
                    select m).ToList();
        }

        public static string IsOnlyPiHao(string piHao)
        {
            /*
            int duplicate = (from m in db.tbl_catalogs
                                    where m.deleted == 0 && m.catalog_no.CompareTo(piHao) == 0
                                    select m).Count();
            if (duplicate != 0)
                return CATALOG_SUBMITSTATUS.DUPLICATE_PIHAO;
             */
            return CATALOG_SUBMITSTATUS.SUCCESS_SUBMIT;
        }

        public static bool IsConsistent(string name)
        {
            long region_id = CommonModel.GetCurrentUserRegionCity();
            tbl_catalog item = (from m in db.tbl_catalogs
                                where m.deleted == 0 && m.name == name && m.region_id == region_id
                                select m).FirstOrDefault();
            if (item != null)
                return false;
            else
                return true;
        }
        
            
        public static string InsertCatalog(string ctlg_register, string ctlg_permit, string ctlg_sample, string ctlg_name, string ctlg_nickname, long userId,
                                    string ctlg_product, string ctlg_shape, string ctlg_material, string ctlg_content, string ctlg_level, string ctlg_productarea, string ctlg_description, string imgpath)
        {
          
          
            try
            {
                if (IsConsistent(ctlg_name))
                {

                    tbl_catalog newitem = new tbl_catalog();
                    newitem.register_id = ctlg_register;
                    newitem.permit_id = ctlg_permit;
                    newitem.sample_id = ctlg_sample;
                    newitem.name = ctlg_name;
                    newitem.nickname = ctlg_nickname;
                    //newitem.usingname = ctlg_nickname;
                    newitem.product = ctlg_product;
                    newitem.shape = ctlg_shape;
                    newitem.material = ctlg_material;
                    newitem.content = Convert.ToDecimal(ctlg_content);

                    newitem.userid = userId;
                    newitem.regtime = DateTime.Now;
                    newitem.product_area = ctlg_productarea;
                   
                    if (ctlg_level == null)
                        newitem.level = 0;
                    else
                        newitem.level = 1;
                    newitem.description = ctlg_description;
                    newitem.image = imgpath;
                    newitem.shop_id = CommonModel.GetCurrentUserShopId();
                    newitem.shown = 0;
                    newitem.region_id = CommonModel.GetCurrentUserRegionCity();

                    db.tbl_catalogs.InsertOnSubmit(newitem);
                    db.SubmitChanges();

                    return CATALOG_SUBMITSTATUS.SUCCESS_SUBMIT;
                }
                else
                {
                    return CATALOG_SUBMITSTATUS.DUPLICATE_NAME;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("CatalogModel", "InsertItem()", e.ToString());
                return CATALOG_SUBMITSTATUS.ERROR_SUBMIT;
            }
           
        }

        public static tbl_catalog GetCatalogInfo(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            return (from m in db.tbl_catalogs
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
        }

        public static long GetNongyaoIdOfCatalog(long id)
        {
            var item = GetCatalogInfo(id);
            if (item != null)
                return (long)item.kind;
            else
                return 0;
        }

        public static string GetPassedCatalog()
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();
            List<tbl_catalog> showinglist = (from m in db.tbl_catalogs
                                 where m.deleted == 0 && m.shop_id == shop_id && m.pass == 1 && m.shown != 1
                                 select m).ToList();

            foreach (tbl_catalog item in showinglist)
            {
                item.shown = 1;
                db.SubmitChanges();
            }

            List<string> list = (from m in showinglist
                                select m.name).ToList();
            if (list.Count > 0)
            {
                string rst = "";
                foreach (string s in list)
                {
                    rst += s + ", ";
                }

                return rst.TrimEnd(new char[] { ' ', ',' });
            }
            else
                return "";
        }

        public static List<UnpassedCatalogInfo> GetUnpassedCatalog()
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            long shop_id = CommonModel.GetCurrentUserShopId();
            List<tbl_catalog> showinglist = (from m in db.tbl_catalogs
                              where m.deleted == 0 && m.shop_id == shop_id && m.pass == 2 && m.shown != 1
                              select m).ToList();
            foreach (tbl_catalog item in showinglist)
            {
                item.shown = 1;
                db.SubmitChanges();
            }

            var list = (from m in showinglist
                                 select new UnpassedCatalogInfo {
                                     id = m.id,
                                     name = m.name,
                                     reason = m.reason
                                 }).ToList();
            return list;
        }

        public static bool DeleteCatalogsForShopId(long shop_id)
        {
            try
            {
                var items = from m in db.tbl_catalogs
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

        // Additional ///////////////
        public static List<tbl_catalog> SearchCatalogList(string search)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            long shop_id = CommonModel.GetCurrentUserShopId();
            byte level = CommonModel.GetCurrentUserLevel();
            long region_id = CommonModel.GetCurrentUserRegionCity();
            return (from m in db.tbl_catalogs
                    where m.deleted == 0 && /*m.shop_id == shop_id &&*/ m.pass == 1 && m.level <= level && m.region_id == region_id &&
                        (m.register_id.Contains(search) || m.nickname.Contains(search))
                    select m).ToList();
        }

        public static int GetCatalogPassInfo(string ctlg_register)
        {
            NongYaoModelDataContext context = new NongYaoModelDataContext();

            long region_id = CommonModel.GetCurrentUserRegionCity();

            tbl_catalog cur_catalog =(from m in context.tbl_catalogs
                                where m.deleted == 0 && m.register_id == ctlg_register && m.region_id == region_id               
                              select m).FirstOrDefault();
            if (cur_catalog != null)
                return cur_catalog.pass;
            else
                return -1;
        }

        public static string GetCatalogNumForId(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            var item = (from m in db.tbl_catalogs
                        where m.deleted == 0 && m.id == id
                        select m).FirstOrDefault();
            if (item != null)
                return item.catalog_num;
            else
                return "";
        }

        public static string GetCatalogNameForId(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            var item = (from m in db.tbl_catalogs
                        where m.deleted == 0 && m.id == id
                        select m).FirstOrDefault();
            if (item != null)
                return item.name;
            else
                return "";
        }

        public static string GetCatalogRegisteridForId(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            var item = (from m in db.tbl_catalogs
                        where m.deleted == 0 && m.id == id
                        select m).FirstOrDefault();
            if (item != null)
                return item.register_id;
            else
                return "";
        }

        public static string GetCatalogProductareaForId(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            var item = (from m in db.tbl_catalogs
                        where m.deleted == 0 && m.id == id
                        select m).FirstOrDefault();
            if (item != null)
                return item.product_area;
            else
                return "";
        }

        public int GetCatalogAvailDateForId(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            var item = (from m in db.tbl_catalogs
                        where m.deleted == 0 && m.id == id
                        select m).FirstOrDefault();
            if (item != null)
                return (int)item.avail_date;
            else
                return 0;
        }
    }
}