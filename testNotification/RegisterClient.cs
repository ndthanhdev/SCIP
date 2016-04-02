using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace testNotification
{
    public class RegisterClient
    {
        private string POST_URL;

        private class DeviceRegistration
        {
            public string Platform { get; set; }
            public string Handle { get; set; }
            public string[] Tags { get; set; }
        }

        public RegisterClient(string backendEndpoint)
        {
            POST_URL = backendEndpoint + "/api/register";
        }

        public async Task RegisterAsync(string handle, IEnumerable<string> tags)
        {
            var regId = await RetrieveRegistrationIdOrRequestNewOneAsync();

            var deviceRegistration = new DeviceRegistration
            {
                Platform = "wns",
                Handle = handle,
                Tags = tags.ToArray()
            };

            var statusCode = await UpdateRegistrationAsync(regId, deviceRegistration);

            if (statusCode == HttpStatusCode.Gone)
            {
                // regId is expired, deleting from local storage & recreating
                var settings = ApplicationData.Current.LocalSettings.Values;
                settings.Remove("__NHRegistrationId");
                regId = await RetrieveRegistrationIdOrRequestNewOneAsync();
                statusCode = await UpdateRegistrationAsync(regId, deviceRegistration);
            }

            if (statusCode != HttpStatusCode.Accepted)
            {
                // log or throw
                throw new System.Net.WebException(statusCode.ToString());
            }
        }

        private async Task<HttpStatusCode> UpdateRegistrationAsync(string regId, DeviceRegistration deviceRegistration)
        {
            
            var settings = ApplicationData.Current.LocalSettings.Values;
            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Authorization", "Basic " + (string)settings["AuthenticationToken"]);

                
          //  var putUri = POST_URL + "/" + regId;

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("id", regId);
            //param.Add("Platform", deviceRegistration.Platform);
            //param.Add("Handle", deviceRegistration.Handle);
            //param.Add("Tags", deviceRegistration.Tags);
            // param.Add("deviceUpdate", JsonConvert.SerializeObject(deviceRegistration));
            var json = JsonConvert.SerializeObject(deviceRegistration);
            var response= await App.MobileService.InvokeApiAsync("register",new StringContent(json, Encoding.UTF8, "application/json"), HttpMethod.Put,header,param);
            return response.StatusCode;
            
        }

        private async Task<string> RetrieveRegistrationIdOrRequestNewOneAsync()
        {
            var settings = ApplicationData.Current.LocalSettings.Values;
            if (!settings.ContainsKey("__NHRegistrationId"))
            {
                Dictionary<string, string> header = new Dictionary<string, string>();
                header.Add("Authorization", "Basic " + (string)settings["AuthenticationToken"]);

                var response = await App.MobileService.InvokeApiAsync("register", null, HttpMethod.Post, header, null);

                if (response.IsSuccessStatusCode)
                {
                    string regId = await response.Content.ReadAsStringAsync();
                    regId = regId.Substring(1, regId.Length - 2);
                    settings.Add("__NHRegistrationId", regId);
                }
                else
                {
                    throw new System.Net.WebException(response.StatusCode.ToString());
                }
            }
            return (string)settings["__NHRegistrationId"];

        }
    }
}
