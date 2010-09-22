
namespace SARA.Core
{
    /// <summary>
    /// Abstract class that is representing multi-dimensional data matrix of any type.
    /// </summary>
    /// <remarks>
    /// This is base class for generic class DataMatrix.
    /// </remarks>
    public abstract class BaseDataMatrix
    {
        /// <summary>
        /// Convert to DataMatrix of specified type.
        /// </summary>
        /// <typeparam name="Out">
        /// Output type of DataMatrix.
        /// </typeparam>
        /// <returns>
        /// DataMatrix converted to specified type.
        /// </returns>
        public abstract DataMatrix<Out> Convert<Out>();

        /// <summary>
        /// Create copy of matrix.
        /// </summary>
        /// <returns>
        /// Copy of matrix.
        /// </returns>
        public abstract BaseDataMatrix Copy();

        /// <summary>
        /// Number of pixels/cells on matrix.
        /// </summary>
        public abstract int Size
        {
            get;
        }

        /// <summary>
        /// Dimensions of matrix
        /// </summary>
        public abstract int[] Dimensions
        {
            get;
        }

        /// <summary>
        /// Convert matrix to FloatMatrix.
        /// </summary>
        /// <remarks>
        /// FloatMatrix format of data is used by most of SARA functions.
        /// </remarks>
        /// <returns>
        /// Matrix converted to FloatMatrix.
        /// </returns>
        public FloatMatrix ToFloatMatrix()
        {
            return new FloatMatrix(this);
        }
    }
}
