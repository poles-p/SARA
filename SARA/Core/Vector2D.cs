
namespace SARA.Core
{
    /// <summary>
    /// Two dimensional vector.
    /// </summary>
    /// <remarks>
    /// SARA use this structure to represent positions on images, plots, etc. 
    /// </remarks>
    public struct Vector2D
    {
        /// <summary>
        /// X coordinate of vector.
        /// </summary>
        public float X;
        /// <summary>
        /// Y coordinate of vector.
        /// </summary>
        public float Y;

        /// <summary>
        /// Create new vector.
        /// </summary>
        /// <param name="x">
        /// X coordinate of new vector.
        /// </param>
        /// <param name="y">
        /// Y coordinate of new vector.
        /// </param>
        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        #region Arithmetic

        /// <summary>
        /// Add vectors.
        /// </summary>
        /// <param name="a">
        /// First summand.
        /// </param>
        /// <param name="b">
        /// Second summand.
        /// </param>
        /// <returns>
        /// Sum of vectors.
        /// </returns>
        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Subtract vectors.
        /// </summary>
        /// <param name="a">
        /// Minuend.
        /// </param>
        /// <param name="b">
        /// Subtrahend.
        /// </param>
        /// <returns>
        /// Difference beetwen vectors.
        /// </returns>
        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Multiply vector by scalar.
        /// </summary>
        /// <param name="c">
        /// Scalar factor.
        /// </param>
        /// <param name="v">
        /// Vector factor.
        /// </param>
        /// <returns>
        /// Scaled vector.
        /// </returns>
        public static Vector2D operator *(float c, Vector2D v)
        {
            return new Vector2D(c * v.X, c * v.Y);
        }

        /// <summary>
        /// Multiply vector by scalar.
        /// </summary>
        /// <param name="v">
        /// Vector factor.
        /// </param>
        /// <param name="c">
        /// Scalar factor.
        /// </param>
        /// <returns>
        /// Scaled vector.
        /// </returns>
        public static Vector2D operator *(Vector2D v, float c)
        {
            return new Vector2D(c * v.X, c * v.Y);
        }

        /// <summary>
        /// Divide vector by scalar.
        /// </summary>
        /// <param name="v">
        /// Vector.
        /// </param>
        /// <param name="c">
        /// Scalar.
        /// </param>
        /// <returns>
        /// Divided vector.
        /// </returns>
        public static Vector2D operator /(Vector2D v, float c)
        {
            return new Vector2D(v.X / c, v.Y / c);
        }

        /// <summary>
        /// Dot product of vectors.
        /// </summary>
        /// <param name="a">
        /// First factor.
        /// </param>
        /// <param name="b">
        /// Second factor.
        /// </param>
        /// <returns>
        /// Dot product of vectors.
        /// </returns>
        public static float operator *(Vector2D a, Vector2D b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        #endregion

        /// <summary>
        /// Zero vector.
        /// </summary>
        public static Vector2D Zero
        {
            get { return new Vector2D(); }
        }
    }
}
