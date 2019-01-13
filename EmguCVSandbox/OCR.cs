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
        public static string DecodeImg(Bitmap image, List<Bitmap> library)
        {
            string num = "";

            List<Tuple<double, string>> results = new List<Tuple<double, string>>();

            foreach (Bitmap number in library)
            {
                num = (string)number.Tag;
                double ocrResult = ImageRecognition.SingleTemplateMatch(ImageFilters.SobelEdgeDetection( image), ImageFilters.SobelEdgeDetection(number));
                results.Add(new Tuple<double, string>(ocrResult, num + "@" + Math.Round(ocrResult, 1)));
            }

            double max = results.Select(m => m.Item1).Max();
            string valueToReturn = "";
            foreach (var item in results)
            {
                if (item.Item1==max)
                {
                    valueToReturn = item.Item2;
                }
            }
            return valueToReturn;
        }
    }
}
