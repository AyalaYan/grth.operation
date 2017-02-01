using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMP.Operation.Functions.Filters
{
    public class ExceptionPublisherExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext exceptionContext)
        {
            var exception = exceptionContext.Exception;
            //var request = exceptionContext.HttpContext.Request;
            
            Errors.Write(exception);
        }
    }
}