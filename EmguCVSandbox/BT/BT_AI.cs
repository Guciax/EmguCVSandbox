using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmguCVSandbox.ObjectsStructure;
using static EmguCVSandbox.Form1;

namespace EmguCVSandbox.BT
{
    public class BT_AI
    {
        public struct TargetKiller { public MobInfo target; public HeroAllyInfo killer; }

        public static TargetKiller ChooseTargetAndKiller(GameCurrentState gameState)
        {
            TargetKiller result = new TargetKiller();
            if (gameState.mobsAndQuests.activeMobsCount == 1) //1 activemob
            {
                var target = gameState.mobsAndQuests.mobs.Where(m => m.active).First();
                var killer = ChooseBestKillerForMob(target, gameState.heroesAndAllies.activeHeroesList);

                result.target = target;
                result.killer = killer;
                return result;
            }
            else if (gameState.mobsAndQuests.activeMobsCount > 1) //more than 1
            {
                foreach (var mob in gameState.mobsAndQuests.mobs.Where(m => m.active).OrderByDescending(mDang => mDang.attackPriority)) 
                {
                    var bestKiller = ChooseBestKillerForMob(mob, gameState.heroesAndAllies.activeHeroesList);
                    if (bestKiller != null) //If noone can kill him choose best dmg...
                    {
                        result.killer = bestKiller;
                        result.target = mob;
                        return result;
                    }
                }

                //If noone can kill, deal some dmg...
                return ChooseBestDmgTarget(gameState.mobsAndQuests.mobs.Where(m => m.active).OrderByDescending(mDang => mDang.attackPriority), gameState.heroesAndAllies.activeHeroesList); 
            }
            else //no active mob
            {
                return ChooseBestDmgTarget(gameState.mobsAndQuests.mobs.OrderByDescending(mDang => mDang.attackPriority), gameState.heroesAndAllies.activeHeroesList);
            }
        }

        public static HeroAllyInfo ChooseBestKillerForMob(MobInfo target, List<HeroAllyInfo> heroList)
        {
            var heroWhoCanKill = heroList.Where(h => h.attack >= target.hp).OrderBy(h => h.attack).ToList();
            if (heroWhoCanKill.Count > 0)
            {
                return heroWhoCanKill[0];
            }
            else
            {
                return null;
            }
        }

        public static TargetKiller ChooseBestDmgTarget(IEnumerable<MobInfo> targets, IEnumerable<HeroAllyInfo> activeHeroList)
        {
            TargetKiller result = new TargetKiller();
            foreach (var mob in targets)
            {
                if (activeHeroList.Select(h => h.attack).Sum() >= mob.hp) 
                {
                    result.target = mob;
                    result.killer = activeHeroList.OrderByDescending(h => h.attack).First();
                    return result;
                }
            }

            result.target = targets.OrderByDescending(m => m.attackPriority).First();
            result.killer = activeHeroList.OrderBy(h => h.attack).First();
            return result;
        }
    }
}
