using System;

namespace SARA.FITS
{
    /// <summary>
    /// An exception that can be throwed by <see cref="FitsHeaderEntry"/>.
    /// </summary>
    public class FitsHeaderEntryException : Exception
    {
        /// <summary>
        /// <see cref="FitsHeaderEntryException"/> constructor.
        /// </summary>
        /// <param name="message">
        /// Message that describes an error.
        /// </param>
        public FitsHeaderEntryException(string message)
            : base(message)
        {
        }
    }
}
