using NLog;
using PagedList;
using PFG.DataSource;
using PFG.Library.Extension;
using PFG.QisQualityControl.Web.Filters;
using PFG.QisQualityControl.Web.Models;
using PFG.QisQualityControl.Web.Utils;
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
    public class AccountController : Controller
    {
        protected static Logger _logger = LogManager.GetCurrentClassLogger();





        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Query(AccountParameters parameters)
        {
            using (TestEntities db = new TestEntities())
            {
                AccountGridListViewModel viewModel = new AccountGridListViewModel
                {
                    Parameters = parameters
                };
                var query = db.Users.AsQueryable();
                if (!string.IsNullOrEmpty(parameters.NotesID))
                {
                    query = query.Where(x => x.Account.Contains(parameters.NotesID));
                }
                if (!string.IsNullOrEmpty(parameters.DisplayName))
                {
                    query = query.Where(x => x.Name.Contains(parameters.DisplayName));
                }
                var result = query.Select(x => new UserViewModel()
                {
                    Account = x.Account,
                    CreatorAccount = x.CreatorAccount,
                    DateCreated = x.DateCreated,
                    Email = x.Email,
                    Name = x.Name
                });
                viewModel.GridList = result.OrderBy(x => x.Account).ToPagedList(parameters.PageNo, parameters.PageSize);
                return PartialView("_GridList", viewModel);
            }
        }


        /// <summary>
        /// 帳號管理 - 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            AccountViewModel viewModel = new AccountViewModel
            {
                SaveMode = EnumSaveMode.Create
            };
            return View("_Create", viewModel);
        }

        public ActionResult Save(AccountViewModel model)
        {
            using (TestEntities db = new TestEntities())
            {
                var userInfo = User.GetMVCUser();

                if (ModelState.IsValid)
                {
                    try
                    {

                        if (model.SaveMode == EnumSaveMode.Create)
                        {
                            //自訂驗證
                            if (db.Users.Where(x => x.Account == model.Account).Any())
                            {
                                ModelState.AddModelError("", "已有相同的帳號");
                            }

                            if (ModelState.IsValid)
                            {

                                var newItem = new Users
                                {
                                    CreatorAccount = userInfo.UserId,
                                    DateCreated = DateTime.Now,
                                    Account = model.Account,
                                    Email = model.Email,
                                    Name = model.Name,
                                    PasswordHash = model.PasswordHash
                                };
                                //新增權限
                                var role = db.Roles.Where(x => model.UserRoles.Contains(x.RoleID)).ToList();
                                newItem.Roles = role;
                                db.Entry<Users>(newItem).State = EntityState.Added;
                                db.SaveChanges();

                                return Json(new { success = true });
                            }

                        }
                        else
                        {

                            var oldItem = db.Users.Where(y => y.Account == model.Account).SingleOrDefault();
                            if (oldItem != null)
                            {
                                oldItem.PasswordHash = model.ConfirmPassword;
                                oldItem.Name = model.Name;
                                oldItem.Email = model.Email;
                                oldItem.ModifierAccount = userInfo.UserId;
                                oldItem.DateModified = DateTime.Now;

                                //權限 全砍再新增
                                if (oldItem.Roles.Count > 0)
                                {
                                    oldItem.Roles.Clear();
                                }

                                if (model.UserRoles != null)
                                {
                                    var role = db.Roles.Where(x => model.UserRoles.Contains(x.RoleID)).ToList();
                                    oldItem.Roles = role;
                                }
                                db.Entry<Users>(oldItem).State = EntityState.Modified;
                                db.SaveChanges();
                                return Json(new { success = true });
                            }
                            else
                            {
                                ModelState.AddModelError("", "此帳號不存在");
                            }

                        }



                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Save - 發生錯誤：{0}", ex.Message);
                        ModelState.AddModelError("", "系統發生錯誤");
                    }
                }

                return Json(new { errors = ModelState.GetErrors() });
            }
        }

        /// <summary>
        /// 帳號管理 - 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TestEntities db = new TestEntities())
                    {
                        var oldItem = db.Users.Where(y => y.Account == id).SingleOrDefault();
                        if (oldItem != null)
                        {
                            if (oldItem.Roles.Count > 0)
                            {
                                oldItem.Roles.Clear();
                            }
                            db.Entry<Users>(oldItem).State = EntityState.Deleted;
                            db.SaveChanges();
                            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            ModelState.AddModelError("", "資料不存在,可能已經被刪除!?");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "系統發生錯誤");
                    _logger.Error(ex, "Delete - 發生錯誤:{0}");
                }

            }

            return Json(new { errors = ModelState.GetErrors() }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 帳號管理 - 詳細內容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(string id)
        {
            using (TestEntities db = new TestEntities())
            {
                DetailAccountViewModel viewModel = new DetailAccountViewModel();
                var user = db.Users.Where(y => y.Account == id).SingleOrDefault();
                viewModel.Account = user.Account;
                viewModel.Name = user.Name;
                viewModel.Email = user.Email;
                viewModel.UserRoles = user.Roles.Select(x => x.RoleID).ToList();
                viewModel.PasswordHash = user.PasswordHash;
                viewModel.ModifyTime = user.DateModified;
                return View("_Detail", viewModel);
            }

        }

        /// <summary>
        /// 帳號管理 - 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string id)
        {
            using (TestEntities db = new TestEntities())
            {
                AccountViewModel viewModel = new AccountViewModel { };
                var user = db.Users.Where(y => y.Account == id).SingleOrDefault();
                viewModel.SaveMode = EnumSaveMode.Update;
                viewModel.Account = user.Account;
                viewModel.Email = user.Email;
                viewModel.Name = user.Name;
                viewModel.PasswordHash = user.PasswordHash;
                viewModel.ConfirmPassword = user.PasswordHash;
                viewModel.UserRoles = user.Roles.Select(x => x.RoleID).ToList();
                return View("_Create", viewModel);
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            return View("ChangePassword", model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            using (TestEntities db = new TestEntities())
            {
                var current_user = User.GetMVCUser().UserId;
                var item = db.Users.SingleOrDefault(x => x.Account == current_user);
                if (item != null)
                {
                    item.PasswordHash = model.NewPassword;
                    db.Entry<Users>(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    return Json(new { success = false, errors = "用戶信息已失效，請重新登入后再試！" });
                }
                return Json(new { success = true });
            }
        }

        public JsonResult GetOldPwd(string oldPwd)
        {
            using (TestEntities db = new TestEntities())
            {
                var userInfo = User.GetMVCUser();
                var user = db.Users.SingleOrDefault(x => x.Account == userInfo.UserId);
                if (user != null)
                {
                    if (user.PasswordHash == oldPwd)
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json("原密碼輸入錯誤", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("該用戶信息已失效，請重新登入后再試！", JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetNewPwd(string newPwd)
        {
            using (TestEntities db = new TestEntities())
            {
                var userInfo = User.GetMVCUser();
                var user = db.Users.SingleOrDefault(x => x.Account == userInfo.UserId);
                if (user != null)
                {
                    if (user.PasswordHash == newPwd)
                    {
                        return Json("新密碼和原密碼不能相同", JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("該用戶信息已失效，請重新登入后再試！", JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}