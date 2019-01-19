using Emgu.CV;
using Emgu.CV.CvEnum;
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
    class ImageRecognition
    {

        public static double SingleTemplateMatch(Bitmap background, Bitmap searchedImage, TemplateMatchingType type)
        {
            bool boolRresult = false;
            Image<Bgr, byte> bckgImg = new Image<Bgr, byte>(background); // Image B
            Image<Bgr, byte> searchedImg = new Image<Bgr, byte>(searchedImage); // Image A
            Image<Bgr, byte> imageToShow = bckgImg.Copy();

            using (Image<Gray, float> result = bckgImg.MatchTemplate(searchedImg, type))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                return maxValues[0];
               
            }

        }

        public static Point[] GetPointsOfTemplateImage(Bitmap SourceImages, Bitmap searchedImage, Color rectangleColor,double minScore)
        {
            Image<Bgr, byte> image_source = new Image<Bgr, byte>(SourceImages);
            Image<Bgr, byte> image_partial = new Image<Bgr, byte>(searchedImage);

            double threshold = minScore;
            ImageFinder imageFinder = new ImageFinder(image_source, image_partial, threshold);

            imageFinder.FindThenShow(rectangleColor);
            List<Point> result = new List<Point>();
            foreach (var rect in imageFinder.Rectangles)
            {
                int x = rect.Location.X + rect.Width / 2;
                int y = rect.Location.Y + rect.Height / 2;
                if (x < 0 || x > image_source.Width) continue;
                if (y < 0 || y > image_source.Height) continue;

                result.Add(new Point(x, y));
            }

            image_source.Dispose();
            image_partial.Dispose();
            
            return result.ToArray();
        }

        class ImageFinder
        {
            private List<Rectangle> rectangles;
            private List<string> scores;
            public Image<Bgr, byte> BaseImage { get; set; }
            public Image<Bgr, byte> SubImage { get; set; }
            public Image<Bgr, byte> ResultImage { get; set; }
            public double Threashold { get; set; }

            public List<Rectangle> Rectangles
            {
                get { return rectangles; }
            }

            public List<string> Scores
            {
                get { return scores; }
            }

            public ImageFinder(Image<Bgr, byte> baseImage, Image<Bgr, byte> subImage, double threashold)
            {
                rectangles = new List<Rectangle>();
                scores = new List<string>();
                BaseImage = baseImage;
                SubImage = subImage;
                Threashold = threashold;
            }

            public void FindThenShow(Color rectangleColor)
            {
                FindImage();
                //DrawOnResultRectanglesOnImage(rectangleColor);
                //ShowImage();
            }

            public void DrawOnResultRectanglesOnImage(Color rectangleColor)
            {
                ResultImage = BaseImage.Copy();
                for(int i=0;i<this.rectangles.Count;i++)
                {
                    ResultImage.Draw(rectangles[i], new Bgr(rectangleColor), 2);
                    ResultImage.Draw(scores[i],new Point( rectangles[i].Location.X, rectangles[i].Location.Y - 2), FontFace.HersheyComplex, 1, new Bgr(rectangleColor));
                }
            }

            public void FindImage()
            {
                rectangles = new List<Rectangle>();

                using (Image<Bgr, byte> imgSrc = BaseImage.Copy())
                {
                    while (true)
                    {
                        using (Image<Gray, float> result = imgSrc.MatchTemplate(SubImage, TemplateMatchingType.CcoeffNormed))
                        {
                            double[] minValues, maxValues;
                            Point[] minLocations, maxLocations;
                            result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                            if (maxValues[0] > Threashold)
                            {
                                Rectangle match = new Rectangle(maxLocations[0], SubImage.Size);
                                imgSrc.Draw(match, new Bgr(Color.Blue), -1);
                                rectangles.Add(match);
                                scores.Add(Math.Round(maxValues[0],2).ToString());
                               // imgSrc.Bitmap.Save(@"C:\Users\Gucci\Desktop\Nowy folder\" + "D" + ".jpg");
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            public void ShowImage()
            {
                Random rNo = new Random();
                string outFilename = "matched Templates" + rNo.Next();
                CvInvoke.Imshow(outFilename, ResultImage);
            }

        }
    }
}
