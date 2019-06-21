using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Castalia.WEB.Infrastructure
{
    public class NotConfirmedNameExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled &&
                    filterContext.Exception is NotConfirmedNameException)
            {
                string val = ((NotConfirmedNameException)filterContext.Exception).Message;
                filterContext.Result = new ViewResult
                {
                    ViewName = "NotConfirmedNameError",
                    ViewData = new ViewDataDictionary<string>(val)
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }

    class NotConfirmedNameException : Exception
    {
        public NotConfirmedNameException(string message)
            : base(message)
        { }
    }
}