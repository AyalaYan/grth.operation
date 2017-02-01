using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMP.Operation.Functions.Extensions
{
    public static class ModelStateExtension
    {
        public static string Message(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                string messages = string.Join("", modelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => " <li> " + x.ErrorMessage.ToString() + "</li>"));
                if (messages != "")
                    messages = "<ul>" + messages + "</ul>";
                messages = "<div class='validation-summary-errors'>Form is not valid! Please correct it and try again. <br>  " + messages + "</div>";
                return messages;
            }
            return "";
        }
    }
}