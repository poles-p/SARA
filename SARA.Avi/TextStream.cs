using System;
using SARA.Avi.AviMarshal;

namespace SARA.Avi
{
    /// <summary>
    /// AVI text stream.
    /// </summary>
    public class TextStream
        : AviStream
    {
        /// <summary>
        /// TextStream constructor.
        /// </summary>
        /// <param name="aviStream">
        /// Handle to AviFil32 avi stream.
        /// </param>
        /// <param name="streamInfo">
        /// Stream info header.
        /// </param>
        internal TextStream(IntPtr aviStream, AVISTREAMINFO streamInfo)
            : base(aviStream, streamInfo)
        {
            if (streamInfo.fccType != AviFil32.StreamType.TEXT)
            {
                AviFil32.AVIStreamRelease(_aviStream);
                AviFil32.AVIFileExit();
                _aviStream = IntPtr.Zero;

                throw new AviException("Can not create text stream from not text stream.");
            }
        }
    }
}
