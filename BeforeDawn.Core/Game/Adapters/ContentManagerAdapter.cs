using System;
using BeforeDawn.Core.Game.Adapters.Abstract;
using Microsoft.Xna.Framework.Content;

namespace BeforeDawn.Core.Game.Adapters
{
    class ContentManagerAdapter : IContentManagerAdapter
    {
        private ContentManager _contentManager;

        public void Create(IServiceProvider serviceProvider, string rootPath)
        {
            _contentManager = new ContentManager(serviceProvider, rootPath);
        }

        public T Load<T>(string assetName)
        {
            return _contentManager.Load<T>(assetName);
        }
    }
}