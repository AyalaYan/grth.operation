using CMP.Operation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMP.Operation.Functions.Filters
{
    public class GetCurrentAdminMenu : FilterAttribute, IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
          //  throw new NotImplementedException();
        }

        //Eventually didn't needed ..
        public void OnResultExecuting(ResultExecutingContext filterContext)
        {

          if (filterContext.Controller.TempData["MenuLevl"] == null)
            {
                string actionName = filterContext.RouteData.Values["action"].ToString();
                string controllerName = filterContext.RouteData.Values["controller"].ToString();
                List<AdminMenu> MenuLevl = AdminMenu.AdminMenuData.GetLevel(actionName, controllerName);
                filterContext.Controller.TempData["MenuLevl"] = MenuLevl;
                if (MenuLevl != null)
                {
                    filterContext.Controller.TempData["CurrentParentMenuID"] = MenuLevl.First().ID;
                    filterContext.Controller.TempData["CurrentMenuID"] = MenuLevl.Last().ID;
                }
            }
        }
    }
}