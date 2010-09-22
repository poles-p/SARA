using SARA.Core;

namespace SARA.Fotometry
{
    /// <summary>
    /// Aperture interface for aperture fotometry.
    /// </summary>
    public interface IAperture
    {
        /// <summary>
        /// Get result of fotometry on specified image.
        /// </summary>
        /// <param name="image">
        /// Source image for fotometry.
        /// </param>
        /// <returns>
        /// Result of fotometry.
        /// </returns>
        FotometryResult GetResult(FloatMatrix image);
    }
}
