using System.Collections.Generic;

namespace RPG_Game
{
    class AllAllies
    {
        public List<SpriteBase> Call(BattleState battleState)
        {
            List<SpriteBase> temp = new List<SpriteBase>();

            for (int i = 0; i < battleState.heroes.Count; i++)
            {
                temp.Add(battleState.heroes[i]);
            }

            return temp;
        }
    }
}
