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
        private NewRecognition newRecognition;
        private Bot bot;
        public Form1()
        {
            InitializeComponent();
            NewRecognition newRecognition = new NewRecognition(this,logbook, mobBitmaps, ocrHeroNumber, ocrMobNumbers, questsImages, sharpNumbersImages, cardValueImages, heroAlltImages, MyAlltImages, moneyImages, okImages, defendImages, attachmentImages);
            Bot bot = new Bot(logbook);
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
            public static readonly int heroSocketWidth = 151;
            public static readonly int mobSocketWidth = 142;
            public static readonly Point[] pointsToCheckOkMark = new Point[] { new Point(0, 0), new Point(1, 1) }; //uzupelnic
            public static readonly Rectangle threadRegion = new Rectangle(1620, 692, 38, 27);
            public static readonly Point guardPoint = new Point(310, 875);

            public static readonly Dictionary<int, Bitmap> emptyFieldDict = new Dictionary<int, Bitmap>
                {
                        { 1, new Bitmap(@"Images\empty1.png") },
                        { 2, new Bitmap(@"Images\empty1.png") } //uzupelnic
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

            //FileInfo[] okFiles = okDir.GetFiles();
            //foreach (var okF in okFiles)
            //{
            //    Bitmap newBmp = new Bitmap(okF.FullName);
            //    newBmp.Tag = okF.Name.Split('.')[0];
            //    okImages.Add(newBmp);
            //}

            //FileInfo[] attachmentFiles = attachmentDir.GetFiles();
            //foreach (var attachmentF in attachmentFiles)
            //{
            //    Bitmap newBmp = new Bitmap(attachmentF.FullName);
            //    newBmp.Tag = attachmentF.Name.Split('.')[0];
            //    attachmentImages.Add(newBmp);
            //}


            //FileInfo[] defendFiles = defendDir.GetFiles();
            //foreach (var defendF in defendFiles)
            //{
            //    Bitmap newBmp = new Bitmap(defendF.FullName);
            //    newBmp.Tag = defendF.Name.Split('.')[0];
            //    defendImages.Add(newBmp);
            //}

            FileInfo[] heroFiles = herosDir.GetFiles();
            foreach (var heroF in heroFiles)
            {
                Bitmap newBmp = new Bitmap(heroF.FullName);
                newBmp.Tag = heroF.Name.Split('.')[0];
                heroAlltImages.Add(newBmp);
            }

            //FileInfo[] alliesFiles = alliesDir.GetFiles();
            //foreach (var alliesF in alliesFiles)
            //{
            //    Bitmap newBmp = new Bitmap(alliesF.FullName);
            //    newBmp.Tag = alliesF.Name.Split('.')[0];
            //    MyAlltImages.Add(newBmp);
            //}

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
            OCR.TesseractOcr(new Bitmap(@"Images\thread.PNG"));
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
                OnScreenDisplay osdForm = new OnScreenDisplay(AllyOnBattlefield, cardsInHand, mobsOnBattlefield, questOnBattlefield, heroesAllyOnBattlefield, checkBox1, Windows.GameWindowRectangle().Location);

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
            bbotscan_Click(sender,e);
            int activemobscount = 0;
            for (int i = 0; i < mobsOnBattlefield.Count; i++)
            {
                if (mobsOnBattlefield[i].active == true)
                {
                    tologbook("moby aktywne " + i + ": " + mobsOnBattlefield[i].name + " X=" + mobsOnBattlefield[i].location.X + " Y=" + mobsOnBattlefield[i].location.Y);
                    activemobscount++;
                }
            }

            tologbook("Aktywnych potworów jest "+ activemobscount);
            tologbook("Próba obrony");

            if (activemobscount > 0)
            {
                bool obronione = false;
                for (int i = 0; i < AllyOnBattlefield.Count; i++)
                {
                    if (AllyOnBattlefield[i].active == true)
                    {
                        tologbook("bronie aktywnym " + i + ": " + AllyOnBattlefield[i].name + " X=" + AllyOnBattlefield[i].location.X + " Y=" + AllyOnBattlefield[i].location.Y);
                        obronione = true;


                        tologbook("klikam goscia");
                        tologbook(" X=" + AllyOnBattlefield[i].location.X + GlobalParameters.heroRegion.X + " Y=" + AllyOnBattlefield[i].location.Y + GlobalParameters.heroRegion.Y);
                        //260, 590
                        mysz.MouseLeftClick(AllyOnBattlefield[i].location.X + GlobalParameters.heroRegion.X, AllyOnBattlefield[i].location.Y + GlobalParameters.heroRegion.Y);
                        Thread.Sleep(500);

                        tologbook("szukam obrony");
                        Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
                        defendOnScreen = NewRecognition.ScanDefend(defendImages, ss);
                        if (defendOnScreen.Count() > 0)
                        {
                            tologbook("klikam obrone");
                            tologbook(" X=" + defendOnScreen[0].location.X + " Y=" + defendOnScreen[0].location.Y);
                            mysz.MouseLeftClick(defendOnScreen[0].location.X, defendOnScreen[0].location.Y);
                        }
                        else
                        {
                            tologbook("nieznalazlem obrony - we are fucked");
                        }
                        break;
                    }
                }
                if (obronione==false)
                {
                    tologbook("nie mam przyjaciół do obrony przed aktywnymi mobami");
                }
            }
            else
            {
                tologbook("nie ma aktywnych enemy, nie bronie");
            }

        }

        private void bbotokscan_Click(object sender, EventArgs e)
        {
            Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
            okOnScreen = NewRecognition.ScanOk(okImages, ss);

            for (int i = 0; i < okOnScreen.Count; i++)
            {
                tologbook("Ok on screen " + i + ": " + okOnScreen[i].value + " X=" + okOnScreen[i].location.X + " Y=" + okOnScreen[i].location.Y);
            }

            tologbook("klikam ok");
            tologbook(" X=" + okOnScreen[0].location.X + " Y=" + okOnScreen[0].location.Y);
            mysz.MouseLeftClick(okOnScreen[0].location.X, okOnScreen[0].location.Y);
            Thread.Sleep(500);

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
            bbotscan_Click(sender, e);
            bbotscancards_Click(sender, e);
            int cards1 = 0;
            int cards2 = 0;
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                if (cardsInHand[i].value=="1")
                {
                    cards1++;
                }
                else if (cardsInHand[i].value =="2")
                {
                    cards2++;
                }
                
            }
            tologbook("kart o koszcie 1: " + cards1 + " kart o koszcie 2: " + cards2);
            if (money > 1 && cards2 > 0)
            {
                tologbook("zagrywam karte o koszcie 2");
                for (int i = 0; i < cardsInHand.Count; i++)
                {
                    if (cardsInHand[i].value == "2")
                    {
                        mysz.MouseDragLeft(cardsInHand[i].location.X+ GlobalParameters.cardsRegion.X, cardsInHand[i].location.Y + GlobalParameters.cardsRegion.Y, 1300, 600);
                        tologbook("x: " + (cardsInHand[i].location.X + 410) + " Y: " + (cardsInHand[i].location.Y + 866));
                        tologbook("x: " + (cardsInHand[i].location.X) + " Y: " + (cardsInHand[i].location.Y));
                        Thread.Sleep(500);
                        tologbook("sprawdzam czy to nie jest attachment");
                        Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
                        attachmentOnScreen = NewRecognition.ScanAttachment(attachmentImages, ss);
                        if (attachmentOnScreen.Count()>0)
                        {
                            tologbook("to jest attachment "+attachmentOnScreen[0].name);
                            tologbook("daje bohaterowi 0");
                            
                            mysz.MouseLeftClick(heroesAllyOnBattlefield[0].location.X + GlobalParameters.heroRegion.X, heroesAllyOnBattlefield[0].location.Y + GlobalParameters.heroRegion.Y);
                            Thread.Sleep(500);
                        }

                        
                        
                        
                        Thread.Sleep(500);

                        break;

                    }
 
                }
            }
            else if (money > 1 && cards1 > 0)
            {
                tologbook("zagrywam karte o koszcie 1");
                for (int i = 0; i < cardsInHand.Count; i++)
                {
                    if (cardsInHand[i].value == "1")
                    {
                        mysz.MouseDragLeft(cardsInHand[i].location.X + GlobalParameters.cardsRegion.X, cardsInHand[i].location.Y + GlobalParameters.cardsRegion.Y, 1300, 600);

                        Thread.Sleep(500);
                        break;

                    }

                }
            }
            else if (money == 1 && cards1 > 0)
            {
                tologbook("zagrywam karte o koszcie 1");
                for (int i = 0; i < cardsInHand.Count; i++)
                {
                    if (cardsInHand[i].value == "1")
                    {
                        mysz.MouseDragLeft(cardsInHand[i].location.X + GlobalParameters.cardsRegion.X, cardsInHand[i].location.Y + GlobalParameters.cardsRegion.Y, 1300, 600);

                        Thread.Sleep(500);
                        break;

                    }

                }
            }
            else
            {
                tologbook("No money to play cards or no cards to play");
            }


       

            

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
            GameCurrentState cur = newRecognition.ScanGame();
            ;
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
                OnScreenDisplay osdForm = new OnScreenDisplay(AllyOnBattlefield, cardsInHand, mobsOnBattlefield, questOnBattlefield, heroesAllyOnBattlefield, botosd, Windows.GameWindowRectangle().Location);

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


    }
}
