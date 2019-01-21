using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EmguCVSandbox
{
    class OCR
    {
        public static int DecodeImg(Bitmap windowScreenshot, Rectangle cropRectangle, List<Bitmap> library, double binarisationThreshold=0)
        {
            int petlaexit = 0;
            Bitmap crop = BitmapTransformations.Crop(windowScreenshot, cropRectangle);
            int result = 0;
            bool failedTheFirstTime = false;
            do
            {
                if (failedTheFirstTime)
                {
                    crop = BitmapTransformations.Crop(ScreenShot.GetScreenShot(Windows.GameWindowRectangle()), cropRectangle);
                }
                string num = "";

                List<Tuple<double, string>> results = new List<Tuple<double, string>>();
                Bitmap noColor = ImageFilters.RemoveColorFromImage(crop, 75);
                var img = new Image<Bgr, byte>(noColor);
                Bitmap binarisedBmp = img.Convert<Gray, byte>().ThresholdBinary(new Gray(200), new Gray(255)).Bitmap;

                foreach (Bitmap number in library)
                {
                    num = (string)number.Tag;

                    double ocrResult = ImageRecognition.SingleTemplateMatch(noColor, number, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
                    //number.Save(@"Images\ocrResult\"+ num + "@" + Math.Round(ocrResult, 2)+".png");
                    results.Add(new Tuple<double, string>(ocrResult, num));
                }

                double max = results.Select(m => m.Item1).Max();

                foreach (var item in results)
                {
                    if (item.Item1 == max)
                    {
                        result = int.Parse(item.Item2);
                    }
                }
                failedTheFirstTime = true;
                petlaexit = petlaexit++;
            } while (result < 0.9 || petlaexit > 30);
            
            return result;
        }
    }
}
