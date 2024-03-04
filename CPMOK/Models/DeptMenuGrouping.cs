using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CPMOK.Models
{
    public class DeptMenuGrouping
    {
        private readonly DB_MOKDataContext db;

        public DeptMenuGrouping()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
            db = new DB_MOKDataContext(connectionString);
        }

        public List<TBL_M_MENU_GROUPING> GET()
        {
            try
            {
                var res = db.TBL_M_MENU_GROUPINGs.OrderBy(item => item.NAMA_GROUP).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}