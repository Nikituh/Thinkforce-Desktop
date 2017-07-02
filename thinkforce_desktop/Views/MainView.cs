
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using thinkforce_desktop.Base;
using thinkforce_desktop.Networking;
using thinkforce_desktop.Views.Components;

namespace thinkforce_desktop.Views
{
    public class MainView : UIView
    {
        int smallPadding = 5;
        int indicatorSize = 30;

        public EventHandler LoadComplete;
        public EventHandler<KeyDownEventArgs> CorrectKeyPressed;

        UILabel Label { get; set; }
        UILabel InstructionLabel { get; set; }
        UILabel BottomInstructionLabel { get; set; }

        public ConnectionIndicator Indicator { get; set; }

        KeyConfirmationBox turnOff;
        KeyConfirmationBox turnOn;
        KeyConfirmationBox colorChange1;
        KeyConfirmationBox colorChange2;

        public MainView()
        {
            Text = "ThinkForce";
            KeyPreview = true;
            BackColor = Color.FromArgb(245, 245, 245);

            Label = new UILabel();

            Label.Text = "BRAINWAVE RECEIVER | LAMP CONTROL";
            Label.TextAlign = ContentAlignment.MiddleLeft;
            Label.Font = new Font("Trebuchet MS", 9, FontStyle.Bold);

            AddView(Label);

            InstructionLabel = new UILabel();
            InstructionLabel.TextAlign = ContentAlignment.MiddleLeft;
            InstructionLabel.Font = new Font("Trebuchet MS", 9, FontStyle.Regular);
            InstructionLabel.Text = "Assign keys to turn lamps on, off or to change their color ";

            AddView(InstructionLabel);

            Indicator = new ConnectionIndicator();

            AddView(Indicator);

            turnOn = new KeyConfirmationBox("Turn on ");
            turnOff = new KeyConfirmationBox("Turn off ");

            colorChange1 = new KeyConfirmationBox("Change color to ", true);
            colorChange2 = new KeyConfirmationBox("Change color to ", true);

            AddView(turnOn);
            AddView(turnOff);

            AddView(colorChange1);
            AddView(colorChange2);

            BottomInstructionLabel = new UILabel();
            BottomInstructionLabel.TextAlign = ContentAlignment.MiddleLeft;
            BottomInstructionLabel.Font = new Font("Trebuchet MS", 9, FontStyle.Regular);
            BottomInstructionLabel.Text = "* Click on a color to 'choose' that color";

            AddView(BottomInstructionLabel);

        }

        public override void LayoutSubviews()
        {
            int padding = 5;

            int x = padding;
            int y = 0;
            int w = Frame.W - (2 * padding + 2 * indicatorSize);
            int h = Frame.H / 8;

            Label.Frame = new Frame(x, y, w, h);

            y += h;

            w = Frame.W - (2 * padding);

            InstructionLabel.Frame = new Frame(x, y, w, h);

            w = indicatorSize;
            h = w;
            x = Frame.W - (w + smallPadding);
            y = smallPadding;

            Indicator.Frame = new Frame(x, y, w, h);

            x = padding;
            y = InstructionLabel.Frame.Bottom;
            w = Frame.W - 4 * padding;
            h = Frame.H / 10;

            turnOn.Frame = new Frame(x, y, w, h);

            y += h + padding;

            turnOff.Frame = new Frame(x, y, w, h);

            y += h + padding;

            colorChange1.Frame = new Frame(x, y, w, h);

            y += h + padding;

            colorChange2.Frame = new Frame(x, y, w, h);

            y += h + padding;
            w = InstructionLabel.Frame.W;

            BottomInstructionLabel.Frame = new Frame(x, y, w, h);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            string key = e.KeyCode.ToString().ToLower();

            if (turnOn.Text == key)
            {
                CorrectKeyPressed?.Invoke(null, new KeyDownEventArgs { Command = Program.COMMANDON });
            }
            else if (turnOff.Text == key)
            {
                CorrectKeyPressed?.Invoke(null, new KeyDownEventArgs { Command = Program.COMMANDOFF });

            } else if (colorChange1.Text == key) {

                CorrectKeyPressed?.Invoke(null, new KeyDownEventArgs {
                    Color = colorChange1.SelectedColor, Command = Program.COMMANDCOLOR
                });

            } else if (colorChange2.Text == key) {

                CorrectKeyPressed?.Invoke(null, new KeyDownEventArgs {
                    Color = colorChange2.SelectedColor, Command = Program.COMMANDCOLOR
                });

            }
        }

    }

    public class KeyDownEventArgs : EventArgs
    {
        public string Command { get; set; }

        public LampColor Color { get; set; }
    }
}
