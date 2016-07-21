using System;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    public class Sprite : SpriteBase
    {
        // fastest absolute speed the sprite will move
        public double MaxSpeed = 10;

        // the desired number of milliseconds between animation frame changes
        // if zero (default), animation will advae onnc each call to animate().
        public int AnimationInterval = 0;

        // the time in milliseconds when the last animation frame was changed
        private int lastAnimationTime = 0;

        // public flag indicating whether or not animation frames should be continuously looped
        public bool ContinuousAnimation = true;

        // Whether or not the sprite is animating forwards through the spritesheet instead of forwards
        public bool reverseAnimating;

        // if ContinuousAnimation = false, this flag indicates whether or not a 
        // "short" animation sequence is currently active
        public bool animationShortStarted = false;

        // this variable contains the stop frame for a short animation sequence
        private int animationShortStopFrame = 0;

        // this variable contains a single frame that will be displayed after
        // the short animation sequence is complete.
        private int animationShortFinalFrame = 0;

        private Matrix getTransformMatrix()
        {
            // see this link for a great description of transformation matrix creation:
            // http://www.riemers.net/eng/Tutorials/XNA/Csharp/Series2D/Coll_Detection_Matrices.php
            return
                Matrix.CreateTranslation(-Origin.X / Scale.X, -Origin.Y / Scale.Y, 0) *
                Matrix.CreateRotationZ(-MathHelper.ToRadians((float)this.RotationAngle)) *
                Matrix.CreateScale(Scale.X, Scale.Y, 1.0f) *
                Matrix.CreateTranslation(UpperLeft.X + (float)Origin.X, UpperLeft.Y + (float)Origin.Y, 0);
        }

        // adjust the current rotation angle by the indicated amount (in degrees)
        public void ChangeRotationAngle(double delta)
        {
            // adjust the sprite's current angle by the input amount (may be positive or negative)
            RotationAngle += delta;

            // keep the sprite's angle between [0,359] so it's easy to read and understand
            if (RotationAngle < 0.0)
                RotationAngle += 360.0;

            if (RotationAngle >= 360.0)
                RotationAngle -= 360.0;
        }

        public Rectangle GetBoundingRectangle()
        {
            if (RotationAngle == 0)
            {
                // when there is no rotation, return simple rectangle without resorting to complex math!
                // construct a new rectangle based on the UpperLeft point and current scaled width, height
                return new Rectangle((int)UpperLeft.X, (int)UpperLeft.Y, GetWidth(), GetHeight());
            }
            else
            {
                // this sprite is currently rotated, so we need to compute new UpperLeft and LowerRight
                // points based on the current rotation and scaling, then form bounding rectangle from those points.
                Matrix xformMatrix = getTransformMatrix();

                int width = textureColors.GetLength(0) / numFrames;
                int height = textureColors.GetLength(1);
                Vector2 actualUpperLeft = Vector2.Transform(new Vector2(0, 0), xformMatrix);
                Vector2 actualUpperRight = Vector2.Transform(new Vector2(width, 0), xformMatrix);
                Vector2 actualLowerLeft = Vector2.Transform(new Vector2(0, height), xformMatrix);
                Vector2 actualLowerRight = Vector2.Transform(new Vector2(width, height), xformMatrix);

                int minX = Math.Min((int)actualUpperLeft.X, (int)actualUpperRight.X);
                minX = Math.Min(minX, (int)actualLowerLeft.X);
                minX = Math.Min(minX, (int)actualLowerRight.X);

                int minY = Math.Min((int)actualUpperLeft.Y, (int)actualUpperRight.Y);
                minY = Math.Min(minY, (int)actualLowerLeft.Y);
                minY = Math.Min(minY, (int)actualLowerRight.Y);

                int maxX = Math.Max((int)actualUpperLeft.X, (int)actualUpperRight.X);
                maxX = Math.Max(maxX, (int)actualLowerLeft.X);
                maxX = Math.Max(maxX, (int)actualLowerRight.X);

                int maxY = Math.Max((int)actualUpperLeft.Y, (int)actualUpperRight.Y);
                maxY = Math.Max(maxY, (int)actualLowerLeft.Y);
                maxY = Math.Max(maxY, (int)actualLowerRight.Y);

                return new Rectangle(minX, minY, maxX - minX, maxY - minY);
            }
        }

        // this public method will launch a "short" animation sequence starting 
        // at the specified frame and stopping at the specified frame.  After the
        // animation ends the image will revert to the static "final" frame
        public void StartAnimationShort(GameTime gameTime, int startFrame, int stopFrame, int finalFrame)
        {
            int line = 0;

            if (!reverseAnimating)  // If animating forwards
            {
                // For each line in the spritesheet
                for (int i = 1; i <= numLines; i++)
                {
                    // If current frame is lower than the highest frame on this line
                    if (startFrame < lineFrames * i)
                    {
                        // Set current line to appropriate line
                        i--;

                        line = i;
                        break;
                    }
                }

                // reduce starting frame by one, to make sure we're animating the first frame
                startFrame--;

                // set starting frame as current frame (inside the bounds of lineFrames)
                currentFrame = startFrame - (lineFrames * line);

                // set starting frame as total frame
                totalFrame = startFrame;


                // store other input variables
                animationShortStopFrame = stopFrame;
            }
            else
            {
                // For each line in the spritesheet
                for (int i = 1; i < numLines; i++)
                {
                    // If current frame is lower than the highest frame on this line
                    if (startFrame < lineFrames * i)
                    {
                        // Set current line to appropriate line
                        i--;

                        line = i;
                        break;
                    }
                }
                // set starting frame as current frame (inside the bounds of lineFrames)
                currentFrame = stopFrame - (lineFrames * line);

                // set ending frame as total frame
                totalFrame = stopFrame;

                // store other input variables
                animationShortStopFrame = startFrame;
            }
            animationShortFinalFrame = finalFrame;

            // launch the short animation!
            animationShortStarted = true;
        }

        // this public method will return true if the image is animating either
        // continuously or is amidst a "short" animation sequence
        public bool IsAnimating()
        {
            return (animationShortStarted || ContinuousAnimation);
        }

        // games will call this public method to allow the sprite to advance to
        // the next animation frame if required, based on the pre-configured
        // animation interval and current animation state.
        public void Animate(GameTime gameTime)
        {
            // if we are currently supposed to be animating for any reason
            if (IsAnimating())
            {
                // advance to the next frame if enough game time has elapsed
                advanceFrame(gameTime);
            }
        }


        // change the animation to the next frame 
        private void advanceFrame(GameTime gameTime)
        {
            // get the current time
            int now = (int)gameTime.TotalGameTime.TotalMilliseconds;

            // Math modifier for if we're reversing
            int modifier;

            // if we have not yet reached our next scheduled frame change. Or, if we are not animating at all
            if ((now < (lastAnimationTime + AnimationInterval)) || !(IsAnimating()))
            {
                return; // not time to advance frame yet
            }

            // figure out our last frame based on continuous or "short" mode
            int endFrame = numFrames - 1;   // default for continuous animation
            if (animationShortStarted)
                endFrame = animationShortStopFrame;

            if (reverseAnimating)
            {
                currentFrame--;  // move to the previous frame
                totalFrame--;

                modifier = -1;
            }
            else
            {
                currentFrame++;  // move to the next frame
                totalFrame++;

                modifier = 1;
            }

            // if we are not yet done with this sequence
            if (totalFrame * modifier < (endFrame + 1) * modifier)
            {
                // For each line in the spritesheet
                for (int i = 1; i <= numLines; i++)
                {
                    // If current frame is lower than the highest frame on this line
                    if (totalFrame < lineFrames * i)
                    {
                        // Set current line to appropriate line
                        i--;

                        currentLine = i;
                        break;
                    }
                }

            }
            else
            {
                // if continuous animation, reset sequence to 0
                if (ContinuousAnimation)
                {
                    currentFrame = 0;
                    totalFrame = 0;
                }
                else
                {
                    // for animation short, set current frame to final frame
                    currentFrame = animationShortFinalFrame - (lineFrames * currentLine);
                    totalFrame = animationShortFinalFrame;
                    animationShortStarted = false;  // no longer animating
                }
            }

            if (totalFrame > numFrames - 1 || totalFrame < 0)  // safety check!
            {
                totalFrame = 0;
                currentFrame = 0;
                currentLine = 0;
            }
            if (currentFrame > lineFrames - 1)
            {
                currentFrame = 0;
            }

            // adjust imageRect to match new current frame
            imageRect = new Rectangle(currentFrame * frameWidth, currentLine * frameHeight, frameWidth, frameHeight);

            // update last animation time
            lastAnimationTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
        }
    }
}
