using System;
using System.Collections.Generic;
using System.Linq;
using SARA.Avi.AviMarshal;
using SARA.Core;

namespace SARA.Avi
{
    /// <summary>
    /// Object to grab frames from AVI.
    /// </summary>
    public class FrameGrabber
    {
        private IntPtr _getFrameObj;
        private AVISTREAMINFO _streamInfo;

        /// <summary>
        /// Create new FrameGrabber from GetFrame object.
        /// </summary>
        /// <param name="getFrameObj">
        /// GetFrame object to create FrameGrabber.
        /// </param>
        /// <param name="streamInfo">
        /// Information about stream in <see cref="SARA.Avi.AviMarshal.AVISTREAMINFO"/> structure
        /// </param>
        internal FrameGrabber(IntPtr getFrameObj, AVISTREAMINFO streamInfo)
        {
            if (getFrameObj == IntPtr.Zero)
                throw new AviException("Can not create FrameGrabber from null GetFrame object.");
            _getFrameObj = getFrameObj;
            _streamInfo = streamInfo;
            AviFil32.AVIFileInit();
        }

        /// <summary>
        /// FrameGrabber destructor.
        /// </summary>
        ~FrameGrabber()
        {
            if (_getFrameObj != IntPtr.Zero)
            {
                AviFil32.AVIStreamGetFrameClose(_getFrameObj);
                AviFil32.AVIFileExit();
            }
        }

        /// <summary>
        /// Get frame from AVI as 2-dimensional black and white<see cref="SARA.Core.FloatMatrix"/>
        /// </summary>
        /// <remarks>
        /// This function losts information of pixel colors.
        /// </remarks>
        /// <param name="frameId">
        /// Id of requested frame.
        /// </param>
        /// <returns>
        /// <see cref="SARA.Core.FloatMatrix"/> with requested frame or null.
        /// </returns>
        public FloatMatrix GetFloatMatrix(int frameId)
        {
            IntPtr frameDBI = AviFil32.AVIStreamGetFrame(_getFrameObj, frameId - (int)_streamInfo.dwStart);
            if (frameDBI == IntPtr.Zero)
                throw new AviException("GetFrame returned null frame.");

            FloatMatrix result;

            unsafe
            {
                int* header = (int*)frameDBI.ToPointer();
                
                result = new FloatMatrix(new DataMatrix<float>(new int[] { header[1], header[2] }));
                byte *bitmapData = (byte*)(frameDBI.ToInt32() + header[0]);

                int size = result.DataMatrix.Size;
                fixed (float* destData = result.Data)
                {
                    for (int i = 0; i < size; i++)
                    {
                        destData[i] = (float)(bitmapData[0]) + (float)(bitmapData[1]) + (float)(bitmapData[2]);
                        bitmapData += 3;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Get sequence of all frames in AVI file.
        /// </summary>
        /// <returns>
        /// Sequence of all frames.
        /// </returns>
        public IEnumerable<FloatMatrix> GetAllFrames()
        {
            return from id in Enumerable.Range(0, (int)_streamInfo.dwLength)
                   select GetFloatMatrix(id);
        }
    }
}
