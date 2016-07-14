using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace RPG_Game
{
    class StateManager
    {
        public int targetState;

        public KeyboardState currentKeyState;
        public KeyboardState oldKeyState;

        public MouseState currentMouseState;
        public MouseState oldMouseState;
        public Point mousePosition;
        public bool mouseMoving;

        public Input upInput = new Input();
        public Input downInput = new Input();
        public Input leftInput = new Input();
        public Input rightInput = new Input();
        public Input activateInput = new Input();

        public Texture2D pointerTexture;
        public Sprite pointer = new Sprite();

        public Box box;

        public List<Box> allBoxes = new List<Box>(1);

        public List<Button> activeButtons;

        public Texture2D iconTexture;
        public Button button;

        public Texture2D cornerTexture;
        public Texture2D wallTexture;
        public Texture2D backTexture;

        public SpriteFont calibri;

        public double timer;


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

            mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

            oldKeyState = currentKeyState;
            oldMouseState = currentMouseState;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

    }
}
