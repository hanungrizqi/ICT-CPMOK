using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CPMOK.Models
{
    public class District
    {
        private readonly DB_MOKDataContext db;

        public District()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
            db = new DB_MOKDataContext(connectionString);
        }

        public List<VW_DISTRIK> GET()
        {
            try
            {
                var res = db.VW_DISTRIKs.OrderBy(item => item.DSTRCT_CODE).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<EmployeeInfo> GETNRP()
        {
            try
            {
                var result = db.TBL_M_KARYAWANs
                    .OrderBy(item => item.EMPLOYEE_ID)
                    .Select(item => new EmployeeInfo
                    {
                        EMPLOYEE_ID = item.EMPLOYEE_ID,
                        NAME = item.NAME
                    })
                    .ToList();

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public class EmployeeInfo
        {
            public string EMPLOYEE_ID { get; set; }
            public string NAME { get; set; }
        }

    }
}