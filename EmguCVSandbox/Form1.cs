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
        public Form1()
        {
            InitializeComponent();
            NewRecognition newRecognition = new NewRecognition(this, mobBitmaps, ocrHeroNumber, ocrMobNumbers, questsImages, sharpNumbersImages, cardValueImages, heroAlltImages, MyAlltImages, moneyImages, okImages, defendImages, attachmentImages);

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
            public static readonly Point[] pointsToCheckOkMark = new Point[] {
                new Point(846, 921),
                new Point(836, 258),
                new Point(836, 917),
                new Point(1293, 848),
                new Point(836, 831),
                new Point(1291, 834)};

            public static readonly Rectangle threadRegion = new Rectangle(1620, 692, 38, 27);
            public static readonly Point guardActionButtonPoint = new Point(310, 875);

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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                OnScreenDisplay osdForm = new OnScreenDisplay(AllyOnBattlefield, cardsInHand, mobsOnBattlefield, questOnBattlefield, heroesAllyOnBattlefield, checkBox1, Windows.GameWindowRectangle().Location);

                    osdForm.Show();

            }
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

    }
}
