using a7D.PDV.Componentes.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace a7D.PDV.Componentes
{
    public class FormTouch : Form
    {
        private static readonly Color AzulColor = Color.FromArgb(0, 197, 215);
        private static readonly StringFormat CenterAlign = new StringFormat() { Alignment = StringAlignment.Center };
        private static readonly int CantoSize = 30;

        public int TituloSize { get; set; } = 60;

        private bool move;
        private int moveX;
        private int moveY;
        private bool resizeNWSE;
        private bool resizeNESW;
        private int sizeX;
        private int sizeY;
        private int sizeWidth;
        private int sizeHeight;

        protected bool CanResize => this.WindowState == FormWindowState.Normal && MinimumSize.IsEmpty || MinimumSize != MaximumSize;

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        public FormTouch()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            //Cursor.Current = Cursors.Hand;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var topoTriangulo = new Point[] {
                new Point(3, TituloSize-10),
                new Point(Width-3, TituloSize-10),
                new Point(Width/2, TituloSize+5)
            };

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.Clear(AzulColor);
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, Width - 1, Height - 1);
            e.Graphics.FillRectangle(Brushes.White, 3, TituloSize - 9, Width - 6, Height - TituloSize + 6);
            e.Graphics.FillPolygon(new SolidBrush(AzulColor), topoTriangulo);

            e.Graphics.DrawString(Text,
                new Font(this.Font.Name, TituloSize / 3, FontStyle.Bold, GraphicsUnit.Pixel),
                Brushes.White,
                new Rectangle(3, TituloSize / 4, Width - 20, TituloSize - 3), CenterAlign);

            var imgClose = Resources.close_box_outline_red;
            e.Graphics.DrawImage(imgClose, Width - TituloSize, 0, TituloSize - 5, TituloSize - 5);

            if (CanResize)
            {
                var cantoEsquerdo = new Point[] {
                    new Point(3, Height-3),
                    new Point(CantoSize+3, Height-3),
                    new Point(3, Height - CantoSize-3)
                };

                var cantoDireito = new Point[] {
                    new Point(Width - CantoSize-3, Height-3),
                    new Point(Width-3, Height-3),
                    new Point(Width-3, Height - CantoSize-3)
                };

                e.Graphics.FillPolygon(Brushes.LightGray, cantoEsquerdo);
                e.Graphics.FillPolygon(Brushes.LightGray, cantoDireito);
            }
        }

        bool AreaTOP(MouseEventArgs e)
        {
            return e.Y < TituloSize && e.X < Width - TituloSize;
        }

        bool AreaClose(MouseEventArgs e)
        {
            return e.Y < TituloSize && e.X > Width - TituloSize;
        }

        bool AreaNWSE(MouseEventArgs e)
        {
            return e.X > Width - CantoSize && e.Y > Height - CantoSize;
        }

        bool AreaNESW(MouseEventArgs e)
        {
            return e.X < CantoSize && e.Y > Height - CantoSize;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            resizeNWSE = resizeNESW = move = false;
            base.OnMouseDown(e);
            if (AreaNWSE(e) && CanResize)
            {
                var minTop = Screen.AllScreens.Min(s => s.Bounds.Top);
                if (this.Top < minTop)
                    this.Top = minTop;

                var minLeft = Screen.AllScreens.Min(s => s.Bounds.Left);
                if (this.Left < minLeft)
                    this.Left = minLeft;

                resizeNWSE = true;
                Cursor.Current = Cursors.SizeNWSE;
            }
            else if (AreaNESW(e) && CanResize)
            {
                resizeNESW = true;
                Cursor.Current = Cursors.SizeNESW;
            }
            else if (AreaTOP(e) && WindowState == FormWindowState.Normal)
            {
                move = true;
                moveX = e.X;
                moveY = e.Y;
                Cursor.Current = Cursors.Hand;
                return;
            }
            else if (AreaClose(e))
            {
                Close();
            }
            else
                return;

            sizeX = e.X;
            sizeY = e.Y;
            sizeWidth = Width;
            sizeHeight = Height;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            resizeNESW = resizeNWSE = move = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (resizeNWSE)
            {
                int w = sizeWidth + e.X - sizeX;
                int h = sizeHeight + e.Y - sizeY;
                Size = new Size(Math.Max(w, MinimumSize.Width), Math.Max(h, MinimumSize.Height));
            }
            else if (resizeNESW)
            {
                int offsetX = this.Left;
                this.Left = MousePosition.X - sizeX;
                int w = Width + offsetX - this.Left;
                int h = sizeHeight + e.Y - sizeY;
                Size = new Size(Math.Max(w, MinimumSize.Width), Math.Max(h, MinimumSize.Height));
            }
            else if (move)
            {
                Left = MousePosition.X - moveX;
                Top = MousePosition.Y - moveY;
            }
        }
    }
}
