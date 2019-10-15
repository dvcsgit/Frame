using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace PFG.QisQualityControl.Web.Utils
{
    public class PageUtils
    {
        public static PagedListRenderOptions GetDefaultPagerOptions(bool isAjax)
        {

            PagedListRenderOptions result = null;
            var options = new PagedListRenderOptions()
            {
                DisplayLinkToFirstPage = PagedListDisplayMode.Always,
                DisplayLinkToLastPage = PagedListDisplayMode.Always,
                DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                DisplayLinkToNextPage = PagedListDisplayMode.Always,
                DisplayLinkToIndividualPages = true,
                ContainerDivClasses = null,
                //UlElementClasses = new[] { "pager" },
                ClassToApplyToFirstListItemInPager = null,
                ClassToApplyToLastListItemInPager = null,
                LinkToPreviousPageFormat = "上一頁",
                LinkToNextPageFormat = "下一頁",
                LinkToFirstPageFormat = "首頁",
                LinkToLastPageFormat = "尾頁"
            };

            if (isAjax)
            {
                result = PagedListRenderOptions.EnableUnobtrusiveAjaxReplacing(options,AjaxUtils.GetAjaxOptions());
            }
            else
            {
                result = options;
            }

            return options;
        }
    }
}