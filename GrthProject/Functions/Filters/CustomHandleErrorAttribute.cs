using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Web.Mvc;
using CMP.Operation.Functions;

namespace CMP.Operation.Functions.Filters
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {

#if DEBUG
        public override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
            Errors.Write(filterContext.Exception);
            base.OnException(filterContext);
        }
#endif
    }
}