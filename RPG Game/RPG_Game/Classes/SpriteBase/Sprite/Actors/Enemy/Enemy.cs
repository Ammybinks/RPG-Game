﻿using System.Collections.Generic;

namespace RPG_Game
{
    public class Enemy : Battler
    {
        public int goldYield;

        public int XPYield;

        public List<PotentialDrops> potentialDrops;
    }
}