using Castalia.WEB.Models;
using System;
using System.Text;
using System.Web.Mvc;

namespace Castalia.WEB.Heplers
{
    /// <summary>
    /// allows to make pagination
    /// </summary>
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            //if items only for one page dint display pagination
            if (pagingInfo.TotalPages > 1)
            {
                for (int i = 1; i <= pagingInfo.TotalPages; i++)
                {
                    TagBuilder tag = new TagBuilder("a");
                    tag.MergeAttribute("href", pageUrl(i));
                    tag.InnerHtml = i.ToString();
                    tag.AddCssClass("btn-primary");
                    //if is selected add class selected
                    if (i == pagingInfo.CurrentPage)
                        tag.AddCssClass("selected");
                    result.Append(tag.ToString());
                }
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}