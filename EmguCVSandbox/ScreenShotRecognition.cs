using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmguCVSandbox.Form1;
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox
{
    class ScreenShotRecognition
    {
        public static List<MobInfo> ScanMobs(List<Bitmap> mobBitmaps, Bitmap mobsScreenshotCrop, List<Bitmap> sharpNumbersImages)
        {
            List<MobInfo> mobsOnBattlefield = new List<MobInfo>();
            Bitmap sharpenedBitmap = ImageFilters.SharpenGaussian(mobsScreenshotCrop, 11, 19, 84, 500, 500).Bitmap;

            foreach (var mob in mobBitmaps)
            {
                Point[] mobLocations = ImageRecognition.multipleTemplateMatch(mobsScreenshotCrop, mob, Color.Red, 0.75);
                if (mobLocations.Count() > 0)
                {
                    foreach (var pt in mobLocations)
                    {
                        MobInfo newMob = new MobInfo();
                        newMob.location = pt;
                        newMob.name = ((string)mob.Tag).Split('.')[0];

                        newMob.active = ImageFilters.IsThisPixelRGB(mobsScreenshotCrop, new Point(pt.X, pt.Y), 6);

                        string ocrAtt = OCR.DecodeImg(BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 60, pt.Y + 35, 60, 35)), sharpNumbersImages);
                        string ocrHp = OCR.DecodeImg(BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X, pt.Y + 35, 60, 35)), sharpNumbersImages);

                        newMob.attack = ocrAtt;
                        newMob.hp = ocrHp;
                        mobsOnBattlefield.Add(newMob);
                    }
                }
            }
            return mobsOnBattlefield;
        }

        public static List<HeroAllyInfo> ScanHeroes(List<Bitmap> heroAllyBitmaps, Bitmap heroCrop, List<Bitmap> numbersHeroImages)
        {
            Debug.WriteLine("<--------------> ");
            List<HeroAllyInfo> result = new List<HeroAllyInfo>();
            Bitmap sharpenedBitmap = ImageFilters.SharpenGaussian(heroCrop, 11, 19, 84, 500, 500).Bitmap;
            heroCrop.Save(@"C:\Users\Gucci\Desktop\Nowy folder\hero.jpg");
            foreach (var hero in heroAllyBitmaps)
            {
                Debug.WriteLine("Checking " + hero.Tag.ToString());
                Point[] questLocations = ImageRecognition.multipleTemplateMatch(heroCrop, hero, Color.Red, 0.8);
                if (questLocations.Count() > 0)
                {
                    foreach (var pt in questLocations)
                    {
                        HeroAllyInfo newHero = new HeroAllyInfo();
                        newHero.location = pt;
                        newHero.name = ((string)hero.Tag).Split('.')[0];

                        string ocrAtt = OCR.DecodeImg(BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 60, pt.Y + 35, 61, 35)), numbersHeroImages);
                        string ocrHp = OCR.DecodeImg(BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X, pt.Y + 35, 60, 35)), numbersHeroImages);

                        newHero.attack = ocrAtt;
                        newHero.hp = ocrHp;
                        result.Add(newHero);
                    }
                }
            }
            return result;
        }

        public static List<QuestInfo> ScanQuests(List<Bitmap> questsBitmaps, Bitmap mobsScreenshotCrop, List<Bitmap> numberQuestImages)
        {
            List<QuestInfo> result = new List<QuestInfo>();
            foreach (var quest in questsBitmaps)
            {
                Point[] questLocations = ImageRecognition.multipleTemplateMatch(mobsScreenshotCrop, quest, Color.Red, 0.7);
                if (questLocations.Count() > 0)
                {
                    foreach (var pt in questLocations)
                    {
                        QuestInfo newQuest = new QuestInfo();
                        newQuest.location = pt;
                        newQuest.name = ((string)quest.Tag).Split('.')[0];

                        string ocr = OCR.DecodeImg(BitmapTransformations.Crop(mobsScreenshotCrop, new Rectangle(pt.X - 20, pt.Y + 35, 50, 35)), numberQuestImages);

                        newQuest.value = ocr;
                        result.Add(newQuest);
                    }
                }
            }
            return result;
        }

        public static List<CardInfo> ScanCards(List<Bitmap> cardValueImages, Bitmap cardsScreenShot)
        {
            List<CardInfo> result = new List<CardInfo>();

            foreach (var cardValue in cardValueImages)
            {
                Point[] cardLocations = ImageRecognition.multipleTemplateMatch(cardsScreenShot, cardValue, Color.Red, 0.6);
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
    }
}
