using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VendorManagementSystem.Models;
using VendorManagementSystem.ProjectViewModel;

namespace VendorManagementSystem.Controllers
{
    public class VendorDetailsController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DBConnection dbd = new DBConnection();
        // GET: VendorDetails
        public ActionResult Index()
        {
            return View();
        }

        //*********************Adding Vendor's common Details****************
        [HttpGet]
        public ActionResult PCDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PCDetails(VendorViewDetails vvd)
        {
            try
            {
            
            VendorDetails vd = new VendorDetails();
                if (Session["Admin"] != null)
                {
                    vd.VendorId = Convert.ToInt32(Session["Admin"]);
                    vd.NumberofProducts = vvd.NumberofProducts;
                    vd.AlreadySellingOnline = vvd.AlreadySellingOnline;
                    vd.NatureOfBusiness = vvd.NatureOfBusiness;
                    vd.OperatingFrom = vvd.OperatingFrom;
                    vd.HandleBy = vvd.HandleBy;
                    vd.Gender = vvd.Gender;
                    vd.Age = vvd.Age;
                    vd.MinPrice = vvd.MinPrice;
                    vd.MaxPrice = vvd.MaxPrice;
                    dbd.vendordetails.Add(vd);
                    dbd.SaveChanges();
                    return RedirectToAction("BusinessDetails");
                }
                ViewBag.errormessage = "Data not entered properly.";
                return View();
            }
            catch (Exception)
            {
                ViewBag.errormessage = "Data can't be inserted.";
                return View();
            }
      }


        [HttpGet]
        public ActionResult BusinessDetails()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BusinessDetails(BusinessDetailsView bdv, HttpPostedFileBase file, HttpPostedFileBase file1, HttpPostedFileBase file2)
        {
            try {
            
                
                if(file == null )
                {
                    ViewBag.filemsg = "Please upload GSTIN Document";
                    return View();
                }
                else
                {
                    var a = Path.GetExtension(file.FileName);
                    if(a == ".pdf" || a ==".docx")
                    {
                        if (file1 == null)
                        {
                            ViewBag.filemsg = "Please upload TAN Document";
                            return View();
                        }

                        else
                        {
                            var b = Path.GetExtension(file1.FileName);
                            if (b == ".pdf" || b == ".docx")
                            {
                                if (file2 == null)
                                {
                                    ViewBag.filemsg = "Please upload CIN Document";
                                    return View();
                                }
                                else
                                {
                                    var c = Path.GetExtension(file2.FileName);
                                    if (c == ".pdf" || c == ".docx")
                                    {
                                        BusinessDetails bd = new BusinessDetails();
                                        if(Session["Admin"] != null)
                                        {
                                            bd.VendorId = Convert.ToInt32(Session["Admin"]);
                                        
                                       

                                        bd.BusinessName = bdv.BusinessName;

                                        bd.CIN = bdv.CIN;
                                        string cin = Path.GetFileName(file2.FileName);
                                        string path2 = Path.Combine(Server.MapPath("~/CINDocument"), cin);
                                        file2.SaveAs(path2);
                                        bd.CINDocument = cin;

                                        bd.TANNumber = bdv.TANNumber;
                                        string tan = Path.GetFileName(file1.FileName);
                                        string path1 = Path.Combine(Server.MapPath("~/TANDocument"), tan);
                                        file1.SaveAs(path1);
                                        bd.TANDocument = tan;

                                        bd.GSTINProvisionalID = bdv.GSTINProvisionalID;
                                        string gst = Path.GetFileName(file.FileName);
                                        string path = Path.Combine(Server.MapPath("~/GSTINDocument"), gst);
                                        file.SaveAs(path);
                                        bd.GSTINDucument = gst;
                                        bd.RegisteredBusineesAddress = bdv.RegisteredBusineesAddress;

                                        dbd.businessdetails.Add(bd);
                                        dbd.SaveChanges();
                                        return RedirectToAction("BankDetails");
                                    }
                                    }
                                    else
                                    {
                                        ViewBag.filemsg = "Please enter CIN of correct type.";
                                        return View();
                                    }
                                    return View();
                                }
                            }
                            else
                            {
                                ViewBag.filemsg = "Please enter TAN of correct type.";
                                return View();
                            }
                        }
                    }
                    else
                    {
                        ViewBag.filemsg = "Please enter GSTIN of correct type.";
                        return View();
                    }
                    
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public ActionResult loadStates(int countryId)
        {
            return Json(dbd.states.Where(x => x.CountryId == countryId).Select(x => new {
                Id = x.StateId,
                Name = x.StateName
            }).ToList(),JsonRequestBehavior.AllowGet);
        }

        //public ActionResult loadCities(int stateId)
        //{
        //    return Json(dbd.cities.Where(s => s.StateId == stateId).Select(s => new {
        //        Id = s.CityId,
        //        Name = s.CityName
        //    }).ToList(), JsonRequestBehavior.AllowGet);
        //}

        [HttpGet]
        public ActionResult BankDetails()
        {
            ViewBag.CountryList = dbd.countries.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult BankDetails(BankDetailsView bdv, HttpPostedFileBase file, HttpPostedFileBase file1, HttpPostedFileBase file2)
        {
            try
            {

                VendorBankDetails bd = new VendorBankDetails();
                if (Session["Admin"] != null)
                {
                    bd.VendorId = Convert.ToInt32(Session["Admin"]);
                }

                if (file != null && file1 != null && file2 != null)
                {
                    if (file.ContentLength > 1048576 )
                    {
                        ViewBag.CountryList = dbd.countries.ToList();
                        Response.Write("<script>alert('Pan Document must not be greater than 1MB.')</script>");
                        return View();
                    }
                    if (file1.ContentLength > 1048576)
                    {
                        ViewBag.CountryList = dbd.countries.ToList();
                        Response.Write("<script>alert('Address proof Document must not be greater than 1MB.')</script>");
                        return View();
                    }
                    if (file.ContentLength > 1048576)
                    {
                        ViewBag.CountryList = dbd.countries.ToList();
                        Response.Write("<script>alert('Cancelled check Document must not be greater than 1MB.')</script>");
                        return View();
                    }

                    string extension1 = Path.GetExtension(file.FileName);
                    string extension2 = Path.GetExtension(file1.FileName);
                    string extension3 = Path.GetExtension(file2.FileName);

                    if (extension1 == ".pdf")
                    {
                        if (extension2 == ".pdf")
                        {
                            if (extension3 == ".pdf")
                            {
                                ViewBag.CountryList = dbd.countries.ToList();
                                bd.AccountHoderName = bdv.AccountHoderName;
                                bd.AccountNumber = bdv.AccountNumber;
                                bd.BankName = bdv.BankName;

                                bd.AddressProof = bdv.AddressProof;
                                string address = Path.GetFileName(file1.FileName);
                                string path = Path.Combine(Server.MapPath("~/AddressProof"), address);
                                file1.SaveAs(path);
                                bd.AddressProofDocument = address;

                                bd.PanCard = bdv.PanCard;
                                string pan = Path.GetFileName(file.FileName);
                                string path1 = Path.Combine(Server.MapPath("~/PanCard"), pan);
                                file.SaveAs(path1);
                                bd.PancardDocument = pan;

                                string cc = Path.GetFileName(file2.FileName);
                                string path2 = Path.Combine(Server.MapPath("~/CancelledCheque"), cc);
                                file2.SaveAs(path2);
                                bd.CancelledCheque = cc;

                                bd.IFSC = bdv.IFSC;
                                bd.Country = bdv.Country;
                                bd.StateName = bdv.StateName;
                                bd.AddedOn = DateTime.Now;
                                dbd.bankdetails.Add(bd);
                                dbd.SaveChanges();
                            }
                            else
                            {
                                ViewBag.CountryList = dbd.countries.ToList();
                                Response.Write("<script>alert('Please upload only pdf format of Cancelled Check.')</script>");
                                return View();
                            }
                        }
                        else
                        {
                            ViewBag.CountryList = dbd.countries.ToList();
                            Response.Write("<script>alert('Please upload only pdf format of Address Proof.')</script>");
                            return View();
                        }
                    }
                    else
                    {
                        ViewBag.CountryList = dbd.countries.ToList();
                        Response.Write("<script>alert('Please upload only pdf format of Pancard.')</script>");
                        return View();
                    }
                }
                else
                {
                    ViewBag.CountryList = dbd.countries.ToList();
                    Response.Write("<script>alert('Please upload file.')</script>");
                    return View();
                }
            }
            catch
            {
                Response.Write("<script>alert('Please Properly enter the fields.')</script>");
            }
            return RedirectToAction("completeForm");
        }


        public string Decrypt_Password(string encryptpassword)
        {
            string pswstr = string.Empty;

            System.Text.UTF8Encoding encode_psw = new System.Text.UTF8Encoding();

            System.Text.Decoder Decode = encode_psw.GetDecoder();

            byte[] todecode_byte = Convert.FromBase64String(encryptpassword);

            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);

            char[] decoded_char = new char[charCount];

            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

            pswstr = new String(decoded_char);

            return pswstr;
        }


        [HttpGet]
        public ActionResult completeForm()
        {
            int i = Convert.ToInt32(Session["Admin"]);
            string name = dbd.vendor.Where(x => x.VendorId == i).Select(x => x.VendorName).FirstOrDefault();
            string emailid = dbd.vendor.Where(s => s.VendorId == i).Select(s => s.VendorEmail).FirstOrDefault();
            var fromAddress = new MailAddress("vmssoftwareapp@gmail.com");
            var toAddress = new MailAddress(emailid, name);
            var pass = dbd.vendor.Where(m => m.VendorEmail == emailid).Select(m => m.VendorPassword).FirstOrDefault();

            string a = Decrypt_Password(pass);

            const string fromPass = "vms@software";
            const string subject = "Confirmation mail";
            string body = "Congratulation" + " " + name + "," + Environment.NewLine + Environment.NewLine + "Thanks for choosing us for your businees." + Environment.NewLine + " Please Login Your account by following credential and add required fields." + Environment.NewLine + Environment.NewLine + " Vendor ID: " + i + Environment.NewLine + "Login Id: " + emailid + Environment.NewLine + " Password:  " + a;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPass)
            };

            using (var mess = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            }
            )
            {
                smtp.Send(mess);
            }


            ViewBag.username = name;
            ViewBag.emailid = emailid;

            return View();
        }

    }
}

