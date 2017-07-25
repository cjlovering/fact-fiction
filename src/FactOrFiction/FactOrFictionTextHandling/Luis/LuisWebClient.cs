using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FactOrFictionTextHandling.Luis
{
    public class LuisWebClient : WebClient
    {
        //public string BaseUri { get; private set; }
        ////https://eastus2.api.cognitive.microsoft.com/luis/v2.0/apps/79af6370-41bd-4d03-9c7c-5f234eb6049c?subscription-key=3a134d05a51641a18fecd02f9cf4bfd7&timezoneOffset=0&verbose=true&q=

        //LuisClient(string baseUri)
        //{
        //    BaseUri = baseUri;
        //}
        public LuisWebClient()
        {
            
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            HttpWebRequest httpRequest = request as HttpWebRequest;
            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse resp = null;
            try
            {
                resp = base.GetWebResponse(request);
            }
            catch (WebException ex)
            {
                var httpWebResponseObj = (ex.Response as HttpWebResponse);
                if (httpWebResponseObj != null && httpWebResponseObj.StatusCode == HttpStatusCode.InternalServerError)
                {
                    string exceptionMessageText = ex.InnerException != null ? ex.InnerException.Message + httpWebResponseObj.StatusDescription : httpWebResponseObj.StatusDescription;
                    throw new WebException(exceptionMessageText, ex);
                }
            }
            return resp;
        }
    }
}
