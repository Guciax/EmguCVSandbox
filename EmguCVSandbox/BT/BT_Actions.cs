using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox.BT
{
    class BT_Actions
    {
        public static void KillEnemy(GameCurrentState gameState)
        {
            foreach (var mob in gameState.mobs.OrderByDescending(mob=>mob.hp))
            {
                foreach (var heroAlly in gameState.heroesAlly)
                {

                }
            }
        }

    }
}
