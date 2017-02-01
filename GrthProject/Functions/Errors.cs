using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Web;

namespace CMP.Operation.Functions
{
    public static class Errors
    {
        const string sourceName = "CMP.Operation";
        public const string MessageErrorAllowDelete = "This Record is has dependencies , you cant delete that";
        public const string MessageError = "An error occurred while processing your request";
        public static string Write(Exception ex)
        {
#if !DEBUG 
            if (!EventLog.SourceExists(sourceName))
                EventLog.CreateEventSource(sourceName, "Application");
           
            EventLog.WriteEntry(sourceName, string.Format("{0} {1}User: {2}", ex, Environment.NewLine, HttpContext.Current.User.Identity.Name), EventLogEntryType.Error);
#endif
            return MessageError;
        }
        public static void Write(string ErrorMessage)
        {
#if !DEBUG
            if (!EventLog.SourceExists(sourceName))
                EventLog.CreateEventSource(sourceName, "Application");
            EventLog.WriteEntry(sourceName, ErrorMessage,EventLogEntryType.Error);
#endif
        }

    }
}
