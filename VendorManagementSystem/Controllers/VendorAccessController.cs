using DocumentFormat.OpenXml.Drawing.Charts;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using VendorManagementSystem.CustomeAuth;
using VendorManagementSystem.Models;
using VendorManagementSystem.ProjectViewModel;

namespace VendorManagementSystem.Controllers
{
    [AuthAttribute]
    public class VendorAccessController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //Database controller connectivity.
        DBConnection db = new DBConnection();

        // GET: VendorAccess
        [Authorize(Roles = "Vendor")]
        public ActionResult Index()
        {
         try
          {
            VendorAccessDetails vad = new VendorAccessDetails();
         
                //int a = Convert.ToInt32(Session["Adminid"]);
                string email = User.Identity.Name;
                int a = db.vendor.Where(x => x.VendorEmail == email).Select(x => x.VendorId).FirstOrDefault();
                string name = db.vendor.Where(x => x.VendorId == a).ToList().FirstOrDefault().VendorName;//.Select(x => x.VendorName).ToString();
                /*string email = db.vendor.Where(x => x.VendorId == a).ToList().FirstOrDefault().VendorEmail;*///Select(x => x.VendorEmail).ToString();
                string contact = db.vendor.Where(x => x.VendorId == a).ToList().FirstOrDefault().VendorContact;//Select(x => x.VendorContact).ToString();
                string sell = db.vendor.Where(x => x.VendorId == a).ToList().FirstOrDefault().SellingCategory;//Select(x => x.SellingCategory).ToString();
                string gender = db.vendordetails.Where(y => y.VendorId == a).ToList().FirstOrDefault().Gender;//Select(y => y.Gender).ToString();
                int age = db.vendordetails.Where(y => y.VendorId == a).ToList().FirstOrDefault().Age;//Select(y => y.Age).ToString();
                string handler = db.vendordetails.Where(y => y.VendorId == a).ToList().FirstOrDefault().HandleBy;//Select(y => y.HandleBy).ToString();
                string company = db.businessdetails.Where(z => z.VendorId == a).ToList().FirstOrDefault().BusinessName;//Select(z => z.BusinessName).ToString();

                vad.Name = name;
                vad.Email = email;
                vad.Company = company;
                vad.Contact = contact;
                vad.Age = age;
                vad.Gender = gender;
                vad.Sell = sell;
                vad.Id = a;
                return View(vad);
         }
        catch(Exception)
         {
           Response.Write("<script>alert('Something went wrong')</script>");
           return View();
         }
       }

        // GET: VendorAccess/Details/5
        [Authorize(Roles = "admin")]
        public ActionResult ProductDetails(int? id)
        {
            if(id != null)
            {
                var pdetail = db.uploadproduct.Where(x => x.ProductId == id).FirstOrDefault();
                string imgs = pdetail.ProductImage;
                ViewBag.image = imgs;
                return View(pdetail);
            }
            else
            {
                RedirectToAction("HomePage","RegisteLogin");
            }
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public ActionResult changeImage(int? id)
        {
            var prodImg = db.uploadproduct.Where(x => x.ProductId == id).FirstOrDefault();
            string pict = prodImg.ProductImage;
            ViewBag.pict = pict;
            return View(prodImg);
        }

        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public ActionResult changeImage(UploadProduct puimg, HttpPostedFileBase[] file)
        {
           
               string files = null;
               string extension;
               string filename;
               string path;
               if (file[0] != null)
                {
                 foreach (var item in file)
                  {
                    extension = Path.GetExtension(item.FileName);
                    if (extension == ".jpg" || extension == ".jpeg")
                     {
                      filename = Path.GetFileName(item.FileName);
                      path = Path.Combine(Server.MapPath("~/ProductImage"), filename);
                      item.SaveAs(path);
                      files += filename + ",";
                     }
                    else
                     {
                        Response.Write("<script>alert('Please upload product image in .jpg formate')</script>");
                        var prodImg2 = db.uploadproduct.Where(x => x.ProductId == puimg.ProductId).FirstOrDefault();
                        string pict2 = prodImg2.ProductImage;
                        ViewBag.pict = pict2;
                        return View(prodImg2);
                     }
                    }
                    files = files.TrimEnd(',');
                 }
                else
                  {
                    Response.Write("<script>alert('Please choose image.')</script>");
                    var prodImg1 = db.uploadproduct.Where(x => x.ProductId == puimg.ProductId).FirstOrDefault();
                    string pict1 = prodImg1.ProductImage;
                    ViewBag.pict = pict1;
                    return View(prodImg1);
                 }

              puimg.ProductImage = files;
              db.Entry(puimg).State = EntityState.Modified;
              db.SaveChanges();
              var prodImg = db.uploadproduct.Where(x => x.ProductId == puimg.ProductId).FirstOrDefault();
              string pict = prodImg.ProductImage;
              ViewBag.pict = pict;
              return View(prodImg);
            }


        // GET: VendorAccess/Create
        [Authorize(Roles = "Vendor")]
        public ActionResult ProductUploadMethod()
        {
            return View();
        }

        // POST: VendorAccess/Create
        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public ActionResult ProductUploadMethod(ProductUploadView puv,  HttpPostedFileBase[] file)
        {
            try
            {
                    if (puv.SellingCategory != null)
                    {
                        UploadProduct up = new UploadProduct();
                        up.VendorId = Convert.ToInt32(Session["Adminid"]);
                        up.ProductName = puv.ProductName;
                        up.ProductPrice = puv.ProductPrice;
                        up.ProductDescription = puv.ProductDescription;
                        up.OrderApproved = false;
                        up.OrderRecieved = false;
                        up.OrderDelivered = false;
                        string files = null;
                        if(file != null)
                        {
                            foreach (var item in file)
                            {
                                string extension = Path.GetExtension(item.FileName);
                                if (extension == ".jpg" || extension == ".jpeg")
                                {
                                    string filename = Path.GetFileName(item.FileName);
                                    string path = Path.Combine(Server.MapPath("~/ProductImage"), filename);
                                    item.SaveAs(path);
                                    files += filename + ",";
                                }
                                else
                                {
                                    Response.Write("<script>alert('Please upload product image in .jpg formate')</script>");
                                    files = null;
                                    return View(puv);
                                }
                            }
                            files = files.TrimEnd(',');
                        }
                        else
                        {
                            files = "defaultimage.png";
                        }


                        up.ProductImage = files;
                        up.ProductQuantity = puv.ProductQuantity;
                        up.SellingCategory = puv.SellingCategory;
                        up.AddedOn = DateTime.Now;
                        db.uploadproduct.Add(up);
                        db.SaveChanges();

                    //Sending confiramtion mail regarding product has been uploaded
                    //int i = Convert.ToInt32(Session["Adminid"]);
                    
                        string email = User.Identity.Name;
                        int i = db.vendor.Where(x => x.VendorEmail == email).Select(x => x.VendorId).FirstOrDefault();
                        string name = db.vendor.Where(s => s.VendorId == i).Select(s => s.VendorName).FirstOrDefault();
                        var fromAddress = new MailAddress("vmssoftwareapp@gmail.com");
                        var toAddress = new MailAddress(email, name);

                        var pdetails = db.uploadproduct.Where(x => x.VendorId == i && (x.ProductName == puv.ProductName)).FirstOrDefault();
                        var pid = pdetails.ProductId;
                        var pname = pdetails.ProductName;
                        var pquant = pdetails.ProductQuantity;


                        const string fromPass = "vms@software";
                        const string subject = "Product has been uploaded.";
                        string body = "Congratulation" + " " + name + "," + Environment.NewLine + Environment.NewLine + "You have successfully added following product." + Environment.NewLine + "Product Id: " + pid + Environment.NewLine + " Product Name: " + pname + Environment.NewLine + "Quantity: " + pquant + Environment.NewLine + Environment.NewLine + " Thanks for being with us" + Environment.NewLine + " Happy Selling " + Environment.NewLine + "VMS Team";
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
                        return RedirectToAction("VendorList");
                    }
                    else
                    {
                        Response.Write("<script>alert('Please upload again.')</script>");
                        return View(puv);
                    }
            }
          catch
           {
              Response.Write("<script>alert('Please fill all required field properly.')</script>");
              return View();
           }
        }

        //[HttpPost]
        //public JsonResult uploadThroughModal(ProductListProductUploadMethod plpum, HttpPostedFileBase Upload)
        //{
        //    try
        //    {
        //        if (Session["Adminid"] != null)
        //        {
        //            if (plpum.upload != null && plpum.upload.SellingCategory != null)
        //            {
        //                UploadProduct up1 = new UploadProduct();
        //                up1.VendorId = Convert.ToInt32(Session["Adminid"]);
        //                up1.ProductName = plpum.upload.ProductName;
        //                up1.ProductPrice = plpum.upload.ProductPrice;
        //                up1.ProductDescription = plpum.upload.ProductDescription;

        //                if (Upload != null)
        //                {
        //                    string extension = Path.GetExtension(Upload.FileName);
        //                    string filename = Path.GetFileName(Upload.FileName);
        //                    string path = Path.Combine(Server.MapPath("~/ProductImage"), filename);
        //                    Upload.SaveAs(path);
        //                    up1.ProductImage = filename;
        //                }
        //                else
        //                {
        //                    up1.ProductImage = "defaultimage.png";
        //                }

        //                up1.ProductQuantity = plpum.upload.ProductQuantity;
        //                up1.SellingCategory = plpum.upload.SellingCategory;
        //                up1.AddedOn = DateTime.Now;
        //                up1.OrderApproved = false;
        //                up1.OrderRecieved = false;
        //                up1.OrderDelivered = false;
        //                db.uploadproduct.Add(up1);
        //                db.SaveChanges();

        //                //Sending confiramation mail regarding product upload
        //                int i = Convert.ToInt32(Session["Adminid"]);
        //                string name = db.vendor.Where(x => x.VendorId == i).Select(x => x.VendorName).FirstOrDefault();
        //                string emailid = db.vendor.Where(s => s.VendorId == i).Select(s => s.VendorEmail).FirstOrDefault();
        //                var fromAddress = new MailAddress("vmssoftwareapp@gmail.com");
        //                var toAddress = new MailAddress(emailid, name);

        //                var pdetails = db.uploadproduct.Where(x => x.VendorId == i && (x.ProductName == plpum.upload.ProductName)).FirstOrDefault();
        //                var pid = pdetails.ProductId;
        //                var pname = pdetails.ProductName;
        //                var pquant = pdetails.ProductQuantity;


        //                const string fromPass = "vms@software";
        //                const string subject = "Product has been uploaded.";
        //                string body = "Congratulation" + " " + name + "," + Environment.NewLine + Environment.NewLine + "You have successfully added following product." + Environment.NewLine + "Product Id: " + pid + Environment.NewLine + " Product Name: " + pname + Environment.NewLine + "Quantity: " + pquant + Environment.NewLine + Environment.NewLine + " Thanks for being with us" + Environment.NewLine + " Happy Selling " + Environment.NewLine + " VMS Team";
        //                var smtp = new SmtpClient
        //                {
        //                    Host = "smtp.gmail.com",
        //                    Port = 587,
        //                    EnableSsl = true,
        //                    DeliveryMethod = SmtpDeliveryMethod.Network,
        //                    UseDefaultCredentials = false,
        //                    Credentials = new NetworkCredential(fromAddress.Address, fromPass)
        //                };

        //                using (var mess = new MailMessage(fromAddress, toAddress)
        //                {
        //                    Subject = subject,
        //                    Body = body
        //                })
        //                {
        //                    smtp.Send(mess);
        //                }
        //                return Json(true, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //                Response.Write("<script>alert('Please upload again.')</script>");
        //                return Json(false, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        else
        //        {
        //            Response.Write("<script>alert('Please upload again.')</script>");
        //            return Json(false, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch
        //    {
        //        Response.Write("<script>alert('Please fill all required field properly.')</script>");
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}


        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public ActionResult VendorList(int? page, string search)
        {
            ProductListProductUploadMethod upobj = new ProductListProductUploadMethod();
            if (search != "" && search != null)
            {
                string email = User.Identity.Name;
                int id = db.vendor.Where(x => x.VendorEmail == email).Select(x => x.VendorId).FirstOrDefault();
                //int id = Convert.ToInt32(Session["Adminid"]);
                upobj.uploadproduct = db.uploadproduct.Where(x => x.ProductName.Contains(search) && (x.VendorId == id) || (x.SellingCategory == search && (x.VendorId == id))).ToList().ToPagedList(page ?? 1, 2);
                if(upobj.uploadproduct.Count() > 0)
                {
                    return View(upobj);
                }
                else
                {
                    return View(upobj);
                }
            }
            else
            {
                //int id = Convert.ToInt32(Session["Adminid"]);
                string email = User.Identity.Name;
                int id = db.vendor.Where(x => x.VendorEmail == email).Select(x => x.VendorId).FirstOrDefault();
                upobj.uploadproduct = db.uploadproduct.Where(x => x.VendorId == id).ToList().ToPagedList(page ?? 1, 2);
                return View(upobj);
            }

        }

        // view method to edit uploaded record.
        [Authorize(Roles = "Vendor")]
        public ActionResult EditRecord(int? id)
        {
            var er = db.uploadproduct.Find(id);
            return View(er);
        }

        // Edit method for uploaded rocord.
        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public ActionResult EditRecord(int? id, UploadProduct upe)
        {
            try
            {
                //int eid = Convert.ToInt32(Session["Adminid"]);
                string email = User.Identity.Name;
                int eid = db.vendor.Where(x => x.VendorEmail == email).Select(x => x.VendorId).FirstOrDefault();
                upe.VendorId = eid;
                string dbimage = db.uploadproduct.Where(x => x.ProductId == eid).Select(x => x.ProductImage).FirstOrDefault();
                upe.ProductImage = dbimage;
                db.Entry(upe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("VendorList");
            }
            catch(Exception)
            {
                return View();
            }
        }

        //Exporting Pdf File.
        [HttpPost]
        [ValidateInput(false)]
        public FileResult Export(string GridHtml)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(GridHtml);
                Document pdfDoc = new Document(PageSize.A4, 1f, 1f, 50f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", "Data.pdf");
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportToWord(string GridHtml)
        {
            // set the data source
            var data = db.vendor.Where(x => x.DataCompleted == true).ToList().Select(x => new { x.VendorId, x.VendorName, x.VendorEmail, x.VendorContact });
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;
            // set the header
            Response.AddHeader("content-disposition", "attachment: filename = visitor.doc");
            Response.ContentType = "application/ms-word";
            Response.Charset = "";
            // create HtmlTextWriter object with StringWriter
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // render the GridView to the HtmlTextWriter
                    gridview.RenderControl(htw);
                    // Output the GridView content saved into StringWriter
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportExcel(string GridHtml)
        {
            var data = db.vendor.Where(x => x.DataCompleted == true).ToList().Select(x => new { x.VendorId, x.VendorName, x.VendorEmail, x.VendorContact });
            GridView grid = new GridView();
            grid.DataSource = data;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment: filename=ExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
            //return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Grid.xls");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportToWordVA(string GridHtml)
        {
            // set the data source
            int b = 0;
            var data = db.uploadproduct.Where(x => x.IsDeleted == false).ToList().Select(x => new { x.VendorId, x.ProductId, x.ProductName, x.SellingCategory, x.ProductDescription });
            //if (Session["Adminid"] != null)
            //{
                 //b = Convert.ToInt32(Session["Adminid"]);
                string email = User.Identity.Name;
                b = db.vendor.Where(x => x.VendorEmail == email).Select(x => x.VendorId).FirstOrDefault();
                data = db.uploadproduct.Where(x => x.IsDeleted == false && (x.VendorId == b)).ToList().Select(x => new { x.VendorId, x.ProductId, x.ProductName, x.SellingCategory, x.ProductDescription });
            //}
            GridView gridview = new GridView();
            gridview.DataSource = data;
            gridview.DataBind();

            // Clear all the content from the current response
            Response.ClearContent();
            Response.Buffer = true;
            // set the header
            Response.AddHeader("content-disposition", "attachment: filename = visitor.doc");
            Response.ContentType = "application/ms-word";
            Response.Charset = "";
            // create HtmlTextWriter object with StringWriter
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                {
                    // render the GridView to the HtmlTextWriter
                    gridview.RenderControl(htw);
                    // Output the GridView content saved into StringWriter
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportExcelVA(string GridHtml)
        {
            int b = 0;
            //if (Session["Adminid"] != null)
            //{
            //    b = Convert.ToInt32(Session["Adminid"]);
            //}
            //if (Session["superid"] != null)
            //{
            //    b = Convert.ToInt32(Session["superid"]);
            //}
            string email = User.Identity.Name;
            b = db.vendor.Where(x => x.VendorEmail == email).Select(x => x.VendorId).FirstOrDefault();
            var data = db.uploadproduct.Where(x => x.IsDeleted == false && (x.VendorId == b)).ToList().Select(x => new { x.VendorId, x.ProductId, x.ProductName, x.SellingCategory, x.ProductDescription });
            GridView grid = new GridView();
            grid.DataSource = data;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment: filename=ExcelFile.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
            //return File(Encoding.ASCII.GetBytes(GridHtml), "application/vnd.ms-excel", "Grid.xls");
        }

        [HttpGet]
        [Authorize(Roles = "Vendor")]
        public ActionResult VendorDetailEdit(int? id)
        {
            List<SellingCategoryModel> sc = db.sellcategory.Where(x => x.IsDeleted == false).ToList();
            ViewBag.sellingcat = sc;
            if (id != null)
            {
                var pdetail = db.vendor.Where(x => x.VendorId == id).FirstOrDefault();
                ViewBag.checkeditem = pdetail.SellingCategory;
                return View(pdetail);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Vendor")]
        public ActionResult VendorDetailEdit(List<SellingCategoryModel> hobby, VendorModel vma)
        {
            try
            {
                var str1 = "";
                foreach (var item in hobby.Where(x => x.IsChecked).ToList())
                {
                    if (item.IsChecked)
                    {
                        str1 += item.hobbyname.ToString() + ",";
                    }
                }
                str1 = str1.TrimEnd(',');
                vma.SellingCategory = str1;
                db.Entry(vma).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("VendorDetailEdit");
            }
                
        }

        [HttpGet]
        public ActionResult ServerNotify()
        {
            return View();
        }


        [HttpGet]
        public ActionResult recievedOrder()
        {
            //int vid = Convert.ToInt32(Session["Adminid"]);
            string email = User.Identity.Name;
            int vid = db.vendor.Where(x => x.VendorEmail == email).Select(x => x.VendorId).FirstOrDefault();
            var Rorder = db.uploadproduct.Where(x => x.VendorId == vid && (x.OrderApproved == true) && (x.OrderDelivered == false)).ToList();
            return View(Rorder);
        }

        //Confirming order recieved.
        public ActionResult ConfirmRecieving(int? prodId, int? vendId)
        {
            var orderconf = db.uploadproduct.Where(x => x.VendorId == vendId && (x.ProductId == prodId)).FirstOrDefault();
            orderconf.OrderRecieved = true;
            db.SaveChanges();
            return RedirectToAction("recievedOrder");
        }

        //Confirming order delivered.
        public ActionResult ConfirmDelivery(int? prodId, int? vendId)
        {
            var orderRecConfirm = db.uploadproduct.Where(x => x.VendorId == vendId && (x.ProductId == prodId)).FirstOrDefault();
            if(orderRecConfirm.OrderRecieved == true)
            {
                orderRecConfirm.OrderDelivered = true;
                db.SaveChanges();
                return RedirectToAction("recievedOrder");
            }
            else
            {
                ViewBag.mess = "Please confirm order first.";
                return RedirectToAction("recievedOrder");
            }
        }

        //Autocomplete implemetation.
        public JsonResult GetRecords(string term)
        {
            List<string> ac;
            //int id = Convert.ToInt32(Session["Adminid"]);
            string email = User.Identity.Name;
            int id = db.vendor.Where(x => x.VendorEmail == email).Select(x => x.VendorId).FirstOrDefault();
            ac = db.uploadproduct.Where(x => x.ProductName.StartsWith(term) && (x.VendorId == id)).Select(x => x.ProductName).Distinct().ToList();
            //var records = db.uploadproduct.Where(x => x.ProductName.Contains(prefix)).Select(x => x.ProductName).ToList();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddProduct()
        {
            return PartialView("View");
        }
    }
}
