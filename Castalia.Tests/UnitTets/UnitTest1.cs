using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Castalia.WEB.Heplers;
using Castalia.WEB.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Castalia.Tests.UnitTets
{
    [TestClass]
    public class UnitTest1
    {
      

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            // Creating object PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Delegat configuration with lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Action
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

    }
}
