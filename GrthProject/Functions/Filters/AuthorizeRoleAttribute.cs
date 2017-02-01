using System.Web;
using System.Web.Mvc;
using CMP.Operation.Functions;
using static CMP.Operation.Functions.UserManage;

namespace CMP.Operation.Functions.Filters
{
    public class AuthorizeRoleAttribute
    {
        public class AuthorizeRolesAttribute : AuthorizeAttribute
        {
            private readonly Roles[] userAssignedRoles;

            public AuthorizeRolesAttribute(params Roles[] roles)
            {
                this.userAssignedRoles = roles;
            }
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                bool authorize = false;
                UserManage UM = new UserManage();
                foreach (var roles in userAssignedRoles)
                {
                    authorize = UM.IsUserInRole(httpContext.User.Identity.Name, roles);
                    if (authorize)
                        return authorize;
                }

                return authorize;
            }
            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new RedirectResult("~/Admin/Customer");
            }
        }
    }
}