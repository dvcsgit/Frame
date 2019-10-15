using PFG.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PFG.QisQualityControl.Web.Models
{
    /// <summary>
    /// 到View會有
    /// </summary>
    public class UserModel
    {
        public bool IsUserAuthenticated { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
        public EnumOperation Operation { get; set; }
        public string FacId { get; set; }
        public string Dept { get; set; }
        public string ClassID { get; set; }
    }
}