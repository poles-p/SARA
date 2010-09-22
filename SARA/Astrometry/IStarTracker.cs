using SARA.Core;

namespace SARA.Astrometry
{
    /// <summary>
    /// Star tracker interface
    /// </summary>
    /// <remarks>
    /// Star tracker track star on images
    /// </remarks>
    public interface IStarTracker
    {
        /// <summary>
        /// Track star method
        /// </summary>
        /// <remarks>
        /// Find selected star on image. Updates Position property.
        /// </remarks>
        /// <param name="image">
        /// An image to find selected star.
        /// </param>
        void Track(FloatMatrix image);

        /// <summary>
        /// Last position of selected star.
        /// </summary>
        /// <remarks>
        /// Value should be updated every Track method call.
        /// </remarks>
        Vector2D Position
        {
            get;
        }
    }
}
