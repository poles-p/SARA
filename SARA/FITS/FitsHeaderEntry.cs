using System;
using System.IO;

namespace SARA.FITS
{
    /// <summary>
    /// Entry of FITS header.
    /// </summary>
    public class FitsHeaderEntry
    {
        private byte[] data;
        private string _keyword;
        private bool   _hasValue;

        private bool _hasIntValue;
        private long _intValue;

        /// <summary>
        /// Constructor that creates new <see cref="FitsHeaderEntry"/> from raw 80-bytes data.
        /// </summary>
        /// <param name="entryData">
        /// Raw data of entry.
        /// </param>
        public FitsHeaderEntry(byte[] entryData)
        {
            if (entryData.Length < 80)
                throw new FitsFormatException("Too short FITS header entry (expected 80 bytes)");

            data = new byte[80];
            Array.Copy(entryData, data, 80);

            _hasValue = (data[8] == '=' && data[9] == ' ');

            unsafe
            {
                fixed (byte* dataPtr0 = data)
                {
                    sbyte* dataPtr = (sbyte*)dataPtr0;
                    
                    int i = 7;
                    while (i >= 0 && dataPtr[i] == ' ') 
                        i--;
                    
                    _keyword = new string(dataPtr, 0, i+1);

                    if (_hasValue)
                    {
                        _hasIntValue = long.TryParse(new string(dataPtr, 10, 20), out _intValue);
                    }
                }
            }
        }

        /// <summary>
        /// Keyword in entry.
        /// </summary>
        public string Keyword
        {
            get { return _keyword; }
        }

        /// <summary>
        /// Value indicating that entry contains any value.
        /// </summary>
        public bool HasValue
        {
            get { return _hasValue; }
        }

        /// <summary>
        /// Value indicating that entry contains bool value.
        /// </summary>
        public bool HasBoolValue
        {
            get { return _hasValue && (data[29] == 'T' || data[29] == 'F'); }
        }

        /// <summary>
        /// Bool value of entry.
        /// </summary>
        public bool BoolValue
        {
            get
            {
                if (!_hasValue)
                    throw new FitsHeaderEntryException("No value");
                else if (data[29] == 'T')
                    return true;
                else if (data[29] == 'F')
                    return false;
                else
                    throw new FitsHeaderEntryException("No bool value");
            }
        }

        /// <summary>
        /// Value indicating that entry contains integer value.
        /// </summary>
        public bool HasIntValue
        {
            get { return _hasValue && _hasIntValue; }
        }

        /// <summary>
        /// Integer value of entry.
        /// </summary>
        public long IntValue
        {
            get
            {
                if (!_hasValue)
                    throw new FitsHeaderEntryException("No value");
                else if (!_hasIntValue)
                    throw new FitsHeaderEntryException("No int value");
                else
                    return _intValue;
            }
        }

        /// <summary>
        /// Read <see cref="FitsHeaderEntry"/> from stream.
        /// </summary>
        /// <param name="reader">
        /// Stream that contains a FITS header entry.
        /// </param>
        /// <returns>
        /// Entry read from stream.
        /// </returns>
        public static FitsHeaderEntry ReadEntry(BinaryReader reader)
        {
            return new FitsHeaderEntry( reader.ReadBytes(80) );
        }
    }
}
