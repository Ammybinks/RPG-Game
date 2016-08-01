using System.Collections.Generic;

namespace RPG_Game
{
    public class Box : Parts
    {
        public int activatorState;

        public List<Button> buttons;

        public virtual List<Button> GetButtons()
        {
            return buttons;
        }
    }
}
