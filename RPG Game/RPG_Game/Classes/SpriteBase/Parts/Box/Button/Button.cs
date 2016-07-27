using System;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    public class Button : Parts
    {
        public Sprite icon = new Sprite();

        public string display;

        public Action<GameTime> action;
    }
}