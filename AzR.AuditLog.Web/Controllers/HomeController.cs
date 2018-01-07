using AzR.AuditLog.Business.Models;
using System;
using System.Web.Mvc;

namespace AzR.AuditLog.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index(bool ShowDeleted = false)
        {
            var SD = new SampleModel();
            return View(SD.GetAllData(ShowDeleted));
        }


        public ActionResult Edit(int id)
        {
            SampleModel SD = new SampleModel();
            return PartialView("Save", SD.GetData(id));
        }

        public ActionResult Create()
        {
            var SD = new SampleModel
            {
                Id = -1,
                DateOfBirth = DateTime.Now.AddYears(-25)
            };
            // indicates record not yet saved
            return PartialView("Save", SD);
        }

        public void Delete(int id)
        {
            var SD = new SampleModel();
            SD.DeleteRecord(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(SampleModel Rec)
        {
            SampleModel SD = new SampleModel();
            if (Rec.Id == -1)
            {
                SD.CreateRecord(Rec);
            }
            else
            {
                SD.UpdateRecord(Rec);
            }
            return
                Json(
                    new
                    {
                        redirectTo = Url.Action("Index", "Home", new { Area = "" }),
                        message = "Record Saved successfully!!!",
                        position = "mainContent"
                    });
        }

        public JsonResult Audit(int id)
        {
            SampleModel SD = new SampleModel();
            var AuditTrail = SD.GetAudit(id);
            return Json(AuditTrail, JsonRequestBehavior.AllowGet);
        }


    }
}