using PFG.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PFG.Library.Extension
{
    public static class SecurityExtension
    {
        public static MVCUser GetMVCUser(this IPrincipal principal)
        {
            if (principal.Identity is MVCUser)
                return (MVCUser)principal.Identity;
            else
                return new MVCUser();
        }
    }
}
