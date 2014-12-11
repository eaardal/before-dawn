using System;
using Microsoft.Xna.Framework.Content;

namespace BeforeDawn.Core.Game.Adapters.Abstract
{
    internal interface IContentManagerAdapter
    {
        void Create(IServiceProvider serviceProvider, string rootPath);
        T Load<T>(string assetName);
    }
}