using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    class MultiButton : Button
    {
        public List<Button> extraButtons;

        public void SetAllParts(Texture2D cornerTexture, Texture2D wallTexture, Texture2D backTexture)
        {
            for(int i = 0; i < extraButtons.Count; i++)
            {
                extraButtons[i].SetParts(cornerTexture, wallTexture, backTexture);
            }
        }

        public override void DrawParts(SpriteBatch spriteBatch, SpriteFont font, bool isSelected)
        {
            if(showOnSelected)
            {
                if(!isSelected)
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

                if (icon != null)
                {
                    icon.Draw(spriteBatch);
                }

                if (display != null)
                {
                    if (icon != null)
                    {
                        spriteBatch.DrawString(font,
                                               display,
                                               new Vector2(UpperLeft.X + icon.GetWidth() + 30, UpperLeft.Y + 8),
                                               Color.Black,
                                               0,
                                               new Vector2(0, 0),
                                               1f,
                                               SpriteEffects.None, 0);
                    }
                    else
                    {
                        spriteBatch.DrawString(font,
                                               display,
                                               new Vector2(UpperLeft.X + 20, UpperLeft.Y + 8),
                                               Color.Black,
                                               0,
                                               new Vector2(0, 0),
                                               1f,
                                               SpriteEffects.None, 0);
                    }
                }

                for (int i = 0; i < extraButtons.Count; i++)
                {
                    extraButtons[i].DrawParts(spriteBatch, font, isSelected);
                }
            }
        }
    }
}
