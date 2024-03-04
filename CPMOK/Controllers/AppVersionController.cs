using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CPMOK.Models;

namespace CPMOK.Controllers
{
    public class AppVersionController : Controller
    {
        // GET: AppVersion
        public ActionResult Index()
        {
            try
            {
                if (Session["username"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult GetVersion(CPMOK.Models.Version version)
        {
            try
            {
                var result = version.GET();

                Response.StatusCode = 200;
                return Json(new { Data = result, Status = true, Message = "Data Berhasil Diambil" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult createVersion(CPMOK.Models.Version req)
        {
            try
            {
                var result = req.CREATE();

                Response.StatusCode = 200;
                return Json(new { Data = result, Status = true, Message = "Data Berhasil Diambil" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateVersion(CPMOK.Models.Version req)
        {
            try
            {
                var result = req.Update();

                Response.StatusCode = 200;
                return Json(new { Data = result, Status = true, Message = "Data Berhasil Diupdate" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteVersion(CPMOK.Models.Version req)
        {
            try
            {
                var result = req.Delete();

                Response.StatusCode = 200;
                return Json(new { Data = result, Status = true, Message = "Data Berhasil Dihapus" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}