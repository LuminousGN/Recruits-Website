using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CTU_Recruits.Models;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Net.Mail;
using PagedList;

namespace CTU_Recruits.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                                            .ToList(), "Name", "Name");
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Exclude = "UserPhoto")]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                // To convert the user uploaded Photo as Byte Array before save to DB
                byte[] imageData = null;
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase poImgFile = Request.Files["UserPhoto"];

                    using (var binary = new BinaryReader(poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(poImgFile.ContentLength);
                    }
                }

                var user = new ApplicationUser
                {
                    #region Profile_Details

                    UserRoles = model.UserRoles,
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ShortDescription = model.ShortDescription,
                    DateOfBirth = model.DateOfBirth,
                    UserPhoto = imageData,
                    #endregion

                    #region CV_Details
                    Work_Title1 = model.Work_Title1,
                    Work_Title2 = model.Work_Title2,
                    Work_TItle3 = model.Work_TItle3,
                    Work_Name1 = model.Work_Name1,
                    Work_Name2 = model.Work_Name2,
                    Work_Name3 = model.Work_Name3,
                    Work_Description1 = model.Work_Description1,
                    Work_Description2 = model.Work_Description2,
                    Work_Description3 = model.Work_Description3,
                    Work_DateStart1 = model.Work_DateStart1,
                    Work_DateStart2 = model.Work_DateStart2,
                    Work_DateStart3 = model.Work_DateStart3,
                    Work_DateEnd1 = model.Work_DateEnd1,
                    Work_DateEnd2 = model.Work_DateEnd2,
                    Work_DateEnd3 = model.Work_DateEnd3,
                    Edu_Title1 = model.Edu_Title1,
                    Edu_Title2 = model.Edu_Title2,
                    Edu_Title3 = model.Edu_Title3,
                    Edu_School1 = model.Edu_School1,
                    Edu_School2 = model.Edu_School2,
                    Edu_School3 = model.Edu_School3,
                    Edu_Description1 = model.Edu_Description1,
                    Edu_Description2 = model.Edu_Description2,
                    Edu_Description3 = model.Edu_Description3,
                    Edu_DateStart1 = model.Edu_DateStart1,
                    Edu_DateStart2 = model.Edu_DateStart2,
                    Edu_DateStart3 = model.Edu_DateStart3,
                    Edu_DateEnd1 = model.Edu_DateEnd1,
                    Edu_DateEnd2 = model.Edu_DateEnd2,
                    Edu_DateEnd3 = model.Edu_DateEnd3,
                    Skill1 = model.Skill1,
                    Skill2 = model.Skill2,
                    Skill3 = model.Skill3,
                    Hobbies_Desc = model.Hobbies_Desc,
                    Visibility = model.Visibility,
                    Success = model.Success,
                    CV_Category = model.CV_Category,
                    #endregion

                    #region Company_Details

                    CompanyName = model.CompanyName,
                    PhysicalAddress = model.PhysicalAddress,
                    ContactNumber = model.ContactNumber,
                    CompanyEmail = model.CompanyEmail,
                    CompanyWebsite = model.CompanyWebsite,
                    #endregion
                };

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    await this.UserManager.AddToRoleAsync(user.Id, model.UserRoles);
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                                          .ToList(), "Name", "Name");
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
        [Authorize(Roles = "Employer,Superadmin,Admin,ModeratorAdmin")]
        public ActionResult HireMe()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            ViewBag.Email = currentUser.Email;
            ViewBag.Number = currentUser.PhoneNumber;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Employer,Superadmin,Admin,ModeratorAdmin")]
        public ActionResult HireMe(string Name, string EmailId, string PhoneNo, string Subject, string Message)
        {
            try
            {
                AspNetUser aspNetUser = new AspNetUser();
                MailMessage mail = new MailMessage();
                mail.To.Add(EmailId);
                mail.From = new MailAddress("exampleemail@example.com");
                mail.Subject = Subject;

                string userMessage = "";
                userMessage = "<br/>Name :" + aspNetUser.FirstName;
                userMessage = userMessage + "<br/>Email: " + aspNetUser.Email;
                userMessage = userMessage + "<br/>Contact No: " + PhoneNo;
                userMessage = userMessage + "<br/>Message: " + Message;
                string Body = "Hi, <br/><br/> A new enquiry by user. Detail is as follows:<br/><br/> " + userMessage + "<br/><br/>Thanks";


                mail.Body = Body;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                //SMTP Server Address of gmail
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("exampleemail@example.com", "examplepassword");
                // Smtp Email ID and Password For authentication
                smtp.EnableSsl = true;
                smtp.Send(mail);
                ViewBag.Message = "Your Email has been sent!";
            }
            catch
            {
                throw;
            }

            return View();
        }

        //This is the controller code that allows us to edit user data

        private ApplicationDbContext context = new ApplicationDbContext();
        // This contains a view to display a list of all users, 
        // edit users
        // and delete users

        #region User_ActionResults

        [Authorize(Roles = "Superadmin,Admin,ModeratorAdmin")]
        public ActionResult ListUser()
        {
            return View(context.Users.ToList());
        }

        //This is the controller code that allows us to edit user data

        public ActionResult EditUser(string email)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindByEmail(email);
            UserEdit user = new UserEdit();
            user.Email = appUser.Email;
            user.FirstName = appUser.FirstName;
            user.LastName = appUser.LastName;
            user.PhoneNumber = appUser.PhoneNumber;
            user.ShortDescription = appUser.ShortDescription;
            user.UserPhoto = appUser.UserPhoto;
            user.DateOfBirth = appUser.DateOfBirth;

            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(UserEdit model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);
            var currentUser = manager.FindByEmail(model.Email);
            currentUser.Email = model.Email;
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.ShortDescription = model.ShortDescription;
            currentUser.UserPhoto = model.UserPhoto;
            currentUser.DateOfBirth = model.DateOfBirth;
            await manager.UpdateAsync(currentUser);
            var ctx = store.Context;
            ctx.SaveChanges();
            TempData["msg"] = "Profile Changes Saved !";
            return RedirectToAction("ListUser");
        }

        // This is the controller code that allows admins to delete users

        [Authorize(Roles = "Superadmin,Admin,ModeratorAdmin")]
        public ActionResult DeleteUser(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = context.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(context.Users.Find(id));
        }

        [Authorize(Roles = "Superadmin,Admin,ModeratorAdmin")]
        public async Task<ActionResult> UserDeleteConfirmed(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["UserDeleted"] = "User Successfully Deleted";
                return RedirectToAction("ManageEditors");
            }
            else
            {
                TempData["UserDeleted"] = "Error Deleting User";
                return RedirectToAction("ManageEditors");
            }
        }

        #endregion

        // This contains a view to display a list of all CV's 
        // and a view to edit users

        #region CV_ActionResults
        private Entities db = new Entities();

        [Authorize(Roles = "Employer,Superadmin,Admin,ModeratorAdmin")]
        public ActionResult ListCV(string sortOrder, string currentFilter, string SearchAll, /*string TitleSearch, string KeywordSearch, string NameSearch,*/ int? page)
        {
            // adding search, paging and filter functionality to my CV list page
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            //ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";

            if (SearchAll != null)
            {
                page = 1;
            }
            else
            {
                SearchAll = currentFilter;
            }

            ViewBag.CurrentFilter = SearchAll;

            var UserCV = from s in db.AspNetUsers
                         select s;

            // searches through everything
            if (!String.IsNullOrEmpty(SearchAll))
            {
                UserCV = UserCV.Where(s => s.CV_Category.Contains(SearchAll)
                                       || s.ShortDescription.Contains(SearchAll));

            }
            //// Only searches through categorys
            //if (!String.IsNullOrEmpty(TitleSearch))
            //{
            //    UserCV = UserCV.Where(s => s.CV_Category.Contains(TitleSearch));
            //}
            //// searches through categorys and descriptions
            //if (!String.IsNullOrEmpty(KeywordSearch))
            //{
            //    UserCV = UserCV.Where(s => s.ShortDescription.Contains(KeywordSearch)
            //                           || s.CV_Category.Contains(KeywordSearch));
            //}
            //// searches through First Name and Last Name
            //if (!String.IsNullOrEmpty(NameSearch))
            //{
            //    UserCV = UserCV.Where(s => s.LastName.Contains(NameSearch)
            //                           || s.FirstName.Contains(NameSearch));
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    UserCV = UserCV.OrderByDescending(s => s.FirstName);
                    break;               
                //case "lastname_desc":
                //    UserCV = UserCV.OrderByDescending(s => s.LastName);
                //    break;
                case "Title":
                    UserCV = UserCV.OrderBy(s => s.CV_Category);
                    break;
                case "title_desc":
                    UserCV = UserCV.OrderByDescending(s => s.CV_Category);
                    break;
                default:
                    UserCV = UserCV.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 18;
            int pageNumber = (page ?? 1);
            return View(context.Users.OrderBy(i => i.CV_Category).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "JobSeeker,Superadmin,Admin,ModeratorAdmin")]
        public ActionResult PersonalCV()
        {
            return View(context.Users.ToList());
        }

        public ActionResult Profile()
        {
            return View(context.Users.ToList());
        }

        [Authorize(Roles = "Employer,Superadmin,Admin,ModeratorAdmin")]
        public ActionResult SuccessPage(string sortOrder, string currentFilter, string SearchAll, /*string TitleSearch, string KeywordSearch, string NameSearch,*/ int? page)
        {
            // adding search, paging and filter functionality to my CV list page
            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "firstname_desc" : "";
            //ViewBag.LastNameSortParm = String.IsNullOrEmpty(sortOrder) ? "lastname_desc" : "";
            ViewBag.TitleSortParm = sortOrder == "Title" ? "title_desc" : "Title";

            if (SearchAll != null)
            {
                page = 1;
            }
            else
            {
                SearchAll = currentFilter;
            }

            ViewBag.CurrentFilter = SearchAll;

            var UserCV = from s in db.AspNetUsers
                         select s;

            // searches through everything
            if (!String.IsNullOrEmpty(SearchAll))
            {
                UserCV = UserCV.Where(s => s.CV_Category.Contains(SearchAll)
                                       || s.ShortDescription.Contains(SearchAll));

            }
            //// Only searches through categorys
            //if (!String.IsNullOrEmpty(TitleSearch))
            //{
            //    UserCV = UserCV.Where(s => s.CV_Category.Contains(TitleSearch));
            //}
            //// searches through categorys and descriptions
            //if (!String.IsNullOrEmpty(KeywordSearch))
            //{
            //    UserCV = UserCV.Where(s => s.ShortDescription.Contains(KeywordSearch)
            //                           || s.CV_Category.Contains(KeywordSearch));
            //}
            //// searches through First Name and Last Name
            //if (!String.IsNullOrEmpty(NameSearch))
            //{
            //    UserCV = UserCV.Where(s => s.LastName.Contains(NameSearch)
            //                           || s.FirstName.Contains(NameSearch));
            //}
            switch (sortOrder)
            {
                case "name_desc":
                    UserCV = UserCV.OrderByDescending(s => s.FirstName);
                    break;
                //case "lastname_desc":
                //    UserCV = UserCV.OrderByDescending(s => s.LastName);
                //    break;
                case "Title":
                    UserCV = UserCV.OrderBy(s => s.CV_Category);
                    break;
                case "title_desc":
                    UserCV = UserCV.OrderByDescending(s => s.CV_Category);
                    break;
                default:
                    UserCV = UserCV.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 18;
            int pageNumber = (page ?? 1);
            return View(context.Users.OrderBy(i => i.CV_Category).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "JobSeeker,Superadmin,Admin,ModeratorAdmin")]
        public ActionResult EditCV(string email)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindByEmail(email);
            EditCV cv = new EditCV();
            cv.Email = appUser.Email;
            cv.Work_Title1 = appUser.Work_Title1;
            cv.Work_Title2 = appUser.Work_Title2;
            cv.Work_TItle3 = appUser.Work_TItle3;
            cv.Work_Name1 = appUser.Work_Name1;
            cv.Work_Name2 = appUser.Work_Name2;
            cv.Work_Name3 = appUser.Work_Name3;
            cv.Work_Description1 = appUser.Work_Description1;
            cv.Work_Description2 = appUser.Work_Description2;
            cv.Work_Description3 = appUser.Work_Description3;
            cv.Work_DateStart1 = appUser.Work_DateStart1;
            cv.Work_DateStart2 = appUser.Work_DateStart2;
            cv.Work_DateStart3 = appUser.Work_DateStart3;
            cv.Work_DateEnd1 = appUser.Work_DateEnd1;
            cv.Work_DateEnd2 = appUser.Work_DateEnd2;
            cv.Work_DateEnd3 = appUser.Work_DateEnd3;
            cv.Edu_Title1 = appUser.Edu_Title1;
            cv.Edu_Title2 = appUser.Edu_Title2;
            cv.Edu_Title3 = appUser.Edu_Title3;
            cv.Edu_School1 = appUser.Edu_School1;
            cv.Edu_School2 = appUser.Edu_School2;
            cv.Edu_School3 = appUser.Edu_School3;
            cv.Edu_Description1 = appUser.Edu_Description1;
            cv.Edu_Description2 = appUser.Edu_Description2;
            cv.Edu_Description3 = appUser.Edu_Description3;
            cv.Edu_DateStart1 = appUser.Edu_DateStart1;
            cv.Edu_DateStart2 = appUser.Edu_DateStart2;
            cv.Edu_DateStart3 = appUser.Edu_DateStart3;
            cv.Edu_DateEnd1 = appUser.Edu_DateEnd1;
            cv.Edu_DateEnd2 = appUser.Edu_DateEnd2;
            cv.Edu_DateEnd3 = appUser.Edu_DateEnd3;
            cv.Skill1 = appUser.Skill1;
            cv.Skill2 = appUser.Skill2;
            cv.Skill3 = appUser.Skill3;
            cv.Hobbies_Desc = appUser.Hobbies_Desc;
            cv.Visibility = appUser.Visibility;
            cv.Success = appUser.Success;
            cv.CV_Category = appUser.CV_Category;

            return View(cv);
        }

        [HttpPost]
        public async Task<ActionResult> EditCV(EditCV model)
        {
            ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Nothing"))
                                          .ToList(), "Name", "Name");

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);
            var currentUser = manager.FindByEmail(model.Email);
            currentUser.Email = model.Email;
            currentUser.Work_Title1 = model.Work_Title1;
            currentUser.Work_Title2 = model.Work_Title2;
            currentUser.Work_TItle3 = model.Work_TItle3;
            currentUser.Work_Name1 = model.Work_Name1;
            currentUser.Work_Name2 = model.Work_Name2;
            currentUser.Work_Name3 = model.Work_Name3;
            currentUser.Work_Description1 = model.Work_Description1;
            currentUser.Work_Description2 = model.Work_Description2;
            currentUser.Work_Description3 = model.Work_Description3;
            currentUser.Work_DateStart1 = model.Work_DateStart1;
            currentUser.Work_DateStart2 = model.Work_DateStart2;
            currentUser.Work_DateStart3 = model.Work_DateStart3;
            currentUser.Work_DateEnd1 = model.Work_DateEnd1;
            currentUser.Work_DateEnd2 = model.Work_DateEnd2;
            currentUser.Work_DateEnd3 = model.Work_DateEnd3;
            currentUser.Edu_Title1 = model.Edu_Title1;
            currentUser.Edu_Title2 = model.Edu_Title2;
            currentUser.Edu_Title3 = model.Edu_Title3;
            currentUser.Edu_School1 = model.Edu_School1;
            currentUser.Edu_School2 = model.Edu_School2;
            currentUser.Edu_School3 = model.Edu_School3;
            currentUser.Edu_Description1 = model.Edu_Description1;
            currentUser.Edu_Description2 = model.Edu_Description2;
            currentUser.Edu_Description3 = model.Edu_Description3;
            currentUser.Edu_DateStart1 = model.Edu_DateStart1;
            currentUser.Edu_DateStart2 = model.Edu_DateStart2;
            currentUser.Edu_DateStart3 = model.Edu_DateStart3;
            currentUser.Edu_DateEnd1 = model.Edu_DateEnd1;
            currentUser.Edu_DateEnd2 = model.Edu_DateEnd2;
            currentUser.Edu_DateEnd3 = model.Edu_DateEnd3;
            currentUser.Skill1 = model.Skill1;
            currentUser.Skill2 = model.Skill2;
            currentUser.Skill3 = model.Skill3;
            currentUser.Hobbies_Desc = model.Hobbies_Desc;
            currentUser.Visibility = model.Visibility;
            currentUser.Success = model.Success;
            currentUser.CV_Category = model.CV_Category;
            await manager.UpdateAsync(currentUser);
            var ctx = store.Context;
            ctx.SaveChanges();
            TempData["msg"] = "CV Changes Saved !";
            return RedirectToAction("ListCV");
        }

        public ActionResult ViewProfile(string email)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindByEmail(email);
            ViewProfile cv = new ViewProfile();
            cv.Email = appUser.Email;
            cv.FirstName = appUser.FirstName;
            cv.LastName = appUser.LastName;
            cv.PhoneNumber = appUser.PhoneNumber;
            cv.ShortDescription = appUser.ShortDescription;
            cv.UserPhoto = appUser.UserPhoto;
            cv.DateOfBirth = appUser.DateOfBirth;
            cv.Work_Title1 = appUser.Work_Title1;
            cv.Work_Title2 = appUser.Work_Title2;
            cv.Work_TItle3 = appUser.Work_TItle3;
            cv.Work_Name1 = appUser.Work_Name1;
            cv.Work_Name2 = appUser.Work_Name2;
            cv.Work_Name3 = appUser.Work_Name3;
            cv.Work_Description1 = appUser.Work_Description1;
            cv.Work_Description2 = appUser.Work_Description2;
            cv.Work_Description3 = appUser.Work_Description3;
            cv.Work_DateStart1 = appUser.Work_DateStart1;
            cv.Work_DateStart2 = appUser.Work_DateStart2;
            cv.Work_DateStart3 = appUser.Work_DateStart3;
            cv.Work_DateEnd1 = appUser.Work_DateEnd1;
            cv.Work_DateEnd2 = appUser.Work_DateEnd2;
            cv.Work_DateEnd3 = appUser.Work_DateEnd3;
            cv.Edu_Title1 = appUser.Edu_Title1;
            cv.Edu_Title2 = appUser.Edu_Title2;
            cv.Edu_Title3 = appUser.Edu_Title3;
            cv.Edu_School1 = appUser.Edu_School1;
            cv.Edu_School2 = appUser.Edu_School2;
            cv.Edu_School3 = appUser.Edu_School3;
            cv.Edu_Description1 = appUser.Edu_Description1;
            cv.Edu_Description2 = appUser.Edu_Description2;
            cv.Edu_Description3 = appUser.Edu_Description3;
            cv.Edu_DateStart1 = appUser.Edu_DateStart1;
            cv.Edu_DateStart2 = appUser.Edu_DateStart2;
            cv.Edu_DateStart3 = appUser.Edu_DateStart3;
            cv.Edu_DateEnd1 = appUser.Edu_DateEnd1;
            cv.Edu_DateEnd2 = appUser.Edu_DateEnd2;
            cv.Edu_DateEnd3 = appUser.Edu_DateEnd3;
            cv.Skill1 = appUser.Skill1;
            cv.Skill2 = appUser.Skill2;
            cv.Skill3 = appUser.Skill3;
            cv.Hobbies_Desc = appUser.Hobbies_Desc;
            cv.Visibility = appUser.Visibility;
            cv.Success = appUser.Success;
            cv.CV_Category = appUser.CV_Category;

            return View(cv);
        }

        [HttpPost]
        public async Task<ActionResult> ViewProfile(ViewProfile model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);
            var currentUser = manager.FindByEmail(model.Email);
            currentUser.Email = model.Email;
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.ShortDescription = model.ShortDescription;
            currentUser.UserPhoto = model.UserPhoto;
            currentUser.DateOfBirth = model.DateOfBirth;
            currentUser.Work_Title1 = model.Work_Title1;
            currentUser.Work_Title2 = model.Work_Title2;
            currentUser.Work_TItle3 = model.Work_TItle3;
            currentUser.Work_Name1 = model.Work_Name1;
            currentUser.Work_Name2 = model.Work_Name2;
            currentUser.Work_Name3 = model.Work_Name3;
            currentUser.Work_Description1 = model.Work_Description1;
            currentUser.Work_Description2 = model.Work_Description2;
            currentUser.Work_Description3 = model.Work_Description3;
            currentUser.Work_DateStart1 = model.Work_DateStart1;
            currentUser.Work_DateStart2 = model.Work_DateStart2;
            currentUser.Work_DateStart3 = model.Work_DateStart3;
            currentUser.Work_DateEnd1 = model.Work_DateEnd1;
            currentUser.Work_DateEnd2 = model.Work_DateEnd2;
            currentUser.Work_DateEnd3 = model.Work_DateEnd3;
            currentUser.Edu_Title1 = model.Edu_Title1;
            currentUser.Edu_Title2 = model.Edu_Title2;
            currentUser.Edu_Title3 = model.Edu_Title3;
            currentUser.Edu_School1 = model.Edu_School1;
            currentUser.Edu_School2 = model.Edu_School2;
            currentUser.Edu_School3 = model.Edu_School3;
            currentUser.Edu_Description1 = model.Edu_Description1;
            currentUser.Edu_Description2 = model.Edu_Description2;
            currentUser.Edu_Description3 = model.Edu_Description3;
            currentUser.Edu_DateStart1 = model.Edu_DateStart1;
            currentUser.Edu_DateStart2 = model.Edu_DateStart2;
            currentUser.Edu_DateStart3 = model.Edu_DateStart3;
            currentUser.Edu_DateEnd1 = model.Edu_DateEnd1;
            currentUser.Edu_DateEnd2 = model.Edu_DateEnd2;
            currentUser.Edu_DateEnd3 = model.Edu_DateEnd3;
            currentUser.Skill1 = model.Skill1;
            currentUser.Skill2 = model.Skill2;
            currentUser.Skill3 = model.Skill3;
            currentUser.Hobbies_Desc = model.Hobbies_Desc;
            currentUser.Visibility = model.Visibility;
            currentUser.Success = model.Success;
            currentUser.CV_Category = model.CV_Category;
            return RedirectToAction("ListCV");
        }

        #endregion

        // This contains a view to display a list of all companies 
        // and a view to edit users

        #region Company_ActionResults

        [Authorize(Roles = "Superadmin,Admin,ModeratorAdmin")]
        public ActionResult ListCompanies()
        {
            return View(context.Users.ToList());
        }

        public ActionResult EditCompany(string email)
        {
            ApplicationUser appUser = new ApplicationUser();
            appUser = UserManager.FindByEmail(email);
            EditCompany company = new EditCompany();
            company.Email = appUser.Email;
            company.CompanyName = appUser.CompanyName;
            company.PhysicalAddress = appUser.PhysicalAddress;
            company.ContactNumber = appUser.ContactNumber;
            company.CompanyEmail = appUser.CompanyEmail;
            company.CompanyWebsite = appUser.CompanyWebsite;

            return View(company);
        }

        [HttpPost]
        public async Task<ActionResult> EditCompany(EditCompany model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            var manager = new UserManager<ApplicationUser>(store);
            var currentUser = manager.FindByEmail(model.Email);
            currentUser.Email = model.Email;
            currentUser.CompanyName = model.CompanyName;
            currentUser.PhysicalAddress = model.PhysicalAddress;
            currentUser.ContactNumber = model.ContactNumber;
            currentUser.CompanyEmail = model.CompanyEmail;
            currentUser.CompanyWebsite = model.CompanyWebsite;
            await manager.UpdateAsync(currentUser);
            var ctx = store.Context;
            ctx.SaveChanges();
            TempData["msg"] = "Profile Changes Saved !";
            return RedirectToAction("ListUser");
        }

        #endregion

    }
}