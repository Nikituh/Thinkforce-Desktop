
using System.Drawing;
using System.Windows.Forms;

namespace thinkforce_desktop.Base
{
    class Device
    {
        public static int ScreenWidth { get; private set; }

        public static int ScreenHeight { get; private set; }

        public static int MenuBarHeight { get; private set; }

        public static int VerticalScrollBarWidth
        {
            get
            {
                return SystemInformation.VerticalScrollBarWidth;
            }
        }

        public static int HorizontalScrollBarHeight
        {
            get
            {
                return SystemInformation.HorizontalScrollBarHeight;
            }
        }

        public static void Measure(Form form)
        {
            ScreenWidth = Screen.FromControl(form).Bounds.Width;

            ScreenHeight = Screen.FromControl(form).Bounds.Height;

            MenuBarHeight = CalculateMenuHeight(form);
        }

        static int CalculateMenuHeight(Form form)
        {
            Rectangle screenRectangle = form.RectangleToScreen(form.ClientRectangle);
            return screenRectangle.Top - form.Top;
        }
    }
}
