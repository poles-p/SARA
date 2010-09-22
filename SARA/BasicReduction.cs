using System.Collections.Generic;
using System.Linq;
using SARA.Core;

namespace SARA
{
    /// <summary>
    /// Static class with method of basic reduction.
    /// </summary>
    public static class BasicReduction
    {
        /// <summary>
        /// Calculate mean image of sequene (every pixel contains mean of corresponding pixels values from sequence).
        /// </summary>
        /// <param name="sequecne">
        /// Sequence to calculate mean. Every images have to have same dimensions.
        /// </param>
        /// <returns>
        /// Mean of sequence.
        /// </returns>
        public static FloatMatrix SequenceMean(IEnumerable<FloatMatrix> sequecne)
        {
            FloatMatrix result = new FloatMatrix(new DataMatrix<float>(sequecne.First().Dimensions));
            int num = 0;

            foreach (FloatMatrix bias in sequecne)
            {
                result.Add(bias);
                num++;
            }
            result.Divide((float)num);

            return result;
        }

        /// <summary>
        /// Calculate sum of images in sequene (every pixel contains sum of corresponding pixels values from sequence).
        /// </summary>
        /// <param name="sequecne">
        /// Sequence to calculate sum. Every images have to have same dimensions.
        /// </param>
        /// <returns>
        /// Sum of sequence.
        /// </returns>
        public static FloatMatrix SequenceSum(IEnumerable<FloatMatrix> sequecne)
        {
            FloatMatrix result = new FloatMatrix(new DataMatrix<float>(sequecne.First().Dimensions));
            
            foreach (FloatMatrix bias in sequecne)
                result.Add(bias);
                        
            return result;
        }

        /// <summary>
        /// Norm image. Create new image from specified image, where mean value of pixels is 1. Function does not change source image.
        /// </summary>
        /// <param name="image">
        /// Image to norm.
        /// </param>
        /// <returns>
        /// Normed image.
        /// </returns>
        public static FloatMatrix NormImage(FloatMatrix image)
        {
            float pixMean = 0.0f;
            foreach (float v in image.Data)
            {
                pixMean += v;
            }

            return image / (pixMean / image.Size);
        }

        /// <summary>
        /// Calculate master bias of bias sequence. Master bias is a mean of sequene, so that function
        /// encapsulates <see cref="SequenceMean"/>.
        /// </summary>
        /// <param name="biasSequecne">
        /// Sequence of bias images.
        /// </param>
        /// <returns>
        /// Calculated master bias.
        /// </returns>
        public static FloatMatrix MasterBias(IEnumerable<FloatMatrix> biasSequecne)
        {
            return SequenceMean(biasSequecne);
        }

        /// <summary>
        /// Calculate master flat of specified flat sequence. Function subtract bias from every flat image,
        /// and returns normed sum of obtained images.
        /// </summary>
        /// <param name="flatSequence">
        /// Sequence of flat images.
        /// </param>
        /// <param name="bias">
        /// master bias for flat.
        /// </param>
        /// <returns>
        /// Calculated master flat.
        /// </returns>
        public static FloatMatrix MasterFlat(IEnumerable<FloatMatrix> flatSequence, FloatMatrix bias)
        {
            FloatMatrix result = SequenceMean(flatSequence);
            result.Subtract(bias);

            // Norm flat
            float pixMean = 0.0f;
            foreach (float v in result.Data)
                pixMean += v;

            result.Divide(pixMean / (float)result.Size);

            return result;
        }

        /// <summary>
        /// Calculate master flat of specified flat sequence. Function calculate master bias for flat, subtract it from every flat image,
        /// and returns normed sum of obtained images.
        /// </summary>
        /// <param name="flatSequence">
        /// Sequence of flat images.
        /// </param>
        /// <param name="biasSequence">
        /// Sequence of bias images for flat.
        /// </param>
        /// <returns>
        /// Calculated master flat.
        /// </returns>
        public static FloatMatrix MasterFlat(IEnumerable<FloatMatrix> flatSequence, IEnumerable<FloatMatrix> biasSequence)
        {
            return MasterFlat(flatSequence, MasterBias(biasSequence));
        }

        /// <summary>
        /// Calcule master flat of flat sequence. Flat is normed sum of images, so you can use
        /// <c>NormImage(SequenceSum(<paramref name="flatSequence"/>))</c>,
        /// but this function is a little bit faster and more pretty.
        /// </summary>
        /// <param name="flatSequence">
        /// Sequence of flat images.
        /// </param>
        /// <returns>
        /// Calculated master flat.
        /// </returns>
        public static FloatMatrix MasterFlat(IEnumerable<FloatMatrix> flatSequence)
        {
            FloatMatrix result = SequenceSum(flatSequence);
            
            // Norm flat
            float pixMean = 0.0f;
            foreach (float v in result.Data)
                pixMean += v;

            result.Divide(pixMean / (float)result.DataMatrix.Size);

            return result;
        }

        /// <summary>
        /// Debias data. Subtract master bias from every image in sequence.
        /// </summary>
        /// <param name="dataSequence">
        /// Data to debias.
        /// </param>
        /// <param name="masterBias">
        /// Master bias for specified data.
        /// </param>
        /// <returns>
        /// Sequence of debiased data.
        /// </returns>
        public static IEnumerable<FloatMatrix> Debias(IEnumerable<FloatMatrix> dataSequence, FloatMatrix masterBias)
        {
            return from m in dataSequence select m - masterBias;
        }

        /// <summary>
        /// Deflat data. Divide every image in sequence by master flat.
        /// </summary>
        /// <param name="dataSequence">
        /// Data to daflat.
        /// </param>
        /// <param name="masterFlat">
        /// Master flat for specified data.
        /// </param>
        /// <returns>
        /// Sequence of deflated data.
        /// </returns>
        public static IEnumerable<FloatMatrix> Deflat(IEnumerable<FloatMatrix> dataSequence, FloatMatrix masterFlat)
        {
            return from m in dataSequence select m / masterFlat;
        }

        /// <summary>
        /// Debias and deflat data.
        /// </summary>
        /// <param name="dataSequence">
        /// Data to reduct.
        /// </param>
        /// <param name="masterBias">
        /// Master bias for specified data.
        /// </param>
        /// <param name="masterFlat">
        /// Master flat for specified data.
        /// </param>
        /// <returns>
        /// Reducted data.
        /// </returns>
        public static IEnumerable<FloatMatrix> Reduct(IEnumerable<FloatMatrix> dataSequence, FloatMatrix masterBias, FloatMatrix masterFlat)
        {
            return from m in dataSequence select (m - masterBias) / masterFlat;
        }
    }
}
