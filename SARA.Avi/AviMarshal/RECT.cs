using System;
using System.Runtime.InteropServices;

namespace SARA.Avi.AviMarshal
{
    /// <summary>
    /// Marshal of RECT structure.
    /// </summary>
    /// <remarks>
    /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd162897(VS.85).aspx">MSDN</a>.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RECT
    {
        /// <summary>
        /// The x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public Int32 left;

        /// <summary>
        /// The y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public Int32 top;

        /// <summary>
        /// The x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public Int32 right;

        /// <summary>
        /// The y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public Int32 bottom;
    }
}
