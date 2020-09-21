using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CTU_Recruits.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        // Here we add a byte to Save the user Profile Picture
        #region Profile_details
        public string UserRoles { get; set; }
        public byte[] UserPhoto { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string ShortDescription { get; set; }
        #endregion

        #region CV_details
        public string Work_Title1 { get; set; }
        public string Work_Title2 { get; set; }
        public string Work_TItle3 { get; set; }
        public string Work_Name1 { get; set; }
        public string Work_Name2 { get; set; }
        public string Work_Name3 { get; set; }
        public string Work_Description1 { get; set; }
        public string Work_Description2 { get; set; }
        public string Work_Description3 { get; set; }
        public Nullable<System.DateTime> Work_DateStart1 { get; set; }
        public Nullable<System.DateTime> Work_DateStart2 { get; set; }
        public Nullable<System.DateTime> Work_DateStart3 { get; set; }
        public Nullable<System.DateTime> Work_DateEnd1 { get; set; }
        public Nullable<System.DateTime> Work_DateEnd2 { get; set; }
        public Nullable<System.DateTime> Work_DateEnd3 { get; set; }
        public string Edu_Title1 { get; set; }
        public string Edu_Title2 { get; set; }
        public string Edu_Title3 { get; set; }
        public string Edu_School1 { get; set; }
        public string Edu_School2 { get; set; }
        public string Edu_School3 { get; set; }
        public string Edu_Description1 { get; set; }
        public string Edu_Description2 { get; set; }
        public string Edu_Description3 { get; set; }
        public Nullable<System.DateTime> Edu_DateStart1 { get; set; }
        public Nullable<System.DateTime> Edu_DateStart2 { get; set; }
        public Nullable<System.DateTime> Edu_DateStart3 { get; set; }
        public Nullable<System.DateTime> Edu_DateEnd1 { get; set; }
        public Nullable<System.DateTime> Edu_DateEnd2 { get; set; }
        public Nullable<System.DateTime> Edu_DateEnd3 { get; set; }
        public string Skill1 { get; set; }
        public string Skill2 { get; set; }
        public string Skill3 { get; set; }
        public string Hobbies_Desc { get; set; }
        public string Visibility { get; set; }
        public string Success { get; set; }
        public string CV_Category { get; set; }
        #endregion

        #region Company Details
        public string CompanyName { get; set; }
        public string PhysicalAddress { get; set; }
        public string ContactNumber { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWebsite { get; set; }
        #endregion

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}