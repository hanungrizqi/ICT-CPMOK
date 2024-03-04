using CPMOK.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPMOK.Controllers
{
    public class PushNotifController : Controller
    {
        DB_MOKDataContext mok = new DB_MOKDataContext();
        string authKey = ConfigurationManager.AppSettings["AuthKey"].ToString();

        // GET: PushNotif
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

        [HttpPost]
        public ActionResult pushNotifNRP(string nrp, string title, string body, string target, string eventid)
        {
            try
            {
                var notification = new CPMOK.Models.Notification();  // Create an instance
                var result = notification.PUSHNRP(nrp, title, body, target, eventid);  // Call the non-static method on the instance

                Response.StatusCode = 200;
                return Json(new { Data = result, Status = true, Message = "Berhasil Insert Push Notif NRP" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult pb_cust_insertDataPushNotifOnDemand(ClsPushNotifOnDemand pushNotif)
        {
            try
            {
                //ClsPushNotifOnDemand pushNotifOnDemand = new ClsPushNotifOnDemand();
                if (pushNotif.notifType == "DISTRICT")
                {
                    //for type district
                    if (pushNotif.site.ToUpper().Equals("ALL"))
                    {
                        var listRecipients = mok.VW_RECIPIENTs;
                        //mok.VW_RECIPIENTs.Where(vwr => vwr.token == "LD9MQEDsWEqtGQc0JKXO9g==");
                        //mok.VW_RECIPIENTs.Where(vwr => vwr.SITE.ToUpper() == pushNotif.site.ToUpper());

                        foreach (var recipient in listRecipients)
                        {
                            TBL_T_NOTIFICATIONS_ON_DEMAND notif = new TBL_T_NOTIFICATIONS_ON_DEMAND();
                            notif.body = pushNotif.body;
                            notif.date_create = DateTime.Now;
                            notif.recipient = recipient.device_id;
                            notif.sound = "default";
                            notif.target = pushNotif.target;
                            notif.status = 1;
                            notif.title = pushNotif.title;
                            notif.type = 1;
                            notif.event_id = pushNotif.event_id;

                            mok.TBL_T_NOTIFICATIONS_ON_DEMANDs.InsertOnSubmit(notif);
                            mok.SubmitChanges();
                        }

                        var listPushNotif = mok.TBL_T_NOTIFICATIONS_ON_DEMANDs.Where(t => t.status == 1);

                        foreach (TBL_T_NOTIFICATIONS_ON_DEMAND notiff in listPushNotif)
                        {
                            pushNotif.sendPushNotification(authKey, notiff);
                        }
                    }
                    else
                    {
                        var listRecipients = mok.VW_RECIPIENTs.Where(vwr => vwr.SITE.ToUpper() == pushNotif.site.ToUpper());
                        //mok.VW_RECIPIENTs.Where(vwr => vwr.token == "LD9MQEDsWEqtGQc0JKXO9g==");
                        //mok.VW_RECIPIENTs.Where(vwr => vwr.SITE.ToUpper() == pushNotif.site.ToUpper());

                        foreach (var recipient in listRecipients)
                        {
                            TBL_T_NOTIFICATIONS_ON_DEMAND notif = new TBL_T_NOTIFICATIONS_ON_DEMAND();
                            notif.body = pushNotif.body;
                            notif.date_create = DateTime.Now;
                            notif.recipient = recipient.device_id;
                            notif.sound = "default";
                            notif.target = pushNotif.target;
                            notif.status = 1;
                            notif.title = pushNotif.title;
                            notif.type = 1;
                            notif.event_id = pushNotif.event_id;

                            mok.TBL_T_NOTIFICATIONS_ON_DEMANDs.InsertOnSubmit(notif);
                            mok.SubmitChanges();
                        }

                        var listPushNotif = mok.TBL_T_NOTIFICATIONS_ON_DEMANDs.Where(t => t.status == 1);

                        foreach (TBL_T_NOTIFICATIONS_ON_DEMAND notiff in listPushNotif)
                        {
                            pushNotif.sendPushNotification(authKey, notiff);
                        }
                    }
                }
                else if (pushNotif.notifType == "NRP")
                {
                    //for type nrp
                    foreach (var nrp in pushNotif.nrp)
                    {
                        var specificPerson = mok.VW_RECIPIENTs.Where(x => x.username.ToUpper() == nrp.ToUpper());

                        foreach (var person in specificPerson)
                        {
                            TBL_T_NOTIFICATIONS_ON_DEMAND notif = new TBL_T_NOTIFICATIONS_ON_DEMAND();
                            notif.body = pushNotif.body;
                            notif.date_create = DateTime.Now;
                            notif.recipient = person.device_id;
                            notif.sound = "default";
                            notif.target = pushNotif.target;
                            notif.status = 1;
                            notif.title = pushNotif.title;
                            notif.type = 1;
                            notif.event_id = pushNotif.event_id;

                            mok.TBL_T_NOTIFICATIONS_ON_DEMANDs.InsertOnSubmit(notif);
                            mok.SubmitChanges();
                        }
                    }

                    var listPushNotif = mok.TBL_T_NOTIFICATIONS_ON_DEMANDs.Where(t => t.status == 1 && t.title == pushNotif.title);

                    foreach (TBL_T_NOTIFICATIONS_ON_DEMAND notiff in listPushNotif)
                    {
                        pushNotif.sendPushNotification(authKey, notiff);
                    }
                }

                return Json(new { status = true, remarks = "Berhasil insert data.." }, JsonRequestBehavior.AllowGet);
                //return Json(new { status = true, Data = i_obj_provinsi, Total = i_obj_provinsi.Count() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return this.Json(new { status = false, remarks = e.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}