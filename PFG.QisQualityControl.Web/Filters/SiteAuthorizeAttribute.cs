using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PFG.QisQualityControl.Web.Filters
{
    public class SiteAuthorizeAttribute:AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                httpContext.Response.Redirect(FormsAuthentication.LoginUrl + "?RequestUrl=" +
                    HttpUtility.UrlEncode(httpContext.Request.RawUrl));
            }
            return base.AuthorizeCore(httpContext);
        }
    }
}