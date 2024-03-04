using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CPMOK.Models
{
    public class ACLMenuGrouping
    {
        private readonly DB_MOKDataContext db;

        public ACLMenuGrouping()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
            db = new DB_MOKDataContext(connectionString);
        }

        public List<TBL_M_GROUP_PERMISION> GET()
        {
            try
            {
                var res = db.TBL_M_GROUP_PERMISIONs.OrderBy(item => item.GROUP_NAME).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}