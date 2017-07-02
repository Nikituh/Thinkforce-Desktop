using System;
using System.Json;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace thinkforce_desktop.Networking.Wazombi
{
    static class WazombiNetworkingExtensions
    {

        public static WazombiNetworkingResponse WithException(this WazombiNetworkingResponse response, int exception)
        {
            response.Exception = exception;
            return response;
        }

        public static WazombiNetworkingResponse WithStatus(this WazombiNetworkingResponse response, HttpStatusCode statusCode)
        {
            response.StatusCode = statusCode;
            return response;
        }

        public static WazombiNetworkingResponse WithCookies(this WazombiNetworkingResponse response, CookieCollection cookies)
        {
            if (cookies != null && cookies.Count > 0)
            {
                response.Cookies = cookies;
            }
            return response;
        }

        public static WazombiNetworkingResponse WithJson(this WazombiNetworkingResponse response, JsonValue json)
        {
            response.Json = json;
            return response;
        }

        public async static Task<T> WithTimeout<T>(this Task<T> task, int duration)
        {
            var retTask = await Task.WhenAny(task, Task.Delay(duration)).ConfigureAwait(false);

            if (retTask is Task<T>)
            {
                return task.Result;
            }

            return default(T);
        }

        public static async Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest httpWebRequest, TimeSpan timeout, CancellationToken cancellationToken)
        {
            var timeoutCancellationTokenSource = new CancellationTokenSource();
            using (timeoutCancellationTokenSource.Token.Register(httpWebRequest.Abort, false))
            {
                try
                {
                    timeoutCancellationTokenSource.CancelAfter(timeout);
                    return await httpWebRequest.GetResponseAsync(cancellationToken);
                }
                catch (WebException e)
                {
                    if (timeoutCancellationTokenSource.Token.IsCancellationRequested)
                        throw new TimeoutException(e.Message, e);
                    throw;
                }
            }
        }

        static async Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest httpWebRequest, CancellationToken cancellationToken)
        {
            using (cancellationToken.Register(httpWebRequest.Abort, false))
            {
                try
                {
                    var response = await httpWebRequest.GetResponseAsync();
                    return (HttpWebResponse)response;
                }
                catch (WebException e)
                {
                    // WebException is thrown when request.Abort() is called,
                    // but there may be many other reasons,
                    // propagate the WebException to the caller correctly
                    if (cancellationToken.IsCancellationRequested)
                    {
                        // the WebException will be available as Exception.InnerException
                        throw new OperationCanceledException(e.Message, e, cancellationToken);
                    }

                    // cancellation hasn't been requested, rethrow the original WebException
                    throw;
                }
            }
        }
    }
}
