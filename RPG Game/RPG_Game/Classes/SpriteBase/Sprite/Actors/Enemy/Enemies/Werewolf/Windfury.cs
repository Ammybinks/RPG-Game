using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class Windfury : Werewolf
    {
        public Windfury(Main main, NaviState naviState)
        {
            potentialDrops = new List<PotentialDrops>();
            potentialDrops.Add(new PotentialDrops());
            potentialDrops[0].item = main.heldItems[3];
            potentialDrops[0].proportion = 55;
            potentialDrops[0].potentialCounts.Add(new PotentialCount());
            potentialDrops[0].potentialCounts[0].count = 25;
            potentialDrops[0].potentialCounts[0].proportion = 75;
            potentialDrops[0].potentialCounts.Add(new PotentialCount());
            potentialDrops[0].potentialCounts[1].count = 50;
            potentialDrops[0].potentialCounts[1].proportion = 25;
            potentialDrops.Add(new PotentialDrops());
            potentialDrops[1].item = main.heldItems[0];
            potentialDrops[1].proportion = 15;
            potentialDrops[1].potentialCounts.Add(new PotentialCount());
            potentialDrops[1].potentialCounts[0].count = 99;
            potentialDrops[1].potentialCounts[0].proportion = 100;
            potentialDrops.Add(new PotentialDrops());
            potentialDrops[2].item = main.heldItems[1];
            potentialDrops[2].proportion = 15;
            potentialDrops[2].potentialCounts.Add(new PotentialCount());
            potentialDrops[2].potentialCounts[0].count = 50;
            potentialDrops[2].potentialCounts[0].proportion = 100;
            potentialDrops.Add(new PotentialDrops());
            potentialDrops[3].item = main.heldItems[2];
            potentialDrops[3].proportion = 15;
            potentialDrops[3].potentialCounts.Add(new PotentialCount());
            potentialDrops[3].potentialCounts[0].count = 25;
            potentialDrops[3].potentialCounts[0].proportion = 100;

            name = "Windfury, Failed Experiment of Pocketry";
            SetTexture(naviState.werewolfTexture);
            Scale = new Vector2(0.01f, 0.01f);
            maxHealth = 1;
            health = 1;
            PhAtk = 1;
            PhDef = 0;
            speed = 1000;
            Acc = 100;
            Eva = 0;
            friendly = false;
            goldYield = 1000;
            XPYield = 250;
        }
    }
}
