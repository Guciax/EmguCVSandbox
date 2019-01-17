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

        public static Bitmap[] TakeBitmapsInPoints(Bitmap inputBitmap, Point[] points, Size cropSize)
        {
            List<Bitmap> result = new List<Bitmap>();
            foreach (var point in points)
            {
                Point fixedPt = new Point(point.X - cropSize.Width / 2, point.Y - cropSize.Height / 2);
                Bitmap crop = Crop(inputBitmap,  new Rectangle(fixedPt.X,fixedPt.Y,cropSize.Width, cropSize.Height));
                crop.Tag = fixedPt;
                result.Add(crop);
            }
            return result.ToArray();
        }

    }
}
