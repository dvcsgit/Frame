using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PFG.QisQualityControl.Web.Const
{
    public class Configuration
    {
        /// <summary>
        /// 取得Web.config中的AppSetting參數
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string AppSettings(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

    }
}