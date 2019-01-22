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
        public class CardsAndCash
        {
            public List<CardInfo> cardsInHand;
            public int cash;
            public int sauronCash;
            public int cardsInHandCount
            {
                get
                {
                    return cardsInHand.Count();
                }
            }
            public int cards1InHandCount
            {
                get
                {
                    return cardsInHand.Where(c => c.value == "1").Count();
                }
            }
            public int cards2InHandCount
            {
                get
                {
                    return cardsInHand.Where(c => c.value == "2").Count();
                }
            }
            public List<CardInfo> cardsInHand1List
            {
                get
                {
                    return cardsInHand.Where(c => c.value == "1").ToList();
                }
            }

            public List<CardInfo> cardsInHand2List
            {
                get
                {
                    return cardsInHand.Where(c => c.value == "2").ToList();
                }
            }
        }

        public class MobsAndQuests
        {
            public List<MobInfo> mobs;
            public List<QuestInfo> quests;
            public int activeMobsCount
            {
                get
                {
                    return mobs.Select(m => m.active).Count();
                }
            }
        }

        public class HeroesAndAllies
        {
            public List<HeroAllyInfo> heroesAlly;
            public List<HeroAllyInfo> alliesList
            {
                get
                {
                    return heroesAlly.Where(a => a.name == "Ally").ToList();
                }
            }
            public int activeAlliesCount
            {
                get
                {
                    return heroesAlly.Select(a => a.name == "Ally" & a.active).Count();
                }
            }
            public int activeHeroesCount
            {
                get
                {
                    return heroesAlly.Select(a => a.name != "Ally" & a.active).Count();
                }
            }

            public List<HeroAllyInfo> activeHeroesList
            {
                get
                {
                    return heroesAlly.Where(a => a.active).ToList();
                }
            }
        }

        public class GameCurrentState
        {
            public CardsAndCash cardsAndCash;
            public MobsAndQuests mobsAndQuests;
            public HeroesAndAllies heroesAndAllies;

            public int threadLevel;
            public int currentPhase;
        }

        

        public class MobInfo
        {
            List<string> mobsWithGuard = new List<string> { "mob1", "mob2" };
            List<string> mobsWithStalward = new List<string> { "mob1", "mob2" };
            List<string> mobsWithBlock = new List<string> { "mob1", "mob2" };
            Dictionary<string, int> mobDangerDict = new Dictionary<string, int>
            {
                { "mob1",0 },{"mob2",1}
            };

            public Point location;
            public string name;
            public bool active;
            public int attack;
            public int hp;
            public Bitmap mobImage;
            public List<string> matchResults;
            public int attackPriority
            {
                get
                {
                    return mobDangerDict[name];
                }
            }
               

            public bool hasGuard
            {
                get
                {
                    return mobsWithGuard.Contains(name);
                }
            }
            public bool hasStalward
            {
                get
                {
                    return mobsWithStalward.Contains(name);
                }
            }
            public bool hasBlock
            {
                get
                {
                    return mobsWithBlock.Contains(name);
                }
            }
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
            public bool hasEquipment = false;
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
