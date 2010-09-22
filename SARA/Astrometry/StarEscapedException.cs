
namespace SARA.Astrometry
{
    /// <summary>
    /// An exception throwed when star escape out of image.
    /// </summary>
    /// <remarks>
    /// This exception can be throwed when IStarTracker lost star, because it escapes out of image.
    /// </remarks>
    public class StarEscapedException : StarTrackerException
    {
        /// <summary>
        /// Constructor that creates new StarEscapeException.
        /// <param name="message">
        /// </param>
        /// Message that describes an error.
        /// </summary>
        public StarEscapedException(string message) :
            base(message)
        {
        }
    }
}
