using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SARA.Core;

namespace SARA.SimpleGUI.Plot
{
    //! Internal class used by PlotForm
    internal class PlotPanel
        : Panel
    {
        //! Data sets to plot
        public List<DataSet> DataSets = new List<DataSet>();

        public float MinX = -1.0f;
        public float MaxX = 1.0f;
        public float MinY = -1.0f;
        public float MaxY = 1.0f;

        //! Create new PlotPanel
        public PlotPanel()
        {
            Resize += delegate(object sender, EventArgs e) { Refresh(); };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (var set in DataSets)
            {
                foreach (var data in set.Data)
                    set.PlotPoint.DrawPoint(e.Graphics, data, Transform);
            }
        }

        private float LinearTransformX(float x)
        {
            return (float)Size.Width * (x - MinX) / (MaxX - MinX);
        }

        private float LinearTransformY(float y)
        {
            return (float)Size.Height * (1.0f - (y - MinY) / (MaxY - MinY));
        }

        public Point Transform(Vector2D data)
        {
            return new Point((int)LinearTransformX(data.X), (int)LinearTransformY(data.Y));
        }
    }
}
