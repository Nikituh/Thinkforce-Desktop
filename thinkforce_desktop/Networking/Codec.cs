using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinkforce_desktop.Networking.Response;

namespace thinkforce_desktop.Networking
{
    public enum LampColor
    {
        None = 0,
        Yellow = 10000,
        Green = 20000,
        Blue = 40000,
        Red = 60000
    }

    public class Codec
    {
        const string DEVICETYPE = "devicetype";

        const string ERROR = "error";
        const string TYPE = "type";
        const string ADDRESS = "address";
        const string DESCRIPTION = "description";

        const string SUCCESS = "success";
        const string USERNAME = "username";

        public static JsonValue TurnLights(bool _on)
        {
            return new JsonObject { { "on", _on } };
        }

        public static JsonValue EncodeRegistrationJson(string name, string key)
        {
            return new JsonObject { { DEVICETYPE, name + "#" + key } };
        }

        public static LampResponse DecodeLampResponse (JsonValue json)
        {
            LampResponse response = new LampResponse();

            foreach (KeyValuePair<string, JsonValue> child in json)
            {
                if (child.Value.ContainsKey(ERROR))
                {
                    response.Error = DecodeError(child.Value);
                } else if (child.Value.ContainsKey(SUCCESS))
                {
                    response.Username = (string)child.Value[SUCCESS][USERNAME];
                }
            }

            return response;
        }

        public static JsonValue EncodeHueChange(LampColor color)
        {
            return new JsonObject {
                { "hue", (int)color },
                { "sat", 254 },
                { "bri", 150 }
            };
        }

        public static LampError DecodeError (JsonValue json)
        {
            LampError error = new LampError();

            json = json[ERROR];

            foreach (KeyValuePair<string, JsonValue> child in json)
            {
                Console.WriteLine(child.Key + " - " + child.Value);

                if (child.Key == TYPE)
                {
                    error.Type = (int)child.Value;
                }
                else if (child.Key == ADDRESS)
                {
                    error.Address = (string)child.Value;
                }
                else if (child.Key == DESCRIPTION)
                {
                    error.Description = (string)child.Value;
                }
            }

            return error;
        }

        #region DEFAULTPARSE

        static int GetIntValue(JsonValue json, string key, int defaultValue)
        {
            if (!json.ContainsKey(key))
            {
                return defaultValue;
            }

            if (json[key] == null)
            {
                return defaultValue;
            }

            return (int)json[key];
        }

        static string GetStringValue(JsonValue json, string key, string defaultValue)
        {
            if (!json.ContainsKey(key))
            {
                return defaultValue;
            }

            if (json[key] == null)
            {
                return defaultValue;
            }

            return (string)json[key];
        }

        #endregion
    }

}
