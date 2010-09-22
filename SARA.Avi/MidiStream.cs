using System;
using SARA.Avi.AviMarshal;

namespace SARA.Avi
{
    /// <summary>
    /// AVI MIDI stream.
    /// </summary>
    public class MidiStream
        : AviStream
    {
        /// <summary>
        /// MidiStream constructor.
        /// </summary>
        /// <param name="aviStream">
        /// Handle to AviFil32 avi stream.
        /// </param>
        /// <param name="streamInfo">
        /// Stream info header.
        /// </param>
        internal MidiStream(IntPtr aviStream, AVISTREAMINFO streamInfo)
            : base(aviStream, streamInfo)
        {
            if (streamInfo.fccType != AviFil32.StreamType.MIDI)
            {
                AviFil32.AVIStreamRelease(_aviStream);
                AviFil32.AVIFileExit();
                _aviStream = IntPtr.Zero;

                throw new AviException("Can not create midi stream from not midi stream.");
            }
        }
    }
}
