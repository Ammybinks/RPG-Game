using System;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    public class Mover : Sprite
    {
        public Vector2 gridPosition;

        public Vector2 movementModifier;

        public Vector2 lastMoved;

        public double timer;
        public double timerDifference;
        public double timeSinceStop;
        
        public virtual void Move(GameTime gameTime, Tile[,] map)
        {
        }

        public virtual bool MoveOnce(Vector2 modifier, Tile[,] map)
        {
            movementModifier = modifier;

            if (!map[(int)gridPosition.X + (int)movementModifier.X, (int)gridPosition.Y + (int)movementModifier.Y].walkable ||
                map[(int)gridPosition.X + (int)movementModifier.X, (int)gridPosition.Y + (int)movementModifier.Y].occupied)
            {
                map[(int)gridPosition.X, (int)gridPosition.Y].occupied = true;

                lastMoved = movementModifier;

                movementModifier = new Vector2(0, 0);

                return false;
            }
            else
            {
                map[(int)gridPosition.X, (int)gridPosition.Y].occupied = false;
                map[(int)gridPosition.X, (int)gridPosition.Y].occupier = null;

                gridPosition += movementModifier;

                map[(int)gridPosition.X, (int)gridPosition.Y].occupied = true;
                map[(int)gridPosition.X, (int)gridPosition.Y].occupier = this;

                lastMoved = movementModifier;

                return true;
            }
        }
    }
}
