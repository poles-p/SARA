using System;
using SARA.Astrometry;

namespace SARA.Fotometry
{
    /// <summary>
    /// Aperture for fotometry that use three circles: signal is measured in first, 
    /// and background is measured in ring between second and third. 
    /// </summary>
    public class Aperture3R : IAperture
    {
        private float _radius1;
        private float _radius2;
        private float _radius3;
        private IStarTracker _star;

        /// <summary>
        /// Create new Aperture for fotometry.
        /// </summary>
        /// <param name="radius1">
        /// Radius of circle to measure signal of star (and background).
        /// </param>
        /// <param name="radius2">
        /// Inner radius of ring to measure background.
        /// </param>
        /// <param name="radius3">
        /// Outer radius of ring to measure background.
        /// </param>
        /// <param name="star">
        /// Measured star.
        /// </param>
        public Aperture3R(float radius1, float radius2, float radius3, IStarTracker star)
        {
            _radius1 = radius1;
            _radius2 = radius2;
            _radius3 = radius3;
            _star = star;
        }

        /// <summary>
        /// Radius of circle to measure signal of star (and background).
        /// </summary>
        public float Radius1
        {
            get { return _radius1; }
            set { _radius1 = value; }
        }

        /// <summary>
        /// Inner radius of ring to measure background.
        /// </summary>
        public float Radius2
        {
            get { return _radius2; }
            set { _radius2 = value; }
        }

        /// <summary>
        /// Outer radius of ring to measure background.
        /// </summary>
        public float Radius3
        {
            get { return _radius3; }
            set { _radius3 = value; }
        }

        /// <summary>
        /// Measured star.
        /// </summary>
        public IStarTracker Star
        {
            get { return _star; }
            set { _star = value; }
        }

        #region IAperture Members

        /// <summary>
        /// Measure brightness of star.
        /// </summary>
        /// <param name="image">
        /// Image that contains measured star.
        /// </param>
        /// <returns>
        /// Result of fotometry.
        /// </returns>
        public FotometryResult GetResult(SARA.Core.FloatMatrix image)
        {
            if (image.Dimensions.Length != 2)
                throw new ArgumentException("Expected 2D matrix");

            int minX = (int)(_star.Position.X - _radius3 - 1.0f);
            int maxX = (int)(_star.Position.X + _radius3 + 1.0f);
            int minY = (int)(_star.Position.Y - _radius3 - 1.0f);
            int maxY = (int)(_star.Position.Y + _radius3 + 1.0f);

            if (maxX < 0 || maxY < 0 || minX >= image.Dimensions[0] || minY >= image.Dimensions[1])
                throw new FotometryException("Star out of image");

            if (minX < 0) minX = 0;
            if (minY < 0) minY = 0;
            if (maxX >= image.Dimensions[0]) maxX = image.Dimensions[0] - 1;
            if (maxY >= image.Dimensions[1]) maxY = image.Dimensions[1] - 1;

            int pos0 = minY * image.Dimensions[0];
            
            float rsq1 = Sqr(_radius1);
            float rsq2 = Sqr(_radius2);
            float rsq3 = Sqr(_radius3);

            float total = 0.0f;
            int totPix = 0;
            float back = 0.0f;
            int backPix = 0;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    float rsq = Sqr((float)x - _star.Position.X) + Sqr((float)y - _star.Position.Y);
                    if (rsq <= rsq1)
                    {
                        total += image.Data[pos0 + x];
                        totPix++;
                    }
                    else if (rsq >= rsq2 && rsq <= rsq3)
                    {
                        back += image.Data[pos0 + x];
                        backPix++;
                    }
                }
                pos0 += image.Dimensions[0];
            }

            return new FotometryResult(back, backPix, total, totPix);
        }

        #endregion

        private float Sqr(float x)
        {
            return x * x;
        }
    }
}
