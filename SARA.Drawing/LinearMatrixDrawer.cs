using System;
using System.Drawing;
using SARA.Core;

namespace SARA.Drawing
{
    /// <summary>
    /// Matrix drawer with linear projection between matrix values and color. 
    /// This matrix drawer can be used only to draw 2D matrices.
    /// </summary>
    public class LinearMatrixDrawer : IMatrixDrawer
    {
        private float _min;
        private float _max;

        /// <summary>
        /// Create new <see cref="LinearMatrixDrawer"/> with default tresholdes (0 and 255).
        /// </summary>
        public LinearMatrixDrawer()
        {
            _min = 0.0f;
            _max = 255.0f;
        }

        /// <summary>
        /// Create new <see cref="LinearMatrixDrawer"/>.
        /// </summary>
        /// <param name="min">
        /// Black treshold.
        /// </param>
        /// <param name="max">
        /// White Treshold.
        /// </param>
        public LinearMatrixDrawer(float min, float max)
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Black treshold.
        /// </summary>
        public float Min
        {
            get { return _min; }
            set { _min = value; }
        }

        /// <summary>
        /// White treshold.
        /// </summary>
        public float Max
        {
            get { return _max; }
            set { _max = value; }
        }

        #region IMatrixDrawer Members

        /// <summary>
        /// Convert <see cref="FloatMatrix"/> to <see cref="Bitmap"/>. Only for 2D matrices.
        /// </summary>
        /// <param name="matrix">
        /// Matrix to convert.
        /// </param>
        /// <returns>
        /// <see cref="Bitmap"/> that represents specified matrix.
        /// </returns>
        public Bitmap ToBitmap(FloatMatrix matrix)
        {
            if (matrix.Dimensions.Length != 2)
                throw new ArgumentException("Expected 2D matrix");

            int xSize = matrix.Dimensions[0];
            int ySize = matrix.Dimensions[1];

            int[] tmpArray = new int[xSize * ySize];
            Bitmap result;

            float c = 255.0f / (_max - _min);

            unsafe
            {
                fixed (int* dstPtr = tmpArray)
                {
                    fixed (float* srcPtr = matrix.Data)
                    {
                        int pos = 0;
                        for (int y = 0; y < ySize; y++)
                        {
                            int rowPos = (ySize - y - 1) * xSize;
                            for (int x = 0; x < xSize; x++)
                            {
                                float ftmp = (srcPtr[pos] - _min) * c;
                                int itmp = (ftmp < 0.0f) ? 0 : (ftmp > 255.0f) ? 255 : (int)ftmp;
                                dstPtr[rowPos] = itmp | (itmp << 8) | (itmp << 16);
                                pos++;
                                rowPos++;
                            }
                        }
                    }
                    result = new Bitmap(matrix.Dimensions[0],
                            matrix.Dimensions[1],
                            matrix.Dimensions[0] * 4,
                            System.Drawing.Imaging.PixelFormat.Format32bppRgb,
                            new IntPtr(dstPtr));
                    result = result.Clone(new Rectangle(new Point(0, 0), result.Size), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                }
            }    

            return result;
        }

        #endregion
    }
}
