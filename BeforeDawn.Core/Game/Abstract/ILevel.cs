using System;
using System.IO;

namespace BeforeDawn.Core.Game.Abstract
{
    internal interface ILevel : IDisposable
    {
        void LoadNextLevel();
        void Initialize(Stream levelLayoutStream, int levelIndex);
    }
}