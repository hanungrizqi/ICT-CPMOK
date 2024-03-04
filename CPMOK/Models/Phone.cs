using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CPMOK.Models
{
    public class Phone
    {
        private readonly DB_MOKDataContext db;

        public Phone()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
            db = new DB_MOKDataContext(connectionString);
        }

        public string id { get; set; }
        public string nrp { get; set; }
        public string phone { get; set; }

        public List<TBL_R_USER_PHONE> GET()
        {
            try
            {
                var res = db.TBL_R_USER_PHONEs.OrderBy(item => item.nrp).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TBL_R_USER_PHONE CREATE()
        {
            try
            {
                var insert = new TBL_R_USER_PHONE();
                insert.nrp = nrp;
                insert.phoneNo = phone;

                db.TBL_R_USER_PHONEs.InsertOnSubmit(insert);
                db.SubmitChanges();

                var res = db.TBL_R_USER_PHONEs.FirstOrDefault(item => item.nrp == nrp);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TBL_R_USER_PHONE Update()
        {
            try
            {
                var menu = db.TBL_R_USER_PHONEs.FirstOrDefault(item => item.nrp.ToString() == id);

                menu.nrp = nrp;
                menu.phoneNo = phone;

                db.SubmitChanges();

                var res = db.TBL_R_USER_PHONEs.Where(item => item.nrp.ToString() == nrp).FirstOrDefault();

                return res;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public TBL_R_USER_PHONE Delete()
        {
            try
            {
                var menu = db.TBL_R_USER_PHONEs.FirstOrDefault(item => item.nrp.ToString() == nrp);

                if (menu == null)
                {
                    throw new Exception($"Data tidak ditemukan!");
                }

                db.TBL_R_USER_PHONEs.DeleteOnSubmit(menu);

                db.SubmitChanges();

                return menu;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}