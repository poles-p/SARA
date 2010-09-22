using System.Drawing;
using SARA.Core;

namespace SARA.SimpleGUI.Plot
{
    /// <summary>
    /// Interface representing type of ploints using to draw data.
    /// </summary>
    public interface IPlotPoint
    {
        /// <summary>
        /// Draw point on plot.
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
        void DrawPoint(Graphics graphics, Vector2D data, PlotTransform plotTransform);
    }
}
