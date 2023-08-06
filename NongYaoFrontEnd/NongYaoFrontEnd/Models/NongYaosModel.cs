using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Specialized;
using NongYaoFrontEnd.Models.Library;

namespace NongYaoFrontEnd.Models
{
#region NongYaosInfo
    public class NongYaos
    {
        public string id { get; set; }
        public string name { get; set; }
        public byte parentid { get; set; }
        public DateTime regtime { get; set; }
        public byte deleted { get; set; }
    }
#endregion
    public class NongYaosModel
    {
        private static NongYaoModelDataContext db = new NongYaoModelDataContext();
        public static List<tbl_nongyao> GetNongyaoList()
        {
            return (from m in db.tbl_nongyaos
                    where m.deleted == 0
                    select m).ToList();
        }
    }
}