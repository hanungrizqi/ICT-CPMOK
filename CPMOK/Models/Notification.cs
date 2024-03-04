using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace CPMOK.Models
{
    public class Notification
    {
        private readonly DB_MOKDataContext db;

        public Notification()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
            db = new DB_MOKDataContext(connectionString);
        }

        public int PUSHNRP(string nrp, string title, string body, string target, string eventid)
        {
            try
            {
                var result = db.cusp_insertDataPushNotifByNRP(nrp, title, body, target, eventid);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}