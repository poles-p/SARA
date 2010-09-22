using System;
using SARA.Core;

namespace SARA.Astrometry
{
    /// <summary>
    /// Simple and fast star tracker that find star by maximum value on pixel.
    /// </summary>
    /// <remarks>
    /// For this tracker, position of object is a position of brightness pixel. 
    /// Its fast method, but not the best. When seeing was bad, stars take up lot of
    /// pixels on image and this method do not show center of star. Moreover this
    /// method is very sensitive about cosmics.
    /// </remarks>
    public class StarTrackerByMax : IStarTracker
    {
        private Vector2D _position;
        private float _tolerance;

        private IStarTracker _reference;
        private bool _movable;
        private Vector2D _relPosition;

        #region Constructors

        /// <summary>
        /// Creates StarTrackerByMax with no Reference and default Tolerance.
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        public StarTrackerByMax(Vector2D position)
        {
            _position = position;
            _tolerance = 10.0f;
            _reference = null;
        }

        /// <summary>
        /// Creates StarTrackerByMax with no Reference.
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        /// <param name="tolerance">
        /// Tolerance in star tracking.
        /// </param>
        public StarTrackerByMax(Vector2D position, float tolerance)
        {
            _position = position;
            _tolerance = tolerance;
            _reference = null;
        }

        /// <summary>
        /// Creates StarTrackerByMax for static object(star) with default Tolerance.
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        /// <param name="reference">
        /// Reference star in tracking, null means no reference.
        /// </param>
        public StarTrackerByMax(Vector2D position, IStarTracker reference)
        {
            _position = position;
            _tolerance = 10.0f;
            _reference = reference;
            _movable = false;
            if (_reference != null)
                _relPosition = _position - _reference.Position;
        }

        /// <summary>
        /// Creates StarTrackerByMax for static object(star).
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        /// <param name="tolerance">
        /// Tolerance in star tracking.
        /// </param>
        /// <param name="reference">
        /// Reference star in tracking, null means no reference.
        /// </param>
        public StarTrackerByMax(Vector2D position, float tolerance, IStarTracker reference)
        {
            _position = position;
            _tolerance = tolerance;
            _reference = reference;
            _movable = false;
            if (_reference != null)
                _relPosition = _position - _reference.Position;
        }

        /// <summary>
        /// Creates StarTrackerByMax with default Tolerance.
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        /// <param name="reference">
        /// Reference star in tracking, null means no reference.
        /// </param>
        /// <param name="movable">
        /// Flag sets for movable object (planetoids, minor planets) and clear for static objects (stars).
        /// </param>
        public StarTrackerByMax(Vector2D position, IStarTracker reference, bool movable)
        {
            _position = position;
            _tolerance = 10.0f;
            _reference = reference;
            _movable = movable;
            if (_reference != null)
                _relPosition = _position - _reference.Position;
        }

        /// <summary>
        /// Creates StarTrackerByMax.
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        /// <param name="tolerance">
        /// Tolerance in star tracking.
        /// </param>
        /// <param name="reference">
        /// Reference star in tracking, null means no reference.
        /// </param>
        /// <param name="movable">
        /// Flag sets for movable object (planetoids, minor planets) and clear for static objects (stars)
        /// </param>
        public StarTrackerByMax(Vector2D position, float tolerance, IStarTracker reference, bool movable)
        {
            _position = position;
            _tolerance = tolerance;
            _reference = reference;
            _movable = movable;
            if (_reference != null)
                _relPosition = _position - _reference.Position;
        }

        /// <summary>
        /// Creates StarTrackerByMax for static object(star) with default Tolerance.
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        /// <param name="reference">
        /// Reference star in tracking, null means no reference.
        /// </param>
        /// <param name="relPosition">
        /// Position relative to reference star.
        /// </param>
        public StarTrackerByMax(Vector2D position, IStarTracker reference, Vector2D relPosition)
        {
            _position = position;
            _tolerance = 10.0f;
            _reference = reference;
            _movable = false;
            _relPosition = relPosition;
        }

        /// <summary>
        /// Creates StarTrackerByMax for static object(star).
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        /// <param name="tolerance">
        /// Tolerance in star tracking.
        /// </param>
        /// <param name="reference">
        /// Reference star in tracking, null means no reference.
        /// </param>
        /// <param name="relPosition">
        /// Position relative to reference star.
        /// </param>
        public StarTrackerByMax(Vector2D position, float tolerance, IStarTracker reference, Vector2D relPosition)
        {
            _position = position;
            _tolerance = tolerance;
            _reference = reference;
            _movable = false;
            _relPosition = relPosition;
        }

        /// <summary>
        /// Creates StarTrackerByMax with default Tolerance.
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        /// <param name="reference">
        /// Reference star in tracking, null means no reference.
        /// </param>
        /// <param name="movable">
        /// Flag sets for movable object (planetoids, minor planets) and clear for static objects (stars).
        /// </param>
        /// <param name="relPosition">
        /// Position relative to reference star.
        /// </param>
        public StarTrackerByMax(Vector2D position, IStarTracker reference, bool movable, Vector2D relPosition)
        {
            _position = position;
            _tolerance = 10.0f;
            _reference = reference;
            _movable = movable;
            _relPosition = relPosition;
        }

        /// <summary>
        /// Creates StarTrackerByMax.
        /// </summary>
        /// <param name="position">
        /// Initial position of tracked object.
        /// </param>
        /// <param name="tolerance">
        /// Tolerance in star tracking.
        /// </param>
        /// <param name="reference">
        /// Reference star in tracking, null means no reference.
        /// </param>
        /// <param name="movable">
        /// Flag sets for movable object (planetoids, minor planets) and clear for static objects (stars).
        /// </param>
        /// <param name="relPosition">
        /// Position relative to reference star.
        /// </param>
        public StarTrackerByMax(Vector2D position, float tolerance, IStarTracker reference, bool movable, Vector2D relPosition)
        {
            _position = position;
            _tolerance = tolerance;
            _reference = reference;
            _movable = movable;
            _relPosition = relPosition;
        }

        #endregion

        #region Public Parametrs

        /// <summary>
        /// Tolerace in star tracking.
        /// </summary>
        /// <remarks>
        /// Determines maximum distance between looking for and expected star position.
        /// </remarks>
        /// <seealso cref="Reference"/>
        /// <seealso cref="Movable"/>
        /// <seealso cref="RelPosition"/>
        public float Tolerance
        {
            get { return _tolerance; }
            set { _tolerance = value; }
        }

        /// <summary>
        /// Reference star in tracking.
        /// </summary>
        /// <remarks>
        /// When reference is set, expected position of star is calculated by formula : Reference.Position + RelPosition.
        /// Otherwise when Reference is null, expected position is last position of star.
        /// 
        /// On set, RelPosition is updated.
        /// </remarks>
        /// <seealso cref="Movable"/>
        /// <seealso cref="RelPosition"/>
        public IStarTracker Reference
        {
            get { return _reference; }
            set
            { 
                _reference = value;
                if (_reference != null)
                    _relPosition = _position - _reference.Position;
            }
        }

        /// <summary>
        /// Flag sets for movable object (planetoids, minor planets) and clear for static objects (stars).
        /// </summary>
        /// <remarks>
        /// When this flag is set, Track method updates RelPosition after tracking. When Reference is null, this flag is ignored.
        /// </remarks>
        /// <seealso cref="Reference"/>
        /// <seealso cref="RelPosition"/>
        /// <seealso cref="Track"/>
        public bool Movable
        {
            get { return _movable; }
            set { _movable = value; }
        }

        /// <summary>
        /// Relative position of star by reference star.
        /// </summary>
        /// <remarks>
        /// Determines expected position of star using position of reference star. When Reference is null, this value is ignored.
        /// </remarks>
        /// <seealso cref="Reference"/>
        /// <seealso cref="Movable"/>
        public Vector2D RelPosition
        {
            get { return _relPosition; }
            set { _relPosition = value; }
        }

        #endregion

        #region IStarTracker Members

        /// <summary>
        /// Track star method
        /// </summary>
        /// <remarks>
        /// Find selected star on image. Updates Position property, when Movable flag is set, and Reference is not null, 
        /// also updates RelPosition property.
        /// </remarks>
        /// <param name="image">
        /// An image to find selected star.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// Thrown when specified image is not 2D matrix.
        /// </exception>
        /// <exception cref="SARA.Astrometry.StarEscapedException">
        /// Thrown when field where star is looking for is completly out of image.
        /// </exception>
        public void Track(FloatMatrix image)
        {
            if (image.Dimensions.Length != 2)
                throw new ArgumentException("Expected 2D matrix");

            if (_reference != null)
                _position = _reference.Position + _relPosition;

            int minX = (int)(_position.X - _tolerance);
            int maxX = (int)(_position.X + _tolerance);
            int minY = (int)(_position.Y - _tolerance);
            int maxY = (int)(_position.Y + _tolerance);

            if (maxX < 0 || maxY < 0 || minX >= image.Dimensions[0] || minY >= image.Dimensions[1])
                throw new StarEscapedException("Star escaped");

            if (minX < 0) minX = 0;
            if (minY < 0) minY = 0;
            if (maxX >= image.Dimensions[0]) maxX = image.Dimensions[0] - 1;
            if (maxY >= image.Dimensions[1]) maxY = image.Dimensions[1] - 1;

            int pos0 = minY * image.Dimensions[0];
            _position = new Vector2D((float)minX, (float)minY);
            float best = image.Data[pos0 + minX];

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (best < image.Data[pos0 + x])
                    {
                        best = image.Data[pos0 + x];
                        _position = new Vector2D((float)x, (float)y);
                    }
                }
                pos0 += image.Dimensions[0];
            }

            if (_reference != null && _movable)
                _relPosition = _position - _reference.Position;
        }

        /// <summary>
        /// Last position of selected star.
        /// </summary>
        /// <remarks>
        /// Value updated every Track method call.
        /// </remarks>
        public SARA.Core.Vector2D Position
        {
            get { return _position; }
            set { _position = value; }
        }

        #endregion
    }
}
