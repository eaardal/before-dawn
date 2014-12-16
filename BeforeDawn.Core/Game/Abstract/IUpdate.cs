using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Abstract
{
    interface IUpdate
    {
        void Update(GameTime gameTime, KeyboardState keyboardState);
    }
}
