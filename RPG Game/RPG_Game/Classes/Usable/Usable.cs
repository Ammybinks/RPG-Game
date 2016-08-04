using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    public class Usable
    {
        public bool runOnce = false;

        internal virtual List<SpriteBase> GetTargets(BattleState battleState)
        {
            return new List<SpriteBase>();
        }

        public virtual void DrawAll(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {

        }

        public virtual bool Call(GameTime gameTime, BattleState battleState)
        {
            return true;
        }
        public virtual bool Call(GameTime gameTime, NaviState naviState)
        {
            return true;
        }
    }
}
