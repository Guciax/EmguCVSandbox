using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmguCVSandbox
{
    class ImageFilters
    {
        public static Image<Gray, byte> SharpenGaussian(Bitmap bmp, int w, int h, double sigma1, double sigma2, int k)
        {
            Image<Gray, byte> image = new Image<Gray, byte>(bmp);
            w = (w % 2 == 0) ? w - 1 : w;
            h = (h % 2 == 0) ? h - 1 : h;
            //apply gaussian smoothing using w, h and sigma 
            var gaussianSmooth = image.SmoothGaussian(w, h, sigma1, sigma2);
            //obtain the mask by subtracting the gaussian smoothed image from the original one 
            var mask = image - gaussianSmooth;
            //add a weighted value k to the obtained mask 
            mask *= k;
            //sum with the original image 
            image += mask;

            mask.Dispose();
            
            return image;
        }

        public static Bitmap SobelEdgeDetection(Bitmap inputImage)
        {
            Image<Gray, byte> gray = new Image<Gray, byte>(inputImage);


            Image<Gray, float> sobel = gray.Sobel(0, 1, 1).Add(gray.Sobel(1, 0, 1)).AbsDiff(new Gray(0.0));

            return sobel.Bitmap;
        }

        public static Bitmap CannyEdgeDetection(Bitmap inputImage)
        {
            Image<Gray, byte> im = new Image<Gray, byte>(inputImage);
            UMat u = im.ToUMat();
            CvInvoke.Canny(u, u, 500, 100);

            return u.Bitmap;
        }

        
        public static bool IsThisPixelRGB(Bitmap inputBmp, Point pt, int searchedArea)
        {

            //BitmapTransformations.Crop(inputBmp, new Rectangle(pt.X - 2, pt.Y - 2, 4, 4)).Save(@"C:\Users\piotr\Desktop\Nowy folder\rgbCheck.jpg") ;
            
                bool result = false;
            for (int x = pt.X - searchedArea/2; x < pt.X+ searchedArea+1; x++)
            {
                for (int y = pt.Y - searchedArea/2; y < pt.Y+ searchedArea+1; y++)
                {
                    Color color = inputBmp.GetPixel(x, y);
                    if (Math.Abs(color.R - color.G) > 3 || Math.Abs(color.R - color.B) > 3 || Math.Abs(color.G - color.B) > 3)
                    {
                        result = true;
                        break;
                    }
                }
            }
            using (Graphics g = Graphics.FromImage(inputBmp))
            {
                g.DrawRectangle(new Pen(Color.White), new Rectangle(pt.X - 2, pt.Y - 2, 4, 4));
            }
           // inputBmp.Save(@"C:\Users\piotr\Desktop\Nowy folder\rgbCheck.jpg");
            if (result)
            {
                ;
            }

            //single pixel
            //Color color = inputBmp.GetPixel(pt.X, pt.Y);
            //if (Math.Abs(color.R - color.G) < 3 & Math.Abs(color.R - color.B) < 3) 
            //{ result = false; }
            //single pixel


            return result;
        }
            
    }
}
