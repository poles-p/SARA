using System;
using System.Runtime.InteropServices;

namespace SARA.Avi.AviMarshal
{
    /// <summary>
    /// Marshal of AVISTREAMINFO structure. Contains information for a single stream.
    /// </summary>
    /// <remarks>
    /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756832(VS.85).aspx">MSDN</a>.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AVISTREAMINFO
    {
        /// <summary>
        /// Four-character code indicating the stream type.
        /// </summary>
        public AviFil32.StreamType fccType;

        /// <summary>
        /// Four-character code of the compressor handler that will compress this video stream when it is saved.
        /// </summary>
        public UInt32 fccHandler;

        /// <summary>
        /// Applicable flags for the stream.
        /// </summary>
        public UInt32 dwFlags;

        /// <summary>
        /// Capability flags; currently unused.
        /// </summary>
        public UInt32 dwCaps;

        /// <summary>
        /// Priority of the stream.
        /// </summary>
        public UInt16 wPriority;

        /// <summary>
        /// Language of the stream.
        /// </summary>
        public UInt16 wLanguage;

        /// <summary>
        /// Time scale applicable for the stream.
        /// </summary>
        public UInt32 dwScale;

        /// <summary>
        /// Rate in an integer format.
        /// </summary>
        public UInt32 dwRate;

        /// <summary>
        /// Sample number of the first frame of the AVI file.
        /// </summary>
        public UInt32 dwStart;

        /// <summary>
        /// Length of this stream.
        /// </summary>
        public UInt32 dwLength;

        /// <summary>
        /// Audio skew.
        /// </summary>
        public UInt32 dwInitialFrames;

        /// <summary>
        /// Recommended buffer size, in bytes, for the stream.
        /// </summary>
        public UInt32 dwSuggestedBufferSize;

        /// <summary>
        /// Quality indicator of the video data in the stream.
        /// </summary>
        public UInt32 dwQuality;

        /// <summary>
        /// Size, in bytes, of a single data sample.
        /// </summary>
        public UInt32 dwSampleSize;

        /// <summary>
        /// Dimensions of the video destination rectangle.
        /// </summary>
        public RECT rcFrame;

        /// <summary>
        /// Number of times the stream has been edited.
        /// </summary>
        public UInt32 dwEditCount;

        /// <summary>
        /// Number of times the stream format has changed.
        /// </summary>
        public UInt32 dwFormatChangeCount;

        /// <summary>
        /// Number of times the stream format has changed.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public UInt16[] szName;
    }
}
