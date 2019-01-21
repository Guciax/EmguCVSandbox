using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EmguCVSandbox.Form1;
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox
{
    public class Bot
    {
        public Bot(RichTextBox logbook)
        {
            this.logbook = logbook;

            //FileInfo[] attachmentFiles = attachmentDir.GetFiles();
            //foreach (var attachmentF in attachmentFiles)
            //{
            //    Bitmap newBmp = new Bitmap(attachmentF.FullName);
            //    newBmp.Tag = attachmentF.Name.Split('.')[0];
            //    attachmentImages.Add(newBmp);
            //}

        }

        DirectoryInfo okDir = new DirectoryInfo(@"Images\Ok");
        DirectoryInfo defendDir = new DirectoryInfo(@"Images\Defend");
        DirectoryInfo attachmentDir = new DirectoryInfo(@"Images\Attachment");
       

        List<Bitmap> okImages = new List<Bitmap>();
        List<Bitmap> defendImages = new List<Bitmap>();
        List<Bitmap> attachmentImages = new List<Bitmap>();

        List<OkInfo> okOnScreen = new List<OkInfo>();
        List<DefendInfo> defendOnScreen = new List<DefendInfo>();
        List<AttachmentInfo> attachmentOnScreen = new List<AttachmentInfo>();

        bool eowynranger = false;
        bool eowynraven = false;
        
        bool gimliranger = false;
        bool gimliraven = false;

        bool dwalinranger = false;
        bool dwalinraven = false;

        

    Mouse mysz = new Mouse();
        private readonly RichTextBox logbook;
        

        public void playcardfromhand(GameCurrentState gamecurrentstate)
        {
     
                       
            tologbook("kart o koszcie 1: " + gamecurrentstate.cardsInHand1 + " kart o koszcie 2: " + gamecurrentstate.cardsInHand2);
            // Jeśli kasy mamy w lub więcej i mamy kartę o koszcie 2 to ją zagrywamy
            if (gamecurrentstate.cash > 1 && gamecurrentstate.cardsInHand2 > 0)
            {
                tologbook("zagrywam karte o koszcie 2");
                mysz.MouseDragLeft(gamecurrentstate.cardsInHand2List[0].location.X + GlobalParameters.cardsRegion.X, gamecurrentstate.cardsInHand2List[0].location.Y + GlobalParameters.cardsRegion.Y, 1300, 600);
                //        tologbook("x: " + (cardsInHand[i].location.X + 410) + " Y: " + (cardsInHand[i].location.Y + 866));
                //        tologbook("x: " + (cardsInHand[i].location.X) + " Y: " + (cardsInHand[i].location.Y));
                Thread.Sleep(500);

            }
                tologbook("sprawdzam czy to nie jest attachment lub westford");
                Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
                attachmentOnScreen = NewRecognition.ScanAttachment(attachmentImages, ss);
                if (attachmentOnScreen.Count() > 0)
                        {
                            tologbook("to jest attachment " + attachmentOnScreen[0].name);
                        if (attachmentOnScreen[0].name=="westford")
                {
                    tologbook("Westford jest to");
                    tologbook("klikam po mobach");
                    for (int i = 0; i < gamecurrentstate.mobs.Count(); i++)
                    {
                        mysz.MouseLeftClick(gamecurrentstate.mobs[i].location.X + GlobalParameters.mobsRegion.X, gamecurrentstate.mobs[i].location.Y + GlobalParameters.mobsRegion.Y);
                    }
                }
                        else
                {
                    //sprawdzam, czy eowyn nie ma attachmentu
                    bool oddany = false;

                    

                        if (gamecurrentstate.eowyn.Count == 1)
                        {
                        
                        }
                        if (gamecurrentstate.gimli.Count == 1 && oddany==false)
                        {

                        }
                        if (gamecurrentstate.dwalin.Count == 1 && oddany == false)
                        {
                        }




                }
                            

                        //    mysz.MouseLeftClick(heroesAllyOnBattlefield[0].location.X + GlobalParameters.heroRegion.X, heroesAllyOnBattlefield[0].location.Y + GlobalParameters.heroRegion.Y);
                            Thread.Sleep(500);
                        }




            //            Thread.Sleep(500);

            //            break;

            //        }

            //    }
            //}
            //else if (money > 1 && cards1 > 0)
            //{
            //    tologbook("zagrywam karte o koszcie 1");
            //    for (int i = 0; i < cardsInHand.Count; i++)
            //    {
            //        if (cardsInHand[i].value == "1")
            //        {
            //            mysz.MouseDragLeft(cardsInHand[i].location.X + GlobalParameters.cardsRegion.X, cardsInHand[i].location.Y + GlobalParameters.cardsRegion.Y, 1300, 600);

            //            Thread.Sleep(500);
            //            break;

            //        }

            //    }
            //}
            //else if (money == 1 && cards1 > 0)
            //{
            //    tologbook("zagrywam karte o koszcie 1");
            //    for (int i = 0; i < cardsInHand.Count; i++)
            //    {
            //        if (cardsInHand[i].value == "1")
            //        {
            //            mysz.MouseDragLeft(cardsInHand[i].location.X + GlobalParameters.cardsRegion.X, cardsInHand[i].location.Y + GlobalParameters.cardsRegion.Y, 1300, 600);

            //            Thread.Sleep(500);
            //            break;

            //        }

            //    }
            //}
            //else
            //{
            //    tologbook("No money to play cards or no cards to play");
            //}

        }
            public void startquest()
        {
            //tologbook("startuje gre");
            //tologbook("ustawienie: klikam bez rozpoznawania buttonów");
            //tologbook("Thread.Sleep(500);");
            Thread.Sleep(500);
            //tologbook("klikam play");
            mysz.MouseLeftClick(260, 835);
            //tologbook("czekam 2500");
            Thread.Sleep(2500);
            //tologbook("klikam qest");
            mysz.MouseLeftClick(904, 331);
            //tologbook("czekam 700");
            Thread.Sleep(700);
            //tologbook("klikam wybor decku");
            mysz.MouseLeftClick(1280, 800);
            //tologbook("czekam 700");
            Thread.Sleep(700);
            //tologbook("klikam deck drugi z gory");
            mysz.MouseLeftClick(1320, 460);
            //tologbook("czekam 700");
            Thread.Sleep(700);
            //tologbook("wybieram poziom trudnosci");
            mysz.MouseLeftClick(1550, 740);
            //tologbook("czekam 700");
            Thread.Sleep(700);
            //tologbook("no i jedziemy z tematem");
            mysz.MouseLeftClick(1222, 915);
            //tologbook("tu sie moze dlugo wgrywac czekam 10 sekund");
            Thread.Sleep(5000);
            //tologbook("juz 5 zlecialo");
            Thread.Sleep(3000);
            //tologbook("juz 8 zlecialo");
            Thread.Sleep(2000);
            //tologbook("koniec");
            //tologbook("klikam continue tego biadolenia");
            mysz.MouseLeftClick(1446, 990);
            //tologbook("czekam 1000");
            Thread.Sleep(1000);
            //tologbook("klikam continue tego biadolenia 2");
            mysz.MouseLeftClick(1446, 990);
            //tologbook("czekam 6000");
            Thread.Sleep(6000);
            //tologbook("klikam confirm");
            mysz.MouseLeftClick(1370, 1010);
            //tologbook("czekam 3000");
            Thread.Sleep(3000);
            //tologbook("klikam ok");
            mysz.MouseLeftClick(840, 250);
            //tologbook("czekam 1000");
            Thread.Sleep(1000);
            //tologbook("KONIEC BUTTONA NIC WIECEJ NIE ZROBI");
        }
        public void tologbook(string output)
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

    }
}
