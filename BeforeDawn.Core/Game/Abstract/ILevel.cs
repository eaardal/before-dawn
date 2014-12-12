using System;
using System.IO;
using BeforeDawn.Core.Adapters.Abstract;

namespace BeforeDawn.Core.Game.Abstract
{
    internal interface ILevel : IDisposable, IDraw
    {
        void LoadNextLevel();
        void Initialize(IStreamAdapter stream, int levelIndex);
    }
}