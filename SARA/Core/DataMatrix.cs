using System;

namespace SARA.Core
{
    /// <summary>
    /// Multi-dimensional matrix of data of specified type.
    /// </summary>
    /// <typeparam name="T">
    /// Type of matrix cells. Should be one of basic types:
    /// <see cref="System.Byte"/>, <see cref="System.SByte"/>, <see cref="System.UInt16"/>, <see cref="System.Int16"/>, 
    /// <see cref="System.UInt32"/>, <see cref="System.Int32"/>, <see cref="System.UInt64"/>, <see cref="System.Int64"/>, 
    /// <see cref="System.Single"/>, <see cref="System.Double"/> and <see cref="System.Decimal"/>.
    /// </typeparam>
    public class DataMatrix<T> : BaseDataMatrix
    {
        private int[] _dimensions;
        private int   _size;
        private T[]   _data;

        /// <summary>
        /// Creates new matrix filled by zeros.
        /// </summary>
        /// <param name="dimensions">
        /// Dimensions of matrix.
        /// </param>
        public DataMatrix(int[] dimensions)
        {
            _size = 1;
            foreach (int d in dimensions)
            {
                if (d <= 0)
                    throw new ArgumentException("Dimension of DataMatrix shall be greather than 0");

                _size *= d;
            }

            _dimensions = (int[])dimensions.Clone();
            _data = new T[_size];
        }

        /// <summary>
        /// Creates new matrix from data.
        /// </summary>
        /// <param name="dimensions">
        /// Dimensions of matrix.
        /// </param>
        /// <param name="data">
        /// Data to fill the matrix.
        /// </param>
        public DataMatrix(int[] dimensions, T[] data)
        {
            _size = 1;
            foreach (int d in dimensions)
            {
                if (d <= 0)
                    throw new ArgumentException("Dimension of DataMatrix shall be greather than 0");

                _size *= d;
            }
            if (data.Length < _size)
                throw new ArgumentException("Data buffer too small");

            _dimensions = (int[])dimensions.Clone();
            _data = new T[_size];
            Array.Copy(data, _data, _size);
        }

        /// <summary>
        /// Data of matrix.
        /// </summary>
        public T[] Data
        {
            get { return _data; }
        }

        #region BaseDataMatrixMethods

        /// <summary>
        /// Convert to DataMatrix of specified type.
        /// </summary>
        /// <typeparam name="Out">
        /// Output type of DataMatrix.
        /// </typeparam>
        /// <returns>
        /// DataMatrix converted to specified type.
        /// </returns>
        public override DataMatrix<Out> Convert<Out>()
        {
            DataMatrix<Out> result = new DataMatrix<Out>(_dimensions);

            Converter<T, Out> convert = BaseTypeOperations.GetConverter<T, Out>();

            for (int i = 0; i < _size; i++)
                result._data[i] = convert(_data[i]);

            return result;
        }

        /// <summary>
        /// Create copy of matrix.
        /// </summary>
        /// <returns>
        /// Copy of matrix.
        /// </returns>
        public override BaseDataMatrix Copy()
        {
            DataMatrix<T> result = new DataMatrix<T>(_dimensions);

            Array.Copy(_data, result._data, _size);

            return result;
        }

        /// <summary>
        /// Number of pixels/cells on matrix.
        /// </summary>
        public override int Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Dimensions of matrix.
        /// </summary>
        public override int[] Dimensions
        {
            get { return _dimensions; }
        }

        #endregion

        #region Operators

        // TODO: operators

        #endregion

        internal static void CompareDimensions(DataMatrix<T> d1, DataMatrix<T> d2)
        {
            if (d1._dimensions.Length != d2._dimensions.Length)
                throw new ArgumentException("Different dimension number");

            for (int n = 0; n < d1._dimensions.Length; n++)
                if (d1._dimensions[n] != d2._dimensions[n])
                    throw new ArgumentException("Different matrix dimensions");
        }
    }
}
