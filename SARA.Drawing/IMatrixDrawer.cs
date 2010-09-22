using System.Drawing;
using SARA.Core;

namespace SARA.Drawing
{
    /// <summary>
    /// Converter between <see cref="FloatMatrix"/> and <see cref="Bitmap"/>, used to draw matrices.
    /// </summary>
    public interface IMatrixDrawer
    {
        /// <summary>
        /// Convert <see cref="FloatMatrix"/> to <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="matrix">
        /// Matrix to convert.
        /// </param>
        /// <returns>
        /// <see cref="Bitmap"/> that represents specified matrix.
        /// </returns>
        Bitmap ToBitmap(FloatMatrix matrix);
    }
}
