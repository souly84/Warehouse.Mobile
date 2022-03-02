using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Util;
using WebRequest.Elegant;
using WebRequest.Elegant.Core;

namespace Warehouse.Droid
{
    public class LoggedWebRequest : IWebRequest
    {
        private readonly IWebRequest _origin;

        public LoggedWebRequest(IWebRequest origin)
        {
            _origin = origin;
        }

        public IUri Uri => _origin.Uri;

        public IWebRequest WithBody(IBodyContent postBody)
        {
            return new LoggedWebRequest(_origin.WithBody(postBody));
        }

        public async Task<HttpResponseMessage> GetResponseAsync()
        {
            try
            {
                Log.Info("Warehouse.Mobile", _origin.ToString());
                var response = await _origin.GetResponseAsync();
                _=Task.Run(async () =>
                {
                    try
                    {
                        Log.Info("Warehouse.Mobile", await response.Content.ReadAsStringAsync());
                    }
                    catch (System.Exception ex)
                    {
                        //DO NOTHING
                    }
                }); 
                return response;
            }
            catch (System.Exception ex)
            {
                Log.Error("Warehouse.Mobile", ex.Message);
                throw;
            }
        }

        public IWebRequest WithMethod(HttpMethod method)
        {
            return new LoggedWebRequest(
                _origin.WithMethod(method)
            );
        }

        public IWebRequest WithPath(IUri uri)
        {
            return new LoggedWebRequest(
                _origin.WithPath(uri)
            );
        }

        public IWebRequest WithQueryParams(Dictionary<string, string> parameters)
        {
            return new LoggedWebRequest(
                _origin.WithQueryParams(parameters)
            );
        }
    }
}
