using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    public class Ability : Usable
    {
        public Vector2 iconFrame;

        public int cost;

        public bool battleUsable = false;
        public bool mapUsable = false;

        public string name;
        public string description;

        public Ability()
        {
            cost = 0;
            name = "This ability isn't meant to be used, dummy";
        }

        internal override List<SpriteBase> GetTargets(BattleState battleState)
        {
            List<SpriteBase> temp = new List<SpriteBase>();

            temp.Add(battleState.actor);

            return temp;
        }

        //If this is called, the user will instantly die.
        public override bool Call(GameTime gameTime, BattleState battleState)
        {
            battleState.target.health -= 9001;
            return true;
        }
    }
}
