
using System.Windows.Forms;

namespace thinkforce_desktop.Base
{
    public class UILabel : Label
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

        public virtual void LayoutSubviews()
        {

        }
    }
}
