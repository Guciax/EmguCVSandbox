using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace EmguCVSandbox
{
    class OCR
    {
        struct ValueScore { int value; double score; }

        public static int DecodeImg(Bitmap windowScreenshot, Rectangle cropRectangle, List<Bitmap> library, double binarisationThreshold=0)
        {
            int loopCounter = 0;
            Bitmap crop = BitmapTransformations.Crop(windowScreenshot, cropRectangle);
            int result = 0;
            double maxScore = 0;
            Bitmap noColor;
            bool failedTheFirstTime = false;
            do
            {
                if (failedTheFirstTime)
                {
                    crop = BitmapTransformations.Crop(ScreenShot.GetScreenShot(Windows.GameWindowRectangle()), cropRectangle);
                    
                }
                string num = "";

                List<Tuple<double, string>> results = new List<Tuple<double, string>>();
                noColor = ImageFilters.RemoveColorFromImage(crop, 75);
                var img = new Image<Bgr, byte>(noColor);
                //Bitmap binarisedBmp = img.Convert<Gray, byte>().ThresholdBinary(new Gray(200), new Gray(255)).Bitmap;

                foreach (Bitmap number in library)
                {
                    num = (string)number.Tag;

                    double ocrResult = ImageRecognition.SingleTemplateMatch(noColor, number, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
                    //number.Save(@"Images\ocrResult\"+ num + "@" + Math.Round(ocrResult, 2)+".png");
                    results.Add(new Tuple<double, string>(ocrResult, num));
                }

                maxScore = results.Select(m => m.Item1).Max();

                foreach (var item in results)
                {
                    if (item.Item1 == maxScore)
                    {
                        result = int.Parse(item.Item2);
                    }
                }

                failedTheFirstTime = true;
                loopCounter++;
                //Debug.WriteLine($"{petlaexit}.Max: {maxScore}");
            } while (maxScore < 0.55 & loopCounter < 10);
            string tesserResult = TesseractOcr(noColor);

            return result;
        }

        public static string TesseractOcr(Bitmap bmp)
        {
            string result = "";
            using (var engine = new TesseractEngine(@"tessdata/", "eng"))
            {
                engine.SetVariable("tessedit_char_whitelist", "0123456789");

                using (var img = bmp)
                using (var page = engine.Process(img))
                using (var iterator = page.GetIterator())
                {
                    Console.WriteLine(page.GetText());
                    iterator.Begin();

                    result= iterator.GetText(PageIteratorLevel.Word);
                }
            }
            return result;
        }
    }
}
