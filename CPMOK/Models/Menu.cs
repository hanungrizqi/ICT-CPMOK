using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;

namespace CPMOK.Models
{
    public class Menu
    {
        private readonly DB_MOKDataContext db;

        public Menu()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
            db = new DB_MOKDataContext(connectionString);
        }

        public string MENU_PID { get; set; }
        public string MENU_DESC { get; set; }
        public string MENU_LINK { get; set; }
        public string MENU_LINK_IOS { get; set; }
        public string MENU_ICON { get; set; }
        public string DISTRICT { get; set; }
        public bool IS_DEFAULT { get; set; }
        public bool IS_SSO { get; set; }
        public string androidAppId { get; set; }
        public string iosAppId { get; set; }
        public string downloadUrl { get; set; }
        public int GROUP_ID { get; set; }
        public string GROUPING { get; set; }
        public bool IS_AVAILABLE { get; set; }

        public List<VW_MENU> GET()
        {
            try
            {
                var res = db.VW_MENUs.OrderByDescending(item => item.MENU_DESC).ToList();

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public VW_MENU CREATE()
        {
            try
            {
                string pathDownload = ConfigurationManager.AppSettings["pathDownloadIcon"].ToString();
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["pathUploadIcon"].ToString());
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string imageName = Helpers.GenerateUniqueString() + ".png";

                string imgPath = Path.Combine(path, imageName);

                byte[] imageBytes = Convert.FromBase64String(MENU_ICON);
                File.WriteAllBytes(imgPath, imageBytes);

                Image imgSource = Image.FromFile(imgPath, true);
                Image imgPhoto = null;
                int quality = 50;

                imgPhoto = Helpers.ScaleByPercent(imgSource, quality);
                imgSource.Dispose();
                imgPhoto.Save(imgPath, ImageFormat.Png);
                imgPhoto.Dispose();

                Guid uuid = Guid.NewGuid();

                var insert = new TBL_R_MENU();
                insert.MENU_PID = uuid;
                insert.MENU_DESC = MENU_DESC;
                insert.MENU_LINK = MENU_LINK;
                insert.MENU_LINK_IOS = MENU_LINK_IOS;
                insert.MENU_ICON = $"{pathDownload}/{imageName}";
                insert.DISTRICT = DISTRICT;
                insert.IS_DEFAULT = IS_DEFAULT;
                insert.IS_SSO = IS_SSO;
                insert.androidAppId = androidAppId;
                insert.iosAppId = iosAppId;
                insert.downloadUrl = downloadUrl;
                insert.GROUP_ID = GROUP_ID;
                insert.GROUPING = GROUPING;
                insert.IS_AVAILABLE = IS_AVAILABLE;

                db.TBL_R_MENUs.InsertOnSubmit(insert);
                db.SubmitChanges();

                var res = db.VW_MENUs.FirstOrDefault(item => item.MENU_PID == uuid);
                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public VW_MENU Update()
        {
            try
            {
                var menu = db.TBL_R_MENUs.FirstOrDefault(item => item.MENU_PID.ToString() == MENU_PID);

                if (menu == null)
                {
                    throw new Exception($"Data tidak ditemukan!");
                }

                if (menu.MENU_ICON != "")
                {
                    // edit icon menu
                    string pathDownload = ConfigurationManager.AppSettings["pathDownloadIcon"].ToString();
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["pathUploadIcon"].ToString());
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string imageName = Helpers.GenerateUniqueString() + ".png";

                    string imgPath = Path.Combine(path, imageName);

                    byte[] imageBytes = Convert.FromBase64String(MENU_ICON);
                    File.WriteAllBytes(imgPath, imageBytes);

                    Image imgSource = Image.FromFile(imgPath, true);
                    Image imgPhoto = null;
                    int quality = 50;

                    imgPhoto = Helpers.ScaleByPercent(imgSource, quality);
                    imgSource.Dispose();
                    imgPhoto.Save(imgPath, ImageFormat.Png);
                    imgPhoto.Dispose();

                    menu.MENU_ICON = $"{pathDownload}/{imageName}";
                } 

                menu.MENU_DESC = MENU_DESC;
                menu.MENU_LINK = MENU_LINK;
                menu.MENU_LINK_IOS = MENU_LINK_IOS;
                menu.DISTRICT = DISTRICT;
                menu.IS_DEFAULT = IS_DEFAULT;
                menu.IS_SSO = IS_SSO;
                menu.androidAppId = androidAppId;
                menu.iosAppId = iosAppId;
                menu.downloadUrl = downloadUrl;
                menu.GROUP_ID = GROUP_ID;
                menu.GROUPING = GROUPING;
                menu.IS_AVAILABLE = IS_AVAILABLE;

                db.SubmitChanges();

                var res = db.VW_MENUs.Where(item => item.MENU_PID.ToString() == MENU_PID).FirstOrDefault();

                return res;
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public TBL_R_MENU Delete()
        {
            try
            {
                var menu = db.TBL_R_MENUs.FirstOrDefault(item => item.MENU_PID.ToString() == MENU_PID);

                if (menu == null)
                {
                    throw new Exception($"Data tidak ditemukan!");
                }

                db.TBL_R_MENUs.DeleteOnSubmit(menu);

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