
using System.Drawing;
using System.Windows.Forms;

namespace thinkforce_desktop.Base
{
    public class UIButton : Button
    {
        private Frame frame;

        public Frame Frame
        {
            get
            {
                return frame;
            }
            set
            {
                frame = value;

                Location = new System.Drawing.Point(frame.X, frame.Y);
                Size = new System.Drawing.Size(frame.W, frame.H);

                LayoutSubviews();
            }
        }

        public new Image Image
        {
            get
            {
                return base.Image;
            }
            set
            {
                int padding = Frame.H / 5;

                int width = Frame.W - 2 * padding;
                int height = width;

                Image newImage = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.DrawImage(value, 0, 0, width, height);
                }
                base.Image = newImage;
            }
        }

        public UIButton()
        {
            AutoSize = true;
        }

        public virtual void LayoutSubviews()
        {

        }
    }
}
