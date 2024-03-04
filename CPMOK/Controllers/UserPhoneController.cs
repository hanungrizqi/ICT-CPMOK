using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPMOK.Controllers
{
    public class UserPhoneController : Controller
    {
        // GET: UserPhone
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

        public ActionResult GetPhone(CPMOK.Models.Phone version)
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
        public ActionResult createPhone(CPMOK.Models.Phone req)
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
        public ActionResult updatePhone(CPMOK.Models.Phone req)
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
        public ActionResult DeletePhone(CPMOK.Models.Phone req)
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