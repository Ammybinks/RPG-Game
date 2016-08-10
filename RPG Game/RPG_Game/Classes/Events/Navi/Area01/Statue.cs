using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    [Serializable()]
    class Statue : Event
    {
        public override bool Call(GameTime gameTime, NaviState naviState)
        {
            if (typingStrings == null)
            {
                naviState.currentEvent = this;

                eventBox = new Box();

                eventBox.frameWidth = 800;
                eventBox.frameHeight = 200;
                eventBox.UpperLeft = new Vector2(560, 880);

                eventBox.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

                naviState.pointer.Scale = new Vector2(0.4f, 0.4f);
                naviState.pointer.UpperLeft = new Vector2((eventBox.frameWidth + eventBox.UpperLeft.X) - naviState.pointer.GetWidth() - 20,
                                                          (eventBox.frameHeight + eventBox.UpperLeft.Y) - naviState.pointer.GetHeight() - 20);
                naviState.pointer.isAlive = false;

                typingStrings = new TypingStrings();
                typingStrings.lines = new List<string>();
                typingStrings.lines.Add("This is, well. I'm not sure what it is, quite.\n");
                typingStrings.lines.Add("Maybe some sort of altar? I've seen stranger.");
                typingStrings.line = "";
                typingStrings.previousLines = "";
            }

            if (naviState.Type(typingStrings, gameTime, 0.01))
            {
                Complete(naviState);

                return true;
            }

            return false;
        }
    }
}
