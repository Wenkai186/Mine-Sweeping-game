using System.Windows.Forms;

namespace MineSweeping
{
    public partial class GridPanel : Panel
    {
        public GridPanel()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();

            this.DoubleBuffered = true;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);

            Grid grid = e.Control as Grid;
            if (grid == null)
                return;

            int count = this.Controls.Count-1;
            grid.Checked = false;
            grid.Location = new System.Drawing.Point(count % 14 * 24, count / 14 * 24);
        }
    }
}
