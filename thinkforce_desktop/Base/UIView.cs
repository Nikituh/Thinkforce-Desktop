
using System.Windows.Forms;

namespace thinkforce_desktop.Base
{
    public class UIView : Form
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

        public UIView()
        {

        }

        public virtual void LayoutSubviews()
        {

        }

        public void UpdateSize(int w, int h)
        {
            Size = new System.Drawing.Size(w, h);
            frame = new Frame(frame.X, frame.Y, w, h);
        }

        public void AddView(Control child)
        {
            Controls.Add(child);
        }

    }
}
