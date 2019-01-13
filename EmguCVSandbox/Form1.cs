using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmguCVSandbox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        string sceneImage = @"C:\Users\piotr\Desktop\Nowy folder\bavkFullMobs.png";

        string questImage = @"C:\Users\piotr\Desktop\Nowy folder\quest.png";
        string mobImage = @"C:\Users\piotr\Desktop\Nowy folder\mob.png";
        string heroImage = @"C:\Users\piotr\Desktop\Nowy folder\hero.png";
        string sobelMobImage = @"C:\Users\piotr\Desktop\Nowy folder\sobelMob.jpg";
        
        DirectoryInfo mobsDir = new DirectoryInfo(@"C:\Users\piotr\Desktop\Nowy folder\Mobs");
        DirectoryInfo questssDir = new DirectoryInfo(@"C:\Users\piotr\Desktop\Nowy folder\Quests");
        DirectoryInfo numbersAttDir = new DirectoryInfo(@"C:\Users\piotr\Desktop\Nowy folder\Numbers\Att");
        DirectoryInfo numbersHpDir = new DirectoryInfo(@"C:\Users\piotr\Desktop\Nowy folder\Numbers\HP");
        DirectoryInfo numbersQuestDir = new DirectoryInfo(@"C:\Users\piotr\Desktop\Nowy folder\Numbers\Quests");
        DirectoryInfo numbersSharp = new DirectoryInfo(@"C:\Users\piotr\Desktop\Nowy folder\Numbers\Sharp");
        DirectoryInfo numbersCardsDir = new DirectoryInfo(@"C:\Users\piotr\Desktop\Nowy folder\Numbers\Cards");

        List<Bitmap> mobBitmaps = new List<Bitmap>();
        List<Bitmap> numberAttImages = new List<Bitmap>();
        List<Bitmap> numberHpImages = new List<Bitmap>();
        List<Bitmap> numberQuestImages = new List<Bitmap>();
        List<Bitmap> questsImages = new List<Bitmap>();
        List<Bitmap> sharpNumbersImages = new List<Bitmap>();
        List<Bitmap> cardValueImages = new List<Bitmap>();

        private void Form1_Load(object sender, EventArgs e)
        {
            FileInfo[] cardNumberFiles = numbersCardsDir.GetFiles();
            foreach (var cardV in cardNumberFiles)
            {
                Bitmap newBmp = new Bitmap(cardV.FullName);
                newBmp.Tag = cardV.Name.Split('.')[0];
                cardValueImages.Add(newBmp);
            }

            FileInfo[] sharpNumbersFiles = numbersSharp.GetFiles();
            foreach (var sNumFile in sharpNumbersFiles)
            {
                Bitmap newBmp = new Bitmap(sNumFile.FullName);
                newBmp.Tag = sNumFile.Name.Split('.')[0];
                sharpNumbersImages.Add(newBmp);
            }

            FileInfo[] questNumImgFiles = numbersQuestDir.GetFiles();
            foreach (var qNumFile in questNumImgFiles)
            {
                Bitmap newBmp = new Bitmap(qNumFile.FullName);
                newBmp.Tag = qNumFile.Name.Split('.')[0];
                numberQuestImages.Add(newBmp);
            }

            FileInfo[] mobImgFiles = mobsDir.GetFiles();
            foreach (var mobFile in mobImgFiles)
            {
                Bitmap newBmp = new Bitmap(mobFile.FullName);
                newBmp.Tag = mobFile.Name.Split('.')[0];
                mobBitmaps.Add(newBmp);
            }

            FileInfo[] questsFiles = questssDir.GetFiles();
            foreach (var numFile in questsFiles)
            {
                Bitmap newBmp = new Bitmap(numFile.FullName);
                newBmp.Tag = numFile.Name.Split('.')[0];
                questsImages.Add(newBmp);
            }
        }

        internal class MobInfo
        {
            public Point location;
            public string name;
            public bool active;
            public string attack;
            public string hp;
        }

        internal class QuestInfo
        {
            public Point location;
            public string name;
            public string value;
        }
        internal class CardInfo
        {
            public Point location;
            public string value;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap screenshot = ScreenShot.GetScreenShop("Lord of the Rings - LCG");
            //Bitmap screenshot = new Bitmap(sceneImage);

            Bitmap mobsCropImage = BitmapTransformations.Crop(screenshot, new Rectangle(260, 350, 1170, 210));

            List<MobInfo> mobsOnBattlefield = new List<MobInfo>();
            List<QuestInfo> questOnBattlefield = new List<QuestInfo>();
            Bitmap sharpenedBitmap = ImageFilters.SharpenGaussian(mobsCropImage, 11, 19, 84, 500, 500).Bitmap;

            foreach (var mob in mobBitmaps)
            {
                Point[] mobLocations= ImageRecognition.multipleTemplateMatch(mobsCropImage, mob, Color.Red, 0.7);
                if (mobLocations.Count() > 0) 
                {
                    foreach (var pt in mobLocations)
                    {
                        MobInfo newMob = new MobInfo();
                        newMob.location = pt;
                        newMob.name = ((string)mob.Tag).Split('.')[0];

                        newMob.active = ImageFilters.IsThisPixelRGB(mobsCropImage, new Point(pt.X, pt.Y),6);

                        string ocrAtt = OCR.DecodeImg(BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 60, pt.Y + 35, 60, 35)),sharpNumbersImages);
                        string ocrHp = OCR.DecodeImg(BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X , pt.Y + 35, 60, 35)), sharpNumbersImages);
                        
                        newMob.attack = ocrAtt;
                        newMob.hp = ocrHp; 
                        mobsOnBattlefield.Add(newMob);
                    }
                }
            }

            foreach (var quest in questsImages)
            {
                Point[] questLocations = ImageRecognition.multipleTemplateMatch(mobsCropImage, quest, Color.Red, 0.7);
                if (questLocations.Count() > 0)
                {
                    foreach (var pt in questLocations)
                    {
                        QuestInfo newQuest = new QuestInfo();
                        newQuest.location = pt;
                        newQuest.name = ((string)quest.Tag).Split('.')[0];

                        string ocr = OCR.DecodeImg(BitmapTransformations.Crop(mobsCropImage, new Rectangle(pt.X - 20, pt.Y + 35, 50, 35)), numberQuestImages);

                        newQuest.value = ocr;
                        questOnBattlefield.Add(newQuest);
                    }
                }
            }

            mobsCropImage = ResultVisualization.ShowMobsOnBattlefield(mobsOnBattlefield, mobsCropImage);
            mobsCropImage = ResultVisualization.ShowQuestsOnBattlefield(questOnBattlefield, mobsCropImage);
            pictureBox1.Image = mobsCropImage;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImageFilters.SharpenGaussian(ScreenShot.GetScreenShop("Lord of the Rings - LCG"), (int)numericUpDown1.Value,(int)numericUpDown2.Value,(double)numericUpDown3.Value, (double)numericUpDown4.Value,(int)numericUpDown5.Value).Bitmap.Save(@"C:\Users\piotr\Desktop\Nowy folder\sharpened.jpg");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap screenshot = ScreenShot.GetScreenShop("Lord of the Rings - LCG");
            //Bitmap screenshot = new Bitmap(sceneImage);

            Bitmap cardsScreenShot = BitmapTransformations.Crop(screenshot, new Rectangle(410, 840, 865, 160));
            List<CardInfo> cardsInHand = new List<CardInfo>();

            foreach (var cardValue in cardValueImages)
            {
                Point[] cardLocations = ImageRecognition.multipleTemplateMatch(cardsScreenShot, cardValue, Color.Red, 0.5);
                if (cardLocations.Count() > 0)
                {
                    foreach (var pt in cardLocations)
                    {
                        CardInfo newCard = new CardInfo();
                        newCard.location = pt;
                        newCard.value = ((string)cardValue.Tag).Split('.')[0];

                        cardsInHand.Add(newCard);
                    }
                }
            }

            cardsScreenShot = ResultVisualization.ShowCardsInHand(cardsInHand, cardsScreenShot);
            pictureBox1.Image = cardsScreenShot;
        }
    }
}
