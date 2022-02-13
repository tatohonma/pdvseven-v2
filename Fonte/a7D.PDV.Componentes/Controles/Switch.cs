using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace a7D.PDV.Componentes.Controles
{
    public class Switch : CheckBox
    {
        public Switch()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            Padding = new Padding(6);
            MaximumSize = new Size(0, 34);
            Cursor = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.OnPaintBackground(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = new GraphicsPath())
            {
                var padding = Padding.All - 4;
                var radix = Height - 2 * padding;

                path.AddArc(padding, padding, radix, radix, 90, 180);
                path.AddArc(Width - radix - padding, padding, radix, radix, -90, 180);

                path.CloseFigure();

                e.Graphics.FillPath(Checked ? Brushes.DarkGray : Brushes.LightGray, path);


                radix = Height - 1;
                var rect = Checked ? new Rectangle(Width - radix - 1, 0, radix, radix)
                                   : new Rectangle(0, 0, radix, radix);

                e.Graphics.FillEllipse(Checked ? Brushes.Green : Brushes.DimGray, rect);
            }
        }
    }
}
