using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using VendorManagementSystem.CustomeAuth;
using VendorManagementSystem.Models;
using VendorManagementSystem.ProjectViewModel;

namespace VendorManagementSystem.Controllers
{
    [AuthAttribute]
    public class SuperAdminController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DBConnection db = new DBConnection();
        // GET: SuperAdmin
        [Authorize(Roles = "admin")]
        public ActionResult AdminPortal()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult VendorList(int? page)
        {
            try
            {
                    List<VendorModel> vmod = new List<VendorModel>();
                    List<BusinessDetails> vbuss = new List<BusinessDetails>();
                    vmod = db.vendor.Where(x => x.DataCompleted == true).ToList();
                    vbuss = db.businessdetails.Where(x => x.DataCompleted == true).ToList();
                    FinalmodelToDisplayData fmtdd = new FinalmodelToDisplayData();
                    fmtdd.vmode = vmod;
                    fmtdd.vbusss = vbuss;
                    return View(fmtdd);
                    //Response.Write("<script>alert('Are you sure you logged In?')</script>");
                    //return RedirectToAction("VendorLogin", "RegisteLogin");
            }
            catch(Exception)
            {
                Response.Write("<script>alert('List can't be open.')</script>");
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult IndivisualvendorDetail(int? vid)
        {
            try
            {
                var gen = new IndivisualvendorDetailModel();
                var genvar = db.vendor.Where(x => x.VendorId == vid).FirstOrDefault();
                gen.VId = genvar.VendorId;
                gen.SellCategory = genvar.SellingCategory;
                gen.VContact = genvar.VendorContact;
                gen.VEmail = genvar.VendorEmail;
                gen.VName = genvar.VendorName;

                var provar = db.vendordetails.Where(x => x.VendorId == vid).FirstOrDefault();
                gen.NoProducts = provar.NumberofProducts;
                gen.BusinessMode = provar.NatureOfBusiness;
                gen.MiPrice = provar.MinPrice;
                gen.MaPrice = provar.MaxPrice;
                gen.OperateFrom = provar.OperatingFrom;
                gen.HandledBy = provar.HandleBy;
                gen.Gen = provar.Gender;
                gen.Ag = provar.Age;
                gen.SellingOnline = provar.AlreadySellingOnline;

                var busvar = db.businessdetails.Where(z => z.VendorId == vid).FirstOrDefault();
                gen.CompanyName = busvar.BusinessName;
                gen.GSTProvisionalID = busvar.GSTINProvisionalID;
                gen.GSTDucument = busvar.GSTINDucument;
                gen.TANNo = busvar.TANNumber;
                gen.BusId = busvar.BusinessList;
                gen.TANDoc = busvar.TANDocument;
                gen.CINNo = busvar.CIN;
                gen.CINDoc = busvar.CINDocument;
                gen.RegisteredBusineesAddr = busvar.RegisteredBusineesAddress;

                var bankvar = db.bankdetails.Where(a => a.VendorId == vid).FirstOrDefault();
                gen.AccountHolderName = bankvar.AccountHoderName;
                gen.AccountNo = bankvar.AccountNumber;
                gen.IFSCcode = bankvar.IFSC;
                gen.BankName = bankvar.BankName;
                gen.Pancard = bankvar.PanCard;
                gen.panId = bankvar.Id;
                gen.PancardDoc = bankvar.PancardDocument;
                gen.AddressProofId = bankvar.AddressProof;
                gen.AddressProofDoc = bankvar.AddressProofDocument;
                gen.CancelCheque = bankvar.CancelledCheque;
                return View(gen);

                //Response.Write("<script>alert('Are you sure you logged In?')</script>");
                //return RedirectToAction("VendorLogin", "RegisteLogin");

            }

            catch (Exception)
            {
                return View();
            }
        }

        public FileResult downloadPan(int? id)
        {

            try
            {

                string fileName = db.bankdetails.Where(x => x.Id == id).Select(x => x.PancardDocument).FirstOrDefault();
                string _path = Path.Combine(Server.MapPath("~/PanCard"), fileName);

                string contentType = string.Empty;
                if (_path.Count() > 0)
                {
                    if (fileName.Contains(".pdf"))
                    {
                        contentType = "_path/pdf";
                    }

                    else if (fileName.Contains(".docx"))
                    {
                        contentType = "_path/docx";
                    }

                    return File(_path, contentType, fileName);
                }

                return File(_path, contentType, fileName);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileResult downloadAddressproof(int? id)
        {

            try
            {

                string fileName = db.bankdetails.Where(x => x.Id == id).Select(x => x.AddressProofDocument).FirstOrDefault();
                string _path = Path.Combine(Server.MapPath("~/AddressProof"), fileName);

                string contentType = string.Empty;
                if (_path.Count() > 0)
                {
                    if (fileName.Contains(".pdf"))
                    {
                        contentType = "_path/pdf";
                    }

                    else if (fileName.Contains(".docx"))
                    {
                        contentType = "_path/docx";
                    }

                    return File(_path, contentType, fileName);
                }

                return File(_path, contentType, fileName);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileResult downloadCheque(int? id)
        {

            try
            {

                string fileName = db.bankdetails.Where(x => x.Id == id).Select(x => x.CancelledCheque).FirstOrDefault();
                string _path = Path.Combine(Server.MapPath("~/CancelledCheque"), fileName);

                string contentType = string.Empty;
                if (_path.Count() > 0)
                {
                    if (fileName.Contains(".pdf"))
                    {
                        contentType = "_path/pdf";
                    }

                    else if (fileName.Contains(".docx"))
                    {
                        contentType = "_path/docx";
                    }

                    return File(_path, contentType, fileName);
                }

                return File(_path, contentType, fileName);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileResult downloadGST(int? id)
        {

            try
            {

                string fileName = db.businessdetails.Where(x => x.BusinessList == id).Select(x => x.GSTINDucument).FirstOrDefault();
                string _path = Path.Combine(Server.MapPath("~/GSTINDocument"), fileName);

                string contentType = string.Empty;
                if (_path.Count() > 0)
                {
                    if (fileName.Contains(".pdf"))
                    {
                        contentType = "_path/pdf";
                    }

                    else if (fileName.Contains(".docx"))
                    {
                        contentType = "_path/docx";
                    }

                    return File(_path, contentType, fileName);
                }

                return File(_path, contentType, fileName);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileResult downloadTAN(int? id)
        {

            try
            {

                string fileName = db.businessdetails.Where(x => x.BusinessList == id).Select(x => x.TANDocument).FirstOrDefault();
                string _path = Path.Combine(Server.MapPath("~/TANDocument"), fileName);

                string contentType = string.Empty;
                if (_path.Count() > 0)
                {
                    if (fileName.Contains(".pdf"))
                    {
                        contentType = "_path/pdf";
                    }

                    else if (fileName.Contains(".docx"))
                    {
                        contentType = "_path/docx";
                    }

                    return File(_path, contentType, fileName);
                }

                return File(_path, contentType, fileName);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileResult downloadCIN(int? id)
        {

            try
            {

                string fileName = db.businessdetails.Where(x => x.BusinessList == id).Select(x => x.CINDocument).FirstOrDefault();
                string _path = Path.Combine(Server.MapPath("~/CINDocument"), fileName);

                string contentType = string.Empty;
                if (_path.Count() > 0)
                {
                    if (fileName.Contains(".pdf"))
                    {
                        contentType = "_path/pdf";
                    }

                    else if (fileName.Contains(".docx"))
                    {
                        contentType = "_path/docx";
                    }

                    return File(_path, contentType, fileName);
                }

                return File(_path, contentType, fileName);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult ProductList(int? page)
        {
            try
            {
                    var product = db.uploadproduct.Where(x => x.IsDeleted == false).ToList().ToPagedList(page ?? 1, 3);
                    return View(product);

                   //Response.Write("<script>alert('Are you sure you logged In?')</script>");
                   //return RedirectToAction("VendorLogin", "RegisteLogin");
            }
            catch(Exception)
            {
                Response.Write("<script>alert('Something went wrong.')</script>");
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult productDetail(int? id)
        {
            if ( id != null)
            {
                var proddet = db.uploadproduct.Where(x => x.ProductId == id).FirstOrDefault();
                string pict = proddet.ProductImage;
                ViewBag.pic = pict;
                return View(proddet);
            }
            else
            {
                Response.Write("<script>alert('Are you sure you logged In?')</script>");
                return RedirectToAction("VendorLogin", "RegisteLogin");
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult productDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UploadProduct prodrec = db.uploadproduct.Find(id);
            if (prodrec == null)
            {
                return HttpNotFound();
            }
            return View(prodrec);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult productDelete(int? id, string name)
        {
            try
            {
                var prodctdetal = db.uploadproduct.Find(id);
                prodctdetal.IsDeleted = true;
                prodctdetal.TimeOfDeletion = DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("productDelete");
            }
            catch
            {
                return RedirectToAction("productDelete"); 
            }
        }

        public JsonResult productDelete(int ProdId)
        {
            bool result = false;
            try
            {
                var prodctdetal = db.uploadproduct.Find(ProdId);
                prodctdetal.IsDeleted = true;
                prodctdetal.TimeOfDeletion =  DateTime.Now;
                db.SaveChanges();
                result = true;
                return Json(result);
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult VisitorList(int? page, DateTime? searchFrom, DateTime? searchTo)
        {
            try
            {
                    if(searchFrom != null && searchTo != null)
                    {
                        var visitsearch = db.visitor.Where(x => x.IsDeleted == false && (x.MeetingDate >= searchFrom && x.MeetingDate <= searchTo)).ToList().ToPagedList(page ?? 1, 7);
                        return View(visitsearch);

                    }
                    else
                    {
                        var visit = db.visitor.Where(x => x.IsDeleted == false).OrderByDescending(x => x.MeetingDate).ToList().ToPagedList(page ?? 1, 3);
                        return View(visit);
                    }
                    
                    //Response.Write("<script>alert('Please login first.')</script>");
                    //return RedirectToAction("VendorLogin", "RegisteLogin");
            }
            catch (Exception)
            {
                Response.Write("<script>alert('You are not login')</script>");
                return View();
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Visitordetails(int? id)
        {
            if (id != null)
            {
                var visitdet = db.visitor.Where(x => x.Id == id).FirstOrDefault();
                return View(visitdet);
            }
            else
            {
                Response.Write("<script>alert('Are you sure you logged In?')</script>");
                return RedirectToAction("VendorLogin", "RegisteLogin");
            }
        }

        public JsonResult visitorDelete(int? VisitorId)
        {
            bool result = false;
            try
            {
               
                var personalDetail = db.visitor.Find(VisitorId);
                personalDetail.IsDeleted = true;
                personalDetail.TimeOfDeletion = DateTime.Now;
                db.SaveChanges();
                result = true;
                return Json(result);
            }
            catch(Exception)
            {
                return Json(result,JsonRequestBehavior.AllowGet);
            }
       }


        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult AddpurposeOfVisit()
        {
                var prps = db.purpose.Where(x => x.IsDeleted == false).ToList();
                return View(prps);

                //return RedirectToAction("vendorLogin","RegisteLogin");
            
        }

        public JsonResult AddpurposeOfVisit(string ProdId)
        {
            VisitPurpose vp = new VisitPurpose();
            var b = db.purpose.Where(x => x.Purpose == ProdId && (x.IsDeleted == false)).ToList();
            if (b.Count() > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            vp.Purpose = ProdId;
            db.purpose.Add(vp);
            db.SaveChanges();
            int a = db.purpose.Where(x => x.Purpose == ProdId).Select(x => x.PurposeId).FirstOrDefault();
            return Json(a, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditPurpose(int purId)
        {
            var a = db.purpose.Where(x => x.PurposeId == purId).FirstOrDefault();
            return Json(a.Purpose, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditPurpose(string EditValue, int purpId)
        {
            bool result = false;
            VisitPurpose vp = new VisitPurpose();
            vp = db.purpose.Where(x => x.PurposeId == purpId).FirstOrDefault();
            vp.Purpose = EditValue;
            db.SaveChanges();
            result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeletePurpose(int PropId)
        {
            bool result = false;
            try
            {
                var checkpurpose = db.purpose.Where(x => x.PurposeId == PropId).Select(x => x.IsUsed).FirstOrDefault();
                if(checkpurpose == false)
                {
                    var purpdel = db.purpose.Find(PropId);
                    purpdel.IsDeleted = true;
                    db.SaveChanges();
                    result = true;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        // view of selling category.
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult AddSellingCategory()
        {
                var prps = db.sellcategory.Where(x => x.IsDeleted == false).ToList();
                return View(prps);

                //return RedirectToAction("vendorLogin", "RegisteLogin");
        }

        //Adding value in selling category checkbox. 
        public JsonResult AddSellingCategory(string sell)
        {
            SellingCategoryModel vp = new SellingCategoryModel();
            var a = db.sellcategory.Where(x => x.hobbyname == sell && (x.IsDeleted == false)).ToList();
            if(a.Count() > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            vp.hobbyname = sell;
            db.sellcategory.Add(vp);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);


            //if (Session["superid"] != null)
            //{
            //    if(scm.hobbyname != null)
            //    {
            //        db.sellcategory.Add(scm);
            //        db.SaveChanges();
            //        return RedirectToAction("AdminPortal");
            //    }
            //    else
            //    {
            //        ViewBag.smsg = "Blank field can't be inserted.";
            //        return View();
            //    }
            //}
            //else
            //{
            //    ViewBag.smsg = "Please login first.";
            //    return View();
            //}
        }

        public JsonResult EditCategory(int catId)
        {
            var a = db.sellcategory.Where(x => x.Id == catId).FirstOrDefault();
            return Json(a.hobbyname, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditCategory(string EditValue, int catId)
        {
            bool result = false;
            SellingCategoryModel vp = new SellingCategoryModel();
            vp = db.sellcategory.Where(x => x.Id == catId).FirstOrDefault();
            vp.hobbyname = EditValue;
            db.SaveChanges();
            result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCategory(int CtgId)
        {
            bool result = false;
            try
            {
                var checkcategory = db.sellcategory.Where(x => x.Id == CtgId).Select(x => x.IsUsed).FirstOrDefault();
                if(checkcategory == false)
                {
                    var purpdel = db.sellcategory.Find(CtgId);
                    purpdel.IsDeleted = true;
                    db.SaveChanges();
                    result = true;
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        //Deleting multiple records at a time.
        [HttpPost]
        public ActionResult DeleteFiles(int?[] ids)
        {
            try
            {
                if (ids != null)
                {
                    List<int> TaskIds = ids.Select(x => x.Value).ToList();
                    for (var i = 0; i < TaskIds.Count(); i++)
                    {
                        var todo = db.uploadproduct.Find(TaskIds[i]);
                        todo.IsDeleted = true;
                        todo.TimeOfDeletion = DateTime.Now;
                        db.SaveChanges();
                    }
                    return RedirectToAction("ProductList");
                }
                else
                {
                    Response.Write("<script>alert('File not selected')</script>");
                    return RedirectToAction("ProductList");
                }
            }
            catch(Exception)
            {
                Response.Write("<script>alert('File can't be deleted')</script>");
                return RedirectToAction("ProductList");
            }
           
           
        }

        //Storing Login, Logout and time spent of vendor.
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult LoginLogoutDetails(int? page, string search)
        {
            try
            {
                    if (search != "" && search != null)
                    {
                        ViewBag.url = Request.Url.AbsoluteUri;
                        int id;
                        Int32.TryParse(search, out id);

                        var loginlogoutdetails = db.loginouttime.Where(x => x.VendorId == id ).ToList().ToPagedList(page ?? 1, 5);
                        return View(loginlogoutdetails);
                    }
                    else
                    {
                        var loginlogoutdetails = db.loginouttime.ToList().ToPagedList(page ?? 1, 10);
                        return View(loginlogoutdetails);
                    }
                    //return View();
            }
            catch
            {
                int a;
                Int32.TryParse(search, out a);
                var lt = db.loginouttime.Where(x => x.VendorId == a).ToList().ToPagedList(page ?? 1, 1);
                Response.Write("<script>alert('No record found.')</script>");
                return View(lt);
            }
        }

        //view file in webpage
        public FileResult InsuranceReportCIN(int? id)
        {
            string fileName = db.businessdetails.Where(x => x.BusinessList == id).Select(x => x.CINDocument).FirstOrDefault();
            string extension = Path.GetExtension(fileName);
            string path1 = Path.Combine(Server.MapPath("~/CINDocument"), fileName);
            string contentType = "application/pdf";
            if (extension == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (extension == ".docx")
            {
                contentType = "application/docx";
            }
            string file = path1;
            return new FilePathResult(file, contentType);
        }

        //view file in webpage
        public FileResult InsuranceReportGST(int? id)
        {
            string fileName = db.businessdetails.Where(x => x.BusinessList == id).Select(x => x.GSTINDucument).FirstOrDefault();
            string extension = Path.GetExtension(fileName);
            string path1 = Path.Combine(Server.MapPath("~/GSTINDocument"), fileName);
            string contentType = "application/pdf";
            if (extension == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (extension == ".docx")
            {
                contentType = "application/docx";
            }
            string file = path1;
            return new FilePathResult(file, contentType);
        }

        //view file in webpage
        public FileResult InsuranceReportTAN(int? id)
        {
            string fileName = db.businessdetails.Where(x => x.BusinessList == id).Select(x => x.TANDocument).FirstOrDefault();
            string extension = Path.GetExtension(fileName);
            string path1 = Path.Combine(Server.MapPath("~/TANDocument"), fileName);
            string contentType = "application/pdf";
            if (extension == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (extension == ".docx")
            {
                contentType = "application/docx";
            }
            string file = path1;
            return new FilePathResult(file, contentType);
        }

        //view file in webpage
        public FileResult InsuranceReportADD(int? id)
        {
            string fileName = db.bankdetails.Where(x => x.Id == id).Select(x => x.AddressProofDocument).FirstOrDefault();
            string extension = Path.GetExtension(fileName);
            string path1 = Path.Combine(Server.MapPath("~/AddressProof"), fileName);
            string contentType = "application/pdf";
            if (extension == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (extension == ".docx")
            {
                contentType = "application/docx";
            }
            string file = path1;
            return new FilePathResult(file, contentType);
        }

        //view file in webpage
        public FileResult InsuranceReportPAN(int? id)
        {
            string fileName = db.bankdetails.Where(x => x.Id == id).Select(x => x.PancardDocument).FirstOrDefault();
            string extension = Path.GetExtension(fileName);
            string path1 = Path.Combine(Server.MapPath("~/PanCard"), fileName);
            string contentType = "application/pdf";
            if (extension == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (extension == ".docx")
            {
                contentType = "application/docx";
            }
            string file = path1;
            return new FilePathResult(file, contentType);
        }

        //view file in webpage
        public FileResult InsuranceReportCHE(int? id)
        {
            string fileName = db.bankdetails.Where(x => x.Id == id).Select(x => x.CancelledCheque).FirstOrDefault();
            string extension = Path.GetExtension(fileName);
            string path1 = Path.Combine(Server.MapPath("~/CancelledCheque"), fileName);
            string contentType = "application/pdf";
            if (extension == ".pdf")
            {
                contentType = "application/pdf";
            }
            else if (extension == ".docx")
            {
                contentType = "application/docx";
            }
            string file = path1;
            return new FilePathResult(file, contentType);
        }

        //Getting list of orders to sent to vendor.
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult ProductOrder()
        {
            try
            {
                    var listofproductTosend = db.uploadproduct.Where(x => x.OrderApproved == false && (x.IsDeleted == false)).ToList();
                    return View(listofproductTosend);

                    //return RedirectToAction("VendorLogin", "RegisteLogin");
            }
            catch
            {
                return View();
            }
        }

        //Sending order to vendor.
        public ActionResult SendOrderToVendor(int? prodId, int? vendId)
        {
            var prod = db.uploadproduct.Where(x => x.ProductId == prodId && (x.VendorId == vendId)).FirstOrDefault();
            prod.OrderApproved = true;
            prod.ProductQuantity = prod.ProductQuantity - 1;
            db.SaveChanges();
            return RedirectToAction("ProductOrder");
        }

        //Sending multiple orders
        [HttpPost]
        public ActionResult sendMultOrders(int? [] ckboxId)
        {
            try
            {
                if (ckboxId != null)
                {
                    List<int> TaskIds = ckboxId.Select(x => x.Value).ToList();
                    for (var i = 0; i < TaskIds.Count(); i++)
                    {
                        var todo = db.uploadproduct.Find(TaskIds[i]);
                        todo.OrderApproved = true;
                        todo.ProductQuantity = todo.ProductQuantity - 1;
                        db.SaveChanges();
                    }
                    return RedirectToAction("ProductOrder");
                }
                else
                {
                    Response.Write("<script>alert('Order not selected')</script>");
                    return RedirectToAction("ProductOrder");
                }
            }
            catch (Exception)
            {
                Response.Write("<script>alert('Order can't be sent.')</script>");
                return RedirectToAction("ProductOrder");
            }
        }

        //List of order recieved by vendor.
        [HttpGet]
        public ActionResult OrderRecievedByVenodor()
        {
            var orbv = db.uploadproduct.Where(x => x.OrderApproved == true).ToList();
            return View(orbv);
        }

        //Autocomplete implemetation.
        public JsonResult SearchRecords(string term)
        {
            List<string> ac;
            ac = db.loginouttime.Where(x => x.VendorId.ToString().StartsWith(term)).Select(x => x.VendorId.ToString()).Distinct().ToList();
            return Json(ac, JsonRequestBehavior.AllowGet);
        }

        //Exporting
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportToWordVL(string GridHtml)
        {
            // set the data source
            var data = db.visitor.ToList().Select(x => new { x.Name, x.Email, x.Purpose, x.ContactPerson, x.MeetingDate, x.AddressLine });
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
        public ActionResult ExportExcelVL(string GridHtml)
        {
            var data = db.visitor.ToList().Select(x => new { x.Name, x.Email, x.Purpose, x.ContactPerson, x.MeetingDate, x.AddressLine });
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
        public ActionResult ExportToWordLL(string GridHtml)
        {
            // set the data source
            var data = db.loginouttime.ToList().Select(x => new { x.VendorId, x.LogInTime, x.LogOutTime});
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
        public ActionResult ExportExcelLL(string GridHtml)
        {
            var data = db.loginouttime.ToList().Select(x => new { x.VendorId, x.LogInTime, x.LogOutTime });
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

        
        public void PrintExcel() // Export to Excel
        {
            List<UploadProduct> exceldata = new List<UploadProduct>();
            exceldata = db.uploadproduct.ToList();
            //create object to webgrid  
            WebGrid grid = new WebGrid(source: exceldata, canPage: false, canSort: false);
            string gridData = grid.GetHtml(
            columns: grid.Columns(
                            grid.Column("ProductID", "ProductID"),
                            grid.Column("ProductName", "ProductName"),
                            grid.Column("ProductPrice", "ProductPrice"),
                            grid.Column("SellingCategory", "SellingCategory")
                            )).ToString();
            Response.ClearContent();
            //give name to excel sheet.  
            Response.AddHeader("content-disposition", "attachment; filename=UserData.xls");
            //specify content type  
            Response.ContentType = "application/excel";
            //write excel data using this method   
            Response.Write(gridData);
            Response.End();
        }
        public void PrintWord() // Export to MS-Word
        {

            List<UploadProduct> exceldata = new List<UploadProduct>();

            {
                exceldata = db.uploadproduct.ToList();
            }
            //create object to webgrid  
            WebGrid grid = new WebGrid(source: exceldata, canPage: false, canSort: false);
            string gridData = grid.GetHtml(
            columns: grid.Columns(
                            grid.Column("ProductID", "ProductID"),
                            grid.Column("ProductName", "ProductName"),
                            grid.Column("ProductPrice", "ProductPrice"),
                            grid.Column("SellingCategory", "SellingCategory")
                            )).ToString();
            Response.ClearContent();
            //give name to excel sheet.  
            Response.AddHeader("content-disposition", "attachment; filename=UserData.doc");
            //specify content type  
            Response.ContentType = "application/ms-word";
            //write excel data using this method   
            Response.Write(gridData);
            Response.End();
        }
        public FileStreamResult CreatePdf() //Export to PDF
        {
            List<UploadProduct> all = new List<UploadProduct>();
            all = db.uploadproduct.ToList();
            WebGrid grid = new WebGrid(source: all, canPage: false, canSort: false);
            string gridHtml = grid.GetHtml(
                   columns: grid.Columns(
                           grid.Column("ProductID", "ProductID"),
                            grid.Column("ProductName", "ProductName"),
                            grid.Column("ProductPrice", "ProductPrice"),
                            grid.Column("SellingCategory", "SellingCategory")
                           )
                    ).ToString();
            string exportData = String.Format("{0}{1}", "", gridHtml);
            var bytes = System.Text.Encoding.UTF8.GetBytes(exportData);
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();
                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return new FileStreamResult(output, "application/pdf");
            }
        }


        //Database change when new vendor added notification
        public JsonResult getVendorNotification()
        {
            var notificationregisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationClass nc = new NotificationClass();
            var list = nc.GetContacts(notificationregisterTime);
            //update session here to get only new added contacts(notification)
            Session["LastUpdated"] = DateTime.Now;
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        //Database change when new product added notification
        public JsonResult getProductNotification()
        {
            var notificationregisterTime = Session["LastUpdate"] != null ? Convert.ToDateTime(Session["LastUpdate"]) : DateTime.Now;
            NotificationClass nc = new NotificationClass();
            var list = nc.GetProduct(notificationregisterTime);
            //update session here to get only new added contacts(notification)
            Session["LastUpdate"] = DateTime.Now;
            return Json(list, JsonRequestBehavior.AllowGet);
        }

    }
}