using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RPG_Game
{
    public class Parts : SpriteBase
    {
        public List<SpriteBase> parts = new List<SpriteBase>(9);

        public void SetParts(Texture2D cornerTexture, Texture2D wallTexture, Texture2D backTexture)
        {
            SetParts(cornerTexture, wallTexture, backTexture, parts);
        }

        public void SetParts(Texture2D cornerTexture, Texture2D wallTexture, Texture2D backTexture, List<SpriteBase> list)
        {
            list.Clear();

            SpriteBase corner;

            corner = new SpriteBase();
            corner.SetTexture(cornerTexture);
            corner.UpperLeft = UpperLeft;
            list.Add(corner);

            corner = new SpriteBase();
            corner.SetTexture(cornerTexture);
            corner.UpperLeft = new Vector2(UpperLeft.X, UpperLeft.Y + GetHeight());
            corner.RotationAngle = 90;
            list.Add(corner);

            corner = new SpriteBase();
            corner.SetTexture(cornerTexture);
            corner.UpperLeft = new Vector2(UpperLeft.X + GetWidth(), UpperLeft.Y + GetHeight());
            corner.RotationAngle = 180;
            list.Add(corner);

            corner = new SpriteBase();
            corner.SetTexture(cornerTexture);
            corner.UpperLeft = new Vector2(UpperLeft.X + GetWidth(), UpperLeft.Y);
            corner.RotationAngle = 270;
            list.Add(corner);


            SpriteBase wall;

            wall = new SpriteBase();
            wall.SetTexture(wallTexture);
            wall.Scale = new Vector2(1, GetHeight() - corner.GetHeight() * 2);
            wall.UpperLeft = new Vector2(UpperLeft.X, UpperLeft.Y + corner.GetHeight());
            list.Add(wall);

            wall = new SpriteBase();
            wall.SetTexture(wallTexture);
            wall.Scale = new Vector2(1, GetWidth() - corner.GetWidth() * 2);
            wall.UpperLeft = new Vector2(UpperLeft.X + corner.GetWidth(), UpperLeft.Y + GetHeight());
            wall.RotationAngle = 90;
            list.Add(wall);

            wall = new SpriteBase();
            wall.SetTexture(wallTexture);
            wall.Scale = new Vector2(1, GetHeight() - corner.GetHeight() * 2);
            wall.UpperLeft = new Vector2(UpperLeft.X + GetWidth(), UpperLeft.Y + corner.GetHeight() + wall.GetHeight());
            wall.RotationAngle = 180;
            list.Add(wall);

            wall = new SpriteBase();
            wall.SetTexture(wallTexture);
            wall.Scale = new Vector2(1, GetWidth() - corner.GetWidth() * 2);
            wall.UpperLeft = new Vector2(UpperLeft.X + GetWidth() - corner.GetWidth(), UpperLeft.Y);
            wall.RotationAngle = 270;
            list.Add(wall);

            SpriteBase back;

            back = new SpriteBase();
            back.SetTexture(backTexture);
            back.Scale = new Vector2(GetWidth() - corner.GetWidth() * 2, GetHeight() - corner.GetHeight() * 2);
            back.UpperLeft = new Vector2(UpperLeft.X + corner.GetWidth(), UpperLeft.Y + corner.GetHeight());
            list.Add(back);
        }

        public virtual void DrawParts(SpriteBatch spriteBatch)
        {
            if (isAlive)
            {
                for (int i = 0; i < parts.Count; i++)
                {
                    parts[i].Draw(spriteBatch);
                }
            }
        }
    }

}
