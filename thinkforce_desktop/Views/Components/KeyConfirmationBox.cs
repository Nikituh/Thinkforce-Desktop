
using System;
using System.Drawing;
using thinkforce_desktop.Base;
using thinkforce_desktop.Networking;

namespace thinkforce_desktop.Views.Components
{
    public class KeyConfirmationBox : Subview
    {
        UILabel label;
        UITextField field;
        Subview chosenColor;

        public new string Text { get { return field.Text;  } }

        bool IsColorBox;

        ColorBox colorBox;
        public LampColor SelectedColor
        {
            get
            {
                return colorBox.SelectedColor;
            }
        }

        public KeyConfirmationBox(string text, bool isColorBox = false)
        {
            IsColorBox = isColorBox;

            label = new UILabel();
            label.Text = text;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Font = new Font("Trebuchet MS", 9);

            field = new UITextField();

            Controls.Add(label);

            Controls.Add(field);

            BackColor = Color.FromArgb(210, 210, 210);

            if (IsColorBox)
            {
                chosenColor = new Subview();
                Controls.Add(chosenColor);

                colorBox = new ColorBox();
                Controls.Add(colorBox);
                colorBox.ColorClick += UpdateColor;
            }
        }

        public override void LayoutSubviews()
        {
            int padding = 5;

            int x = padding;
            int y = 0;
            int w = (int)(Frame.W / 3.9f);
            int h = Frame.H;

            label.Frame = new Frame(x, y, w, h);

            x += w + padding;

            h = Frame.H / 2;
            w = h;
            y = Frame.H / 2 - h / 2;

            if (chosenColor != null)
            {
                chosenColor.Frame = new Frame(x, y, w, h);
            }

            x += w + padding;
            y = 0;
            w = Frame.W / 4;
            h = Frame.H;

            field.Frame = new Frame(x, y, w, h);

            if (IsColorBox)
            {
                x += w;
                w = w * 2;
                colorBox.Frame = new Frame(x, y, w, h);
            }
        }

        public void UpdateColor (object sender, EventArgs e)
        {
            chosenColor.BackColor = colorBox.SelectedColor.ToColor();
        }
    }
}
