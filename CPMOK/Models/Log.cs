using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CPMOK.Models
{
    public class Log
    {
        DB_MOKDataContext db = new DB_MOKDataContext();

        //public Log()
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
        //    db = new DB_MOKDataContext(connectionString);
        //}

        public Log()
        {
            data = new List<object>();
        }

        public long recordsTotal { get; set; }

        public long recordsFiltered { get; set; }

        public object data { get; set; }


        public List<VW_LOG> GET()
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                var res = db.VW_LOGs.Where(item => item.insertDate.HasValue && item.insertDate.Value.Year == currentYear).OrderByDescending(item => item.headerID).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}