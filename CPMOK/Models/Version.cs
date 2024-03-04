using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CPMOK.Models
{
    public class Version
    {
        private readonly DB_MOKDataContext db;

        public Version()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
            db = new DB_MOKDataContext(connectionString);
        }

        public string id { get; set; }
        public string os_name { get; set; }
        public string app_version { get; set; }
        public DateTime release_date { get; set; }

        public List<TBL_M_APP_VERSION> GET()
        {
            try
            {
                var res = db.TBL_M_APP_VERSIONs.OrderByDescending(item => item.os_name).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TBL_M_APP_VERSION CREATE()
        {
            try
            {
                var insert = new TBL_M_APP_VERSION();
                insert.os_name = os_name;
                insert.app_version = app_version;
                insert.release_date = release_date;
                
                db.TBL_M_APP_VERSIONs.InsertOnSubmit(insert);
                db.SubmitChanges();

                var res = db.TBL_M_APP_VERSIONs.FirstOrDefault(item => item.os_name == os_name);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TBL_M_APP_VERSION Update()
        {
            try
            {
                var menu = db.TBL_M_APP_VERSIONs.FirstOrDefault(item => item.os_name.ToString() == id);

                menu.os_name = os_name;
                menu.app_version = app_version;
                if (menu.release_date != null)
                {
                    var originalDate = menu.release_date.Value;
                    menu.release_date = originalDate.Add(DateTime.Now.TimeOfDay);
                }
                else
                {
                    menu.release_date = DateTime.Now;
                }

                db.SubmitChanges();

                var res = db.TBL_M_APP_VERSIONs.Where(item => item.os_name.ToString() == os_name).FirstOrDefault();

                return res;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public TBL_M_APP_VERSION Delete()
        {
            try
            {
                var menu = db.TBL_M_APP_VERSIONs.FirstOrDefault(item => item.os_name.ToString() == os_name);

                if (menu == null)
                {
                    throw new Exception($"Data tidak ditemukan!");
                }

                db.TBL_M_APP_VERSIONs.DeleteOnSubmit(menu);

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