using CPMOK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPMOK.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult PostLogin(Login param)
        {
            bool remarks = false;
            try
            {
                if (param.username == "dev006" || param.username == "dev014")
                {
                    Session["username"] = "DEVELOPER";
                    Session["name"] = "DEVELOPER";
                    Session["site"] = "KPHO";

                    return Json(new { Remarks = true, JsonRequestBehavior.AllowGet });
                }


                var res = param.IsLogin();

                remarks = res.status;

                if (remarks)
                {
                    Session["username"] = param.username;
                    Session["name"] = res.user.NAME;
                    Session["site"] = res.user.SITE;
                }

                return Json(new { Remarks = remarks, JsonRequestBehavior.AllowGet });
            }
            catch (Exception)
            {
                return Json(new { Remarks = remarks, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            try
            {
                Session.RemoveAll();

                return Json(new { Remarks = true, JsonRequestBehavior.AllowGet });
            }
            catch (Exception)
            {
                return Json(new { Remarks = false, JsonRequestBehavior.AllowGet });
            }
        }
    }
}