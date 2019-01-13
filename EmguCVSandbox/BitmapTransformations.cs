using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmguCVSandbox
{
    class BitmapTransformations
    {
        public static Bitmap Crop(Bitmap inputBitmap,Rectangle rec)
        {
            System.Drawing.Imaging.PixelFormat format =inputBitmap.PixelFormat;
            return inputBitmap.Clone(rec, format);
        }

        public static Bitmap PasteBitmap(Bitmap bigBitmap, Bitmap smallBitmap, Point insertionPoint)
        {
            using (var graphics = Graphics.FromImage(bigBitmap))
            {
                graphics.DrawImage(smallBitmap, insertionPoint);
            }
            return bigBitmap;   
        }
    }
}
