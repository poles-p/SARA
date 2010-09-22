using System;

namespace SARA.Core
{
    /// <summary>
    /// Static class to basic operatons on data of basic types.
    /// </summary> 
    public static class BaseTypeOperations
    {
        #region Byte reversion

        /// <summary>
        /// Reverse bytes in two-bytes groups in specified buffer
        /// </summary>
        /// <remarks>
        /// This method convert data buffer of two-byte type (<see cref="System.Int16"/>, <see cref="System.UInt16"/>) 
        /// from little endian to big endian and vice versa.
        /// </remarks>
        /// <param name="data">
        /// Pointer to buffer to reverse.
        /// </param>
        /// <param name="length">
        /// Number of two-bytes groups to reverse (number of bytes to reverse = length*2).
        /// </param>
        public static unsafe void ReverseBytes2B(byte* data, int length)
        {
            byte tmp;
            for (int i = 0; i < length; i++)
            {
                tmp = *data;
                *data = data[1];
                data[1] = tmp;
                data += 2;
            }
        }

        /// <summary>
        /// Reverse bytes in four-bytes groups in specified buffer.
        /// </summary>
        /// <remarks>
        /// This method convert data buffer of four-byte type (<see cref="System.Int32"/>, <see cref="System.UInt32"/>,
        /// <see cref="System.Single"/>) from little endian to big endian and vice versa.
        /// </remarks>
        /// <param name="data">
        /// Pointer to buffer to reverse.
        /// </param>
        /// <param name="length">
        /// Number of four-bytes groups to reverse (number of bytes to reverse = length*4).
        /// </param>
        public static unsafe void ReverseBytes4B(byte* data, int length)
        {
            byte tmp;
            for (int i = 0; i < length; i++)
            {
                tmp = *data;
                *data = data[3];
                data[3] = tmp;
                tmp = data[1];
                data[1] = data[2];
                data[2] = tmp;
                data += 4;
            }
        }

        /// <summary>
        /// Reverse bytes in eight-bytes groups in specified buffer.
        /// </summary>
        /// <remarks>
        /// This method convert data buffer of eight-byte type (<see cref="System.Int64"/>, <see cref="System.UInt64"/>,
        /// <see cref="System.Double"/>) from little endian to big endian and vice versa.
        /// </remarks>
        /// <param name="data">
        /// Pointer to buffer to reverse.
        /// </param>
        /// <param name="length">
        /// Number of eight-bytes groups to reverse (number of bytes to reverse = length*8).
        /// </param>
        public static unsafe void ReverseBytes8B(byte* data, int length)
        {
            byte tmp;
            for (int i = 0; i < length; i++)
            {
                tmp = *data;
                *data = data[7];
                data[7] = tmp;
                tmp = data[1];
                data[1] = data[6];
                data[6] = tmp;
                tmp = data[2];
                data[2] = data[5];
                data[5] = tmp;
                tmp = data[3];
                data[3] = data[4];
                data[4] = tmp;
                data += 8;
            }
        }

        /// <summary>
        /// Reverse bytes in array.
        /// </summary>
        /// <remarks>
        /// Convert data in array from little endian to big endian and vice versa. Valid types of array are :
        /// <see cref="System.Byte"/>[], <see cref="System.SByte"/>[], <see cref="System.UInt16"/>[], <see cref="System.Int16"/>[], 
        /// <see cref="System.UInt32"/>[], <see cref="System.Int32"/>[], <see cref="System.UInt64"/>[], <see cref="System.Int64"/>[], 
        /// <see cref="System.Single"/>[] and <see cref="System.Double"/>[].
        /// </remarks>
        /// <param name="data">
        /// Data array to convert.
        /// </param>
        public static void ReverseArray<T>(T[] data)
        {
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
                return;
            else if (typeof(T) == typeof(ushort))
            {
                unsafe
                {
                    fixed (ushort* ptr = (ushort[])(object)data)
                    {
                        ReverseBytes2B((byte*)ptr, data.Length);
                    }
                }
            }
            else if (typeof(T) == typeof(short))
            {
                unsafe
                {
                    fixed (short* ptr = (short[])(object)data)
                    {
                        ReverseBytes2B((byte*)ptr, data.Length);
                    }
                }
            }
            else if (typeof(T) == typeof(uint))
            {
                unsafe
                {
                    fixed (uint* ptr = (uint[])(object)data)
                    {
                        ReverseBytes4B((byte*)ptr, data.Length);
                    }
                }
            }
            else if (typeof(T) == typeof(int))
            {
                unsafe
                {
                    fixed (int* ptr = (int[])(object)data)
                    {
                        ReverseBytes4B((byte*)ptr, data.Length);
                    }
                }
            }
            else if (typeof(T) == typeof(float))
            {
                unsafe
                {
                    fixed (float* ptr = (float[])(object)data)
                    {
                        ReverseBytes4B((byte*)ptr, data.Length);
                    }
                }
            }
            else if (typeof(T) == typeof(ulong))
            {
                unsafe
                {
                    fixed (ulong* ptr = (ulong[])(object)data)
                    {
                        ReverseBytes8B((byte*)ptr, data.Length);
                    }
                }
            }
            else if (typeof(T) == typeof(long))
            {
                unsafe
                {
                    fixed (long* ptr = (long[])(object)data)
                    {
                        ReverseBytes8B((byte*)ptr, data.Length);
                    }
                }
            }
            else if (typeof(T) == typeof(double))
            {
                unsafe
                {
                    fixed (double* ptr = (double[])(object)data)
                    {
                        ReverseBytes8B((byte*)ptr, data.Length);
                    }
                }
            }
            else
                throw new ArgumentException("Cannot reverse bytes in " + typeof(T).ToString());
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Get converter between specified types.
        /// </summary>
        /// <remarks>
        /// valid types for TInput and TOutput are <see cref="System.Byte"/>, <see cref="System.SByte"/>, <see cref="System.UInt16"/>, 
        /// <see cref="System.Int16"/>, <see cref="System.UInt32"/>, <see cref="System.Int32"/>, <see cref="System.UInt64"/>, 
        /// <see cref="System.Int64"/>, <see cref="System.Single"/>, <see cref="System.Double"/> and <see cref="System.Decimal"/>.
        /// </remarks>
        /// <typeparam name="TInput">
        /// Source data format.
        /// </typeparam>
        /// <typeparam name="TOutput">
        /// Destination data format.
        /// </typeparam>
        /// <returns>
        /// Converter between specified types.
        /// </returns>
        public static Converter<TInput, TOutput> GetConverter<TInput, TOutput>()
        {
            if (typeof(TInput) == typeof(byte))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, byte>)Identity<byte>;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, sbyte>)UInt8ToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, ushort>)UInt8ToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, short>)UInt8ToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, uint>)UInt8ToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, int>)UInt8ToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, ulong>)UInt8ToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, long>)UInt8ToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, float>)UInt8ToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, double>)UInt8ToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<byte, decimal>)UInt8ToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(sbyte))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, byte>)SInt8ToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, sbyte>)Identity<sbyte>;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, ushort>)SInt8ToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, short>)SInt8ToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, uint>)SInt8ToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, int>)SInt8ToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, ulong>)SInt8ToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, long>)SInt8ToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, float>)SInt8ToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, double>)SInt8ToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<sbyte, decimal>)SInt8ToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(ushort))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, byte>)UInt16ToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, sbyte>)UInt16ToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, ushort>)Identity<ushort>;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, short>)UInt16ToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, uint>)UInt16ToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, int>)UInt16ToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, ulong>)UInt16ToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, long>)UInt16ToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, float>)UInt16ToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, double>)UInt16ToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<ushort, decimal>)UInt16ToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(short))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, byte>)SInt16ToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, sbyte>)SInt16ToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, ushort>)SInt16ToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, short>)Identity<short>;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, uint>)SInt16ToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, int>)SInt16ToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, ulong>)SInt16ToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, long>)SInt16ToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, float>)SInt16ToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, double>)SInt16ToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<short, decimal>)SInt16ToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(uint))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, byte>)UInt32ToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, sbyte>)UInt32ToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, ushort>)UInt32ToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, short>)UInt32ToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, uint>)Identity<uint>;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, int>)UInt32ToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, ulong>)UInt32ToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, long>)UInt32ToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, float>)UInt32ToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, double>)UInt32ToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<uint, decimal>)UInt32ToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(int))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, byte>)SInt32ToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, sbyte>)SInt32ToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, ushort>)SInt32ToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, short>)SInt32ToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, uint>)SInt32ToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, int>)Identity<int>;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, ulong>)SInt32ToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, long>)SInt32ToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, float>)SInt32ToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, double>)SInt32ToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<int, decimal>)SInt32ToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(ulong))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, byte>)UInt64ToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, sbyte>)UInt64ToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, ushort>)UInt64ToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, short>)UInt64ToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, uint>)UInt64ToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, int>)UInt64ToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, ulong>)Identity<ulong>;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, long>)UInt64ToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, float>)UInt64ToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, double>)UInt64ToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<ulong, decimal>)UInt64ToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(long))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, byte>)SInt64ToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, sbyte>)SInt64ToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, ushort>)SInt64ToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, short>)SInt64ToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, uint>)SInt64ToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, int>)SInt64ToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, ulong>)SInt64ToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, long>)Identity<long>;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, float>)SInt64ToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, double>)SInt64ToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<long, decimal>)SInt64ToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(float))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, byte>)FloatToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, sbyte>)FloatToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, ushort>)FloatToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, short>)FloatToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, uint>)FloatToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, int>)FloatToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, ulong>)FloatToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, long>)FloatToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, float>)Identity<float>;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, double>)FloatToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<float, decimal>)FloatToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(double))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, byte>)DoubleToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, sbyte>)DoubleToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, ushort>)DoubleToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, short>)DoubleToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, uint>)DoubleToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, int>)DoubleToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, ulong>)DoubleToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, long>)DoubleToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, float>)DoubleToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, double>)Identity<double>;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<double, decimal>)DoubleToDecimal;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else if (typeof(TInput) == typeof(decimal))
            {
                if (typeof(TOutput) == typeof(byte))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, byte>)DecimalToUInt8;
                else if (typeof(TOutput) == typeof(sbyte))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, sbyte>)DecimalToSInt8;
                else if (typeof(TOutput) == typeof(ushort))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, ushort>)DecimalToUInt16;
                else if (typeof(TOutput) == typeof(short))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, short>)DecimalToSInt16;
                else if (typeof(TOutput) == typeof(uint))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, uint>)DecimalToUInt32;
                else if (typeof(TOutput) == typeof(int))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, int>)DecimalToSInt32;
                else if (typeof(TOutput) == typeof(ulong))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, ulong>)DecimalToUInt64;
                else if (typeof(TOutput) == typeof(long))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, long>)DecimalToSInt64;
                else if (typeof(TOutput) == typeof(float))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, float>)DecimalToFloat;
                else if (typeof(TOutput) == typeof(double))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, double>)DecimalToDouble;
                else if (typeof(TOutput) == typeof(decimal))
                    return (Converter<TInput, TOutput>)(object)(Converter<decimal, decimal>)Identity<decimal>;
                else
                    throw new ArgumentException(typeof(TOutput).ToString() + " is not base type");
            }
            else
                throw new ArgumentException(typeof(TInput).ToString() + " is not base type");
        }

        #region Converters

        /// <summary>
        /// Identity function. Returns own argument.
        /// </summary>
        /// <typeparam name="T">
        /// Type of argument/return value.
        /// </typeparam>
        /// <param name="arg">
        /// Argument to be returned.
        /// </param>
        /// <returns>
        /// Own argument.
        /// </returns>
        private static T Identity<T>(T arg)
        {
            return arg;
        }

        #region UInt8 Converters

        private static sbyte UInt8ToSInt8(byte source)
        {
            return (sbyte)source;
        }

        private static ushort UInt8ToUInt16(byte source)
        {
            return (ushort)source;
        }

        private static short UInt8ToSInt16(byte source)
        {
            return (short)source;
        }

        private static uint UInt8ToUInt32(byte source)
        {
            return (uint)source;
        }

        private static int UInt8ToSInt32(byte source)
        {
            return (int)source;
        }

        private static ulong UInt8ToUInt64(byte source)
        {
            return (ulong)source;
        }

        private static long UInt8ToSInt64(byte source)
        {
            return (long)source;
        }

        private static float UInt8ToFloat(byte source)
        {
            return (float)source;
        }

        private static double UInt8ToDouble(byte source)
        {
            return (double)source;
        }

        private static decimal UInt8ToDecimal(byte source)
        {
            return (decimal)source;
        }

        #endregion
        #region SInt8 Converters

        private static byte SInt8ToUInt8(sbyte source)
        {
            return (byte)source;
        }

        private static ushort SInt8ToUInt16(sbyte source)
        {
            return (ushort)source;
        }

        private static short SInt8ToSInt16(sbyte source)
        {
            return (short)source;
        }

        private static uint SInt8ToUInt32(sbyte source)
        {
            return (uint)source;
        }

        private static int SInt8ToSInt32(sbyte source)
        {
            return (int)source;
        }

        private static ulong SInt8ToUInt64(sbyte source)
        {
            return (ulong)source;
        }

        private static long SInt8ToSInt64(sbyte source)
        {
            return (long)source;
        }

        private static float SInt8ToFloat(sbyte source)
        {
            return (float)source;
        }

        private static double SInt8ToDouble(sbyte source)
        {
            return (double)source;
        }

        private static decimal SInt8ToDecimal(sbyte source)
        {
            return (decimal)source;
        }

        #endregion
        #region UInt16 Converters

        private static byte UInt16ToUInt8(ushort source)
        {
            return (byte)source;
        }

        private static sbyte UInt16ToSInt8(ushort source)
        {
            return (sbyte)source;
        }

        private static short UInt16ToSInt16(ushort source)
        {
            return (short)source;
        }

        private static uint UInt16ToUInt32(ushort source)
        {
            return (uint)source;
        }

        private static int UInt16ToSInt32(ushort source)
        {
            return (int)source;
        }

        private static ulong UInt16ToUInt64(ushort source)
        {
            return (ulong)source;
        }

        private static long UInt16ToSInt64(ushort source)
        {
            return (long)source;
        }

        private static float UInt16ToFloat(ushort source)
        {
            return (float)source;
        }

        private static double UInt16ToDouble(ushort source)
        {
            return (double)source;
        }

        private static decimal UInt16ToDecimal(ushort source)
        {
            return (decimal)source;
        }

        #endregion
        #region SInt16 Converters

        private static byte SInt16ToUInt8(short source)
        {
            return (byte)source;
        }

        private static sbyte SInt16ToSInt8(short source)
        {
            return (sbyte)source;
        }

        private static ushort SInt16ToUInt16(short source)
        {
            return (ushort)source;
        }

        private static uint SInt16ToUInt32(short source)
        {
            return (uint)source;
        }

        private static int SInt16ToSInt32(short source)
        {
            return (int)source;
        }

        private static ulong SInt16ToUInt64(short source)
        {
            return (ulong)source;
        }

        private static long SInt16ToSInt64(short source)
        {
            return (long)source;
        }

        private static float SInt16ToFloat(short source)
        {
            return (float)source;
        }

        private static double SInt16ToDouble(short source)
        {
            return (double)source;
        }

        private static decimal SInt16ToDecimal(short source)
        {
            return (decimal)source;
        }

        #endregion
        #region UInt32 Converters

        private static byte UInt32ToUInt8(uint source)
        {
            return (byte)source;
        }

        private static sbyte UInt32ToSInt8(uint source)
        {
            return (sbyte)source;
        }

        private static ushort UInt32ToUInt16(uint source)
        {
            return (ushort)source;
        }

        private static short UInt32ToSInt16(uint source)
        {
            return (short)source;
        }

        private static int UInt32ToSInt32(uint source)
        {
            return (int)source;
        }

        private static ulong UInt32ToUInt64(uint source)
        {
            return (ulong)source;
        }

        private static long UInt32ToSInt64(uint source)
        {
            return (long)source;
        }

        private static float UInt32ToFloat(uint source)
        {
            return (float)source;
        }

        private static double UInt32ToDouble(uint source)
        {
            return (double)source;
        }

        private static decimal UInt32ToDecimal(uint source)
        {
            return (decimal)source;
        }

        #endregion
        #region SInt32 Converters

        private static byte SInt32ToUInt8(int source)
        {
            return (byte)source;
        }

        private static sbyte SInt32ToSInt8(int source)
        {
            return (sbyte)source;
        }

        private static ushort SInt32ToUInt16(int source)
        {
            return (ushort)source;
        }

        private static short SInt32ToSInt16(int source)
        {
            return (short)source;
        }

        private static uint SInt32ToUInt32(int source)
        {
            return (uint)source;
        }

        private static ulong SInt32ToUInt64(int source)
        {
            return (ulong)source;
        }

        private static long SInt32ToSInt64(int source)
        {
            return (long)source;
        }

        private static float SInt32ToFloat(int source)
        {
            return (float)source;
        }

        private static double SInt32ToDouble(int source)
        {
            return (double)source;
        }

        private static decimal SInt32ToDecimal(int source)
        {
            return (decimal)source;
        }

        #endregion
        #region UInt64 Converters

        private static byte UInt64ToUInt8(ulong source)
        {
            return (byte)source;
        }

        private static sbyte UInt64ToSInt8(ulong source)
        {
            return (sbyte)source;
        }

        private static ushort UInt64ToUInt16(ulong source)
        {
            return (ushort)source;
        }

        private static short UInt64ToSInt16(ulong source)
        {
            return (short)source;
        }

        private static int UInt64ToSInt32(ulong source)
        {
            return (int)source;
        }

        private static uint UInt64ToUInt32(ulong source)
        {
            return (uint)source;
        }

        private static long UInt64ToSInt64(ulong source)
        {
            return (long)source;
        }

        private static float UInt64ToFloat(ulong source)
        {
            return (float)source;
        }

        private static double UInt64ToDouble(ulong source)
        {
            return (double)source;
        }

        private static decimal UInt64ToDecimal(ulong source)
        {
            return (decimal)source;
        }

        #endregion
        #region SInt64 Converters

        private static byte SInt64ToUInt8(long source)
        {
            return (byte)source;
        }

        private static sbyte SInt64ToSInt8(long source)
        {
            return (sbyte)source;
        }

        private static ushort SInt64ToUInt16(long source)
        {
            return (ushort)source;
        }

        private static short SInt64ToSInt16(long source)
        {
            return (short)source;
        }

        private static int SInt64ToSInt32(long source)
        {
            return (int)source;
        }

        private static uint SInt64ToUInt32(long source)
        {
            return (uint)source;
        }

        private static ulong SInt64ToUInt64(long source)
        {
            return (ulong)source;
        }

        private static float SInt64ToFloat(long source)
        {
            return (float)source;
        }

        private static double SInt64ToDouble(long source)
        {
            return (double)source;
        }

        private static decimal SInt64ToDecimal(long source)
        {
            return (decimal)source;
        }

        #endregion
        #region Float Converters

        private static byte FloatToUInt8(float source)
        {
            return (byte)source;
        }

        private static sbyte FloatToSInt8(float source)
        {
            return (sbyte)source;
        }

        private static ushort FloatToUInt16(float source)
        {
            return (ushort)source;
        }

        private static short FloatToSInt16(float source)
        {
            return (short)source;
        }

        private static int FloatToSInt32(float source)
        {
            return (int)source;
        }

        private static uint FloatToUInt32(float source)
        {
            return (uint)source;
        }

        private static ulong FloatToUInt64(float source)
        {
            return (ulong)source;
        }

        private static long FloatToSInt64(float source)
        {
            return (long)source;
        }

        private static double FloatToDouble(float source)
        {
            return (double)source;
        }

        private static decimal FloatToDecimal(float source)
        {
            return (decimal)source;
        }

        #endregion
        #region Double Converters

        private static byte DoubleToUInt8(double source)
        {
            return (byte)source;
        }

        private static sbyte DoubleToSInt8(double source)
        {
            return (sbyte)source;
        }

        private static ushort DoubleToUInt16(double source)
        {
            return (ushort)source;
        }

        private static short DoubleToSInt16(double source)
        {
            return (short)source;
        }

        private static int DoubleToSInt32(double source)
        {
            return (int)source;
        }

        private static uint DoubleToUInt32(double source)
        {
            return (uint)source;
        }

        private static ulong DoubleToUInt64(double source)
        {
            return (ulong)source;
        }

        private static long DoubleToSInt64(double source)
        {
            return (long)source;
        }

        private static float DoubleToFloat(double source)
        {
            return (float)source;
        }

        private static decimal DoubleToDecimal(double source)
        {
            return (decimal)source;
        }

        #endregion
        #region Decimal Converters

        private static byte DecimalToUInt8(decimal source)
        {
            return (byte)source;
        }

        private static sbyte DecimalToSInt8(decimal source)
        {
            return (sbyte)source;
        }

        private static ushort DecimalToUInt16(decimal source)
        {
            return (ushort)source;
        }

        private static short DecimalToSInt16(decimal source)
        {
            return (short)source;
        }

        private static int DecimalToSInt32(decimal source)
        {
            return (int)source;
        }

        private static uint DecimalToUInt32(decimal source)
        {
            return (uint)source;
        }

        private static ulong DecimalToUInt64(decimal source)
        {
            return (ulong)source;
        }

        private static long DecimalToSInt64(decimal source)
        {
            return (long)source;
        }

        private static float DecimalToFloat(decimal source)
        {
            return (float)source;
        }

        private static double DecimalToDouble(decimal source)
        {
            return (double)source;
        }

        #endregion

        #endregion

        #endregion

        #region Block arithmetic

        // TODO: block arithmetics

        #endregion
    }
}
