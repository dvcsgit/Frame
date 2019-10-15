using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PFG.DataSource;

namespace PFG.QisQualityControl.Web.Utils
{
    public static class SelectListUtils
    {
        /// <summary>
        /// 多選
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static MultiSelectList ToSelectList<T>(this IEnumerable<T> collection, List<string> selectedValue)
        {
            return new MultiSelectList(collection, "Value", "Text", selectedValue);
        }
        /// <summary>
        /// 角色選單 多選
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetRoleOptions()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            using (TestEntities db = new TestEntities())
            {
                items = db.Roles.Select(x => new SelectListItem() { Text = x.RoleName, Value = x.RoleID }).ToList();
            }
            return items.AsEnumerable();
        }
    }
}