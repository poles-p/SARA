using System;
using System.Runtime.InteropServices;

namespace SARA.Avi.AviMarshal
{
    /// <summary>
    /// Marshal of BITMAPINFOHEADER structure
    /// </summary>
    /// <remarks>
    /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/aa930622.aspx">MSDN</a>.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        /// <summary>
        /// Specifies the size of the structure, in bytes.
        /// </summary>
        public UInt32 biSize;

        /// <summary>
        /// Specifies the width of the bitmap, in pixels.
        /// </summary>
        public Int32 biWidth;

        /// <summary>
        /// Specifies the height of the bitmap, in pixels.
        /// </summary>
        public Int32 biHeight;

        /// <summary>
        /// Specifies the number of planes for the target device.
        /// </summary>
        public UInt16 biPlanes;

        /// <summary>
        /// Specifies the number of bits per pixel.
        /// </summary>
        public UInt16 biBitCount;

        /// <summary>
        /// Specifies the type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed).
        /// </summary>
        public UInt32 biCompression;

        /// <summary>
        /// Specifies the size, in bytes, of the image.
        /// </summary>
        public UInt32 biSizeImage;

        /// <summary>
        /// Specifies the horizontal resolution, in pixels per meter, of the target device for the bitmap.
        /// </summary>
        public Int32 biXPelsPerMeter;

        /// <summary>
        /// Specifies the vertical resolution, in pixels per meter, of the target device for the bitmap.
        /// </summary>
        public Int32 biYPelsPerMeter;

        /// <summary>
        /// Specifies the number of color indexes in the color table that are actually used by the bitmap.
        /// </summary>
        public UInt32 biClrUsed;

        /// <summary>
        /// Specifies the number of color indexes required for displaying the bitmap.
        /// </summary>
        public UInt32 biClrImportant;
    }
}
