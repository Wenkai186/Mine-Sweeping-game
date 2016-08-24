using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace MineSweeping
{
    partial class GameForm
    {

        #region 标题栏

        Color titleColorStart = Color.CadetBlue;
        public Color TitleColorStart
        {
            get { return titleColorStart; }
            set
            {
                titleColorStart = value;

                if (titleColorStart == null)
                    titleColorStart = Color.Gainsboro;

                this.Invalidate();
            }
        }

        Color titleColorEnd = Color.LightCyan;
        public Color TitleColorEnd
        {
            get { return titleColorEnd; }
            set
            {
                titleColorEnd = value;

                if (titleColorEnd == null)
                    titleColorEnd = Color.White;

                this.Invalidate();
            }
        }

        Image titleImage = null;
        public Image TitleImage
        {
            get { return titleImage; }
            set { titleImage = value; this.Invalidate(); }
        }

        Color foreBackColor = Color.CadetBlue;
        public Color ForeBackColor
        {
            get { return foreBackColor; }
            set
            {
                if (value != null)
                    foreBackColor = value;
                else
                    foreBackColor = Color.Gainsboro;

                this.Invalidate();
            }
        }

        #endregion 标题栏

        #region 窗体

        Color formColorStart = Color.FromArgb(211, 231, 255);
        public Color FormColorStart
        {
            get { return formColorStart; }
            set
            {
                formColorStart = value;

                if (formColorStart == null)
                    formColorStart = Color.FromArgb(211, 231, 255);

                this.Invalidate();
            }
        }

        Color formColorEnd = Color.Azure;
        public Color FormColorEnd
        {
            get { return formColorEnd; }
            set
            {
                formColorEnd = value;

                if (formColorEnd == null)
                    formColorEnd = Color.Azure;

                this.Invalidate();
            }
        }

        float formColorAngle = 0;
        public float FormColorAngle
        {
            get { return formColorAngle; }
            set
            {
                if (value >= 0 && value <= 360)
                {
                    formColorAngle = value;
                    this.Invalidate();
                }
            }
        }

        Color borderColor = Color.SteelBlue;
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; this.Invalidate(); }
        }

        #endregion 窗体

        #region 控制按扭

        Color ctrlBoxNormal = Color.DarkSlateGray;
        public Color CtrlBoxNormal
        {
            get { return ctrlBoxNormal; }
            set
            {
                ctrlBoxNormal = value;
                if (ctrlBoxNormal == null)
                    ctrlBoxNormal = Color.SteelBlue;

                this.Invalidate();
            }
        }

        Color ctrlBoxOn = Color.DarkRed;
        public Color CtrlBoxOn
        {
            get { return ctrlBoxOn; }
            set
            {
                ctrlBoxOn = value;
                if (ctrlBoxOn == null)
                    ctrlBoxOn = Color.DarkRed;

                this.Invalidate();
            }
        }

        Color ctrlBoxDown = Color.Orange;
        public Color CtrlBoxDown
        {
            get { return ctrlBoxDown; }
            set
            {
                ctrlBoxDown = value;
                if (ctrlBoxDown == null)
                    ctrlBoxDown = Color.Orange;

                this.Invalidate();
            }
        }

        Color ctrlBoxDisabled = Color.Gray;
        public Color CtrlBoxDisabled
        {
            get { return ctrlBoxDisabled; }
            set
            {
                ctrlBoxDisabled = value;
                if (ctrlBoxDisabled == null)
                    ctrlBoxDisabled = Color.Gray;
            }
        }

        #endregion 控件按扭

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //窗体
            DrawForm(e.Graphics);

            //标题栏
            DrawTitle(e.Graphics);

            DrawMin(e.Graphics);
            DrawMax(e.Graphics);
            DrawClose(e.Graphics);
        }

        private void DrawForm(Graphics g)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(0, 0, 6, 6, 180, 90);
            gp.AddArc(this.Width - 7, 0, 6, 6, 270, 90);
            gp.AddArc(this.Width - 7, this.Height - 7, 6, 6, 0, 90);
            gp.AddArc(0, this.Height - 7, 6, 6, 90, 90);
            gp.CloseFigure();

            LinearGradientBrush lineBrush = new LinearGradientBrush(this.ClientRectangle, formColorStart, formColorEnd, 360 - formColorAngle);
            g.FillPath(lineBrush, gp);
            lineBrush.Dispose();

            Pen pen = new Pen(borderColor);
            pen.Width = 1;
            g.DrawPath(pen, gp);

            gp.Dispose();
            pen.Dispose();
        }

        private void DrawTitle(Graphics g)
        {
            int split = 12;

            GraphicsPath gpUp = new GraphicsPath();
            gpUp.AddArc(0, 0, 6, 6, 180, 90);
            gpUp.AddArc(this.Width - 7, 0, 6, 6, 270, 90);
            gpUp.AddLine(this.Width - 1, split, 0, split);
            gpUp.CloseFigure();
            Color tempColor1 = Color.FromArgb(180, titleColorStart.R, titleColorStart.G, titleColorStart.B);
            Color tempColor2 = Color.FromArgb(250, titleColorEnd.R, titleColorEnd.G, titleColorEnd.B);
            LinearGradientBrush lineBrushUp = new LinearGradientBrush(new Point(this.Width / 2, -1), new Point(this.Width / 2, split), tempColor2, tempColor1);
            g.FillPath(lineBrushUp, gpUp);

            GraphicsPath gpDown = new GraphicsPath();
            gpDown.AddLine(0, split - 1, this.Width - 1, split - 1);
            gpDown.AddArc(this.Width - 7, 28 - 7, 6, 6, 0, 90);
            gpDown.AddArc(0, 28 - 7, 6, 6, 90, 90);
            gpDown.CloseFigure();
            LinearGradientBrush lineBrushDown = new LinearGradientBrush(new Point(this.Width / 2, split - 2), new Point(this.Width / 2, 28 + 1), titleColorStart, titleColorEnd);
            g.FillPath(lineBrushDown, gpDown);

            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(0, 0, 6, 6, 180, 90);
            gp.AddArc(this.Width - 7, 0, 6, 6, 270, 90);
            gp.AddArc(this.Width - 7, 28 - 7, 6, 6, 0, 90);
            gp.AddArc(0, 28 - 7, 6, 6, 90, 90);
            gp.CloseFigure();
            Pen pen = new Pen(borderColor);
            g.DrawPath(pen, gp);

            gpUp.Dispose();
            lineBrushUp.Dispose();
            gpDown.Dispose();
            lineBrushDown.Dispose();
            gp.Dispose();
            pen.Dispose();


            DrawTitleImage(g);
            DrawText(g);
        }

        private void DrawTitleImage(Graphics g)
        {
            if (titleImage != null)
                g.DrawImage(titleImage, 4, 3, 20, 20);
        }

        private void DrawText(Graphics g)
        {
            if (this.Text == "")
                return;

            int start = titleImage == null ? 6 : 28;
            int top = (int)((28 + 7 - g.MeasureString(this.Text, this.Font).Height) / 2) - 1;
            int height = (int)g.MeasureString(this.Text, this.Font).Height + 1;

            Pen pen1 = new Pen(foreBackColor);
            Brush brush1 = pen1.Brush;
            g.DrawString(this.Text, this.Font, brush1, new Rectangle(start - 1, top - 1, this.Width - 64 - start, height));
            g.DrawString(this.Text, this.Font, brush1, new Rectangle(start, top - 1, this.Width - 64 - start, height));
            g.DrawString(this.Text, this.Font, brush1, new Rectangle(start + 1, top - 1, this.Width - 64 - start, height));
            g.DrawString(this.Text, this.Font, brush1, new Rectangle(start + 1, top, this.Width - 64 - start, height));
            g.DrawString(this.Text, this.Font, brush1, new Rectangle(start + 1, top + 1, this.Width - 64 - start, height));
            g.DrawString(this.Text, this.Font, brush1, new Rectangle(start, top + 1, this.Width - 64 - start, height));
            g.DrawString(this.Text, this.Font, brush1, new Rectangle(start - 1, top + 1, this.Width - 64 - start, height));
            g.DrawString(this.Text, this.Font, brush1, new Rectangle(start - 1, top, this.Width - 64 - start, height));

            Pen pen = new Pen(this.ForeColor);
            Brush brush = pen.Brush;
            g.DrawString(this.Text, this.Font, brush, new Rectangle(start, top, this.Width - 64 - start, height));

            pen1.Dispose();
            brush1.Dispose();
            pen.Dispose();
            brush.Dispose();
        }

        private void DrawMin(Graphics g)
        {
            if (!this.ControlBox)
                return;

            if (!this.MinimizeBox && !this.MaximizeBox)
                return;

            g.SmoothingMode = SmoothingMode.Default;

            Pen pen;
            if (which == Which.Min && this.MinimizeBox)
            {
                if (state == MouseState.On)
                    pen = new Pen(ctrlBoxOn);
                else
                    pen = new Pen(ctrlBoxDown);
            }
            else if (!this.MinimizeBox)
            {
                pen = new Pen(ctrlBoxDisabled);
            }
            else
                pen = new Pen(ctrlBoxNormal);


            Brush brush = pen.Brush;
            Rectangle rect = new Rectangle(this.Width - 58, 15, 11, 3);
            g.FillRectangle(brush, rect);

            pen.Dispose();
            brush.Dispose();
        }

        private void DrawMax(Graphics g)
        {
            if (!this.ControlBox)
                return;

            if (!this.MinimizeBox && !this.MaximizeBox)
                return;

            g.SmoothingMode = SmoothingMode.Default;

            Rectangle rect = new Rectangle(this.Width - 37, 10, 10, 9);
            Pen pen;
            if (which == Which.Max && this.MaximizeBox)
            {
                if (state == MouseState.On)
                    pen = new Pen(ctrlBoxOn);
                else
                    pen = new Pen(ctrlBoxDown);
            }
            else if (!this.MaximizeBox)
            {
                pen = new Pen(ctrlBoxDisabled);

            }
            else
                pen = new Pen(ctrlBoxNormal);

            pen.Width = 2;
            g.DrawRectangle(pen, rect);

            if (!isMax)
            {
                g.DrawLine(pen, this.Width - 37, 14, this.Width - 31, 14);
                g.DrawLine(pen, this.Width - 31, 14, this.Width - 31, 20);
            }

            pen.Dispose();
        }

        private void DrawClose(Graphics g)
        {
            if (!this.ControlBox)
                return;

            g.SmoothingMode = SmoothingMode.Default;

            Pen pen;
            if (which == Which.Close)
            {
                if (state == MouseState.On)
                    pen = new Pen(ctrlBoxOn);
                else
                    pen = new Pen(ctrlBoxDown);
            }
            else
                pen = new Pen(ctrlBoxNormal);

            pen.Width = 2;
            g.DrawLine(pen, this.Width - 19, 9, this.Width - 9, 19);
            g.DrawLine(pen, this.Width - 19, 19, this.Width - 9, 9);

            pen.Dispose();
        }


        enum MouseState
        {
            Normal, On, Down
        }

        enum Which
        {
            Min, Max, Close, Non
        }

        MouseState state = MouseState.Normal;
        Which which = Which.Non;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (move)
            {
                this.Left += e.X - x;
                this.Top += e.Y - y;
            }
            else
            {
                which = Which.Non;
                state = MouseState.Normal;

                if (e.Y > 8 && e.Y < 22 && this.ControlBox && e.X > this.Width - 59 && e.X < this.Width - 7)
                {
                    if (e.X > this.Width - 59 && e.X < this.Width - 45 && this.MinimizeBox)
                    {
                        which = Which.Min;
                    }
                    else if (e.X > this.Width - 39 && e.X < this.Width - 25 && this.MaximizeBox)
                    {
                        which = Which.Max;
                    }
                    else if (e.X > this.Width - 21 && e.X < this.Width - 7)
                    {
                        which = Which.Close;
                    }

                    if (e.Button == MouseButtons.Left)
                        state = MouseState.Down;
                    else
                        state = MouseState.On;                   
                }

                this.Invalidate(new Rectangle(this.Width - 59, 8, 52, 14));
            }
        }

        int x, y;
        bool move = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            which = Which.Non;
            state = MouseState.Normal;
            bool on = false;

            if (e.Y > 8 && e.Y < 22 && e.X > this.Width - 59 && e.X < this.Width - 7 && e.Button == MouseButtons.Left && this.ControlBox)
            {
                if (e.X > this.Width - 59 && e.X < this.Width - 45 && this.MinimizeBox)
                {
                    which = Which.Min;
                    on = true;
                }
                else if (e.X > this.Width - 39 && e.X < this.Width - 25 && this.MaximizeBox)
                {
                    which = Which.Max;
                    on = true;
                }
                else if (e.X > this.Width - 21 && e.X < this.Width - 7)
                {
                    which = Which.Close;
                    on = true;
                }

                state = MouseState.Down;

                this.Invalidate();
            }

            if (e.Y > 0 && e.Y < 28 && e.Button == MouseButtons.Left && !on && WindowState != FormWindowState.Maximized)
            {
                x = e.X;
                y = e.Y;
                move = true;
            }
        }

        bool isMax = false;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (move)
            {
                move = false;
            }
            else if (e.Y > 8 && e.Y < 22 && e.Button == MouseButtons.Left && this.ControlBox)
            {
                if (e.X > this.Width - 59 && e.X < this.Width - 45 && this.MinimizeBox)
                {
                    this.WindowState = FormWindowState.Minimized;

                    isMax = !isMax;
                    which = Which.Non;
                    state = MouseState.Normal;

                    this.Invalidate();
                }
                else if (e.X > this.Width - 39 && e.X < this.Width - 25 && this.MaximizeBox)
                {
                    if (!isMax)
                        this.WindowState = FormWindowState.Maximized;
                    else
                        this.WindowState = FormWindowState.Normal;

                    isMax = !isMax;
                    which = Which.Non;
                    state = MouseState.Normal;

                    this.Invalidate();
                }
                else if (e.X > this.Width - 21 && e.X < this.Width - 7)
                {
                    this.Close();
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (e.Y < 28 && this.MaximizeBox)
            {
                if (WindowState == FormWindowState.Maximized)
                    WindowState = FormWindowState.Normal;
                else
                    WindowState = FormWindowState.Maximized;

                this.Invalidate();
            }
        }
    }
}
