using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    public class EventMover : AIMover
    {
        internal List<Box> allBoxes = new List<Box>();

        public TypingStrings typingStrings;

        public Box box;

        internal int state;
        internal int previousState;

        internal int index;
        internal int previousIndex = -1;

        public virtual void Call(GameTime gameTime, NaviState naviState)
        {
            if (typingStrings == null)
            {
                typingStrings = new TypingStrings();
                typingStrings.lines = new List<string>();
                typingStrings.lines.Add("");
                typingStrings.line = "";
                typingStrings.previousLines = "";

                if (naviState.heroMover.gridPosition == new Vector2(gridPosition.X, gridPosition.Y + 1))
                {
                    setCurrentFrame(1, 0);
                }
                else if (naviState.heroMover.gridPosition == new Vector2(gridPosition.X - 1, gridPosition.Y))
                {
                    setCurrentFrame(1, 1);
                }
                else if (naviState.heroMover.gridPosition == new Vector2(gridPosition.X + 1, gridPosition.Y))
                {
                    setCurrentFrame(1, 2);
                }
                else if (naviState.heroMover.gridPosition == new Vector2(gridPosition.X, gridPosition.Y - 1))
                {
                    setCurrentFrame(1, 3);
                }

                previousState = 0;
                state = 0;
            }

            if (state == 0)
            {
                Idle(naviState, gameTime);
            }
            else if(state == 1)
            {
                ChoiceMenu(naviState, gameTime);
            }
        }

        internal virtual void Idle(NaviState naviState, GameTime gameTime)
        {
            if (naviState.Type(typingStrings, gameTime, 0.01))
            {
                Complete(naviState);
            }
        }

        internal virtual void ChoiceMenu(NaviState naviState, GameTime gameTime)
        {
            List<SpriteBase> tempList = new List<SpriteBase>();
            for (int i = 0; i < box.buttons.Count; i++)
            {
                tempList.Add(box.buttons[i]);
            }

            MenuUpdateReturn temp = naviState.MenuUpdate(tempList, index);
            index = temp.index;

            naviState.pointer.UpperLeft = new Vector2(box.buttons[index].UpperLeft.X + box.buttons[index].frameWidth + 15,
                                                      box.buttons[index].UpperLeft.Y + 5);
            naviState.pointer.isAlive = true;

            if (temp.activate)
            {
                EventButton tempButton = (EventButton)box.buttons[index];
                tempButton.action.Invoke(naviState, gameTime);
            }
        }

        public virtual void DrawAll(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (box != null)
            {
                box.DrawParts(spriteBatch);

                if(typingStrings.line != null)
                {
                    spriteBatch.DrawString(spriteFont,
                                           typingStrings.line,
                                           new Vector2(580, 900),
                                           Color.Black);
                }
            }
        }

        internal void Complete(NaviState naviState)
        {
            naviState.ActivateState(0);

            naviState.eventMover = null;

            naviState.pointer.Scale = new Vector2(0.8f, 0.8f);

            typingStrings = null;
        }
        
        internal void PointerReset(NaviState naviState)
        {
            naviState.pointer.Scale = new Vector2(0.4f, 0.4f);
            naviState.pointer.UpperLeft = new Vector2((box.frameWidth + box.UpperLeft.X) - naviState.pointer.GetWidth() - 20,
                                                      (box.frameHeight + box.UpperLeft.Y) - naviState.pointer.GetHeight() - 20);
            naviState.pointer.RotationAngle = 0;
            naviState.pointer.isAlive = false;
        }

        internal void Initialize(NaviState naviState)
        {
            box = new Box();

            box.frameWidth = 800;
            box.frameHeight = 200;
            box.UpperLeft = new Vector2(560, 880);
            box.buttons = new List<Button>();

            box.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

            allBoxes.Add(box);

            PointerReset(naviState);

            typingStrings = new TypingStrings();
            typingStrings.lines = new List<string>();
            typingStrings.line = "";
            typingStrings.previousLines = "";

            if (naviState.heroMover.gridPosition == new Vector2(gridPosition.X, gridPosition.Y + 1))
            {
                setCurrentFrame(1, 0);
            }
            else if (naviState.heroMover.gridPosition == new Vector2(gridPosition.X - 1, gridPosition.Y))
            {
                setCurrentFrame(1, 1);
            }
            else if (naviState.heroMover.gridPosition == new Vector2(gridPosition.X + 1, gridPosition.Y))
            {
                setCurrentFrame(1, 2);
            }
            else if (naviState.heroMover.gridPosition == new Vector2(gridPosition.X, gridPosition.Y - 1))
            {
                setCurrentFrame(1, 3);
            }
        }

        internal void SwitchState(int targetState)
        {
            previousState = state;
            state = targetState;
        }
    }
}

