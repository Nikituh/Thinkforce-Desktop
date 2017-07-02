
using System.Json;
using System.Net;
using System.Threading;

namespace thinkforce_desktop.Networking.Wazombi
{
    class WazombiNetworkingRequest
    {
        public string Url { get; set; }

        public string Method { get; set; }

        public JsonValue Json { get; set; }

        public WebHeaderCollection Headers { get; set; }

        public CookieCollection Cookies { get; set; }

        public bool HasJson
        {
            get { return Json != null; }
        }

        public bool HasHeaders
        {
            get { return Headers != null && Headers.Count > 0; }
        }

        public bool HasCookies
        {
            get { return Cookies != null && Cookies.Count > 0; }
        }

        CancellationToken cancellationToken;

        public CancellationToken CancellationToken
        {
            get
            {
                return (cancellationToken == default(CancellationToken)) ? WazombiNetworking.DefaultToken : cancellationToken;
            }
            set
            {
                cancellationToken = value;
            }
        }

    }
}
