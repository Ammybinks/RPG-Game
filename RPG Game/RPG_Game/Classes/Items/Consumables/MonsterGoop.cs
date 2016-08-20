using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class MonsterGoop : Item
    {
        public MonsterGoop()
        {
            buyingWorth = 0;
            sellingWorth = 50;

            heldCount = 0;
            maxStack = 999;

            moveInBulk = true;

            battleUsable = false;
            mapUsable = false;

            iconFrame = new Vector2(10, 18);

            name = "Monster Goop";
            description = "A stodgy mess of unnatural bodily fluids. Doesn't do much,\nbut a shopkeep will be able to find a use for it, I'm sure";
        }
    }
}
