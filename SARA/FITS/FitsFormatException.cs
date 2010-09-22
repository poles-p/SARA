using System;

namespace SARA.FITS
{
    /// <summary>
    /// An exception that is throwed when format of FITS file or data is invalid.
    /// </summary>
    public class FitsFormatException : FormatException
    {
        /// <summary>
        /// <see cref="FitsFormatException"/> constructor.
        /// </summary>
        /// <param name="message">
        /// Message that describes an error.
        /// </param>
        public FitsFormatException(string message)
            : base(message)
        {
        }
    }
}
