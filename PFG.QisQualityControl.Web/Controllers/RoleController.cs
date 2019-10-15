using NLog;
using PagedList;
using PFG.DataSource;
using PFG.Library.Extension;
using PFG.QisQualityControl.Web.Const;
using PFG.QisQualityControl.Web.Filters;
using PFG.QisQualityControl.Web.Models;
using PFG.QisQualityControl.Web.Utils;
using PFG.QisQualityControl.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PFG.QisQualityControl.Web.Controllers
{
    [SiteAuthorize]
    public class RoleController : Controller
    {

        protected static Logger _logger = LogManager.GetCurrentClassLogger();


        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 清單查詢
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ActionResult Query(RoleListParameter parameter)
        {
            RoleListViewModel viewModel = new RoleListViewModel() { Parameter = parameter };

            using (TestEntities db = new TestEntities())
            {
                var query = db.Roles.OrderBy(x => x.RoleID).AsQueryable();

                #region 查詢過濾資訊

                if (!string.IsNullOrEmpty(parameter.RoleName))
                {
                    query = query.Where(x => x.RoleName.Contains(parameter.RoleName));
                }

                #endregion
                var resultList = query.Select(y => new RoleItem()
                {
                    Description = y.Description,
                    RoleId = y.RoleID,
                    RoleName = y.RoleName,
                    IsDefault = y.IsDefault
                });

                viewModel.GridList = resultList.ToPagedList(viewModel.Parameter.PageNo, viewModel.Parameter.PageSize);
                return PartialView("_GridList", viewModel);
            }
        }

        /// <summary>
        /// 權限群組管理 - 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            CreateOrEditRoleViewModel viewModel = new CreateOrEditRoleViewModel { };

            viewModel.AllMenu = Menu.GetSubMenu();

            return View("_Create", viewModel);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            CreateOrEditRoleViewModel viewModel = new CreateOrEditRoleViewModel { SaveMode = EnumSaveMode.Update };
            using (TestEntities db = new TestEntities())
            {
                var role = db.Roles.Where(y => y.RoleID == id).SingleOrDefault();
                if (role != null)
                {
                    viewModel.RoleId = role.RoleID;
                    viewModel.RoleName = role.RoleName;
                    viewModel.Description = role.Description;
                    viewModel.AllMenu = Menu.GetSubMenu(id);
                }
                return View("_Edit", viewModel);
            }
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            DetailRoleViewModel viewModel = new DetailRoleViewModel();
            using (TestEntities db = new TestEntities())
            {
                var role = db.Roles.Where(y => y.RoleID == id).SingleOrDefault();
                if (role != null)
                {
                    viewModel.RoleId = role.RoleID;
                    viewModel.RoleName = role.RoleName;
                    viewModel.Description = role.Description;
                    viewModel.AllMenu = Menu.GetSubMenu(id);
                }

                return View("_Detail", viewModel);
            }

        }

        /// <summary>
        /// 儲存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CreateOrEditRoleViewModel model)
        {
            //valid
            //Rule1:驗證是否已經存在相同代碼 (只有新增時 檢查)
            using (TestEntities db = new TestEntities())
            {
                if (model.SaveMode == EnumSaveMode.Create)
                {
                    var isExist = db.Roles.Where(y => y.RoleID == model.RoleId).SingleOrDefault() != null ? true : false;
                    if (isExist)
                    {
                        ModelState.AddModelError("", string.Format("群組代碼{0}已存在", model.RoleId));
                    }
                }

                //Rule2:檢查有沒有勾選權限
                if (model.PermissionList.Count == 0)
                {
                    ModelState.AddModelError("", "請至少勾選一個權限");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var userInfo = User.GetMVCUser();
                        if (model.SaveMode == EnumSaveMode.Create)
                        {
                            Roles role = new Roles
                            {
                                RoleID = model.RoleId,
                                RoleName = model.RoleName,
                                Description = model.Description,
                                CreatorAccount = userInfo.UserId,
                                DateCreated = DateTime.Now,
                                IsDefault = false
                            };

                            //塞權限
                            foreach (var item in model.PermissionList)
                            {
                                //item is "SYS01-Delete"
                                var detailArr = item.Split('-');
                                var permissionID = detailArr[0];
                                var operationID = detailArr[1];
                                var temp = db.PermissionOperations
                                    .Where(x => x.PermissionID == permissionID && x.OperationID == operationID)
                                    .FirstOrDefault();
                                role.PermissionOperations.Add(temp);
                            }

                            db.Entry<Roles>(role).State = EntityState.Added;
                        }
                        else
                        {
                            var role = db.Roles.Where(y => y.RoleID == model.RoleId).SingleOrDefault();
                            role.RoleName = model.RoleName;
                            role.Description = model.Description;
                            role.ModifierAccount = userInfo.UserId;
                            role.DateModified = DateTime.Now;

                            //全砍再新增
                            if (role.PermissionOperations.Count > 0)
                            {
                                role.PermissionOperations.Clear();
                            }

                            //塞權限
                            foreach (var item in model.PermissionList)
                            {
                                //item is "SYS01-Delete"
                                var detailArr = item.Split('-');
                                var permissionID = detailArr[0];
                                var operationID = detailArr[1];
                                var temp = db.PermissionOperations
                                    .Where(x => x.PermissionID == permissionID && x.OperationID == operationID)
                                    .FirstOrDefault();
                                role.PermissionOperations.Add(temp);
                            }

                        }

                        db.SaveChanges();

                        //當編輯成功更新系統的權限快取
                        if (model.SaveMode == EnumSaveMode.Update)
                        {
                            List<string> cacheKeysToDie = new List<string>();
                            // retrieve application Cache enumerator
                            var enumerator = HttpRuntime.Cache.GetEnumerator();

                            // copy all keys that currently exist in Cache
                            while (enumerator.MoveNext())
                            {
                                var tempCache = enumerator.Key.ToString();
                                if (tempCache.Contains(model.RoleId))
                                    cacheKeysToDie.Add(tempCache);
                            }

                            // delete every key from cache

                            foreach (var item in cacheKeysToDie)
                            {
                                HttpRuntime.Cache.Remove(item);
                            }
                        }



                        return Json(new { success = true });

                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        var errors = dbEx.EntityValidationErrors.SelectMany(x => x.ValidationErrors.Select(y => y.PropertyName + ":" + y.ErrorMessage)).FirstOrDefault();
                        ModelState.AddModelError("", "系統發生錯誤:" + errors);
                        _logger.Error( dbEx, "Save - 發生錯誤:{0}");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "系統發生錯誤:" + ex.Message);
                        _logger.Error( ex, "Save - 發生錯誤:{0}");
                    }

                }

                return Json(new { errors = ModelState.GetErrors() });
            }
        }


        /// <summary>
        /// 刪除
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
                        var oldItem = db.Roles.Where(y => y.RoleID == id).SingleOrDefault();
                        if (oldItem != null)
                        {
                            if (oldItem.Users.Count > 0)
                            {
                                oldItem.Users.Clear();
                            }
                            if (oldItem.PermissionOperations.Count > 0)
                            {
                                oldItem.PermissionOperations.Clear();
                            }
                            db.Entry<Roles>(oldItem).State = EntityState.Deleted;
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



    }
}