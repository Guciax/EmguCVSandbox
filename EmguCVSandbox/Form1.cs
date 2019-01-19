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
using System.Threading;
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
        public class GlobalParameters
        {
            public static readonly Rectangle mobsRegion = new Rectangle(260, 355, 1166, 170);
            public static readonly Rectangle heroRegion = new Rectangle(260, 590, 1166, 184);
            public static readonly Rectangle cardsRegion = new Rectangle(410, 866, 865, 160);
            public static readonly Rectangle cashRegion = new Rectangle(311, 844, 29, 35);
            public static readonly Rectangle endPhaseButtonRegion = new Rectangle(1280, 1000, 187, 38);
            public static readonly Rectangle questPhaseNameRegion = new Rectangle(700, 47, 281, 32);
            public static int heroSocketWidth = 151;
            public static int mobSocketWidth = 142;
            public static Point[] pointsToCheckOkMark = new Point[] { new Point(0, 0), new Point(1, 1) }; //uzupelnic

            public static readonly Bitmap emptyBmpPhase1 = new Bitmap(@"Images\empty1.png");
        }

        static string linestr = @"Images\linia.PNG";
        Bitmap linebm = new Bitmap(linestr,true);

        DirectoryInfo mobsDir = new DirectoryInfo(@"Images\Newmobs");
        DirectoryInfo questssDir = new DirectoryInfo(@"Images\Newevents");
        DirectoryInfo numbersAttDir = new DirectoryInfo(@"Images\Numbers\Att");
        DirectoryInfo numbersHpDir = new DirectoryInfo(@"Images\Numbers\HP");
        DirectoryInfo numbersQuestDir = new DirectoryInfo(@"Images\Numbers\Quests");
        DirectoryInfo numbersSharp = new DirectoryInfo(@"Images\Numbers\Sharp");
        DirectoryInfo numbersHeroDir = new DirectoryInfo(@"Images\Numbers\Hero");
        DirectoryInfo numbersMobsDir = new DirectoryInfo(@"Images\Numbers\Mob");
        DirectoryInfo numbersCardsDir = new DirectoryInfo(@"Images\Numbers\Cards");
        DirectoryInfo herosDir = new DirectoryInfo(@"Images\HeroAlly");
        DirectoryInfo moneyDir = new DirectoryInfo(@"Images\Numbers\Money");

        List<Bitmap> mobBitmaps = new List<Bitmap>();

        List<Bitmap> ocrHeroNumber = new List<Bitmap>();
        List<Bitmap> ocrMobNumbers = new List<Bitmap>();
        
        List<Bitmap> questsImages = new List<Bitmap>();
        List<Bitmap> sharpNumbersImages = new List<Bitmap>();
        List<Bitmap> cardValueImages = new List<Bitmap>();
        List<Bitmap> heroAlltImages = new List<Bitmap>();
        List<Bitmap> moneyImages = new List<Bitmap>();

        List<MobInfo> mobsOnBattlefield = new List<MobInfo>();
        List<QuestInfo> questOnBattlefield = new List<QuestInfo>();
        List<HeroAllyInfo> heroesAllyOnBattlefield = new List<HeroAllyInfo>();
        List<CardInfo> cardsInHand = new List<CardInfo>();

        int money = 0;

        Stopwatch stopWatch = new Stopwatch();
        TimeSpan ts = new TimeSpan();

        private void Form1_Load(object sender, EventArgs e)
        {

            FileInfo[] moneyFiles = moneyDir.GetFiles();
            foreach (var moneyF in moneyFiles)
            {
                Bitmap newBmp = new Bitmap(moneyF.FullName);
                newBmp.Tag = moneyF.Name.Split('.')[0];
                moneyImages.Add(newBmp);
            }

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

            FileInfo[] numberHeroFiles = numbersHeroDir.GetFiles();
            foreach (var num in numberHeroFiles)
            {
                Bitmap newBmp = new Bitmap(num.FullName);
                newBmp.Tag = num.Name.Split('.')[0];
                ocrHeroNumber.Add(newBmp);
            }

            FileInfo[] numberMobFiles = numbersMobsDir.GetFiles();
            foreach (var num in numberMobFiles)
            {
                Bitmap newBmp = new Bitmap(num.FullName);
                newBmp.Tag = num.Name.Split('.')[0];
                ocrMobNumbers.Add(newBmp);
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

            pictureBox2.Image = linebm;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImageFilters.SharpenGaussian(ScreenShot.GetScreenShop(Windows.GameWindowRectangle()), (int)numericUpDown1.Value,(int)numericUpDown2.Value,(double)numericUpDown3.Value, (double)numericUpDown4.Value,(int)numericUpDown5.Value).Bitmap.Save(@"Images\sharpened.jpg");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap screenshot = ScreenShot.GetScreenShop(Windows.GameWindowRectangle());
            //Bitmap screenshot = new Bitmap(sceneImage);

            Bitmap cardsScreenShot = BitmapTransformations.Crop(screenshot, new Rectangle(410, 840, 865, 160));
            //List<CardInfo> cardsInHand = ScreenShotRecognition.ScanCards(cardValueImages, cardsScreenShot);

            

            cardsScreenShot = ResultVisualization.ShowCardsInHand(cardsInHand, cardsScreenShot);
            pictureBox1.Image = cardsScreenShot;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                OnScreenDisplay osdForm = new OnScreenDisplay(cardsInHand, mobsOnBattlefield, questOnBattlefield, heroesAllyOnBattlefield, checkBox1, Windows.GameWindowRectangle().Location);

                    osdForm.Show();

            }
        }
        

       // "{i} = {cardsInHand[i].location.X}"
        void outToLog(string output)
        {
            if (!string.IsNullOrWhiteSpace(richTextBox1.Text))
            {
                richTextBox1.AppendText("\r\n" + output);
            }
            else
            {
                richTextBox1.AppendText(output);
            }
            richTextBox1.ScrollToCaret();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;

            Bitmap ss = ScreenShot.GetScreenShop(Windows.GameWindowRectangle());

            mobsOnBattlefield = NewRecognition.ScanMobs(mobBitmaps, ocrMobNumbers, GlobalParameters.emptyBmpPhase1, ss);
            heroesAllyOnBattlefield = NewRecognition.ScanHeroes(heroAlltImages, ocrHeroNumber, GlobalParameters.emptyBmpPhase1, ss);
            questOnBattlefield = NewRecognition.ScanQuests(ref mobsOnBattlefield, questsImages, ocrMobNumbers, ss);
            money = NewRecognition.ScanCash(ss, moneyImages);
            checkBox1.Checked = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                Bitmap screensaver = ScreenShot.ScreenShopSaver("braknazwyscreena", Windows.GameWindowRectangle());
                pictureBox3.Image = screensaver;
            }
            else
            {
                Bitmap screensaver = ScreenShot.ScreenShopSaver( textBox1.Text, Windows.GameWindowRectangle());
                pictureBox3.Image = screensaver;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Bitmap mobCrop = BitmapTransformations.Crop(ScreenShot.GetScreenShop(Windows.GameWindowRectangle()), GlobalParameters.mobsRegion);
            Bitmap emptyCrop = BitmapTransformations.Crop(GlobalParameters.emptyBmpPhase1, GlobalParameters.mobsRegion);
            
            Point[] pointsOfMobs = NewRecognition.pointsOfInterest(mobCrop, emptyCrop, GlobalParameters.mobSocketWidth);
            Bitmap[] bitmapsOfPoints = BitmapTransformations.TakeBitmapsInPoints(mobCrop, pointsOfMobs, new Size(Int32.Parse(textBox2.Text), Int32.Parse(textBox3.Text)));
               foreach (var bmp in bitmapsOfPoints)
               {
                   bmp.Save($@"Images\{textBox1.Text}{((Point)bmp.Tag).X}.png");
               }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                pictureBox3.Image.Save(@"Images\ss.PNG");
            }
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(@"Images\NumbersToTransform");
            FileInfo[] files = dir.GetFiles();
            foreach (var file in files)
            {
                Bitmap bmp = new Bitmap(file.FullName);
                ImageFilters.RemoveColorFromImage(bmp, 75).Save($@"Images\NumbersToTransform\Output\{file.Name}.png");
            }
        }
    }
}
