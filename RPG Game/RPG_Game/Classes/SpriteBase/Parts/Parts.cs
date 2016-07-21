using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace RPG_Game
{
    public class Parts : SpriteBase
    {
        public List<Sprite> parts = new List<Sprite>(9);

        public void SetParts(Texture2D cornerTexture, Texture2D wallTexture, Texture2D backTexture)
        {
            parts.Clear();

            Sprite corner;

            corner = new Sprite();
            corner.SetTexture(cornerTexture);
            corner.UpperLeft = UpperLeft;
            parts.Add(corner);

            corner = new Sprite();
            corner.SetTexture(cornerTexture);
            corner.UpperLeft = new Vector2(UpperLeft.X, UpperLeft.Y + GetHeight());
            corner.RotationAngle = 90;
            parts.Add(corner);

            corner = new Sprite();
            corner.SetTexture(cornerTexture);
            corner.UpperLeft = new Vector2(UpperLeft.X + GetWidth(), UpperLeft.Y + GetHeight());
            corner.RotationAngle = 180;
            parts.Add(corner);

            corner = new Sprite();
            corner.SetTexture(cornerTexture);
            corner.UpperLeft = new Vector2(UpperLeft.X + GetWidth(), UpperLeft.Y);
            corner.RotationAngle = 270;
            parts.Add(corner);


            Sprite wall;

            wall = new Sprite();
            wall.SetTexture(wallTexture);
            wall.Scale = new Vector2(1, GetHeight() - corner.GetHeight() * 2);
            wall.UpperLeft = new Vector2(UpperLeft.X, UpperLeft.Y + corner.GetHeight());
            parts.Add(wall);

            wall = new Sprite();
            wall.SetTexture(wallTexture);
            wall.Scale = new Vector2(1, GetWidth() - corner.GetWidth() * 2);
            wall.UpperLeft = new Vector2(UpperLeft.X + corner.GetWidth(), UpperLeft.Y + GetHeight());
            wall.RotationAngle = 90;
            parts.Add(wall);

            wall = new Sprite();
            wall.SetTexture(wallTexture);
            wall.Scale = new Vector2(1, GetHeight() - corner.GetHeight() * 2);
            wall.UpperLeft = new Vector2(UpperLeft.X + GetWidth(), UpperLeft.Y + corner.GetHeight() + wall.GetHeight());
            wall.RotationAngle = 180;
            parts.Add(wall);

            wall = new Sprite();
            wall.SetTexture(wallTexture);
            wall.Scale = new Vector2(1, GetWidth() - corner.GetWidth() * 2);
            wall.UpperLeft = new Vector2(UpperLeft.X + GetWidth() - corner.GetWidth(), UpperLeft.Y);
            wall.RotationAngle = 270;
            parts.Add(wall);

            Sprite back;

            back = new Sprite();
            back.SetTexture(backTexture);
            back.Scale = new Vector2(GetWidth() - corner.GetWidth() * 2, GetHeight() - corner.GetHeight() * 2);
            back.UpperLeft = new Vector2(UpperLeft.X + corner.GetWidth(), UpperLeft.Y + corner.GetHeight());
            parts.Add(back);
        }

        public void DrawParts(SpriteBatch spriteBatch)
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
