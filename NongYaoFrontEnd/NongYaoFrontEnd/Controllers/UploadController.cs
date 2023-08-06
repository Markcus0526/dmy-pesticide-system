using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Hosting;
using NongYaoFrontEnd.Models;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Drawing;
using NongYaoFrontEnd.Models.Library;
//using Microsoft.Office.Interop;
using System.Data.OleDb;
using System.Data;

namespace NongYaoFrontEnd.Controllers
{
    public class UploadController : Controller
    {
        #region Image processing
        //[Authorize(Roles = "Administrator,Leader,Normal")]
        [HttpPost]
        [SessionExpireFilter]
        public string UploadImage(HttpPostedFileBase userfile)
        {
            string basePath = Server.MapPath("~/Content/uploads/temp");
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string fileName = "";


            // Some browsers send file names with full path. This needs to be stripped.
            fileName = Path.GetFileName(userfile.FileName);
            var physicalPath = Path.Combine(basePath, fileName);

            if (!System.IO.File.Exists(physicalPath))
            {

            }

            {
                string[] tmpFileName = fileName.Split('.');
                //string another = "";
                if (tmpFileName.Count() > 0)
                {
                    Random rand = new Random();
                    String prefix = String.Format("{0:F0}", rand.NextDouble() * 1000000);

                    fileName = prefix + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + tmpFileName[tmpFileName.Count() - 1];
                    physicalPath = Path.Combine(basePath, fileName);
                }
            }

            userfile.SaveAs(physicalPath);

            //return Json(new { ImgUrl = "Content/uploads/temp/" + fileName }, "text/plain");
            return "Content/uploads/temp/" + fileName;
        }

        //[Authorize(Roles = "Administrator,Leader,Normal")]
        [SessionExpireFilter]
        public string RetrieveCropDialogHtml()
        {
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string basePath = Server.MapPath("~/");
            ViewData["rootUri"] = rootUri;

            if (Request.QueryString["cropfile"] != null)
            {
                string cropfile = Request.QueryString["cropfile"].ToString();
                ViewData["cropfile"] = cropfile;

                if (System.IO.File.Exists(basePath + cropfile))
                {
                    Bitmap img = new Bitmap(basePath + cropfile);
                    if (img.Height < 300)
                    {
                        ViewData["height"] = img.Height;
                        ViewData["width"] = img.Width;
                    }
                    else
                    {
                        ViewData["height"] = 300;
                        ViewData["width"] = Math.Round(img.Width * (300 / (decimal)img.Height));

                    }
                }
            }

            string ret = RenderPartialToString("~/Views/Shared/CropModal.ascx", ViewData);
            return ret;
        }

        //[Authorize(Roles = "Administrator,Leader,Normal")]
        [SessionExpireFilter]
        public static string RenderPartialToString(string controlName, object viewData)
        {
            ViewPage viewPage = new ViewPage() { ViewContext = new ViewContext() };

            viewPage.ViewData = new ViewDataDictionary(viewData);
            viewPage.Controls.Add(viewPage.LoadControl(controlName));

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                {
                    viewPage.RenderControl(tw);
                }
            }

            return sb.ToString();
        }


        //[Authorize(Roles = "Administrator,Leader,Normal")]
        //        [SessionExpireFilter]
        [HttpPost]
        //        [AjaxOnly]
        public string ResizeImage(int x, int y, int w, int h, string imgpath, string kind, string size)
        {
            ImageHelper helper = new ImageHelper();

            return helper.ResizeAndCrop(imgpath, x, y, w, h, kind, size);
        }

        //[Authorize(Roles = "Administrator")]
        [SessionExpireFilter]
        public string RemoveImage(string filename)
        {
            string basePath = Server.MapPath("~/");

            var physicalPath = Path.Combine(basePath, filename);

            // TODO: Verify user permissions

            if (System.IO.File.Exists(physicalPath))
            {
                // The files are not actually removed in this demo
                try
                {
                    System.IO.File.Delete(physicalPath);
                }
                catch (System.Exception ex)
                {
                    CommonModel.WriteLogFile(this.GetType().Name, "RemoveTempImg()", ex.Message.ToString());
                }
            }

            // Return an empty string to signify success
            return "";
        }
        #endregion

        #region File processing
        //[Authorize(Roles = "Administrator,Leader,Normal")]
        [HttpPost]
        [SessionExpireFilter]
        public string UploadFile(HttpPostedFileBase uploadfile)
        {
            string basePath = Server.MapPath("~/Content/uploads/temp");
            string baseOldPath = Server.MapPath("~/");
            string oldpath = "";
            if (Request.Form["uploadpath"] != null)
                oldpath = Request.Form["uploadpath"].ToString();
            var oldPhysicPath = Path.Combine(baseOldPath, oldpath);
            if (System.IO.File.Exists(oldPhysicPath))
            {
                System.IO.File.Delete(oldPhysicPath);
            }

            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string fileName = "";


            // Some browsers send file names with full path. This needs to be stripped.
            fileName = Path.GetFileName(uploadfile.FileName);
            var physicalPath = Path.Combine(basePath, fileName);

            if (System.IO.File.Exists(physicalPath))
            {
                string[] tmpFileName = fileName.Split('.');
                //string another = "";
                if (tmpFileName.Count() > 0)
                {
                    fileName = tmpFileName[0] + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + tmpFileName[tmpFileName.Count() - 1];
                    physicalPath = Path.Combine(basePath, fileName);
                }
            }

            uploadfile.SaveAs(physicalPath);

            //return Json(new { FilePath = "Content/uploads/temp/" + fileName }, "text/plain");
            return "Content/uploads/temp/" + fileName;

        }

        [HttpPost]
        [SessionExpireFilter]
        public string UploadFile1(HttpPostedFileBase uploadpatchfile)
        {
            string basePath = Server.MapPath("~/Content/uploads/temp");
            string baseOldPath = Server.MapPath("~/");
            string oldpath = "";
            if (Request.Form["uploadpatchpath"] != null)
                oldpath = Request.Form["uploadpatchpath"].ToString();
            var oldPhysicPath = Path.Combine(baseOldPath, oldpath);
            if (System.IO.File.Exists(oldPhysicPath))
            {
                System.IO.File.Delete(oldPhysicPath);
            }

            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string fileName = "";


            // Some browsers send file names with full path. This needs to be stripped.
            fileName = Path.GetFileName(uploadpatchfile.FileName);
            var physicalPath = Path.Combine(basePath, fileName);

            if (System.IO.File.Exists(physicalPath))
            {
                string[] tmpFileName = fileName.Split('.');
                //string another = "";
                if (tmpFileName.Count() > 0)
                {
                    fileName = tmpFileName[0] + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + tmpFileName[tmpFileName.Count() - 1];
                    physicalPath = Path.Combine(basePath, fileName);
                }
            }

            uploadpatchfile.SaveAs(physicalPath);

            //return Json(new { FilePath = "Content/uploads/temp/" + fileName }, "text/plain");
            return "Content/uploads/temp/" + fileName;

        }
        #endregion

        #region Excel Processing
        [HttpPost]
        public JsonResult UploadActiveDataFile(HttpPostedFileBase xlsfile)
        {
            string basePath = Server.MapPath("~/Content/uploads/temp");
            string rootUri = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            string fileName = "";


            // Some browsers send file names with full path. This needs to be stripped.
            fileName = Path.GetFileName(xlsfile.FileName);
            var physicalPath = Path.Combine(basePath, fileName);

            if (!System.IO.File.Exists(physicalPath))
            {

            }

            {
                string[] tmpFileName = fileName.Split('.');
                //string another = "";
                if (tmpFileName.Count() > 0)
                {
                    fileName = tmpFileName[0] + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "." + tmpFileName[tmpFileName.Count() - 1];
                    physicalPath = Path.Combine(basePath, fileName);
                }
            }

            xlsfile.SaveAs(physicalPath);

            List<string> clist = GetColumnList(physicalPath);

            //return Json(new { ImgUrl = "Content/uploads/temp/" + fileName }, "text/plain");
            //return "Content/uploads/temp/" + fileName;
            return Json(new { fpath = "Content/uploads/temp/" + fileName, clist = clist });
        }

        OleDbConnection oledbConn;

        private List<string> GetColumnList(string path)
        {

            List<string> rst = new List<string>();
            try
            {
                // need to pass relative path after deploying on server
                /* connection string  to work with excel file. HDR=Yes - indicates 
                   that the first row contains columnnames, not data. HDR=No - indicates 
                   the opposite. "IMEX=1;" tells the driver to always read "intermixed" 
                   (numbers, dates, strings etc) data columns as text. 
                Note that this option might affect excel sheet write access negative. */

                if (Path.GetExtension(path) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=2\"");
                }
                else if (Path.GetExtension(path) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=No;IMEX=1;';");
                }
                else if (Path.GetExtension(path) == ".csv")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties='text;'");
                }
                oledbConn.Open();
                var dtSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string Sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME");

                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                // passing list to drop-down list

                // selecting distict list of Slno 
                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT top 1 * FROM [" + Sheet1 + "]";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        rst.Add(reader.GetValue(i).ToString());
                    }
                }
                reader.Close();
            }
            // need to catch possible exceptions
            catch (Exception ex)
            {
            }
            finally
            {
                oledbConn.Close();
            }

            return rst;
        }

        public List<string> GetExcelData(string path)
        {

            List<string> rst = new List<string>();
            try
            {
                // need to pass relative path after deploying on server
                /* connection string  to work with excel file. HDR=Yes - indicates 
                   that the first row contains columnnames, not data. HDR=No - indicates 
                   the opposite. "IMEX=1;" tells the driver to always read "intermixed" 
                   (numbers, dates, strings etc) data columns as text. 
                Note that this option might affect excel sheet write access negative. */

                if (Path.GetExtension(path) == ".xls")
                {
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=No;IMEX=2\"");
                }
                else if (Path.GetExtension(path) == ".xlsx")
                {
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='Excel 12.0;HDR=No;IMEX=1;';");
                }
                oledbConn.Open();
                var dtSchema = oledbConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string Sheet1 = dtSchema.Rows[0].Field<string>("TABLE_NAME");

                OleDbCommand cmd = new OleDbCommand();
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                // passing list to drop-down list

                // selecting distict list of Slno 
                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT top 1 * FROM [" + Sheet1 + "]";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        rst.Add(reader.GetValue(i).ToString());
                    }
                }
                reader.Close();
            }
            // need to catch possible exceptions
            catch (Exception ex)
            {
            }
            finally
            {
                oledbConn.Close();
            }

            return rst;
        }

        #endregion
    }
}
