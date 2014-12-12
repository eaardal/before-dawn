using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BeforeDawn.Core.Game.Abstract
{
    interface IDraw
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
