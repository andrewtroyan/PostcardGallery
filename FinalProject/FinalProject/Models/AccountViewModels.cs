using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceName = "EmailRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Translations))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [Display(Name = "Username", ResourceType = typeof(Resources.Translations))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resources.Translations),
            ErrorMessageResourceName = "IncorrectFormat")]
        [Display(Name = "Password", ResourceType = typeof(Resources.Translations))]
        public string Password { get; set; }

        [Display(Name = "Remember", ResourceType = typeof(Resources.Translations))]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType = 
            typeof(Resources.Translations), ErrorMessageResourceName = "StringLengthError")]
        [Display(Name = "Username", ResourceType = typeof(Resources.Translations))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Translations),
            ErrorMessageResourceName = "IncorrectFormat")]
        [Display(Name = "Email", ResourceType = typeof(Resources.Translations))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequired",
            ErrorMessageResourceType = typeof(Resources.Translations))]
        [StringLength(100, MinimumLength = 6, ErrorMessageResourceType =
            typeof(Resources.Translations), ErrorMessageResourceName = "StringLengthError")]
        [DataType(DataType.Password, ErrorMessageResourceType = typeof(Resources.Translations),
            ErrorMessageResourceName = "IncorrectFormat")]
        [Display(Name = "Password", ResourceType = typeof(Resources.Translations))]
        public string Password { get; set; }
        
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources.Translations))]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Translations),
            ErrorMessageResourceName = "PasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage =
            "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage =
            "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
