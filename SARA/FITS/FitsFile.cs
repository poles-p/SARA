using System;
using System.IO;
using SARA.Core;

namespace SARA.FITS
{
    /// <summary>
    /// Data of FITS file. FITS (Flexibile Image Transport System) is popular format of images in astronomy.
    /// </summary>
    public class FitsFile
    {
        private FitsHeader     _header;
        private BaseDataMatrix _data;

        /// <summary>
        /// Open and read FITS file.
        /// </summary>
        /// <param name="path">
        /// The path to FITS file.
        /// </param>
        public FitsFile(string path)
        {
            Stream stream = File.OpenRead(path);
            BinaryReader reader = new BinaryReader(stream);

            _header = new FitsHeader(reader);

            switch (_header.Bitpix)
            {
            case -64:
                {
                    DataMatrix<double> doubleMap = new DataMatrix<double>(_header.Dimensions);
                    byte[] doubleRaw = reader.ReadBytes(doubleMap.Size * 8);
                    Buffer.BlockCopy(doubleRaw, 0, doubleMap.Data, 0, doubleMap.Size * 8);
                    BaseTypeOperations.ReverseArray<double>(doubleMap.Data);
                    _data = doubleMap;
                    break;
                }
            case -32:
                {
                    DataMatrix<float> floatMap = new DataMatrix<float>(_header.Dimensions);
                    byte[] floatRaw = reader.ReadBytes(floatMap.Size * 4);
                    Buffer.BlockCopy(floatRaw, 0, floatMap.Data, 0, floatMap.Size * 4);
                    BaseTypeOperations.ReverseArray<float>(floatMap.Data);
                    _data = floatMap;
                    break;
                }
            case 8:
                {
                    DataMatrix<byte> byteMap = new DataMatrix<byte>(_header.Dimensions);
                    byte[] byteRaw = reader.ReadBytes(byteMap.Size);
                    Buffer.BlockCopy(byteRaw, 0, byteMap.Data, 0, byteMap.Size);
                    _data = byteMap;
                    break;
                }
            case 16:
                {
                    DataMatrix<short> shortMap = new DataMatrix<short>(_header.Dimensions);
                    byte[] shortRaw = reader.ReadBytes(shortMap.Size * 2);
                    Buffer.BlockCopy(shortRaw, 0, shortMap.Data, 0, shortMap.Size * 2);
                    BaseTypeOperations.ReverseArray<short>(shortMap.Data);
                    _data = shortMap;
                    break;
                }
            case 32:
                {
                    DataMatrix<int> intMap = new DataMatrix<int>(_header.Dimensions);
                    byte[] intRaw = reader.ReadBytes(intMap.Size * 4);
                    Buffer.BlockCopy(intRaw, 0, intMap.Data, 0, intMap.Size * 4);
                    BaseTypeOperations.ReverseArray<int>(intMap.Data);
                    _data = intMap;
                    break;
                }
            case 64:
                {
                    DataMatrix<long> longMap = new DataMatrix<long>(_header.Dimensions);
                    byte[] longRaw = reader.ReadBytes(longMap.Size * 8);
                    Buffer.BlockCopy(longRaw, 0, longMap.Data, 0, longMap.Size * 8);
                    BaseTypeOperations.ReverseArray<long>(longMap.Data);
                    _data = longMap;
                    break;
                }
            }

            reader.Close();
            stream.Close();
        }

        /// <summary>
        /// Header of FITS file.
        /// </summary>
        public FitsHeader Header
        {
            get { return _header; }
        }

        /// <summary>
        /// Main data of FITS file.
        /// </summary>
        public BaseDataMatrix Data
        {
            get { return _data; }
        }
    }
}
