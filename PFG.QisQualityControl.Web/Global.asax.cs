using PFG.Library.Authentication;
using PFG.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace PFG.QisQualityControl.Web
{
    /// <summary>
    /// 必成QIS系統
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {

        public override void Init()
        {
            this.PostAuthenticateRequest += this.PostAuthenticateRequestHandler;
            base.Init();
        }

        private void PostAuthenticateRequestHandler(object sender, EventArgs e)
        {
            HttpCookie authCookie = this.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie!=null&&!string.IsNullOrEmpty(authCookie.Value))
            {
                //var formsAuthentication = DependencyResolver.Current.GetService<IFormsAuthentication>();
                IFormsAuthentication formsAuthentication = new DefaultFormsAuthentication();

                var ticket = formsAuthentication.Decrypt(authCookie.Value);
                try
                {
                    var mvcUser = new MVCUser(ticket);
                    this.Context.User = new GenericPrincipal(mvcUser, null);
                    formsAuthentication.SetAuthCookie(this.Context, ticket);
                }
                catch
                {
                    
                    //清掉Session重登
                    formsAuthentication.Signout();
                    HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                }

            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
