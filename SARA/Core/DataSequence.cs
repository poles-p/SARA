using System.Collections.Generic;
using System.Linq;
using SARA.FITS;

namespace SARA.Core
{
    /// <summary>
    /// Basic sequence processing.
    /// </summary>
    public static class DataSequence
    {
        /// <summary>
        /// Create new FloatMatrix sequence from FITS files.
        /// </summary>
        /// <param name="paths">
        /// Paths array of paths to FITS files to create sequence.
        /// </param>
        /// <returns>
        /// FloatMatrix sequence from FITS files.
        /// </returns>
        public static IEnumerable<FloatMatrix> GetFitsFloatMatrixSequence(string[] paths)
        {
            return from path in paths
                   select (new FitsFile(path)).Data.ToFloatMatrix();
        }
    }
}
