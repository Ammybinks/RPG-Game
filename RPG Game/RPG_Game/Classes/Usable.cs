using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    public class Usable
    {
        internal virtual List<SpriteBase> GetTargets(BattleState battleState)
        {
            return new List<SpriteBase>();
        }

        public virtual bool Call(GameTime gameTime, BattleState battleState)
        {
            return true;
        }
    }
}
