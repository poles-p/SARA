using System.Drawing;
using SARA.Core;

namespace SARA.SimpleGUI.Plot
{
    /// <summary>
    /// Point on plot, that look like cross.
    /// </summary>
    public class PlotPointCross : IPlotPoint
    {
        private int _size;
        private Pen _pen;

        /// <summary>
        /// Create new <see cref="PlotPointCross"/> with default size (10) and color (blue).
        /// </summary>
        public PlotPointCross()
        {
            _size = 10;
            _pen = new Pen(Color.Blue);
        }

        /// <summary>
        /// Create new <see cref="PlotPointCross"/> with default size (10).
        /// </summary>
        /// <param name="color">
        /// Color of cross.
        /// </param>
        public PlotPointCross(Color color)
        {
            _size = 10;
            _pen = new Pen(color);
        }

        /// <summary>
        /// Create new <see cref="PlotPointCross"/>
        /// </summary>
        /// <param name="size">
        /// Size of cross (in pixels).
        /// </param>
        /// <param name="color">
        /// Color of cross.
        /// </param>
        public PlotPointCross(int size, Color color)
        {
            _size = size;
            _pen = new Pen(color);
        }

        /// <summary>
        /// Size of cross in pixels.
        /// </summary>
        public int Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// Color of cross.
        /// </summary>
        public Color Color
        {
            get { return _pen.Color; }
            set { _pen.Color = value; }
        }

        #region IPlotPoint Members

        /// <summary>
        /// Draw cross on plot.
        /// </summary>
        /// <param name="graphics">
        /// Plot drawing surface.
        /// </param>
        /// <param name="data">
        /// Data point to transform and draw.
        /// </param>
        /// <param name="plotTransform">
        /// Transformation between data coordinates and image coordinates on plot.
        /// </param>
        public void DrawPoint(Graphics graphics, Vector2D data, PlotTransform plotTransform)
        {
            Point center = plotTransform(data);
            graphics.DrawLine(_pen, center.X - _size, center.Y, center.X + _size, center.Y);
            graphics.DrawLine(_pen, center.X, center.Y - _size, center.X, center.Y + _size);
        }

        #endregion
    }
}
