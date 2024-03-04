using CPMOK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPMOK.Controllers
{
    public class MasterController : Controller
    {
        public ActionResult GetDistrict(District req)
        {
            try
            {
                var result = req.GET();

                Response.StatusCode = 200;
                return Json(new { Data = result, Status = true, Message = "Data Berhasil Diambil" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDeptGroup(DeptMenuGrouping req)
        {
            try
            {
                var result = req.GET();

                Response.StatusCode = 200;
                return Json(new { Data = result, Status = true, Message = "Data Berhasil Diambil" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetACLGroup(ACLMenuGrouping req)
        {
            try
            {
                var result = req.GET();

                Response.StatusCode = 200;
                return Json(new { Data = result, Status = true, Message = "Data Berhasil Diambil" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetNrp(District req)
        {
            try
            {
                var result = req.GETNRP();

                Response.StatusCode = 200;
                return Json(new { Data = result, Status = true, Message = "Data Berhasil Diambil" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}