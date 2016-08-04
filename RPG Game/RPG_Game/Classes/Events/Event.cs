using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    [Serializable()]
    public class Event
    {
        public bool complete;

        internal TypingStrings typingStrings;
        
        [NonSerialized()]public Box eventBox;
        
        internal bool runOnce;

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
        
        internal void Unload(NaviState naviState)
        {
            naviState.currentEvent = null;

            typingStrings = null;

            complete = false;
        }
    }
}
