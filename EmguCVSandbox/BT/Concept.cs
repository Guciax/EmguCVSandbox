using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmguCVSandbox.ObjectsStructure;

namespace EmguCVSandbox.BT
{
    public interface Node
    {
        void Do();
    }

    public class Empty : Node
    {
        public void Do()
        {
            //Empty finishes tree.
        }
    }

    public class Act : Node
    {
        public void Do()
        {
            //to  trzeba poprawic.
            act(GuccisDomain.s);
            child.Do();
        }

        public Action<GameCurrentState> act;
        public Node child;

        public Act(Action<GameCurrentState> act, Node child)
        {
            this.act = act;
            this.child = child;
        }
        public Act(Action<GameCurrentState> act)
        {
            this.act = act;
            this.child = new Empty();
        }
    }

    class Condition : Node
    {
        public Condition(Func<GameCurrentState, bool> cond, Node onSuccess, Node onFailure)
        {
            Cond = cond;
            OnSuccess = onSuccess;
            OnFailure = onFailure;
        }



        public Func<GameCurrentState, bool> Cond { get; }
        public Node OnSuccess { get; }
        public Node OnFailure { get; }

        public void Do()
        {
            if(Cond(GuccisDomain.s) )
            {
                OnSuccess.Do();
            } else
            {
                OnFailure.Do();
            }
        }
    }

    class GuccisDomain
    {

        public static void PlayAlly(GameCurrentState s)
        {
            //...
        }

        public static void Guard(GameCurrentState s)
        {
            //...
        }

        public static void Attax(GameCurrentState s)
        {
            //...
        }

        public static void ChoosePriorityMany(GameCurrentState s)
        {
            //... choose priority many?
        }
        //narazie static workaround
        public static GameCurrentState s;

        void Stuff()
        {
            Func<GameCurrentState, bool> f = BT_Nodes.CanAllyBePlayed;
            var czy_moge_zagrac_ally = new Condition(BT_Nodes.CanAllyBePlayed,
            new Act(PlayAlly), new Act(ChoosePriorityMany));

            var jest_hero_ktory_wytrzyma_atak = new Condition(
                BT_Nodes.IsThereHeroWhoCanDefend,
            new Act(Guard), new Act(Attax));


        var is_hero_in_danger = new Condition(BT_Nodes.IsHeroLifeInDanger,
            jest_hero_ktory_wytrzyma_atak, czy_moge_zagrac_ally);


            is_hero_in_danger.Do();

            //var have_active_allies = new Condition(BT_Nodes.,
            //jest_hero_ktory_wytrzyma_atak, czy_moge_zagrac_ally);
        }
    }
}
