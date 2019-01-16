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
        public class MobInfo
        {
            public Point location;
            public string name;
            public bool active;
            public string attack;
            public string hp;
        }

        public class HeroAllyInfo
        {
            public Point location;
            public string name;
            public bool active;
            public string attack;
            public string hp;
        }

        public class QuestInfo
        {
            public Point location;
            public string name;
            public string value;
        }
        public class CardInfo
        {
            public Point location;
            public string value;
        }
    }
}
