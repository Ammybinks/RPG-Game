﻿using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class Camera
    {
        public Vector2 UpperLeft = new Vector2();

        public int ViewWidth = 0;
        public int ViewHeight = 0;
        public int WorldWidth = 0;
        public int WorldHeight = 0;

        public void LockCamera()
        {
            if (UpperLeft.X < 0)
            {
                UpperLeft.X = 0;
            }
            if ((UpperLeft.X + ViewWidth) > WorldWidth)
            {
                UpperLeft.X = WorldWidth - ViewWidth;
            }

            if (UpperLeft.Y < 0)
            {
                UpperLeft.Y = 0;
            }
            if ((UpperLeft.Y + ViewHeight) > WorldHeight)
            {
                UpperLeft.Y = WorldHeight - ViewHeight;
            }
        }

        public bool IsVisible(Vector2 objectUpperLeft, int objectWidth, int objectHeight)
        {
            Rectangle cameraRect = new Rectangle((int)UpperLeft.X, (int)UpperLeft.Y, ViewWidth, ViewHeight);

            Rectangle objectRect = new Rectangle((int)objectUpperLeft.X, (int)objectUpperLeft.Y, objectWidth, objectHeight);

            return cameraRect.Intersects(objectRect);
        }
    }
}
