using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmguCVSandbox
{
    public class ObjectsStructure
    {
        public class GameCurrentState
        {
            public List<MobInfo> mobs;
            public List<HeroAllyInfo> heroesAlly;
            public List<QuestInfo> quests;
            public List<CardInfo> cardsInHand;
            public int currentPhase;
            public int cash;
            public int sauronCash;
        }

        public class MobInfo
        {
            public Point location;
            public string name;
            public bool active;
            public int attack;
            public int hp;
            public Bitmap mobImage;
            public List<string> matchResults;
        }

        public class HeroAllyInfo
        {
            public Point location;
            public string name;
            public bool active;
            public int attack;
            public int hp;
            public int lore;
            public Bitmap heroImage;
            public List<string> matchResults;
            public Bitmap attackImg;
            public Bitmap defImg;
            bool hasEquipment = false;
        }

        public class MyAllyInfo
        {
            public Point location;
            public string name;
            public bool active;
            public int attack;
            public int hp;
            public int lore;
            public Bitmap heroImage;
            public List<string> matchResults;
            public Bitmap attackImg;
            public Bitmap defImg;
            bool hasEquipment = false;
        }


        public class QuestInfo
        {
            public Point location;
            public string name;
            public int value;
            public Bitmap questImage;
            public List<string> matchResults;
        }
        public class CardInfo
        {
            public Point location;
            public string value;
        }
        public class OkInfo
        {
            public Point location;
            public string value;
        }

        public class DefendInfo
        {
            public Point location;
            public string value;
        }

        public class AttachmentInfo
        {
            public Point location;
            public string name;
        }


    }
}
