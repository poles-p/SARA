using SARA.Core;

namespace SARA.SimpleGUI.Plot
{
    /// <summary>
    /// Data set to plot.
    /// </summary>
    public class DataSet
    {
        private Vector2D[] _data;
        private IPlotPoint _plotPoint;

        /// <summary>
        /// Create new DataSet.
        /// </summary>
        /// <param name="data">
        /// Data to plot.
        /// </param>
        /// <param name="plotPoint">
        /// Point type used on plot.
        /// </param>
        public DataSet(Vector2D[] data, IPlotPoint plotPoint)
        {
            _data = data;
            _plotPoint = plotPoint;
        }

        /// <summary>
        /// Data to plot.
        /// </summary>
        public Vector2D[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Point type used on plot.
        /// </summary>
        public IPlotPoint PlotPoint
        {
            get { return _plotPoint; }
            set { _plotPoint = value; }
        }
    }
}
