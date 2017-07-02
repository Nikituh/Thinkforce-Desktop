
using System.Json;
using System.Net;

namespace thinkforce_desktop.Networking.Wazombi
{
    class WazombiNetworkingResponse
    {
        public JsonValue Json { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public CookieCollection Cookies { get; set; }

        public int Exception { get; set; }

        public bool WasCancelled { get { return Exception == WazombiNetworking.RequestCancelled; } }

        public bool HasCookies { get { return Cookies != null && Cookies.Count > 0; } }

        public bool HasException { get { return Exception != 0; } }

        public bool HasJson { get { return Json != null; } }

        public string Message { get; set; }

    }
}
