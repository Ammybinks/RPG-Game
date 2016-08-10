using System;
using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class EventButton : Button
    {
        public new Action<NaviState, GameTime> action;
    }
}
