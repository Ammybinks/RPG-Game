using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    public class Ability : Usable
    {
        public Vector2 iconFrame;

        public int cost;

        public bool battleUsable;
        public bool mapUsable;

        public string name;
        public string description;

        public Ability()
        {
            cost = -1;
            name = "This ability isn't meant to be used, dummy";

            battleUsable = false;
            mapUsable = false;
        }
        
        internal override List<SpriteBase> GetTargets(BattleState battleState)
        {
            List<SpriteBase> temp = new List<SpriteBase>();

            temp.Add(battleState.actor);

            return temp;
        }
        
        internal void Complete(NaviState naviState, GameTime gameTime)
        {
            Ability temp = (Ability)naviState.currentAction;

            if (naviState.actor.mana < temp.cost)
            {
                naviState.SkillsRefresh();
                naviState.currentAction = new SkillsMenuSwitcher();
                naviState.currentState = 4;
            }
            else
            {
                naviState.TargetRefresh();
                naviState.currentState = 6;
            }

            naviState.state[1] = false;
            naviState.previousState[1] = false;

            runOnce = false;
        }

        internal void LoadOnce(NaviState naviState)
        {
            naviState.SkillsRefresh();
            naviState.TargetRefresh();

            runOnce = true;
        }
    }
}
