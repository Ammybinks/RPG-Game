using System.Collections.Generic;

namespace RPG_Game
{
    public class BattleEvent : Event
    {
        internal List<PotentialEnemy> potentialEnemies = new List<PotentialEnemy>();

        public virtual void InitializePotentials(NaviState naviState, Main main)
        {
        }
    }
}
