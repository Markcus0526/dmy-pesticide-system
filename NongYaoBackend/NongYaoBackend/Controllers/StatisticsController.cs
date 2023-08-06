using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using NongYaoBackend.Models;
using NongYaoBackend.Models.Library;
namespace NongYaoBackend.Controllers
{
    public class StatisticsController : Controller
    {
        //
        // GET: /Area/
        static long region_id = 0;
        static long salesman_id = 0;
        //static string start_time = DateTime.Now;
        //static string end_time = "";
        static DateTime start_time = DateTime.Now.AddMonths(-1);
        static DateTime end_time = DateTime.Now;
        static byte nongyaotype = 0;   //default:quandou
        static long nongyao_id = 0;
        public static string tmpUserFile;

        public ActionResult Index()
        {
            if (User.Identity.Name.Length > 5)
            {
                ViewBag.username = User.Identity.Name.Substring(0, 5) + "...";
            }
            else
            {
                ViewBag.username = User.Identity.Name;
            }
            byte role = CommonModel.GetCurrentUserRole();
            ViewBag.role = role;
            
            return View();
        }

#region ShopPart
        static long shopRegionId = 0;
        static long shopId = 0;
        static byte level = 0;
        static DateTime shopStartTime = CommonModel.GetFirstDayofMonth();
        static DateTime shopEndTime = DateTime.Now;
        public ActionResult Shop()
        {
            ViewBag.role = CommonModel.GetCurrentUserRole();

            shopRegionId = 0;
            shopId = 0;
            level = 0;
            
            if (ViewBag.role == 0)
            {
                shopRegionId = 0;
            }
            else if (ViewBag.role == 1)
            {
                long cur_id = CommonModel.GetCurrentUserId();
                long city_id = AccountModel.GetCurrentUserRegionId(cur_id);
                ViewBag.curCityId = city_id;
                ViewBag.curDistrictList = AreaModel.GetDistrictListByCityId(city_id);
                shopRegionId = city_id;
            }
            else if (ViewBag.role == 2)
            {
                long cur_id = CommonModel.GetCurrentUserId();
                long district_id = AccountModel.GetCurrentUserRegionId(cur_id);
                ViewBag.curDistrictId = district_id;
                ViewBag.curDistrictName = AreaModel.GetRegionNameById(district_id);
                shopRegionId = district_id;
            }

            ViewBag.cityregion = AreaModel.GetCityList();
            int b_month = DateTime.Now.Month;
            int b_year = DateTime.Now.Year;
            int b_day = 1;
            ViewBag.year = b_year;
            ViewBag.month = b_month;
            ViewBag.day = b_day;
            return View();
        }
        public JsonResult GetShopTableData(JQueryDataTableParamModel param)
        {
            StatisticsModel sModel = new StatisticsModel();
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            JqDataTableInfo rst = sModel.GetShopTable(param, Request.QueryString, rootUri, shopRegionId, shopId, shopStartTime, shopEndTime, level);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult changeShopRegion(string regionid)
        {
            bool rst = true;
            shopId = 0;
            shopRegionId = Convert.ToInt64(regionid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult searchSalesman(string basepinyin)
        {
            StatisticsModel sModel = new StatisticsModel();
            var rst = sModel.GetSalesmanListFromPinyin(basepinyin, shopRegionId);
            var ret = Json(new { salesmanlist = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult changeSalesman(string shopid)
        {
            bool rst = true;
            if (shopid.Length > 0)
                shopId = Convert.ToInt64(shopid);
            else
                shopId = 0;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult changeSalesmanLevel(string lev_value)
        {
            bool rst = true;
            if (lev_value == "on")
                level = 1;
            else
                level = 0;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult shopStartDateChange(DateTime startDate)
        {
            bool rst = true;
            shopStartTime = startDate;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult shopEndDateChange(DateTime endDate)
        {
            bool rst = true;
            shopEndTime = endDate;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteShop(string id)
        {
            StatisticsModel sModel = new StatisticsModel();
            bool rst = true;

            rst = sModel.DeleteShop(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportShopData()
        {
            return ExportShopToExcel();
            //return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Download()
        {
            String[] ff = tmpUserFile.Split('/');
            String tmp = "";
            if (ff.Length == 5) tmp = ff[4];
            Response.ContentType = "Application/x-msexcel";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + tmp);
            Response.TransmitFile(Server.MapPath(tmpUserFile));
            Response.End();

            return View();
        }

        [HttpPost]
        public ActionResult ExportShopToExcel()
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            Excel.Range chartRange;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            #region Add Columns
            int idx = 2;
            xlWorkSheet.Cells[1, 1] = "";
            xlWorkSheet.Cells[1, idx++] = "经营许可证代码";
            xlWorkSheet.Cells[1, idx++] = "地区";
            xlWorkSheet.Cells[1, idx++] = "经销商名称";
            xlWorkSheet.Cells[1, idx++] = "法人";
            xlWorkSheet.Cells[1, idx++] = "联系方式";
            xlWorkSheet.Cells[1, idx++] = "库存总数量";
            xlWorkSheet.Cells[1, idx++] = "销售总数量";
            #endregion

            #region Add Rows
            StatisticsModel sModel = new StatisticsModel();
            List<ShopStatInfo> data = sModel.GetShopData(shopRegionId, shopId, shopStartTime, shopEndTime, level);
            long row_index = 2;
            foreach (ShopStatInfo item in data)
            {
                xlWorkSheet.Cells[row_index, 2] = item.permit_id;
                xlWorkSheet.Cells[row_index, 3] = item.area;
                xlWorkSheet.Cells[row_index, 4] = item.shop_name;
                xlWorkSheet.Cells[row_index, 5] = item.law_man;
                xlWorkSheet.Cells[row_index, 6] = item.phone_num;
                xlWorkSheet.Cells[row_index, 7] = item.sale_count;
                xlWorkSheet.Cells[row_index, 8] = item.remain_count;
                row_index++;
            }
            #endregion

            chartRange = xlWorkSheet.get_Range("a1", "v1");
            chartRange.Font.Bold = true;

            tmpUserFile = "~/Content/uploads/tempexcel/经销商统计" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
            xlWorkBook.SaveAs(Server.MapPath(tmpUserFile), Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();
            /*Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=经销商统计" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls");
            FileStream fs = new FileStream(Server.MapPath(tmpUserFile), FileMode.Open);
            xlApp.Quit();*/

            CommonModel.releaseObject(xlApp);
            CommonModel.releaseObject(xlWorkBook);
            CommonModel.releaseObject(xlWorkSheet);

            return Redirect("Download");
            //int filelen = (int)fs.Length;
         /*   byte[] dataArray = new byte[filelen];
            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(dataArray, 0, filelen);
            fs.Close();
            //System.IO.File.Delete(Server.MapPath(tmpUserFile));
            return File(dataArray, "application/ms-excel");*/
        }
#endregion
#region NongyaoPart
        static long nongyaoRegionId = 0;
        static long nongyaoType = 0;
        static long nongyaoId = 0;
        static long catalogId = 0;
        static long standardId = 0;
        static DateTime nongyaoStartTime = CommonModel.GetFirstDayofMonth();
        static DateTime nongyaoEndTime = DateTime.Now;
        public ActionResult Nongyao()
        {
            TypeModel tModel = new TypeModel();
            nongyaoRegionId = 0;
            nongyaoType = 0;
            nongyaoId = 0;
            catalogId = 0;
            standardId = 0;

            ViewBag.role = CommonModel.GetCurrentUserRole();

            if (ViewBag.role == 1)
            {
                long cur_id = CommonModel.GetCurrentUserId();
                long city_id = AccountModel.GetCurrentUserRegionId(cur_id);
                ViewBag.curCityId = city_id;
                ViewBag.curDistrictList = AreaModel.GetDistrictListByCityId(city_id);
                nongyaoRegionId = city_id;
            }
            else if (ViewBag.role == 2)
            {
                long cur_id = CommonModel.GetCurrentUserId();
                long district_id = AccountModel.GetCurrentUserRegionId(cur_id);
                ViewBag.curDistrictId = district_id;
                ViewBag.curDistrictName = AreaModel.GetRegionNameById(district_id);
                nongyaoRegionId = district_id;
            }

            ViewBag.cityregion = AreaModel.GetCityList();
            ViewBag.nongyaos = tModel.GetParentNongyaos();
            int b_month = DateTime.Now.Month;
            int b_year = DateTime.Now.Year;
            int b_day = 1;
            ViewBag.year = b_year;
            ViewBag.month = b_month;
            ViewBag.day = b_day;
            return View();
        }
        public JsonResult GetNongyaoTableData(JQueryDataTableParamModel param)
        {
            StatisticsModel sModel = new StatisticsModel();
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            JqDataTableInfo rst = sModel.GetNongyaoTable(param, Request.QueryString, rootUri, nongyaoRegionId, nongyaoType, nongyaoId, nongyaoStartTime, nongyaoEndTime);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportNongyaoData()
        {
            return ExportNongyaoToExcel();            
        }
        
        public ActionResult ExportNongyaoToExcel()
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            Excel.Range chartRange;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            #region Add Columns
            int idx = 2;
            xlWorkSheet.Cells[1, 1] = "";
            xlWorkSheet.Cells[1, idx++] = "农药登记证号";
            xlWorkSheet.Cells[1, idx++] = "农药名称";
            xlWorkSheet.Cells[1, idx++] = "生产厂家";
            xlWorkSheet.Cells[1, idx++] = "规格";
            xlWorkSheet.Cells[1, idx++] = "库存总量";
            xlWorkSheet.Cells[1, idx++] = "销售总量";
            #endregion

            #region Add Rows
            StatisticsModel sModel = new StatisticsModel();
            List<NongyaoStatInfo> data = sModel.GetNongyaoData(nongyaoRegionId, nongyaoType, nongyaoId, nongyaoStartTime, nongyaoEndTime);
            long row_index = 2;
            foreach (NongyaoStatInfo item in data)
            {
                xlWorkSheet.Cells[row_index, 2] = item.permit_id;
                xlWorkSheet.Cells[row_index, 3] = item.name;
                xlWorkSheet.Cells[row_index, 4] = item.product;
                xlWorkSheet.Cells[row_index, 5] = item.standard;
                xlWorkSheet.Cells[row_index, 6] = item.remain_count;
                xlWorkSheet.Cells[row_index, 7] = item.sale_count;
                row_index++;
            }
            #endregion

            chartRange = xlWorkSheet.get_Range("a1", "v1");
            chartRange.Font.Bold = true;

            tmpUserFile = "~/Content/uploads/tempexcel/农药统计" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
            xlWorkBook.SaveAs(Server.MapPath(tmpUserFile), Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
          /*  Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=农药统计" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls");
            FileStream fs = new FileStream(Server.MapPath(tmpUserFile), FileMode.Open);*/
            xlApp.Quit();

            CommonModel.releaseObject(xlApp);
            CommonModel.releaseObject(xlWorkBook);
            CommonModel.releaseObject(xlWorkSheet);

           /* int filelen = (int)fs.Length;
            byte[] dataArray = new byte[filelen];
            fs.Seek(0, SeekOrigin.Begin);
            fs.Read(dataArray, 0, filelen);
            fs.Close();
            //System.IO.File.Delete(Server.MapPath(tmpUserFile));
            return File(dataArray, "application/ms-excel");*/
            return Redirect("Download");
        }
        public JsonResult changeNongyaoRegion(string regionid)
        {
            bool rst = true;
            nongyaoId = 0;
            nongyaoType = 0;
            nongyaoRegionId = Convert.ToInt64(regionid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult changeNongyaoType(string typeid)
        {
            bool rst = true;
            nongyaoId = 0;
            nongyaoType = Convert.ToInt64(typeid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult searchNongyao(string basepinyin)
        {
            StatisticsModel sModel = new StatisticsModel();
            var rst = sModel.GetNongyaoListFromPinyin(basepinyin, nongyaoType);
            var ret = Json(new { nongyaolist = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult changeNongyao(string shopid)
        {
            bool rst = true;
            if (shopid != "")
                nongyaoId = Convert.ToInt64(shopid);
            else
                nongyaoId = 0;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult nongyaoStartDateChange(DateTime startDate)
        {
            bool rst = true;
            nongyaoStartTime = startDate;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult nongyaoEndDateChange(DateTime endDate)
        {
            bool rst = true;
            nongyaoEndTime = endDate;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult changeDetailNongyaoId(string catalog_id, string standard_id)
        {
            string rst = "";
            catalogId = Convert.ToInt64(catalog_id);
            standardId = Convert.ToInt64(standard_id);
            if (catalogId > 0)
            {
                rst = ProductModel.GetPrintName(standardId);
            }

            return Json(new { printtext = rst }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDetailNongyaoTableData(JQueryDataTableParamModel param)
        {
            StatisticsModel sModel = new StatisticsModel();
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            JqDataTableInfo rst = sModel.GetDetailNongyaoTable(param, Request.QueryString, rootUri, catalogId, standardId, nongyaoStartTime, nongyaoEndTime);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ExportDetailNongyaoData()
        {            
            return ExportDetailNongyaoToExcel();            
        }

        
        public ActionResult ExportDetailNongyaoToExcel()
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            Excel.Range chartRange;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            #region Add Columns
            int idx = 2;
            xlWorkSheet.Cells[1, 1] = "";
            xlWorkSheet.Cells[1, idx++] = "生产批号";
            xlWorkSheet.Cells[1, idx++] = "生产日期";
            xlWorkSheet.Cells[1, idx++] = "有效期";
            xlWorkSheet.Cells[1, idx++] = "库存数量";
            xlWorkSheet.Cells[1, idx++] = "销售数量";
            xlWorkSheet.Cells[1, idx++] = "合计数量";
            xlWorkSheet.Cells[1, idx++] = "地区";
            xlWorkSheet.Cells[1, idx++] = "经销商";
            #endregion

            #region Add Rows
            StatisticsModel sModel = new StatisticsModel();
            List<DetailNongyaoStatInfo> data = sModel.GetDetailNongyaoData(catalogId, nongyaoStartTime, nongyaoEndTime);
            long row_index = 2;
            foreach (DetailNongyaoStatInfo item in data)
            {
                xlWorkSheet.Cells[row_index, 2] = item.product_number;
                xlWorkSheet.Cells[row_index, 3] = item.product_date;
                xlWorkSheet.Cells[row_index, 4] = item.avail_date;
                xlWorkSheet.Cells[row_index, 5] = item.remain_count;
                xlWorkSheet.Cells[row_index, 6] = item.sale_count;
                xlWorkSheet.Cells[row_index, 7] = item.sum_count;
                xlWorkSheet.Cells[row_index, 8] = item.area;
                xlWorkSheet.Cells[row_index, 9] = item.salesman;
                row_index++;
            }
            #endregion

            chartRange = xlWorkSheet.get_Range("a1", "v1");
            chartRange.Font.Bold = true;

            tmpUserFile = "~/Content/uploads/tempexcel/农药详细统计" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls";
            xlWorkBook.SaveAs(Server.MapPath(tmpUserFile), Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            xlWorkBook.Close(true, misValue, misValue);
           /* Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=农药详细统计" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".xls");
            FileStream fs = new FileStream(Server.MapPath(tmpUserFile), FileMode.Open);*/
            xlApp.Quit();

            CommonModel.releaseObject(xlApp);
            CommonModel.releaseObject(xlWorkBook);
            CommonModel.releaseObject(xlWorkSheet);

            return Redirect("Download");
        }
#endregion
        #region StatisticsPart
        static int scrollCount = AreaModel.GetDistrictList().Count()-10;
        static int detail_scrollCount = AreaModel.GetDistrictList().Count() - 10;
        static DateTime detailStartTime = CommonModel.GetFirstDayofMonth();
        static DateTime detailEndTime = DateTime.Now;
        static long     detailNongyaoType = 0;
        static long     detailNongyaoId = 0;
        public ActionResult Statistics()
        {
            TypeModel tModel = new TypeModel();
            ViewBag.nongyaos = tModel.GetParentNongyaos();
            ViewBag.region = AreaModel.GetRegionList();
            int b_month = DateTime.Now.Month;
            int b_year = DateTime.Now.Year;
            int b_day = 1;
            ViewBag.year = b_year;
            ViewBag.month = b_month;
            ViewBag.day = b_day;
            ViewBag.DistrictCount = AreaModel.GetDistrictList().Count();
            List<int> year_list = new List<int>();
            for (int i = 2013; i <= b_year; i++)
            {
                year_list.Add(i);
            }
            ViewBag.years = year_list;

            return View();
        }
        public JsonResult detailNongyaoStartDateChange(DateTime startDate)
        {
            bool rst = true;
            detailStartTime = startDate;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult detailNongyaoEndDateChange(DateTime endDate)
        {
            bool rst = true;
            detailEndTime = endDate;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getYearValue(string year)
        {
            StatisticsModel sModel = new StatisticsModel();
            YearSaleCount res = new YearSaleCount();
            res = sModel.GetYearSaleCount(year);
            var ret = Json(new { yearCount = res }, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult GetTypeSaleCount(string year)
        {
            StatisticsModel sModel = new StatisticsModel();
            List<TypeSaleCountInfo> res = new List<TypeSaleCountInfo>();
            res = sModel.GetTypeSaleCount(year);
            var ret = Json(res, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult GetMonthSaleCount(string year)
        {
            StatisticsModel sModel = new StatisticsModel();
            List<MonthSaleCountInfo> res = new List<MonthSaleCountInfo>();
            res = sModel.GetMonthSaleCount(year);
            var ret = Json(res, JsonRequestBehavior.AllowGet);
            return ret;
        }

        public JsonResult changeDetailNongyaoType(string typeid)
        {
            bool rst = true;
            detailNongyaoId = 0;
            detailNongyaoType = Convert.ToInt64(typeid);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult changeDetailNongId(string nongyao_id)
        {
            bool rst = true;
            detailNongyaoId = Convert.ToInt64(nongyao_id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult cleanDetailFilter(string nongyao_id)
        {
            bool rst = true;
            detailStartTime = CommonModel.GetFirstDayofMonth();
            detailEndTime = DateTime.Now;
            detailNongyaoType = 0;
            detailNongyaoId = 0;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult detail_searchNongyao(string basepinyin)
        {
            StatisticsModel sModel = new StatisticsModel();
            var rst = sModel.GetNongyaoListFromPinyin(basepinyin, nongyaoType);
            var ret = Json(new { nongyaolist = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult detail_changeNongyao(string nongyao_id)
        {
            bool rst = true;
            detailNongyaoId = Convert.ToInt64(nongyao_id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult nongyaoStartDateChange(DateTime startDate)
        //{
        //    bool rst = true;
        //    nongyaoStartTime = startDate;
        //    return Json(rst, JsonRequestBehavior.AllowGet);
        //}
        //public JsonResult nongyaoEndDateChange(DateTime endDate)
        //{
        //    bool rst = true;
        //    nongyaoEndTime = endDate;
        //    return Json(rst, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetDetailAreaSaleCount()
        {
            StatisticsModel sModel = new StatisticsModel();
            List<AreaSaleCountInfo> res = new List<AreaSaleCountInfo>();
            res = sModel.GetDetailAreaSaleCount(detailStartTime, detailEndTime, detailNongyaoType, detailNongyaoId, detail_scrollCount);
            var ret = Json(res, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult re_GetDetailAreaSaleCount(int cur_val)
        {
            StatisticsModel sModel = new StatisticsModel();
            List<AreaSaleCountInfo> res = new List<AreaSaleCountInfo>();
            detail_scrollCount = cur_val;
            res = sModel.GetDetailAreaSaleCount(detailStartTime, detailEndTime, detailNongyaoType, detailNongyaoId, detail_scrollCount);
            var ret = Json(res, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult changeSlide(int scroll_val)
        {
            bool rst = true;
            scrollCount = scroll_val;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult changeDetailSlide(int scroll_val)
        {
            bool rst = true;
            detail_scrollCount = scroll_val;
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAreaSaleCount(string year)
        {
            StatisticsModel sModel = new StatisticsModel();
            List<AreaSaleCountInfo> res = new List<AreaSaleCountInfo>();
            res = sModel.GetAreaSaleCount(year,scrollCount);
            var ret = Json(res, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult re_GetAreaSaleCount(string year, int cur_val)
        {
            StatisticsModel sModel = new StatisticsModel();
            List<AreaSaleCountInfo> res = new List<AreaSaleCountInfo>();
            scrollCount = cur_val;
            res = sModel.GetAreaSaleCount(year, cur_val);
            var ret = Json(res, JsonRequestBehavior.AllowGet);
            return ret;
        }
        


        #endregion
        public ActionResult Detail()
        {
            TypeModel tModel = new TypeModel();
            ViewBag.nongyaos = tModel.GetParentNongyaos();
           
            int b_month = DateTime.Now.Month - 1;
            int b_year = DateTime.Now.Year;
            int b_day = 1;
            ViewBag.year = b_year;
            ViewBag.month = b_month;
            ViewBag.day = b_day;

            return View();
        }
        

        [HttpPost]
        public JsonResult DeleteSalesman(string id)
        {
            SalesmanModel sModel = new SalesmanModel();
            bool rst = true;

            rst = sModel.DeleteSalesman(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaiNongYao()
        {
            TypeModel sModel = new TypeModel();
            var rst = sModel.GetNongyaoAtParentList(0);
            var ret = Json(new { chucaoData = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult GetXiaoNongYao()
        {
            TypeModel sModel = new TypeModel();
            var rst = sModel.GetXiaoNongyaoList();
            var ret = Json(new { shachongData = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult getArea()
        {
            AreaModel sModel = new AreaModel();
            var rst = sModel.GetRegionListToString();
            var ret = Json(new { areaData = rst }, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult GetSaleCount(string area, string type, string time)
        {
            byte nongyaoType = Convert.ToByte(type);
            long quantity = 0;
            StatisticsModel sModel = new StatisticsModel();
            if (area.CompareTo("全都") == 0)
            {
                quantity = sModel.GetSaleCount(area, nongyaoType);
            }
            else
            {
                quantity = sModel.GetSaleCount(area, nongyaoType);
                //sModel.GetSaleCount(time, type);
            }
            
            var ret = Json(new { quantityData = quantity }, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult GetChartData(int value,DateTime start_time, string area, string type)
        {
            long area_id = Convert.ToInt64(area);
            byte nongyaoType = Convert.ToByte(type);
            StatisticsModel sModel = new StatisticsModel();
            DateTime start_date = start_time.AddDays(value);
            DateTime end_date = start_time.AddDays(value + 30);
            List<DateSaleCountInfo> res = new List<DateSaleCountInfo>();
            res = sModel.GetDateSaleCount(area_id,salesman_id, nongyaoType,nongyao_id, start_date, end_date);
            var ret = Json(res, JsonRequestBehavior.AllowGet);
            return ret;
        }
        public JsonResult GetNongyaoTypeCount(int value, DateTime start_time, string area, string type)
        {
            long area_id = Convert.ToInt64(area);
            byte nongyaoType = Convert.ToByte(type);
            StatisticsModel sModel = new StatisticsModel();
            DateTime start_date = start_time.AddDays(value - 30);
            DateTime end_date = start_time.AddDays(value);
            List<DateSaleCountInfo> res = new List<DateSaleCountInfo>();
            res = sModel.GetDateSaleCount(area_id, salesman_id, nongyaoType, nongyao_id, start_date, end_date);
            var ret = Json(res, JsonRequestBehavior.AllowGet);
            return ret;
        }
        
        [HttpPost]
        public JsonResult DeleteUser(string id)
        {
            AccountModel aModel = new AccountModel();
            bool rst = true;

            rst = aModel.DeleteUser(id);

            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTableData(JQueryDataTableParamModel param)
        {
            StatisticsModel sModel = new StatisticsModel();
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            JqDataTableInfo rst = sModel.GetStatisticsTable(param, Request.QueryString, rootUri, region_id, salesman_id, start_time, end_time, nongyaotype, nongyao_id);
            return Json(rst, JsonRequestBehavior.AllowGet);
        }
        public int FilterStat(string region, string salesman, string start_date, string end_date, string nongyao_type,string nongyao_name)
        {
            region_id = Convert.ToInt64(region);
            if (region_id != 0)
            {
                salesman_id = Convert.ToInt64(salesman);
            }
            
            start_time = Convert.ToDateTime(start_date);
            end_time = Convert.ToDateTime(end_date);
            nongyaotype = Convert.ToByte(nongyao_type);
            nongyao_id = Convert.ToInt64(nongyao_name);
            DateTime start = Convert.ToDateTime(start_date);
            DateTime end = Convert.ToDateTime(end_date);
            var offset = end - start;

            return offset.Days;
        }
        public JsonResult GetSalesman(string regionid)
        {
            SalesmanModel sModel = new SalesmanModel();
            long region_id = Convert.ToInt64(regionid);
            List<SalesmanIDInfo> rst = sModel.GetSalesmanID(region_id);
            return Json(new { salesmanInfo = rst }, JsonRequestBehavior.AllowGet);
        }
    }
}
