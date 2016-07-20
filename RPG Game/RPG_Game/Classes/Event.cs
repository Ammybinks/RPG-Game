using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Game
{
    [Serializable()]
    public class Event
    {
        public bool complete;

        public string line;
        public string previousLines;

        [NonSerialized()]public Box eventBox;

        internal List<string> lines;

        internal void Initialize(NaviState naviState)
        {
            naviState.currentEvent = this;
        }
    }
}
