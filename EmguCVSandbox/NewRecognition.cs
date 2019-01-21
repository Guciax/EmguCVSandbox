using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EmguCVSandbox.Form1;
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox
{
    public class NewRecognition
    {
        private readonly Form1 form;
        private readonly RichTextBox logbook;
        private readonly List<Bitmap> mobBitmaps;
        private readonly List<Bitmap> ocrHeroNumber;
        private readonly List<Bitmap> ocrMobNumbers;
        private readonly List<Bitmap> questsImages;
        private readonly List<Bitmap> sharpNumbersImages;
        private readonly List<Bitmap> cardValueImages;
        private readonly List<Bitmap> heroAlltImages;
        private readonly List<Bitmap> myAlltImages;
        private readonly List<Bitmap> moneyImages;
        private readonly List<Bitmap> okImages;
        private readonly List<Bitmap> defendImages;
        private readonly List<Bitmap> attachmentImages;

        public NewRecognition( Form1 form, RichTextBox logbook,
            List<Bitmap> mobBitmaps,
        List<Bitmap> ocrHeroNumber,
        List<Bitmap> ocrMobNumbers,
        List<Bitmap> questsImages,
        List<Bitmap> sharpNumbersImages,
        List<Bitmap> cardValueImages,
        List<Bitmap> heroAlltImages,
        List<Bitmap> MyAlltImages,
        List<Bitmap> moneyImages,
        List<Bitmap> okImages,
        List<Bitmap> defendImages,
        List<Bitmap> attachmentImages)  
        {
            this.form = form;
            this.logbook = logbook;
            this.mobBitmaps = mobBitmaps;
            this.ocrHeroNumber = ocrHeroNumber;
            this.ocrMobNumbers = ocrMobNumbers;
            this.questsImages = questsImages;
            this.sharpNumbersImages = sharpNumbersImages;
            this.cardValueImages = cardValueImages;
            this.heroAlltImages = heroAlltImages;
            this.myAlltImages = MyAlltImages;
            this.moneyImages = moneyImages;
            this.okImages = okImages;
            this.defendImages = defendImages;
            this.attachmentImages = attachmentImages;
        }

        public GameCurrentState ScanGame()
        {
            Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());

            GameCurrentState result = new GameCurrentState();
            result.currentPhase = 1; // I need some method
            result.cardsInHand = ScanCards(cardValueImages, BitmapTransformations.Crop(ss, GlobalParameters.cardsRegion));
            result.mobs = ScanMobs(mobBitmaps, ocrMobNumbers, GlobalParameters.emptyFieldDict[result.currentPhase], ss);
            result.heroesAlly = ScanHeroes(heroAlltImages, ocrHeroNumber, GlobalParameters.emptyFieldDict[result.currentPhase], ss);
            result.quests = ScanQuests(ref result.mobs, questsImages, ocrMobNumbers, ss);
            result.cash = ScanCash(ss, moneyImages);
            
            
            return result;
        }

        
        public static Point[] pointsOfInterest(Bitmap scannedCropImage, Bitmap emptyCropImage, int socketWidth)
        {
            //Bitmap ss = ScreenShot.GetScreenShop("Lord of the Rings - LCG");
            //Bitmap fullEnemyCrop = BitmapTransformations.Crop(ScreenShot.GetScreenShop("Lord of the Rings - LCG"), new Rectangle(260, 350, 1170, 190));
            Image<Bgr, byte> ssImage = new Image<Bgr, byte>(scannedCropImage);

            //Bitmap noEnemyBmp = new Bitmap(@"Images\empty1.PNG");
            
            Image<Bgr, byte> emptyImage = new Image<Bgr, byte>(emptyCropImage);

            var sub = ssImage - emptyImage;
            var filtered = sub.Convert<Gray, byte>().ThresholdBinary(new Gray(70), new Gray(255)).Bitmap;

            int mobStartA = 0;
            int mobStartB = 0;

            bool lineAdone=false;
            bool lineBdone = false;

            for (int x = 20; x < filtered.Width; x++)
            {
                var pixelA = filtered.GetPixel(x, scannedCropImage.Height/2);
                var pixelB = filtered.GetPixel(x, (int)Math.Round(scannedCropImage.Height * 0.8, 0));

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
            int mobCount = (int)Math.Round((double)mobAreaWidth / socketWidth, 0);

            int mobStart = (filtered.Width - mobCount * socketWidth) / 2;

            List<Point> result = new List<Point>();
            for (int rX = 0; rX < mobCount; rX++)
            {
                result.Add(new Point(mobStart + rX * socketWidth + socketWidth / 2, scannedCropImage.Height / 2));
            }

            return result.ToArray();
        }

        public static List<MobInfo> ScanMobs(List<Bitmap> mobBitmaps, List<Bitmap> sharpNumbersImages, Bitmap emptyBattlefield, Bitmap currentScreenshot)
        {
            Bitmap noEnemyCrop = BitmapTransformations.Crop(emptyBattlefield, GlobalParameters.mobsRegion);
            List<MobInfo> mobsOnBattlefield = new List<MobInfo>();
            Bitmap mobCrop = BitmapTransformations.Crop(currentScreenshot, GlobalParameters.mobsRegion);
            Point[] pointsOfMobs = NewRecognition.pointsOfInterest(mobCrop, noEnemyCrop, GlobalParameters.mobSocketWidth);
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
                
                Bitmap attCrop = BitmapTransformations.Crop(mobCrop, new Rectangle(pt.X - 40, pt.Y + 69, 40, 30));
                Bitmap defCrop = BitmapTransformations.Crop(mobCrop, new Rectangle(pt.X + 40, pt.Y + 69, 40, 30));

                Rectangle attRegionGlobal = new Rectangle(pt.X - 40 + GlobalParameters.mobsRegion.X, pt.Y + 60 + GlobalParameters.mobsRegion.Y, 40, 30);
                Rectangle hpRegionGlobal = new Rectangle(pt.X + 40 + GlobalParameters.mobsRegion.X, pt.Y + 60 + GlobalParameters.mobsRegion.Y, 40, 30);
                Debug.WriteLine($"<-------- mobX={pt.X} scanAtt");
                int ocrAtt = OCR.DecodeImg(currentScreenshot, attRegionGlobal, sharpNumbersImages, 113);
                Debug.WriteLine($"<-------- mobX={pt.X} scanDef");
                int ocrHp = OCR.DecodeImg(currentScreenshot, hpRegionGlobal, sharpNumbersImages, 66);
                MobInfo newMob = new MobInfo();

                foreach (var mobBmp in mobBitmaps) 
                {
                    double matchResult = ImageRecognition.SingleTemplateMatch(mobBmp, bmp, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
                    listOfMatches.Add(mobBmp.Tag.ToString() + "@" + matchResult);
                    if (matchResult < 0.6) continue;

                    newMob.name = mobBmp.Tag.ToString();
                    matchFound = true;
                    break;
                }
                if (!matchFound)
                {
                    newMob.name = "unknown";
                }

                newMob.active = ImageFilters.IsThisPixelRGB(mobCrop, pt, 6);
                newMob.attack = ocrAtt;
                newMob.hp = ocrHp;
                newMob.matchResults = listOfMatches;
                newMob.mobImage = bmp;
                newMob.location = pt;

                mobsOnBattlefield.Add(newMob);

                //attCrop.Save(@"Images\\ocrResult\mobAt" + newMob.attack + ".png");
                //defCrop.Save(@"Images\\ocrResult\mobHp" + newMob.hp + ".png");
            }
            return mobsOnBattlefield;
        }

        public static List<QuestInfo> ScanQuests(ref List<MobInfo> mobsOnBF, List<Bitmap> questsLibrary, List<Bitmap> numbersLibrary, Bitmap currentScreenshot)
        {
            Bitmap mobcrop = BitmapTransformations.Crop(currentScreenshot, GlobalParameters.mobsRegion);
            List<QuestInfo> result = new List<QuestInfo>();
            foreach (var mob in mobsOnBF)
            {
                List<string> listOfMatches = new List<string>();
                bool matchFound = false;
                if (mob.name != "unknown") continue;
                foreach (var questImg in questsLibrary)
                {
                    double matchResult = ImageRecognition.SingleTemplateMatch(questImg, mob.mobImage, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
                    listOfMatches.Add(questImg.Tag.ToString() + "@" + matchResult);
                    if (matchResult < 0.8) continue;
                    matchFound = true;
                    QuestInfo newQ = new QuestInfo();
                    newQ.name = questImg.Tag.ToString();
                    newQ.location = mob.location;
                    newQ.questImage = questImg;
                    newQ.matchResults = listOfMatches;
                    
                    mob.name = "quest";

                    Rectangle valueRRegionGlobal = new Rectangle(mob.location.X - 5 + GlobalParameters.mobsRegion.X, mob.location.Y + 65 + GlobalParameters.mobsRegion.Y, 40, 30);
                    Debug.WriteLine($"<-------- Quest={mob.location.X} scan");
                    int ocr = OCR.DecodeImg(currentScreenshot, valueRRegionGlobal, numbersLibrary, 113);
                    newQ.value = ocr;
                    result.Add(newQ);
                    //valueCrop.Save(@"Images\\ocrResult\questV" + newQ.value + ".png");
                    break;
                }
            }

            mobsOnBF = mobsOnBF.Where(m => m.name != "quest").ToList();

            return result;
        }

        public static List<HeroAllyInfo> ScanHeroes(List<Bitmap> heroAllyLibrary, List<Bitmap> sharpNumbersImages, Bitmap emptyBattlefield, Bitmap currentScreenshot)
        {
            bool removehero = false;
            List<HeroAllyInfo> heroAllyOnBattlefield = new List<HeroAllyInfo>();
            Bitmap emptyBattlefieldCrop = BitmapTransformations.Crop(emptyBattlefield, GlobalParameters.heroRegion);
            
            Bitmap heroCrop = BitmapTransformations.Crop(currentScreenshot, GlobalParameters.heroRegion);
            Point[] pointsOfHerroAlly = NewRecognition.pointsOfInterest(heroCrop, emptyBattlefieldCrop, GlobalParameters.heroSocketWidth);
            Bitmap[] bitmapsOfPoints = BitmapTransformations.TakeBitmapsInPoints(heroCrop, pointsOfHerroAlly, new Size(30, 30));

            //Bitmap sharpenedBitmap = ImageFilters.SharpenGaussian(heroCrop, 11, 19, 84, 500, 500).Bitmap;
            foreach (var bmp in bitmapsOfPoints)
            {
                
                List<string> listOfMatches = new List<string>();
                Point pt = (Point)bmp.Tag;
                bool matchFound = false;

                HeroAllyInfo newHeroAlly = new HeroAllyInfo();

                foreach (var heroAllyBmp in heroAllyLibrary) 
                {
                    double matchResult = ImageRecognition.SingleTemplateMatch(heroAllyBmp, bmp, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
                    listOfMatches.Add(heroAllyBmp.Tag.ToString() + "@" + matchResult);
                    if (matchResult < 0.8) continue;

                    matchFound = true;

                    
                    newHeroAlly.name = heroAllyBmp.Tag.ToString();
                    break;
                }
                if (!matchFound)
                {
                    newHeroAlly.name = "Ally";
                    removehero = true;

                }


                Rectangle attRegionGlobal = new Rectangle(pt.X - 50 + GlobalParameters.heroRegion.X, pt.Y + 40 + GlobalParameters.heroRegion.Y, 40, 35);
                Rectangle hpRegionGlobal = new Rectangle(pt.X + 37 + GlobalParameters.heroRegion.X, pt.Y + 40 + GlobalParameters.heroRegion.Y, 40, 35);
                Rectangle loreRegionGlobal = new Rectangle(pt.X - 5 + GlobalParameters.heroRegion.X, pt.Y + 57 + GlobalParameters.heroRegion.Y, 40, 30);

                newHeroAlly.active = ImageFilters.IsThisPixelRGB(heroCrop, pt, 6);
                newHeroAlly.matchResults = listOfMatches;
                newHeroAlly.location = pt;
                Debug.WriteLine($"<-------- heroX={pt.X} scanLore");
                int ocrLore = OCR.DecodeImg(currentScreenshot, loreRegionGlobal, sharpNumbersImages, 113);
                newHeroAlly.lore = ocrLore;
                Debug.WriteLine($"<----------------scanAtt");
                int ocrAtt = OCR.DecodeImg(currentScreenshot, attRegionGlobal, sharpNumbersImages, 113);
                newHeroAlly.attack = ocrAtt;

                Debug.WriteLine($"<---------------- scanDef");
                int ocrHp = OCR.DecodeImg(currentScreenshot, hpRegionGlobal, sharpNumbersImages, 66);
                newHeroAlly.hp = ocrHp;

                if (removehero == true)
                {
                    removehero = false;
                }
                else
                {
                    heroAllyOnBattlefield.Add(newHeroAlly);
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
                double matchResult = ImageRecognition.SingleTemplateMatch(moneyImg, moneyCrop, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
                if (matchResult < 0.8) continue;

                result = int.Parse(moneyImg.Tag.ToString());
            }
            return result;
        }
        public static List<MyAllyInfo> ScanAllies(List<Bitmap> AllyLibrary, List<Bitmap> sharpNumbersImages, Bitmap emptyBattlefield, Bitmap currentScreenshot)
        {
            bool removeallies = false;
            List<MyAllyInfo> AllyOnBattlefield = new List<MyAllyInfo>();
            Bitmap emptyBattlefieldCrop = BitmapTransformations.Crop(emptyBattlefield, GlobalParameters.heroRegion);

            Bitmap heroCrop = BitmapTransformations.Crop(currentScreenshot, GlobalParameters.heroRegion);
            Point[] pointsOfAlly = NewRecognition.pointsOfInterest(heroCrop, emptyBattlefieldCrop, GlobalParameters.heroSocketWidth);
            Bitmap[] bitmapsOfPoints = BitmapTransformations.TakeBitmapsInPoints(heroCrop, pointsOfAlly, new Size(30, 30));

            //Bitmap sharpenedBitmap = ImageFilters.SharpenGaussian(heroCrop, 11, 19, 84, 500, 500).Bitmap;
            foreach (var bmp in bitmapsOfPoints)
            {
                List<string> listOfMatches = new List<string>();
                Point pt = (Point)bmp.Tag;
                bool matchFound = false;

                MyAllyInfo newAlly = new MyAllyInfo();

                foreach (var MyAllyBmp in AllyLibrary)
                {
                    double matchResult = ImageRecognition.SingleTemplateMatch(MyAllyBmp, bmp, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed);
                    listOfMatches.Add(MyAllyBmp.Tag.ToString() + "@" + matchResult);
                    if (matchResult < 0.8) continue;

                    matchFound = true;


                    newAlly.name = MyAllyBmp.Tag.ToString();
                    break;
                }
                if (!matchFound)
                {
                    removeallies = true;
                    newAlly.name = "Ally";

                }


                Rectangle attRegionGlobal = new Rectangle(pt.X - 50 + GlobalParameters.heroRegion.X, pt.Y + 45 + GlobalParameters.heroRegion.Y, 40, 35);
                Rectangle hpRegionGlobal = new Rectangle(pt.X + 37 + GlobalParameters.heroRegion.X, pt.Y + 45 + GlobalParameters.heroRegion.Y, 40, 35);
                Rectangle loreRegionGlobal = new Rectangle(pt.X - 5 + GlobalParameters.heroRegion.X, pt.Y + 65 + GlobalParameters.heroRegion.Y, 40, 30);

                newAlly.active = ImageFilters.IsThisPixelRGB(heroCrop, pt, 6);
                newAlly.matchResults = listOfMatches;
                newAlly.location = pt;

                int ocrLore = OCR.DecodeImg(currentScreenshot, loreRegionGlobal, sharpNumbersImages, 113);
                newAlly.lore = ocrLore;

                int ocrAtt = OCR.DecodeImg(currentScreenshot, attRegionGlobal, sharpNumbersImages, 113);
                newAlly.attack = ocrAtt;


                int ocrHp = OCR.DecodeImg(currentScreenshot, hpRegionGlobal, sharpNumbersImages, 66);
                newAlly.hp = ocrHp;

                if (removeallies == true)
                {
                    removeallies = false;
                }
                else
                {
                    AllyOnBattlefield.Add(newAlly);
                }
            }
            return AllyOnBattlefield;


        }
        public static List<CardInfo> ScanCards(List<Bitmap> cardValueImages, Bitmap cardsScreenShot)
        {
            List<CardInfo> result = new List<CardInfo>();
            Bitmap cardsCrop = BitmapTransformations.Crop(cardsScreenShot, GlobalParameters.cardsRegion);

            foreach (var cardValue in cardValueImages)
            {
                //Debug.WriteLine("Scanning for card value: " + cardValue.Tag.ToString());
                Point[] cardLocations = ImageRecognition.GetPointsOfTemplateImage(cardsCrop, cardValue, Color.Red, 0.7);
                if (cardLocations.Count() > 0)
                {
                    foreach (var pt in cardLocations)
                    {
                        CardInfo newCard = new CardInfo();
                        newCard.location = pt;
                        newCard.value = ((string)cardValue.Tag).Split('.')[0];
                        result.Add(newCard);
                    }
                }
            }
            return result;
        }

        public static List<OkInfo> ScanOk(List<Bitmap> cardValueImages, Bitmap cardsScreenShot)
        {
            List<OkInfo> result = new List<OkInfo>();

            foreach (var cardValue in cardValueImages)
            {
                //Debug.WriteLine("Scanning for card value: " + cardValue.Tag.ToString());
                Point[] cardLocations = ImageRecognition.GetPointsOfTemplateImage(cardsScreenShot, cardValue, Color.Red, 0.9);
                if (cardLocations.Count() > 0)
                {
                    foreach (var pt in cardLocations)
                    {
                        OkInfo newCard = new OkInfo();
                        newCard.location = pt;
                        newCard.value = ((string)cardValue.Tag).Split('.')[0];
                        result.Add(newCard);
                    }
                }
            }
            return result;
        }
            public static List<AttachmentInfo> ScanAttachment(List<Bitmap> attachmentImages, Bitmap attachmentScreenShot)
            {
                List<AttachmentInfo> result = new List<AttachmentInfo>();
            Bitmap attachmentCrop = BitmapTransformations.Crop(attachmentScreenShot, GlobalParameters.attachmentRegion);

            foreach (var cardValue in attachmentImages)
                {
                    //Debug.WriteLine("Scanning for card value: " + cardValue.Tag.ToString());
                    Point[] cardLocations = ImageRecognition.GetPointsOfTemplateImage(attachmentScreenShot, cardValue, Color.Red, 0.9);
                    if (cardLocations.Count() > 0)
                    {
                        foreach (var pt in cardLocations)
                        {
                        AttachmentInfo newCard = new AttachmentInfo();
                            newCard.location = pt;
                            newCard.name = ((string)cardValue.Tag).Split('.')[0];
                            result.Add(newCard);
                        }
                    }
                }
                return result;
            }
        public static List<DefendInfo> ScanDefend(List<Bitmap> defendImages, Bitmap cardsScreenShot)
        {
            List<DefendInfo> result = new List<DefendInfo>();

            foreach (var cardValue in defendImages)
            {
                //Debug.WriteLine("Scanning for card value: " + cardValue.Tag.ToString());
                Point[] cardLocations = ImageRecognition.GetPointsOfTemplateImage(cardsScreenShot, cardValue, Color.Red, 0.9);
                if (cardLocations.Count() > 0)
                {
                    foreach (var pt in cardLocations)
                    {
                        DefendInfo newCard = new DefendInfo();
                        newCard.location = pt;
                        newCard.value = ((string)cardValue.Tag).Split('.')[0];
                        result.Add(newCard);
                    }
                }
            }
            return result;
        }
    }
}
