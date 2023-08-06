using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace NongYaoBackend.Models
{
    public class CustomRoleProvider : RoleProvider
    {

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string cookieName = FormsAuthentication.FormsCookieName;// FormsAuthentication.FormsCookieName;

            HttpCookie authCookie = HttpContext.Current.Request.Cookies[cookieName];

            FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            /* string[] roles = authTicket.UserData.Split(new char[] { '|' });

             FormsIdentity fId = (FormsIdentity)HttpContext.Current.User.Identity;

             FormsAuthenticationTicket authTicket = fId.Ticket;

             //FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(tmpcookie.Value);*/
            string[] udata = authTicket.UserData.Split(new Char[] { '|' });

            if (udata.Count() > 2)
            {
                string[] roles = udata[0].Split(new Char[] { ',' });
                return roles;
            }

            return null;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}