using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class SkillsMenuSwitcher : Usable
    {
        public override bool Call(GameTime gameTime, NaviState naviState)
        {
            naviState.state[1] = false;
            naviState.previousState[1] = false;

            naviState.actor = naviState.target;

            naviState.SkillsMenuSwitch(gameTime);

            return true;
        }
    }
}
