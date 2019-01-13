using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmguCVSandbox.Form1;

namespace EmguCVSandbox
{
    class ResultVisualization
    {
        public static Bitmap ShowMobsOnBattlefield(List<MobInfo> mobs, Bitmap img)
        {
            using (Graphics g = Graphics.FromImage(img))
            {
                using (Brush b = new SolidBrush(Color.LightYellow))
                {
                    
                    foreach (var mob in mobs)
                    {
                        string active = mob.active ? "active" : "exaused";
                        g.FillEllipse(b, mob.location.X - 3, mob.location.Y - 3, 6, 6);
                        g.DrawString(mob.name, new Font("Arial", 14, FontStyle.Regular),b,new Point(mob.location.X-30,mob.location.Y+5));
                        g.DrawString(active, new Font("Arial", 14, FontStyle.Regular),b,new Point(mob.location.X - 30, mob.location.Y+25));
                        g.DrawString("Att:"+mob.attack, new Font("Arial", 14, FontStyle.Regular),b,new Point(mob.location.X - 30, mob.location.Y+45));
                        g.DrawString("HP:"+mob.hp, new Font("Arial", 14, FontStyle.Regular),b,new Point(mob.location.X - 30, mob.location.Y+65));
                    }
                }

            }
            return img;
        }

        public static Bitmap ShowQuestsOnBattlefield(List<QuestInfo> mobs, Bitmap img)
        {
            using (Graphics g = Graphics.FromImage(img))
            {
                using (Brush b = new SolidBrush(Color.Yellow))
                {

                    foreach (var quest in mobs)
                    {
                        g.FillEllipse(b, quest.location.X - 3, quest.location.Y - 3, 6, 6);
                        g.DrawString(quest.name, new Font("Arial", 14, FontStyle.Regular), b, new Point(quest.location.X - 30, quest.location.Y + 5));
                        g.DrawString("Value:" + quest.value, new Font("Arial", 14, FontStyle.Regular), b, new Point(quest.location.X - 30, quest.location.Y + 25));
                        
                    }
                }

            }
            return img;
        }

        public static Bitmap ShowCardsInHand(List<CardInfo> cards, Bitmap img)
        {
            using (Graphics g = Graphics.FromImage(img))
            {
                using (Brush b = new SolidBrush(Color.Yellow))
                {

                    foreach (var card in cards)
                    {
                        g.DrawEllipse(new Pen(Color.Yellow),card.location.X - 20, card.location.Y - 20, 40, 40);
                        g.DrawString(card.value, new Font("Arial", 14, FontStyle.Regular), b, new Point(card.location.X - 30, card.location.Y + 5));
                    }
                }

            }
            return img;
        }
    }
}
