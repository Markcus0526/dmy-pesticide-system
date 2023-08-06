using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NongYaoBackend.Models.Library;

namespace NongYaoBackend.Models
{
    public class UserModel
    {
        NongYaoModelDataContext db = new NongYaoModelDataContext();

        public static tbl_user GetUserInfo(long id)
        {
            NongYaoModelDataContext db = new NongYaoModelDataContext();
            return (from m in db.tbl_users
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
        }

        public static string GetUserName(long id)
        {
            var userInfo = GetUserInfo(id);
            if(userInfo != null)
                return userInfo.name;
            else
                return "";
        }

    }
}
