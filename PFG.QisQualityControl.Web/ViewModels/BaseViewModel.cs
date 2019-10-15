using PFG.QisQualityControl.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PFG.QisQualityControl.Web.ViewModels
{
    /// <summary>
    /// 基礎視圖 
    /// 給其他view拿來記住 目前分頁大小 目前所在分頁
    /// </summary>
    /// <typeparam name="T">排序欄位</typeparam>
    public abstract class BasicParameters
    {
        /// <summary>
        /// 目前所在分頁
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 分頁大小
        /// </summary>
        public int PageSize { get; set; }

        public BasicParameters()
        {
            this.PageNo = 1;
            var pageSize = 10;
            this.PageSize = pageSize;
        }
    }


    /// <summary>
    /// 基礎視圖 有排序
    /// 給其他view拿來記住 目前分頁大小 目前所在分頁
    /// </summary>
    /// <typeparam name="T">排序欄位</typeparam>
    public abstract class BasicParameters<T>
    {
        /// <summary>
        /// 目前所在分頁
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 分頁大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 目前排序欄位
        /// </summary>
        public T SortingField { get; set; }

        /// 目前排序方向
        /// </summary>
        //public SortingDirection SortingDirection { get; set; }

        public BasicParameters()
        {
            this.PageNo = 1;
            var pageSize = 10;
            this.PageSize = pageSize;
        }
    }

    /// <summary>
    /// 基礎視圖 
    /// 提供給 新增 修改 回傳的訊息註記
    /// </summary>
    public abstract class BasicSaveMode
    {
        /// <summary>
        /// 新增或編輯
        /// </summary>
        public EnumSaveMode SaveMode { get; set; }
    }
}