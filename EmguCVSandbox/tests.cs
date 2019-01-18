using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmguCVSandbox.Form1;

namespace EmguCVSandbox
{
    class tests
    {
        public static Bitmap pointsOfInterest(Bitmap emptyFullImage)
        {
            Bitmap ss = ScreenShot.GetScreenShop(Windows.GameWindowRectangle());
            Bitmap ssCropImage = BitmapTransformations.Crop(ScreenShot.GetScreenShop(Windows.GameWindowRectangle()), GlobalParameters.heroRegion);
            Image<Bgr, byte> ssImage = new Image<Bgr, byte>(ssCropImage);

            Bitmap emptyCropImage = BitmapTransformations.Crop(emptyFullImage, GlobalParameters.heroRegion);
            Image<Bgr, byte> emptyImage = new Image<Bgr, byte>(emptyCropImage);


            var sub = ssImage - emptyImage;
            var filtered = sub.Convert<Gray, byte>().ThresholdBinary(new Gray(90), new Gray(255)).Bitmap;

            int mobSocketWidth = 150;

            int mobStartA = 0;
            int mobStartB = 0;

            bool lineAdone = false;
            bool lineBdone = false;

            for (int x = 1; x < filtered.Width; x++)
            {
                var pixelA = filtered.GetPixel(x, 162);
                var pixelB = filtered.GetPixel(x, 162);

                if (!lineAdone)
                {
                    if (pixelA.R > 5 & pixelA.G > 5 & pixelA.B > 5)
                    {
                        //double rest = (filtered.Width - 2 * x) / mobSocketWidth - Math.Round(((double)filtered.Width - 2 * x) / mobSocketWidth, 0);
                        //if (rest > 0.9 || rest < 0.1)
                        {
                            mobStartA = x;
                            lineAdone = true;
                        }
                    }
                }
                if (!lineBdone)
                {
                    if (pixelB.R > 5 & pixelB.G > 5 & pixelB.B > 5)
                    {
                        //double rest = (filtered.Width - 2 * x) / mobSocketWidth - Math.Round(((double)filtered.Width - 2 * x) / mobSocketWidth, 0);
                        //if (rest > 0.9 || rest < 0.1)
                        {
                            mobStartB = x;
                            lineBdone = true;
                        }
                    }

                }
                if (lineAdone & lineBdone) break;
            }


            int mobStart = (Math.Min(mobStartA, mobStartB) - 5);
            int mobAreaWidth = filtered.Width - 2 * mobStart;
            double div = (double)mobAreaWidth / (double)mobSocketWidth;
            int mobCount = (int)Math.Round(div, 0);

            

            List<Point> result = new List<Point>();
            for (int rX = 0; rX < mobCount; rX++)
            {
                result.Add(new Point(mobStart + rX * mobSocketWidth + mobSocketWidth / 2, ssCropImage.Height / 2));
            }

            using (Graphics g = Graphics.FromImage(ssCropImage))
            {
                for (int i = 0; i < mobCount; i++)
                {
                    g.DrawRectangle(new Pen(Color.Red), new Rectangle(mobStart + i * mobSocketWidth, 0, mobSocketWidth, ssCropImage.Height));
                }
            }

            Bitmap combinedBmp = new Bitmap(ssCropImage.Width, ssCropImage.Height * 2);
            combinedBmp = BitmapTransformations.PasteBitmap(combinedBmp, ssCropImage, new Point(0, 0));
            combinedBmp = BitmapTransformations.PasteBitmap(combinedBmp, filtered, new Point(0, ssCropImage.Height));



            return combinedBmp;
        }

    }
}
