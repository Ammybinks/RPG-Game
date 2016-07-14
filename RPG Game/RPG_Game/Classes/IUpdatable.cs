using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RPG_Game
{
    interface IUpdatable
    {
        void LoadContent(Main main);
        
        void GetInput(GameTime gameTime);

        void Update(GameTime gameTime);

        void Draw(SpriteBatch spriteBatch);
    }
}
