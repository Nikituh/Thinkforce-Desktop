
namespace thinkforce_desktop.Base
{
    public class Frame
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int W { get; set; }

        public int H { get; set; }

        public int Bottom { get { return Y + H; } }

        public Frame Bounds { get { return new Frame(0, 0, W, H); } }

        public Frame()
        {
        }

        public Frame(int w, int h)
        {
            this.W = w;

            this.H = h;
        }

        public Frame(int x, int y, int w, int h)
        {
            this.X = x;

            this.Y = y;

            this.W = w;

            this.H = h;
        }

        public override string ToString()
        {
            return X + " - " + Y + " - " + W + " - " + H;
        }

        public static Frame CenterEmptyInParent(UIView view)
        {
            return new Frame(view.Frame.W / 2, view.Frame.H / 2, 0, 0);
        }

        public static Frame CenterEmptyInParent(Subview view)
        {
            return new Frame(view.Frame.W / 2, view.Frame.H / 2, 0, 0);
        }

    }
}
