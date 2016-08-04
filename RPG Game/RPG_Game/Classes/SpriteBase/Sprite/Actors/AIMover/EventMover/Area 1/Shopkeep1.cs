using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class Shopkeep1 : EventMover
    {
        public override void Call(GameTime gameTime, NaviState naviState)
        {
            if (typingStrings == null)
            {
                box = new Box();

                box.frameWidth = 800;
                box.frameHeight = 200;
                box.UpperLeft = new Vector2(560, 880);

                box.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

                naviState.pointer.Scale = new Vector2(0.4f, 0.4f);
                naviState.pointer.UpperLeft = new Vector2((box.frameWidth + box.UpperLeft.X) - naviState.pointer.GetWidth() - 20, 
                                                          (box.frameHeight + box.UpperLeft.Y) - naviState.pointer.GetHeight() - 20);
                naviState.pointer.isAlive = false;

                typingStrings = new TypingStrings();
                typingStrings.lines = new List<string>();
                typingStrings.lines.Add("I'm supposed to be a shopkeeper.\n\n");
                typingStrings.lines.Add("But really, I'm only here because Nye\nthinks I look weirdly like Shantae.");
                typingStrings.lines.Add("I guess I haven't been implemented yet, what a bore.");
                typingStrings.line = "";
                typingStrings.previousLines = "";

                if(naviState.heroMover.gridPosition == new Vector2(gridPosition.X, gridPosition.Y + 1))
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

            if (naviState.Type(typingStrings, gameTime, 0.01))
            {
                Complete(naviState);
            }
        }
    }
}
