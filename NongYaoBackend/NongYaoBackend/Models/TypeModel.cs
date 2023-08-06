using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NongYaoBackend.Models
{
    public class NongyaoListInfo
    {
        public long uid { get; set; }
        public string name { get; set; }
        public string usingname { get; set; }
    }
    
    public class TYPE_SUBMITSTATUS
    {
        public const string DUPLICATE_NAME = "操作失败： 农药分类重复！";
        public const string SUCCESS_SUBMIT = "";
        public const string ERROR_SUBMIT = "操作失败";
    }

    public class TypeModel
    {
        NongYaoModelDataContext db = new NongYaoModelDataContext();

        public List<tbl_nongyao> GetNongyaoAtParent(int i)
        {
            return (from m in db.tbl_nongyaos
                        where m.deleted == 0 && m.parentid == i
                        select m).ToList();
        }
        public List<tbl_nongyao> GetParentNongyaos()
        {
            return (from m in db.tbl_nongyaos
                    where m.deleted == 0 && m.parentid == 0
                    select m).ToList();
        }
        public List<tbl_nongyao> GetNongyaoAllAtParent(int i)
        {
            List<tbl_nongyao> nongyaos = new List<tbl_nongyao>();
            return (from m in db.tbl_nongyaos
                    where m.deleted == 0 && m.parentid == i
                    select m).ToList();
        }
        public List<tbl_nongyao> GetXiaoNongyao()
        {
            return (from m in db.tbl_nongyaos
                        where m.deleted == 0 && m.parentid != 0
                        select m).ToList();
        }
        public List<NongyaoListInfo> GetNongyaoAtParentList(int i)
        {
            List<NongyaoListInfo> ret = new List<NongyaoListInfo>();
            List<tbl_nongyao> nongyao_list = (from m in db.tbl_nongyaos
                    where m.deleted == 0 && m.parentid == i
                    select m).ToList();
            foreach (tbl_nongyao item in nongyao_list)
            {
                NongyaoListInfo nli = new NongyaoListInfo();
                nli.uid = item.id;
                nli.name = item.name;
                ret.Add(nli);
            }
            return ret;
        }
        public List<NongyaoListInfo> GetXiaoNongyaoList()
        {
             List<NongyaoListInfo> ret = new List<NongyaoListInfo>();
            List<tbl_nongyao> nongyao_list = (from m in db.tbl_nongyaos
                    where m.deleted == 0 && m.parentid != 0
                    select m).ToList();
            foreach (tbl_nongyao item in nongyao_list)
            {
                NongyaoListInfo nli = new NongyaoListInfo();
                nli.uid = item.id;
                nli.name = item.name;
                ret.Add(nli);
            }
            return ret;
        }
        
        public List<tbl_nongyao> GetNongyaoNotParent()
        {
            return (from m in db.tbl_nongyaos
                    where m.deleted == 0 && m.parentid != 0
                    select m).ToList();
        }

        public List<List<tbl_nongyao>> GetNongyaoAtParent(List<tbl_nongyao> parentids)
        {
            List<List<tbl_nongyao>> list = new List<List<tbl_nongyao>>();
            for (int i = 0; i < parentids.Count; i++)
            {
                tbl_nongyao parent = parentids.ElementAt(i);
                List<tbl_nongyao> children = (from m in db.tbl_nongyaos
                                              where m.deleted == 0 && m.parentid == parent.id
                                              select m).ToList();
                list.Add(children);
            }

            return list;
        }

        public List<string> GetNames(List<tbl_nongyao> list)
        {
            return (from m in list
                    select m.name).ToList();
        }

        public List<string> GetNongyaoAtParentToString(int i)
        {
            return GetNames(GetNongyaoAtParent(i));
        }
        public List<string> GetXiaoNongyaoName()
        {
            return GetNames(GetXiaoNongyao());
        }

        public List<List<string>> GetNongyaoAtParentToString(List<tbl_nongyao> parentids)
        {
            List<List<string>> list = new List<List<string>>();
            for (int i = 0; i < parentids.Count; i++)
            {
                tbl_nongyao parent = parentids.ElementAt(i);
                List<tbl_nongyao> children = (from m in db.tbl_nongyaos
                                              where m.deleted == 0 && m.parentid == parent.id
                                              select m).ToList();
                list.Add(GetNames(children));
            }

            return list;
        }

        public tbl_nongyao GetNongyaoItem(int id)
        {
            return (from m in db.tbl_nongyaos
                    where m.deleted == 0 && m.id == id
                    select m).FirstOrDefault();
        }
        
        public tbl_nongyao GetNongyaoItem(string name)
        {
            return (from m in db.tbl_nongyaos
                    where m.deleted == 0 && m.name == name
                    select m).FirstOrDefault();
        }

        public long GetParentId(string name)
        {
            tbl_nongyao item = (from m in db.tbl_nongyaos
                                where m.deleted == 0 && m.name == name
                                select m).FirstOrDefault();
            if (item != null)
                return item.id;
            else
                return 0;
        }

        public bool IsConsistent(string name)
        {
            tbl_nongyao item = (from m in db.tbl_nongyaos
                        where m.deleted == 0 && m.name == name
                        select m).FirstOrDefault();
            if (item != null)
                return false;
            else
                return
                    true;
        }

        public string InsertType(string name, string parent)
        {
            try
            {
                if (IsConsistent(name))
                {
                    long parentid = GetParentId(parent);
                    tbl_nongyao newitem = new tbl_nongyao();

                    newitem.name = name;
                    newitem.parentid = parentid;
                    newitem.regtime = DateTime.Now;
                    newitem.deleted = 0;

                    db.tbl_nongyaos.InsertOnSubmit(newitem);
                    db.SubmitChanges();

                    return TYPE_SUBMITSTATUS.SUCCESS_SUBMIT;
                }
                else
                {
                    return TYPE_SUBMITSTATUS.DUPLICATE_NAME;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TypeModel", "InsertType()", e.ToString());
                return TYPE_SUBMITSTATUS.ERROR_SUBMIT;
            }
        }

        public string UpdateType(string name, string parent, string originname, string originparent)
        {
            try
            {
                if (IsConsistent(name) || name == originname)
                {
                    long originparentid = GetParentId(originparent);
                    var edititem = (from m in db.tbl_nongyaos
                                    where m.deleted == 0 && m.name == originname && m.parentid == originparentid
                                    select m).FirstOrDefault();
                    if (edititem != null)
                    {
                        long parentid = GetParentId(parent);
                        edititem.name = name;
                        edititem.parentid = parentid;
                        db.SubmitChanges();

                        return TYPE_SUBMITSTATUS.SUCCESS_SUBMIT;
                    }
                }
                else
                {
                    return TYPE_SUBMITSTATUS.DUPLICATE_NAME;
                }
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TypeModel", "UpdateType()", e.ToString());
                return TYPE_SUBMITSTATUS.ERROR_SUBMIT;
            }

            return TYPE_SUBMITSTATUS.ERROR_SUBMIT;
        }

        public bool DeleteType(string name, string parent)
        {
            try
            {
                long parentid = GetParentId(parent);
                if (parentid != 0)
                {
                    var delitem = (from m in db.tbl_nongyaos
                                   where m.deleted == 0 && m.name == name && m.parentid == parentid
                                   select m).FirstOrDefault();

                    if (delitem != null)
                    {
                        delitem.deleted = 1;
                        db.SubmitChanges();

                        return true;
                    }
                }
                else
                {
                    var delitem = (from m in db.tbl_nongyaos
                                   where m.deleted == 0 && m.name == name && m.parentid == 0
                                   select m).FirstOrDefault();

                    if (delitem != null)
                    {
                        var children = (from m in db.tbl_nongyaos
                                        where m.deleted == 0 && m.parentid == delitem.id
                                        select m);
                        foreach (var child in children)
                        {
                            child.deleted = 1;
                            db.SubmitChanges();
                        }

                        delitem.deleted = 1;
                        db.SubmitChanges();

                        return true;
                    }
                } 
            }
            catch (Exception e)
            {
                CommonModel.WriteLogFile("TypeModel", "DeleteType()", e.ToString());
                return false;
            }

            return false;
        } 

    }
}
