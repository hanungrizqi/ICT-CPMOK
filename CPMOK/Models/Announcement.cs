using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace CPMOK.Models
{
    public class Announcement
    {
        private readonly DB_MOKDataContext db;

        public Announcement()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
            db = new DB_MOKDataContext(connectionString);
        }

        public string id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int order { get; set; }
        public string imageUrl { get; set; }
        public string targetUrl { get; set; }
        public string type { get; set; }
        public int status { get; set; }
        public string location { get; set; }



        public List<TBL_T_ANNOUNCEMENT> GET()
        {
            try
            {
                var res = db.TBL_T_ANNOUNCEMENTs.OrderBy(item => item.order).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TBL_T_ANNOUNCEMENT CREATE()
        {
            try
            {
                var res = new TBL_T_ANNOUNCEMENT();

                string pathDownload = ConfigurationManager.AppSettings["pathDownloadIcon"].ToString();
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["pathUploadIcon"].ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string imageName = Helpers.GenerateUniqueString() + ".png";
                string imgPath = Path.Combine(path, imageName);

                byte[] imageBytes = Convert.FromBase64String(imageUrl);
                File.WriteAllBytes(imgPath, imageBytes);

                Image imgSource = Image.FromFile(imgPath, true);
                Image imgPhoto = null;
                int quality = 50;

                imgPhoto = Helpers.ScaleByPercent(imgSource, quality);
                imgSource.Dispose();
                imgPhoto.Save(imgPath, ImageFormat.Png);
                imgPhoto.Dispose();

                Guid uuid = Guid.NewGuid();

                res.id = uuid;
                res.title = title;
                res.body = body;
                res.order = order;
                res.imageUrl = imageUrl;
                res.targetUrl = targetUrl;
                res.type = type;
                res.status = status;
                res.location = location;

                db.TBL_T_ANNOUNCEMENTs.InsertOnSubmit(res);
                db.SubmitChanges();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public TBL_T_ANNOUNCEMENT Update()
        {
            try
            {
                //var menu = db.TBL_R_MENUs.FirstOrDefault(item => item.MENU_PID.ToString() == MENU_PID);

                //if (menu == null)
                //{
                //    throw new Exception($"Data tidak ditemukan!");
                //}

                //if (menu.MENU_ICON != "")
                //{
                //    // edit icon menu
                //}

                //menu.MENU_DESC = MENU_DESC;
                //menu.MENU_LINK = MENU_LINK;
                //menu.MENU_LINK_IOS = MENU_LINK_IOS;
                //menu.DISTRICT = DISTRICT;
                //menu.IS_DEFAULT = IS_DEFAULT;
                //menu.IS_SSO = IS_SSO;
                //menu.androidAppId = androidAppId;
                //menu.iosAppId = iosAppId;
                //menu.downloadUrl = downloadUrl;
                //menu.GROUP_ID = GROUP_ID;
                //menu.GROUPING = GROUPING;
                //menu.IS_AVAILABLE = IS_AVAILABLE;

                //db.SubmitChanges();

                var res = db.TBL_T_ANNOUNCEMENTs.Where(item => item.id.ToString() == id).FirstOrDefault();

                return res;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public TBL_T_ANNOUNCEMENT Delete()
        {
            try
            {
                var announcement = db.TBL_T_ANNOUNCEMENTs.FirstOrDefault(item => item.id.ToString() == id);

                if (announcement == null)
                {
                    throw new Exception($"Data tidak ditemukan!");
                }

                db.TBL_T_ANNOUNCEMENTs.DeleteOnSubmit(announcement);

                db.SubmitChanges();

                return announcement;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}