using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class AllEnemies
    {
        public List<SpriteBase> Call(BattleState battleState)
        {
            List<SpriteBase> temp = new List<SpriteBase>();

            for(int i = 0; i < battleState.enemies.Count; i++)
            {
                temp.Add(battleState.enemies[i]);
            }

            return temp;
        }
    }
}
