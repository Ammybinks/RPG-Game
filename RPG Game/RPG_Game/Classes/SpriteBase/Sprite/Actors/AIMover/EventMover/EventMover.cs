using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    public class EventMover : AIMover
    {
        public TypingStrings typingStrings;

        public Box box;

        public virtual void Call(GameTime gameTime, NaviState naviState)
        {
        }

        public virtual void DrawAll(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (box != null)
            {
                box.DrawParts(spriteBatch);

                spriteBatch.DrawString(spriteFont,
                                       typingStrings.line,
                                       new Vector2(580, 900),
                                       Color.Black);
            }
        }

        internal void Complete(NaviState naviState)
        {
            naviState.ActivateState(0);

            naviState.eventMover = null;

            typingStrings = null;
        }
    }
}

