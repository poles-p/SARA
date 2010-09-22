using System;
using SARA.Avi.AviMarshal;

namespace SARA.Avi
{
    /// <summary>
    /// AVI audio stream.
    /// </summary>
    public class AudioStream
        : AviStream
    {
        /// <summary>
        /// AudioStream constructor.
        /// </summary>
        /// <param name="aviStream">
        /// Handle to AviFil32 avi stream.
        /// </param>
        /// <param name="streamInfo">
        /// Stream info header.
        /// </param>
        internal AudioStream(IntPtr aviStream, AVISTREAMINFO streamInfo)
            : base(aviStream, streamInfo)
        {
            if (streamInfo.fccType != AviFil32.StreamType.AUDIO)
            {
                AviFil32.AVIStreamRelease(_aviStream);
                AviFil32.AVIFileExit();
                _aviStream = IntPtr.Zero;

                throw new AviException("Can not create audio stream from not audio stream.");
            }
        }
    }
}
