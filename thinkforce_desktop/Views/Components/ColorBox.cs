using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using thinkforce_desktop.Base;
using thinkforce_desktop.Networking;

namespace thinkforce_desktop.Views.Components
{
    public class ColorBox : Subview
    {
        public EventHandler ColorClick;

        Subview color1, color2, color3, color4;

        LampColor selected;

        public LampColor SelectedColor
        {
            get
            {
                return selected;
            }
        }

        public ColorBox()
        {
            color1 = new Subview();
            color1.BackColor = Color.Red;
            Controls.Add(color1);

            color2 = new Subview();
            color2.BackColor = Color.Green;
            Controls.Add(color2);

            color3 = new Subview();
            color3.BackColor = Color.Blue;
            Controls.Add(color3);

            color4 = new Subview();
            color4.BackColor = Color.Yellow;
            Controls.Add(color4);

            Cursor = System.Windows.Forms.Cursors.Hand;

            color1.Click += OnColorClicked;
            color2.Click += OnColorClicked;
            color3.Click += OnColorClicked;
            color4.Click += OnColorClicked;
        }

        private void OnColorClicked(object sender, EventArgs e)
        {
            Subview box = (Subview)sender;

            selected = box.BackColor.ToLampColor();

            ColorClick?.Invoke(sender, e);
        }

        public override void LayoutSubviews()
        {
            int smallPadding = 5;

            int padding = 10;
            int height = 25;
            int width = height;

            int x = padding;
            int y = Frame.H / 2 - height / 2;
            int w = width;
            int h = height;

            color1.Frame = new Frame(x, y, w, h);

            x += w + smallPadding;

            color2.Frame = new Frame(x, y, w, h);

            x += w + smallPadding;

            color3.Frame = new Frame(x, y, w, h);

            x += w + smallPadding;

            color4.Frame = new Frame(x, y, w, h);
        }
    }

    public static class ColorExtensions
    {
        public static LampColor ToLampColor (this Color color)
        {
            if (color == Color.Red)
            {
                return LampColor.Red;
            }
            else if (color == Color.Green)
            {
                return LampColor.Green;
            }
            else if (color == Color.Blue)
            {
                return LampColor.Blue;
            }
            else if (color == Color.Yellow)
            {
                return LampColor.Yellow;
            }

            return LampColor.None;
        }

        public static Color ToColor(this LampColor color)
        {
            if (color == LampColor.Red)
            {
                return Color.Red;
            }
            else if (color == LampColor.Blue)
            {
                return Color.Blue;
            }
            else if (color == LampColor.Green)
            {
                return Color.Green;
            }
            else if (color == LampColor.Yellow)
            {
                return Color.Yellow;
            }

            return Color.DimGray;
        }
    }
}
