using System;
using System.Runtime.InteropServices;

namespace SARA.Avi.AviMarshal
{
    /// <summary>
    /// Marshal of avifil32.dll library
    /// </summary>
    public static class AviFil32
    {
        /// <summary>
        /// Error codes that can be returned by avifil32.dll functions.
        /// </summary>
        public enum AVIError
        {
            /// <summary>
            /// No error occured.
            /// </summary>
            AVIERR_OK = 0,

            /// <summary>
            /// The file couldn't be read, indicating a corrupt file or an unrecognized format.
            /// </summary>
            AVIERR_BADFORMAT    = unchecked( (int)0x80044066 ),

            /// <summary>
            /// The file could not be opened because of insufficient memory.
            /// </summary>
            AVIERR_MEMORY       = unchecked( (int)0x80044067 ),

            /// <summary>
            /// A disk error occurred while reading the file.
            /// </summary>
            AVIERR_FILEREAD     = unchecked( (int)0x8004406d ),

            /// <summary>
            /// A disk error occurred while opening the file.
            /// </summary>
            AVIERR_FILEOPEN     = unchecked( (int)0x8004406f ),

            /// <summary>
            /// The file does not contains requested data.
            /// </summary>
            AVIERR_NODATA       = unchecked( (int)0x80044073 ),

            /// <summary>
            /// According to the registry, the type of file does not have a handler to process it.
            /// </summary>
            REGDB_E_CLASSNOTREG = unchecked( (int)0x80040154 )
        }

        /// <summary>
        /// Access mode to use when opening the AVI file.
        /// </summary>
        public enum AVIFileOpenMode
        {
            /// <summary>
            /// Creates a new file. If the file already exists, it is truncated to zero length.
            /// </summary>
            OF_CREATE           = 0x00001000,

            /// <summary>
            /// Skips time-consuming operations, such as building an index. Set this flag if you want the function to 
            /// return as quickly as possible—for example, if you are going to query the file properties but not read the file.
            /// </summary>
            OF_PARSE            = 0x00000100,

            /// <summary>
            /// Opens the file for reading.
            /// </summary>
            OF_READ             = 0x00000000,

            /// <summary>
            /// Opens the file for reading and writing.
            /// </summary>
            OF_READWRITE        = 0x00000002,

            /// <summary>
            /// Opens the file nonexclusively. Other processes can open the file with read or write access. 
            /// AVIFileOpen fails if another process has opened the file in compatibility mode.
            /// </summary>
            OF_SHARE_DENY_NONE  = 0x00000040,

            /// <summary>
            /// Opens the file nonexclusively. Other processes can open the file with write access. 
            /// AVIFileOpen fails if another process has opened the file in compatibility mode or has read access to it.
            /// </summary>
            OF_SHARE_DENY_READ  = 0x00000030,

            /// <summary>
            /// Opens the file nonexclusively. Other processes can open the file with read access. AVIFileOpen fails 
            /// if another process has opened the file in compatibility mode or has write access to it.
            /// </summary>
            OF_SHARE_DENY_WRITE = 0x00000020,

            /// <summary>
            /// Opens the file and denies other processes any access to it. 
            /// AVIFileOpen fails if any other process has opened the file.
            /// </summary>
            OF_SHARE_EXCLUSIVE  = 0x00000010,

            /// <summary>
            /// Opens the file for writing.
            /// </summary>
            OF_WRITE            = 0x00000001
        }

        /// <summary>
        /// Four-character codes indicating the type of stream.
        /// </summary>
        public enum StreamType
        {
            /// <summary>
            /// Any stream type (only to open).
            /// </summary>
            ANY   = 0,

            /// <summary>
            /// Audio stream.
            /// </summary>
            AUDIO = 0x73647561,

            /// <summary>
            /// MIDI stream.
            /// </summary>
            MIDI  = 0x7364696d,

            /// <summary>
            /// Text stream.
            /// </summary>
            TEXT  = 0x73747874,

            /// <summary>
            /// Video stream.
            /// </summary>
            VIDEO = 0x73646976
        }

        /// <summary>
        /// Marshal of AVIFileExit function. Exits the AVIFile library.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756795(VS.85).aspx">MSDN</a>.
        /// </remarks>
        [DllImport("avifil32.dll")]
        public static extern void AVIFileExit();

        /// <summary>
        /// Marshal of AVIFileGetStream function. Gets stream from AVI file.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756796(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="pfile">
        /// Handle to an open AVI file.
        /// </param>
        /// <param name="ppavi">
        /// Pointer to the new stream interface.
        /// </param>
        /// <param name="fccType">
        /// Four-character code indicating the type of stream to open. Zero indicates any stream can be opened. 
        /// </param>
        /// <param name="lParam">
        /// Count of the stream type. Identifies which occurrence of the specified stream type to access.
        /// </param>
        /// <returns>
        /// Returns zero if successful or an error otherwise.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern AVIError AVIFileGetStream(IntPtr pfile, ref IntPtr ppavi, StreamType fccType, int lParam);

        /// <summary>
        /// Marshal of AVIFileInit function. Initializes the AVIFile library.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756799(VS.85).aspx">MSDN</a>.
        /// </remarks>
        [DllImport("avifil32.dll")]
        public static extern void AVIFileInit();

        /// <summary>
        /// Marshal of AVIFileOpen function. Opens an AVI file.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756800(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="ppfile">
        /// Pointer to a buffer that receives the new IAVIFile interface pointer.
        /// </param>
        /// <param name="szFile">
        /// String containing the name of the file to open.
        /// </param>
        /// <param name="mode">
        /// Access mode to use when opening the file. The default access mode is OF_READ.
        /// </param>
        /// <param name="pclsidHandler">
        /// Pointer to a class identifier of the standard or custom handler you want to use. If the value is NULL, 
        /// the system chooses a handler from the registry based on the file extension or the RIFF type specified in the file.
        /// </param>
        /// <returns>
        /// Returns zero if successful or an error otherwise.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern AVIError AVIFileOpen(ref IntPtr ppfile, String szFile, AVIFileOpenMode mode, IntPtr pclsidHandler);

        /// <summary>
        /// Marshal of AVIFileRelease function. Decrements the reference count of an AVI file interface handle 
        /// and closes the file if the count reaches zero.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756802(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="pfile">
        /// Handle to an open AVI file.
        /// </param>
        /// <returns>
        /// Returns the reference count of the file. This return value should be used only for debugging purposes.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern uint AVIFileRelease(IntPtr pfile);

        /// <summary>
        /// Marshal of AVIStreamGetFrame function. Returns the address of a decompressed video frame.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756828(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="pgf">
        /// Pointer to the IGetFrame interface.
        /// </param>
        /// <param name="lPos">
        /// Position, in samples, within the stream of the desired frame.
        /// </param>
        /// <returns>
        /// Returns a pointer to the frame data if successful or NULL otherwise.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern IntPtr AVIStreamGetFrame(IntPtr pgf, int lPos);

        /// <summary>
        /// Marshal of AVISreamGetFrameClose function. Releases resources used to decompress video frames.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756829(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="pget">
        /// Handle returned from the AVIStreamGetFrameOpen function.
        /// </param>
        /// <returns>
        /// Returns zero if successful or an error otherwise.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamGetFrameClose(IntPtr pget);

        /// <summary>
        /// Marshal of AVIStreamGetFrameOpen function. Prepares to decompress video frames from the specified video stream.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756830(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="pavi">
        /// Pointer to the video stream used as the video source.
        /// </param>
        /// <param name="lpbiWanted">
        /// Pointer to a structure that defines the desired video format.
        /// </param>
        /// <returns>
        /// Returns a GetFrame object that can be used with the AVIStreamGetFrame function.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern IntPtr AVIStreamGetFrameOpen(IntPtr pavi, ref BITMAPINFOHEADER lpbiWanted);

        /// <summary>
        /// Marshal of AVIStreamInfo function. Obtains stream header information.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756831(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="pavi">
        /// Handle to an open stream.
        /// </param>
        /// <param name="psi">
        /// Pointer to a structure to contain the stream information.
        /// </param>
        /// <param name="lSize">
        /// Size, in bytes, of the structure used for psi.
        /// </param>
        /// <returns>
        /// Returns zero if successful or an error otherwise.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern AVIError AVIStreamInfo(IntPtr pavi, ref AVISTREAMINFO psi, int lSize);

        /// <summary>
        /// Marshal of AVIStreamReadFormat function. Reads the stream format data.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756851(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="pavi">
        /// Handle to an open stream. 
        /// </param>
        /// <param name="lPos">
        /// Position in the stream used to obtain the format data. 
        /// </param>
        /// <param name="lpFormat">
        /// Pointer to a buffer to contain the format data. 
        /// </param>
        /// <param name="lpcbFormat">
        /// Pointer to a location indicating the size of the memory block referenced by lpFormat.</param>
        /// <returns>
        /// Returns zero if successful or an error otherwise.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern AVIError AVIStreamReadFormat(IntPtr pavi, int lPos, ref BITMAPINFOHEADER lpFormat, ref int lpcbFormat);

        /// <summary>
        /// Marshal of AVIStreamReadFormat function. Reads the stream format data.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756851(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="pavi">
        /// Handle to an open stream. 
        /// </param>
        /// <param name="lPos">
        /// Position in the stream used to obtain the format data. 
        /// </param>
        /// <param name="lpFormat">
        /// Pointer to a buffer to contain the format data. 
        /// </param>
        /// <param name="lpcbFormat">
        /// Pointer to a location indicating the size of the memory block referenced by lpFormat.</param>
        /// <returns>
        /// Returns zero if successful or an error otherwise.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern AVIError AVIStreamReadFormat(IntPtr pavi, int lPos, IntPtr lpFormat, ref int lpcbFormat);

        /// <summary>
        /// Marshal of AVIStreamRelease function. Decrements the reference count of an AVI stream interface handle, 
        /// and closes the stream if the count reaches zero.
        /// </summary>
        /// <remarks>
        /// See full documentation on <a href="http://msdn.microsoft.com/en-us/library/dd756852(VS.85).aspx">MSDN</a>.
        /// </remarks>
        /// <param name="pavi">
        /// Handle to an open stream.
        /// </param>
        /// <returns>
        /// Returns the current reference count of the stream. This value should be used only for debugging purposes.
        /// </returns>
        [DllImport("avifil32.dll")]
        public static extern int AVIStreamRelease(IntPtr pavi);
    }
}
