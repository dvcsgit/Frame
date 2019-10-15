using PFG.QisQualityControl.Web.Filters;
using System.Web;
using System.Web.Mvc;

namespace PFG.QisQualityControl.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new SystemPermissionAttribute());
        }
    }
}
