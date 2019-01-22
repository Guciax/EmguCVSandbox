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
        public Bot(RichTextBox logbook,
                   List<Bitmap> okImages,
                   List<Bitmap> defendImages,
                   List<Bitmap> attachmentImages
           
        

            )
        {
            this.logbook = logbook;
            this.okImages = okImages;
            this.defendImages = defendImages;
            this.attachmentImages = attachmentImages;


        }

        private readonly List<Bitmap> okImages;
        private readonly List<Bitmap> defendImages;
        private readonly List<Bitmap> attachmentImages;
        List<AttachmentInfo> attachmentOnScreen = new List<AttachmentInfo>();
        List<OkInfo> okOnScreen = new List<OkInfo>();




        bool eowynranger = false;
        bool eowynraven = false;

        bool gimliranger = false;
        bool gimliraven = false;

        bool dwalinranger = false;
        bool dwalinraven = false;



        Mouse mysz = new Mouse();
        private readonly RichTextBox logbook;

        public void checkOkButtonAndClick()
        {

            Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
            okOnScreen = NewRecognition.ScanOk(okImages, ss);

            tologbook("widze ok w liczbie:" + okOnScreen.Count);
            for (int i = 0; i < okOnScreen.Count; i++)
            {
                tologbook("Ok on screen " + i + ": " + okOnScreen[i].value + " X=" + okOnScreen[i].location.X + " Y=" + okOnScreen[i].location.Y);
                tologbook("klikam ok");
                mysz.MouseLeftClick(okOnScreen[0].location.X, okOnScreen[0].location.Y);
                Thread.Sleep(500);
            }
        }


        public bool checkEndButton()
        {
 
            Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
            Point endPoint = new Point(1292, 1011);
            Color endPointColor = Color.FromArgb(255, 255, 255, 195);
            bool czyend = ImageFilters.IsThisPixelPixel(ss, endPoint, endPointColor);
            //
            Point defPoint = new Point(309, 878);
            Color defPointColor = Color.FromArgb(255, 231, 224, 178);
            bool czydef = ImageFilters.IsThisPixelPixel(ss, defPoint, defPointColor);
            //kolor endphase
            //1290x1010
            Point travelPoint1 = new Point(806, 219);
            Point travelPoint2 = new Point(885, 212);

            Color travelPointColor = Color.FromArgb(255, 255, 255, 255);
            bool czytravel1 = ImageFilters.IsThisPixelPixel(ss, travelPoint1, travelPointColor);
            bool czytravel2 = ImageFilters.IsThisPixelPixel(ss, travelPoint2, travelPointColor);
            bool czytravel = false;
            if (czytravel1 == true && czytravel2 == true)
            {
                czytravel = true;
            }

            tologbook("czy def ?:" + czydef);
            tologbook("czy end ?:" + czyend);
            tologbook("czy travel ?:" + czytravel);
            return czyend;
        }

        public bool defendIfAble(GameCurrentState gamecurrentstate)
        {
            bool obronione = false;

            tologbook("Aktywnych potworów jest " + gamecurrentstate.activeMobsNumber);
            for (int i = 0; i < gamecurrentstate.activeMobsNumber; i++)
            {
                
                    tologbook("moby aktywne " + gamecurrentstate.activeMobs + ": " + gamecurrentstate.activeMobs[i].name + " X=" + gamecurrentstate.activeMobs[i].location.X + " Y=" + gamecurrentstate.activeMobs[i].location.Y);

            }

            
            tologbook("Próba obrony");

            if (gamecurrentstate.activeMobsNumber > 0 && gamecurrentstate.myAlliesActive.Count>0)
            {
                
                for (int i = 0; i < gamecurrentstate.myAlliesActive.Count; i++)
                {

                      //  tologbook("bronie aktywnym " + i + ": " + gamecurrentstate.myAlliesActive[i].name + " X=" + gamecurrentstate.myAlliesActive[i].location.X + " Y=" + gamecurrentstate.myAlliesActive[i].location.Y);
                        obronione = true;


                        tologbook("klikam goscia");
                    //    tologbook(" X=" + gamecurrentstate.myAlliesActive[i].location.X + GlobalParameters.heroRegion.X + " Y=" + gamecurrentstate.myAlliesActive[i].location.Y + GlobalParameters.heroRegion.Y);
                        //260, 590
                        mysz.MouseLeftClick(gamecurrentstate.myAlliesActive[i].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.myAlliesActive[i].location.Y + GlobalParameters.heroRegion.Y);
                        Thread.Sleep(500);

                        tologbook("szukam obrony");
                        Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
                        Point defPoint = new Point(309, 878);
                        Color defPointColor = Color.FromArgb(255, 231, 224, 178);
                        bool czydef = ImageFilters.IsThisPixelPixel(ss, defPoint, defPointColor);
                        
                        if (czydef== true)
                        {
                            tologbook("klikam obrone");
                            tologbook(" X=" + 309 + " Y=" + 878);
                            mysz.MouseLeftClick(309, 878);
                        //ustawiam mysz ponizej
                        mysz.MouseLeftClick(309, 910);

                    }
                        else
                        {
                            tologbook("nieznalazlem obrony - we are fucked");
                        }
                        break;
                    }
            
                if (obronione == false)
                {
                    tologbook("nie mam przyjaciół do obrony przed aktywnymi mobami");
                }
            }
            else
            {
                tologbook("nie ma aktywnych enemy, nie bronie");
            }
            return obronione;
        }


            public bool playcardfromhand(GameCurrentState gamecurrentstate)
        {
            bool kartazagrana = false;

            tologbook("kart o koszcie 1: " + gamecurrentstate.cardsInHand1 + " kart o koszcie 2: " + gamecurrentstate.cardsInHand2);
            tologbook("kasa:"+ gamecurrentstate.cash);
            // Jeśli kasy mamy w lub więcej i mamy kartę o koszcie 2 to ją zagrywamy
            if (gamecurrentstate.cash > 1 && gamecurrentstate.cardsInHand2 > 0)
            {
                kartazagrana = true;
                tologbook("zagrywam karte o koszcie 2");
                mysz.MouseDragLeft(gamecurrentstate.cardsInHand2List[0].location.X + GlobalParameters.cardsRegion.X, gamecurrentstate.cardsInHand2List[0].location.Y + GlobalParameters.cardsRegion.Y, 1300, 600);
                //        tologbook("x: " + (cardsInHand[i].location.X + 410) + " Y: " + (cardsInHand[i].location.Y + 866));
                //        tologbook("x: " + (cardsInHand[i].location.X) + " Y: " + (cardsInHand[i].location.Y));
                Thread.Sleep(500);


                tologbook("sprawdzam czy to nie jest attachment lub westford");
                Bitmap ss = ScreenShot.GetScreenShot(Windows.GameWindowRectangle());
                attachmentOnScreen = NewRecognition.ScanAttachment(attachmentImages, ss);
                if (attachmentOnScreen.Count() > 0)
                {
                    tologbook("to jest attachment " + attachmentOnScreen[0].name);
                    if (attachmentOnScreen[0].name == "westford")
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

                        switch (attachmentOnScreen[0].name)
                        {
                            case "rangerspear":
                                if (gamecurrentstate.eowyn.Count == 1 && eowynranger == false)
                                {
                                    mysz.MouseLeftClick(gamecurrentstate.eowyn[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.eowyn[0].location.Y + GlobalParameters.heroRegion.Y);
                                    eowynranger = true;
                                }
                                else if (gamecurrentstate.gimli.Count == 1 && gimliranger == false)
                                {
                                    mysz.MouseLeftClick(gamecurrentstate.gimli[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.gimli[0].location.Y + GlobalParameters.heroRegion.Y);
                                    gimliranger = true;
                                }
                                else if (gamecurrentstate.dwalin.Count == 1 && dwalinranger == false)
                                {
                                    mysz.MouseLeftClick(gamecurrentstate.dwalin[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.dwalin[0].location.Y + GlobalParameters.heroRegion.Y);
                                    dwalinranger = true;
                                }
                                break;

                            case "raven":

                                if (gamecurrentstate.gimli.Count == 1 && gimliraven == false)
                                {
                                    mysz.MouseLeftClick(gamecurrentstate.gimli[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.gimli[0].location.Y + GlobalParameters.heroRegion.Y);
                                    gimliraven = true;
                                }
                                else if (gamecurrentstate.dwalin.Count == 1 && dwalinraven == false)
                                {
                                    mysz.MouseLeftClick(gamecurrentstate.dwalin[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.dwalin[0].location.Y + GlobalParameters.heroRegion.Y);
                                    dwalinraven = true;
                                }
                                else if (gamecurrentstate.eowyn.Count == 1 && eowynraven == false)
                                {
                                    mysz.MouseLeftClick(gamecurrentstate.eowyn[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.eowyn[0].location.Y + GlobalParameters.heroRegion.Y);
                                    eowynraven = true;
                                }

                                break;

                            case "sting":
                                if (gamecurrentstate.dwalin.Count == 1 && dwalinranger == false)
                                {
                                    mysz.MouseLeftClick(gamecurrentstate.dwalin[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.dwalin[0].location.Y + GlobalParameters.heroRegion.Y);
                                    dwalinranger = true;
                                }
                                else if (gamecurrentstate.gimli.Count == 1 && gimliranger == false)
                                {
                                    mysz.MouseLeftClick(gamecurrentstate.gimli[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.gimli[0].location.Y + GlobalParameters.heroRegion.Y);
                                    gimliranger = true;
                                }
                                else if (gamecurrentstate.eowyn.Count == 1 && eowynranger == false)
                                {
                                    mysz.MouseLeftClick(gamecurrentstate.eowyn[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.eowyn[0].location.Y + GlobalParameters.heroRegion.Y);
                                    eowynranger = true;
                                }

                                break;
                            default:
                                mysz.MouseLeftClick(gamecurrentstate.gimli[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.gimli[0].location.Y + GlobalParameters.heroRegion.Y);
                                gimliranger = true;

                                break;
                        }

                    }


                    //    mysz.MouseLeftClick(heroesAllyOnBattlefield[0].location.X + GlobalParameters.heroRegion.X, heroesAllyOnBattlefield[0].location.Y + GlobalParameters.heroRegion.Y);
                }
                
            }


            else if (gamecurrentstate.cash >= 1 && gamecurrentstate.cardsInHand1 > 0)
            {
                kartazagrana = true;
                tologbook("zagrywam karte o koszcie 1");

                mysz.MouseDragLeft(gamecurrentstate.cardsInHand1List[0].location.X + GlobalParameters.cardsRegion.X, gamecurrentstate.cardsInHand1List[0].location.Y + GlobalParameters.cardsRegion.Y, 1300, 600);
                //        tologbook("x: " + (cardsInHand[i].location.X + 410) + " Y: " + (cardsInHand[i].location.Y + 866));
                //        tologbook("x: " + (cardsInHand[i].location.X) + " Y: " + (cardsInHand[i].location.Y));
                Thread.Sleep(500);
            }
            else
            {
                tologbook("No money to play cards or no cards to play");
                //}

            }
            return kartazagrana;
        }
        public void attackMob(GameCurrentState gamecurrentstate)
        {
            //jesli sa aktywne moby atakuj aktywnego moba
            bool atakally = false;
            if (gamecurrentstate.activeAlliesNumber > 0)
            {
                atakally = true;
            }
            bool atakhero = false;
            if (gamecurrentstate.activeHeroesNumber > 0)
            {
                atakhero = true;
            }

            tologbook("atakuje ally:" + atakally + "atakuje hero:" + atakhero);
            tologbook("aktywne moby:" + gamecurrentstate.activeMobsNumber);
            tologbook("moby total:" + gamecurrentstate.mobs.Count);
            // Jeśli kasy mamy w lub więcej i mamy kartę o koszcie 2 to ją zagrywamy
            if (gamecurrentstate.activeMobsNumber > 0)
            {

                if (gamecurrentstate.activeMobspriority1.Count > 0)
                {
                    if (atakally == true)
                    {
                        mysz.MouseDragLeft(gamecurrentstate.myAlliesActive[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.myAlliesActive[0].location.Y + GlobalParameters.heroRegion.Y, gamecurrentstate.activeMobspriority1[0].location.X + GlobalParameters.mobsRegion.X, gamecurrentstate.activeMobspriority1[0].location.Y + GlobalParameters.mobsRegion.Y);
                    }
                    else if (atakhero == true)
                    {
                        mysz.MouseDragLeft(gamecurrentstate.activeHeroesList[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.activeHeroesList[0].location.Y + GlobalParameters.heroRegion.Y, gamecurrentstate.activeMobspriority1[0].location.X + GlobalParameters.mobsRegion.X, gamecurrentstate.activeMobspriority1[0].location.Y + GlobalParameters.mobsRegion.Y);
                    }
                    tologbook("atakuje aktywnego moba z priorytetem 1");
                }
                else if (gamecurrentstate.activeMobspriority2.Count > 0)
                {
                    if (atakally == true)
                    {
                        mysz.MouseDragLeft(gamecurrentstate.myAlliesActive[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.myAlliesActive[0].location.Y + GlobalParameters.heroRegion.Y, gamecurrentstate.activeMobspriority2[0].location.X + GlobalParameters.mobsRegion.X, gamecurrentstate.activeMobspriority2[0].location.Y + GlobalParameters.mobsRegion.Y);
                    }
                    else if (atakhero == true)
                    {
                        mysz.MouseDragLeft(gamecurrentstate.activeHeroesList[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.activeHeroesList[0].location.Y + GlobalParameters.heroRegion.Y, gamecurrentstate.activeMobspriority2[0].location.X + GlobalParameters.mobsRegion.X, gamecurrentstate.activeMobspriority2[0].location.Y + GlobalParameters.mobsRegion.Y);
                    }
                    tologbook("atakuje aktywnego moba z priorytetem 2");
                }
                else if (gamecurrentstate.activeMobsNumber > 0)
                {
                    if (atakally == true)
                    {
                        mysz.MouseDragLeft(gamecurrentstate.myAlliesActive[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.myAlliesActive[0].location.Y + GlobalParameters.heroRegion.Y, gamecurrentstate.activeMobs[0].location.X + GlobalParameters.mobsRegion.X, gamecurrentstate.activeMobs[0].location.Y + GlobalParameters.mobsRegion.Y);
                    }
                    else if (atakhero == true)
                    {
                        mysz.MouseDragLeft(gamecurrentstate.activeHeroesList[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.activeHeroesList[0].location.Y + GlobalParameters.heroRegion.Y, gamecurrentstate.activeMobs[0].location.X + GlobalParameters.mobsRegion.X, gamecurrentstate.activeMobs[0].location.Y + GlobalParameters.mobsRegion.Y);
                    }
                    tologbook("atakuje aktywnego moba bez priorytetu");

                }


            }
            else if (gamecurrentstate.mobs.Count > 0)
            {
                if (atakally == true)
                {
                    mysz.MouseDragLeft(gamecurrentstate.myAlliesActive[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.myAlliesActive[0].location.Y + GlobalParameters.heroRegion.Y, gamecurrentstate.mobsMaxatt[0].location.X + GlobalParameters.mobsRegion.X, gamecurrentstate.mobsMaxatt[0].location.Y + GlobalParameters.mobsRegion.Y);
                }
                else if (atakhero == true)
                {
                    mysz.MouseDragLeft(gamecurrentstate.activeHeroesList[0].location.X + GlobalParameters.heroRegion.X, gamecurrentstate.activeHeroesList[0].location.Y + GlobalParameters.heroRegion.Y, gamecurrentstate.mobsMaxatt[0].location.X + GlobalParameters.mobsRegion.X, gamecurrentstate.mobsMaxatt[0].location.Y + GlobalParameters.mobsRegion.Y);
                }
                tologbook("atakuje moba o najwyzzszym att");
                //   Thread.Sleep(500);
            }
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
