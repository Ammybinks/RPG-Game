using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    [Serializable()]
    public class Area01 : Event
    {
        public void Statue(NaviState naviState, GameTime gameTime)
        {
            if (line == null)
            {
                Initialize(naviState);

                eventBox = new Box();

                eventBox.frameWidth = 800;
                eventBox.frameHeight = 200;
                eventBox.UpperLeft = new Vector2(560, 880);

                eventBox.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

                naviState.pointer.Scale = new Vector2(0.4f, 0.4f);
                naviState.pointer.UpperLeft = new Vector2(1360 - naviState.pointer.GetWidth() - 20, 1080 - naviState.pointer.GetHeight() - 20);
                naviState.pointer.isAlive = false;

                lineUpperLeft = new Vector2(580, 900);

                lines = new List<string>();
                lines.Add("This is, well. I'm not sure what it is, quite.\n");
                lines.Add("Maybe some sort of altar? I've seen stranger.");
                line = "";
                previousLines = "";
            }

            Type(naviState, gameTime, 0.01);
        }
    }
}
