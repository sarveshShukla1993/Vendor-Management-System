using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using VendorManagementSystem.Models;
using VendorManagementSystem.ProjectViewModel;

namespace VendorManagementSystem.Controllers
{
    public class RegisteLoginController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DBConnection db = new DBConnection();

        // GET: RegisteLogin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HomePage()
        {
            return View();
        }


        //ActionMethod for Storing visitors information
        [HttpGet]
        public ActionResult visitorList()
        {
            logger.Error("In Visitor List.");
            List<VisitPurpose> vp = db.purpose.Where(x => x.IsDeleted == false).ToList();
            ViewBag.visitlist = vp;
            return View();
        }

        [HttpPost]
        public ActionResult visitorList(VisitorView modData)
        {
            try
            {
                if(modData.Purpose == "-1")
                {
                    List<VisitPurpose> vp = db.purpose.Where(x => x.IsDeleted == false).ToList();
                    ViewBag.visitlist = vp;
                    Response.Write("<script>alert('Please select purpose of visit.')</script>");
                    return View();
                }
                int p = Convert.ToInt32(modData.Purpose);
                var pmodif = db.purpose.Where(x => x.PurposeId == p).FirstOrDefault();
                pmodif.IsUsed = true;
                var log1 = db.purpose.Where(x => x.PurposeId == p).FirstOrDefault();
                Visitor dbmod = new Visitor();
                if (log1.Purpose == "Meeting")
                {
                    if(modData.ContactPerson == "" || modData.ContactPerson == null)
                    {
                        List<VisitPurpose> vp = db.purpose.Where(x => x.IsDeleted == false).ToList();
                        ViewBag.visitlist = vp;
                        Response.Write("<script>alert('Please enter contact person.')</script>");
                        return View();
                    }
                }
                dbmod.ContactPerson = modData.ContactPerson;
                dbmod.Name = modData.PersonName;
                dbmod.Age = modData.PersonAge;
                dbmod.Email = modData.Email;
                dbmod.Purpose = log1.Purpose;
                dbmod.MeetingDate = DateTime.Now.Date; ;
                dbmod.MeetingTime = DateTime.Now.TimeOfDay;
                dbmod.AddressLine = modData.AddressLine;
                dbmod.Count = 1;
                db.visitor.Add(dbmod);
                db.SaveChanges();
                Session["visitors"] = db.visitor.Where(x => x.Email == modData.Email).Select(x => x.Id).FirstOrDefault();
                //To clear the all input fields in view

                return RedirectToAction("visitorpage");
            }
            catch(ArgumentNullException ex)
            {
                throw ex;
            }
           
        }

        //Checking if visitor email already present.
        //public JsonResult checkVisitorEmail()
        //{
        //    return Json();
        //}

        [HttpGet]
        public ActionResult visitorpage()
        {
            try
            {
                if (Session["visitors"] != null)
                {
                    int i = Convert.ToInt32(Session["visitors"]);
                    var vp = db.visitor.Where(x => x.Id == i).FirstOrDefault();
                    List<VisitPurpose> vp1 = db.purpose.Where(x => x.IsDeleted == false).ToList();
                    ViewBag.visitlist = vp1;
                    return View(vp);
                }
                else
                {
                    return View("visitorList");
                }
            }
            catch
            {
                Response.Write("<script>alert('Data Can't be populated.)</script>");
                return View();
            }
        }


        [HttpGet]
        public ActionResult SaveRecord()
        {
            HttpContext.Session.Abandon();
            HttpContext.Session.Clear();
            return RedirectToAction("visitorList");
        }

        [HttpPost]
        public ActionResult EditRecord(Visitor vstr)
        {
            try
            {
                if (Session["visitors"] != null)
                {
                    db.Entry(vstr).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("visitorpage");
                }
                else
                {
                    List<VisitPurpose> vp1 = db.purpose.Where(x => x.IsDeleted == false).ToList();
                    ViewBag.visitlist = vp1;
                    return RedirectToAction("visitorList");
                }
            }
            catch
            {
                List<VisitPurpose> vp1 = db.purpose.Where(x => x.IsDeleted == false).ToList();
                ViewBag.visitlist = vp1;
                return RedirectToAction("HomePage");
            }
            
        }


        //Vendor registration form
        [HttpGet]
        public ActionResult vendor()
        {
            List<SellingCategoryModel> sc = db.sellcategory.Where(x => x.IsDeleted == false).ToList();
            ViewBag.sellingcat = sc;

            return View();
        }

        //Method to encrypt password.
        private string Encrypt_Password(string password)
        {
            string pswstr = string.Empty;

            byte[] psw_encode = new byte[password.Length];

            psw_encode = System.Text.Encoding.UTF8.GetBytes(password);

            pswstr = Convert.ToBase64String(psw_encode);

            return pswstr;
        }


        [HttpPost]
        public ActionResult vendor(VendorViewModel vvm, List<SellingCategoryModel> hobby)
        {
            try
            {
                foreach(var it in hobby.Where(x => x.IsChecked).ToList())
                {
                    if(it.IsChecked)
                    {
                        var id = db.sellcategory.Find(it.Id);
                        id.IsUsed = true;
                    }
                }
                var str1 = "";
                foreach (var item in hobby.Where(x => x.IsChecked).ToList())
                {
                    if (item.IsChecked)
                    {
                        str1 += item.hobbyname.ToString() + ",";
                    }
                }
                str1 = str1.TrimEnd(',');
                string p = vvm.VendorPassword + "@" + "vms";
                string password = Encrypt_Password(p);
                SellingCategoryModel scm = new SellingCategoryModel();
                scm.IsUsed = true;
                VendorModel vm = new VendorModel();
                vm.SellingCategory = str1;
                vm.VendorContact = vvm.VendorContact;
                vm.VendorEmail = vvm.VendorEmail;
                vm.VendorName = vvm.VendorName;
                vm.VendorPassword = password;
                db.vendor.Add(vm);
                db.SaveChanges();

                int i = db.vendor.Where(x => x.VendorEmail == vvm.VendorEmail).Select(x => x.VendorId).FirstOrDefault();
                //Adding roles to vendor
                UserRoles uRole = new UserRoles();
                uRole.RoleName = "Vendor";
                uRole.VendorId = i;
                db.userrole.Add(uRole);
                db.SaveChanges();

                //Adding session to complete the form. It will identify every registration forms value to particular vendor.
                Session["Admin"] = i;
                return RedirectToAction("PCDetails","VendorDetails");
            }
            catch (Exception)
            {
                ViewBag.value = "Something went wrong";
                return View();
            }
        }

        [HttpGet]
        public ActionResult vendorLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> vendorLogin(vendorViewLogin vvl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(vvl);
                }

                AdminLogin admin = new AdminLogin();
                admin = db.adminlog.Where(x => x.EmailAddress == vvl.VendorEmail && (x.Passkey == vvl.VendorPassword)).FirstOrDefault();
                if (admin != null)
                { 
                    //Session["superid"] = vvl.VendorEmail;
                    //Session["EmailId"] = vvl.VendorEmail;
                    //ViewBag.messg = vvl.VendorEmail;
                    FormsAuthentication.SetAuthCookie(vvl.VendorEmail, false);
                    string Roles = "admin";
                    var authTicket = new FormsAuthenticationTicket(1, admin.EmailAddress, DateTime.Now, DateTime.Now.AddMinutes(30), false, Roles);
                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                    HttpContext.Response.Cookies.Add(authCookie);
                    return RedirectToAction("AdminPortal", "SuperAdmin");
                }
                else
                {
                    string q = vvl.VendorPassword;
                    string pass = Encrypt_Password(q);
                    VendorModel vendor = new VendorModel();
                    vendor = db.vendor.Where(m => m.VendorEmail == vvl.VendorEmail && (m.VendorPassword == pass)).FirstOrDefault();
                    if (vendor != null)
                    {
                        var id = db.vendor.Where(m => m.VendorEmail == vvl.VendorEmail).Select(m => m.VendorId).FirstOrDefault();
                        int vid = Convert.ToInt32(id);
                        //Session["Adminid"] = vid;
                        //Session["EmailId"] = vvl.VendorEmail;
                        VendorLogInOutTime vliot = new VendorLogInOutTime();
                        vliot.LogInTime = DateTime.Now;
                        vliot.VendorId = vid;
                        vliot.LogOutTime = null;
                        db.loginouttime.Add(vliot);
                        var a = db.vendor.Where(m => m.VendorEmail == vvl.VendorEmail).FirstOrDefault();
                        a.DataCompleted = true;
                        var b = db.businessdetails.Where(x => x.VendorId == id).FirstOrDefault();
                        b.DataCompleted = true;
                        db.SaveChanges();

                        string Roles = db.userrole.Where(x => x.VendorId == vid).Select(x => x.RoleName).FirstOrDefault();
                        FormsAuthentication.SetAuthCookie(vvl.VendorEmail, false);
                        var authTicket = new FormsAuthenticationTicket(1, vendor.VendorEmail, DateTime.Now, DateTime.Now.AddMinutes(20), false, Roles);
                        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                        HttpContext.Response.Cookies.Add(authCookie);
                        return RedirectToAction("Index", "VendorAccess");
                   }
                   else
                   {
                       ViewBag.errorvalue = "Please enter valid Login Id and Password.";
                       return View();
                   }

                }
            }
            catch (Exception e)
            {
                Response.Write("<script>alert('Please enter emailId and password')</script>");
                return View();
            }
        
           
        }

        [HttpGet]
        public ActionResult logout()
        {
            try
            {
                if (User.Identity.GetUserName() == "vmsadmin@gmail.com")
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("HomePage");

                }
                else
                {
                    //var vid = Session["Adminid"];
                    string ident = User.Identity.GetUserName();
                    var venId = db.vendor.Where(x => x.VendorEmail == ident).Select(x => x.VendorId).FirstOrDefault();
                    int id = Convert.ToInt32(venId);
                    var rec = db.loginouttime.Where(x => x.LogOutTime == null && (x.VendorId == id)).FirstOrDefault();
                    rec.LogOutTime = DateTime.Now;
                    //db.Entry(rec).State = EntityState.Modified;
                    db.SaveChanges();
                    //HttpContext.Session.Abandon();
                    //HttpContext.Session.Clear();
                    FormsAuthentication.SignOut();
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    //HttpContext.Response.Cookies["ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
                    //HttpCookie myCookie = new HttpCookie("ASPXAUTH");
                    ////Here, we are setting the time to a previous time.
                    ////When the browser detect it next time, it will be deleted automatically.
                    //myCookie.Expires = DateTime.Now.AddDays(-1);
                    //Response.Cookies.Add(myCookie);
                    //Response.Cookies.Clear();
                    return RedirectToAction("HomePage");

                    //HttpContext.Session.Abandon();
                    //HttpContext.Session.Clear();


                }
            }
            catch
            {
                return RedirectToAction("HomePage");
            }
        }

        //Method to log off external login.
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailId)
        {
            string message = "";
            bool status = false;
            var account = db.vendor.Where(x => x.VendorEmail == EmailId).FirstOrDefault();
            if(account != null)
            {
                string resetCode = Guid.NewGuid().ToString();
                SendVerificationLinkEmail(account.VendorEmail, resetCode, "ResetPassword");
                account.ResetPasswordCode = resetCode;
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = false;
                message = "Reset password link has been sent to your registered emial address.";
            }
            else
            {
                message = "Account not found.";
            }
            ViewBag.Message = message;
            return View();
        }


        [NonAction]
        public void SendVerificationLinkEmail(string EmailId, string activationCode, string emailFor="verifyAccount")
        {
            var verifyUrl = "/RegisteLogin/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery,verifyUrl);
            var fromEmail = new MailAddress("vmssoftwareapp@gmail.com", "Reset Password.");
            var toEmail = new MailAddress(EmailId);
            string fromEmailPassword = "vms@software";
            string subject = "";
            string body = "";
            if(emailFor == "verifyAccount")
            {
                subject = "Your account is successfully created.";
                body = link + ">" + link + "</a>";
            }
            else if(emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = link;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
                };

                using (var mess = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
               smtp.Send(mess);
            }
        }


        [HttpGet]
        public ActionResult ResetPassword(string Id)
        {
            var user = db.vendor.Where(x => x.ResetPasswordCode == Id).FirstOrDefault();
            if(user != null)
            {
                ResetPasswordModel rpm = new ResetPasswordModel();
                rpm.ResetCode = Id;
                return View(rpm);
            }
            else
            {
                return HttpNotFound();
            }
        }


        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            var user = db.vendor.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
            if(user != null)
            {
               // var p = Crypto.Hash(model.NewPassword);
                user.VendorPassword = Encrypt_Password(model.NewPassword);
                user.ResetPasswordCode = "";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
                message = "New Password has been updated Successfully. Please login!!";
            }
            ViewBag.Message = message;
            return View(model);
        }

        public JsonResult visitoListContactPersonAutoC(string term)
        {
            List<string> ac;
            ac = db.personcontactname.Where(x => x.Name.StartsWith(term)).Select(x => x.Name).Distinct().ToList();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        //remote validation for visitor
        public JsonResult IsUserNameAlreadyExists(string Email)
        {
            bool result = true;
            bool a = db.visitor.Any(x => x.Email == Email);
            if(a == true)
            {
                var b = db.visitor.Where(x => x.Email == Email).FirstOrDefault();
                double c = b.Count;
                b.Count = c + 0.5;
                db.SaveChanges();
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //remote validation for vendor
        public JsonResult IsVendorAlreadyExists(string VendorEmail)
        {
            bool result = true;
            bool a = db.vendor.Any(x => x.VendorEmail == VendorEmail);
            if (a == true)
            {
                result = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}


