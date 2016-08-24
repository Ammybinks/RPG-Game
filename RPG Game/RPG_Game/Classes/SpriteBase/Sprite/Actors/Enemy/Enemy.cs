using System.Collections.Generic;

namespace RPG_Game
{
    public class Enemy : Battler
    {
        public int goldYield;

        public int XPYield;

        public int healthDeviance = 0;
        public int manaDeviance = 0;
        public int speedDeviance = 0;
        public int accDeviance = 0;
        public int evaDeviance = 0;

        public List<PotentialDrops> potentialDrops;
    }
}
