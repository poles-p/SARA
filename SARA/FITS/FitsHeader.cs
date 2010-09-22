using System;
using System.IO;

namespace SARA.FITS
{
    /// <summary>
    /// Header of FITS file. Header contains iformations about image.
    /// </summary>
    public class FitsHeader
    {
        private enum ValidateResult { NotValidated, Vaild, Invalid }

        private FitsHeaderEntry[] _header;

        private ValidateResult _simpleValidated = ValidateResult.NotValidated;
        private int _bitpix;
        private int _naxis;
        private int[] _dimensions;

        /// <summary>
        /// Create FITS header read from stream. And move stream position to end of header.
        /// </summary>
        /// <param name="reader">
        /// Stream that contains FITS header.
        /// </param>
        public FitsHeader(BinaryReader reader)
        {
            _header = new FitsHeaderEntry[36];

            for (int i = 0; i < 36; i++)
                _header[i] = FitsHeaderEntry.ReadEntry(reader);
        }

        private void ValidateSimple()
        {
            _simpleValidated = ValidateResult.Invalid;

            if (_header[0].Keyword != "SIMPLE" || !_header[0].HasBoolValue || !_header[0].BoolValue)
                throw new FitsFormatException("Expected SIMPLE FITS header");

            if (_header[1].Keyword != "BITPIX")
                throw new FitsFormatException("Expected BITPIX keyword");
            if (!_header[1].HasIntValue)
                throw new FitsFormatException("Expected int value");
            _bitpix = Convert.ToInt32( _header[1].IntValue );
            if (_bitpix != 8 && _bitpix != 16 && _bitpix != 32 && _bitpix != 64 && _bitpix != -32 && _bitpix != -64)
                throw new FitsFormatException("invalid BITPIX value");

            if (_header[2].Keyword != "NAXIS")
                throw new FitsFormatException("Expected NAXIS keyword");
            if (!_header[2].HasIntValue)
                throw new FitsFormatException("Expected int value");
            _naxis = Convert.ToInt32( _header[2].IntValue );
            if (_naxis < 0 || _naxis > 999)
                throw new FitsFormatException("NAXIS shall contain value between 0 and 999");

            _dimensions = new int[_naxis];

            for (int n = 0; n < _naxis; n++)
            {
                if (_header[3+n].Keyword != "NAXIS" + (n+1).ToString())
                    throw new FitsFormatException("Expected NAXIS" + (n+1).ToString() + " keyword");
                if (!_header[3+n].HasIntValue)
                    throw new FitsFormatException("Expected int value");
                _dimensions[n] = Convert.ToInt32( _header[3+n].IntValue );
                if (_dimensions[n] < 0)
                    throw new FitsFormatException("Expected NAXIS" + (n + 1).ToString() + " shall contain value greather than 0");
            }

            _simpleValidated = ValidateResult.Vaild;
        }

        /// <summary>
        /// Bits per pixel of FITS file.
        /// </summary>
        /// <remarks>
        /// Possibile values of BITPIX field are:
        /// <list type="table">
        ///     <listheader>
        ///         <term>Value</term>
        ///         <description>Data represented</description>
        ///     </listheader>
        ///     <item>
        ///         <term>8</term>
        ///         <description>Character or unsigned 8-bit binary integer.</description>
        ///     </item>
        ///     <item>
        ///         <term>16</term>
        ///         <description>Signed 16-bit binary integer.</description>
        ///     </item>
        ///     <item>
        ///         <term>32</term>
        ///         <description>Signed 32-bit binary integer.</description>
        ///     </item>
        ///     <item>
        ///         <term>64</term>
        ///         <description>Signed 64-bit binary integer.</description>
        ///     </item>
        ///     <item>
        ///         <term>-32</term>
        ///         <description>IEEE single precision floating point.</description>
        ///     </item>
        ///     <item>
        ///         <term>-64</term>
        ///         <description>IEEE double precision floating point.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public int Bitpix
        {
            get
            {
                if (_simpleValidated == ValidateResult.NotValidated)
                    ValidateSimple();
                if (_simpleValidated == ValidateResult.Invalid)
                    throw new FitsFormatException("Invalid SIMPLE FITS header");
                return _bitpix;
            }
        }

        /// <summary>
        /// Number of axes in FITS data. A value of zero signifies that no data follow the header.
        /// </summary>
        public int Naxis
        {
            get
            {
                if (_simpleValidated == ValidateResult.NotValidated)
                    ValidateSimple();
                if (_simpleValidated == ValidateResult.Invalid)
                    throw new FitsFormatException("Invalid SIMPLE FITS header");
                return _naxis;
            }
        }

        /// <summary>
        /// Dimensions of matrix representing FITS data.
        /// </summary>
        public int[] Dimensions
        {
            get
            {
                if (_simpleValidated == ValidateResult.NotValidated)
                    ValidateSimple();
                if (_simpleValidated == ValidateResult.Invalid)
                    throw new FitsFormatException("Invalid SIMPLE FITS header");
                return (int[])_dimensions.Clone();
            }
        }
    }
}
