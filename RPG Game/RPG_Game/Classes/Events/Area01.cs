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

                eventBox.SetParts(naviState.cornerTexture, naviState.wallTexture, naviState.backTexture);

                naviState.pointer.Scale = new Vector2(0.4f, 0.4f);
                naviState.pointer.UpperLeft = new Vector2(800 - naviState.pointer.GetWidth() - 20, 200 - naviState.pointer.GetHeight() - 20);

                lines = new List<string>();
                lines.Add("This is, well. I'm not sure what it is, quite.\n");
                lines.Add("Maybe some sort of altar? I've seen stranger.");
                line = "";
                previousLines = "";
            }

            if (line.Length >= previousLines.Length)
            {
                line = line.Substring(previousLines.Length);
            }

            if (line.Equals(lines[0]))
            {
                naviState.timer = gameTime.TotalGameTime.TotalSeconds;

                if (naviState.activateInput.inputState == Input.inputStates.pressed)
                {
                    if (lines[0].EndsWith("\n") && lines.Count > 1)
                    {
                        previousLines += lines[0];
                        lines.RemoveAt(0);
                        line = "";
                    }
                    else
                    {
                        lines.RemoveAt(0);
                        line = "";
                    }

                    if (lines.Count == 0)
                    {
                        naviState.pointer.Scale = new Vector2(0.8f, 0.8f);

                        lines = null;
                        line = null;

                        complete = true;

                        return;
                    }
                }
            }

            if (gameTime.TotalGameTime.TotalSeconds < naviState.timer + 0.1)
            {
                line = previousLines + line;

                return;
            }

            if (line.Length + 1 < lines[0].Length)
            {
                string i = lines[0].Substring(line.Length);
                if (i.StartsWith(" "))
                {
                    int o = 0;

                    while (i.StartsWith(" "))
                    {
                        i = lines[0].Substring(line.Length + o);

                        o++;
                    }

                    line = lines[0].Remove(line.Length + o);
                }
                else
                {
                    line = lines[0].Remove(line.Length + 1);
                }
            }
            else
            {
                line = lines[0];
            }

            line = previousLines + line;

            naviState.timer = gameTime.TotalGameTime.TotalSeconds;
        }
    }
}
