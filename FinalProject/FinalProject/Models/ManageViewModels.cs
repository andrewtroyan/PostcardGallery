using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace FinalProject.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage =
            "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage =
            "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessageResourceName = "PasswordRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resources.Translations),
            ErrorMessageResourceName = "IncorrectFormat")]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Resources.Translations))]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType =
            typeof(Resources.Translations), ErrorMessageResourceName = "StringLengthError")]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resources.Translations),
            ErrorMessageResourceName = "IncorrectFormat")]
        [Display(Name = "NewPassword", ResourceType = typeof(Resources.Translations))]
        public string NewPassword { get; set; }
        
        [Display(Name = "ConfirmNewPassword", ResourceType = typeof(Resources.Translations))]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resources.Translations),
            ErrorMessageResourceName = "PasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeDataViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [Display(Name = "Username", ResourceType = typeof(Resources.Translations))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Translations),
            ErrorMessageResourceName = "IncorrectFormat")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Translations))]
        public string Email { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Translations))]
        public string Name { get; set; }

        [Display(Name = "Surname", ResourceType = typeof(Resources.Translations))]
        public string Surname { get; set; }

        [Phone(ErrorMessageResourceType = typeof(Resources.Translations),
            ErrorMessageResourceName = "IncorrectFormat")]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Translations))]
        public string PhoneNumber { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}