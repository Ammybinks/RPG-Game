using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace RPG_Game
{
    public class Sprite
    {
        // upper-left coordinate of the sprite image on the screen
        public Vector2 UpperLeft = new Vector2(0,0);

        // X and Y stretching factors to adjust the final sprite dimensions
        public Vector2 Scale = new Vector2(1.0f, 1.0f);

        // this member holds the current Texture for the sprite
        private Texture2D spriteTexture;

        // if true, then sprite is visible and can move and interact (collide) with others
        public bool IsAlive = true;

        // fastest absolute speed the sprite will move
        public double MaxSpeed = 10;

        // current sprite velocity (X and Y speed components)
        public Vector2 velocity;

        // current value, in degrees, of the rotation of the sprite
        public double RotationAngle = 0;

        // origin for the sprite (defaults to Upper-Left)
        public Vector2 Origin = new Vector2(0, 0);

        // indication of which depth to use when drawing sprite (for layering purposes)
        public float LayerDepth = 0;

        // color array used internally for collision detection
        private Color[,] textureColors;

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

        // This internal member tracks the current frame numerically, regardless of current line
        private int totalFrame = 0;

        // This internal member tracks the number of frames and lines in the animation strip
        private int numFrames = 1;
        private int numLines = 1;

        // This internal member tracks the number of frames in any given line
        private int lineFrames;

        // This internal member represents the current source rectangle in the animation strip
        private Rectangle imageRect;

        // These internal members specify the width and height of a single frame in the animation strip
        // (Note:  overall strip dimensions should be an even multiple of this value!)
        public int frameWidth;
        public int frameHeight;

        // This internal member shows the current animation frame on the current line (should be 0 -> numFrames-1)
        private int currentFrame = 0;

        // This internal member shows the current animation line
        private int currentLine = 0;


        public int getCurrentFrame()
        {
            return currentFrame;
        }

        public int getTotalFrame()
        {
            return totalFrame;
        }

        // Sets the current frame the sprite is displaying
        // (NOTE: Input is taken in the form of the line, and frame along that line (Upper limit for frame is TotalFrames / TotalLines)
        public void setCurrentFrame(int frame, int line)
        {
            if (frame > lineFrames - 1 || frame < 0)  // safety check!
            {
                frame = 0;
            }

            if(line > numLines - 1 || line < 0)
            {
                line = 0;
            }

            currentFrame = frame;

            currentLine = line;

            totalFrame = frame + (lineFrames * line);


            imageRect = new Rectangle(frameWidth * frame, frameHeight * line, frameWidth, frameHeight);
        }

        // This method will load the Texture based on the image name
        public void SetTexture(Texture2D texture)
        {
            SetTexture(texture, 1);
        }

        // This method will load the Texture based on the image name and number of frames
        public void SetTexture(Texture2D texture, int frames)
        {
            SetTexture(texture, frames, 1);
        }

        // This method will load the Texture based on the image name and number of frames on each line and lines total
        public void SetTexture(Texture2D texture, int frames, int lines)
        {
            lineFrames = frames;
            numLines = lines;

            numFrames = frames * lines;

            spriteTexture = texture;
            int width = spriteTexture.Width;
            int height = spriteTexture.Height;

            frameWidth = width / lineFrames;   // does not include effects of scaling!
            frameHeight = height / numLines;

            // does not include effects of scaling, which may change after SetTexture is finished!
            imageRect = new Rectangle(0, 0, frameWidth, frameHeight);

            // create a color matrix that we'll use later for collision detection
            // contains colors for the entire image (including any animation strip)
            Color[] colorData = new Color[width * height];
            spriteTexture.GetData(colorData);

            textureColors = new Color[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    textureColors[x, y] = colorData[x + y * width];
                }
            }
        }

        // This method will draw the sprite using the current position, rotation, scale, and layer depth
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            float radians = MathHelper.ToRadians((float)RotationAngle);
            if (IsAlive)
                theSpriteBatch.Draw(spriteTexture, UpperLeft + Origin, imageRect, Color.White,
                                    -radians, Origin / Scale, Scale, SpriteEffects.None, LayerDepth);
        }

        // This method will draw the sprite using the current position, rotation, scale, and layer depth
        public virtual void Draw(SpriteBatch theSpriteBatch, Vector2 cameraUpperLeft)
        {
            UpperLeft -= cameraUpperLeft;
            Draw(theSpriteBatch);
            UpperLeft += cameraUpperLeft;
        }

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

        // calculate final sprite width, accounting for scale and assuming zero rotation
        public int GetWidth()
        {
            return (int)(frameWidth * Scale.X);
        }

        // calculate final sprite height, accounting for scale and assuming zero rotation
        public int GetHeight()
        {
            return (int)(frameHeight * Scale.Y);
        }

        // calculate current center offset from the UpperLeft, accounting for scale and assuming zero rotation
        public Vector2 GetCenter()
        {
            return new Vector2(GetWidth() / 2, GetHeight() / 2);
        }

        // this function will move the sprite according to it's current Velocity
        public bool Move()
        {
            // create and calculate the new position based on current upper-left coordinate and velocity
            Vector2 newPosition;

            newPosition.X = UpperLeft.X + velocity.X;
            newPosition.Y = UpperLeft.Y + velocity.Y;

            // update the sprite's current UpperLeft coordinates with the final position
            UpperLeft = newPosition;

            return true;
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
        public void StartAnimationShort(int startFrame, int stopFrame, int finalFrame)
        {
            if(!reverseAnimating)  // If animating forwards
            {
                // For each line in the spritesheet
                for (int i = 1; i <= numLines; i++)
                {
                    // If current frame is lower than the highest frame on this line
                    if (startFrame <= lineFrames * i)
                    {
                        // Set current line to appropriate line
                        i--;

                        currentLine = i;
                        break;
                    }
                }
                // set starting frame as current frame (inside the bounds of lineFrames)
                currentFrame = startFrame - (lineFrames * currentLine);

                // set starting frame as total frame
                totalFrame = startFrame;


                // store other input variables
                animationShortStopFrame = stopFrame;
            }
            else
            {
                // For each line in the spritesheet
                for (int i = 1; i <= numLines; i++)
                {
                    // If current frame is lower than the highest frame on this line
                    if (startFrame <= lineFrames * i)
                    {
                        // Set current line to appropriate line
                        i--;

                        currentLine = i;
                        break;
                    }
                }
                // set starting frame as current frame (inside the bounds of lineFrames)
                currentFrame = stopFrame - (lineFrames * currentLine);

                // set ending frame as current frame
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
            if (totalFrame * modifier < endFrame * modifier)
            {
                // For each line in the spritesheet
                for (int i = 1; i <= numLines; i++)
                {
                    // If current frame is lower than the highest frame on this line
                    if (totalFrame <= lineFrames * i)
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

            if(totalFrame > numFrames - 1 || totalFrame < 0)  // safety check!
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
    public class Character : Sprite
    {
        public int health;

        public float PhAtk;
        public float MgAtk;

        public float PhDef;
        public float MgDef;

        public float speed;

        public float Acc;
        public float Eva;

        public float meter;
        public float pastMeter;

        public Sprite meterSprite = new Sprite();

        public Sprite shadow = new Sprite();

        public List<Ability> abilities = new List<Ability>();
        
        public Vector2 battleOrigin;
        public bool friendly;
    }

    public class Box : Sprite
    {
        public List<Sprite> parts = new List<Sprite>(9);

        public void SetParts(Texture2D cornerTexture, Texture2D wallTexture, Texture2D backTexture)
        {
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
    }
    public class Button : Box
    {
        public Sprite icon;

        public string action;
    }

    public class Ability : Button
    {
        public int cost;
    }
}
