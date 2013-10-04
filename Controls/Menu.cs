using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

public class MenuItems
{
    public string linkText { get; set; }
    public string actionName { get; set; }
    public string controllerName { get; set; }
}

public static class HelperMethods
{

    static string MenuCSS(HtmlHelper helper)
    {
        TagBuilder tb = new TagBuilder("style");
        tb.MergeAttribute("type", "text/css");

        string css = @"
.menu {
    text-align: left;
    width: auto;
    padding: 0;
    margin: 0;
}
    .menu ul {
        padding: 0;
        margin: 0;
        list-style-type:none;
    }

        .menu ul li {
            margin: 0;
            padding: 0;
        }

            .menu ul li a {
                display: block;
                margin: 0;
                padding: 3px 6px;
                text-decoration: none;
            }";
        tb.InnerHtml = css;
        return tb.ToString();
    }
    
    public static MvcHtmlString Menu(this HtmlHelper helper, List<MenuItems> menuItems, bool IncludeBasicStyle)
    {
        StringBuilder sb = new StringBuilder();
        TagBuilder UL = new TagBuilder("ul");
        TagBuilder DIV = new TagBuilder("div");

        foreach (var item in menuItems)
            sb.Append(GetListItem(helper, item.linkText, item.actionName, item.controllerName).ToString());
        DIV.MergeAttribute("class", "menu");
        UL.InnerHtml = sb.ToString();
        DIV.InnerHtml = UL.ToString();
        return MvcHtmlString.Create(DIV.ToString() + (IncludeBasicStyle ? MenuCSS(helper) : ""));
    }

    public static MvcHtmlString ListItem(this HtmlHelper helper, string linkText, string actionName, string controllerName)
    {
        TagBuilder builder = GetListItem(helper, linkText, actionName, controllerName);
        return MvcHtmlString.Create(builder.ToString());
    }

    static TagBuilder GetListItem(HtmlHelper helper, string linkText, string actionName, string controllerName)
    {
        var linkHtml = HtmlHelper.GenerateLink(helper.ViewContext.RequestContext, helper.RouteCollection, linkText, null, actionName, controllerName, null, null);

        var builder = new TagBuilder("li");

        var isCurrent = IsCurrentAction(helper, actionName, controllerName);

        if (isCurrent)
            builder.MergeAttribute("class", "current");

        builder.InnerHtml = linkHtml;
        return builder;
    }

    static bool IsCurrentAction(HtmlHelper helper, string actionName, string controllerName)
    {
        string currentControllerName = (string)helper.ViewContext.RouteData.Values["controller"];
        string currentActionName = (string)helper.ViewContext.RouteData.Values["action"];
        return (currentControllerName.Equals(controllerName, StringComparison.CurrentCultureIgnoreCase) && currentActionName.Equals(actionName, StringComparison.CurrentCultureIgnoreCase));
    }

    public static string BeginListItem(this HtmlHelper helper, string linkText, string actionName, string controllerName)
    {
        string result;

        TagBuilder builder = GetListItem(helper, linkText, actionName, controllerName);
        result = builder.ToString(TagRenderMode.StartTag);
        result += builder.InnerHtml;

        return result;
    }

    public static void EndListItem(this HtmlHelper helper)
    {
        HttpResponseBase httpResponse = helper.ViewContext.HttpContext.Response;
        httpResponse.Write("</li>");
    }

}
