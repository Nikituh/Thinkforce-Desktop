using System;
using System.IO;
using System.Json;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace thinkforce_desktop.Networking.Wazombi
{
    static class WazombiNetworking
    {
        #region conf

        const bool shouldWriteLog = true;
        const int TIMEOUT_IN_MILLISECONDS = 10 * 1000;

        #endregion

        const string GET = "GET";
        const string POST = "POST";
        const string PUT = "PUT";
        const string DELETE = "DELETE";

        public const int ErrorCreatingStream = -6;
        public const int ErrorWritingStream = -7;
        public const int ErrorGettingResponse = -8;
        public const int ErrorParsingResponse = -9;
        public const int RequestCancelled = -10;

        static TimeSpan timeoutSpan = new TimeSpan(0, 0, 0, 0, TIMEOUT_IN_MILLISECONDS);

        static CancellationTokenSource cts = new CancellationTokenSource();
        public static CancellationToken DefaultToken { get { return cts.Token; } }

        public static void CancelDefaultToken()
        {
            CancellationTokenSource oldCts = cts;
            cts = new CancellationTokenSource();
            oldCts.Cancel();
            oldCts.Dispose();
        }

        public async static Task<WazombiNetworkingResponse> Get(string url, CancellationToken token = default(CancellationToken))
        {
            var requestInfo = new WazombiNetworkingRequest
            {
                Method = GET,
                Url = url,
                CancellationToken = token
            };

            return await SendData(requestInfo);
        }

        public async static Task<WazombiNetworkingResponse> Get(WazombiNetworkingRequest requestInfo)
        {
            requestInfo.Method = GET;
            return await SendData(requestInfo);
        }

        public async static Task<WazombiNetworkingResponse> Post(string url, JsonValue json, CancellationToken token = default(CancellationToken))
        {
            var requestInfo = new WazombiNetworkingRequest
            {
                Method = POST,
                Url = url,
                Json = json,
                CancellationToken = token
            };
            return await SendData(requestInfo);
        }

        public async static Task<WazombiNetworkingResponse> Post(WazombiNetworkingRequest requestInfo)
        {
            requestInfo.Method = POST;
            return await SendData(requestInfo);
        }

        public async static Task<WazombiNetworkingResponse> Delete(string url, CancellationToken token = default(CancellationToken))
        {
            var requestInfo = new WazombiNetworkingRequest
            {
                Method = DELETE,
                Url = url,
                CancellationToken = token
            };
            return await SendData(requestInfo);
        }

        public async static Task<WazombiNetworkingResponse> Delete(WazombiNetworkingRequest requestInfo)
        {
            requestInfo.Method = DELETE;
            return await SendData(requestInfo);
        }

        public async static Task<WazombiNetworkingResponse> Put(string url, JsonValue json, CancellationToken token = default(CancellationToken))
        {
            var requestInfo = new WazombiNetworkingRequest
            {
                Method = PUT,
                Url = url,
                Json = json,
                CancellationToken = token
            };
            return await SendData(requestInfo);
        }

        public async static Task<WazombiNetworkingResponse> Put(WazombiNetworkingRequest requestInfo)
        {
            requestInfo.Method = PUT;
            return await SendData(requestInfo);
        }

        public async static Task<WazombiNetworkingResponse> SendData(WazombiNetworkingRequest requestInfo)
        {
            LogNetworking("WazombiNetworking SendData: " + requestInfo.Url + " - " + requestInfo.Method);

            string url = requestInfo.Url;
            Uri uri = new Uri(url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);

            byte[] byteArray = requestInfo.HasJson ? Encoding.UTF8.GetBytes(requestInfo.Json.ToString()) : new byte[0];

            request.ContentLength = byteArray.Length;
            request.Method = requestInfo.Method;
            request.Timeout = TIMEOUT_IN_MILLISECONDS;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (requestInfo.HasHeaders)
            {
                request.Headers = requestInfo.Headers;
            }

            if (requestInfo.HasCookies)
            {
                CookieContainer cookieContainer = new CookieContainer();
                foreach (Cookie cookie in requestInfo.Cookies)
                {
                    cookie.Domain = uri.Host;
                    cookieContainer.Add(cookie);
                }
                request.CookieContainer = cookieContainer;
            }
            else
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(uri, new CookieCollection());
            }

            WazombiNetworkingResponse result = new WazombiNetworkingResponse();

            if (request.Method == POST || request.Method == PUT)
            {
                if (requestInfo.HasJson)
                {
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
                }
                request.AllowAutoRedirect = false;

                Stream dataStream;

                try
                {
                    dataStream = await request.GetRequestStreamAsync().WithTimeout(TIMEOUT_IN_MILLISECONDS);
                    if (requestInfo.CancellationToken.IsCancellationRequested) return result.WithException(RequestCancelled);
                }
                catch (Exception e)
                {
                    LogNetworking("WazombiNetworking SendData exception at request.GetRequestStreamAsync: " + e);
                    return result.WithException(ErrorCreatingStream);
                }

                try
                {
                    if (requestInfo.HasJson)
                    {
                        await dataStream.WriteAsync(byteArray, 0, byteArray.Length);
                        if (requestInfo.CancellationToken.IsCancellationRequested) return result.WithException(RequestCancelled);
                    }
                }
                catch (Exception e)
                {
                    LogNetworking("WazombiNetworking SendData exception at dataStream.WriteAsync: " + e);
                    return result.WithException(ErrorWritingStream);
                }
                finally
                {
                    if (dataStream != null)
                    {
                        dataStream.Dispose();
                    }
                }
            }

            try
            {
                using (HttpWebResponse response = await request.GetResponseAsync(timeoutSpan, requestInfo.CancellationToken))
                {
                    if (requestInfo.CancellationToken.IsCancellationRequested) return result.WithException(RequestCancelled);

                    if (response == null)
                    {
                        LogNetworking("WazombiNetworking SendData response == null");
                        return result.WithException(ErrorGettingResponse);
                    }

                    Stream stream = response.GetResponseStream();
                    string returnValue = StreamToString(stream);

                    if (string.IsNullOrEmpty(returnValue))
                    {
                        return result.WithStatus(response.StatusCode).WithCookies(response.Cookies);
                    }

                    try
                    {
                        JsonValue parsedJson = JsonValue.Parse(returnValue);
                        return result.WithJson(parsedJson).WithStatus(response.StatusCode).WithCookies(response.Cookies);
                    }
                    catch (Exception e)
                    {
                        LogNetworking("WazombiNetworking SendData exception at JsonValue.Parse (returnValue): " + e);
                        return result.WithException(ErrorParsingResponse);
                    }
                }
            }
            catch (Exception e)
            {
                LogNetworking("WazombiNetworking SendData exception at request.GetResponseAsync: " + e);
                return result.WithException(ErrorGettingResponse);
            }
        }

        public static string StreamToString(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        static void LogNetworking(string str)
        {
            if (shouldWriteLog)
            {
                Console.WriteLine(str);
            }
        }

    }

}
