using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPG_Game
{
    class Event
    {
        public virtual bool Call(Character[] characters)
        {
            return false;
        }
    }
}
