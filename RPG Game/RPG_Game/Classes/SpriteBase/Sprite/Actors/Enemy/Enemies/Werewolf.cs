using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Game
{
    class Werewolf : Enemy
    {
        public Werewolf(Main main)
        {
            potentialDrops = new List<PotentialDrops>();
            potentialDrops.Add(new PotentialDrops());
            potentialDrops[0].item = main.heldItems[3];
            potentialDrops[0].proportion = 50;
            potentialDrops[0].potentialCounts.Add(new PotentialCount());
            potentialDrops[0].potentialCounts[0].count = 1;
            potentialDrops[0].potentialCounts[0].proportion = 75;
            potentialDrops[0].potentialCounts.Add(new PotentialCount());
            potentialDrops[0].potentialCounts[1].count = 2;
            potentialDrops[0].potentialCounts[1].proportion = 25;
            potentialDrops.Add(new PotentialDrops());
            potentialDrops[1].item = main.heldItems[0];
            potentialDrops[1].proportion = 15;
            potentialDrops[1].potentialCounts.Add(new PotentialCount());
            potentialDrops[1].potentialCounts[0].count = 1;
            potentialDrops[1].potentialCounts[0].proportion = 95;
            potentialDrops[1].potentialCounts.Add(new PotentialCount());
            potentialDrops[1].potentialCounts[1].count = 5;
            potentialDrops[1].potentialCounts[1].proportion = 5;
        }
    }
}
