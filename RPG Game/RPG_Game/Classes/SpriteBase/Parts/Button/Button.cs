using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    public class Button : Parts
    {
        public Sprite icon = new Sprite();

        public string display;
        public float displaySize = 1f;
        public Color displayColour = Color.Black;

        public bool showOnSelected;
        public bool selectable = true;

        public Action action;

        public virtual void DrawParts(SpriteBatch spriteBatch, SpriteFont font, bool isSelected)
        {
            if (showOnSelected)
            {
                if (!isSelected)
                {
                    return;
                }
            }

            if (isAlive)
            {
                for (int i = 0; i < parts.Count; i++)
                {
                    parts[i].Draw(spriteBatch);
                }
            }

            if (icon != null)
            {
                icon.Draw(spriteBatch);
            }

            DrawDisplay(spriteBatch, font);
        }

        internal void DrawDisplay(SpriteBatch spriteBatch, SpriteFont font)
        {
            if (display != null)
            {
                if (icon != null)
                {
                    spriteBatch.DrawString(font,
                                           display,
                                           new Vector2(UpperLeft.X + icon.GetWidth() + 30, UpperLeft.Y + 8),
                                           displayColour,
                                           0,
                                           new Vector2(0, 0),
                                           displaySize,
                                           SpriteEffects.None, 0);
                }
                else
                {
                    spriteBatch.DrawString(font,
                                           display,
                                           new Vector2(UpperLeft.X + 10, UpperLeft.Y + 8),
                                           displayColour,
                                           0,
                                           new Vector2(0, 0),
                                           displaySize,
                                           SpriteEffects.None, 0);
                }
            }
        }
    }
}