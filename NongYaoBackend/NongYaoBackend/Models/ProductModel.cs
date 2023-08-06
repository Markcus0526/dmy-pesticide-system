using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections.Specialized;
using NongYaoBackend.Models.Library;

namespace NongYaoBackend.Models
{
    public class ProductModel
    {
        NongYaoModelDataContext db = new NongYaoModelDataContext();

        public List<tbl_catalog> GetCatalogList(long country)
        {
            if (country != 0)
            {
                /*return (from m in db.tbl_catalogs
                            from l in db.tbl_shops
                            from k in db.tbl_regions
                            where m.deleted == 0 && l.deleted == 0 && k.deleted == 0 && 
                                m.shop_id == l.id && l.region == k.id &&
                                k.parentid == country
                            select m)
                            .ToList();*/

                return db.tbl_catalogs
                    .Where(m => m.deleted == 0)
                    .Join(db.tbl_shops, m => m.shop_id, l => l.id, (m, l) => new { cataInfo = m, shopInfo = l })
                    .Join(db.tbl_regions, n => n.shopInfo.region, k => k.id, (n, k) => new { shopInfo = n, regionInfo = k })
                    .Where(p => (p.regionInfo.parentid == country) || (p.regionInfo.parentid != 0 && p.regionInfo.id == country))
                    .Select(row => row.shopInfo.cataInfo)
                    .OrderByDescending(m => m.id).ToList();
            }
            else
            {
                return db.tbl_catalogs
                       .Where(m => m.deleted == 0)
                       .OrderByDescending(m => m.id).ToList();
            }
        }

        public tbl_catalog GetCatalogInfo(long id)
        {
            return db.tbl_catalogs
                .Where(m => m.deleted == 0 && m.id == id)
                .FirstOrDefault();
        }

        public JqDataTableInfo GetCatalogDataTable(JQueryDataTableParamModel param, NameValueCollection Request, String rootUri, long country, string search_word, byte search_pass)
        {
            JqDataTableInfo rst = new JqDataTableInfo();
            IEnumerable<tbl_catalog> filteredCompanies;

            var alllist = GetCatalogList(country);
            //Check whether the companies should be filtered by keyword
            /*if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Optionally check whether the columns are searchable at all 
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isUsingNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                filteredCompanies = alllist
                   .Where(c => (isNameSearchable && c.name.ToLower().Contains(param.sSearch.ToLower()) ||
                            isUsingNameSearchable && c.usingname.Contains(param.sSearch.ToLower())) &&
                            c.pass != 1 - search_pass);
            }
            else
            {
                filteredCompanies = alllist;
            }*/
            if (search_pass == 3)
                filteredCompanies = alllist
                   .Where(c => ((c.name != null && c.name.ToLower().Contains(search_word)) ||
                            ((c.usingname != null) && c.usingname.Contains(search_word))));
            else
                filteredCompanies = alllist
                   .Where(c => (c.name.ToLower().Contains(search_word) ||
                            ((c.usingname != null) && c.usingname.Contains(search_word))) &&
                            c.pass == search_pass);

            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<tbl_catalog, string> orderingFunction = (c => sortColumnIndex == 1 && isNameSortable ? c.name : "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);

            var displayedCompanies = filteredCompanies.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedCompanies
                         select new[] { 
                c.catalog_num != null ? Convert.ToString(c.catalog_num) : "未定",
                Convert.ToString(c.name),
                c.usingname != null ? Convert.ToString(c.usingname) : "",
                c.image != null ? Convert.ToString(c.image) : "",
                (c.register_id != null && c.barcode != null) ? BarCodeToHTML.get39(c.register_id, 1, 30) : "",
                c.product != null ? Convert.ToString(c.product) : "",
                c.product_area != null ? Convert.ToString(c.product_area) : "",
                c.level != null ? Convert.ToString(c.level) : "0",
                c.pass != null ? Convert.ToString(c.pass) : "0",
                Convert.ToString(c.id)/*,
                Convert.ToString(c.permit_id),
                c.nickname != null ? Convert.ToString(c.nickname) : "",
                c.catalog_no != null ? Convert.ToString(c.catalog_no) : "",
                c.unit != null ? Convert.ToString(c.unit) : "0",
                c.price != null ? Convert.ToString(c.price) : "",
                c.userid != null ? UserModel.GetUserName((long)c.userid) : "",
                c.regtime != null ? String.Format("{0:yyyy-MM-dd HH:mm:ss}", c.regtime) : "",
                c.kind != null ?  Convert.ToString(c.kind) : "0",
                c.description != null ? Convert.ToString(c.description) : "",
                c.reason != null ? Convert.ToString(c.reason) : "",
                c.image != null ? Convert.ToString(c.image) : "",
                c.level != null ? Convert.ToString(c.level) : "0",
                c.standard != null ? Convert.ToString(c.standard) : "",
                c.barcode != null ? Convert.ToString(c.barcode) : "",
                c.pass != null ? Convert.ToString(c.pass) : "0",*/
            };
            
            rst.sEcho = param.sEcho;
            rst.iTotalRecords = alllist.Count();
            rst.iTotalDisplayRecords = filteredCompanies.Count();
            rst.aaData = result;

            return rst;
        }
        public static bool IsUniqueNickname(string check_val)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            tbl_catalog catinfo = (from m in db.tbl_catalogs
                                   where m.deleted == 0 && m.nickname == check_val
                                   select m).FirstOrDefault();
            if (catinfo != null)
                return false;
            else
                return true;

        }
        public static bool IsUniqueUsingname(string usingname, long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();

            tbl_catalog item = (from m in db.tbl_catalogs
                                where m.deleted == 0 && m.id == id
                                select m).FirstOrDefault();

            tbl_catalog catinfo = (from m in db.tbl_catalogs
                                   where m.deleted == 0 && m.usingname == usingname && m.id != id && m.region_id == item.region_id && m.pass == 1
                                   select m).FirstOrDefault();
            if (catinfo != null)
                return false;
            else
                return true;

        }
        public bool UpdateCatalog(
            long id,
            string catalog_num,
            string nickname,
            string barcode,
            string shape,
            string material,
            decimal content,
            int avail_date,
            string product,
            
            byte level,
            int kind,
            string product_area,
            string description,
            string imgpath,
            string reason,
            byte pass,
            string usingname)
        {
            try
            {
                tbl_catalog edititem = (from m in db.tbl_catalogs
                                     where m.deleted == 0 && m.id == id
                                     select m).FirstOrDefault();

                if (pass == 1)
                    if (!IsUniqueUsingname(usingname, id))
                        return false;
                if (edititem != null)
                {
                    if(pass == 1)
                        edititem.catalog_num = catalog_num;
                    edititem.barcode = barcode;
                    edititem.nickname = nickname;
                    edititem.product = product;
                    edititem.shape = shape;
                    edititem.material = material;
                    edititem.content = content;
                    edititem.avail_date = avail_date;
                    //edititem.standard = Convert.ToInt32(standard);
                    //edititem.unit = unit;
                    //edititem.price = price;
                    edititem.usingname = usingname;
                    edititem.level = level;
                    edititem.kind = kind;
                    edititem.product_area = product_area;
                    edititem.description = description;
                    edititem.image = imgpath;
                    edititem.reason = reason;                    
                    edititem.pass = pass;
                    edititem.regtime = DateTime.Now;

                    db.SubmitChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("UserModel", "UpdateUser()", e.ToString());
                return false;
            }

            return false;
        }

        public bool DeleteCatalog(long id)
        {
            var delitem = (from m in db.tbl_catalogs
                           where m.deleted == 0 && m.id == id
                           select m).FirstOrDefault();

            if (delitem != null)
            {
                delitem.deleted = 1;
                db.SubmitChanges();

                return true;
            }

            return false;
        }
        public static string GetPrintName(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            
            var standarditem = (from m in db.tbl_standards
                                where m.deleted == 0 && m.id == id
                                select m).FirstOrDefault();
            var unititem = (from m in db.tbl_units
                            where m.deleted == 0 && m.id == standarditem.unit_id
                            select m).FirstOrDefault();
            var st_str = "";
            if(standarditem != null && unititem != null)
                st_str = Convert.ToString(standarditem.quantity) + (standarditem.mass == 0 ? "克" : "毫升") + "/" + unititem.name; 
            string ret = "";
            ret = st_str;
            return ret;
        }
        public static string GetStandardText(long standard_id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            tbl_unit unititem = new tbl_unit();
            
            var standarditem = (from m in db.tbl_standards
                                where m.deleted == 0 && m.id == standard_id
                                select m).FirstOrDefault();
            if(standarditem != null)
            { 
                unititem = (from m in db.tbl_units
                            where m.deleted == 0 && m.id == standarditem.unit_id
                            select m).FirstOrDefault();
            }
            var st_str = "";
            if (standarditem != null && unititem != null)
                st_str = Convert.ToString(standarditem.quantity) + (standarditem.mass == 0 ? "克" : "毫升") + "/" + unititem.name;
            return st_str;
        }
    }
}
