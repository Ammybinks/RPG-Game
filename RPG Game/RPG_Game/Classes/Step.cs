using Microsoft.Xna.Framework;

namespace RPG_Game
{
    class Step
    {
        public bool Invoke(GameTime gameTime, double timer, float speed, double duration, SpriteBase sprite, Vector2 modifier)
        {
            Vector2 i = speed * modifier;

            sprite.UpperLeft += i;

            if (gameTime.TotalGameTime.TotalSeconds >= timer + duration)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
