using System.Windows.Forms;
using System.Collections.Generic;

namespace MineSweeping
{
    public partial class GameForm : Form
    {
        public GameForm()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.UpdateStyles();

            this.DoubleBuffered = true;
        }

        Grid[,] gridList = new Grid[14, 14];
        internal int safeCount = 0;
        internal int mineCount = 0;

        void start()
        {
            safeCount = 0;

            this.gridPanel1.Controls.Clear();
            System.Random rand = new System.Random();

            //控件
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    Grid grid = new Grid();
                    grid.Name = "grid_" + i + "_" + "j";
                    grid.Pos = new System.Drawing.Point(i, j);
                    grid.MouseClick += new MouseEventHandler(grid_MouseClick);
                    grid.MouseLeave += new System.EventHandler(grid_MouseLeave);
                    grid.BeenChecked += new Grid.BeenCheckedHandler(grid_BeenChecked);

                    //雷                    
                    if (rand.Next(200) % 10 < (radioButton1.Checked ? 1 : radioButton2.Checked ? 2 : 3))
                        grid.IsMine = true;
                    else
                        safeCount++;

                    this.gridPanel1.Controls.Add(grid);

                    gridList[i, j] = new Grid();
                    gridList[i, j] = grid;
                }
            }

            mineCount = 14 * 14 - safeCount;
            this.label3.Text = "剩余雷数：" + mineCount.ToString();

            #region 数字
            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    Grid grid = gridList[i, j];
                    if (!grid.IsMine)
                        continue;

                    if (i > 0 && j > 0)//左上
                        gridList[i-1, j-1].Number++;
                    if (i > 0)//上
                        gridList[i - 1, j].Number++;
                    if (i >0 && j <13)//右上
                        gridList[i - 1, j + 1].Number++;
                    if (j < 13)//右
                        gridList[i, j + 1].Number++;
                    if (i <13 && j < 13)//右下
                        gridList[i + 1, j + 1].Number++;
                    if (i < 13)//下
                        gridList[i + 1, j].Number++;
                    if (i <13 && j >0)//左下
                        gridList[i + 1, j - 1].Number++;
                    if (j >0)//左
                        gridList[i, j-1].Number++;
                }
            }
            #endregion
        }

        void grid_BeenChecked(Grid sender)
        {
            if (sender.Pos.X > 0 && sender.Pos.Y > 0)//左上
            {
                if (!gridList[sender.Pos.X - 1, sender.Pos.Y - 1].Checked)
                    gridList[sender.Pos.X - 1, sender.Pos.Y - 1].MustBeCheck();
            }
            if (sender.Pos.X > 0)//上
            {
                if (!gridList[sender.Pos.X - 1, sender.Pos.Y].Checked)
                    gridList[sender.Pos.X - 1, sender.Pos.Y].MustBeCheck();
            }
            if (sender.Pos.X > 0 && sender.Pos.Y < 13)//右上
            {
                if (!gridList[sender.Pos.X - 1, sender.Pos.Y + 1].Checked)
                    gridList[sender.Pos.X - 1, sender.Pos.Y + 1].MustBeCheck();
            }
            if (sender.Pos.Y < 13)//右
            {
                if (!gridList[sender.Pos.X, sender.Pos.Y + 1].Checked)
                    gridList[sender.Pos.X, sender.Pos.Y + 1].MustBeCheck();
            }
            if (sender.Pos.X < 13 && sender.Pos.Y < 13)//右下
            {
                if (!gridList[sender.Pos.X + 1, sender.Pos.Y + 1].Checked)
                    gridList[sender.Pos.X + 1, sender.Pos.Y + 1].MustBeCheck();
            }
            if (sender.Pos.X < 13)//下
            {
                if (!gridList[sender.Pos.X + 1, sender.Pos.Y].Checked)
                    gridList[sender.Pos.X + 1, sender.Pos.Y].MustBeCheck();
            }
            if (sender.Pos.X < 13 && sender.Pos.Y > 0)//左下
            {
                if (!gridList[sender.Pos.X + 1, sender.Pos.Y - 1].Checked)
                    gridList[sender.Pos.X + 1, sender.Pos.Y - 1].MustBeCheck();
            }
            if (sender.Pos.Y > 0)//左
            {
                if (!gridList[sender.Pos.X, sender.Pos.Y - 1].Checked)
                    gridList[sender.Pos.X, sender.Pos.Y - 1].MustBeCheck();
            }
        }

        void grid_MouseLeave(object sender, System.EventArgs e)
        {
            Grid grid = sender as Grid;
            if (!grid.HasClick)
                return;

            grid.HasClick = false;

            #region 消除边框
            //左上
            if (grid.Pos.X == 0 && grid.Pos.Y == 0)
                grid.ClearBorder();
            else if (grid.Pos.Y == 0)
                gridList[grid.Pos.X - 1, grid.Pos.Y].ClearBorder();
            else if (grid.Pos.X == 0)
                gridList[grid.Pos.X, grid.Pos.Y - 1].ClearBorder();
            else
                gridList[grid.Pos.X - 1, grid.Pos.Y - 1].ClearBorder();
            //上
            if (grid.Pos.X == 0)
                grid.ClearBorder();
            else
                gridList[grid.Pos.X - 1, grid.Pos.Y].ClearBorder();
            //右上
            if (grid.Pos.X == 0 && grid.Pos.Y == 13)
                grid.ClearBorder();
            else if (grid.Pos.X == 0)
                gridList[grid.Pos.X, grid.Pos.Y + 1].ClearBorder();
            else if (grid.Pos.Y == 13)
                gridList[grid.Pos.X - 1, grid.Pos.Y].ClearBorder();
            else
                gridList[grid.Pos.X - 1, grid.Pos.Y + 1].ClearBorder();
            //右
            if (grid.Pos.Y == 13)
                grid.ClearBorder();
            else
                gridList[grid.Pos.X, grid.Pos.Y + 1].ClearBorder();
            //右下
            if (grid.Pos.X == 13 && grid.Pos.Y == 13)
                grid.ClearBorder();
            else if (grid.Pos.X == 13)
                gridList[grid.Pos.X, grid.Pos.Y + 1].ClearBorder();
            else if (grid.Pos.Y == 13)
                gridList[grid.Pos.X + 1, grid.Pos.Y].ClearBorder();
            else
                gridList[grid.Pos.X + 1, grid.Pos.Y + 1].ClearBorder();
            //下
            if (grid.Pos.X == 13)
                grid.ClearBorder();
            else
                gridList[grid.Pos.X + 1, grid.Pos.Y].ClearBorder();
            //左下
            if (grid.Pos.X == 13 && grid.Pos.Y == 0)
                grid.ClearBorder();
            else if (grid.Pos.X == 13)
                gridList[grid.Pos.X , grid.Pos.Y - 1].ClearBorder();
            else if (grid.Pos.Y == 0)
                gridList[grid.Pos.X+1, grid.Pos.Y].ClearBorder();
            else
                gridList[grid.Pos.X + 1, grid.Pos.Y - 1].ClearBorder();
            //左
            if (grid.Pos.Y == 0)
                grid.ClearBorder();
            else
                gridList[grid.Pos.X, grid.Pos.Y - 1].ClearBorder();
            #endregion
        }

        void grid_MouseClick(object sender, MouseEventArgs e)
        {
            Grid grid = sender as Grid;

            if (e.Button != MouseButtons.Left)
                return;

            if (grid.IsMine)
            {
                this.label2.Text = "...碰雷了...";

                grid.ClickTheMine = true;
                grid.Invalidate();

                for (int i = 0; i < 14; i++)
                {
                    for (int j=0; j<14; j++)
                    {
                        Grid temp = gridList[i, j];

                        if (temp.IsMine)
                        {
                            temp.Checked = true;
                            temp.Invalidate();
                        }
                    }
                }

                return;
            }

            if (grid.Checked && grid.Number>0)
            {
                #region 画边框
                //左上
                if (grid.Pos.X == 0 && grid.Pos.Y == 0)
                    grid.DrawBorderAlign(Grid.DrawAlign.TopLeft);
                else if (grid.Pos.Y == 0)
                    gridList[grid.Pos.X - 1, grid.Pos.Y].DrawBorderAlign(Grid.DrawAlign.TopLeft);
                else if (grid.Pos.X == 0)
                    gridList[grid.Pos.X, grid.Pos.Y - 1].DrawBorderAlign(Grid.DrawAlign.TopLeft);
                else
                    gridList[grid.Pos.X - 1, grid.Pos.Y - 1].DrawBorderAlign(Grid.DrawAlign.TopLeft);
                //上
                if (grid.Pos.X != 0 && gridList[grid.Pos.X - 1, grid.Pos.Y].DrawType == Grid.DrawAlign.None)
                    gridList[grid.Pos.X - 1, grid.Pos.Y].DrawBorderAlign(Grid.DrawAlign.Top);
                else if (grid.Pos.X == 0 && grid.DrawType== Grid.DrawAlign.None)
                    grid.DrawBorderAlign(Grid.DrawAlign.Top);
                //右上
                if (grid.Pos.X == 0 && grid.Pos.Y == 13)
                    grid.DrawBorderAlign(Grid.DrawAlign.TopRight);
                else if (grid.Pos.X == 0)
                    gridList[grid.Pos.X, grid.Pos.Y + 1].DrawBorderAlign(Grid.DrawAlign.TopRight);
                else if (grid.Pos.Y == 13)
                    gridList[grid.Pos.X - 1, grid.Pos.Y].DrawBorderAlign(Grid.DrawAlign.TopRight);
                else
                    gridList[grid.Pos.X - 1, grid.Pos.Y + 1].DrawBorderAlign(Grid.DrawAlign.TopRight);
                //右
                if (grid.Pos.Y != 13 && gridList[grid.Pos.X, grid.Pos.Y + 1].DrawType == Grid.DrawAlign.None)
                    gridList[grid.Pos.X, grid.Pos.Y + 1].DrawBorderAlign(Grid.DrawAlign.Right);
                else if (grid.Pos.Y == 13 && grid.DrawType == Grid.DrawAlign.None)
                    grid.DrawBorderAlign(Grid.DrawAlign.Right);
                //右下
                if (grid.Pos.X == 13 && grid.Pos.Y == 13)
                    grid.DrawBorderAlign(Grid.DrawAlign.BottomRight);
                else if (grid.Pos.X == 13)
                    gridList[grid.Pos.X, grid.Pos.Y + 1].DrawBorderAlign(Grid.DrawAlign.BottomRight);
                else if (grid.Pos.Y == 13)
                    gridList[grid.Pos.X + 1, grid.Pos.Y].DrawBorderAlign(Grid.DrawAlign.BottomRight);
                else
                    gridList[grid.Pos.X + 1, grid.Pos.Y + 1].DrawBorderAlign(Grid.DrawAlign.BottomRight);
                //下
                if (grid.Pos.X != 13 && gridList[grid.Pos.X + 1, grid.Pos.Y].DrawType == Grid.DrawAlign.None)
                    gridList[grid.Pos.X + 1, grid.Pos.Y].DrawBorderAlign(Grid.DrawAlign.Bottom);
                else if (grid.Pos.X == 13 && grid.DrawType == Grid.DrawAlign.None)
                    grid.DrawBorderAlign(Grid.DrawAlign.Bottom);
                //左下
                if (grid.Pos.X == 13 && grid.Pos.Y == 0)
                    grid.DrawBorderAlign(Grid.DrawAlign.BottomLeft);
                else if (grid.Pos.X == 13)
                    gridList[grid.Pos.X, grid.Pos.Y-1].DrawBorderAlign(Grid.DrawAlign.BottomLeft);
                else if (grid.Pos.Y == 0)
                    gridList[grid.Pos.X+1, grid.Pos.Y].DrawBorderAlign(Grid.DrawAlign.BottomLeft);
                else
                    gridList[grid.Pos.X + 1, grid.Pos.Y - 1].DrawBorderAlign(Grid.DrawAlign.BottomLeft);
                //左
                if (grid.Pos.Y != 0 && gridList[grid.Pos.X, grid.Pos.Y - 1].DrawType == Grid.DrawAlign.None)
                    gridList[grid.Pos.X, grid.Pos.Y - 1].DrawBorderAlign(Grid.DrawAlign.Left);
                else if (grid.Pos.Y == 0 && grid.DrawType == Grid.DrawAlign.None)
                    grid.DrawBorderAlign(Grid.DrawAlign.Left);
                #endregion
            }

            if (!grid.Checked)
            {
                safeCount--;
                if (safeCount == 0)
                    this.label2.Text = "胜利啦~~~~";
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.label2.Text = "要小心哦...";
            start();
        }
    }
}
