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
                Debug.WriteLine("Scanning for mob: " + mob.Tag.ToString());
                Point[] mobLocations = ImageRecognition.multipleTemplateMatch(mobsScreenshotCrop, mob, Color.Red, 0.75);
                if (mobLocations.Count() > 0)
                {
                    foreach (var pt in mobLocations)
                    {
                        MobInfo newMob = new MobInfo();
                        newMob.location = pt;
                        newMob.name = ((string)mob.Tag).Split('.')[0];

                        newMob.active = ImageFilters.IsThisPixelRGB(mobsScreenshotCrop, new Point(pt.X, pt.Y), 6);
                        Debug.WriteLine($"Making mobs attackNum crop inputBmp: {sharpenedBitmap.Width}x{sharpenedBitmap.Height} rect:{pt.X - 60}x{pt.Y + 35}x60x35");
                        Bitmap attCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 60, pt.Y + 35, 60, 35));
                        string ocrAtt = OCR.DecodeImg(attCrop, sharpNumbersImages);
                        Debug.WriteLine($"Making mobs defNum crop inputBmp: {sharpenedBitmap.Width}x{sharpenedBitmap.Height} rect:{pt.X}x{pt.Y}x60x35");
                        Bitmap defCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X, pt.Y + 35, 60, 35));
                        string ocrHp = OCR.DecodeImg(defCrop, sharpNumbersImages);

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
            //Debug.WriteLine("<--------------> ");
            List<HeroAllyInfo> result = new List<HeroAllyInfo>();
            Bitmap sharpenedBitmap = ImageFilters.SharpenGaussian(heroCrop, 11, 19, 84, 500, 500).Bitmap;
            //heroCrop.Save(@"C:\Users\Gucci\Desktop\Nowy folder\hero.jpg");
            foreach (var hero in heroAllyBitmaps)
            {
                Debug.WriteLine("Scanning for hero: " + hero.Tag.ToString());
                Point[] heroLocations = ImageRecognition.multipleTemplateMatch(heroCrop, hero, Color.Red, 0.75);
                if (heroLocations.Count() > 0)
                {
                    foreach (var pt in heroLocations)
                    {
                        HeroAllyInfo newHero = new HeroAllyInfo();
                        newHero.location = pt;
                        newHero.name = ((string)hero.Tag).Split('.')[0];
                        newHero.active = ImageFilters.IsThisPixelRGB(heroCrop, new Point(pt.X, pt.Y), 6);

                        Debug.WriteLine($"Making mobs attackNum crop inputBmp: {sharpenedBitmap.Width}x{sharpenedBitmap.Height} rect:{pt.X - 60}x{pt.Y + 35}x61x35");
                        Bitmap attCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 60, pt.Y + 35, 61, 35));
                        string ocrAtt = OCR.DecodeImg(attCrop, numbersHeroImages);
                       // int ocrAtt = int.Parse(OCR.DecodeImg(attCrop, numbersHeroImages));
                        Debug.WriteLine($"Making mobs defNum crop inputBmp: {sharpenedBitmap.Width}x{sharpenedBitmap.Height} rect:{pt.X}x{pt.Y}x60x35");
                        Bitmap defCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X, pt.Y + 35, 60, 35));
                        string ocrHp = OCR.DecodeImg(defCrop, numbersHeroImages);

                        Bitmap loreCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 30, pt.Y + 47, 60, 35));
                        string ocrLore = OCR.DecodeImg(loreCrop, numbersHeroImages);

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
                Debug.WriteLine("Scanning for quest: " + quest.Tag.ToString());
                Point[] questLocations = ImageRecognition.multipleTemplateMatch(mobsScreenshotCrop, quest, Color.Red, 0.7);
                if (questLocations.Count() > 0)
                {
                    foreach (var pt in questLocations)
                    {
                        QuestInfo newQuest = new QuestInfo();
                        newQuest.location = pt;
                        newQuest.name = ((string)quest.Tag).Split('.')[0];

                        Debug.WriteLine("Making quest Number crop");
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
                Debug.WriteLine("Scanning for card value: " + cardValue.Tag.ToString());
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
