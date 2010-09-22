using System;
using SARA.Avi.AviMarshal;

namespace SARA.Avi
{
    /// <summary>
    /// Encapsulation of PAVISTREAM. Represents AVI stream.
    /// </summary>
    public abstract class AviStream
    {
        /// <summary>
        /// Handle to AviFil32 avi stream.
        /// </summary>
        protected IntPtr _aviStream = IntPtr.Zero;

        /// <summary>
        /// Stream info header.
        /// </summary>
        protected AVISTREAMINFO _streamInfo;

        /// <summary>
        /// Avi stream constructor.
        /// </summary>
        /// <param name="aviStream">
        /// Handle to AviFil32 avi stream.
        /// </param>
        /// <param name="streamInfo">
        /// Stream info header.
        /// </param>
        internal AviStream(IntPtr aviStream, AVISTREAMINFO streamInfo)
        {
            if (aviStream == IntPtr.Zero)
                throw new AviException("Can not create avi stream from null stream");

            _aviStream = aviStream;
            _streamInfo = streamInfo;
            AviFil32.AVIFileInit();
        }

        /// <summary>
        /// Avi stream destructor.
        /// </summary>
        ~AviStream()
        {
            if (_aviStream != IntPtr.Zero)
            {
                AviFil32.AVIStreamRelease(_aviStream);
                AviFil32.AVIFileExit();
            }
        }

        /// <summary>
        /// Length of stream in samples/frames.
        /// </summary>
        public int Length
        {
            get { return (int)_streamInfo.dwLength; }
        }
    }
}
