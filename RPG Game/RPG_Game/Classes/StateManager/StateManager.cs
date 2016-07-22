using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RPG_Game
{
    public class StateManager
    {
        internal int targetState;

        internal KeyboardState currentKeyState;
        internal KeyboardState oldKeyState;

        internal MouseState currentMouseState;
        internal MouseState oldMouseState;
        internal Point mousePosition;
        internal bool mouseMoving;

        internal Input upInput = new Input();
        internal Input downInput = new Input();
        internal Input leftInput = new Input();
        internal Input rightInput = new Input();
        internal Input activateInput = new Input();
        internal Input menuInput = new Input();

        internal Texture2D pointerTexture;
        internal Sprite pointer = new Sprite();

        internal Box box;

        internal List<Box> allBoxes = new List<Box>();

        internal List<Button> activeButtons;

        internal Texture2D iconTexture;
        internal Button button;

        internal Texture2D cornerTexture;
        internal Texture2D wallTexture;
        internal Texture2D backTexture;

        internal SpriteFont calibri;

        internal int buttonIndex = 0;

        internal double timer;

        internal Step step = new Step();

        public bool quitting = false;

        public virtual void LoadContent(Main main)
        {

        }

        public virtual void GetInput(GameTime gameTime)
        {
            currentKeyState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            if (oldKeyState == null)
            {
                oldKeyState = currentKeyState;
            }
            if (oldMouseState == null)
            {
                oldMouseState = currentMouseState;
                mousePosition = new Point(currentMouseState.X, currentMouseState.Y);
            }

            ////Mouse input handling
            if (currentMouseState.X < oldMouseState.X || currentMouseState.X > oldMouseState.X ||
               currentMouseState.Y < oldMouseState.Y || currentMouseState.Y > oldMouseState.Y)
            {
                mouseMoving = true;
            }
            else
            {
                mouseMoving = false;
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                activateInput.inputState = Input.inputStates.pressed;
                activateInput.inputType = Input.inputTypes.mouse;
            }
            else
            {
                activateInput.inputState = Input.inputStates.released;
                activateInput.inputType = Input.inputTypes.mouse;
            }

            ////Keyboard input handling
            //Up input handling
            if ((currentKeyState.IsKeyDown(Keys.W)) && (oldKeyState.IsKeyUp(Keys.W)))
            {
                upInput.inputState = Input.inputStates.pressed;
                upInput.inputType = Input.inputTypes.keyboard;
            }
            else if ((currentKeyState.IsKeyDown(Keys.W) && oldKeyState.IsKeyDown(Keys.W)))
            {
                upInput.inputState = Input.inputStates.held;
                upInput.inputType = Input.inputTypes.keyboard;
            }
            else if ((currentKeyState.IsKeyUp(Keys.W) && oldKeyState.IsKeyDown(Keys.W)))
            {
                upInput.inputState = Input.inputStates.released;
                upInput.inputType = Input.inputTypes.keyboard;
            }

            //Down input handling
            if ((currentKeyState.IsKeyDown(Keys.S)) && (oldKeyState.IsKeyUp(Keys.S)))
            {
                downInput.inputState = Input.inputStates.pressed;
                downInput.inputType = Input.inputTypes.keyboard;
            }
            else if ((currentKeyState.IsKeyDown(Keys.S) && oldKeyState.IsKeyDown(Keys.S)))
            {
                downInput.inputState = Input.inputStates.held;
                downInput.inputType = Input.inputTypes.keyboard;
            }
            else if ((currentKeyState.IsKeyUp(Keys.S) && oldKeyState.IsKeyDown(Keys.S)))
            {
                downInput.inputState = Input.inputStates.released;
                downInput.inputType = Input.inputTypes.keyboard;
            }

            //Left input handling
            if ((currentKeyState.IsKeyDown(Keys.A)) && (oldKeyState.IsKeyUp(Keys.A)))
            {
                leftInput.inputState = Input.inputStates.pressed;
                leftInput.inputType = Input.inputTypes.keyboard;
            }
            else if ((currentKeyState.IsKeyDown(Keys.A) && oldKeyState.IsKeyDown(Keys.A)))
            {
                leftInput.inputState = Input.inputStates.held;
                leftInput.inputType = Input.inputTypes.keyboard;
            }
            else if ((currentKeyState.IsKeyUp(Keys.A) && oldKeyState.IsKeyDown(Keys.A)))
            {
                leftInput.inputState = Input.inputStates.released;
                leftInput.inputType = Input.inputTypes.keyboard;
            }

            //Right input handling
            if ((currentKeyState.IsKeyDown(Keys.D)) && (oldKeyState.IsKeyUp(Keys.D)))
            {
                rightInput.inputState = Input.inputStates.pressed;
                rightInput.inputType = Input.inputTypes.keyboard;
            }
            else if ((currentKeyState.IsKeyDown(Keys.D) && oldKeyState.IsKeyDown(Keys.D)))
            {
                rightInput.inputState = Input.inputStates.held;
                rightInput.inputType = Input.inputTypes.keyboard;
            }
            else if ((currentKeyState.IsKeyUp(Keys.D) && oldKeyState.IsKeyDown(Keys.D)))
            {
                rightInput.inputState = Input.inputStates.released;
                rightInput.inputType = Input.inputTypes.keyboard;
            }

            //Activate input handling
            if (((currentKeyState.IsKeyDown(Keys.Z)) && (oldKeyState.IsKeyUp(Keys.Z))) ||
                ((currentKeyState.IsKeyDown(Keys.Space)) && (oldKeyState.IsKeyUp(Keys.Space))))
            {
                activateInput.inputState = Input.inputStates.pressed;
                activateInput.inputType = Input.inputTypes.keyboard;
            }
            else if (((currentKeyState.IsKeyDown(Keys.Z) && oldKeyState.IsKeyDown(Keys.Z))) ||
                     ((currentKeyState.IsKeyDown(Keys.Space)) && (oldKeyState.IsKeyDown(Keys.Space))))
            {
                activateInput.inputState = Input.inputStates.held;
                activateInput.inputType = Input.inputTypes.keyboard;
            }
            else if (((currentKeyState.IsKeyUp(Keys.Z) && oldKeyState.IsKeyDown(Keys.Z))) ||
                     ((currentKeyState.IsKeyUp(Keys.Space)) && (oldKeyState.IsKeyDown(Keys.Space))))
            {
                activateInput.inputState = Input.inputStates.released;
                activateInput.inputType = Input.inputTypes.keyboard;
            }

            //Menu input handling
            if (((currentKeyState.IsKeyDown(Keys.X)) && (oldKeyState.IsKeyUp(Keys.X))) ||
                ((currentKeyState.IsKeyDown(Keys.Escape)) && (oldKeyState.IsKeyUp(Keys.Escape))))
            {
                menuInput.inputState = Input.inputStates.pressed;
                menuInput.inputType = Input.inputTypes.keyboard;
            }
            else if (((currentKeyState.IsKeyDown(Keys.X) && oldKeyState.IsKeyDown(Keys.X))) ||
                     ((currentKeyState.IsKeyDown(Keys.Escape)) && (oldKeyState.IsKeyDown(Keys.Escape))))
            {
                menuInput.inputState = Input.inputStates.held;
                menuInput.inputType = Input.inputTypes.keyboard;
            }
            else if (((currentKeyState.IsKeyUp(Keys.X) && oldKeyState.IsKeyDown(Keys.X))) ||
                     ((currentKeyState.IsKeyUp(Keys.Escape)) && (oldKeyState.IsKeyDown(Keys.Escape))))
            {
                menuInput.inputState = Input.inputStates.released;
                menuInput.inputType = Input.inputTypes.keyboard;
            }

            mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

            oldKeyState = currentKeyState;
            oldMouseState = currentMouseState;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, Main main)
        {

        }

    }
}
