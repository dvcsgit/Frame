using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFG.Library.Enums
{
    [Flags]
    public enum EnumOperation
    {
        /// <summary>
        /// 無
        /// </summary>
        None = 0,

        /// <summary>
        /// Index
        /// </summary>
        [Description("查詢")]
        Query = 1 << 1,

        /// <summary>
        /// Create
        /// </summary>
        [Description("新增")]
        Create = 1 << 2,

        /// <summary>
        /// Edit
        /// </summary>
        [Description("編輯")]
        Edit = 1 << 3,

        /// <summary>
        /// Delete
        /// </summary>
        [Description("刪除")]
        Delete = 1 << 4,

        /// <summary>
        /// Export
        /// </summary>
        [Description("匯出")]
        Export = 1 << 5,

        /// <summary>
        /// Upload
        /// </summary>
        [Description("上傳")]
        Upload = 1 << 6,


    }
}
