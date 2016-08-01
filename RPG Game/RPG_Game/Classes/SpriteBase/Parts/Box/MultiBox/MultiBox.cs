﻿using System.Collections.Generic;

namespace RPG_Game
{
    class MultiBox : Box
    {
        public List<MultiButton> multiButtons;

        public override List<Button> GetButtons()
        {
            List<Button> temp = new List<Button>();

            for(int i = 0; i < multiButtons.Count; i++)
            {
                temp.Add(multiButtons[i]);
            }

            return temp;
        }
    }
}
