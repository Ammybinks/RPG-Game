using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace RPG_Game
{
    class BattleMover : EventMover
    {
        internal List<PotentialEnemy> potentialEnemies = new List<PotentialEnemy>();

        internal List<Troop> potentialTroops = new List<Troop>();

        public virtual void InitializePotentials(NaviState naviState, Main main)
        {
        }

        public virtual void InitializeTroops(NaviState naviState, Main main)
        {
        }
    }
}
