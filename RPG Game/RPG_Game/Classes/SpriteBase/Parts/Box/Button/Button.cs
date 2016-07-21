using System;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    public class Button : Parts
    {
        public Sprite icon = new Sprite();

        public string display;
    }

    public class Ability : Button
    {
        public int cost;

        public Action<GameTime> action;
    }
}
