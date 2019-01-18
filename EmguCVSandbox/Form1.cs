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

        Point windowLoc = new Point(58, 1);

        string sceneImage = @"Images\bavkFullMobs.png";
        string questImage = @"Images\quest.png";
        string mobImage = @"Images\mob.png";
        string heroImage = @"Images\hero.png";
        string sobelMobImage = @"Images\sobelMob.jpg";
        static string linestr = @"Images\linia.PNG";

        Bitmap linebm = new Bitmap(linestr,true);

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

        Stopwatch stopWatch = new Stopwatch();
        TimeSpan ts = new TimeSpan();


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

            pictureBox2.Image = linebm;
        }

        


        private void button1_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("<----- Scan Start ----->");
            bool turnOnOsd = false;
            if (checkBox1.Checked)
            {
                checkBox1.Checked = false;
                turnOnOsd = true;
            }
            stopWatch.Start();
            


            Bitmap screenshot = ScreenShot.GetScreenShop("Lord of the Rings - LCG");

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("Bitmap screenshot= {"+ Math.Round(ts.TotalMilliseconds,0) + "}");
            stopWatch.Reset();
            stopWatch.Start();
            Debug.WriteLine("Making mobs crop");
            Bitmap mobsCropImage = BitmapTransformations.Crop(screenshot, new Rectangle(260, 350, 1170, 210));
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("Bitmap mobsCropImage= {"+Math.Round(ts.TotalMilliseconds,0)+"}");
            stopWatch.Reset();
            stopWatch.Start();
            Debug.WriteLine("Making cards crop");
            Bitmap cardsScreenShot = BitmapTransformations.Crop(screenshot, new Rectangle(410, 840, 865, 160));
            stopWatch.Stop();           
            ts = stopWatch.Elapsed;
            outToLog("Bitmap cardsScreenShot= {"+Math.Round(ts.TotalMilliseconds,0)+"}");
            stopWatch.Reset();
            stopWatch.Start();
            Debug.WriteLine("Making hero crop");
            Bitmap heroesScreenShot = BitmapTransformations.Crop(screenshot, new Rectangle(350, 570, 1075, 210));
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("Bitmap heroesScreenShot= {"+Math.Round(ts.TotalMilliseconds,0)+"}");
            stopWatch.Reset();

            stopWatch.Start();
            
            mobsOnBattlefield = ScreenShotRecognition.ScanMobs(mobBitmaps, mobsCropImage, sharpNumbersImages);
            lnumberofenemies.Text = cardsInHand.Count().ToString();

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("ScreenShotRecognition.ScanMobs= {" + Math.Round(ts.TotalMilliseconds, 0) + "}");
            stopWatch.Reset();
            stopWatch.Start();
            questOnBattlefield = ScreenShotRecognition.ScanQuests(questsImages, mobsCropImage, numberQuestImages);
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("ScreenShotRecognition.ScanQuests= {" + Math.Round(ts.TotalMilliseconds, 0) + "}");
            stopWatch.Reset();
            stopWatch.Start();
            heroesAllyOnBattlefield = ScreenShotRecognition.ScanHeroes(heroAlltImages, heroesScreenShot, sharpNumbersImages);
            lnumberofallies.Text = cardsInHand.Count().ToString();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("ScreenShotRecognition.ScanHeroes= {"+Math.Round(ts.TotalMilliseconds,0)+"}");
            stopWatch.Reset();
            stopWatch.Start();

            mobsCropImage = ResultVisualization.ShowMobsOnBattlefield(mobsOnBattlefield, mobsCropImage);
            mobsCropImage = ResultVisualization.ShowQuestsOnBattlefield(questOnBattlefield, mobsCropImage);

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("ResultVisualization= {"+Math.Round(ts.TotalMilliseconds,0)+"}");
            stopWatch.Reset();
            stopWatch.Start();
            cardsInHand = ScreenShotRecognition.ScanCards(cardValueImages, cardsScreenShot);
            lnumberofcards.Text=cardsInHand.Count().ToString();
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("ScreenShotRecognition.ScanCards= {"+Math.Round(ts.TotalMilliseconds,0)+"}");
            stopWatch.Reset();
            stopWatch.Start();
            cardsScreenShot = ResultVisualization.ShowCardsInHand(cardsInHand, cardsScreenShot);
            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("ResultVisualization.ShowCardsInHand= {"+Math.Round(ts.TotalMilliseconds,0)+"}");
            stopWatch.Reset();
            stopWatch.Start();

            BitmapTransformations.PasteBitmap(screenshot, mobsCropImage, new Point(260, 350));
            BitmapTransformations.PasteBitmap(screenshot, cardsScreenShot, new Point(410, 840));

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("BitmapTransformations.PasteBitmap= {"+Math.Round(ts.TotalMilliseconds,0)+"}");
            stopWatch.Reset();
            stopWatch.Start();



            pictureBox1.Image = screenshot;

            stopWatch.Stop();
            ts = stopWatch.Elapsed;
            outToLog("pictureBox1.Image = screenshot= {"+Math.Round(ts.TotalMilliseconds,0)+"}");

            stopWatch.Reset();
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
            Stopwatch st = new Stopwatch();
            st.Start();
            Bitmap mobCrop = BitmapTransformations.Crop(ScreenShot.GetScreenShop("Lord of the Rings - LCG"), new Rectangle(260, 350, 1170, 190));
            Point[] pointsOfMobs = NewRecognition.pointsOfInterest(mobCrop);
            Bitmap[] bitmapsOfPoints = BitmapTransformations.TakeBitmapsInPoints(mobCrop,pointsOfMobs, new Size(30,30));
         //   foreach (var bmp in bitmapsOfPoints)
         //   {
         //       bmp.Save(@"Images\" + ((Point)bmp.Tag).X + "png");
         //   }
            Bitmap sharpenedBitmap = ImageFilters.SharpenGaussian(mobCrop, 11, 19, 84, 500, 500).Bitmap;
            foreach (var bmp in bitmapsOfPoints)
            {
                foreach (var mobBmp in mobBitmaps) //new mob bitmaps!!!! :(((
                {
                    double matchResult = ImageRecognition.SingleTemplateMatch(mobBmp, bmp);
                    if (matchResult < 0.9) continue;

                    Point pt = (Point)bmp.Tag; 
                    MobInfo newMob = new MobInfo();
                    newMob.location = pt;
                    newMob.name = mobBmp.Tag.ToString();
                    newMob.active = ImageFilters.IsThisPixelRGB(mobCrop, pt, 6);
                    Bitmap attCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X - 60, pt.Y + 35, 60, 35));
                    string ocrAtt = OCR.DecodeImg(attCrop, sharpNumbersImages);

                    Bitmap defCrop = BitmapTransformations.Crop(sharpenedBitmap, new Rectangle(pt.X, pt.Y + 35, 60, 35));
                    string ocrHp = OCR.DecodeImg(defCrop, sharpNumbersImages);

                    newMob.attack = ocrAtt;
                    newMob.hp = ocrHp;
                    mobsOnBattlefield.Add(newMob);
                }
            }
            st.Stop();
            ;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                Bitmap screensaver = ScreenShot.ScreenShopSaver("Lord of the Rings - LCG", "braknazwyscreena");
                pictureBox3.Image = screensaver;
            }
            else
            {
                Bitmap screensaver = ScreenShot.ScreenShopSaver("Lord of the Rings - LCG", textBox1.Text);
                pictureBox3.Image = screensaver;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Bitmap mobCrop = BitmapTransformations.Crop(ScreenShot.GetScreenShop("Lord of the Rings - LCG"), new Rectangle(260, 350, 1170, 190));
            Point[] pointsOfMobs = NewRecognition.pointsOfInterest(mobCrop);
            Bitmap[] bitmapsOfPoints = BitmapTransformations.TakeBitmapsInPoints(mobCrop, pointsOfMobs, new Size(Int32.Parse(textBox2.Text), Int32.Parse(textBox3.Text)));
               foreach (var bmp in bitmapsOfPoints)
               {
                   bmp.Save($@"Images\{textBox1.Text}{((Point)bmp.Tag).X}.png");
               }
        }
    }
}
