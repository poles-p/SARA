
namespace SARA.Core
{
    /// <summary>
    /// Multidimensional matrix of floats.
    /// </summary>
    /// <remarks>
    /// FloatMatrix is basic type to reperesents data (images, spectrums, matrices,...) in SARA. This type is 
    /// encapsulation of DataMatrix.
    /// </remarks>
    public class FloatMatrix
    {
        private DataMatrix<float> _matrix;

        /// <summary>
        /// Create new FloatMatrix.
        /// </summary>
        /// <remarks>
        /// Data of matrix are not copied, so when we change data in new FloatMatrix, we also change it in source.
        /// </remarks>
        /// <param name="dataMatrix">
        /// dataMatrix source matrix to create FloatMatrix.
        /// </param>
        public FloatMatrix(DataMatrix<float> dataMatrix)
        {
            _matrix = dataMatrix;
        }

        /// <summary>
        /// Create new FloatMatrix.
        /// </summary>
        /// <remarks>
        /// This constructor create copy of source matrix.
        /// </remarks>
        /// <param name="dataMatrix">
        /// Source matrix to create FloatMatrix.
        /// </param>
        public FloatMatrix(BaseDataMatrix dataMatrix)
        {
            _matrix = dataMatrix.Convert<float>();
        }

        /// <summary>
        /// Encapsulated DataMatrix.
        /// </summary>
        public DataMatrix<float> DataMatrix
        {
            get { return _matrix; }
        }

        /// <summary>
        /// Data of matrix.
        /// </summary>
        public float[] Data
        {
            get { return _matrix.Data; }
        }

        /// <summary>
        /// Dimensions of matrix.
        /// </summary>
        public int[] Dimensions
        {
            get { return _matrix.Dimensions; }
        }

        /// <summary>
        /// Number of pixels/cell on matrix.
        /// </summary>
        public int Size
        {
            get { return _matrix.Size; }
        }

        /// <summary>
        /// Create copy of matrix.
        /// </summary>
        /// <returns>
        /// Copy of matrix.
        /// </returns>
        public FloatMatrix Copy()
        {
            return new FloatMatrix((DataMatrix<float>)_matrix.Copy());
        }

        #region Arithmetic

        /// <summary>
        /// Add other matrix.
        /// </summary>
        /// <remarks>
        /// Add value of corresponding cell to every cell of matrix. Matrices have to have the same dimensions.
        /// </remarks>
        /// <param name="m">
        /// Matrix to be added. Method does not change data in m.
        /// </param>
        public void Add(FloatMatrix m)
        {
            DataMatrix<float>.CompareDimensions(_matrix, m._matrix);

            int size = _matrix.Size;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    fixed (float* src = m._matrix.Data)
                    {
                        for (int i = 0; i < size; i++)
                            dst[i] += src[i];
                    }
                }
            }
        }

        /// <summary>
        /// Add constans to matrix.
        /// </summary>
        /// <remarks>
        /// Add constans to every cell of matrix.
        /// </remarks>
        /// <param name="c">
        /// Constans to be added.
        /// </param>
        public void Add(float c)
        {
            int size = _matrix.Size;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    for (int i = 0; i < size; i++)
                        dst[i] += c;
                }
            }
        }

        /// <summary>
        /// Subtract other matrix.
        /// </summary>
        /// <remarks>
        /// Subtract value of corresponding cell to every cell of matrix. Matrices have to have the same dimensions.
        /// </remarks>
        /// <param name="m">
        /// Matrix to be subtracted. Method does not change data in m.
        /// </param>
        public void Subtract(FloatMatrix m)
        {
            DataMatrix<float>.CompareDimensions(_matrix, m._matrix);

            int size = _matrix.Size;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    fixed (float* src = m._matrix.Data)
                    {
                        for (int i = 0; i < size; i++)
                            dst[i] -= src[i];
                    }
                }
            }
        }

        /// <summary>
        /// Subtract constans from matrix.
        /// </summary>
        /// <remarks>
        /// Subtract constans from every cell of matrix.
        /// </remarks>
        /// <param name="c">
        /// Constans to be subtracted.
        /// </param>
        public void Subtract(float c)
        {
            int size = _matrix.Size;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    for (int i = 0; i < size; i++)
                        dst[i] -= c;
                }
            }
        }

        /// <summary>
        /// Multiply by other matrix.
        /// </summary>
        /// <remarks>
        /// Multiply every cell by corresponding cell from other matrix. It is not algebraic matrix multiply!
        /// Matrices have to have same dimensions.
        /// </remarks>
        /// <param name="m">
        /// Matrix to be multiplied. Method does not change data in m.
        /// </param>
        public void Multiply(FloatMatrix m)
        {
            DataMatrix<float>.CompareDimensions(_matrix, m._matrix);

            int size = _matrix.Size;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    fixed (float* src = m._matrix.Data)
                    {
                        for (int i = 0; i < size; i++)
                            dst[i] *= src[i];
                    }
                }
            }
        }

        /// <summary>
        /// Multiply by constans.
        /// </summary>
        /// <remarks>
        /// Multiply every cell by constans.
        /// </remarks>
        /// <param name="c">
        /// Constans to be multiplied.
        /// </param>
        public void Multiply(float c)
        {
            int size = _matrix.Size;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    for (int i = 0; i < size; i++)
                        dst[i] *= c;
                }
            }
        }

        /// <summary>
        /// Divide by other matrix.
        /// </summary>
        /// <remarks>
        /// Divide every cell by corresponding cell from other matrix. It is not algebraic matrix division!
        /// Matrices have to have same dimensions.
        /// </remarks>
        /// <param name="m">
        /// Matrix to be divided by. Method does not change data in m.
        /// </param>
        public void Divide(FloatMatrix m)
        {
            DataMatrix<float>.CompareDimensions(_matrix, m._matrix);

            int size = _matrix.Size;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    fixed (float* src = m._matrix.Data)
                    {
                        for (int i = 0; i < size; i++)
                            dst[i] /= src[i];
                    }
                }
            }
        }

        /// <summary>
        /// Divide by constans.
        /// </summary>
        /// <remarks>
        /// Divide every cell of matrix by constans.
        /// </remarks>
        /// <param name="c">
        /// Constans to be divided by.
        /// </param>
        public void Divide(float c)
        {
            int size = _matrix.Size;
            float ic = 1.0f / c;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    for (int i = 0; i < size; i++)
                        dst[i] *= ic;
                }
            }
        }

        /// <summary>
        /// Additive inverse of matrix.
        /// </summary>
        /// <remarks>
        /// Substitute every cell of matrix by its additive inverse.
        /// </remarks>
        public void Negate()
        {
            int size = _matrix.Size;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    for (int i = 0; i < size; i++)
                        dst[i] = -dst[i];
                }
            }
        }

        /// <summary>
        /// Multiplicative inverse of matrix.
        /// </summary>
        /// <remarks>
        /// Substitute every cell of matrix by its multiplicative inverse.
        /// </remarks>
        public void Inverse()
        {
            int size = _matrix.Size;

            unsafe
            {
                fixed (float* dst = _matrix.Data)
                {
                    for (int i = 0; i < size; i++)
                        dst[i] = 1.0f / dst[i];
                }
            }
        }

        /// <summary>
        /// Add matrices.
        /// </summary>
        /// <remarks>
        /// Create new matrix and fill cells by sums of corresponding cells from source matrices.
        /// Source matrices have to have same dimensions. Operator does not change source matrices.
        /// </remarks>
        /// <param name="a">
        /// First summand.
        /// </param>
        /// <param name="b">
        /// Second summand.
        /// </param>
        /// <returns>
        /// New matrix beeing sum of source matrices.
        /// </returns>
        public static FloatMatrix operator +(FloatMatrix a, FloatMatrix b)
        {
            FloatMatrix result = a.Copy();
            result.Add(b);

            return result;
        }

        /// <summary>
        /// Add constans to matrix.
        /// </summary>
        /// <remarks>
        /// Operator does not change source matrix.
        /// </remarks>
        /// <param name="a">
        /// Source matrix.
        /// </param>
        /// <param name="c">
        /// Constans to be added.
        /// </param>
        /// <returns>
        /// New matrix beeing source matrix after adding constans to every cells.
        /// </returns>
        public static FloatMatrix operator +(FloatMatrix a, float c)
        {
            FloatMatrix result = a.Copy();
            result.Add(c);

            return result;
        }

        /// <summary>
        /// Add constans to matrix.
        /// </summary>
        /// <remarks>
        /// Operator does not change source matrix.
        /// </remarks>
        /// <param name="c">
        /// Constans to be added.
        /// </param>
        /// <param name="a">
        /// Source matrix.
        /// </param>        
        /// <returns>
        /// New matrix beeing source matrix after adding constans to every cells.
        /// </returns>
        public static FloatMatrix operator +(float c, FloatMatrix a)
        {
            FloatMatrix result = a.Copy();
            result.Add(c);

            return result;
        }

        /// <summary>
        /// Subtract matrices.
        /// </summary>
        /// <remarks>
        /// Matrices have to have same dimensions. Operator does not change source matrices.
        /// </remarks>
        /// <param name="a">
        /// Minuend of subtaction.
        /// </param>
        /// <param name="b">
        /// Subtrahend of subtraction.
        /// </param>
        /// <returns>
        /// New matrix beeing difference of source matrices.
        /// </returns>
        public static FloatMatrix operator -(FloatMatrix a, FloatMatrix b)
        {
            FloatMatrix result = a.Copy();
            result.Subtract(b);

            return result;
        }

        /// <summary>
        /// Subtract constans from matrix.
        /// </summary>
        /// <remarks>
        /// Subtract constans from all matrix cells. Operator does not change source matrix.
        /// </remarks>
        /// <param name="a">
        /// Source matrix.
        /// </param>
        /// <param name="c">
        /// A constans to subtract.
        /// </param>
        /// <returns>
        /// A new matrix beeing result of subtraction.
        /// </returns>
        public static FloatMatrix operator -(FloatMatrix a, float c)
        {
            FloatMatrix result = a.Copy();
            result.Subtract(c);

            return result;
        }

        /// <summary>
        /// Subtract matrix from constans (negate matrix and add constans).
        /// </summary>
        /// <remarks>
        /// Matrix does not change source matrix.
        /// </remarks>
        /// <param name="c">
        /// Constans.
        /// </param>
        /// <param name="a">
        /// Matrix.
        /// </param>
        /// <returns>
        /// A new matrix beeing result of subtraction.
        /// </returns>
        public static FloatMatrix operator -(float c, FloatMatrix a)
        {
            FloatMatrix result = a.Copy();
            result.Negate();
            result.Add(c);

            return result;
        }

        /// <summary>
        /// Multiply every cells of matrices.
        /// </summary>
        /// <remarks>
        /// Multiply corresponding cells of matrices. It is not algebraic matrix multiply!
        /// Matrices have to have same dimensions. Operator does not change source matrices.
        /// </remarks>
        /// <param name="a">
        /// First factor.
        /// </param>
        /// <param name="b">
        /// Second factor.
        /// </param>
        /// <returns>
        /// New matrix beeing result of multiplication.
        /// </returns>
        public static FloatMatrix operator *(FloatMatrix a, FloatMatrix b)
        {
            FloatMatrix result = a.Copy();
            result.Multiply(b);

            return result;
        }

        /// <summary>
        /// Multiply matrix by scalar.
        /// </summary>
        /// <remarks>
        /// Operator does not change source matrix.
        /// </remarks>
        /// <param name="a">
        /// Matrix factor.
        /// </param>
        /// <param name="c">
        /// Scalar factor.
        /// </param>
        /// <returns>
        /// New scaled matrix.
        /// </returns>
        public static FloatMatrix operator *(FloatMatrix a, float c)
        {
            FloatMatrix result = a.Copy();
            result.Multiply(c);

            return result;
        }

        /// <summary>
        /// Multiply matrix by scalar.
        /// </summary>
        /// <remarks>
        /// Operator does not change source matrix.
        /// </remarks>
        /// <param name="c">
        /// Scalar factor.
        /// </param>
        /// <param name="a">
        /// Matrix factor.
        /// </param>       
        /// <returns>
        /// New scaled matrix.
        /// </returns>
        public static FloatMatrix operator *(float c, FloatMatrix a)
        {
            FloatMatrix result = a.Copy();
            result.Multiply(c);

            return result;
        }

        /// <summary>
        /// Divide matrices.
        /// </summary>
        /// <remarks>
        /// Divide corresponding cells of matrices. It is not algebraic matrix division!
        /// Matrices have to have same dimensions. Operator does not change source matrices.
        /// </remarks>
        /// <param name="a">
        /// Divident matrix.
        /// </param>
        /// <param name="b">
        /// Divisor matrix.
        /// </param>
        /// <returns>
        /// New matrix beeing result of division.
        /// </returns>
        public static FloatMatrix operator /(FloatMatrix a, FloatMatrix b)
        {
            FloatMatrix result = a.Copy();
            result.Divide(b);

            return result;
        }

        /// <summary>
        /// Divide matrix by scalar.
        /// </summary>
        /// <remarks>
        /// Operator does not change source matrix.
        /// </remarks>
        /// <param name="a">
        /// Divident matrix.
        /// </param>
        /// <param name="c">
        /// Divisor scalar.
        /// </param>
        /// <returns>
        /// New divided matrix.
        /// </returns>
        public static FloatMatrix operator /(FloatMatrix a, float c)
        {
            FloatMatrix result = a.Copy();
            result.Divide(c);

            return result;
        }

        /// <summary>
        /// Divide scalar by matrix ((not algebraic) inverse matrix and multiply by scalar).
        /// </summary>
        /// <remarks>
        /// Operator does not change source matrix.
        /// </remarks>
        /// <param name="c">
        /// Matrix
        /// </param>
        /// <param name="a">
        /// Scalar.</param>
        /// <returns>
        /// New matrix beeing result of division.
        /// </returns>
        public static FloatMatrix operator /(float c, FloatMatrix a)
        {
            FloatMatrix result = a.Copy();
            result.Inverse();
            result.Multiply(c);

            return result;
        }

        #endregion
    }
}
