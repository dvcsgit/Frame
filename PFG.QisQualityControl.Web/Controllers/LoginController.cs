using NLog;
using PFG.DataSource;
using PFG.Library.Authentication;
using PFG.Library.Models;
using PFG.QisQualityControl.Web.Const;
using PFG.QisQualityControl.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PFG.QisQualityControl.Web.Controllers
{
    public class LoginController : Controller
    {
        //private readonly IFormsAuthentication _formAuthentication;

        protected static Logger _logger = LogManager.GetCurrentClassLogger();

        //public LoginController(IFormsAuthentication formsAuthentication)
        //{

        //    this._formAuthentication = formsAuthentication;

        //}



        /// <summary>
        /// 登入頁
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            var cookies = HttpContext.Request.Cookies.AllKeys;
            if (cookies.Contains(SiteConst.RememberCookie))
            {
                var cookieAccout = HttpContext.Request.Cookies[SiteConst.RememberCookie].Values["account"];

                LoginViewModel viewModel = new LoginViewModel()
                {
                    Account = cookieAccout,
                    RememberMe = !string.IsNullOrEmpty(cookieAccout) ? true : false
                };

                return View(viewModel);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Index(LoginViewModel model, string requestUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (TestEntities db = new TestEntities())
                    {
                        LogOnStatus status = LogOnStatus.Failure;
                        var currentDate = DateTime.Now;
                        var user = db.Users.Where(x => x.Account == model.Account).SingleOrDefault();
                        if (user != null)
                        {
                            if (user.PasswordHash == model.Password)
                            {
                                //成功的流程 寫入登入時間
                                status = LogOnStatus.Successful;
                                user.DateLastLogin = currentDate;
                                db.Entry<Users>(user).State = EntityState.Modified;

                                db.SaveChanges();
                            }
                            else
                            {
                                status = LogOnStatus.FailPassword;
                            }
                        }
                        else
                        {
                            status = LogOnStatus.NotExist;
                        }
                        switch (status)
                        {
                            case LogOnStatus.Successful:
                                //note:
                                AuthenticationTicketUserData userInfo = new AuthenticationTicketUserData
                                {
                                    UserId = user.Account,
                                    Name = user.Name
                                };

                                //當都沒有權限 則會用GUEST
                                List<string> roles = new List<string>();

                                if (user.Roles.Count == 0)
                                {
                                    roles.Add("Guest");
                                }
                                else
                                {
                                    roles = user.Roles.Select(x => x.RoleID).ToList();
                                }

                                userInfo.Roles = roles;
                                IFormsAuthentication _formAuthentication = new DefaultFormsAuthentication();
                                //IFormsAuthentication _formAuthentication = DependencyResolver.Current.GetService<IFormsAuthentication>();
                                _formAuthentication.SetAuthCookie(this.HttpContext, new FormsAuthenticationTicket(1, userInfo.UserId, DateTime.Now, DateTime.Now.Add(FormsAuthentication.Timeout), false, userInfo.ToString()));

                                var cookies = HttpContext.Request.Cookies.AllKeys;
                                if (cookies.Contains(SiteConst.RememberCookie))
                                {
                                    string rem_cookie = HttpContext.Request.Cookies[SiteConst.RememberCookie].Values["account"];
                                    //記住帳號實做
                                    if (model.RememberMe)
                                    {
                                        if (string.IsNullOrEmpty(rem_cookie))
                                        {

                                            HttpCookie cookie = new HttpCookie(SiteConst.RememberCookie);
                                            cookie["account"] = model.Account;
                                            cookie.Expires = DateTime.Now.AddYears(100);
                                            Response.Cookies.Add(cookie);
                                        }
                                        else
                                        {
                                            if (rem_cookie != model.Account)
                                            {
                                                this.Request.Cookies[SiteConst.RememberCookie].Expires = DateTime.Now.AddHours(-1);
                                                Response.Cookies.Add(Request.Cookies[SiteConst.RememberCookie]);
                                                HttpCookie cookie = new HttpCookie(SiteConst.RememberCookie);
                                                cookie["account"] = model.Account;
                                                cookie.Expires = DateTime.Now.AddYears(100);
                                                Response.Cookies.Add(cookie);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(rem_cookie))
                                        {
                                            this.Request.Cookies[SiteConst.RememberCookie].Expires = DateTime.Now.AddHours(-1);
                                            Response.Cookies.Add(Request.Cookies[SiteConst.RememberCookie]);
                                        }
                                    }
                                }
                                else
                                {
                                    HttpCookie cookie = new HttpCookie(SiteConst.RememberCookie);
                                    cookie["account"] = model.Account;
                                    cookie.Expires = DateTime.Now.AddYears(100);
                                    Response.Cookies.Add(cookie);
                                }

                                var returnUrlBack = requestUrl;

                                if (!Url.IsLocalUrl(requestUrl))
                                {
                                    returnUrlBack = Url.Action("Index", "Home");
                                }

                                return Json(new { success = true, returnUrl = returnUrlBack });

                            case LogOnStatus.Disabled:
                                ModelState.AddModelError("", "該帳號已被停權");
                                break;
                            case LogOnStatus.Failure:
                                ModelState.AddModelError("", "帳號或密碼錯誤");
                                break;
                            case LogOnStatus.UnActived:
                                ModelState.AddModelError("", "帳號尚未啟用");
                                break;
                            default:
                                ModelState.AddModelError("", "帳號或密碼錯誤");
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                _logger.Info("Index - Error{0}", ex.Message);
            }
            finally
            {
                _logger.Info("Index - end");
            }


            // If we got this far, something failed
            return Json(new { errors = ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage)) });

        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            IFormsAuthentication _formAuthentication = new DefaultFormsAuthentication();
            //IFormsAuthentication _formAuthentication = DependencyResolver.Current.GetService<IFormsAuthentication>();
            _formAuthentication.Signout();
            return RedirectToAction("Index", "Login");
        }
    }
}