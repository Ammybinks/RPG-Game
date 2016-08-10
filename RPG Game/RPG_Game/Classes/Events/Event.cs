using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    [Serializable()]
    public class Event
    {
        internal TypingStrings typingStrings;
        
        [NonSerialized()]public Box eventBox;
        
        public virtual bool Call(GameTime gameTime, NaviState naviState)
        {
            return true;
        }

        public virtual void DrawAll(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (eventBox != null)
            {
                eventBox.DrawParts(spriteBatch);

                spriteBatch.DrawString(spriteFont,
                                       typingStrings.line,
                                       new Vector2(580, 900),
                                       Color.Black);
            }
        }

        internal void Complete(NaviState naviState)
        {
            naviState.ActivateState(0);

            naviState.currentEvent = null;

            typingStrings = null;
        }

        internal void Unload(NaviState naviState)
        {
        }
    }
}
