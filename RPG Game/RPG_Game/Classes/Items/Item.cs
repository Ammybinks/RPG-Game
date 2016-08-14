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

        public string name;
        public string description;

        public bool moveInBulk;

        public bool battleUsable = false;
        public bool mapUsable = false;

        public Item()
        {
            buyingWorth = -1;
            sellingWorth = -1;

            heldCount = -1;
            maxStack = 1;

            iconFrame = new Vector2(1, 1);

            name = "You're not meant to use this item, dummy";
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
        public override bool Call(GameTime gameTime, NaviState naviState)
        {
            return true;
        }


        public void Consume(StateManager state)
        {
            if(heldCount > 0)
            {
                heldCount--;
            }
        }

        internal void Complete(NaviState naviState)
        {
            int tempButtons = naviState.inventoryBox.buttons.Count;
            int tempItems = naviState.allItems.Count;

            if (heldCount == 0)
            {
                naviState.ItemRefresh();
                naviState.currentState = 3;

                naviState.currentAction = null;
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
            naviState.ItemRefresh();
            naviState.TargetRefresh();

            runOnce = true;
        }
    }
}
