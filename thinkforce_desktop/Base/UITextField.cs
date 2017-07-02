
using System;
using System.Windows.Forms;

namespace thinkforce_desktop.Base
{
    public class UITextField : TextBox
    {
        public EventHandler<EventArgs> EditingEnded;

        private Frame frame;

        public const int HEIGHT = 20;

        public Frame Frame
        {
            get
            {
                return frame;
            }
            set
            {
                frame = value;

                int y = frame.H / 2 - HEIGHT / 2;

                Location = new System.Drawing.Point(frame.X, y);
                Size = new System.Drawing.Size(frame.W, HEIGHT);

                LayoutSubviews();
            }
        }

        public UITextField()
        {
            LostFocus += delegate
            {
                if (EditingEnded != null)
                {
                    EditingEnded(this, new EventArgs());
                }
            };
        }

        public virtual void LayoutSubviews()
        {

        }

    }
}
