using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using thinkforce_desktop.Base;

namespace thinkforce_desktop.Views.Components
{
    public class ConnectionIndicator : Subview
    {
        bool isConnected;

        public bool IsConnected
        {
            get { return IsConnected; }
            set
            {
                isConnected = value;

                if (isConnected)
                {
                    BackColor = Color.Green;
                } else
                {
                    BackColor = Color.Red;
                }
            }
        }

        Pen _pen;
        SolidBrush _brush;
        SolidBrush _brushInside;

        public ConnectionIndicator()
        {
            Color _color = Color.Red;

            _pen = new Pen(_color);
            _brush = new SolidBrush(Color.FromKnownColor(KnownColor.Control));
            _brushInside = new SolidBrush(_color);

            BackColor = _color;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            System.Drawing.Drawing2D.GraphicsPath buttonPath = new System.Drawing.Drawing2D.GraphicsPath();
            
            // Set a new rectangle to the same size as the button's ClientRectangle property.
            Rectangle newRectangle = ClientRectangle;

            // Decrease the size of the rectangle.
            newRectangle.Inflate(-10, -10);

            // Draw the button's border.
            e.Graphics.DrawEllipse(Pens.Black, newRectangle);

            // Increase the size of the rectangle to include the border.
            newRectangle.Inflate(1, 1);

            // Create a circle within the new rectangle.
            buttonPath.AddEllipse(newRectangle);

            // Set the button's Region property to the newly created circle region.
            Region = new Region(buttonPath);
        }
    }
}
