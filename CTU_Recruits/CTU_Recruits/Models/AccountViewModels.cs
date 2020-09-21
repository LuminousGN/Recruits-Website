using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CTU_Recruits.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
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
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        #region Profile_Details
        [Required]
        [Display(Name = "User Role")]
        public string UserRoles { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [MaxLength(25)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [MaxLength(25)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[] UserPhoto { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        [MaxLength(25)]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Display(Name = "Profile Summary")]
        [MaxLength(100)]
        public string ShortDescription { get; set; }

        [Display(Name = "Contact Number")]
        [MaxLength(25)]
        public string PhoneNumber { get; set; }
        #endregion

        #region CV_details
        [Display(Name = "Title (ex.Web Developer)")]
        public string Work_Title1 { get; set; }

        [Display(Name = "Title (ex.App Developer)")]
        public string Work_Title2 { get; set; }

        [Display(Name = "Title (ex.Game Developer)")]
        public string Work_TItle3 { get; set; }

        [Display(Name = "Company Name")]
        public string Work_Name1 { get; set; }

        [Display(Name = "Company Name")]
        public string Work_Name2 { get; set; }

        [Display(Name = "Company Name")]
        public string Work_Name3 { get; set; }

        [Display(Name = "Description(what was your purpose)")]
        public string Work_Description1 { get; set; }

        [Display(Name = "Description(what was your purpose)")]
        public string Work_Description2 { get; set; }

        [Display(Name = "Description(what was your purpose)")]
        public string Work_Description3 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateStart1 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateStart2 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateStart3 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateEnd1 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateEnd2 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateEnd3 { get; set; }

        [Display(Name = "Title (ex. High School or Applied Physics)")]
        public string Edu_Title1 { get; set; }

        [Display(Name = "Title (ex. High School or Applied Physics)")]
        public string Edu_Title2 { get; set; }

        [Display(Name = "Title (ex. High School or Applied Physics)")]
        public string Edu_Title3 { get; set; }

        [Display(Name = "Institution / school name")]
        public string Edu_School1 { get; set; }

        [Display(Name = "Institution / school name")]
        public string Edu_School2 { get; set; }

        [Display(Name = "Institution / school name")]
        public string Edu_School3 { get; set; }

        [Display(Name = "Description (what exactly was it that you studied)")]
        public string Edu_Description1 { get; set; }

        [Display(Name = "Description (what exactly was it that you studied)")]
        public string Edu_Description2 { get; set; }

        [Display(Name = "Description (what exactly was it that you studied)")]
        public string Edu_Description3 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateStart1 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateStart2 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateStart3 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateEnd1 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateEnd2 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateEnd3 { get; set; }

        [Display(Name = "1st Skill (ex. C#)")]
        public string Skill1 { get; set; }

        [Display(Name = "2nd Skill (ex. Welding)")]
        public string Skill2 { get; set; }

        [Display(Name = "3rd Skill (ex. Milling)")]
        public string Skill3 { get; set; }

        [Display(Name = "Tell us what you like to do in your spare time")]
        public string Hobbies_Desc { get; set; }

        [Display(Name = "Tell us what you like to do in your spare time")]
        public string Visibility { get; set; }

        [Display(Name = "Tell us what you like to do in your spare time")]
        public string Success { get; set; }

        [Display(Name = "Job Category (ex. Web Development, Engineering)")]
        public string CV_Category { get; set; }
        #endregion

        #region Company Details
        public string CompanyName { get; set; }
        public string PhysicalAddress { get; set; }
        public string ContactNumber { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWebsite { get; set; }
        #endregion
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

    public class UserEdit
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[] UserPhoto { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Profile Summary")]
        public string ShortDescription { get; set; }

        [Required]
        [MaxLength(15)]
        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; }
    }

    public class DeleteUser
    {
        public string Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[] UserPhoto { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Display(Name = "Profile Summary")]
        public string ShortDescription { get; set; }

        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; }
    }

    public class EditCV
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Profile Summary")]
        public string ShortDescription { get; set; }

        [MaxLength(50)]
        [Display(Name = "Title (ex.Web Developer)")]
        public string Work_Title1 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Title (ex.App Developer)")]
        public string Work_Title2 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Title (ex.Game Developer)")]
        public string Work_TItle3 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Company Name")]
        public string Work_Name1 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Company Name")]
        public string Work_Name2 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Company Name")]
        public string Work_Name3 { get; set; }

        [MaxLength(100)]
        [Display(Name = "Description(what was your purpose)")]
        public string Work_Description1 { get; set; }

        [MaxLength(100)]
        [Display(Name = "Description(what was your purpose)")]
        public string Work_Description2 { get; set; }

        [MaxLength(100)]
        [Display(Name = "Description(what was your purpose)")]
        public string Work_Description3 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateStart1 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateStart2 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateStart3 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateEnd1 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateEnd2 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateEnd3 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Title (ex. High School or Applied Physics)")]
        public string Edu_Title1 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Title (ex. High School or Applied Physics)")]
        public string Edu_Title2 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Title (ex. High School or Applied Physics)")]
        public string Edu_Title3 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Institution / school name")]
        public string Edu_School1 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Institution / school name")]
        public string Edu_School2 { get; set; }

        [MaxLength(50)]
        [Display(Name = "Institution / school name")]
        public string Edu_School3 { get; set; }

        [MaxLength(100)]
        [Display(Name = "Description (what exactly was it that you studied)")]
        public string Edu_Description1 { get; set; }

        [MaxLength(100)]
        [Display(Name = "Description (what exactly was it that you studied)")]
        public string Edu_Description2 { get; set; }

        [MaxLength(100)]
        [Display(Name = "Description (what exactly was it that you studied)")]
        public string Edu_Description3 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateStart1 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateStart2 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateStart3 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateEnd1 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateEnd2 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateEnd3 { get; set; }

        [MaxLength(25)]
        [Display(Name = "1st Skill (ex. C#)")]
        public string Skill1 { get; set; }

        [MaxLength(25)]
        [Display(Name = "2nd Skill (ex. Welding)")]
        public string Skill2 { get; set; }

        [MaxLength(25)]
        [Display(Name = "3rd Skill (ex. Milling)")]
        public string Skill3 { get; set; }

        [MaxLength(100)]
        [Display(Name = "Tell us what you like to do in your spare time")]
        public string Hobbies_Desc { get; set; }

        [Required]
        [Display(Name = "CV Visiblity")]
        public string Visibility { get; set; }

        [Display(Name = "Did you find your dream job?")]
        public string Success { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "Job Category (ex. Web Development, Engineering)")]
        public string CV_Category { get; set; }
    }

    public class ViewProfile
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Profile Picture")]
        public byte[] UserPhoto { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

        [Display(Name = "Profile Summary")]
        public string ShortDescription { get; set; }

        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Title (ex.Web Developer)")]
        public string Work_Title1 { get; set; }

        [Display(Name = "Title (ex.App Developer)")]
        public string Work_Title2 { get; set; }

        [Display(Name = "Title (ex.Game Developer)")]
        public string Work_TItle3 { get; set; }

        [Display(Name = "Company Name")]
        public string Work_Name1 { get; set; }

        [Display(Name = "Company Name")]
        public string Work_Name2 { get; set; }

        [Display(Name = "Company Name")]
        public string Work_Name3 { get; set; }

        [Display(Name = "Description(what was your purpose)")]
        public string Work_Description1 { get; set; }

        [Display(Name = "Description(what was your purpose)")]
        public string Work_Description2 { get; set; }

        [Display(Name = "Description(what was your purpose)")]
        public string Work_Description3 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateStart1 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateStart2 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateStart3 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateEnd1 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateEnd2 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Work_DateEnd3 { get; set; }

        [Display(Name = "Title (ex. High School or Applied Physics)")]
        public string Edu_Title1 { get; set; }

        [Display(Name = "Title (ex. High School or Applied Physics)")]
        public string Edu_Title2 { get; set; }

        [Display(Name = "Title (ex. High School or Applied Physics)")]
        public string Edu_Title3 { get; set; }

        [Display(Name = "Institution / school name")]
        public string Edu_School1 { get; set; }

        [Display(Name = "Institution / school name")]
        public string Edu_School2 { get; set; }

        [Display(Name = "Institution / school name")]
        public string Edu_School3 { get; set; }

        [Display(Name = "Description (what exactly was it that you studied)")]
        public string Edu_Description1 { get; set; }

        [Display(Name = "Description (what exactly was it that you studied)")]
        public string Edu_Description2 { get; set; }

        [Display(Name = "Description (what exactly was it that you studied)")]
        public string Edu_Description3 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateStart1 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateStart2 { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateStart3 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateEnd1 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateEnd2 { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Edu_DateEnd3 { get; set; }

        [Display(Name = "1st Skill (ex. C#)")]
        public string Skill1 { get; set; }

        [Display(Name = "2nd Skill (ex. Welding)")]
        public string Skill2 { get; set; }

        [Display(Name = "3rd Skill (ex. Milling)")]
        public string Skill3 { get; set; }

        [Display(Name = "Tell us what you like to do in your spare time")]
        public string Hobbies_Desc { get; set; }

        [Display(Name = "Tell us what you like to do in your spare time")]
        public string Visibility { get; set; }

        [Display(Name = "Tell us what you like to do in your spare time")]
        public string Success { get; set; }

        [Display(Name = "Job Category (ex. Web Development, Engineering)")]
        public string CV_Category { get; set; }
    }

    public class EditCompany
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string CompanyName { get; set; }
        public string PhysicalAddress { get; set; }
        public string ContactNumber { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWebsite { get; set; }
    }
}
