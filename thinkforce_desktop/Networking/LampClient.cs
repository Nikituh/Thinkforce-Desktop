using Q42.HueApi;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using thinkforce_desktop.Networking.Response;
using thinkforce_desktop.Networking.Wazombi;

namespace thinkforce_desktop.Networking
{
    public class LampClient
    {
        public static EventHandler IPConfigured;
        public static EventHandler FailedToRegisterIP;

        //static string LIGHTS_BASE = "http://192.168.37.241/api/74911eed7a0f0f48641a562561102c9c/";

        static string LIGHTS_BASE
        {
            get
            {
                return "http://" + IP + "/api/" + REGISTEREDUSERNAME + "/";
            }
        }

        const string LIGHTS_API = "lights/";
        const string LIGHTS_GROUP_API = "groups/0/action/";

        public static string IP = "";
        public static string REGISTEREDUSERNAME = "";

        const string APPNAME = "THINKFORCE_APP";
        const string APPKEY = "TH1NKF0RC3";

        public static async void ConfigureIP(List<Account> accounts)
        {
            IBridgeLocator locator = new HttpBridgeLocator();

            //For Windows 8 and .NET45 projects you can use the SSDPBridgeLocator which actually scans your network. 
            //See the included BridgeDiscoveryTests and the specific .NET and .WinRT projects
            IEnumerable<string> bridgeIPs = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

            if (bridgeIPs.ToList().Count > 0)
            {
                IP = bridgeIPs.ToList()[0];
                Console.WriteLine("Registered IP: " + IP);

                Account found = accounts.Find(account => account.IPAddress == IP);

                if (found != null) {
                    REGISTEREDUSERNAME = found.Username;
                    IPConfigured?.Invoke(true, EventArgs.Empty);
                } else
                {
                    IPConfigured?.Invoke(false, EventArgs.Empty);
                }
            } else
            {
                IP = null;
                Console.WriteLine("Failed to register IP: " + IP);

                FailedToRegisterIP?.Invoke(false, EventArgs.Empty);
            }

        }

        public static async Task<LampResponse> RegisterDevice()
        {
            LampResponse response = new LampResponse();

            string url = "http://" + IP + "/api";
            JsonValue json = Codec.EncodeRegistrationJson(APPNAME, APPKEY);

            WazombiNetworkingResponse intermediary = await WazombiNetworking.Post(url, json);

            if (intermediary.HasException) {
                response.Error = new LampError {
                    Type = intermediary.Exception,
                    Description = "Oops! Something went terribly wrong. Please try again"
                };
            }
            else
            {
                response = Codec.DecodeLampResponse(intermediary.Json);

                if (response.IsOk)
                {
                    REGISTEREDUSERNAME = response.Username;
                }
            }

            return response;
        }

        public static async void TurnLightsOn()
        {
            await TurnLights(true);
        }

        public static async void TurnLightsOff()
        {
            await TurnLights(false);
        }

        async static Task<WazombiNetworkingResponse> TurnLights(bool _on)
        {
            string url = LIGHTS_BASE + LIGHTS_GROUP_API;
            JsonValue json = Codec.TurnLights(_on);

            WazombiNetworkingResponse response = await WazombiNetworking.Put(url, json);

            return response;
        }

        public async static void TurnLampToColor(LampColor color)
        {
            string url = LIGHTS_BASE + LIGHTS_GROUP_API;
            JsonValue json = Codec.EncodeHueChange(color);

            WazombiNetworkingResponse response = await WazombiNetworking.Put(url, json);
        }
    }
}
