using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox.BT
{
    class BT_Nodes
    {
        public static bool CanOneHitKill(GameCurrentState gameState)
        {
            return gameState.mobs.Select(mob => mob.hp).Min() <= gameState.heroesAlly.Where(h => h.active).Select(h => h.attack).Max();
        }

        public static bool IsHeroLifeInDanger(GameCurrentState gameState)
        {
            return gameState.mobs.Select(mob => mob.attack).Max() >= gameState.heroesAlly.Select(h => h.hp).Min();
        }

        public static bool IsThereHeroWhoCanDefend(GameCurrentState gameState)
        {
            return gameState.mobs.Select(mob => mob.attack).Max() < gameState.heroesAlly.Where(h => h.active).Select(h => h.hp).Max();
        }

        public static bool CanAllyBePlayed(GameCurrentState gameState)
        {
            if (gameState.heroesAlly.Count > 8) return false;
            if (gameState.cardsInHand.Count == 0) return false;
            //if (gameState.cardsInHand.Select(c => c.value).Min() > gameState.cash) return false;
            return true;
        }

        public static bool IsThreatLevelSafe(GameCurrentState gameState)
        {
            if (gameState.currentPhase == 1 & gameState.threadLevel > 30) return false;
            if (gameState.currentPhase == 2 & gameState.threadLevel > 35) return false;
            if (gameState.currentPhase == 3 & gameState.threadLevel > 38) return false;
            return true;

        }
    }
}
