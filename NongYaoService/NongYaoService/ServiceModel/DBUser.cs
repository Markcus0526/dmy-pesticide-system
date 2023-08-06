using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NongYaoService.ServiceLibrary;
using System.Runtime.Serialization;
using System.Xml;
using System.Net;
using System.IO;

namespace NongYaoService.ServiceModel
{
    public class DBUser
    {
        public long CheckUser(ServiceDBDataContext db, long uid)
        {
            long result = -1;

            try
            {
                tbl_user user = (from m in db.tbl_users
                                 where m.id == uid && m.deleted == 0
                                 select m).FirstOrDefault();
                if (user != null)
                {
                    result = user.id;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return result;
        }

        public bool IsTrialVersion()
        {
            return false;

            /*
            string xmlstr = GetYahooCurrTime();

            try
            {
                if (String.IsNullOrEmpty(xmlstr))
                {
                    return true;
                }

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlstr);

                XmlNodeList tnode = doc.DocumentElement.GetElementsByTagName("Timestamp");
                string currtime = tnode[0].InnerText;

                long currtimestamp = long.Parse(currtime);

                TimeSpan tspan = (new DateTime(2014, 10, 1, 0, 0, 0) - new DateTime(1970, 1, 1, 0, 0, 0, 0));

                if (currtimestamp > tspan.TotalSeconds)
                {
                    return true;
                }

            }
            catch
            {

            }

            return false;
            */
        }
        public static string GetYahooCurrTime()
        {
            string reqUrl = "http://developer.yahooapis.com/TimeService/V1/getTime?appid=YahooDemo";
            string responseBody = null;
            string contentType = "application/text";

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(reqUrl);
            req.Method = "GET";
            req.ContentType = contentType;

            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
            }
            catch
            {
                return null;
            }

            Stream respStream = resp.GetResponseStream();
            if (respStream != null)
            {
                responseBody = new StreamReader(respStream).ReadToEnd();
                return responseBody;
            }

            return null;
        }

        public NongYaoResponseData Login(ServiceDBDataContext db, string userid, string password)
        {
            NongYaoResponseData result = new NongYaoResponseData();

            if (IsTrialVersion() == true)
            {
                result.Result = NONGYAOERROR.ERR_TRIAL_VERSION;

                return result;
            }

            try
            {
                //NongYaoCommon.LogErrors(userid);
                tbl_user user = (from m in db.tbl_users
                                 where m.userid == userid && m.password == password && m.deleted == 0
                                 select m).FirstOrDefault();
                if (user != null)
                {
                    tbl_shop shop = (from m in db.tbl_shops
                                     where m.id == user.shop_id && m.deleted == 0
                                     select m).FirstOrDefault();

                    result.Result = NONGYAOERROR.ERR_SUCCESS;
                    result.Data = new
                    {
                        type = 0,
                        uid = user.id,
                        username = user.name,
                        shop_id = user.shop_id,
                        shop_name = shop.name,
                        role = user.role
                    };
                }
                else
                {
                    var admin = (from m in db.tbl_admins
                                 where m.deleted == 0 && m.userid == userid && m.password == password
                                 select m).FirstOrDefault();
                    if (admin != null)
                    {
                        result.Result = NONGYAOERROR.ERR_SUCCESS;
                        result.Data = new
                        {
                            type = 1,
                            uid = admin.id,
                            username = admin.name,
                            //
                            admin_role = admin.role,
                            region_id = admin.regionid
                        };
                    } else 
                        result.Result = NONGYAOERROR.ERR_NOUSER;
                }
            }
            catch (System.Exception ex)
            {
                NongYaoCommon.LogErrors(ex.ToString());
                result.Result = NONGYAOERROR.ERR_FAILURE;
            }
            return result;
        }
    }
}