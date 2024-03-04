using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace CPMOK.Models
{
    public class ClsPushNotifOnDemand
    {
        public string title { get; set; }
        public string body { get; set; }
        public string target { get; set; }
        public string site { get; set; }
        public string event_id { get; set; }
        public List<string> nrp { get; set; }
        public string notifType { get; set; }

        public class FCMResponse
        {
            public long multicast_id { get; set; }
            public int success { get; set; }
            public int failure { get; set; }
            public int canonical_ids { get; set; }
            public List<FCMResult> results { get; set; }
        }

        public class FCMResult
        {
            public string message_id { get; set; }
        }

        public void updateStatusAfterSendNotification(int idNotif, bool isOK, string keterangan)
        {
            try
            {
                DB_MOKDataContext mok = new DB_MOKDataContext();

                var query = mok.TBL_T_NOTIFICATIONS_ON_DEMANDs.Where(ttn => ttn.id == idNotif).FirstOrDefault();
                query.date_sent = DateTime.Now;
                query.remarks = keterangan;

                if (isOK)
                {
                    query.status = 2;
                }
                else
                {
                    query.status = 3;
                }

                mok.SubmitChanges();
            }
            catch (Exception e)
            {

            }
        }

        public void sendPushNotification(string authKey, TBL_T_NOTIFICATIONS_ON_DEMAND pushNotif)
        {
            try
            {
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                //inisialisasi awal penampung
                var objNotification = new
                {
                    to = pushNotif.recipient,//notification.DeviceToken,
                    notification = new
                    {
                        title = pushNotif.title,
                        sound = pushNotif.sound,
                        body = pushNotif.body
                    },
                    //priority = "high",
                    data = new
                    {
                        target = pushNotif.target,
                        id = pushNotif.event_id
                    }
                };

                string jsonNotificationFormat = Newtonsoft.Json.JsonConvert.SerializeObject(objNotification);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonNotificationFormat);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", authKey));//serverKey));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", ""));//senderId));
                tRequest.ContentLength = byteArray.Length;
                tRequest.ContentType = "application/json";

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String responseFromFirebaseServer = tReader.ReadToEnd();

                                FCMResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<FCMResponse>(responseFromFirebaseServer);
                                if (response.success == 1)
                                {
                                    foreach (var result in response.results)
                                    {
                                        updateStatusAfterSendNotification(pushNotif.id, true, result.message_id);
                                    }
                                    //new NotificationBLL().InsertNotificationLog(dayNumber, notification, true);
                                }
                                else if (response.failure == 1)
                                {
                                    foreach (var result in response.results)
                                    {
                                        updateStatusAfterSendNotification(pushNotif.id, false, result.message_id);
                                    }
                                    //updateStatusAfterSendNotification(pushNotif.id, false, response.results.ToString());
                                    //new NotificationBLL().InsertNotificationLog(dayNumber, notification, false);
                                    //sbLogger.AppendLine(string.Format("Error sent from FCM server, after sending request : {0} , for following device info: {1}", responseFromFirebaseServer, jsonNotificationFormat));
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}