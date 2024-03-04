using CPMOK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace CPMOK.Controllers
{
    public class LogController : Controller
    {
        DB_MOKDataContext mok = new DB_MOKDataContext();
        // GET: Log
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

        public ActionResult GetLog(CPMOK.Models.Log version)
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
        public JsonResult ServerSideData(int Length,
                                        int Start,
                                        string Search,
                                        string OrderType,
                                        int OrderCol,
                                        int Draw)
        {
            string[] orderMapping = new string[] { "headerID", "username", "os_name", "AppVersion", "actionAPI", "insertDate" };
            var orderBy = orderMapping[OrderCol];

            var baseQuery = mok.VW_LOGs.AsQueryable();

            // Search Section
            var filteredQuery = baseQuery.Where(x => x.username.Contains(Search));
            var orderedQuery = filteredQuery;

            // Order Section
            switch (OrderCol)
            {
                case 1:
                    if (OrderType == "asc")
                    {
                        orderedQuery = orderedQuery.OrderBy(x => x.headerID);
                    }
                    else
                    {
                        orderedQuery = orderedQuery.OrderByDescending(x => x.headerID);
                    }
                    break;
                case 2:
                    if (OrderType == "asc")
                    {
                        orderedQuery = orderedQuery.OrderBy(x => x.username);
                    }
                    else
                    {
                        orderedQuery = orderedQuery.OrderByDescending(x => x.username);
                    }
                    break;
                case 3:
                    if (OrderType == "asc")
                    {
                        orderedQuery = orderedQuery.OrderBy(x => x.os_name);
                    }
                    else
                    {
                        orderedQuery = orderedQuery.OrderByDescending(x => x.os_name);
                    }
                    break;
                case 4:
                    if (OrderType == "asc")
                    {
                        orderedQuery = orderedQuery.OrderBy(x => x.AppVersion);
                    }
                    else
                    {
                        orderedQuery = orderedQuery.OrderByDescending(x => x.AppVersion);
                    }
                    break;
                case 5:
                    if (OrderType == "asc")
                    {
                        orderedQuery = orderedQuery.OrderBy(x => x.actionAPI);
                    }
                    else
                    {
                        orderedQuery = orderedQuery.OrderByDescending(x => x.actionAPI);
                    }
                    break;
                case 6:
                    if (OrderType == "asc")
                    {
                        orderedQuery = orderedQuery.OrderBy(x => x.insertDate);
                    }
                    else
                    {
                        orderedQuery = orderedQuery.OrderByDescending(x => x.insertDate);
                    }
                    break;
                default:
                    break;
            }

            var listData = orderedQuery.Skip(Start).Take(Length).ToList();

            var recordsTotal = mok.ExecuteQuery<int>("SELECT COUNT (username) FROM VW_LOGS").FirstOrDefault();

            //Log response = new Log()
            //{
            //    data = listData,
            //    recordsTotal =  /*baseQuery.Count()*/,
            //    recordsFiltered = 10 /*filteredQuery.Count()*/
            //};

            return Json(new { status = true, remarks = "Data berhasil diambil", draw = Draw, data = listData, recordsTotal = recordsTotal }, JsonRequestBehavior.AllowGet);
            //return Json(new { status = true, remarks = "Data berhasil diambil", data = response }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetLog()
        //{
        //    try
        //    {
        //        int draw = int.Parse(Request["draw"]);
        //        int start = int.Parse(Request["start"]);
        //        int length = int.Parse(Request["length"]);
        //        string searchValue = Request["search[value]"];
        //        string orderColumn = Request["order[0][column]"];
        //        string orderDir = Request["order[0][dir]"];

        //        var version = new CPMOK.Models.Log(); // Create an instance if necessary
        //        var result = version.GET(start, length, searchValue, orderColumn, orderDir);

        //        Response.StatusCode = 200;
        //        return Json(new { draw = draw, recordsTotal = result.TotalRecords, recordsFiltered = result.TotalRecords, data = result.Data }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new { Status = false, Message = e.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

    }
}