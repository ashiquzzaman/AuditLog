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
            SampleModel SD = new SampleModel();
            return View(SD.GetAllData(ShowDeleted));
        }


        public ActionResult Edit(int id)
        {
            SampleModel SD = new SampleModel();
            return View(SD.GetData(id));
        }

        public ActionResult Create()
        {
            SampleModel SD = new SampleModel();
            SD.Id = -1; // indicates record not yet saved
            SD.DateOfBirth = DateTime.Now.AddYears(-25);
            return View("Edit", SD);
        }

        public void Delete(int id)
        {
            SampleModel SD = new SampleModel();
            SD.DeleteRecord(id);
        }

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
            return Redirect("/");
        }

        public JsonResult Audit(int id)
        {
            SampleModel SD = new SampleModel();
            var AuditTrail = SD.GetAudit(id);
            return Json(AuditTrail, JsonRequestBehavior.AllowGet);
        }


    }
}