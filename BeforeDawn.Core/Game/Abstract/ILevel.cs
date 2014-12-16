using System;
using System.IO;
using BeforeDawn.Core.Adapters.Abstract;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BeforeDawn.Core.Game.Abstract
{
    internal interface ILevel : IDisposable, IDraw
    {
        void LoadNextLevel();
        void Initialize(IStreamAdapter stream, int levelIndex, Action levelCompleted);
        void Update(GameTime gameTime, KeyboardState keyboardState);
    }
}