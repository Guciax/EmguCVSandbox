using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmguCVSandbox
{
    public class NewRecognition
    {
        public static Point[] pointsOfInterest(Bitmap image)
        {
            //Bitmap ss = ScreenShot.GetScreenShop("Lord of the Rings - LCG");
            //Bitmap fullEnemyCrop = BitmapTransformations.Crop(ScreenShot.GetScreenShop("Lord of the Rings - LCG"), new Rectangle(260, 350, 1170, 190));
            Image<Bgr, byte> ssImage = new Image<Bgr, byte>(image);

            Bitmap noEnemyBmp = new Bitmap(@"Images\noEnemyImage.PNG");
            Bitmap noEnemyCrop = BitmapTransformations.Crop(noEnemyBmp, new Rectangle(260, 350, 1170, 190));
            Image<Bgr, byte> emptyImage = new Image<Bgr, byte>(noEnemyCrop);

            var sub = ssImage - emptyImage;
            var filtered = sub.Convert<Gray, byte>().ThresholdBinary(new Gray(70), new Gray(255)).Bitmap;



            int mobSocketWidth = 143;

            int mobStartA = 0;
            int mobStartB = 0;


            for (int x = 1; x < filtered.Width; x++)
            {
                var pixel = filtered.GetPixel(x, 162);
                if (pixel.R > 5 & pixel.G > 5 & pixel.B > 5)
                {
                    double rest = (filtered.Width - 2 * x) / mobSocketWidth - Math.Round(((double)filtered.Width - 2 * x) / mobSocketWidth, 0);
                    if (rest > 0.9 || rest < 0.1)
                    {
                        mobStartA = x;
                        break;
                    }
                }
            }

            for (int x = 1; x < filtered.Width; x++)
            {
                var pixel = filtered.GetPixel(x, filtered.Height / 2);
                if (pixel.R > 5 & pixel.G > 5 & pixel.B > 5)
                {
                    mobStartB = x;
                    break;
                }
            }

            int mobAreaWidth = filtered.Width - 2 * (Math.Min(mobStartA, mobStartB) - 5);
            int mobCount = (int)Math.Round((double)mobAreaWidth / mobSocketWidth, 0);

            int mobStart = (filtered.Width - mobCount * mobSocketWidth) / 2;

            List<Point> result = new List<Point>();
            for (int rX = 0; rX < mobCount; rX++)
            {
                result.Add(new Point(mobStart + rX * mobSocketWidth + mobSocketWidth / 2, image.Height / 2));
            }

            return result.ToArray();

            //using (Graphics g = Graphics.FromImage(fullEnemyCrop))
            //{
            //    for (int rX = 0; rX < mobCount; rX++)
            //    {
            //        g.DrawRectangle(new Pen(Color.Blue), new Rectangle(mobStart + rX * mobSocketWidth, 0, mobSocketWidth, fullEnemyCrop.Height));
            //    }
            //}

                //pictureBox3.Image = fullEnemyCrop;
        }
    }
}
