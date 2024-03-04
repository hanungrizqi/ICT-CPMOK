using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace CPMOK.Models
{
    public class Login
    {
        private readonly DB_MOKDataContext db;

        public Login()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB_ICT_mOK_KPDConnectionString"].ConnectionString;
            db = new DB_MOKDataContext(connectionString);
        }

        public string username { get; set; }
        public string password { get; set; }


        public GetUser IsLogin()
        {
            bool status = false;
            bool status_login = false;
            string nrp = "";
            var data_user = new VW_KARYAWAN();
            var res = new GetUser();


            if (username.Count() > 7)
            {
                nrp = username.Substring(username.Length - 7);
            }
            else
            {
                nrp = username;
            }


            status_login = CheckValidLogin();

            if (status_login == false)
            {
                status_login = OpenLdap(username, password);
            }

            if (status_login == true)
            {

                data_user = db.VW_KARYAWANs.Where(x => x.EMPLOYEE_ID == nrp).SingleOrDefault();
                if (data_user != null)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }
            }


            res.status = status;
            res.user = data_user;

            return res;
        }


        public bool CheckValidLogin()
        {

            try
            {
                bool stat = false;
                //var ldap = new LdapAuthentication("LDAP://KPPMINING:389");
                //stat = ldap.IsAuthenticated("KPPMINING", username, password);

                PrincipalContext i_cls_principalcontext = new PrincipalContext(ContextType.Domain, "KPPMINING.NET");
                stat = i_cls_principalcontext.ValidateCredentials(username, password);


                //stat = true;
                return stat;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public bool OpenLdap(string username = "", string password = "")
        {
            String uid = "cn=" + username + ",ou=Users,dc=kpp,dc=net";

            DirectoryEntry root = new DirectoryEntry("LDAP://10.12.101.102", uid, password, AuthenticationTypes.None);

            try
            {
                object connected = root.NativeObject;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }

    public class GetUser
    {
        public bool status { get; set; }
        public VW_KARYAWAN user { get; set; }
    }
}