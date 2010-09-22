using SARA.Core;

namespace SARA.SimpleGUI.Image
{
    /// <summary>
    /// Encapsulation of ImageForm that creates new form, when older was closed.
    /// </summary>
    public class ImageWindow
    {
        private ImageForm _imgForm = null;

        /// <summary>
        /// Create new ImageWindow, but don't show anything.
        /// </summary>
        public ImageWindow()
        {
        }

        /// <summary>
        /// Show image.
        /// </summary>
        /// <param name="image">
        /// Image to show.
        /// </param>
        public void ShowImage(FloatMatrix image)
        {
            if (_imgForm == null || _imgForm.IsClosed)
                _imgForm = ImageForm.ShowImage(image);
            else
                _imgForm.SetImage(image);
        }

        /// <summary>
        /// Read only encapsulated ImageForm
        /// </summary>
        public ImageForm ImgForm
        {
            get { return _imgForm; }
        }
    }
}
