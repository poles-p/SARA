using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SARA.Astrometry;
using SARA.Core;
using SARA.Drawing;

namespace SARA.SimpleGUI.Image
{
    /// <summary>
    /// Form to drawing images.
    /// </summary>
    public class ImageForm : Form
    {
        private Bitmap _bitmap;
        private bool _isClosed = false;
        private List<IStarTracker> _stars = new List<IStarTracker>();

        private Queue<SelectionRequest> _selectionQueue = new Queue<SelectionRequest>();

        /// <summary>
        /// Create <see cref="ImageForm"/> that contains specified image.
        /// </summary>
        /// <param name="image">
        /// Image drawed on form.
        /// </param>
        /// <remarks>
        /// To simple show image use <see cref="ShowImage"/>.
        /// </remarks>
        public ImageForm(FloatMatrix image)
        {
            SetImage(image);

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        /// <summary>
        /// Set image to draw on form. New image will cover previous, and window will be resized to new image.
        /// </summary>
        /// <param name="image">
        /// New image to draw on form.
        /// </param>
        public void SetImage(FloatMatrix image)
        {
            float min = image.Data[0];
            float max = min;

            for (int i = 0; i < image.DataMatrix.Size; i++)
            {
                if (min > image.Data[i])
                    min = image.Data[i];
                else if (max < image.Data[i])
                    max = image.Data[i];
            }

            LinearMatrixDrawer drawer = new LinearMatrixDrawer(min, max);
            _bitmap = drawer.ToBitmap(image);

            ClientSize = _bitmap.Size;

            Refresh();
        }

        /// <summary>
        /// Add star, that will be marked on image.
        /// </summary>
        /// <param name="star">
        /// A star that will be marked on image.
        /// </param>
        public void AddStar(IStarTracker star)
        {
            _stars.Add(star);
            Refresh();
        }

        /// <summary>
        /// Wait until user select point on image by mouse click.
        /// </summary>
        /// <returns>
        /// Coordinates of selected point.
        /// </returns>
        public Vector2D SelectPoint()
        {
            this.Cursor = Cursors.Cross;

            SelectionRequest req = new SelectionRequest();
            _selectionQueue.Enqueue(req);

            req.waitEvent.WaitOne();
            req.waitEvent.Close();

            return req.position;
        }

        /// <summary>
        /// When this value is true, form is closed.
        /// </summary>
        public bool IsClosed
        {
            get { return _isClosed; }
        }

        /// <summary>
        /// Paint image.
        /// </summary>
        /// <param name="e">
        /// A <see cref="PaintEventArgs"/> that contains the event data. 
        /// </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawImage(_bitmap, 0, 0);
            foreach (IStarTracker star in _stars)
                e.Graphics.DrawEllipse(Pens.Red, star.Position.X - 8.0f, (float)(_bitmap.Height-1) - star.Position.Y - 8.0f, 16.0f, 16.0f);
        }

        /// <summary>
        /// Method called on mouse click.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data. 
        /// </param>
        protected override void OnClick(EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (_selectionQueue.Count > 0)
            {
                SelectionRequest req = _selectionQueue.Dequeue();

                req.position.X = (float)me.X;
                req.position.Y = (float)(_bitmap.Height - 1 - me.Y);

                if (_selectionQueue.Count == 0)
                    this.Cursor = Cursors.Default;

                req.waitEvent.Set();
            }
        }

        private void ThreadFunction()
        {
            ShowDialog();
            _isClosed = true;
        }

        /// <summary>
        /// Creates new <see cref="ImageForm"/> to show specified image. Form use another thread,
        /// so program is not blocked.
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static ImageForm ShowImage(FloatMatrix image)
        {
            ImageForm form = new ImageForm(image);

            Thread thread = new Thread(new ThreadStart(form.ThreadFunction));
            thread.Start();

            return form;
        }

        private class SelectionRequest
        {
            public System.Threading.EventWaitHandle waitEvent;
            public Vector2D position;

            public SelectionRequest()
            {
                waitEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
                position = new Vector2D(0.0f, 0.0f);
            }
        }
    }
}
