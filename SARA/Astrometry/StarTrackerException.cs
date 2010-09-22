using System;

namespace SARA.Astrometry
{
    /// <summary>
    /// An exception that can be throwed by star tracker.
    /// </summary>
    /// <remarks>
    /// This exception can be throwed by <see cref="IStarTracker"/>.
    /// </remarks>
    public class StarTrackerException : Exception
    {
        /// <summary>
        /// Constructor that creates new StarTrackerException
        /// </summary>
        /// <param name="message">
        /// Message that describes an error.
        /// </param>
        public StarTrackerException(string message) :
            base(message)
        {
        }
    }
}
