using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Ajax;

namespace PFG.QisQualityControl.Web.Utils
{
    public class AjaxUtils
    {
        /// <summary>
        /// 預設的Ajax From Options
        /// </summary>
        /// <returns></returns>
        public static AjaxOptions GetAjaxOptions()
        {
            var updateTargetId = "divGridView";
            return new AjaxOptions
            {
                UpdateTargetId = updateTargetId,
                InsertionMode = InsertionMode.Replace,
                OnBegin = "$.ShowGridLoading('" + updateTargetId + "')",
                OnComplete = "$.HideGridLoading('" + updateTargetId + "')"
            };
        }

        public static AjaxOptions GetAjaxOptions(string updateTargetId)
        {
            return new AjaxOptions
            {
                UpdateTargetId = updateTargetId,
                InsertionMode = InsertionMode.Replace,
                OnBegin = "$.ShowGridLoading('" + updateTargetId + "')",
                OnComplete = "$.HideGridLoading('" + updateTargetId + "')"
            };
        }
    }
}