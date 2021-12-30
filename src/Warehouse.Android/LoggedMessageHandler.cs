using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Android.Util;

namespace Warehouse.Droid
{
    public class LoggedMessageHandler : DelegatingHandler
    {
        public LoggedMessageHandler() : this(new HttpClientHandler())
        {
        }
        public LoggedMessageHandler(HttpMessageHandler httpMessageHandler)
        {
            InnerHandler = httpMessageHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var postContent = string.Empty;
            if (request.Content != null)
            {
                postContent = await request.Content
                    .ReadAsStringAsync()
                    .ConfigureAwait(false);
            }

            Log.Debug("WebRequest: Uri => ", request.RequestUri.ToString());
            Log.Debug("WebRequest: PostContent => ", postContent);
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response.Content != null)
            {
                Log.Debug("WebRequest: ResponseContent =>", await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
            return response;
        }
    }
}
