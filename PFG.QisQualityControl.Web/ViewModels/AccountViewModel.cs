using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PFG.QisQualityControl.Web.ViewModels
{
    public class AccountParameters : BasicParameters
    {
        [Display(Name = "NotesID")]
        public string NotesID { get; set; }
        [Display(Name = "姓名")]
        public string DisplayName { get; set; }
    }

    public class AccountGridListViewModel
    {
        public IPagedList<UserViewModel> GridList { get; set; }
        public AccountParameters Parameters { get; set; }
    }
    public class UserViewModel
    {
        [Display(Name = "NotesID")]
        public string Account { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "密碼")]
        public string PasswordHash { get; set; }

        [Display(Name = "上一次登入日期")]
        public DateTime? DateLastLogin { get; set; }


        [Display(Name = "新增人員")]
        public string CreatorAccount { get; set; }

        [Display(Name = "新增日期")]
        public DateTime? DateCreated { get; set; }

        [Display(Name = "修改人員")]
        public string ModifierAccount { get; set; }

        [Display(Name = "修改日期")]
        public DateTime? DateModified { get; set; }

        [Display(Name = "隸屬角色")]
        public List<string> UserRoles { get; set; }
    }


    public class AccountViewModel : BasicSaveMode
    {
        [Display(Name = "NotesID")]
        [Required(ErrorMessage = "{0} 欄位必填")]
        public string Account { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0} 欄位必填")]
        public string Name { get; set; }



        [Display(Name = "密碼")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "{0} 欄位必填")]
        [MinLength(4, ErrorMessage = "密碼不得少於4個字符")]
        [MaxLength(12, ErrorMessage = "密碼不得多於12個字符")]
        public string PasswordHash { get; set; }

        [Display(Name = "確認密碼")]
        [DataType(DataType.Password)]
        [Compare("PasswordHash", ErrorMessage = "密碼與確認新密碼不符合")]
        [Required(ErrorMessage = "{0} 欄位必填")]
        [MinLength(4, ErrorMessage = "密碼不得少於4個字符")]
        [MaxLength(12, ErrorMessage = "密碼不得多於12個字符")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "隸屬角色")]
        [Required(ErrorMessage = "{0} 欄位必填")]
        public List<string> UserRoles { get; set; }


        public AccountViewModel()
        {
            UserRoles = new List<string>() { "GUEST" };
        }
    }

    public class DetailAccountViewModel : AccountViewModel
    {
        [Display(Name = "新增日期")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "修改日期")]
        public DateTime? ModifyTime { get; set; }

        [Display(Name = "上次登入日期")]
        public DateTime? DateLastLogin { get; set; }
    }




    public class CreateRoleViewModel
    {

        [Display(Name = "角色代號")]
        [Required]
        public string RoleID { get; set; }

        [Display(Name = "角色名稱")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "角色描述")]
        public string Description { get; set; }

    }




    public class ProfileViewModel
    {
        [Display(Name = "使用者帳號")]
        public string Account { get; set; }

        [Display(Name = "Email")]
        [StringLength(200)]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}必填")]
        [MaxLength(20)]
        public string Name { get; set; }



    }
}