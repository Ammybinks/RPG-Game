using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    [Serializable()]
    public class Event
    {
        public bool complete;

        public string line;
        public string previousLines;

        public Vector2 lineUpperLeft;

        [NonSerialized()]public Box eventBox;

        internal List<string> lines;
        
        internal void Type(NaviState naviState, GameTime gameTime)
        {
            Type(naviState, gameTime, 0.05);
        }
        internal void Type(NaviState naviState, GameTime gameTime, double typeSpeed)
        {
            if (line.Length >= previousLines.Length)
            {
                line = line.Substring(previousLines.Length);
            }

            if (line.Equals(lines[0]))
            {
                naviState.timer = gameTime.TotalGameTime.TotalSeconds;

                naviState.pointer.isAlive = true;

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
            else
            {
                naviState.pointer.isAlive = false;
            }

            if (gameTime.TotalGameTime.TotalSeconds < naviState.timer + typeSpeed)
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

        internal void Initialize(NaviState naviState)
        {
            naviState.currentEvent = this;

            lineUpperLeft = new Vector2(20, 20);
        }
    }
}
