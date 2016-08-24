using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Game
{
    public class PotentialDrops
    {
        public Item item;

        public int proportion;

        public List<PotentialCount> potentialCounts = new List<PotentialCount>();
    }

    public class PotentialCount
    {
        public int count;

        public int proportion;
    }
}
