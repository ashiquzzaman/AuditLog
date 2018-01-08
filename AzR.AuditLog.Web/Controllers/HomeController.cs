using AzR.AuditLog.Business.Models;
using AzR.AuditLog.Business.Services;
using System;
using System.Web.Mvc;

namespace AzR.AuditLog.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private SampleService _sample;

        public HomeController(SampleService sample)
        {
            _sample = sample;
        }

        public ActionResult Index(bool ShowDeleted = false)
        {
            return View(_sample.GetAll(ShowDeleted));
        }


        public ActionResult Edit(int id)
        {
            var model = _sample.Get(id);
            return PartialView("Save", model);
        }

        public ActionResult Create()
        {
            var model = new SampleViewModel
            {
                Id = -1,
                DateOfBirth = DateTime.Now.AddYears(-25),
                Active = true,
            };
            return PartialView("Save", model);
        }

        public void Delete(int id)
        {
            _sample.Delete(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(SampleViewModel Rec)
        {
            if (Rec.Id == -1)
            {
                _sample.Create(Rec);
            }
            else
            {
                _sample.Update(Rec);
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
            var logs = _sample.GetAudit(id);
            return Json(logs, JsonRequestBehavior.AllowGet);
        }


    }
}