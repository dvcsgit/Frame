using Newtonsoft.Json;
using PFG.Library.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace PFG.Library.Models
{
    [Serializable]
    public class MVCUser : IIdentity
    {

        public MVCUser() { }
        public MVCUser(string name, string userId, string roleName, string facId,string classID,string dept,string tel)
        {
            this.Name = name;
            this.UserId = userId;
            this.RoleName = roleName;
            this.FacId = facId;
            this.ClassID =ClassID;
            this.Dept =dept;
            this.Tel = tel;
        }

        public MVCUser(string name, AuthenticationTicketUserData userData)
        {
            if (userData == null) throw new ArgumentNullException("userData");
            this.Name = name;
            this.UserId = userData.UserId;
            this.RoleName = userData.RoleName;
            this.Roles = userData.Roles;
            this.ClassID = userData.ClassID;
            this.Dept = userData.Dept;
            this.FacId = userData.FacID;
            this.Tel = userData.Tel;
        }

        /// <summary>
        /// web global use
        /// </summary>
        /// <param name="ticket"></param>
        public MVCUser(FormsAuthenticationTicket ticket)
            : this(ticket.Name, AuthenticationTicketUserData.FromString(ticket.UserData))
        {
            if (ticket == null) throw new ArgumentNullException("ticket");
        }

        public string AuthenticationType
        {
            get { return "MVCForms"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name { get; private set; }
        public string RoleName { get; private set; }
        public string FacId { get; private set; }
        public List<string> Roles { get; private set; }
        public string UserId { get; private set; }
        public string Tel { get; private set; }
        public string Dept { get; private set; }
        public string ClassID { get; private set; }
        public Dictionary<string, EnumOperation> PermissionOperation { get; set; }
    }

    public class AuthenticationTicketUserData
    {
        public string Name { get; set; }
        public string FacID { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public List<string> Roles { get; set; }

        public string Tel { get; private set; }
        public string Dept { get; private set; }
        public string ClassID { get; private set; }
        /// <summary>
        /// 序列化成JSON
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 還原成物件
        /// </summary>
        /// <param name="userContextData"></param>
        /// <returns></returns>
        public static AuthenticationTicketUserData FromString(string userContextData)
        {
            return JsonConvert.DeserializeObject<AuthenticationTicketUserData>(userContextData);
        }

        public AuthenticationTicketUserData()
        {
            this.Roles = new List<string>();
        }
    }
}
