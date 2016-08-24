using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MineSweeping
{
    internal partial class Grid : UserControl
    {
        internal Grid()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();

            this.DoubleBuffered = true;
            this.BackColor = Color.Transparent;


            this.Checked = false;
            this.mouseMove = false;
            this.Number = 0;
            DrawType = DrawAlign.None;
            flag = Flag.None;
        }


        internal bool Checked { get; set; }
        internal bool IsMine { get; set; }
        bool mouseMove;
        internal int Number { get; set; }
        internal Point Pos { get; set; }
        internal DrawAlign DrawType { get; set; }
        bool havetoDrawBorder = false;
        internal bool ClickTheMine { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (!this.Checked)
            {
                e.Graphics.FillRectangle(new Pen(Color.FromArgb(120, Color.WhiteSmoke)).Brush, this.ClientRectangle);

                if (mouseMove)
                {
                    Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                    e.Graphics.DrawRectangle(Pens.Azure, rect);
                }

                switch (flag)
                {
                    case Flag.Red:
                        e.Graphics.DrawImage(Properties.Resources.flag_red, 4, 4, 16, 16);
                        break;
                    case Flag.DontKnow:
                        e.Graphics.DrawString("?", new Font("Times New Roman", 16, FontStyle.Bold), Brushes.SteelBlue, 4, 0);
                        break;
                    case Flag.None:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (this.IsMine)
                {
                    GraphicsPath gp = new GraphicsPath();
                    gp.AddEllipse(new Rectangle(3, 3, this.Width - 6, this.Height - 6));
                    PathGradientBrush pathBrush = new PathGradientBrush(gp);
                    pathBrush.CenterColor = Color.DarkGray;
                    pathBrush.SurroundColors = new Color[] { Color.Black };
                    e.Graphics.FillPath(pathBrush, gp);
                    e.Graphics.DrawPath(Pens.Gainsboro, gp);

                    if (ClickTheMine)
                        e.Graphics.DrawImage(Properties.Resources.cross, 4, 4, 16, 16);
                }
                else
                {
                    if (this.Number != 0)
                    {
                        string text = this.Number.ToString();
                        Font font = new Font("Times New Roman", 12, FontStyle.Bold);
                        Brush brush = Brushes.Azure;
                        int left = (int)(this.Width - e.Graphics.MeasureString(text, font).Width) / 2;
                        int top = (int)(this.Height - e.Graphics.MeasureString(text, font).Height) / 2;

                        e.Graphics.DrawString(text, font, brush, left - 1, top - 1);
                        e.Graphics.DrawString(text, font, brush, left, top - 1);
                        e.Graphics.DrawString(text, font, brush, left + 1, top - 1);
                        e.Graphics.DrawString(text, font, brush, left + 1, top);
                        e.Graphics.DrawString(text, font, brush, left + 1, top + 1);
                        e.Graphics.DrawString(text, font, brush, left, top + 1);
                        e.Graphics.DrawString(text, font, brush, left - 1, top + 1);
                        e.Graphics.DrawString(text, font, brush, left - 1, top);

                        e.Graphics.DrawString(text, font, Brushes.SteelBlue, left, top);
                    }
                }
            }

            if (havetoDrawBorder)
                drawBorder(e.Graphics);
        }

        void drawBorder(Graphics g)
        {
            Pen pen = new Pen(Color.White);
            pen.Width = 2;

            switch (this.DrawType)
            {
                case DrawAlign.Left:
                    g.DrawLine(pen, 1, 1, 1, this.Height - 2);
                    break;
                case DrawAlign.TopLeft:
                    g.DrawLine(pen, 1, 1, 1, this.Height - 2);
                    g.DrawLine(pen, 1, 1, this.Width - 2, 1);
                    break;
                case DrawAlign.Top:
                    g.DrawLine(pen, 1, 1, this.Width - 2, 1);
                    break;
                case DrawAlign.TopRight:
                    g.DrawLine(pen, 1, 1, this.Width - 2, 1);
                    g.DrawLine(pen, this.Width - 2, 1, this.Width - 2, this.Height - 2);
                    break;
                case DrawAlign.Right:
                    g.DrawLine(pen, this.Width - 2, 1, this.Width - 2, this.Height - 2);
                    break;
                case DrawAlign.BottomRight:
                    g.DrawLine(pen, this.Width - 2, 1, this.Width - 2, this.Height - 2);
                    g.DrawLine(pen, 1, this.Height - 2, this.Width - 2, this.Height - 2);
                    break;
                case DrawAlign.Bottom:
                    g.DrawLine(pen, 1, this.Height - 2, this.Width - 2, this.Height - 2);
                    break;
                case DrawAlign.BottomLeft:
                    g.DrawLine(pen, 1, this.Height - 2, this.Width - 2, this.Height - 2);
                    g.DrawLine(pen, 1, 1, 1, this.Height - 2);
                    break;
                default:
                    break;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!mouseMove)
            {
                mouseMove = true;
                this.Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            mouseMove = false;
            this.Invalidate();
        }

        internal bool HasClick { get; set; }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button== MouseButtons.Left)
            {
                if (this.Checked)
                    HasClick = true;
                else
                {
                    this.Checked = true;
                    this.Invalidate();

                    if (flag == Flag.Red)
                    {
                        GameForm parent = this.ParentForm as GameForm;
                        int n = ++parent.mineCount;
                        parent.label3.Text = "剩余雷数：" + n.ToString();
                    }

                    MustBeCheck();
                }
            }
            else if (!this.Checked && e.Button == MouseButtons.Right)
            {
                GameForm parent=this.ParentForm as GameForm;
                int n = 0;
                switch (flag)
                {
                    case Flag.Red:
                        flag = Flag.DontKnow;
                        n = ++parent.mineCount;
                        parent.label3.Text = "剩余雷数：" + n.ToString();
                        break;
                    case Flag.DontKnow:
                        flag = Flag.None;
                        break;
                    case Flag.None:
                        flag = Flag.Red;
                        n = --parent.mineCount;
                        parent.label3.Text = "剩余雷数：" + n.ToString();
                        break;
                    default:
                        break;
                }

                this.Invalidate();
            }
        }

        internal enum DrawAlign
        {
            Left,
            TopLeft,
            Top,
            TopRight,
            Right,
            BottomRight,
            Bottom,
            BottomLeft,
            None
        }

        internal void DrawBorderAlign(DrawAlign Align)
        {
            this.DrawType = Align;
            havetoDrawBorder = true;
            this.Invalidate();
        }

        internal void ClearBorder()
        {
            havetoDrawBorder = false;
            DrawType = DrawAlign.None;
            this.Invalidate();
        }

        internal delegate void BeenCheckedHandler(Grid sender);
        internal event BeenCheckedHandler BeenChecked;

        internal void MustBeCheck()
        {
            if (!this.IsMine)
            {
                if (!this.Checked)
                {
                    this.Checked = true;
                    this.Invalidate();
                    (this.ParentForm as GameForm).safeCount--;
                }

                if (this.Number == 0)
                {
                    if (BeenChecked != null)
                        BeenChecked(this);
                }
            }
        }

        Flag flag;
        enum Flag
        {
            Red,
            DontKnow,
            None
        }
    }
}
