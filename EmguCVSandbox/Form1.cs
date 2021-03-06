﻿using Emgu.CV;
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
        private NewRecognition newRecognition;
        private Bot bot;
        public Form1()
        {
            InitializeComponent();
            newRecognition = new NewRecognition(this,logbook, mobBitmaps, ocrHeroNumber, ocrMobNumbers, questsImages, sharpNumbersImages, cardValueImages, heroAlltImages, MyAlltImages, moneyImages, okImages, defendImages, attachmentImages);
            bot = new Bot(logbook,okImages,defendImages,attachmentImages);
        }



        public class GlobalParameters
        {
            public static readonly Rectangle mobsRegion = new Rectangle(260, 355, 1166, 170);
            public static readonly Rectangle heroRegion = new Rectangle(260, 590, 1166, 184);
            public static readonly Rectangle cardsRegion = new Rectangle(410, 866, 865, 160);
            public static readonly Rectangle cashRegion = new Rectangle(311, 844, 29, 35);
            public static readonly Rectangle attachmentRegion = new Rectangle(464, 176, 120, 120);
            public static readonly Rectangle endPhaseButtonRegion = new Rectangle(1280, 1000, 187, 38);
            public static readonly Rectangle questPhaseNameRegion = new Rectangle(700, 47, 281, 32);
            public static int heroSocketWidth = 151;
            public static int mobSocketWidth = 142;
            public static Point[] pointsToCheckOkMark = new Point[] { new Point(0, 0), new Point(1, 1) }; //uzupelnic

            public static readonly Dictionary<int, Bitmap> emptyFieldDict = new Dictionary<int, Bitmap>
                {
                        { 1, new Bitmap(@"Images\empty1.png") },
                        { 2, new Bitmap(@"Images\empty2.png") } //uzupelnic
            };
            public static readonly Bitmap emptyBmpPhase1 = new Bitmap(@"Images\empty1.png");
            public static string procname = "Lord of the Rings - LCG";
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
        DirectoryInfo alliesDir = new DirectoryInfo(@"Images\Newallay");
        DirectoryInfo moneyDir = new DirectoryInfo(@"Images\Numbers\Money");
        DirectoryInfo okDir = new DirectoryInfo(@"Images\Ok");
        DirectoryInfo defendDir = new DirectoryInfo(@"Images\Defend");
        DirectoryInfo attachmentDir = new DirectoryInfo(@"Images\Attachment");


        List<Bitmap> mobBitmaps = new List<Bitmap>();

        List<Bitmap> ocrHeroNumber = new List<Bitmap>();
        List<Bitmap> ocrMobNumbers = new List<Bitmap>();
        
        List<Bitmap> questsImages = new List<Bitmap>();
        List<Bitmap> sharpNumbersImages = new List<Bitmap>();
        List<Bitmap> cardValueImages = new List<Bitmap>();
        List<Bitmap> heroAlltImages = new List<Bitmap>();
        List<Bitmap> MyAlltImages = new List<Bitmap>();
        List<Bitmap> moneyImages = new List<Bitmap>();
        List<Bitmap> okImages = new List<Bitmap>();
        List<Bitmap> defendImages = new List<Bitmap>();
        List<Bitmap> attachmentImages = new List<Bitmap>();

        List<MobInfo> mobsOnBattlefield = new List<MobInfo>();
        List<QuestInfo> questOnBattlefield = new List<QuestInfo>();
        List<HeroAllyInfo> heroesAllyOnBattlefield = new List<HeroAllyInfo>();
        List<MyAllyInfo> AllyOnBattlefield = new List<MyAllyInfo>();
        List<CardInfo> cardsInHand = new List<CardInfo>();
        List<OkInfo> okOnScreen = new List<OkInfo>();
        List<DefendInfo> defendOnScreen = new List<DefendInfo>();
        List<AttachmentInfo> attachmentOnScreen = new List<AttachmentInfo>();

        //BOT data
        
        int money = 0;
        int nherose = 0;
        int ncards = 0;
        int nallies = 0;
        int nenemies = 0;


        //do przeniesienia do bota
        //---------------------------------
        bool eowynranger = false;
        bool eowynraven = false;

        bool gimliranger = false;
        bool gimliraven = false;

        bool dwalinranger = false;
        bool dwalinraven = false;
        //-------------------------------

        Stopwatch stopWatch = new Stopwatch();
        TimeSpan ts = new TimeSpan();
        Mouse mysz = new Mouse();
        


        private void Form1_Load(object sender, EventArgs e)
        {
           
            

            FileInfo[] moneyFiles = moneyDir.GetFiles();
            foreach (var moneyF in moneyFiles)
            {
                Bitmap newBmp = new Bitmap(moneyF.FullName);
                newBmp.Tag = moneyF.Name.Split('.')[0];
                moneyImages.Add(newBmp);
            }

            FileInfo[] okFiles = okDir.GetFiles();
            foreach (var okF in okFiles)
            {
                Bitmap newBmp = new Bitmap(okF.FullName);
                newBmp.Tag = okF.Name.Split('.')[0];
                okImages.Add(newBmp);
            }

            FileInfo[] attachmentFiles = attachmentDir.GetFiles();
            foreach (var attachmentF in attachmentFiles)
            {
                Bitmap newBmp = new Bitmap(attachmentF.FullName);
                newBmp.Tag = attachmentF.Name.Split('.')[0];
                attachmentImages.Add(newBmp);
            }


            FileInfo[] defendFiles = defendDir.GetFiles();
            foreach (var defendF in defendFiles)
            {
                Bitmap newBmp = new Bitmap(defendF.FullName);
                newBmp.Tag = defendF.Name.Split('.')[0];
                defendImages.Add(newBmp);
            }

            FileInfo[] heroFiles = herosDir.GetFiles();
            foreach (var heroF in heroFiles)
            {
                Bitmap newBmp = new Bitmap(heroF.FullName);
                newBmp.Tag = heroF.Name.Split('.')[0];
                heroAlltImages.Add(newBmp);
            }

            FileInfo[] alliesFiles = alliesDir.GetFiles();
            foreach (var alliesF in alliesFiles)
            {
                Bitmap newBmp = new Bitmap(alliesF.FullName);
                newBmp.Tag = alliesF.Name.Split('.')[0];
                MyAlltImages.Add(newBmp);
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
            ImageFilters.SharpenGaussian(ScreenShot.GetScreenShot(Windows.GameWindowRectangle()), (int)numericUpDown1.Value,(int)numericUpDown2.Value,(double)numericUpDown3.Value, (double)numericUpDown4.Value,(int)numericUpDown5.Value).Bitmap.Save(@"Images\sharpened.jpg");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap screenshot = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
            //Bitmap screenshot = new Bitmap(sceneImage);

            Bitmap cardsScreenShot = BitmapTransformations.Crop(screenshot, new Rectangle(410, 840, 865, 160));
            List<CardInfo> cardsInHand = ScreenShotRecognition.ScanCards(cardValueImages, cardsScreenShot);

            

            //cardsScreenShot = ResultVisualization.ShowCardsInHand(cardsInHand, cardsScreenShot);
            //pictureBox1.Image = cardsScreenShot;
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

        void tologbook(string output)
        {

            if (!string.IsNullOrWhiteSpace(logbook.Text))
            {
                logbook.AppendText("\r\n" + output);
            }
            else
            {
                logbook.AppendText(output);
            }
            logbook.ScrollToCaret();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;

            Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
            
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
            Bitmap mobCrop = BitmapTransformations.Crop(ScreenShot.GetScreenShot(Windows.GameWindowRectangle()), GlobalParameters.mobsRegion);
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

        private void bbotstart_Click(object sender, EventArgs e)
        {
            bot.startquest();
           
        }

        private void bbotscancards_Click(object sender, EventArgs e)
        {
            Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
            cardsInHand = NewRecognition.ScanCards(cardValueImages, ss);
            ncards = cardsInHand.Count();
            tologbook("Ilosc kart" + ncards);
            lbotncards.Text = ncards.ToString();

            for (int i = 0; i < cardsInHand.Count; i++)
            {
                tologbook("karta " + i + ": " +cardsInHand[i].value + " X=" + cardsInHand[i].location.X+" Y="+cardsInHand[i].location.Y);
            }

        }

        private void bbotserchactiveenemy_Click(object sender, EventArgs e)
        {
            GameCurrentState gamecurrentstate = newRecognition.ScanGame(1);
            bot.defendIfAble(gamecurrentstate);

        }

        private void bbotokscan_Click(object sender, EventArgs e)
        {
            

            

        }

        private void bbotclickok_Click(object sender, EventArgs e)
        {
            tologbook("klikam ok");
            tologbook(" X=" + okOnScreen[0].location.X + " Y=" + okOnScreen[0].location.Y);
            mysz.MouseLeftClick(okOnScreen[0].location.X, okOnScreen[0].location.Y);
            Thread.Sleep(500);
        }

        private void bplaycard_Click(object sender, EventArgs e)
        {
            GameCurrentState gamecurrentstate = newRecognition.ScanGame(1);
            bot.playcardfromhand(gamecurrentstate);

        }

        private void bbotscan_Click(object sender, EventArgs e)
        {
            tologbook("skanuje gre");
            //   Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
            //   mobsOnBattlefield = NewRecognition.ScanMobs(mobBitmaps, ocrMobNumbers, GlobalParameters.emptyBmpPhase1, ss);
            //   heroesAllyOnBattlefield = NewRecognition.ScanHeroes(heroAlltImages, ocrHeroNumber, GlobalParameters.emptyBmpPhase1, ss);
            //   AllyOnBattlefield = NewRecognition.ScanAllies(MyAlltImages, ocrHeroNumber, GlobalParameters.emptyBmpPhase1, ss);
            //   questOnBattlefield = NewRecognition.ScanQuests(ref mobsOnBattlefield, questsImages, ocrMobNumbers, ss);
            //cardsInHand = NewRecognition.ScanCards(cardValueImages, ss);
            GameCurrentState gamecurrentstate = newRecognition.ScanGame(1);


            //money = NewRecognition.ScanCash(ss, moneyImages);
            //tologbook("Kasa:" + money);
            //nherose = heroesAllyOnBattlefield.Count();
            //tologbook("Ilosc bohaterów" + nherose);
            //nenemies = mobsOnBattlefield.Count();
            //tologbook("Ilosc wrogów" + nenemies);
            //tologbook("Ilosc questów" + questOnBattlefield.Count());
            //lbotnenemies.Text = nenemies.ToString();
            //lbotnheroes.Text = nherose.ToString();
            //lbotnmoney.Text = money.ToString();
            //nallies = AllyOnBattlefield.Count();
            //tologbook("Ilosc alliasow" + nallies);
            //lbotnallies.Text = nallies.ToString();

            //ncards = cardsInHand.Count();
            //tologbook("Ilosc kart" + ncards);
            //lbotncards.Text = ncards.ToString();


        }
        private void bbotclearlog_Click(object sender, EventArgs e)
        {
            logbook.Clear();
        }

        private void button9_Click(object sender, EventArgs e)
        {

            ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
            procInfo.FileName = ("mspaint.exe");

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                procInfo.Arguments = @"Images\braknazwyscreena.PNG";

            }
            else
            {
                procInfo.Arguments = @"Images\" + textBox1.Text + ".PNG";
            }


            Process.Start(procInfo);
        }
        private void botosd_CheckedChanged(object sender, EventArgs e)
        {
            if (botosd.Checked)
            {
                GameCurrentState cur = newRecognition.ScanGame(1);
                OnScreenDisplay osdForm = new OnScreenDisplay(cur.cardsInHand, cur.mobs, cur.quests, cur.heroesAlly, botosd, Windows.GameWindowRectangle().Location);

                osdForm.Show();
                
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {

            
            Bitmap emptyBattlefieldCrop = BitmapTransformations.Crop(GlobalParameters.emptyBmpPhase1, GlobalParameters.heroRegion);

            Bitmap heroCrop = BitmapTransformations.Crop(ScreenShot.GetScreenShot(Windows.GameWindowRectangle()), GlobalParameters.heroRegion);
            Point[] pointsOfHerroAlly = NewRecognition.pointsOfInterest(heroCrop, emptyBattlefieldCrop, GlobalParameters.heroSocketWidth);
            Bitmap[] bitmapsOfPoints = BitmapTransformations.TakeBitmapsInPoints(heroCrop, pointsOfHerroAlly, new Size(Int32.Parse(textBox2.Text), Int32.Parse(textBox3.Text)));


           foreach (var bmp in bitmapsOfPoints)
            {
                bmp.Save($@"Images\{textBox1.Text}{((Point)bmp.Tag).X}.png");
            }
        }

        private void bbotanalyse_Click(object sender, EventArgs e)
        {

            GameCurrentState gamecurrentstate = newRecognition.ScanGame(2);
            tologbook("ile qestow" + gamecurrentstate.quests.Count);
            if (gamecurrentstate.quests.Count > 0)
            {
                tologbook("ile qestow" + gamecurrentstate.quests[0].location.X);
                tologbook("ile qestow" + gamecurrentstate.quests[0].location.Y);
            }
            tologbook("ile aktywnychalliesow:" + gamecurrentstate.activeHeroesList.Count);
            for (int i=0; gamecurrentstate.activeHeroesList.Count<i;i++ )
            {
                tologbook("ktory " + gamecurrentstate.activeHeroesList[i].name);
                tologbook("ktory X " + gamecurrentstate.activeHeroesList[i].location.X);
                tologbook("ktory Y " + gamecurrentstate.activeHeroesList[i].location.Y);

            }
            

            //bot.checkEndButton();
            //GameCurrentState gamecurrentstate = newRecognition.ScanGame();
            //tologbook("gamecurrentstate.mobs.Count:" + gamecurrentstate.mobs.Count);
            //tologbook("activeAlliesNumber:" + gamecurrentstate.activeAlliesNumber);
            //tologbook("myAllies.Count:" + gamecurrentstate.myAllies.Count);
            //tologbook("activeHeroesNumber:" + gamecurrentstate.activeHeroesNumber);
            //tologbook("heroesAlly.Count:" + gamecurrentstate.heroesAlly.Count);
            //tologbook("mobsMaxatt[0].attack:" + gamecurrentstate.mobsMaxatt[0].attack);
            //tologbook("cash:" + gamecurrentstate.cash);
            //tologbook("cardsInHand.Count:" + gamecurrentstate.cardsInHand.Count);
            //tologbook("activeMobspriority1:" + gamecurrentstate.activeMobspriority1);





        }

        private void botplay_Click(object sender, EventArgs e)
        {
            int phase = 1;
            int questposzeld = 0;
            int jedziemy = Convert.ToInt32(Math.Round(numericUpDownBotIterations.Value, 0));
            for (int i = 0; jedziemy > i; i++)
            {

                Thread.Sleep(500);
                // skanuje czy koniec tury
                if (bot.checkEndButton(phase) == false)
                {
                    bot.checkOkButtonAndClick();
                }
                else
                {
                    
                    //skanuje gre
                    GameCurrentState gamecurrentstate = newRecognition.ScanGame(phase);
                    //patrze czy moge sie bronic jak sie bronie to ok jak nie to karta
                    if (bot.defendIfAble(gamecurrentstate) == true)
                    {
                        //obronilem
                        questposzeld = 0;
                    }
                    //jak sie nie obronilem to patrze czy moge zagrac karte
                    else if (bot.playcardfromhand(gamecurrentstate) == true)
                    {
                        bot.checkOkButtonAndClick();
                        questposzeld = 0;
                    }
                    //teraz napindalam
                    else if (bot.attackMob(gamecurrentstate) == true)
                    {
                        // mysz.MouseLeftClick(1365, 1027);
                        // klikaj koniec tury
                        bot.checkOkButtonAndClick();
                        questposzeld = 0;
                    }
                    //jak nie ma mobów to sprawdzam quest

                    else if (bot.goonquest(gamecurrentstate )== true)
                    {
                        bot.checkOkButtonAndClick();
                        questposzeld = questposzeld + 1;
                        if (questposzeld > 5)
                        {
                            mysz.MouseLeftClick(1292, 1011);
                        }


                    }
                    //jak nie ma mobów to sprawdzam travel
                    else if (gamecurrentstate.mobs.Count() == 0)
                    {
                        if (bot.checkTravelButton() == true)
                        {
                            questposzeld = 0;
                            mysz.MouseLeftClick(837, 220);
                            phase = 2;
                            Thread.Sleep(5000);
                            
                            //tologbook("klikam continue tego biadolenia");
                            mysz.MouseLeftClick(1446, 990);
                            //tologbook("czekam 1000");
                            Thread.Sleep(1000);
                            //tologbook("klikam continue tego biadolenia 2");
                            mysz.MouseLeftClick(1446, 990);
                            Thread.Sleep(1000);
                            //tologbook("klikam continue tego biadolenia 3");
                            mysz.MouseLeftClick(1446, 990);
                            //tologbook("czekam 6000");
                            Thread.Sleep(6000);
                            
                            //tologbook("klikam ok");
                            mysz.MouseLeftClick(840, 250);




                        }
                        
                    }
                    //klikam koniec tury bo nic nie pozostalo do roboty
                    else
                    {
                        questposzeld = 0;
                        mysz.MouseLeftClick(1292, 1011);
                    }
                }
                    //zagrywam karte

                    //sprawdzam ok
                    
                    bot.checkOkButtonAndClick();

            }


            }
        }

        }
   

