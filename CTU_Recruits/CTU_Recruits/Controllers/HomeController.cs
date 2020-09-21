using CTU_Recruits.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CTU_Recruits.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            ViewBag.Email = currentUser.Email;
            ViewBag.Number = currentUser.PhoneNumber;            
            return View();
        }

        [HttpPost]
        public ActionResult Contact(string Name, string EmailId, string PhoneNo, string Subject, string Message)
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
                ViewBag.Message = "Thank you for contacting us.";
            }
            catch
            {
                throw;
            }

            return View();
        }

        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/img/profile-placeholder.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }
                // to get the user details to load user Image
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

                if (currentUser.UserPhoto == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/img/profile-placeholder.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");
                }
                else
                {
                    var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                    var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();

                    return new FileContentResult(userImage.UserPhoto, "image/jpeg");
                }
            }
            else
            {
                string fileName = HttpContext.Server.MapPath(@"~/img/profile-placeholder.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
                return File(imageData, "image/png");

            }
        }
    }
}