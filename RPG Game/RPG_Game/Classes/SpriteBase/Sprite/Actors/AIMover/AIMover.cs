using System;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    public class AIMover : Mover
    {
        Random rand = new Random();
        int temp;

        public override void Move(GameTime gameTime, Tile[,] map)
        {
            Step step = new Step();

            if (movementModifier != new Vector2(0, 0))
            {
                if (step.Invoke(gameTime, timer, 2, 0.4, this, movementModifier))
                {
                    timerDifference = gameTime.TotalGameTime.TotalSeconds;

                    movementModifier = new Vector2(0, 0);
                }
            }
            else
            {
                if (gameTime.TotalGameTime.TotalSeconds > timer + timeSinceStop + 2)
                {
                    temp = rand.Next(1, 5);
                    Vector2 tempModifier = new Vector2(0, 0);
                    int animationShortStart = 0;
                    int animationShortEnd = 2;
                    Vector2 failFrame = new Vector2(0, 0);

                    if (temp == 1)
                    {
                        tempModifier = new Vector2(0, -1);
                        animationShortStart = 9;
                        animationShortEnd = 11;
                        failFrame = new Vector2(1, 3);
                    }
                    else if (temp == 2)
                    {
                        tempModifier = new Vector2(0, 1);
                        animationShortStart = 0;
                        animationShortEnd = 2;
                        failFrame = new Vector2(1, 0);
                    }
                    else if (temp == 3)
                    {
                        tempModifier = new Vector2(1, 0);
                        animationShortStart = 6;
                        animationShortEnd = 8;
                        failFrame = new Vector2(1, 2);
                    }
                    else if (temp == 4)
                    {
                        tempModifier = new Vector2(-1, 0);
                        animationShortStart = 3;
                        animationShortEnd = 5;
                        failFrame = new Vector2(1, 1);
                    }
                    else
                    {
                        tempModifier = new Vector2(-1, -1);
                    }

                    if (MoveOnce(tempModifier, map))
                    {
                        StartAnimationShort(gameTime, animationShortStart, animationShortEnd, animationShortStart + 1);
                    }
                    else
                    {
                        setCurrentFrame((int)failFrame.X, (int)failFrame.Y);
                    }

                    timeSinceStop = 0;
                    timer = gameTime.TotalGameTime.TotalSeconds;
                }
            }
        }
    }
}
