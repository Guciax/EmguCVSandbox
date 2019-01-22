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
            public int threadLevel;
            public List<HeroAllyInfo> myAllies
            {
                get
                {
                    return heroesAlly.Where(a => a.name == "Ally").ToList();
                }
                
            }
            public int currentPhase;
            public int cash;
            public int sauronCash;

            public int cardsInHandQty
            {
                get
                {
                    return cardsInHand.Count();
                }
            }
            public int cardsInHand1
            {
                get
                {
                    return cardsInHand.Where(c => c.value == "1").Count();
                }
            }
            public int cardsInHand2
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
                    return cardsInHand.Where(c=>c.value == "1").ToList();
                }
            }

            public List<CardInfo> cardsInHand2List
            {
                get
                {
                    return cardsInHand.Where(c => c.value == "2").ToList();
                }
            }
            public int activeMobsNumber
            {
                get
                {
                    return mobs.Where(m => m.active).Count();
                }
            }

            public List<MobInfo> activeMobs
            {
                get
                {
                    return mobs.Where(a => a.active).ToList();
                }

            }
            public List<MobInfo> activeMobspriority1
            {
                get
                {
                    return mobs.Where(a => (a.active && a.name == "hiveguardian") || (a.active && a.name == "forestspider") || (a.active && a.name == "spiderguard")).ToList();
                }

            }
            public List<MobInfo> mobsMaxatt
            {
                get
                {
                    return mobs.Where(a => a.attack == mobs.Max(ab => ab.attack)).ToList();
                }

            }
            public List<MobInfo> activeMobspriority2
            {
                get
                {
                    return mobs.Where(a => (a.active && a.name == "bloodmother") || (a.active && a.name == "kirous")).ToList();
                }

            }

            public int alliesQty
            {
                get
                {
                    return heroesAlly.Where(a => a.name == "Ally").Count();
                }
            }

            
            public int activeAlliesNumber
            {
                get
                {
                    return heroesAlly.Where(a => (a.name == "Ally" && a.active)).Count();
                }
            }

            public List<HeroAllyInfo> myAlliesActive
            {
                get
                {
                    return heroesAlly.Where(a => (a.name == "Ally" && a.active)).ToList();
                }
            }
            public int activeHeroesNumber
            {
                get
                {
                    return heroesAlly.Where(a => (a.name != "Ally" && a.active)).Count();
                }
            }

            public List<HeroAllyInfo> activeHeroesList
            {
                get
                {
                    return heroesAlly.Where(a => a.active).ToList();
                }
            }
            public List<HeroAllyInfo> eowyn
            {
                get
                {
                    return heroesAlly.Where(a => a.name == "eowyn").ToList();
                }
            }
            public List<HeroAllyInfo> gimli
            {
                get
                {
                    return heroesAlly.Where(a => a.name == "gimli").ToList();
                }
            }
            public List<HeroAllyInfo> dwalin
            {
                get
                {
                    return heroesAlly.Where(a => a.name == "dwalin").ToList();
                }
            }
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
            public bool hasBlock = false;
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
