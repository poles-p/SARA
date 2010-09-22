using System;
using System.Runtime.InteropServices;
using SARA.Avi.AviMarshal;

namespace SARA.Avi
{
    /// <summary>
    /// Encapsulation of PAVIFILE. Represents AVI file.
    /// </summary>
    public class AviFile
    {
        private IntPtr _aviFile;

        /// <summary>
        /// Open AVI file.
        /// </summary>
        /// <param name="path">
        /// Path to AVI file.
        /// </param>
        public AviFile(string path)
        {
            AviFil32.AVIFileInit();

            AviFil32.AVIError error = AviFil32.AVIFileOpen(ref _aviFile, path, AviFil32.AVIFileOpenMode.OF_READ, IntPtr.Zero);
            if (error != AviFil32.AVIError.AVIERR_OK)
            {
                _aviFile = IntPtr.Zero;
                AviFil32.AVIFileExit();

                switch (error)
                {
                    case AviFil32.AVIError.AVIERR_BADFORMAT:
                        throw new AviException("The file couldn't be read, indicating a corrupt file or an unrecognized format.");
                    case AviFil32.AVIError.AVIERR_FILEOPEN:
                        throw new AviException("The file could not be opened because of insufficient memory.");
                    case AviFil32.AVIError.AVIERR_FILEREAD:
                        throw new AviException("A disk error occurred while reading the file.");
                    case AviFil32.AVIError.AVIERR_MEMORY:
                        throw new AviException("A disk error occurred while opening the file.");
                    case AviFil32.AVIError.REGDB_E_CLASSNOTREG:
                        throw new AviException("According to the registry, the type of file specified in AVIFileOpen does not have a handler to process it.");
                    default:
                        throw new AviException("Unrecognised AVI exception (0x" + Convert.ToString((int)error, 16) + ").");
                }
            }
        }

        /// <summary>
        /// AviFile destructor.
        /// </summary>
        ~AviFile()
        {
            if (_aviFile != IntPtr.Zero)
            {
                AviFil32.AVIFileRelease(_aviFile);
                AviFil32.AVIFileExit();
            }
        }

        /// <summary>
        /// Get avi stream from AviFile.
        /// </summary>
        /// <param name="id">
        /// Stream id.
        /// </param>
        /// <returns>
        /// Avi stream or null when stream does not exist.
        /// </returns>
        public AviStream GetStream(int id)
        {
            IntPtr aviStream = new IntPtr();
            AVISTREAMINFO streamInfo = new AVISTREAMINFO();

            AviFil32.AVIError error = AviFil32.AVIFileGetStream(_aviFile, ref aviStream, AviFil32.StreamType.ANY, id);
            if (error != AviFil32.AVIError.AVIERR_OK)
            {
                switch (error)
                {
                    case AviFil32.AVIError.AVIERR_NODATA:
                        throw new AviException("The file does not contain a stream corresponding to specified values.");
                    case AviFil32.AVIError.AVIERR_MEMORY:
                        throw new AviException("Not enough memory.");
                    default:
                        throw new AviException("Unrecognised AVI exception (0x" + Convert.ToString((int)error, 16) + ").");
                }
            }
            if (aviStream == IntPtr.Zero)
                return null;

            error = AviFil32.AVIStreamInfo(aviStream, ref streamInfo, Marshal.SizeOf(streamInfo));
            if (error != AviFil32.AVIError.AVIERR_OK)
                throw new AviException("Unrecognised AVI exception (0x" + Convert.ToString((int)error, 16) + ").");

            switch (streamInfo.fccType)
            {
                case AviFil32.StreamType.AUDIO:
                    return new AudioStream(aviStream, streamInfo);
                case AviFil32.StreamType.MIDI:
                    return new MidiStream(aviStream, streamInfo);
                case AviFil32.StreamType.TEXT:
                    return new TextStream(aviStream, streamInfo);
                case AviFil32.StreamType.VIDEO:
                    return new VideoStream(aviStream, streamInfo);
                default:
                    throw new AviException("Unknown stream type (0x" + Convert.ToString((int)streamInfo.fccType, 16) + ").");
            }
        }

        /// <summary>
        /// Get video stream from AVI file.
        /// </summary>
        /// <returns>
        /// Video stream.
        /// </returns>
        public VideoStream GetVideoStream()
        {
            IntPtr aviStream = new IntPtr();
            AVISTREAMINFO streamInfo = new AVISTREAMINFO();

            AviFil32.AVIError error = AviFil32.AVIFileGetStream(_aviFile, ref aviStream, AviFil32.StreamType.VIDEO, 0);
            if (error != AviFil32.AVIError.AVIERR_OK)
            {
                switch (error)
                {
                    case AviFil32.AVIError.AVIERR_NODATA:
                        throw new AviException("The file does not contain a stream corresponding to specified values.");
                    case AviFil32.AVIError.AVIERR_MEMORY:
                        throw new AviException("Not enough memory.");
                    default:
                        throw new AviException("Unrecognised AVI exception (0x" + Convert.ToString((int)error, 16) + ").");
                }
            }
            if (aviStream == IntPtr.Zero)
                return null;

            error = AviFil32.AVIStreamInfo(aviStream, ref streamInfo, Marshal.SizeOf(streamInfo));
            if (error != AviFil32.AVIError.AVIERR_OK)
                throw new AviException("Unrecognised AVI exception (0x" + Convert.ToString((int)error, 16) + ").");

            switch (streamInfo.fccType)
            {
                case AviFil32.StreamType.AUDIO:
                case AviFil32.StreamType.MIDI:
                case AviFil32.StreamType.TEXT:
                    throw new AviException("Invalid stream type");
                case AviFil32.StreamType.VIDEO:
                    return new VideoStream(aviStream, streamInfo);
                default:
                    throw new AviException("Unknown stream type (0x" + Convert.ToString((int)streamInfo.fccType, 16) + ").");
            }
        }
    }
}
