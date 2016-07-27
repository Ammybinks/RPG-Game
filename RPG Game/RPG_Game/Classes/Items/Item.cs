using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    public class Item : Usable
    {
        public int buyingWorth;
        public int sellingWorth;

        public int heldCount;
        public int maxStack;

        public Vector2 iconFrame;

        public string display;

        public Item()
        {
            buyingWorth = 9001;
            sellingWorth = -9001;

            heldCount = 1;
            maxStack = 1;

            iconFrame = new Vector2(1, 1);

            display = "You're not meant to use this item, dummy";
        }

        internal override List<SpriteBase> GetTargets(BattleState battleState)
        {
            List<SpriteBase> temp = new List<SpriteBase>();

            temp.Add(battleState.actor);

            return temp;
        }

        public override bool Call(GameTime gameTime, BattleState battleState)
        {
            return true;
        }

        public void Consume(BattleState battleState)
        {
            heldCount--;

            if(heldCount <= 0)
            {
                battleState.heldItems.Remove(this);
            }
        }
    }
}
