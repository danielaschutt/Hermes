using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Hermes.Models;
using Newtonsoft.Json;

namespace Hermes.Helpers
{
    public class SendPushNotifications
    {
        private static Uri FireBasePushNotificationsURL = new Uri("https://fcm.googleapis.com/fcm/send");

        private static string ServerKey = Environment.GetEnvironmentVariable("FIREBASE_KEY");
        
        public static async Task<bool> send(string token, Notification notification, AlertPayload data)
        {
            
            bool sent = false;

            if (!string.IsNullOrEmpty(token))
            {
                var messageInformation = new Message
                {
                    notification = notification,
                    data = data,
                    to = token
                };

                string jsonMessage = JsonConvert.SerializeObject(messageInformation);

                var request = new HttpRequestMessage(HttpMethod.Post, FireBasePushNotificationsURL);
                request.Headers.TryAddWithoutValidation("Authorization", "key=" + ServerKey);
                request.Content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
                HttpResponseMessage result;

                using (var client = new HttpClient())
                {
                    result = await client.SendAsync(request);

                    var a = result;
                    sent = sent && result.IsSuccessStatusCode;
                }
            }

            return sent;
        }
    }
}