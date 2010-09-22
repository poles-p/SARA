using System;

namespace SARA.Avi
{
    /// <summary>
    /// An exception that can be thrown by any function from <see cref="SARA.Avi"/> module.
    /// </summary>
    class AviException : Exception
    {
        /// <summary>
        /// Constructor that creates new AviException.
        /// </summary>
        /// <param name="message">
        /// Message that describes an error.
        /// </param>
        public AviException(string message) :
            base(message)
        {
        }
    }
}
