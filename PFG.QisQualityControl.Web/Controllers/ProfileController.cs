using PFG.DataSource;
using PFG.Library.Extension;
using PFG.QisQualityControl.Web.Filters;
using PFG.QisQualityControl.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PFG.QisQualityControl.Web.Controllers
{
    [SiteAuthorize]
    public class ProfileController : Controller
    {
        // GET: Profile個人資訊
        public ActionResult Index()
        {
            using (TestEntities db = new TestEntities())
            {
                var currentAccount = User.GetMVCUser().UserId;
                var query = db.Users.Where(x => x.Account == currentAccount).SingleOrDefault();
                if (query != null)
                {
                    var model = new ProfileViewModel()
                    {
                        Account = query.Account,
                        Email = query.Email,
                        Name = query.Name
                    };
                    return PartialView("_Profile", model);
                }
                else
                {
                    return Json(new { success = false, errors = "該用戶已被註銷！請聯繫管理員！" });
                }
            }
        }


        [HttpPost]
        public ActionResult Edit(ProfileViewModel model)
        {
            using (TestEntities db = new TestEntities())
            {
                var currentUser = User.GetMVCUser().UserId;
                var query = db.Users.Where(x => x.Account == model.Account).SingleOrDefault();
                if (query != null)
                {
                    query.DateModified = DateTime.Now;
                    query.ModifierAccount = currentUser;
                    query.Name = model.Name;
                    query.Email = model.Email;
                    db.Entry<Users>(query).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, errors = "該用戶已被註銷！請聯繫管理員！" });
                }
            }
        }
    }
}