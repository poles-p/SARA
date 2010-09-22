using System;

namespace SARA.Fotometry
{
    /// <summary>
    /// An exception that can be throwed at fotometry.
    /// </summary>
    public class FotometryException : Exception
    {
        /// <summary>
        /// <see cref="FotometryException"/> constructor.
        /// </summary>
        /// <param name="message">
        /// Message that describes an error.
        /// </param>
        public FotometryException(string message) :
            base(message)
        {
        }
    }
}
