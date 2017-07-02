using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using thinkforce_desktop.Base;
using thinkforce_desktop.Networking;
using thinkforce_desktop.Networking.Response;
using thinkforce_desktop.Views;

namespace thinkforce_desktop
{
    static class Program
    {
        public const string COMMANDON = "COMMANDON";
        public const string COMMANDOFF = "COMMANDOFF";
        public const string COMMANDCOLOR = "COMMANDCOLOR";

        static Frame WindowSize
        {
            get
            {
                int w = 400;
                int h = 450;
                int x = Device.ScreenWidth / 2 - w / 2;
                int y = Device.ScreenHeight / 2 - h / 2;

                return new Frame(x, y, w, h);
            }
        }

        public static MainView ContentView { get; set; }

        static List<Account> Accounts { get; set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ContentView = new MainView();

            Device.Measure(ContentView);

            ContentView.Frame = WindowSize;

            Accounts = LocalStorage.Instance.Read();

            LampClient.IPConfigured += RegisterDevice;
            LampClient.FailedToRegisterIP += EpicFail;

            ContentView.CorrectKeyPressed += OnCorrectKeyPress;

            LampClient.ConfigureIP(Accounts);

            try
            {
                Application.Run(ContentView);
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION! -" + e.Message);
            }
        }

        static async Task RegisterDevice()
        {
            LampResponse response = await LampClient.RegisterDevice();

            if (response.IsOk)
            {
                ContentView.Indicator.IsConnected = true;
                Accounts.Add(new Account(LampClient.IP, LampClient.REGISTEREDUSERNAME));
                LocalStorage.Instance.Write(Accounts);
            }
            else
            {

                if (response.LinkNotPressedError)
                {
                    CreateLinkAlert();
                } else
                {
                    CreateAlert(response.Error.Description, "Something went terribly wrong", 
                        delegate { }, 
                        delegate { }
                    );
                }
            }
        }

        static async void RegisterDevice(object sender, EventArgs e)
        {
            bool foundAccount = (bool)sender;

            if (foundAccount)
            {
                ContentView.Indicator.IsConnected = true;
            }
            else
            {
                await RegisterDevice();
            }
        }

        static void EpicFail(object sender, EventArgs e)
        {
            string description = "Couldn't register IP. Are you sure your device is turned on?";

            CreateOkAlert(description, "Something went terribly wrong");
        }

        static void CreateLinkAlert()
        {
            string text = "You need to link this application to the lamp." +
                "Press the large round button the bridge and then click 'ok' to retry";
            string caption = "Please do stuff.";

            CreateAlert(text, caption, 
                async delegate {
                    await RegisterDevice(); },
                delegate {

                });
        }

        static void CreateAlert(string text, string caption, Action ok, Action cancel)
        {
            DialogResult result = MessageBox.Show(text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                ok();
            }
            else
            {
                cancel();
            }
        }

        static void CreateOkAlert(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }


        static void OnCorrectKeyPress(object sender, KeyDownEventArgs e)
        {
            string command = e.Command;

            if (command == COMMANDON)
            {
                TurnLightsOn();
            }
            else if (command == COMMANDOFF)
            {
                TurnLightsOff();
            }
            else if (command == COMMANDCOLOR)
            {
                ChangeLampColor(e.Color);
            }
        }

        static void TurnLightsOn()
        {
           LampClient.TurnLightsOn();
        }

        static void TurnLightsOff()
        {
            LampClient.TurnLightsOff();
        }

        static void ChangeLampColor(LampColor color)
        {
            LampClient.TurnLampToColor(color);
        }

    }
}
