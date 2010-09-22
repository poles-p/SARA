using System.Drawing;
using System.Windows.Forms;
using SARA.Core;

namespace SARA.SimpleGUI.Plot
{
    /// <summary>
    /// Transformation using to convert data to coordinates on plot.
    /// </summary>
    /// <param name="data">
    /// Data point to transform.
    /// </param>
    /// <returns>
    /// Image coordinates of point on plot.
    /// </returns>
    public delegate Point PlotTransform(Vector2D data);

    /// <summary>
    /// Form to drawing plots.
    /// </summary>
    public class PlotForm
        : Form
    {
        private PlotPanel _plotPanel = new PlotPanel();

        private int _leftMargin = 100;
        private int _rightMargin = 20;
        private int _topMargin = 20;
        private int _bottomMargin = 60;

        /// <summary>
        /// Create new <see cref="PlotForm"/>.
        /// </summary>
        public PlotForm()
        {
            SuspendLayout();
            _plotPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            _plotPanel.Size = new Size(ClientSize.Width - _leftMargin - _rightMargin, ClientSize.Height - _topMargin - _bottomMargin);
            _plotPanel.Location = new Point(_leftMargin, _topMargin);
            Controls.Add(_plotPanel);
            ResumeLayout();
            PerformLayout();
        }

        /// <summary>
        /// Add data set to draw on plot.
        /// </summary>
        /// <param name="dataSet">
        /// Data set to draw on plot.
        /// </param>
        public void Add(DataSet dataSet)
        {
            _plotPanel.DataSets.Add(dataSet);
        }

        /// <summary>
        /// X coordinate of left edge of plot.
        /// </summary>
        public float MinX
        {
            get { return _plotPanel.MinX; }
            set
            {
                _plotPanel.MinX = value;
                Refresh();
            }
        }

        /// <summary>
        /// X coordinate of right edge of plot.
        /// </summary>
        public float MaxX
        {
            get { return _plotPanel.MaxX; }
            set
            {
                _plotPanel.MaxX = value;
                Refresh();
            }
        }

        /// <summary>
        /// Y coordinate of bottom edge of plot.
        /// </summary>
        public float MinY
        {
            get { return _plotPanel.MinY; }
            set
            {
                _plotPanel.MinY = value;
                Refresh();
            }
        }

        /// <summary>
        /// Y coordinate of top edge of plot.
        /// </summary>
        public float MaxY
        {
            get { return _plotPanel.MaxY; }
            set
            {
                _plotPanel.MaxY = value;
                Refresh();
            }
        }

        /// <summary>
        /// Paint the plot.
        /// </summary>
        /// <param name="e">
        /// A <see cref="PaintEventArgs"/> that contains the event data. 
        /// </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }
}
