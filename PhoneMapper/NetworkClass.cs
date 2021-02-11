using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Windows.Forms;
using System.Net;

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



    }
}
