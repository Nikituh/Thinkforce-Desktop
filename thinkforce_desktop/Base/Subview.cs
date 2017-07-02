
using System.Windows.Forms;

namespace thinkforce_desktop.Base
{
    public class Subview : Control
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

        public Subview()
        {

        }

        public virtual void LayoutSubviews()
        {

        }

        public void UpdateHeight(int h)
        {
            Size = new System.Drawing.Size(Frame.W, h);
        }

        public void UpdateWidth(int w)
        {
            Size = new System.Drawing.Size(w, Frame.H);
        }
    }
}
