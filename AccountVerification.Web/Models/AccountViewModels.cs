using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountVerification.Web.Models
{
    public class ResendVerifyCodeViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Account E-mail")]
        public string Email { get; set; }
    }


    public class VerifyCodeViewModel:ResendVerifyCodeViewModel
    {
        [Required]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Name can't be blank")]
        [Display(Name = "Tell us your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail can't be blank")]
        [EmailAddress]
        [Display(Name = "Enter your E-mail address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Enter a password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Country code can't be blank")]
        [Display(Name = "Country code")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "Phone number can't be blank")]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
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
