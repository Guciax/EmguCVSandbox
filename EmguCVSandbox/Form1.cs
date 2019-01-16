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
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Point windowLoc = new Point(58, 1);

        string sceneImage = @"Images\bavkFullMobs.png";

        string questImage = @"Images\quest.png";
        string mobImage = @"Images\mob.png";
        string heroImage = @"Images\hero.png";
        string sobelMobImage = @"Images\sobelMob.jpg";
        
        DirectoryInfo mobsDir = new DirectoryInfo(@"Images\Mobs");
        DirectoryInfo questssDir = new DirectoryInfo(@"Images\Quests");
        DirectoryInfo numbersAttDir = new DirectoryInfo(@"Images\Numbers\Att");
        DirectoryInfo numbersHpDir = new DirectoryInfo(@"Images\Numbers\HP");
        DirectoryInfo numbersQuestDir = new DirectoryInfo(@"Images\Numbers\Quests");
        DirectoryInfo numbersSharp = new DirectoryInfo(@"Images\Numbers\Sharp");
        DirectoryInfo numbersCardsDir = new DirectoryInfo(@"Images\Numbers\Cards");
        DirectoryInfo herosDir = new DirectoryInfo(@"Images\HeroAlly");

        List<Bitmap> mobBitmaps = new List<Bitmap>();
        List<Bitmap> numberAttImages = new List<Bitmap>();
        List<Bitmap> numberHpImages = new List<Bitmap>();
        List<Bitmap> numberQuestImages = new List<Bitmap>();
        List<Bitmap> questsImages = new List<Bitmap>();
        List<Bitmap> sharpNumbersImages = new List<Bitmap>();
        List<Bitmap> cardValueImages = new List<Bitmap>();
        List<Bitmap> heroAlltImages = new List<Bitmap>();

        List<MobInfo> mobsOnBattlefield = new List<MobInfo>();
        List<QuestInfo> questOnBattlefield = new List<QuestInfo>();
        List<HeroAllyInfo> heroesAllyOnBattlefield = new List<HeroAllyInfo>();
        List<CardInfo> cardsInHand = new List<CardInfo>();

        private void Form1_Load(object sender, EventArgs e)
        {
            FileInfo[] heroFiles = herosDir.GetFiles();
            foreach (var heroF in heroFiles)
            {
                Bitmap newBmp = new Bitmap(heroF.FullName);
                newBmp.Tag = heroF.Name.Split('.')[0];
                heroAlltImages.Add(newBmp);
            }

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

        


        private void button1_Click(object sender, EventArgs e)
        {
            bool turnOnOsd = false;
            if (checkBox1.Checked)
            {
                checkBox1.Checked = false;
                turnOnOsd = true;
            }
            Bitmap screenshot = ScreenShot.GetScreenShop("Lord of the Rings - LCG");
            Debug.WriteLine("Making mobs crop");
            Bitmap mobsCropImage = BitmapTransformations.Crop(screenshot, new Rectangle(260, 350, 1170, 210));
            Debug.WriteLine("Making cards crop");
            Bitmap cardsScreenShot = BitmapTransformations.Crop(screenshot, new Rectangle(410, 840, 865, 160));
            Debug.WriteLine("Making hero crop");
            Bitmap heroesScreenShot = BitmapTransformations.Crop(screenshot, new Rectangle(350, 550, 1075, 210));

            mobsOnBattlefield = ScreenShotRecognition.ScanMobs(mobBitmaps, mobsCropImage, sharpNumbersImages);
            questOnBattlefield = ScreenShotRecognition.ScanQuests(questsImages, mobsCropImage, numberQuestImages);
            heroesAllyOnBattlefield = ScreenShotRecognition.ScanHeroes(heroAlltImages, heroesScreenShot, sharpNumbersImages);

            mobsCropImage = ResultVisualization.ShowMobsOnBattlefield(mobsOnBattlefield, mobsCropImage);
            mobsCropImage = ResultVisualization.ShowQuestsOnBattlefield(questOnBattlefield, mobsCropImage);

            cardsInHand = ScreenShotRecognition.ScanCards(cardValueImages, cardsScreenShot);
            cardsScreenShot = ResultVisualization.ShowCardsInHand(cardsInHand, cardsScreenShot);

            BitmapTransformations.PasteBitmap(screenshot, mobsCropImage, new Point(260, 350));
            BitmapTransformations.PasteBitmap(screenshot, cardsScreenShot, new Point(410, 840));

            pictureBox1.Image = screenshot;
            if (turnOnOsd)
            {
                checkBox1.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImageFilters.SharpenGaussian(ScreenShot.GetScreenShop("Lord of the Rings - LCG"), (int)numericUpDown1.Value,(int)numericUpDown2.Value,(double)numericUpDown3.Value, (double)numericUpDown4.Value,(int)numericUpDown5.Value).Bitmap.Save(@"Images\sharpened.jpg");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap screenshot = ScreenShot.GetScreenShop("Lord of the Rings - LCG");
            //Bitmap screenshot = new Bitmap(sceneImage);

            Bitmap cardsScreenShot = BitmapTransformations.Crop(screenshot, new Rectangle(410, 840, 865, 160));
            List<CardInfo> cardsInHand = ScreenShotRecognition.ScanCards(cardValueImages, cardsScreenShot);

            

            cardsScreenShot = ResultVisualization.ShowCardsInHand(cardsInHand, cardsScreenShot);
            pictureBox1.Image = cardsScreenShot;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                OnScreenDisplay osdForm = new OnScreenDisplay(cardsInHand, mobsOnBattlefield, questOnBattlefield, heroesAllyOnBattlefield, checkBox1, windowLoc);

                    osdForm.Show();

            }
        }
    }
}
