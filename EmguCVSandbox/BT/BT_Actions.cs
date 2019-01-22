using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static EmguCVSandbox.Form1;
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox.BT
{
    class BT_Actions
    {
       
        public static void AttackMobOrQuest(MobInfo target, HeroAllyInfo killer)
        {
            Mouse mouse = new Mouse();
            mouse.MouseDragLeft(killer.location, target.location);
        }

        public static void PlayCardFromHand(CardInfo card)
        {
            Mouse mo = new Mouse();
            mo.MouseDragLeft(card.location, new System.Drawing.Point(GlobalParameters.heroRegion.X + GlobalParameters.heroRegion.Width / 2, GlobalParameters.heroRegion.Y + GlobalParameters.heroRegion.Height / 2));
        }

        public static void GuardCharacter(HeroAllyInfo character)
        {
            Mouse mo = new Mouse();
            mo.MouseLeftClick(character.location);
            Thread.Sleep(50);
            mo.MouseLeftClick(GlobalParameters.guardActionButtonPoint);
        }

    }
}
