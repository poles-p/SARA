using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SARA.Avi.AviMarshal;
using SARA.Core;

namespace SARA.Avi
{
    /// <summary>
    /// AVI video stream.
    /// </summary>
    public class VideoStream
        : AviStream
    {
        /// <summary>
        /// VideoStream constructor.
        /// </summary>
        /// <param name="aviStream">
        /// Handle to AviFil32 avi stream.
        /// </param>
        /// <param name="streamInfo">
        /// Stream info header.
        /// </param>
        internal VideoStream(IntPtr aviStream, AVISTREAMINFO streamInfo)
            : base(aviStream, streamInfo)
        {
            if (streamInfo.fccType != AviFil32.StreamType.VIDEO)
            {
                AviFil32.AVIStreamRelease(_aviStream);
                AviFil32.AVIFileExit();
                _aviStream = IntPtr.Zero;

                throw new AviException("Can not create video stream from not video stream.");
            }
        }

        /// <summary>
        /// Get FrameGrabber to grab frames from AVI.
        /// </summary>
        /// <returns>
        /// new FrameGrabber object.
        /// </returns>
        public FrameGrabber GetFrameGrabber()
        {
            int formatSize = 0;
            BITMAPINFOHEADER bmpHeader = new BITMAPINFOHEADER();
            BITMAPINFOHEADER bmpHeaderOut = new BITMAPINFOHEADER();

            AviFil32.AVIError error = AviFil32.AVIStreamReadFormat(_aviStream, 0, IntPtr.Zero, ref formatSize);
            if (error != AviFil32.AVIError.AVIERR_OK)
                throw new AviException("Unrecognised AVI exception (0x" + Convert.ToString((int)error, 16) + ").");
            if (formatSize > Marshal.SizeOf(typeof(BITMAPINFOHEADER)))
                throw new AviException("Unknown format (too big).");

            error = AviFil32.AVIStreamReadFormat(_aviStream, 0, ref bmpHeader, ref formatSize);
            if (error != AviFil32.AVIError.AVIERR_OK)
                throw new AviException("Unrecognised AVI exception (0x" + Convert.ToString((int)error, 16) + ").");

            bmpHeaderOut.biSize          = bmpHeader.biSize;
            bmpHeaderOut.biWidth         = bmpHeader.biWidth;
            bmpHeaderOut.biHeight        = bmpHeader.biHeight;
            bmpHeaderOut.biPlanes        = 1;
            bmpHeaderOut.biBitCount      = 24;
            bmpHeaderOut.biCompression   = 0; // RGB
            bmpHeaderOut.biXPelsPerMeter = bmpHeader.biXPelsPerMeter;
            bmpHeaderOut.biYPelsPerMeter = bmpHeader.biYPelsPerMeter;

            IntPtr getFrameObject = AviFil32.AVIStreamGetFrameOpen(_aviStream, ref bmpHeaderOut);

            if (getFrameObject == IntPtr.Zero)
                throw new AviException("Can not find video decompressor.");

            return new FrameGrabber(getFrameObject, _streamInfo);
        }

        /// <summary>
        /// Get all frames from AVI file.
        /// </summary>
        /// <returns>
        /// Sequence of all frames in AVI file.
        /// </returns>
        public IEnumerable<FloatMatrix> GetAllFrames()
        {
            return GetFrameGrabber().GetAllFrames();
        }
    }
}
