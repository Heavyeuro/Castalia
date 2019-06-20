using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Castalia.WEB.Infrastructure
{
    public class BlockedStudentExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled &&
                    filterContext.Exception is BlockedStudentException)
            {
                string val = ((BlockedStudentException)filterContext.Exception).Message;
                filterContext.Result = new ViewResult
                {
                    ViewName = "BlockedStudentError",
                    ViewData = new ViewDataDictionary<string>(val)
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }

    class BlockedStudentException : Exception
    {
        public BlockedStudentException(string message)
            : base(message)
        { }
    }
}