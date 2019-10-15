using PFG.Library.Enums;
using PFG.Library.Extension;
using PFG.Library.Models;
using PFG.QisQualityControl.Web.Models;
using PFG.QisQualityControl.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PFG.QisQualityControl.Web.Filters
{
    public class SystemPermissionAttribute: ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                UserModel userModel;
                if (filterContext.Controller.ViewBag.UserModel == null)
                {
                    userModel = new UserModel();
                    filterContext.Controller.ViewBag.UserModel = userModel;
                }
                else
                {
                    userModel = filterContext.Controller.ViewBag.UserModel as UserModel;
                }

                if (filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    MVCUser mvcUser = filterContext.HttpContext.User.GetMVCUser();
                    userModel.IsUserAuthenticated = mvcUser.IsAuthenticated;
                    userModel.Name = mvcUser.Name;
                    userModel.RoleName = mvcUser.RoleName;
                    userModel.FacId = mvcUser.FacId;
                    userModel.Dept = mvcUser.Dept;
                    userModel.ClassID = mvcUser.ClassID;
                    

                    var roleKey = "RoleKey";
                    foreach (var item in mvcUser.Roles)
                    {
                        roleKey += item;
                    }

                    var controllerName = filterContext.RouteData.Values["controller"].ToString();
                    filterContext.Controller.ViewBag.ReturnUrl = controllerName;
                    //只能從Controller 因設計上是用Controller去切功能模組
                    var permissionOperationMap = HttpRuntime.Cache.GetOrInsert<Dictionary<string, EnumOperation>>(roleKey, () => PermissionUtils.GetPermissionOperationMap(mvcUser.Roles));

                    userModel.Operation = permissionOperationMap.ContainsKey(controllerName) ?
                        permissionOperationMap[controllerName] : EnumOperation.None;


                }

                base.OnActionExecuted(filterContext);
            }


        }
    }
}