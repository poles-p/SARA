using System;

namespace SARA.Fotometry
{
    /// <summary>
    /// Result of aperture fotometry.
    /// </summary>
    public class FotometryResult
    {
        float _background;
        float _total;

        /// <summary>
        /// Create result of aperture fotometry (when background level is 0).
        /// </summary>
        /// <param name="total">
        /// Total signal of star and background.
        /// </param>
        public FotometryResult(float total)
        {
            _background = 0.0f;
            _total = total;
        }

        /// <summary>
        /// Create result of aperture fotometry for normed background and total signal.
        /// </summary>
        /// <param name="background">
        /// Signal of background.
        /// </param>
        /// <param name="total">
        /// Total signal of star and background.
        /// </param>
        public FotometryResult(float background, float total)
        {
            _background = background;
            _total = total;
        }

        /// <summary>
        /// Create result of aperture fotometry.
        /// </summary>
        /// <param name="background">
        /// Signal of background.
        /// </param>
        /// <param name="bkgPixels">
        /// Number of pixels used to measure background signal.
        /// </param>
        /// <param name="total">
        /// Total signal of star and background.
        /// </param>
        /// <param name="totPixels">
        /// Number of pixels used to measure total signal.
        /// </param>
        public FotometryResult(float background, int bkgPixels, float total, int totPixels)
        {
            _background = background / (float)bkgPixels;
            _total = total / (float)totPixels;
        }

        /// <summary>
        /// Normed signal of background.
        /// </summary>
        public float Background
        {
            get { return _background; }
        }

        /// <summary>
        /// Normed total signal of star and background.
        /// </summary>
        public float Total
        {
            get { return _total; }
        }

        /// <summary>
        /// Normed signal of star (<see cref="Total"/> - <see cref="Background"/>).
        /// </summary>
        public float Signal
        {
            get { return _total - _background; }
        }

        /// <summary>
        /// Star brightness in magnitudo.
        /// </summary>
        public float Magnitudo
        {
            get { return -2.5f * (float)Math.Log10(_total - _background); }
        }
    }
}
