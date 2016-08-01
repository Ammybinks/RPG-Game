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

                DrawDisplay(spriteBatch, font);

                if (showOnSelected)
                {
                    if (!isSelected)
                    {
                        return;
                    }
                }

                for (int i = 0; i < extraButtons.Count; i++)
                {
                    extraButtons[i].DrawParts(spriteBatch, font, isSelected);
                }
            }
        }

        public void propagateColours()
        {
            for (int i = 0; i < extraButtons.Count; i++)
            {
                extraButtons[i].displayColour = displayColour;
            }
        }
    }
}
