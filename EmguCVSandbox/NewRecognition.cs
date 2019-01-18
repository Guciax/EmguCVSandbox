using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmguCVSandbox.Form1;
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox
{
    public class NewRecognition
    {
        public static Point[] pointsOfInterest(Bitmap scannedCropImage, Bitmap emptyCropImage)
        {
            //Bitmap ss = ScreenShot.GetScreenShop("Lord of the Rings - LCG");
            //Bitmap fullEnemyCrop = BitmapTransformations.Crop(ScreenShot.GetScreenShop("Lord of the Rings - LCG"), new Rectangle(260, 350, 1170, 190));
            Image<Bgr, byte> ssImage = new Image<Bgr, byte>(scannedCropImage);

            //Bitmap noEnemyBmp = new Bitmap(@"Images\empty1.PNG");
            
            Image<Bgr, byte> emptyImage = new Image<Bgr, byte>(emptyCropImage);

            var sub = ssImage - emptyImage;
            var filtered = sub.Convert<Gray, byte>().ThresholdBinary(new Gray(70), new Gray(255)).Bitmap;

            int mobSocketWidth = 143;

            int mobStartA = 0;
            int mobStartB = 0;

            bool lineAdone=false;
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

            //for (int x = 1; x < filtered.Width; x++)
            //{
            //    var pixel = filtered.GetPixel(x, filtered.Height / 2);
            //    if (pixel.R > 5 & pixel.G > 5 & pixel.B > 5)
            //    {
            //        mobStartB = x;
            //        break;
            //    }
            //}

            int mobAreaWidth = filtered.Width - 2 * (Math.Min(mobStartA, mobStartB) - 5);
            int mobCount = (int)Math.Round((double)mobAreaWidth / mobSocketWidth, 0);

            int mobStart = (filtered.Width - mobCount * mobSocketWidth) / 2;

            List<Point> result = new List<Point>();
            for (int rX = 0; rX < mobCount; rX++)
            {
                result.Add(new Point(mobStart + rX * mobSocketWidth + mobSocketWidth / 2, scannedCropImage.Height / 2));
            }

            return result.ToArray();
        }

        public static List<MobInfo> ScanMobs(List<Bitmap> mobBitmaps, List<Bitmap> sharpNumbersImages, Bitmap emptyBattlefield, Bitmap currentScreenshot)
        {
            Bitmap noEnemyCrop = BitmapTransformations.Crop(emptyBattlefield, GlobalParameters.mobsRegion);
            List<MobInfo> mobsOnBattlefield = new List<MobInfo>();
            Bitmap mobCrop = BitmapTransformations.Crop(currentScreenshot, GlobalParameters.mobsRegion);
            Point[] pointsOfMobs = NewRecognition.pointsOfInterest(mobCrop, noEnemyCrop);
            Bitmap[] bitmapsOfPoints = BitmapTransformations.TakeBitmapsInPoints(mobCrop, pointsOfMobs, new Size(30, 30));
            //   foreach (var bmp in bitmapsOfPoints)
            //   {
            //       bmp.Save(@"Images\" + ((Point)bmp.Tag).X + "png");
            //   }
            Bitmap sharpenedBitmap = ImageFilters.SharpenGaussian(mobCrop, 11, 19, 84, 500, 500).Bitmap;
            foreach (var bmp in bitmapsOfPoints)
            {
                List<string> listOfMatches = new List<string>();
                Point pt = (Point)bmp.Tag;
                bool matchFound = false;
                foreach (var mobBmp in mobBitmaps) 
                {
                    double matchResult = ImageRecognition.SingleTemplateMatch(mobBmp, bmp);
                    listOfMatches.Add(mobBmp.Tag.ToString() + "@" + matchResult);
                    if (matchResult < 0.65) continue;

                    MobInfo newMob = new MobInfo();
                    newMob.mobImage = bmp;
                    newMob.location = pt;
                    newMob.name = mobBmp.Tag.ToString();
                    newMob.active = ImageFilters.IsThisPixelRGB(mobCrop, pt, 6);

                    Bitmap attCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 60, pt.Y + 58, 60, 35));
                    string ocrAtt = OCR.DecodeImg(attCrop, sharpNumbersImages);


                    Bitmap defCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X, pt.Y + 58, 60, 35));
                    string ocrHp = OCR.DecodeImg(defCrop, sharpNumbersImages);

                    newMob.attack = ocrAtt;
                    newMob.hp = ocrHp;
                    newMob.matchResults = listOfMatches;
                    mobsOnBattlefield.Add(newMob);
                    matchFound = true;
                    break;
                }
                if (!matchFound)
                {
                    MobInfo unknownMob = new MobInfo();
                    unknownMob.location = pt;
                    unknownMob.name = "unknown";
                    unknownMob.mobImage = bmp;
                    unknownMob.matchResults = listOfMatches;
                    mobsOnBattlefield.Add(unknownMob);
                }
            }
            return mobsOnBattlefield;
        }

        public static List<QuestInfo> ScanQuests(ref List<MobInfo> mobsOnBF, List<Bitmap> questsLibrary)
        {
            List<QuestInfo> result = new List<QuestInfo>();
            foreach (var mob in mobsOnBF)
            {
                List<string> listOfMatches = new List<string>();
                bool matchFound = false;
                if (mob.name != "unknown") continue;
                foreach (var questImg in questsLibrary)
                {
                    double matchResult = ImageRecognition.SingleTemplateMatch(questImg, mob.mobImage);
                    listOfMatches.Add(questImg.Tag.ToString() + "@" + matchResult);
                    if (matchResult < 0.8) continue;
                    matchFound = true;
                    QuestInfo newQ = new QuestInfo();
                    newQ.name = questImg.Tag.ToString();
                    newQ.location = mob.location;
                    newQ.questImage = questImg;
                    newQ.matchResults = listOfMatches;
                    result.Add(newQ);
                    mob.name = "quest";
                }
            }

            mobsOnBF = mobsOnBF.Where(m => m.name != "quest").ToList();

            return result;
        }

        public static List<HeroAllyInfo> ScanHeroes(List<Bitmap> heroAllyLibrary, List<Bitmap> sharpNumbersImages, Bitmap emptyBattlefield, Bitmap currentScreenshot)
        {
            List<HeroAllyInfo> heroAllyOnBattlefield = new List<HeroAllyInfo>();
            Bitmap emptyBattlefieldCrop = BitmapTransformations.Crop(emptyBattlefield, GlobalParameters.heroRegion);
            
            Bitmap heroCrop = BitmapTransformations.Crop(currentScreenshot, GlobalParameters.heroRegion);
            Point[] pointsOfHerroAlly = NewRecognition.pointsOfInterest(heroCrop, emptyBattlefieldCrop);
            Bitmap[] bitmapsOfPoints = BitmapTransformations.TakeBitmapsInPoints(heroCrop, pointsOfHerroAlly, new Size(30, 30));

            Bitmap sharpenedBitmap = ImageFilters.SharpenGaussian(heroCrop, 11, 19, 84, 500, 500).Bitmap;
            foreach (var bmp in bitmapsOfPoints)
            {
                List<string> listOfMatches = new List<string>();
                Point pt = (Point)bmp.Tag;
                bool matchFound = false;
                foreach (var heroAllyBmp in heroAllyLibrary) 
                {
                    double matchResult = ImageRecognition.SingleTemplateMatch(heroAllyBmp, bmp);
                    if (matchResult < 0.8) continue;

                    matchFound = true;
                    HeroAllyInfo newHeroAlly = new HeroAllyInfo();
                    newHeroAlly.location = pt;
                    newHeroAlly.name = heroAllyBmp.Tag.ToString();
                    newHeroAlly.active = ImageFilters.IsThisPixelRGB(heroCrop, pt, 6);


                    Bitmap attCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 60, pt.Y + 50, 60, 35));
                    string ocrAtt = OCR.DecodeImg(attCrop, sharpNumbersImages);


                    Bitmap defCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X + 15, pt.Y + 50, 60, 35));
                    string ocrHp = OCR.DecodeImg(defCrop, sharpNumbersImages);

                    newHeroAlly.attack = ocrAtt;
                    newHeroAlly.hp = ocrHp;
                    heroAllyOnBattlefield.Add(newHeroAlly);
                    break;
                }
                if (!matchFound)
                {
                    HeroAllyInfo ally = new HeroAllyInfo();
                    ally.location = pt;
                    ally.name = "Ally";
                    ally.heroImage = bmp;
                    ally.matchResults = listOfMatches;
                    ally.active = ImageFilters.IsThisPixelRGB(heroCrop, pt, 6);

                    Bitmap attCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 60, pt.Y + 50, 60, 35));
                    string ocrAtt = OCR.DecodeImg(attCrop, sharpNumbersImages);

                    Bitmap defCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X + 15, pt.Y + 50, 60, 35));
                    string ocrHp = OCR.DecodeImg(defCrop, sharpNumbersImages);
                    ally.attack = ocrAtt;
                    ally.hp = ocrHp;

                    heroAllyOnBattlefield.Add(ally);
                }

            }
            return heroAllyOnBattlefield;
        }

        public static int ScanCash(Bitmap currentScreenshot, List<Bitmap> moneyLibrary)
        {
            Bitmap moneyCrop = BitmapTransformations.Crop(currentScreenshot, GlobalParameters.cashRegion);
            int result = 0;
            foreach (var moneyImg in moneyLibrary)
            {
                double matchResult = ImageRecognition.SingleTemplateMatch(moneyImg, moneyCrop);
                if (matchResult < 0.8) continue;

                result = int.Parse(moneyImg.Tag.ToString());
            }
            return result;
        }
    }
}
