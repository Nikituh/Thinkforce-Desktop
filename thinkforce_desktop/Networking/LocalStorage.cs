using System;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thinkforce_desktop.Networking
{

    public  class LocalStorage
    {
        public static LocalStorage Instance = new LocalStorage();

        const string FILENAME = "accounts";
        const string JSON = ".json";

        static string FULLPATH = Directory.GetCurrentDirectory();
        static string PATH = FULLPATH + "\\" + FILENAME + JSON;

        string[] TextFiles
        {
            get
            {
                return Directory.GetFiles(FULLPATH, "*" + JSON);
            }
        }

        public bool ContainsFile
        {
            get
            {
                return TextFiles.Contains(PATH);
            }
        }

        public void Write(List<Account> accounts)
        {
            JsonValue json = EncodeAccounts(accounts);
            
            File.WriteAllText(PATH, json.ToString());
        }

        public List<Account> Read()
        {
            List<Account> accounts = new List<Account>();

            if (!File.Exists(FILENAME + JSON))
            {
                FileStream stream = File.Create(FILENAME + JSON);
                stream.Close();
            }

            using (StreamReader r = new StreamReader(FILENAME + JSON))
            {
                string contents = r.ReadToEnd();

                if (string.IsNullOrWhiteSpace(contents))
                {
                    return accounts;
                }

                JsonValue json = JsonValue.Parse(contents);

                accounts = DecodeAccounts(json);
            }

            return accounts;
        }

        JsonValue EncodeAccounts(List<Account> accounts)
        {
            string content = "";

            for (int i = 0; i < accounts.Count; i++)
            {
                Account account = accounts[i];
                content += EncodeAccount(account).ToString();

                if (i < accounts.Count - 1)
                {
                    content += ", ";
                }
                
            }

            content += "";

            return JsonValue.Parse(content);
        }

        JsonValue EncodeAccount(Account account)
        {
            return new JsonObject { { account.IPAddress, account.Username } };
        }

        List<Account> DecodeAccounts(JsonValue json)
        {
            List<Account> accounts = new List<Account>();

            foreach (KeyValuePair<string, JsonValue> child in json)
            {
                Account account = new Account {
                    IPAddress = child.Key,
                    Username = (string)child.Value
                };
                accounts.Add(account);
            }

            return accounts;
        }

        public List<string> DecodeUsernames(JsonValue json)
        {
            List<string> usernames = new List<string>();

            foreach (KeyValuePair<string, JsonValue> child in json)
            {
                string username = (string)child.Value;
                usernames.Add(username);
            }

            return usernames;
        }
    }

    public class Account
    {
        public string IPAddress { get; set; }

        public string Username { get; set; }

        public Account()
        {

        }

        public Account(string ip, string username)
        {
            IPAddress = ip;
            Username = username;
        }
    }
}
