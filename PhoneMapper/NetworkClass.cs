using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace PhoneMapper
{
    public static class NetworkClass
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async static void DownloadFile(string dl_link, IFileDownloads fileDownloads)
        {

            try
            {
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(dl_link); //code won't pass this line if no internet
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    HttpContent httpContent = httpResponseMessage.Content;
                    string contents = await httpContent.ReadAsStringAsync();
                    fileDownloads.FileDownloaded(contents);
                }
                else
                {
                    fileDownloads.FileNotDownloaded();
                }

            }
            catch (HttpRequestException webex)
            {
                fileDownloads.NoInternet();
            }
        }

        //reverse Geocoding (latlong to address name) using OSM nominatim API
        //DEPRECATED. Too slow. Takes anywhere from a few hundred milliseconds to a few seconds for each request. Unacceptable
        public static string GetAddressOSM(string lat, string lon)
        {
            string requestURL = ConstantsClass.OSMReverseAddressLookupURL;
            string adjustedRequestURL = requestURL.Replace("<ph1>", lat).Replace("<ph2>", lon);
            string address = "";

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Host = "nominatim.openstreetmap.org";
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.128 Safari/537.36");
            var httpResponseMessageTask = httpClient.GetAsync(adjustedRequestURL); //code will throw exception here if no internet
            HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                HttpContent httpContent = httpResponseMessage.Content;
                string content = httpContent.ReadAsStringAsync().Result;
                JObject jObject = JObject.Parse(content);
                address = (string)jObject["display_name"];

            }

            return address;
        }

        //reverse Geocoding (latlong to address name) using HERE API... ref: developer.here.com
        public static string GetAddressHERE(string lat, string lon, string apiKey)
        {
            string requestURL = ConstantsClass.HereApiReverseAddressLookupURL;
            string adjustedRequestURL = requestURL.Replace("<ph1>", lat).Replace("<ph2>", lon).Replace("<ph3>", apiKey);
            string address = "";

            httpClient.DefaultRequestHeaders.Clear();
            var httpResponseMessageTask = httpClient.GetAsync(adjustedRequestURL);
            HttpResponseMessage httpResponseMessage = httpResponseMessageTask.Result;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                HttpContent httpContent = httpResponseMessage.Content;
                string content = httpContent.ReadAsStringAsync().Result;
                JObject jObject = JObject.Parse(content);
                address = (string)jObject["items"][0]["address"]["label"];
            }

            return address;
        }


    }
}
