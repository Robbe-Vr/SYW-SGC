using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudioControlGestureRecognition.ImageProcessing
{
    internal class ImageReader
    {
        internal static Bitmap? LoadImageFromFile(string imagePath)
        {
            try
            {
                Image img = Image.FromFile(imagePath);

                return new Bitmap(img);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);

                return null;
            }
        }
    }
}
