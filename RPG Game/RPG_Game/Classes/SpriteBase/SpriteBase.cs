using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    public class SpriteBase
    {
        public bool isAlive = true;

        // upper-left coordinate of the sprite image on the screen
        public Vector2 UpperLeft = new Vector2(0, 0);

        // X and Y stretching factors to adjust the final sprite dimensions
        public Vector2 Scale = new Vector2(1.0f, 1.0f);

        // this member holds the current Texture for the sprite
        protected Texture2D spriteTexture;

        // color array used internally for collision detection
        protected Color[,] textureColors;

        // This internal member represents the current source rectangle in the animation strip
        protected Rectangle imageRect;

        // current value, in degrees, of the rotation of the sprite
        public double RotationAngle = 0;

        // origin for the sprite (defaults to Upper-Left)
        public Vector2 Origin = new Vector2(0, 0);

        // indication of which depth to use when drawing sprite (for layering purposes)
        public float LayerDepth = 0;

        // This internal member tracks the current frame numerically, regardless of current line
        protected int totalFrame = 0;

        // This internal member tracks the number of frames and lines in the animation strip
        protected int numFrames = 1;
        protected int numLines = 1;

        // This internal member tracks the number of frames in any given line
        protected int lineFrames;

        // These internal members specify the width and height of a single frame in the animation strip
        // (Note:  overall strip dimensions should be an even multiple of this value!)
        public int frameWidth;
        public int frameHeight;

        public Color drawColour = Color.White;

        // This internal member shows the current animation frame on the current line (should be 0 -> numFrames-1)
        protected int currentFrame = 0;

        // This internal member shows the current animation line
        protected int currentLine = 0;
        
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

            if (line > numLines - 1 || line < 0)
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

            frameWidth = spriteTexture.Width / lineFrames;   // does not include effects of scaling!
            frameHeight = spriteTexture.Height / numLines;

            // does not include effects of scaling, which may change after SetTexture is finished!
            imageRect = new Rectangle(0, 0, frameWidth, frameHeight);

        }

        // This method will draw the sprite using the current position, rotation, scale, and layer depth
        public virtual void Draw(SpriteBatch theSpriteBatch)
        {
            if (isAlive)
            {
                float radians = MathHelper.ToRadians((float)RotationAngle);

                theSpriteBatch.Draw(spriteTexture, UpperLeft + Origin, imageRect, drawColour,
                                    -radians, Origin / Scale, Scale, SpriteEffects.None, LayerDepth);
            }
        }

        // This method will draw the sprite using the current position, rotation, scale, and layer depth
        public virtual void Draw(SpriteBatch theSpriteBatch, Vector2 cameraUpperLeft)
        {
            UpperLeft -= cameraUpperLeft;
            Draw(theSpriteBatch);
            UpperLeft += cameraUpperLeft;
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

    }
}
